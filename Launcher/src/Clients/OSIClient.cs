using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Collections.Concurrent;
using System.Threading;
using Microsoft.Win32.SafeHandles;

namespace Assistant
{
    internal class OSIClient : Client
    {
        internal class HiddenWindow : System.Windows.Forms.Form
        {
            Client m_Client;
            internal HiddenWindow(Client activeClient) 
                : base()
            {
                m_Client = activeClient;
                this.Load += new System.EventHandler(this.MainForm_Load);
            }
            private void MainForm_Load(object sender, System.EventArgs e)
            {
                if (!m_Client.InstallHooks(this.Handle)) 
                {
                    this.Close();
                    System.Diagnostics.Process.GetCurrentProcess().Kill();
                    return;
                }
            }

            protected override void WndProc(ref Message msg)
            {
                if (msg.Msg == 1025)
                {
                    msg.Result = (IntPtr)(m_Client.OnMessage((uint)msg.WParam.ToInt32(), msg.LParam.ToInt32()) ? 1 : 0);
                    return;
                }
                if (msg.Msg >= 1224 && msg.Msg <= 1338)
                {
                    msg.Result = (IntPtr)m_Client.OnUOAMessage(msg.Msg, msg.WParam.ToInt32(), msg.LParam.ToInt32());
                    return;
                }
                base.WndProc(ref msg);
            }

        }

        Form m_HiddenForm;
        Process ClientProc;
        public DateTime ConnectionStart { get; set; }
        
        protected System.Net.IPAddress LastConnection { get; set; }
        private bool Ready { get; set; }

        private readonly ConcurrentQueue<Packet> m_SendQueue;
        private readonly ConcurrentQueue<Packet> m_RecvQueue;

        private unsafe Buffer* m_InRecv;
        private unsafe Buffer* m_OutRecv;
        private unsafe Buffer* m_InSend;
        private unsafe Buffer* m_OutSend;
        private unsafe  byte* m_TitleStr;

        internal enum UONetMessage
        {
            Send = 1,
            Recv = 2,
            Ready = 3,
            NotReady = 4,
            Connect = 5,
            Disconnect = 6,
            KeyDown = 7,
            Mouse = 8,
            Activate = 9,
            Focus = 10,
            Close = 11,
            StatBar = 12,
            NotoHue = 13,
            DLL_Error = 14,
            DeathMsg = 15,
            OpenRPV = 18,
            SetGameSize = 19,
            FindData = 20,
            SmartCPU = 21,
            Negotiate = 22,
            SetMapHWnd = 23,

            DwmFree = 25
        }

        private enum InitError
        {
            SUCCESS,
            NO_UOWND,
            NO_TID,
            NO_HOOK,
            NO_SHAREMEM,
            LIB_DISABLED,
            NO_PATCH,
            NO_MEMCOPY,
            INVALID_PARAMS,

            UNKNOWN,
        }

        internal OSIClient(RazorEnhanced.Shard shard)
            : base(shard)
        {
            m_SendQueue = new ConcurrentQueue<Packet>();
            m_RecvQueue = new ConcurrentQueue<Packet>();
        }
        public override void RunUI()
        {
            Application.EnableVisualStyles();

            // Create a hidden form
            m_HiddenForm = new HiddenWindow(this);
            m_HiddenForm.ShowInTaskbar = false;
            m_HiddenForm.WindowState = FormWindowState.Minimized;
            m_HiddenForm.Visible = false;

            // Add a message loop to the hidden form
            Application.Idle += delegate { Application.DoEvents(); };

            // Run the hidden form
            Application.Run(m_HiddenForm);

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
                }
                catch
                {
                }

            }

            return ClientProc == null ? Loader_Error.UNKNOWN_ERROR : err;
        }

        public override bool InstallHooks(IntPtr mainWindow)
        {
            InitError error;
            int flags = 0;

            if (m_Shard.PatchEnc)
                flags |= 0x08;

            if (m_Shard.OSIEnc)
                flags |= 0x10;

            DLLImport.Razor.WaitForWindow(ClientProc.Id);

            unsafe
            {
                error = (InitError)DLLImport.Razor.InstallLibrary(mainWindow, ClientProc.Id, flags);

                if (error != InitError.SUCCESS)
                {
                    FatalInit(error);
                    return false;
                }

                byte* baseAddr = (byte*)DLLImport.Razor.GetSharedAddress().ToPointer();

                m_InRecv = (Buffer*)baseAddr;
                m_OutRecv = (Buffer*)(baseAddr + sizeof(Buffer));
                m_InSend = (Buffer*)(baseAddr + sizeof(Buffer) * 2);
                m_OutSend = (Buffer*)(baseAddr + sizeof(Buffer) * 3);
                m_TitleStr = baseAddr + sizeof(Buffer) * 4;
            }

            var addr = Utility.Resolve(Addr);
            byte[] ipBytes = addr.GetAddressBytes();
            uint ip = (uint)ipBytes[3] << 24;
            ip += (uint)ipBytes[2] << 16;
            ip += (uint)ipBytes[1] << 8;
            ip += (uint)ipBytes[0];

            DLLImport.Razor.SetServer(ip, Port);

            CommMutex = new Mutex { SafeWaitHandle = (new SafeWaitHandle(DLLImport.Razor.GetCommMutex(), true)) };

            try
            {
                DLLImport.Razor.SetDataPath(Path.GetDirectoryName(m_Shard.ClientFolder));
            }
            catch
            {
                DLLImport.Razor.SetDataPath("");
            }

            return true;
        }

        public override void SetConnectionInfo(System.Net.IPAddress addr, int port)
        {

        }

        public override bool OnMessage(uint wParam, int lParam)
        {
            bool retVal = true;
            bool m_hidetoolbar = false;
            bool m_hidespellgrid = false;

            switch ((UONetMessage)(wParam & 0xFFFF))
            {
                case UONetMessage.OpenRPV:
                    {
                        break;
                    }
                case UONetMessage.Ready: //Patch status
                    if (lParam == (int)InitError.NO_MEMCOPY)
                    {
                        if (MessageBox.Show(Engine.ActiveWindow, "No Mem Copy", "No Client MemCopy", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No)
                        {
                            //m_Ready = false;
                            ClientProc = null;
                            //Engine.MainWindow.CanClose = true;
                            m_HiddenForm.Close();
                            break;
                        }
                    }

                    try
                    {
                        DLLImport.Razor.SetDataPath(m_Shard.ClientFolder);
                    }
                    catch
                    {
                        DLLImport.Razor.SetDataPath("");
                    }
                    UoMod.InjectUoMod(this);
                    Ready = true;
                    break;

                case UONetMessage.NotReady:
                    Ready = false;
                    FatalInit((InitError)lParam);
                    ClientProc = null;
                    m_HiddenForm.Close();
                    break;

                // Network events
                case UONetMessage.Recv:
                    OnRecv();
                    break;

                case UONetMessage.Send:
                    OnSend();
                    break;

                case UONetMessage.Connect:
                    ConnectionStart = DateTime.UtcNow;
                    try
                    {
                        LastConnection = new System.Net.IPAddress((uint)lParam);
                    }
                    catch
                    {
                    }
                    break;

                case UONetMessage.Disconnect:
                    //OnLogout(false);
                    break;

                case UONetMessage.Close:
                    //OnLogout();
                    ClientProc = null;
                    //Engine.MainWindow.CanClose = true;
                    m_HiddenForm.Close();
                    break;

                // Hot Keys
                case UONetMessage.Mouse:
                    //RazorEnhanced.HotKey.OnMouse((ushort)(lParam & 0xFFFF), (short)(lParam >> 16));
                    break;

                case UONetMessage.KeyDown:
                    //retVal = HotKey.OnKeyDown(lParam);
                    //retVal = RazorEnhanced.HotKey.KeyDown((Keys)(lParam));
                    break;

                // Activation Tracking
                case UONetMessage.Activate:
                    break;

                case UONetMessage.Focus:
                    break;

                case UONetMessage.DLL_Error:
                    {
                        string error = "Unknown";
                        switch ((UONetMessage)lParam)
                        {
                            case UONetMessage.StatBar:
                                error = "Unable to patch status bar.";
                                break;
                        }

                        MessageBox.Show(Engine.ActiveWindow, "An Error has occured : \n" + error, "Error Reported", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    break;
                case UONetMessage.FindData:
                //  FindData.Message((wParam & 0xFFFF0000) >> 16, lParam);
                  break;

                // Unknown
                default:
                    //MessageBox.Show(Engine.ActiveWindow, "Unknown message from uo client\n" + ((int)wParam).ToString(), "Error?");
                    break;
            }

            return retVal;
        }

        private unsafe void OnRecv()
        {
            //ScriptWaitRecv = true;
            //QueueRecv = true;
            HandleComm(m_InRecv, m_OutRecv, m_RecvQueue, PacketPath.ServerToClient);
            //QueueRecv = false;
            //ScriptWaitRecv = false;
        }

        private unsafe void OnSend()
        {
            //ScriptWaitSend = true;
            //QueueSend = true;
            HandleComm(m_InSend, m_OutSend, m_SendQueue, PacketPath.ClientToServer);
            //QueueSend = false;
            //ScriptWaitSend = false;
        }


        private void FatalInit(InitError error)
        {
            MessageBox.Show("Initialization Error " + error.ToString(), "Init Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
        }


        public override int OnUOAMessage(int Msg, int wParam, int lParam)
        {
            return 0;
        }

        #region PacketHandler
        private const int SHARED_BUFF_SIZE = 524288; // 262144; // 250k
        private static Mutex CommMutex;

        [StructLayout(LayoutKind.Explicit, Size = 8 + SHARED_BUFF_SIZE)]
        private struct Buffer
        {
            [FieldOffset(0)]
            internal int Length;

            [FieldOffset(4)]
            internal int Start;

            [FieldOffset(8)]
            internal byte Buff0;
        }

        private unsafe void CopyToBuffer(Buffer* buffer, byte* data, int len)
        {
            //if ( buffer->Length + buffer->Start + len >= SHARED_BUFF_SIZE )
            //  throw new NullReferenceException( String.Format( "Buffer OVERFLOW in CopyToBuffer [{0} + {1}] <- {2}", buffer->Start, buffer->Length, len ) );

            /*      IntPtr to = (IntPtr)(&buffer->Buff0 + buffer->Start + buffer->Length);
                    IntPtr from = (IntPtr)data;
                    DLLImport.Win.memcpy(to, from, new UIntPtr((uint)len));
                    buffer->Length += len;*/

            DLLImport.Win.memcpy((&buffer->Buff0) + buffer->Start + buffer->Length, data, len);
            buffer->Length += len;
        }
        public Packet MakePacketFrom(PacketReader pr)
        {
            byte[] data = pr.CopyBytes(0, pr.Length);
            return new Packet(data, pr.Length, pr.DynamicLength);
        }

        [DllImport("Crypt.dll")]
        internal static unsafe extern IntPtr FindUOWindow();
        public override IntPtr GetWindowHandle()
        {
            return FindUOWindow();
        }

        private unsafe void HandleComm(Buffer* inBuff, Buffer* outBuff, ConcurrentQueue<Packet> queue, PacketPath path)
        {
            CommMutex.WaitOne();
            while (inBuff->Length > 0)
            {
                byte* buff = (&inBuff->Buff0) + inBuff->Start;

                int len = DLLImport.Razor.GetPacketLength(buff, inBuff->Length);
                if (len > inBuff->Length || len <= 0)
                    break;

                inBuff->Start += len;
                inBuff->Length -= len;

                bool viewer = false;
                bool filter = false;

                switch (path)
                {
                    case PacketPath.ClientToServer:
                        viewer = PacketHandler.HasClientViewer(buff[0]);
                        filter = PacketHandler.HasClientFilter(buff[0]);
                        break;

                    case PacketPath.ServerToClient:
                        viewer = PacketHandler.HasServerViewer(buff[0]);
                        filter = PacketHandler.HasServerFilter(buff[0]);
                        break;
                }

                Packet p = null;
                PacketReader pr = null;
                if (viewer)
                {
                    pr = new PacketReader(buff, len, DLLImport.Razor.IsDynLength(buff[0]));
                    if (filter)
                        p = MakePacketFrom(pr);
                }
                else if (filter)
                {
                    byte[] temp = new byte[len];
                    fixed (byte* ptr = temp)
                        DLLImport.Win.memcpy(ptr, buff, len);
                    p = new Packet(temp, len, DLLImport.Razor.IsDynLength(buff[0]));

                    /*byte[] temp = new byte[len];
                    fixed (byte* ptr = temp)
                    {
                        IntPtr to = (IntPtr)ptr;
                        IntPtr from = (IntPtr)buff;
                        DLLImport.Win.memcpy(to, from, new UIntPtr((uint)len));
                    }
                    p = new Packet(temp, len, DLLImport.Razor.IsDynLength(buff[0]));
                    */

                }

                bool blocked = false;
                switch (path)
                {
                    // yes it should be this way
                    case PacketPath.ClientToServer:
                        {
                            blocked = PacketHandler.OnClientPacket(buff[0], pr, p);
                            break;
                        }
                    case PacketPath.ServerToClient:
                        {
                            blocked = PacketHandler.OnServerPacket(buff[0], pr, p);
                            break;
                        }
                }


                if (filter)
                {
                    byte[] data = p.Compile();
                    if (data[0] == 0xc8 || data[0] == 0x73 || data[0] == 0xbf || data[0] == 0xdc)
                    {
                    }
                    else
                    {
                        //int something = len;
                        //Debug.WriteLine("Packet id 0x{0:X}", data[0]);
                    }

                    fixed (byte* ptr = data)
                    {
                        Packet.Log(path, ptr, data.Length, blocked);
                        if (!blocked)
                            CopyToBuffer(outBuff, ptr, data.Length);
                    }
                }
                else
                {
                    //Packet.Log(path, buff, len, blocked);
                    if (!blocked)
                        CopyToBuffer(outBuff, buff, len);
                }

                while (queue.Count > 0)
                {
                    if (!queue.TryDequeue(out p))
                        continue;

                    byte[] data = p.Compile();
                    fixed (byte* ptr = data)
                    {
                        CopyToBuffer(outBuff, ptr, data.Length);
                        //Packet.Log((PacketPath)(((int)path) + 1), ptr, data.Length);
                    }
                }
            }
            CommMutex.ReleaseMutex();
        }
        #endregion
    }
    internal class UoMod
    {
        internal enum PATCH_TYPE
        {
            PT_FPS = 1,
            PT_STAMINA,
            PT_ALWAYS_LIGHT,
            PT_PAPERDOLL_SLOTS,
            PT_SPLASH_SCREEN,
            PT_RESOLUTION,
            PT_OPTIONS_NOTIFICATION,
            PT_MULTI_UO,
            PT_NO_CRYPT,
            PT_GLOBAL_SOUND,
            PT_VIEW_RANGE,
            PT_COUNT
        };

        internal enum PATCH_STATE
        {
            PS_DISABLE = 0,
            PS_ENABLE,
            PS_NOT_FOUND
        };

        internal enum PATCH_MESSAGES
        {
            PM_INSTALL = Client.WM_USER + 666,
            PM_INFO,
            PM_ENABLE,
            PM_DISABLE,
            PM_VIEW_RANGE_VALUE
        };

        private static IntPtr m_modhandle = IntPtr.Zero;
        private static bool m_soundpatch = false;
        private static bool m_fpspatch = false;
        private static bool m_paperdollpatch = false;
        private static bool m_viewrangepatch = false;

        // privileges
        const int PROCESS_CREATE_THREAD = 0x0002;
        const int PROCESS_QUERY_INFORMATION = 0x0400;
        const int PROCESS_VM_OPERATION = 0x0008;
        const int PROCESS_VM_WRITE = 0x0020;
        const int PROCESS_VM_READ = 0x0010;

        // used for memory allocation
        const uint MEM_COMMIT = 0x00001000;
        const uint MEM_RESERVE = 0x00002000;
        const uint PAGE_READWRITE = 4;


        internal static void InjectUoMod(Client client)
        {
            if (Engine.ClientMajor < 7)
                return;

            //      if (Engine.ClientBuild > 49)
            //          return;

            String path = AppDomain.CurrentDomain.BaseDirectory + "\\UOMod.dll";

            IntPtr hp = DLLImport.Win.OpenProcess(PROCESS_CREATE_THREAD | PROCESS_VM_OPERATION | PROCESS_VM_WRITE | PROCESS_QUERY_INFORMATION | PROCESS_VM_READ, true, DLLImport.Razor.GetUOProcId());

            if (hp != IntPtr.Zero)
            {
                IntPtr hProcess = IntPtr.Zero;
                IntPtr hThread = IntPtr.Zero;
                IntPtr pszLibFileRemote = IntPtr.Zero;
                try
                {
                    hProcess = DLLImport.Win.OpenProcess(
                        PROCESS_QUERY_INFORMATION | // Required by Alpha
                        PROCESS_CREATE_THREAD | // For CreateRemoteThread
                        PROCESS_VM_OPERATION | // For VirtualAllocEx/VirtualFreeEx
                        PROCESS_VM_WRITE | // For WriteProcessMemory
                        PROCESS_VM_READ,
                        false, DLLImport.Razor.GetUOProcId());

                    if (hProcess == IntPtr.Zero)
                        return;

                    int cch = 1 + DLLImport.Win.lstrlen(path);
                    int cb = cch * sizeof(char);

                    pszLibFileRemote = DLLImport.Win.VirtualAllocEx(hProcess, IntPtr.Zero, (uint)cb, MEM_COMMIT, PAGE_READWRITE);
                    if (pszLibFileRemote == IntPtr.Zero)
                        return;

                    UIntPtr bytesWritten;
                    if (!DLLImport.Win.WriteProcessMemory(hProcess, pszLibFileRemote, Encoding.Default.GetBytes(path), (uint)((path.Length + 1) * Marshal.SizeOf(typeof(char))), out bytesWritten))
                        return;

                    IntPtr pfnThreadRtn = DLLImport.Win.GetProcAddress(DLLImport.Win.GetModuleHandle("Kernel32"), "LoadLibraryA");
                    if (pfnThreadRtn == IntPtr.Zero)
                        return;

                    hThread = DLLImport.Win.CreateRemoteThread(hProcess, IntPtr.Zero, 0, pfnThreadRtn, pszLibFileRemote, 0, IntPtr.Zero);
                    if (hThread == IntPtr.Zero)
                        return;

                    DLLImport.Win.WaitForSingleObject(hThread, int.MaxValue);
                }
                finally
                {
                    if (pszLibFileRemote != IntPtr.Zero)
                        DLLImport.Win.VirtualFreeEx(hProcess, pszLibFileRemote, 0, DLLImport.Win.FreeType.MEM_RELEASE);

                    if (hThread != IntPtr.Zero)
                        DLLImport.Win.CloseHandle(hThread);

                    if (hProcess != IntPtr.Zero)
                        DLLImport.Win.CloseHandle(hProcess);
                }
            }

            // Thread attesa che la windowhandle sia disponibile
            new Thread(() =>
            {
                Thread.CurrentThread.IsBackground = true;
                Thread.Sleep(1500);
                m_modhandle = DLLImport.Win.FindWindow("UOModWindow_" + client.GetWindowHandle().ToString("x8").ToUpper(), null);

                if (m_modhandle != IntPtr.Zero)
                {
                    DLLImport.Win.SendMessage(m_modhandle, (int)PATCH_MESSAGES.PM_VIEW_RANGE_VALUE, 0, 0);
                    DLLImport.Win.SendMessage(m_modhandle, (int)PATCH_MESSAGES.PM_INFO, 0, 0xFFFFFFFF);

                    //  SendMessage(dllWindow, PM_VIEW_RANGE_VALUE, (DWORD)Handle, 0);
                    //  SendMessage(dllWindow, PM_INFO, (DWORD)Handle, 0xFFFFFFFF);
                    EnableOnStartMod();
                }

            }).Start();
        }

        internal static void EnableDisable(bool enable, int patch)
        {
            if (Engine.ClientMajor < 7)
                return;

            if (m_modhandle == IntPtr.Zero)
                return;

            int m_enable;
            if (enable)
                m_enable = (int)PATCH_MESSAGES.PM_ENABLE;
            else
                m_enable = (int)PATCH_MESSAGES.PM_DISABLE;

            switch (patch)
            {
                case (int)PATCH_TYPE.PT_FPS:
                    DLLImport.Win.SendMessage(m_modhandle, m_enable, 0, (int)PATCH_TYPE.PT_FPS);
                    m_fpspatch = enable;
                    break;
                case (int)PATCH_TYPE.PT_PAPERDOLL_SLOTS:
                    DLLImport.Win.SendMessage(m_modhandle, m_enable, 0, (int)PATCH_TYPE.PT_PAPERDOLL_SLOTS);
                    m_paperdollpatch = enable;
                    break;
                case (int)PATCH_TYPE.PT_GLOBAL_SOUND:
                    DLLImport.Win.SendMessage(m_modhandle, m_enable, 0, (int)PATCH_TYPE.PT_GLOBAL_SOUND);
                    m_soundpatch = enable;
                    break;
                default:
                    break;
            }
        }

        private static void EnableOnStartMod()
        {
            if (Engine.ClientMajor < 7)
                return;

            if (m_modhandle == IntPtr.Zero)
                return;

            /*
            if (RazorEnhanced.Settings.General.ReadBool("ForceSizeEnabled") && Engine.ClientBuild < 49 && Assistant.Engine.IP != Engine.Resolve("37.143.10.137"))
            {
                DLLImport.Win.SendMessage(m_modhandle, (int)PATCH_MESSAGES.PM_VIEW_RANGE_VALUE, 0, 30);
                DLLImport.Win.SendMessage(m_modhandle, (int)PATCH_MESSAGES.PM_ENABLE, 0, (int)PATCH_TYPE.PT_VIEW_RANGE);
                SendToClient(new SetUpdateRange(31));
                m_viewrangepatch = true;
            }

            if (RazorEnhanced.Settings.General.ReadBool("UoModFPS"))
            {
                DLLImport.Win.SendMessage(m_modhandle, (int)PATCH_MESSAGES.PM_ENABLE, 0, (int)PATCH_TYPE.PT_FPS);
                m_fpspatch = true;
            }

            if (RazorEnhanced.Settings.General.ReadBool("UoModPaperdoll"))
            {
                DLLImport.Win.SendMessage(m_modhandle, (int)PATCH_MESSAGES.PM_ENABLE, 0, (int)PATCH_TYPE.PT_PAPERDOLL_SLOTS);
                m_paperdollpatch = true;
            }

            if (RazorEnhanced.Settings.General.ReadBool("UoModSound"))
            {
                DLLImport.Win.SendMessage(m_modhandle, (int)PATCH_MESSAGES.PM_ENABLE, 0, (int)PATCH_TYPE.PT_GLOBAL_SOUND);
                m_soundpatch = true;
            }
            */
        }

        internal static void ProfileChange()
        {
            if (Engine.ClientMajor < 7)
                return;

            //if (Engine.ClientBuild > 49)
            //  return;

            if (m_modhandle == IntPtr.Zero)
                return;

            /*
            // ViewRange
            if (Engine.ClientBuild < 49 && Assistant.Engine.IP != Engine.Resolve("37.143.10.137"))
            {
                if (RazorEnhanced.Settings.General.ReadBool("ForceSizeEnabled"))
                {
                    if (!m_viewrangepatch)
                    {
                        DLLImport.Win.SendMessage(m_modhandle, (int)PATCH_MESSAGES.PM_VIEW_RANGE_VALUE, 0, 30);
                        DLLImport.Win.SendMessage(m_modhandle, (int)PATCH_MESSAGES.PM_ENABLE, 0, (int)PATCH_TYPE.PT_VIEW_RANGE);
                        SendToClient(new SetUpdateRange(31));
                        m_viewrangepatch = true;
                    }
                }
                else
                {
                    if (m_viewrangepatch)
                    {
                        DLLImport.Win.SendMessage(m_modhandle, (int)PATCH_MESSAGES.PM_DISABLE, 0, (int)PATCH_TYPE.PT_VIEW_RANGE);
                        m_viewrangepatch = false;
                    }
                }
            }

            // FPS
            if (RazorEnhanced.Settings.General.ReadBool("UoModFPS"))
            {
                if (!m_fpspatch)
                {
                    DLLImport.Win.SendMessage(m_modhandle, (int)PATCH_MESSAGES.PM_ENABLE, 0, (int)PATCH_TYPE.PT_FPS);
                    m_fpspatch = true;
                }
            }
            else
            {
                if (m_fpspatch)
                {
                    DLLImport.Win.SendMessage(m_modhandle, (int)PATCH_MESSAGES.PM_DISABLE, 0, (int)PATCH_TYPE.PT_FPS);
                    m_fpspatch = false;
                }
            }

            // Paperdoll
            if (RazorEnhanced.Settings.General.ReadBool("UoModPaperdoll"))
            {
                if (!m_paperdollpatch)
                {
                    DLLImport.Win.SendMessage(m_modhandle, (int)PATCH_MESSAGES.PM_ENABLE, 0, (int)PATCH_TYPE.PT_PAPERDOLL_SLOTS);
                    m_paperdollpatch = true;
                }
            }
            else
            {
                if (m_paperdollpatch)
                {
                    DLLImport.Win.SendMessage(m_modhandle, (int)PATCH_MESSAGES.PM_DISABLE, 0, (int)PATCH_TYPE.PT_PAPERDOLL_SLOTS);
                    m_paperdollpatch = false;
                }
            }

            //Global Sound
            if (RazorEnhanced.Settings.General.ReadBool("UoModSound"))
            {
                if (!m_soundpatch)
                {
                    DLLImport.Win.SendMessage(m_modhandle, (int)PATCH_MESSAGES.PM_ENABLE, 0, (int)PATCH_TYPE.PT_GLOBAL_SOUND);
                    m_soundpatch = true;
                }
            }
            else
            {
                if (m_soundpatch)
                {
                    DLLImport.Win.SendMessage(m_modhandle, (int)PATCH_MESSAGES.PM_DISABLE, 0, (int)PATCH_TYPE.PT_GLOBAL_SOUND);
                    m_soundpatch = false;
                }
            }
            */
        }
    }

}
