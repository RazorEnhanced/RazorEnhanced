using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assistant
{
    internal class CUOClient : Client
    {

        internal CUOClient(RazorEnhanced.Shard shard)
            : base(shard)
        {

        }

        public override Loader_Error LaunchClient()
        {
            // Spin up CUO
            Process cuo = new Process();
            cuo.StartInfo.FileName = m_Shard.CUOClient;
            FileVersionInfo version = FileVersionInfo.GetVersionInfo(m_Shard.ClientPath);
            int osiEnc = 0;
            if (m_Shard.OSIEnc)
            {
                osiEnc = 5;
            }

            string verString = String.Format("{0:00}.{1:0}.{2:0}.{3:D1}", version.FileMajorPart, version.FileMinorPart, version.FileBuildPart, version.FilePrivatePart);
            cuo.StartInfo.Arguments = String.Format("-ip {0} -port {1} -uopath \"{2}\" -no_server_ping -encryption {3} -plugins \"{4}\" -clientversion \"{5}\"",
                                        m_Shard.Host, m_Shard.Port, ShortFileName(m_Shard.ClientFolder), osiEnc,
                                        ShortFileName(System.Reflection.Assembly.GetExecutingAssembly().Location),
                                        verString);
            cuo.Start();
            return Loader_Error.SUCCESS;
        }
        public override IntPtr GetWindowHandle()
        {
            return (IntPtr)0;
        }
        public unsafe override bool InstallHooks(IntPtr pluginPtr)
        {          
            return true;
        }
        public override bool OnMessage(uint wParam, int lParam)
        {
            return false;
        }
        public override int OnUOAMessage(int Msg, int wParam, int lParam)
        {
            return 0;
        }
        public override void RunUI()
        {
        }
        public override void SetConnectionInfo(System.Net.IPAddress addr, int port)
        {
        }
    }
}
