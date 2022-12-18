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
using AutoUpdaterDotNET;
using CrashReporterDotNET;

namespace Assistant
{


    public partial class Engine
    {
        private static string _rootPath = null;
        internal static int ClientBuild = 50;
        internal static int ClientMajor = 6;

        public static string RootPath =>
            _rootPath ?? (_rootPath = Path.GetDirectoryName(Assembly.GetAssembly(typeof(Engine)).Location));

        private static IPAddress m_ip;

        internal static IPAddress IP
        {
            get { return m_ip; }
            set { m_ip = value; }
        }

        internal static void LogCrash(object exception)
        {
            if (exception == null || (exception is ThreadAbortException))
                return;

            ReportCrash((Exception)exception);

            using (StreamWriter txt = new StreamWriter("Crash.log", true))
            {
                txt.AutoFlush = true;
                txt.WriteLine("Exception @ {0}", DateTime.Now.ToString("MM-dd-yy HH:mm:ss.ffff"));
                txt.WriteLine(exception.ToString());
                txt.WriteLine("");
                txt.WriteLine("");
            }
        }

        internal static Form ActiveWindow { get; set; }

        internal static string ShardList { get; private set; }

        [STAThread]
        public static void Main(string[] Args)
        {

            Application.ThreadException += ApplicationThreadException;

            AppDomain.CurrentDomain.UnhandledException += CurrentDomainOnUnhandledException;

            Thread.CurrentThread.Name = "Razor Main Thread";
            Client theClient = Init();
            if (theClient != null)
            {
                Assistant.Client.Loader_Error result = theClient.LaunchClient();
                if (result != Assistant.Client.Loader_Error.SUCCESS)
                {
                    if (theClient.ClientPath == null || (!File.Exists(theClient.ClientPath)))
                        MessageBox.Show("Unable to find the client " + theClient.ClientPath, "ERROR!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    else
                        MessageBox.Show("Unable to launch the client " + theClient.ClientPath, "ERROR!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    //RazorEnhanced.Settings.General.WriteBool("NotShowLauncher", false);
                    return;
                }


                IPAddress ip = Utility.Resolve(theClient.Addr);
                if (Engine.IP == IPAddress.None || theClient.Port == 0)
                {
                    MessageBox.Show("Bad Server Address", "ERROR!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    //SplashScreen.End();
                    //RazorEnhanced.Settings.General.WriteBool("NotShowLauncher", false);
                    return;
                }

                int ClientBuild = FileVersionInfo.GetVersionInfo(theClient.ClientPath).FileBuildPart;
                int ClientMajor = FileVersionInfo.GetVersionInfo(theClient.ClientPath).FileMajorPart;

                theClient.RunUI();

            }
        }


        internal static Client Init()
        {
            FileVersionInfo version = null;

            System.IO.Directory.CreateDirectory(Path.Combine(Assistant.Engine.RootPath, "Profiles"));
            System.IO.Directory.CreateDirectory(Path.Combine(Assistant.Engine.RootPath, "Backup"));
            System.IO.Directory.CreateDirectory(Path.Combine(Assistant.Engine.RootPath, "Scripts"));

            // Setup AutoUpdater Parameters
            // AutoUpdater
            System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();
            System.Diagnostics.FileVersionInfo fvi = System.Diagnostics.FileVersionInfo.GetVersionInfo(assembly.Location);
            string versionString = fvi.FileVersion;
            AutoUpdater.ReportErrors = true;
            AutoUpdater.InstalledVersion = new Version(fvi.FileVersion);
            AutoUpdater.InstallationPath = Assistant.Engine.RootPath;
            AutoUpdater.CheckForUpdateEvent += AutoUpdaterOnCheckForUpdateEvent;

            // Shard Bookmarks
            RazorEnhanced.Shard.Load();
            RazorEnhanced.Shard.Read(out List<RazorEnhanced.Shard> shards);

            RazorEnhanced.Shard selected = shards.FirstOrDefault(s => s.Selected);

            RazorEnhanced.UI.EnhancedLauncher launcher = new RazorEnhanced.UI.EnhancedLauncher();
            DialogResult laucherdialog = DialogResult.Retry;
            while (laucherdialog == DialogResult.Retry)
            {
                laucherdialog = launcher.ShowDialog();
            }

            if (laucherdialog == DialogResult.OK)                   // Avvia solo se premuto launch e non se exit
            {
                if (selected == null)
                {
                    MessageBox.Show("You must select a valid shard!", "ERROR!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    RazorEnhanced.Shard.Read(out shards);
                    selected = shards.FirstOrDefault(s => s.Selected);
                    version = FileVersionInfo.GetVersionInfo(selected.ClientPath);
                    if (launcher.ActiveControl.Text == "Launch CUO")
                    {
                        //return new CUOClient(selected);
                    }
                    else
                    {
                        return new OSIClient(selected);
                    }
                }
            }
            else
            {
                return null;
            }

            return null;

        }
        
        
        // Used for auto update  prompt
        private static void AutoUpdaterOnCheckForUpdateEvent(UpdateInfoEventArgs args)
        {
            if (args != null)
            {
                if (args.IsUpdateAvailable)
                {
                    DialogResult dialogResult;

                    dialogResult =
                        MessageBox.Show(
                            $@"There is new version {args.CurrentVersion} available. You are using version {args.InstalledVersion}. Do you want to update the application now?", @"Update Available",
                            MessageBoxButtons.YesNo,
                            MessageBoxIcon.Information);

                    if (dialogResult.Equals(DialogResult.Yes))
                    {
                        try
                        {
                            string[] argsx = Environment.GetCommandLineArgs();
                            if (AutoUpdater.DownloadUpdate(args))
                            {
                                Application.Exit();
                                Thread.Sleep(2000); // attesa uscita
                            }
                        }
                        catch (Exception exception)
                        {
                            MessageBox.Show(exception.Message, exception.GetType().ToString(), MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
                        }
                    }
                    else
                    {
                        string reminderPath = Path.Combine(Assistant.Engine.RootPath, "Profiles", "RazorEnhanced.reminder.json");
                        DateTime reminderDate = DateTime.Now.AddDays(7);
                        string xml = Newtonsoft.Json.JsonConvert.SerializeObject(reminderDate, Newtonsoft.Json.Formatting.Indented);
                        File.WriteAllText(reminderPath, xml);
                    }
                }
            }
            else
            {
                MessageBox.Show(
                        @"There is a problem reaching update server please check your internet connection and try again later.",
                        @"Update check failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        internal static void ReportCrash(Exception exception)
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
