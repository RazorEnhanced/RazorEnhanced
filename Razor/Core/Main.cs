using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Threading;
using System.Windows.Forms;
using CrashReporterDotNET; 

namespace Assistant
{
	internal class Engine
	{
		private static DateTime m_ExpireDate = new DateTime(2018, 4, 1);

		private static IPAddress m_ip;

		internal static IPAddress IP
		{
			get { return m_ip; }
		}

		internal static void LogCrash(object exception)
		{
			if (exception == null || (exception is ThreadAbortException))
				return;

			using (StreamWriter txt = new StreamWriter("Crash.log", true))
			{
				txt.AutoFlush = true;
				txt.WriteLine("Exception @ {0}", DateTime.Now.ToString("MM-dd-yy HH:mm:ss.ffff"));
				txt.WriteLine(exception.ToString());
				txt.WriteLine("");
				txt.WriteLine("");
			}
		}

		private static Version m_ClientVersion = null;

		private static string GetManagedString(IntPtr toManage)
		{
			// Receive the pointer to ANSI character array
			// from API.
			IntPtr pStr = toManage;
			// Construct a string from the pointer.
			string str = Marshal.PtrToStringAnsi(pStr);
			// Free the memory pointed to by the pointer.
			// Marshal.FreeHGlobal(pStr);
			// pStr = IntPtr.Zero;
			// Display the string.
			return str;
			// Console.WriteLine("Returned string : " + str);
		}

		internal static Version ClientVersion
		{
			get
			{
				if (m_ClientVersion == null || m_ClientVersion.Major < 2)
				{
					IntPtr version = ClientCommunication.GetUOVersion();
					string[] split = GetManagedString(version).Split('.');

					if (split.Length < 3)
						return new Version(4, 0, 0, 0);

					int rev = 0;

					if (split.Length > 3)
						rev = Utility.ToInt32(split[3], 0);

					m_ClientVersion = new Version(
						Utility.ToInt32(split[0], 0),
						Utility.ToInt32(split[1], 0),
						Utility.ToInt32(split[2], 0),
						rev);

					if (m_ClientVersion.Major == 0) // sanity check if the client returns 0.0.0.0
						m_ClientVersion = new Version(4, 0, 0, 0);
				}

				return m_ClientVersion;
			}
		}

		internal static int ClientBuild = 50;
		internal static int ClientMajor = 6;

		internal static bool UseNewMobileIncoming
		{
			get
			{
				if (ClientVersion.Major > 7)
				{
					return true;
				}
				else if (ClientVersion.Major == 7)
				{
					if (ClientVersion.Minor > 0 || ClientVersion.Build >= 33)
					{
						return true;
					}
				}

				return false;
			}
		}

		internal static bool UsePostHSChanges
		{
			get
			{
				if (ClientVersion.Major > 7)
				{
					return true;
				}
				else if (ClientVersion.Major == 7)
				{
					if (ClientVersion.Minor > 0)
					{
						return true;
					}
					else if (ClientVersion.Build >= 9)
					{
						return true;
					}
				}

				return false;
			}
		}

		internal static bool UsePostSAChanges
		{
			get
			{
				if (ClientVersion.Major >= 7)
				{
					return true;
				}

				return false;
			}
		}

		internal static bool UsePostKRPackets
		{
			get
			{
				if (ClientVersion.Major >= 7)
				{
					return true;
				}
				else if (ClientVersion.Major >= 6)
				{
					if (ClientVersion.Minor == 0)
					{
						if (ClientVersion.Build == 1)
						{
							if (ClientVersion.Revision >= 7)
								return true;
						}
						else if (ClientVersion.Build > 1)
						{
							return true;
						}
					}
					else
					{
						return true;
					}
				}

				return false;
			}
		}

		internal static string ExePath { get { return Process.GetCurrentProcess().MainModule.FileName; } }
		internal static MainForm MainWindow { get { return MainWnd; } }
		internal static bool Running { get { return m_Running; } }
		internal static Form ActiveWindow { get { return m_ActiveWnd; } set { m_ActiveWnd = value; } }

		// Blocco parametri salvataggio uscita
		internal static int MainWindowX { get { return m_MainWindowX; } set { m_MainWindowX = value; } }

		internal static int MainWindowY { get { return m_MainWindowY; } set { m_MainWindowY = value; } }
		internal static int ToolBarX { get { return m_ToolBarX; } set { m_ToolBarX = value; } }
		internal static int ToolBarY { get { return m_ToolBarY; } set { m_ToolBarY = value; } }
		internal static int GridX { get { return m_GridX; } set { m_GridX = value; } }
		internal static int GridY { get { return m_GridY; } set { m_GridY = value; } }

		internal static string Version
		{
			get
			{
				if (m_Version == null)
				{
					Version v = Assembly.GetCallingAssembly().GetName().Version;
					m_Version = String.Format("{0}.{1}.{2}", v.Major, v.Minor, v.Build);//, v.Revision
				}

				return m_Version;
			}
		}

		internal static string ShardList { get; private set; }

		private static MainForm MainWnd;
		private static Form m_ActiveWnd;
		private static bool m_Running;
		private static int m_ToolBarX;
		private static int m_ToolBarY;
		private static int m_GridX;
		private static int m_GridY;
		private static int m_MainWindowX;
		private static int m_MainWindowY;

		private static string m_Version;

		[STAThread]
		public static void Main(string[] Args)
		{
			Application.ThreadException += ApplicationThreadException;

			AppDomain.CurrentDomain.UnhandledException += CurrentDomainOnUnhandledException;

			m_Running = true;
			Thread.CurrentThread.Name = "Razor Main Thread";

			if (ClientCommunication.InitializeLibrary(Engine.Version) == 0)
				throw new InvalidOperationException("This Razor installation is corrupted.");

			DateTime local = DateTime.Now;
			if (local > m_ExpireDate)
			{
				MessageBox.Show("This Razor installation has expired!, Download new version from: http://www.razorenhanced.org/", "ERROR!", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}

			// Profili
			RazorEnhanced.Profiles.Load();

			// Shard Bookmarks
			RazorEnhanced.Shard.Load();

			// Parametri di razor
			if (RazorEnhanced.Profiles.LastUsed() == "default")
				RazorEnhanced.Settings.ProfileFiles = "RazorEnhanced.settings";
			else
				RazorEnhanced.Settings.ProfileFiles = "RazorEnhanced." + RazorEnhanced.Profiles.LastUsed() + ".settings";

			RazorEnhanced.Settings.Load();


			RazorEnhanced.Shard.Read(out List<RazorEnhanced.Shard> shards);
			RazorEnhanced.Shard selected = shards.FirstOrDefault(s => s.Selected);

			if (RazorEnhanced.Settings.General.ReadBool("NotShowLauncher") && File.Exists(selected.ClientPath) && Directory.Exists(selected.ClientFolder) && selected != null)
			{
				Start(selected);
			}
			else
			{
				RazorEnhanced.UI.EnhancedLauncher launcher = new RazorEnhanced.UI.EnhancedLauncher();
				DialogResult laucherdialog = launcher.ShowDialog();

				if (laucherdialog != DialogResult.Cancel)                   // Avvia solo se premuto launch e non se exit
				{
					if (selected == null)
					{
						MessageBox.Show("You must select a valid shard!", "ERROR!", MessageBoxButtons.OK, MessageBoxIcon.Error);
					}
					else
					{
						RazorEnhanced.Shard.Read(out shards);
						selected = shards.FirstOrDefault(s => s.Selected);
						Start(selected);
					}
				}
			}
		}

		internal static void Start(RazorEnhanced.Shard selected)
		{
			ClientCommunication.ClientEncrypted = selected.PatchEnc;
			ClientCommunication.ServerEncrypted = selected.OSIEnc;
			string clientPath = selected.ClientPath;
			string dataDir = selected.ClientFolder;
			string addr = selected.Host;
			int port = selected.Port;

			Ultima.Files.Directory = selected.ClientFolder;

			if (!Language.Load("ENU"))
			{
				SplashScreen.End();
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
			Initialize(typeof(Assistant.Engine).Assembly);

			ClientCommunication.SetConnectionInfo(IPAddress.None, -1);
			ClientCommunication.Loader_Error result = ClientCommunication.Loader_Error.UNKNOWN_ERROR;

			if (clientPath != null && File.Exists(clientPath))
				result = ClientCommunication.LaunchClient(clientPath);

			if (result != ClientCommunication.Loader_Error.SUCCESS)
			{
				if (clientPath == null && File.Exists(clientPath))
					MessageBox.Show(SplashScreen.Instance, "Unable to find the client " + clientPath, "ERROR!", MessageBoxButtons.OK, MessageBoxIcon.Error);
				else
					MessageBox.Show(SplashScreen.Instance, "Unable to launch the client " + clientPath, "ERROR!", MessageBoxButtons.OK, MessageBoxIcon.Error);
				SplashScreen.End();
				RazorEnhanced.Settings.General.WriteBool("NotShowLauncher", false);
				return;
			}

			// if these are null then the registry entry does not exist (old razor version)
			m_ip = Resolve(addr);
			if (m_ip == IPAddress.None || port == 0)
			{
				MessageBox.Show(Language.GetString(LocString.BadServerAddr), "ERROR!", MessageBoxButtons.OK, MessageBoxIcon.Error);
				SplashScreen.End();
				RazorEnhanced.Settings.General.WriteBool("NotShowLauncher", false);
				return;
			}

			ClientBuild = FileVersionInfo.GetVersionInfo(clientPath).FileBuildPart;
			ClientMajor = FileVersionInfo.GetVersionInfo(clientPath).FileMajorPart;

			SplashScreen.Start();
			m_ActiveWnd = SplashScreen.Instance;

			ClientCommunication.SetConnectionInfo(m_ip, port);

			Ultima.Multis.PostHSFormat = UsePostHSChanges;

			MainWnd = new MainForm();
			Application.Run(MainWnd);

			m_Running = false;

			RazorEnhanced.Settings.General.SaveExitData();

			// Chiuto toolbar
			if (RazorEnhanced.ToolBar.ToolBarForm!= null)
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

			ClientCommunication.Close();
		}

		private static void Initialize(Assembly a)
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

			try
			{
				ipAddr = IPAddress.Parse(addr);
			}
			catch
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

		internal static bool IsElevated
		{
			get
			{
				return new WindowsPrincipal(WindowsIdentity.GetCurrent()).IsInRole(WindowsBuiltInRole.Administrator);
			}
		}
		private static void CurrentDomainOnUnhandledException(object sender, UnhandledExceptionEventArgs unhandledExceptionEventArgs)
		{
			ReportCrash((Exception)unhandledExceptionEventArgs.ExceptionObject);
			Environment.Exit(0);
		}

		private static void ApplicationThreadException(object sender, ThreadExceptionEventArgs e)
		{
			ReportCrash(e.Exception);
		}

		private static void ReportCrash(Exception exception)
		{
			ReportCrash reportCrash = new ReportCrash("razorenhanced@gmail.com");
			reportCrash.CaptureScreen = true;
			reportCrash.IncludeScreenshot = true;
			reportCrash.DoctorDumpSettings = new DoctorDumpSettings
			{
				ApplicationID = new Guid("87af7b0b-2407-4944-b572-caee9d031325"),
			};
			reportCrash.Send(exception);
		}

	}
}