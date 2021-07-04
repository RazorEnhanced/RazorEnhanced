using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using Microsoft.Win32.SafeHandles;
using System.Linq;
using System.Reflection;

namespace Assistant
{
	public class FeatureBit
	{
		public static readonly int WeatherFilter = 0;
		public static readonly int LightFilter = 1;
		public static readonly int SmartLT = 2;
		public static readonly int RangeCheckLT = 3;
		public static readonly int AutoOpenDoors = 4;
		public static readonly int UnequipBeforeCast = 5;
		public static readonly int AutoPotionEquip = 6;
		public static readonly int BlockHealPoisoned = 7;
		public static readonly int LoopingMacros = 8; // includes fors and macros running macros
		public static readonly int UseOnceAgent = 9;
		public static readonly int RestockAgent = 10;
		public static readonly int SellAgent = 11;
		public static readonly int BuyAgent = 12;
		public static readonly int PotionHotkeys = 13;
		public static readonly int RandomTargets = 14;
		public static readonly int ClosestTargets = 15;
		public static readonly int OverheadHealth = 16;
		public static readonly int AutolootAgent = 17;
		public static readonly int BoneCutterAgent = 18;
		public static readonly int AdvancedMacros = 19;
		public static readonly int AutoRemount = 20;
		public static readonly int AutoBandage = 21;
		public static readonly int EnemyTargetShare = 22;
		public static readonly int FilterSeason = 23;
		public static readonly int SpellTargetShare = 24;
		public static readonly int HumanoidHealthChecks = 25;
		public static readonly int SpeechJournalChecks = 26;

		public static readonly int MaxBit = 26;
	}
    public abstract class Client
    {
        public static Client Instance;
        public static bool IsOSI;

        private static bool m_Running;
        internal static bool Running { get { return m_Running; } }


        public bool Init(bool isOSI)
        // returns false on cancel
        {

            System.IO.Directory.CreateDirectory(Path.Combine(Assistant.Engine.RootPath, "Profiles"));
            System.IO.Directory.CreateDirectory(Path.Combine(Assistant.Engine.RootPath, "Backup"));
            System.IO.Directory.CreateDirectory(Path.Combine(Assistant.Engine.RootPath, "Scripts"));

            Initialize(typeof(Assistant.Engine).Assembly);

            // Profile
            RazorEnhanced.Profiles.Load();

            // Shard Bookmarks
            RazorEnhanced.Shard.Load();

            RazorEnhanced.Settings.Load(RazorEnhanced.Profiles.LastUsed());

            RazorEnhanced.Shard.Read(out List<RazorEnhanced.Shard> shards);

            RazorEnhanced.Shard selected = Client.Instance.SelectShard(shards);
            m_Running = true;

            if ((!isOSI) || (RazorEnhanced.Settings.General.ReadBool("NotShowLauncher") && File.Exists(selected.ClientPath) && Directory.Exists(selected.ClientFolder) && selected != null))
            {
                Instance.Start(selected);
            }
            else
            {
                RazorEnhanced.UI.EnhancedLauncher launcher = new RazorEnhanced.UI.EnhancedLauncher();
                DialogResult laucherdialog = launcher.ShowDialog();

                if (laucherdialog == DialogResult.OK)                   // Avvia solo se premuto launch e non se exit
                {
                    if (selected == null)
                    {
                        MessageBox.Show("You must select a valid shard!", "ERROR!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                    {
                        RazorEnhanced.Shard.Read(out shards);
                        selected = Instance.SelectShard(shards);
                        if (launcher.ActiveControl.Text == "Launch CUO")
                        {
                            // Spin up CUO
                            Process cuo = new Process();
                            cuo.StartInfo.FileName = selected.CUOClient;
                            int osiEnc = 0;
                            if (selected.OSIEnc)
                            {
                                osiEnc = 5;
                            }
                            var version = FileVersionInfo.GetVersionInfo(selected.ClientPath);
                            string verString = String.Format("{0:00}.{1:0}.{2:0}.{3:D1}", version.FileMajorPart, version.FileMinorPart, version.FileBuildPart, version.FilePrivatePart);
                            cuo.StartInfo.Arguments = String.Format("-ip {0} -port {1} -uopath \"{2}\" -encryption {3} -plugins \"{4}\" -clientversion \"{5}\"",
                                                        selected.Host, selected.Port, selected.ClientFolder, osiEnc,
                                                        System.Reflection.Assembly.GetExecutingAssembly().Location,
                                                        verString);
                            cuo.Start();
                            m_Running = false;
                            return false;
                        }
                        else
                        {
                            Instance.Start(selected);
                        }
                    }
                }
                else
                {
                    m_Running = false;
                    return false;
                }
            }
            return true;

        }



        internal void Start(RazorEnhanced.Shard selected)
        {
            ClientEncrypted = selected.PatchEnc;
            ServerEncrypted = selected.OSIEnc;
            string clientPath = selected.ClientPath;
            string dataDir = selected.ClientFolder;
            string addr = selected.Host;
            int port = selected.Port;

            Ultima.Files.Directory = selected.ClientFolder;
            if (!Language.Load("ENU"))
            {
                //SplashScreen.End();
                MessageBox.Show("Unable to load required file Language/Razor_lang.enu", "ERROR!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                RazorEnhanced.Settings.General.WriteBool("NotShowLauncher", false);
                return;
            }

            if (dataDir != null && Directory.Exists(dataDir))
            {
                Ultima.Files.SetMulPath(dataDir);
            }
            else
            {
                MessageBox.Show("Unable to find the Data Folder " + dataDir, "ERROR!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                RazorEnhanced.Settings.General.WriteBool("NotShowLauncher", false);
                return;
            }

            Language.LoadCliLoc();

            Assistant.Client.Instance.SetConnectionInfo(IPAddress.None, -1);
            if (IsOSI)
            {
                Assistant.Client.Loader_Error result = Assistant.Client.Loader_Error.UNKNOWN_ERROR;

                if (clientPath != null && File.Exists(clientPath))
                    result = Assistant.Client.Instance.LaunchClient(clientPath);

                if (result != Assistant.Client.Loader_Error.SUCCESS)
                {
                    if (clientPath == null && File.Exists(clientPath))
                        MessageBox.Show("Unable to find the client " + clientPath, "ERROR!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    else
                        MessageBox.Show("Unable to launch the client " + clientPath, "ERROR!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    //SplashScreen.End();
                    RazorEnhanced.Settings.General.WriteBool("NotShowLauncher", false);
                    return;
                }
            }


            // if these are null then the registry entry does not exist (old razor version)
            Engine.IP = Resolve(addr);
            if (Engine.IP == IPAddress.None || port == 0)
            {
                MessageBox.Show(Language.GetString(LocString.BadServerAddr), "ERROR!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //SplashScreen.End();
                RazorEnhanced.Settings.General.WriteBool("NotShowLauncher", false);
                return;
            }

            Engine.ClientBuild = FileVersionInfo.GetVersionInfo(clientPath).FileBuildPart;
            Engine.ClientMajor = FileVersionInfo.GetVersionInfo(clientPath).FileMajorPart;

            //SplashScreen.Start();
            //m_ActiveWnd = SplashScreen.Instance;

            Assistant.Client.Instance.SetConnectionInfo(Engine.IP, port);

            Ultima.Multis.PostHSFormat = UsePostHSChanges;

        }
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool SetForegroundWindow(IntPtr hWnd);

        [DllImport("user32.dll", EntryPoint = "SetWindowPos")]
        public static extern IntPtr SetWindowPos(IntPtr hWnd, int hWndInsertAfter, int x, int Y, int cx, int cy, int wFlags);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr ShowWindow(IntPtr hWnd, int nCmdShow);


        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr SetFocus(IntPtr hWnd);
        public static void BringToFront(IntPtr hWnd)
        {
            const int HWND_TOP = 0;
            const int SWP_NOMOVE = 2;
            const int SWP_NOSIZE = 1;
            const int SW_SHOW = 5;
            SetWindowPos(hWnd, HWND_TOP, 0, 0, 0, 0, SWP_NOMOVE | SWP_NOSIZE);
            ShowWindow(hWnd, SW_SHOW);
            SetForegroundWindow(hWnd);
            SetFocus(hWnd);
        }

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool GetWindowRect(HandleRef hWnd, out RECT lpRect);
        [StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            public int Left;        // x position of upper-left corner
            public int Top;         // y position of upper-left corner
            public int Right;       // x position of lower-right corner
            public int Bottom;      // y position of lower-right corner
        }

        public System.Drawing.Rectangle GetUoWindowPos()
        {
            RECT rect;
            System.Drawing.Rectangle outRect = new System.Drawing.Rectangle(-1, -1, 0, 0);
            if (GetWindowRect(new HandleRef(this, Assistant.Client.Instance.GetWindowHandle()), out rect))
            {
                outRect.X = rect.Left;
                outRect.Y = rect.Top;
                outRect.Width = rect.Right - rect.Left;
                outRect.Height = rect.Bottom - rect.Top;
            }
            return outRect;
        }



        private static void Initialize(System.Reflection.Assembly a)
        {
            Type[] types = a.GetTypes();

            foreach (Type t in types)
            {
                MethodInfo init = t.GetMethod("Initialize", BindingFlags.Static | BindingFlags.Public);

                if (init != null)
                    init.Invoke(null, null);
            }
        }

        internal static IPAddress Resolve(string addr)
        {
            IPAddress ipAddr = IPAddress.None;

            if (string.IsNullOrEmpty(addr))
                return ipAddr;

            if (!IPAddress.TryParse(addr, out ipAddr))
            {
                try
                {
                    IPHostEntry iphe = Dns.GetHostEntry(addr);

                    if (iphe.AddressList.Length > 0)
                        ipAddr = iphe.AddressList[iphe.AddressList.Length - 1];
                }
                catch
                {
                }
            }

            return ipAddr;
        }

        internal bool UsePostHSChanges
        {

            get
            {
                if (Engine.ClientVersion.Major > 7)
                {
                    return true;
                }
                else if (Engine.ClientVersion.Major == 7)
                {
                    if (Engine.ClientVersion.Minor > 0)
                    {
                        return true;
                    }
                    else if (Engine.ClientVersion.Build >= 9)
                    {
                        return true;
                    }
                }

                return false;
            }
        }

        public const int WM_USER = 0x400;

        public const int WM_COPYDATA = 0x4A;
        public const int WM_UONETEVENT = WM_USER + 1;

        private ulong m_Features = 0;

        public bool AllowBit(int bit)
        {
            // ENABLE IT ONLY FOR DEBUG
            if (File.Exists(Path.Combine(Assistant.Engine.RootPath, "bypassnegotiate")))
                return true;
            return (m_Features & (1U << bit)) == 0;
        }

        public void SetFeatures(ulong features)
        {
            m_Features = features;
        }
        public abstract void RunUI();
        public abstract RazorEnhanced.Shard SelectShard(System.Collections.Generic.List<RazorEnhanced.Shard> shards);

        protected DateTime m_ConnectionStart;
        //public  DateTime ConnectionStart { get; }
        public DateTime ConnectionStart => m_ConnectionStart;

        protected static IPAddress m_LastConnection;
        public IPAddress LastConnection {
            get {
                if (m_LastConnection == null)
                    return Engine.IP;     // CUO was not calling OnConnect soon enough
                else
                    return m_LastConnection;
            }
        }

        public bool SmartCpuChecked { get { return RazorEnhanced.Settings.General.ReadBool("SmartCPU"); }
                                      set{
                                        RazorEnhanced.Settings.General.WriteBool("SmartCPU", value);
                                        this.SetSmartCPU(value);
                                         }
                                    }

        public abstract Process ClientProcess { get; }
		public abstract  bool ClientRunning { get; }

		public abstract void SetMapWndHandle(Form mapWnd);

		public abstract void RequestStatbarPatch(bool preAOS);

		public abstract void SetCustomNotoHue(int hue);

		public abstract void SetSmartCPU(bool enabled);

		public abstract void SetGameSize(int x, int y);

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
		public abstract Loader_Error LaunchClient(string client);

		public abstract bool ClientEncrypted { get; set; }
		public abstract bool ServerEncrypted { get; set; }

		public abstract bool InstallHooks(IntPtr mainWindow);

		public abstract void SetConnectionInfo(IPAddress addr, int port);
		public abstract void SetNegotiate(bool negotiate);
		public abstract bool Attach(int pid);

		public virtual void Close()
		{
			Client.m_Running = false;

			RazorEnhanced.Settings.General.SaveExitData();

			// Chiuto toolbar
			if (RazorEnhanced.ToolBar.ToolBarForm != null)
				RazorEnhanced.ToolBar.ToolBarForm.Close();

			// Chiuto Spellgrid
			if (RazorEnhanced.SpellGrid.SpellGridForm != null)
				RazorEnhanced.SpellGrid.SpellGridForm.Close();

			// Stoppo tick timer agent
			if (RazorEnhanced.Scripts.Timer != null)
				RazorEnhanced.Scripts.Timer.Close();

			// Stop forzato di tutti i thread agent
			RazorEnhanced.AutoLoot.AutoMode = false;
			RazorEnhanced.Scavenger.AutoMode = false;
			RazorEnhanced.BandageHeal.AutoMode = false;

			if (Assistant.Engine.MainWindow.OrganizerStop.Enabled == true)
				Assistant.Engine.MainWindow.OrganizerStop.PerformClick();

			if (Assistant.Engine.MainWindow.DressStopButton.Enabled == true)
				Assistant.Engine.MainWindow.DressStopButton.PerformClick();

			if (Assistant.Engine.MainWindow.RestockStop.Enabled == true)
				Assistant.Engine.MainWindow.RestockStop.PerformClick();

			RazorEnhanced.UI.EnhancedScriptEditor.End();
		}

		public abstract void SetTitleStr(string str);

		public abstract bool OnMessage(MainForm razor, uint wParam, int lParam);
		public abstract bool OnCopyData(IntPtr wparam, IntPtr lparam);
		public abstract void SendToServer(Packet p);
		public abstract void SendToServer(PacketReader pr);

		public abstract void SendToClient(Packet p);
		public abstract void ForceSendToClient(Packet p);
		public abstract void ForceSendToServer(Packet p);

		// ONLY in Razor Client abstraction
		public abstract void SetPosition(uint x, uint y, uint z, byte dir);
		public abstract string GetClientVersion();
		public abstract string GetUoFilePath();
		public abstract IntPtr GetWindowHandle();
		public abstract uint TotalDataIn();

		public abstract uint TotalDataOut();

        internal abstract void RequestMove(Direction m_Dir);
		public abstract void PathFindTo(Assistant.Point3D location);


		public void RequestTitlebarUpdate()
        {
            // throttle updates, since things like counters might request 1000000 million updates/sec
            if (m_TBTimer == null)
                m_TBTimer = new TitleBarThrottle();

            if (!m_TBTimer.Running)
                m_TBTimer.Start();
        }

        private class TitleBarThrottle : Timer
        {
            public TitleBarThrottle() : base(TimeSpan.FromSeconds(0.25))
            {
            }

            protected override void OnTick()
            {
                Instance.UpdateTitleBar();
            }
        }

        private Timer m_TBTimer;
        public StringBuilder TitleBarBuilder = new StringBuilder();
        private string m_LastPlayerName = "";

        public void ResetTitleBarBuilder()
        {
            // reuse the same sb each time for less damn allocations
            //TitleBarBuilder.Remove(0, TitleBarBuilder.Length);
            //TitleBarBuilder.Insert(0, $"{Config.GetString("TitleBarText")}");
        }

        public virtual void UpdateTitleBar()
        {
            if (!ClientRunning)
                return;

            StringBuilder sb = TitleBarBuilder;

            PlayerData p = World.Player;

            if (p.Name != m_LastPlayerName)
            {
                m_LastPlayerName = p.Name;

                Engine.MainWindow.UpdateTitle();
            }

            sb.Replace(@"{shard}", World.ShardName);

            sb.Replace(@"{str}", p.Str.ToString());
            sb.Replace(@"{hpmax}", p.HitsMax.ToString());

            sb.Replace(@"{dex}", World.Player.Dex.ToString());
            sb.Replace(@"{stammax}", World.Player.StamMax.ToString());

            sb.Replace(@"{int}", World.Player.Int.ToString());
            sb.Replace(@"{manamax}", World.Player.ManaMax.ToString());

            sb.Replace(@"{ar}", p.AR.ToString());
            sb.Replace(@"{tithe}", p.Tithe.ToString());

            sb.Replace(@"{physresist}", p.AR.ToString());
            sb.Replace(@"{fireresist}", p.FireResistance.ToString());
            sb.Replace(@"{coldresist}", p.ColdResistance.ToString());
            sb.Replace(@"{poisonresist}", p.PoisonResistance.ToString());
            sb.Replace(@"{energyresist}", p.EnergyResistance.ToString());

            sb.Replace(@"{luck}", p.Luck.ToString());

            sb.Replace(@"{damage}", String.Format("{0}-{1}", p.DamageMin, p.DamageMax));

            sb.Replace(@"{maxweight}", World.Player.MaxWeight.ToString());

            sb.Replace(@"{followers}", World.Player.Followers.ToString());
            sb.Replace(@"{followersmax}", World.Player.FollowersMax.ToString());

            sb.Replace(@"{gold}", World.Player.Gold.ToString());

            //sb.Replace(@"{gps}", GoldPerHourTimer.Running ? $"{GoldPerHourTimer.GoldPerSecond:N2}" : "-");
            //sb.Replace(@"{gpm}", GoldPerHourTimer.Running ? $"{GoldPerHourTimer.GoldPerMinute:N2}" : "-");
            //sb.Replace(@"{gph}", GoldPerHourTimer.Running ? $"{GoldPerHourTimer.GoldPerHour:N2}" : "-");
            //sb.Replace(@"{goldtotal}", GoldPerHourTimer.Running ? $"{GoldPerHourTimer.GoldSinceStart}" : "-");
            //sb.Replace(@"{goldtotalmin}", GoldPerHourTimer.Running ? $"{GoldPerHourTimer.TotalMinutes:N2} min" : "-");

            //sb.Replace(@"{skill}", SkillTimer.Running ? $"{SkillTimer.Count}" : "-");
            //sb.Replace(@"{gate}", GateTimer.Running ? $"{GateTimer.Count}" : "-");

            sb.Replace(@"{stealthsteps}", StealthSteps.Counting ? StealthSteps.Count.ToString() : "-");
            //Client.ConnectionStart != DateTime.MinValue )
            //time = (int)((DateTime.UtcNow - Client.ConnectionStart).TotalSeconds);
            sb.Replace(@"{uptime}",
                ConnectionStart != DateTime.MinValue
                    ? Utility.FormatTime((int)((DateTime.UtcNow - ConnectionStart).TotalSeconds))
                    : "-");

           // sb.Replace(@"{dps}", DamageTracker.Running ? $"{DamageTracker.DamagePerSecond:N2}" : "-");
            //sb.Replace(@"{maxdps}", DamageTracker.Running ? $"{DamageTracker.MaxDamagePerSecond:N2}" : "-");
            //sb.Replace(@"{maxdamagedealt}", DamageTracker.Running ? $"{DamageTracker.MaxSingleDamageDealt}" : "-");
            //sb.Replace(@"{maxdamagetaken}", DamageTracker.Running ? $"{DamageTracker.MaxSingleDamageTaken}" : "-");
            //sb.Replace(@"{totaldamagedealt}", DamageTracker.Running ? $"{DamageTracker.TotalDamageDealt}" : "-");
            //sb.Replace(@"{totaldamagetaken}", DamageTracker.Running ? $"{DamageTracker.TotalDamageTaken}" : "-");


            string buffList = string.Empty;

#if notimplemented
            if (BuffsTimer.Running)
            {
                StringBuilder buffs = new StringBuilder();
                foreach (BuffsDebuffs buff in World.Player.BuffsDebuffs)
                {
                    int timeLeft = 0;

                    if (buff.Duration > 0)
                    {
                        TimeSpan diff = DateTime.UtcNow - buff.Timestamp;
                        timeLeft = buff.Duration - (int)diff.TotalSeconds;
                    }

                    buffs.Append(timeLeft <= 0
                        ? $"{buff.ClilocMessage1}, "
                        : $"{buff.ClilocMessage1} ({timeLeft}), ");
                }

                buffs.Length = buffs.Length - 2;
                buffList = buffs.ToString();
                sb.Replace(@"{buffsdebuffs}", buffList);
            }
            else
            {
                sb.Replace(@"{buffsdebuffs}", "-");
            }
#endif
            SetTitleStr(sb.ToString());
        }

        public Packet MakePacketFrom(PacketReader pr)
        {
            byte[] data = pr.CopyBytes(0, pr.Length);
            return new Packet(data, pr.Length, pr.DynamicLength);
        }


    // NOT IN Razor Client abstract definition
    public abstract bool Ready { get; }

		public abstract void InitSendFlush();
		public abstract void BeginCalibratePosition();
        public abstract void SendToClientWait(Packet p);
        public abstract void SendToServerWait(Packet p);


    }
}
