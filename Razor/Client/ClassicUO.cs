using CUO_API;
using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using Assistant;
using Assistant.UI;
using System.Linq;

// For CUO Settings
//using Microsoft.Xna.Framework;
using Newtonsoft.Json;


namespace Assistant
{
    public partial class Engine
    {

        public static unsafe void RE_Install(PluginHeader* plugin, System.Collections.Generic.Dictionary<string, IntPtr> fns)
        {
            AppDomain.CurrentDomain.AssemblyResolve += (sender, e) =>
            {
                string[] fields = e.Name.Split(',');
                string name = fields[0];
                string culture = fields[2];

                if (name.EndsWith(".resources") && !culture.EndsWith("neutral"))
                {
                    return null;
                }

                AssemblyName askedassembly = new AssemblyName(e.Name);

                bool isdll = File.Exists(Path.Combine(RootPath, askedassembly.Name + ".dll"));

                return Assembly.LoadFile(Path.Combine(RootPath, askedassembly.Name + (isdll ? ".dll" : ".exe")));
            };


            //ClassicUO.Configuration.Settings settings = ClassicUO.Configuration.Settings.Get();
            Install2(plugin);
        }

        public static unsafe void Install(PluginHeader* plugin)
        {
            AppDomain.CurrentDomain.AssemblyResolve += (sender, e) =>
            {
                string[] fields = e.Name.Split(',');
                string name = fields[0];
                string culture = fields[2];

                if (name.EndsWith(".resources") && !culture.EndsWith("neutral"))
                {
                    return null;
                }

                AssemblyName askedassembly = new AssemblyName(e.Name);

                bool isdll = File.Exists(Path.Combine(RootPath, askedassembly.Name + ".dll"));

                return Assembly.LoadFile(Path.Combine(RootPath, askedassembly.Name + (isdll ? ".dll" : ".exe")));
            };


            //ClassicUO.Configuration.Settings settings = ClassicUO.Configuration.Settings.Get();
            Install2(plugin);
        }

        public static unsafe void Install2(PluginHeader* plugin)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            //SplashScreen.Start();
            m_ActiveWnd = SplashScreen.Instance;

            ClassicUOClient.UOFilePath =
                ((OnGetUOFilePath)Marshal.GetDelegateForFunctionPointer(plugin->GetUOFilePath, typeof(OnGetUOFilePath))
                )();

            ClassicUOClient cuo = new ClassicUOClient();
            Client.Instance = cuo;
            cuo.InitPlugin(plugin);
            cuo.Init(false);
            cuo.RunUI();

            //if (!(Client.Instance as ClassicUOClient).Install(plugin, HostExecutionContext, character))
            //{
            //    Process.GetCurrentProcess().Kill();
            //    return;
            //}

        }

    }
    public class ClassicUOClient : Client
    {


        public static string UOFilePath { get; set; }
        public override Process ClientProcess => m_ClientProcess;
        public override bool ClientRunning => m_ClientRunning;
        private uint m_In, m_Out;

        private Process m_ClientProcess = null;
        private bool m_ClientRunning = false;
        private string m_ClientVersion;

        private static OnPacketSendRecv _sendToClient, _sendToServer, _recv, _send;
        private static OnGetPacketLength _getPacketLength;
        private static OnGetPlayerPosition _getPlayerPosition;
        private static OnCastSpell _castSpell;
        private static OnGetStaticImage _getStaticImage;
        private static OnTick _tick;
        private static RequestMove _requestMove;
        private static OnSetTitle _setTitle;
        private static OnGetUOFilePath _uoFilePath;


        private static OnHotkey _onHotkeyPressed;
        private static OnMouse _onMouse;
        private static OnUpdatePlayerPosition _onUpdatePlayerPosition;
        private static OnClientClose _onClientClose;
        private static OnInitialize _onInitialize;
        private static OnConnected _onConnected;
        private static OnDisconnected _onDisconnected;
        private static OnFocusGained _onFocusGained;
        private static OnFocusLost _onFocusLost;
        private IntPtr m_ClientWindow;
        private static bool m_Ready = false;

        static ClassicUOClient()
        {
            Client.IsOSI = false;
        }

        public override void SetMapWndHandle(Form mapWnd)
        {
        }

        public override void RequestStatbarPatch(bool preAOS)
        {
        }

        public override void SetCustomNotoHue(int hue)
        {
        }

        public override void SetSmartCPU(bool enabled)
        {
        }

        public override void SetGameSize(int x, int y)
        {
        }

        public override Loader_Error LaunchClient(string client)
        {
            return Loader_Error.SUCCESS;
        }

        public override bool ClientEncrypted { get; set; }

        public override bool ServerEncrypted { get; set; }

        public unsafe bool InitPlugin(PluginHeader* header)
        {
            _sendToClient =
                (OnPacketSendRecv) Marshal.GetDelegateForFunctionPointer(header->Recv, typeof(OnPacketSendRecv));
            _sendToServer =
                (OnPacketSendRecv) Marshal.GetDelegateForFunctionPointer(header->Send, typeof(OnPacketSendRecv));
            _getPacketLength =
                (OnGetPacketLength) Marshal.GetDelegateForFunctionPointer(header->GetPacketLength,
                    typeof(OnGetPacketLength));
            _getPlayerPosition =
                (OnGetPlayerPosition) Marshal.GetDelegateForFunctionPointer(header->GetPlayerPosition,
                    typeof(OnGetPlayerPosition));
            _castSpell = (OnCastSpell) Marshal.GetDelegateForFunctionPointer(header->CastSpell, typeof(OnCastSpell));
            _getStaticImage =
                (OnGetStaticImage) Marshal.GetDelegateForFunctionPointer(header->GetStaticImage,
                    typeof(OnGetStaticImage));
            _requestMove =
                (RequestMove) Marshal.GetDelegateForFunctionPointer(header->RequestMove, typeof(RequestMove));
            _setTitle = (OnSetTitle) Marshal.GetDelegateForFunctionPointer(header->SetTitle, typeof(OnSetTitle));
            _uoFilePath =
                (OnGetUOFilePath) Marshal.GetDelegateForFunctionPointer(header->GetUOFilePath, typeof(OnGetUOFilePath));
            m_ClientVersion = new Version((byte) (header->ClientVersion >> 24), (byte) (header->ClientVersion >> 16),
                (byte) (header->ClientVersion >> 8), (byte) header->ClientVersion).ToString();
            m_ClientRunning = true;
            m_ClientWindow = header->HWND;
            _tick = Tick;
            _recv = OnRecv;
            _send = OnSend;
            _onHotkeyPressed = OnHotKeyHandler;
            _onMouse = OnMouseHandler;
            _onUpdatePlayerPosition = OnPlayerPositionChanged;
            _onClientClose = OnClientClosing;
            _onInitialize = OnInitialize;
            _onConnected = OnConnected;
            _onDisconnected = OnDisconnected;
            _onFocusGained = OnFocusGained;
            _onFocusLost = OnFocusLost;
            header->Tick = Marshal.GetFunctionPointerForDelegate(_tick);
            header->OnRecv = Marshal.GetFunctionPointerForDelegate(_recv);
            header->OnSend = Marshal.GetFunctionPointerForDelegate(_send);
            header->OnHotkeyPressed = Marshal.GetFunctionPointerForDelegate(_onHotkeyPressed);
            header->OnMouse = Marshal.GetFunctionPointerForDelegate(_onMouse);
            header->OnPlayerPositionChanged = Marshal.GetFunctionPointerForDelegate(_onUpdatePlayerPosition);
            header->OnClientClosing = Marshal.GetFunctionPointerForDelegate(_onClientClose);
            header->OnInitialize = Marshal.GetFunctionPointerForDelegate(_onInitialize);
            header->OnConnected = Marshal.GetFunctionPointerForDelegate(_onConnected);
            header->OnDisconnected = Marshal.GetFunctionPointerForDelegate(_onDisconnected);
            header->OnFocusGained = Marshal.GetFunctionPointerForDelegate(_onFocusGained);
            header->OnFocusLost = Marshal.GetFunctionPointerForDelegate(_onFocusLost);
            m_Ready = true;
            return true;
        }

        public unsafe override bool InstallHooks(IntPtr pluginPtr)
        {
            //Engine.MainWindow.SafeAction((s) => { Engine.MainWindow.MainForm_EndLoad(); });
            return true;
        }

        private void Tick()
        {
            Timer.Slice();
        }

        private void OnPlayerPositionChanged(int x, int y, int z)
        {
            World.Player.Position = new Point3D(x, y, z);
            World.Player.WalkScriptRequest = 2;
        }

        internal static void RunTheUI()
        {
            Engine.MainWnd = new MainForm();
            if (! IsOSI) {
                Engine.MainWindow.SafeAction(s => { s.DisableRecorder(); });
                Engine.MainWindow.SafeAction(s => { s.DisableSmartCpu("Not available with CUO"); });
            }
            Application.Run(Engine.MainWnd);
        }
        public override void RunUI()
        {
            Thread t = new Thread(() => { RunTheUI(); });
            t.SetApartmentState(ApartmentState.STA);
            t.IsBackground = true;
            t.Start();
        }
        public override RazorEnhanced.Shard SelectShard(System.Collections.Generic.List<RazorEnhanced.Shard> shards)
        {
            RazorEnhanced.Shard cuo_shard =
                new RazorEnhanced.Shard("Classic UO Default", Path.Combine(ClassicUOClient.UOFilePath, "client.exe"),
                ClassicUOClient.UOFilePath, "", "127.0.0.1", 1000, true, false, true);
            return cuo_shard;
        }

        private unsafe bool OnRecv(ref byte[] data, ref int length)
        {
            m_In += (uint) length;
            fixed (byte* ptr = data)
            {
                bool result = true;
                byte id = data[0];

                PacketReader reader = null;
                Packet packet = null;
                bool isView = PacketHandler.HasServerViewer(id);
                bool isFilter = PacketHandler.HasServerFilter(id);

                if (isView)
                {
                    reader = new PacketReader(ptr, length, PacketsTable.IsDynLength(id));
                    result = !PacketHandler.OnServerPacket(id, reader, packet);
                }
                else if (isFilter)
                {
                    packet = new Packet(data, length, PacketsTable.IsDynLength(id));
                    result = !PacketHandler.OnServerPacket(id, reader, packet);

                    data = packet.Compile();
                    length = (int) packet.Length;
                }

                return result;
            }
        }

        private unsafe bool OnSend(ref byte[] data, ref int length)
        {
            m_Out += (uint) length;
            fixed (byte* ptr = data)
            {
                bool result = true;
                byte id = data[0];

                PacketReader reader = null;
                Packet packet = null;
                bool isView = PacketHandler.HasClientViewer(id);
                bool isFilter = PacketHandler.HasClientFilter(id);

                if (isView)
                {
                    reader = new PacketReader(ptr, length, PacketsTable.IsDynLength(id));
                    result = !PacketHandler.OnClientPacket(id, reader, packet);
                }
                else if (isFilter)
                {
                    packet = new Packet(data, length, PacketsTable.IsDynLength(id));
                    result = !PacketHandler.OnClientPacket(id, reader, packet);

                    data = packet.Compile();
                    length = (int) packet.Length;
                }

                return result;
            }
        }

        private void OnMouseHandler(int button, int wheel)
        {
            if (button > 4)
                button = 3;
            else if (button > 3)
                button = 2;
            else if (button > 2)
                button = 2;
            else if (button > 1)
                button = 1;

            RazorEnhanced.HotKey.OnMouse(button, wheel);
        }

        private enum SDL_Keymod
        {
            KMOD_NONE = 0x0000,
            KMOD_LSHIFT = 0x0001,
            KMOD_RSHIFT = 0x0002,
            KMOD_LCTRL = 0x0040,
            KMOD_RCTRL = 0x0080,
            KMOD_LALT = 0x0100,
            KMOD_RALT = 0x0200,
            KMOD_LGUI = 0x0400,
            KMOD_RGUI = 0x0800,
            KMOD_NUM = 0x1000,
            KMOD_CAPS = 0x2000,
            KMOD_MODE = 0x4000,
            KMOD_RESERVED = 0x8000
        }

        private enum SDL_Keycode_Ignore
        {
            SDLK_LCTRL = 1073742048,
            SDLK_LSHIFT = 1073742049,
            SDLK_LALT = 1073742050,
            SDLK_RCTRL = 1073742052,
            SDLK_RSHIFT = 1073742053,
            SDLK_RALT = 1073742054,
        }

        // Weird situation where CUO was passing in a wrong value
        // for oem keys.
        // so, I special case those, check if down, and return appropriate
        // code
        internal int checkForOmeKeys(int key)
        {
            Keys[] oemKeys = {Keys.Oem1, Keys.Oem102, Keys.Oem2,
            Keys.Oem3, Keys.Oem4, Keys.Oem5, Keys.Oem6, Keys.Oem7, Keys.Oem8,
            Keys.OemBackslash, Keys.OemClear, Keys.OemCloseBrackets,
            Keys.Oemcomma, Keys.OemMinus, Keys.OemOpenBrackets,
            Keys.OemPeriod, Keys.OemPipe, Keys.Oemplus, Keys.OemQuestion,
            Keys.OemQuotes, Keys.OemSemicolon, Keys.Oemtilde };
            foreach (var oemKey in oemKeys)
            {
                if ((Platform.GetAsyncKeyState((int)oemKey) & 0xFF00) != 0)
                    return (int)oemKey;
            }
            return key;
        }

        private bool OnHotKeyHandler(int inkey, int mod, bool ispressed)
        {
            int key = checkForOmeKeys(inkey);
            if (ispressed && !Enum.IsDefined(typeof(SDL_Keycode_Ignore), key))
            {
                RazorEnhanced.ModKeys cur = RazorEnhanced.ModKeys.None;
                SDL_Keymod keymod = (SDL_Keymod)mod;
                if (keymod.HasFlag(SDL_Keymod.KMOD_LCTRL) || keymod.HasFlag(SDL_Keymod.KMOD_RCTRL))
                    cur |= RazorEnhanced.ModKeys.Control;
                if (keymod.HasFlag(SDL_Keymod.KMOD_LALT) || keymod.HasFlag(SDL_Keymod.KMOD_RALT))
                    cur |= RazorEnhanced.ModKeys.Alt;
                if (keymod.HasFlag(SDL_Keymod.KMOD_LSHIFT) || keymod.HasFlag(SDL_Keymod.KMOD_RSHIFT))
                    cur |= RazorEnhanced.ModKeys.Shift;
                return RazorEnhanced.HotKey.OnKeyDown(Win32Platform.MapKey(key), cur);
            }

            return true;
        }
            private void OnDisconnected()
        {
        }

        private void OnConnected()
        {
            m_ConnectionStart = DateTime.UtcNow;
            m_LastConnection = Engine.IP;
        }

        private void OnClientClosing()
        {
            Close();
        }

        private void OnInitialize()
        {

        }

        public override void SetConnectionInfo(IPAddress addr, int port)
        {
        }

        public override void SetNegotiate(bool negotiate)
        {
        }

        public override bool Attach(int pid)
        {
            return false;
        }

        public override void Close()
        {
            base.Close();
        }

        public override void UpdateTitleBar()
        {
        }


        public override void SetTitleStr(string str)
        {
            //_setTitle(str);
        }

        public override bool OnMessage(MainForm razor, uint wParam, int lParam)
        {
            return false;
        }

        public override bool OnCopyData(IntPtr wparam, IntPtr lparam)
        {
            return false;
        }

        public override void SendToServer(Packet p)
        {
            byte[] data = p.Compile();
            int length = (int) p.Length;
            _sendToServer(ref data, ref length);
        }

        public override void SendToServer(PacketReader pr)
        {
            SendToServer(MakePacketFrom(pr));
        }

        public override void SendToClient(Packet p)
        {
            byte[] data = p.Compile();
            int length = (int) p.Length;

            _sendToClient(ref data, ref length);
        }

        public override void ForceSendToClient(Packet p)
        {
            byte[] data = p.Compile();
            int length = (int) p.Length;

            _sendToClient(ref data, ref length);
        }

        public override void ForceSendToServer(Packet p)
        {
            byte[] data = p.Compile();
            int length = (int) p.Length;

            _sendToServer(ref data, ref length);
        }

        public override void SetPosition(uint x, uint y, uint z, byte dir)
        {
        }

        public override string GetClientVersion()
        {
            return m_ClientVersion;
        }

        public override string GetUoFilePath()
        {
            return _uoFilePath();
        }

        public override IntPtr GetWindowHandle()
        {
            return m_ClientWindow;
        }

        public override uint TotalDataIn()
        {
            return m_In;
        }

        public override uint TotalDataOut()
        {
            return m_Out;
        }

        internal override void RequestMove(Direction m_Dir)
        {
            _requestMove((int) m_Dir, true);
        }

        public override void PathFindTo(Assistant.Point3D Location)
        {
            Assistant.Client.Instance.SendToClientWait(new PathFindTo(Location));
        }

        public void OnFocusGained()
        {
        }

        public void OnFocusLost()
        {
        }
        public override unsafe void SendToClientWait(Packet p)
        {
            SendToClient(p);
        }
        public override unsafe void SendToServerWait(Packet p)
        {
            SendToServer(p);
        }
        public override void BeginCalibratePosition()
        { }
        public override void InitSendFlush()
        { }
        public override bool Ready { get { return m_Ready; } }

    }
}

