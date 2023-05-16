using Assistant;
using System;
using System.IO;
using System.Linq;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Forms;



namespace RazorEnhanced
{
    public partial class Scripts
    {
        internal static Thread ScriptEditorThread;
        internal static bool ScriptErrorLog = false;
        internal static bool ScriptStartStopMessage = false;

        internal static List<ScriptItem> PyScripts = new List<ScriptItem>();
        internal static List<ScriptItem> UosScripts = new List<ScriptItem>();
        internal static List<ScriptItem> CsScripts = new List<ScriptItem>();

        internal static void Clear()
        {
            PyScripts = new List<ScriptItem>();
            UosScripts = new List<ScriptItem>();
            CsScripts = new List<ScriptItem>();
        }

        internal static void ClearScriptKey(Keys key)
        {
            foreach (ScriptItem item in PyScripts)
            {
                if (item.Hotkey == key)
                {
                    item.Hotkey = Keys.None;
                    item.HotKeyPass = true;
                    return;
                }
            }
            foreach (ScriptItem item in UosScripts)
            {
                if (item.Hotkey == key)
                {
                    item.Hotkey = Keys.None;
                    item.HotKeyPass = true;
                    return;
                }
            }
            foreach (ScriptItem item in CsScripts)
            {
                if (item.Hotkey == key)
                {
                    item.Hotkey = Keys.None;
                    item.HotKeyPass = true;
                    return;
                }
            }

        }

        internal static void UpdateScriptKey(string name, Keys key, bool passkey)
        {
            foreach (Scripts.ScriptItem item in Scripts.PyScripts)
            {
                if (item.Filename == name)
                {
                    item.Hotkey = key;
                    item.HotKeyPass = passkey;
                    break;
                }
            }
            foreach (Scripts.ScriptItem item in Scripts.UosScripts)
            {
                if (item.Filename == name)
                {
                    item.Hotkey = key;
                    item.HotKeyPass = passkey;
                    break;
                }
            }
            foreach (Scripts.ScriptItem item in Scripts.CsScripts)
            {
                if (item.Filename == name)
                {
                    item.Hotkey = key;
                    item.HotKeyPass = passkey;
                    break;
                }
            }
            Settings.Save();
        }

        internal static void PatchUpHotkeys(string filename)
        {         
            ScriptItem scriptItem = FindScript(filename);
            if (scriptItem == null)
                return;
            TabControl scriptsTab = MainForm.GetAllScriptsTab();
            if (scriptsTab == null)
                return;

            foreach (TabPage tabPage in scriptsTab.Controls)
            {
                ListView.ListViewItemCollection items = ((RazorEnhanced.UI.ScriptListView)tabPage.Controls[0]).Items;
                foreach (ListViewItem item in items)
                {
                    if (item.Text == filename)
                    {
                        //  Danger, if ever the order is changed this will be wrong
                        item.SubItems[5].Text = HotKey.KeyString(scriptItem.Hotkey);
                    }
                }
            }
        }

        internal static bool UsingKey(Keys key)
        {
            foreach (Scripts.ScriptItem item in Scripts.PyScripts)
            {
                if (item.Hotkey == key)
                    return true;
            }
            foreach (Scripts.ScriptItem item in Scripts.UosScripts)
            {
                if (item.Hotkey == key)
                    return true;
            }
            foreach (Scripts.ScriptItem item in Scripts.CsScripts)
            {
                if (item.Hotkey == key)
                    return true;
            }
            return false;
        }

        internal static ScriptItem FindScript(Keys key)
        {
            foreach (Scripts.ScriptItem item in Scripts.PyScripts)
            {
                if (item.Hotkey == key)
                    return item;
            }
            foreach (Scripts.ScriptItem item in Scripts.UosScripts)
            {
                if (item.Hotkey == key)
                    return item;
            }
            foreach (Scripts.ScriptItem item in Scripts.CsScripts)
            {
                if (item.Hotkey == key)
                    return item;
            }
            return null;
        }

        internal static ScriptItem FindScript(string name)
        {
            foreach (Scripts.ScriptItem item in Scripts.PyScripts)
            {
                if (item.Filename == name)
                    return item;
            }
            foreach (Scripts.ScriptItem item in Scripts.UosScripts)
            {
                if (item.Filename == name)
                    return item;
            }
            foreach (Scripts.ScriptItem item in Scripts.CsScripts)
            {
                if (item.Filename == name)
                    return item;
            }

            return null;
        }

        internal static string GetFullPathForScript(string filename)
        {
            foreach (ScriptItem item in PyScripts)
            {
                if (item.Filename == filename)
                {
                    if (item.FullPath == null)
                    {
                        item.FullPath = Path.Combine(Engine.RootPath, "Scripts", filename);
                    }
                    return item.FullPath;
                }
            }
            foreach (ScriptItem item in UosScripts)
            {
                if (item.Filename == filename)
                {
                    if (item.FullPath == null)
                    {
                        item.FullPath = Path.Combine(Engine.RootPath, "Scripts", filename);
                    }
                    return item.FullPath;
                }
            }
            foreach (ScriptItem item in CsScripts)
            {
                if (item.Filename == filename)
                {
                    if (item.FullPath == null)
                    {
                        item.FullPath = Path.Combine(Engine.RootPath, "Scripts", filename);
                    }
                    return item.FullPath;
                }
            }

            return null;
        }


        internal enum RunMode
        {
            None,
            RunOnce,
            Loop,
        }

        internal static void SendMessageScriptError(string msg)
        {
            if (Assistant.World.Player == null)
                return;

            if (RazorEnhanced.Settings.General.ReadBool("ShowScriptMessageCheckBox"))
                Assistant.Client.Instance.SendToClientWait(new UnicodeMessage(0xFFFFFFFF, -1, MessageType.Regular, 945, 3, Language.CliLocName, "System", msg.ToString()));
        }
        public class ScriptItem : ListAbleItem
        {
            public string Filename { get; set;}
            //public string Flag { get; set; }  // appears unused
            public string Status { get; set; }
            public bool Loop { get; set; }
            public bool Wait { get; set; }
            public Keys Hotkey { get; set; }
            public bool HotKeyPass { get; set; }
            public bool AutoStart { get; set; }
            public string FullPath { get; set; }
        }

        internal class ScriptTimer
        {
            private System.Threading.Timer m_Timer;

            private Thread m_AutoLootThread;
            private Thread m_ScavengerThread;
            private Thread m_BandageHealThread;
            private Thread m_AutoCarverThread;
            private Thread m_BoneCutterThread;
            private Thread m_DragDropThread;
            private Thread m_AutoRemountThread;

            internal ScriptTimer()
            {
            }

            public void Start()
            {
                m_Timer = new System.Threading.Timer(new System.Threading.TimerCallback(OnTick), null, m_TimerDelay, m_TimerDelay);
            }

            public void Stop()
            {
                m_Timer.Change(Timeout.Infinite, Timeout.Infinite);

                foreach (EnhancedScript script in EnhancedScripts.Values.ToList())
                {
                    if (script.IsRunning)
                    {
                        script.Stop();
                    }
                }
            }

            private bool IsRunningThread(Thread thread)
            {
                if (thread == null)
                    return false;

                if (thread != null && ( (thread.ThreadState & ThreadState.Running) != 0 || (thread.ThreadState & ThreadState.WaitSleepJoin) != 0 || (thread.ThreadState & ThreadState.AbortRequested) != 0 ))
                    return true;
                else
                    return false;
            }

            internal void Close()
            {
                try
                {
                    UI.EnhancedScriptEditor.End();

                    this.Stop();

                    if (IsRunningThread(m_AutoLootThread))
                    {
                        m_AutoLootThread.Abort();
                    }

                    if (IsRunningThread(m_ScavengerThread))
                    {
                        m_ScavengerThread.Abort();
                    }

                    if (IsRunningThread(m_BandageHealThread))
                    {
                        m_BandageHealThread.Abort();
                    }

                    if (IsRunningThread(m_DragDropThread))
                    {
                        m_DragDropThread.Abort();
                    }

                    if (IsRunningThread(m_AutoCarverThread))
                    {
                        m_AutoCarverThread.Abort();
                    }

                    if (IsRunningThread(m_AutoRemountThread))
                    {
                        m_AutoRemountThread.Abort();
                    }

                }
                catch
                { }
            }

            static readonly object syncLock = new object();
            private void OnTick(object state)
            {
                lock (syncLock)
                {
                    foreach (EnhancedScript script in EnhancedScripts.Values.ToList())
                    {
                        if (script.Run)
                        {
                            if (ScriptStartStopMessage && script.StartMessage)
                            {
                                Misc.SendMessage("START: " + script.Filename, 70, false);
                                script.StartMessage = false;
                                script.StopMessage = true;
                            }

                            if (script.Loop)
                            {
                                if (script.IsStopped)
                                    script.Reset();

                                if (script.IsUnstarted)
                                    script.Start();
                            }
                            else
                            {
                                if (script.IsStopped)
                                    script.Reset();
                                else if (script.IsUnstarted)
                                    script.Start();
                            }
                        }
                        else
                        {
                            if (ScriptStartStopMessage && script.StopMessage)
                            {
                                Misc.SendMessage("HALT: " + script.Filename, 70, false);
                                script.StartMessage = true;
                                script.StopMessage = false;
                            }

                            if (script.IsRunning)
                                script.Stop();

                            if (script.IsStopped)
                                script.Reset();
                        }
                    }

                    if (World.Player != null && Client.Running) // Parte agent
                    {

                        if (AutoLoot.AutoMode && !IsRunningThread(m_AutoLootThread))
                        {
                            try
                            {
                                m_AutoLootThread = new Thread(AutoLoot.AutoRun);
                                m_AutoLootThread.Name = "AutoLoot Thread";
                                m_AutoLootThread.Start();
                                Thread.Sleep(1);
                            }
                            catch { }
                        }

                        if (Scavenger.AutoMode && !IsRunningThread(m_ScavengerThread))
                        {
                            try
                            {
                                m_ScavengerThread = new Thread(Scavenger.AutoRun);
                                m_ScavengerThread.Name = "Scavenger Thread";
                                m_ScavengerThread.Start();
                                Thread.Sleep(1);
                            }
                            catch { }
                        }

                        if (BandageHeal.AutoMode && !IsRunningThread(m_BandageHealThread))
                        {
                            try
                            {
                                m_BandageHealThread = new Thread(BandageHeal.AutoRun);
                                m_BandageHealThread.Name = "Bandage Thread";
                                m_BandageHealThread.Start();
                                Thread.Sleep(1);
                            }
                            catch { }
                        }

                        if ((Scavenger.AutoMode || AutoLoot.AutoMode || Filters.AutoCarver) && !IsRunningThread(m_DragDropThread))
                        {
                            try
                            {
                                m_DragDropThread = new Thread(DragDropManager.AutoRun);
                                m_DragDropThread.Name = "DragDrop Thread";
                                m_DragDropThread.Start();
                                Thread.Sleep(1);
                            }
                            catch { }
                        }

                        if (Filters.AutoCarver && !IsRunningThread(m_AutoCarverThread))
                        {
                            try
                            {
                                m_AutoCarverThread = new Thread(Filters.CarveAutoRun);
                                m_AutoCarverThread.Name = "AutoCarver Thread";
                                m_AutoCarverThread.Start();
                                Thread.Sleep(1);
                            }
                            catch { }
                        }

                        if (Filters.BoneCutter && !IsRunningThread(m_BoneCutterThread))
                        {
                            try
                            {
                                m_BoneCutterThread = new Thread(Filters.BoneCutterRun);
                                m_BoneCutterThread.Name = "BoneCutter Thread";
                                m_BoneCutterThread.Start();
                                Thread.Sleep(1);
                            }
                            catch { }
                        }

                        if (Filters.AutoModeRemount && !IsRunningThread(m_AutoRemountThread))
                        {
                            try
                            {
                                m_AutoRemountThread = new Thread(Filters.RemountAutoRun);
                                m_AutoRemountThread.Name = "AutoRemount Thread";
                                m_AutoRemountThread.Start();
                                Thread.Sleep(1);
                            }
                            catch { }
                        }
                    }
                }
            }
        }

        private static TimeSpan m_TimerDelay = TimeSpan.FromMilliseconds(100);

        internal static TimeSpan TimerDelay
        {
            get { return m_TimerDelay; }
            set
            {
                if (value != m_TimerDelay)
                {
                    m_TimerDelay = value;
                    Init();
                }
            }
        }

        private static ScriptTimer m_Timer = new ScriptTimer();
        internal static ScriptTimer Timer { get { return m_Timer; } }

        private static readonly ConcurrentDictionary<string, EnhancedScript> m_EnhancedScripts = new ConcurrentDictionary<string, EnhancedScript>();
        internal static ConcurrentDictionary<string, EnhancedScript> EnhancedScripts { get { return m_EnhancedScripts; } }

        public static void Initialize()
        {
            m_Timer.Start();
        }

        internal static void Init()
        {
            if (m_Timer != null)
                m_Timer.Stop();

            m_Timer = new ScriptTimer();
            m_Timer.Start();
        }

        static void ScriptChanged(object sender, FileSystemEventArgs e)
        {
            foreach (KeyValuePair<string, EnhancedScript> pair in EnhancedScripts)
            {
                //if (String.Compare(pair.Key.ToLower(), filename.ToLower()) == 0)
                pair.Value.FileChangeDate = DateTime.MinValue;
            }
        }

        static readonly System.IO.FileSystemWatcher Watcher = SetupFileWatcher();

        static System.IO.FileSystemWatcher SetupFileWatcher()
        {
            System.IO.FileSystemWatcher watcher = new System.IO.FileSystemWatcher();

            watcher.Path = Path.Combine(Assistant.Engine.RootPath, "Scripts");
            watcher.Filter = "*.py";
            watcher.NotifyFilter = NotifyFilters.LastWrite;
            watcher.IncludeSubdirectories = true;
            watcher.Changed += new FileSystemEventHandler(ScriptChanged);
            watcher.EnableRaisingEvents = true;

            return watcher;
        }

        internal static EnhancedScript CurrentScript()
        {
            return Search(Thread.CurrentThread);
        }


        internal static EnhancedScript Search(string filename)
        {
            foreach (KeyValuePair<string, EnhancedScript> pair in EnhancedScripts)
            {
                if (pair.Key.ToLower() == filename.ToLower()) { 
                    return pair.Value; 
                }
            }
            return null;
        }

        internal static EnhancedScript Search(Thread thread)
        {
            foreach (var script in EnhancedScripts.Values)
            {
                if (script.Thread.Equals(thread)) { 
                    return script;
                }
            }
            return null;
        }

        // Autostart
        internal static void AutoStart()
        {
            foreach (EnhancedScript script in EnhancedScripts.Values.ToList())
            {
                if (!script.IsRunning && script.AutoStart)
                    script.Start();
            }
        }
    }
}
