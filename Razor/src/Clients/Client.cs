using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Assistant
{


    internal abstract class Client
    {
        private static string _rootPath = null;

        public static string RootPath =>
            _rootPath ?? (_rootPath = Path.GetDirectoryName(Assembly.GetAssembly(typeof(Client)).Location));

        public const int WM_USER = 0x400;

        internal RazorEnhanced.Shard m_Shard;
        internal bool RememberPwds { get; set; }

        internal Client(RazorEnhanced.Shard shard)
        {
            m_Shard = shard;
            RememberPwds = true;
        }

        public enum Loader_Error
        {
            SUCCESS = 0,
            NO_OPEN_EXE,
            NO_MAP_EXE,
            NO_READ_EXE_DATA,

            NO_RUN_EXE,
            NO_ALLOC_MEM,

            NO_WRITE,
            NO_VPROTECT,
            NO_READ,

            UNKNOWN_ERROR = 99
        };
        public abstract Loader_Error LaunchClient();

        internal string ClientPath { get { return m_Shard.ClientPath; } }
        internal string Addr { get { return m_Shard.Host; } }
        internal ushort Port { get { return (ushort)m_Shard.Port; } }

        public abstract void RunUI();

        // Define GetShortPathName API function.
        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        static extern uint GetShortPathName(string lpszLongPath, char[] lpszShortPath, int cchBuffer);

        // Return the short file name for a long file name.
        internal static string ShortFileName(string long_name)
        {
            char[] name_chars = new char[1024];
            long length = GetShortPathName(
                long_name, name_chars,
                name_chars.Length);

            string short_name = new string(name_chars);
            return short_name.Substring(0, (int)length);
        }

        #region remember_password

        // ------------- PASSWORD START -----------------
        internal class PasswordMemory
        {
            internal List<PasswordData> list;

            internal PasswordMemory()
            {
                LoadPasswords();
            }
            internal void AddUpdateUser(string user, string password, string IP)
            {
                list.Add(new PasswordData(IP, user, Utility.Protect(password)));
                SavePasswords();
            }

            internal string GetPassword(string user, string ip)
            {
                foreach (PasswordData entry in list)
                {
                    if (entry.IP == ip && entry.User == user)
                        return Utility.Unprotect(entry.Password);
                }
                return String.Empty;
            }

            internal void LoadPasswords()
            {
                string filename = Path.Combine(Assistant.Engine.RootPath, "Profiles", "RazorEnhanced.password");
                list = Newtonsoft.Json.JsonConvert.DeserializeObject<List<PasswordData>>(File.ReadAllText(filename));
            }

            // Standard save doesnt save passwords, and they are only saved if changed because encryption is expensive
            internal void SavePasswords()
            {
                // ensure we save passwords to top level of profile
                string filename = Path.Combine(Client.RootPath, "Profiles", "RazorEnhanced.password");
                File.WriteAllText(filename + '.' + "password", Newtonsoft.Json.JsonConvert.SerializeObject(list, Newtonsoft.Json.Formatting.Indented));
            }


            [Serializable]
            internal class PasswordData
            {
                public string IP { get; set; }

                public string User { get; set; }

                public string Password { get; set; }

                public PasswordData(string ip, string user, string password)
                {
                    IP = ip;
                    User = user;
                    Password = password;
                }
            }

        }

        // ------------- PASSWORD END -----------------


        private string m_LastPW = "";
        public virtual void GameLogin(Packet p, PacketHandlerEventArgs args)
        {
            if (!m_Shard.RememberPW)
                return;

            p.ReadInt32(); //authID

            string accountName = p.ReadString(30);
            string password = p.ReadString(30);

            if (password == "" && m_LastPW != "")
            {
                p.Seek(35, SeekOrigin.Begin);
                p.WriteAsciiFixed(m_LastPW, 30);
                m_LastPW = "";
            }


        }

        public virtual void ServerListLogin(Packet p, PacketHandlerEventArgs args)
        {
            m_LastPW = "";
            if (!m_Shard.RememberPW)
                return;

            string accountName = p.ReadStringSafe(30);
            string pass = p.ReadStringSafe(30);
            if (pass.Length == 0 || pass[0] == 0xff)
                pass = ""; // bug in CUO

            if (string.IsNullOrEmpty(pass))
            {
                var pw = new PasswordMemory();
                pass = pw.GetPassword(accountName, Addr);
                if (!string.IsNullOrEmpty(pass))
                {
                    p.Seek(31, SeekOrigin.Begin);
                    p.WriteAsciiFixed(pass, 30);
                    m_LastPW = pass;
                }
            }
            else
            {
                var pw = new PasswordMemory();
                pw.AddUpdateUser(accountName, pass, Addr);
            }
        }
        #endregion

        public abstract void SetConnectionInfo(System.Net.IPAddress addr, int port);
        public abstract bool InstallHooks(IntPtr mainWindow);
        public abstract IntPtr GetWindowHandle();
        public abstract bool OnMessage(uint wParam, int lParam);
        public abstract int OnUOAMessage(int Msg, int wParam, int lParam);


    }
}
