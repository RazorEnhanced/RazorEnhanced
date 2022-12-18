using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Assistant
{


    internal abstract class Client
    {
        public const int WM_USER = 0x400;

        internal RazorEnhanced.Shard m_Shard;
        internal Client(RazorEnhanced.Shard shard)
        {
            m_Shard = shard;
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

        public abstract void SetConnectionInfo(System.Net.IPAddress addr, int port);
        public abstract bool InstallHooks(IntPtr mainWindow);
        public abstract IntPtr GetWindowHandle();
        public abstract bool OnMessage(uint wParam, int lParam);
        public abstract int OnUOAMessage(int Msg, int wParam, int lParam);


    }
}
