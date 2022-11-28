using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;

namespace Assistant
{
    internal class OSIClient : Client
    {
        Process ClientProc;

        internal OSIClient(RazorEnhanced.Shard shard)
            : base(shard)
        {
                
        }

        public override Loader_Error LaunchClient()
        {
            string dll = Path.Combine(Assistant.Engine.RootPath, "Crypt.dll");
            uint pid;
            Loader_Error err;
            unsafe
            {
                err = (Loader_Error)DLLImport.Razor.Load(m_Shard.ClientPath, dll, "OnAttach", null, 0, out pid);
            }
            if (err == Loader_Error.SUCCESS)
            {
                try
                {
                    ClientProc = Process.GetProcessById((int)pid);
                    //if (ClientProc != null && !RazorEnhanced.Settings.General.ReadBool("SmartCPU"))
                    //    ClientProc.PriorityClass = (ProcessPriorityClass)Enum.Parse(typeof(ProcessPriorityClass), RazorEnhanced.Settings.General.ReadString("ClientPrio"), true);
                }
                catch
                {
                }

            }

            return ClientProc == null ? Loader_Error.UNKNOWN_ERROR : err;
        }
    }
}
