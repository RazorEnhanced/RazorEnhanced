using Assistant;
using System;
using System.IO;
using System.Linq;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Forms;
using System.Threading.Tasks;
using RazorEnhanced.UOS;
using Accord.Math;

namespace RazorEnhanced
{
    public class Scripts
    {
        //internal static Thread ScriptEditorThread;
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

        public static void UpdateScriptItems() {
            Scripts.PyScripts = EnhancedScript.Service.ScriptListTabPy().Apply(script => script.ToScriptItem()).ToList();
            Scripts.CsScripts = EnhancedScript.Service.ScriptListTabCs().Apply(script => script.ToScriptItem()).ToList();
            Scripts.UosScripts = EnhancedScript.Service.ScriptListTabUos().Apply(script => script.ToScriptItem()).ToList();
        }

        public static void LoadEnhancedScripts(List<RazorEnhanced.Scripts.ScriptItem> scriptItems) {
            EnhancedScript.Service.ClearAll();
            int prevPyIndex = 0;
            int prevUosIndex = 0;
            int prevCsIndex = 0;
            foreach (var item in scriptItems)
            {
                // if no fullname then set it to local 
                // if the file doesn't exist at its full path, is it local
                string defaultPath = Path.Combine(Assistant.Engine.RootPath, "Scripts", item.Filename);
                if (!File.Exists(item.FullPath) && File.Exists(defaultPath))
                {
                    item.FullPath = defaultPath;
                }

                // If no position use the previous index + 1
                // In theory this should only happen first load after update
                string suffix = Path.GetExtension(item.FullPath).ToLower();
                switch (suffix)
                {
                    case ".py":
                        if (item.Position == 0)
                        {
                            prevPyIndex++;
                            item.Position = prevPyIndex;
                        }
                        else
                        {
                            prevPyIndex = item.Position;
                        }
                        Scripts.PyScripts.Add(item);
                        break;
                    case ".uos":
                        if (item.Position == 0)
                        {
                            prevUosIndex++;
                            item.Position = prevUosIndex;
                        }
                        else
                        {
                            prevUosIndex = item.Position;
                        }
                        Scripts.UosScripts.Add(item);
                        break;
                    case ".cs":
                        if (item.Position == 0)
                        {
                            prevCsIndex++;
                            item.Position = prevCsIndex;
                        }
                        else
                        {
                            prevCsIndex = item.Position;
                        }
                        Scripts.CsScripts.Add(item);
                        break;
                    default:
                        // drop it
                        break;
                }

                //Dalamar: find a nice way to instantiate EnhancedScripts
                var script = EnhancedScript.FromScriptItem(item);
                if (script == null)
                {
                    Misc.SendMessage($"ERROR: File not found: {item.FullPath}", 138);
                }
            }
            Scripts.UpdateScriptItems();
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

        internal static void SendMessageScriptError(string msg, int color = 945)
        {
            if (Assistant.World.Player == null) { return; }

            if (RazorEnhanced.Settings.General.ReadBool("ShowScriptMessageCheckBox")) {
                string[] lines;
                if (Client.IsOSI){
                    lines = msg.Split('\n');
                } else {
                    lines = new string[]{msg};
                }
                foreach(var line in lines){
                    Assistant.Client.Instance.SendToClientWait(new UnicodeMessage(0xFFFFFFFF, -1, MessageType.Regular, color, 3, Language.CliLocName, "System", line.ToString()));
                }
                
            }
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
            public int Position { get; set; }
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
                EnhancedScript.Service.StopAll();
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
                    //UI.EnhancedScriptEditor.End();
                    ScriptRecorderService.Instance.RemoveAll();

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

            
            private void OnTick(object state)
            {
                var updateScripts = Task.Run(() => OnTickScripts(state));
                var updateAgents = Task.Run(() => OnTickAgents(state));
                while (!updateAgents.IsCompleted || !updateAgents.IsCompleted) {
                    Misc.Pause(1);
                }
            }

            
            static readonly object syncLockScripts = new object();
            private void OnTickScripts(object state)
            {
                
                lock (syncLockScripts)
                {
                    foreach (EnhancedScript script in EnhancedScript.Service.ScriptList())
                    {
                        if (script.IsRunning)
                        {
                            if (ScriptStartStopMessage && script.StartMessage)
                            {
                                Misc.SendMessage("START: " + script.Filename, 70, false);
                                script.StartMessage = false;
                                script.StopMessage = true;
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
                        }
                    }
                }
            }
            


            static readonly object syncLockAgents = new object();
            private void OnTickAgents(object state)
            {
                lock (syncLockAgents)
                {
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

        //Dalamar: Moved to EnhancedScript.cs
        //private static readonly ConcurrentDictionary<string, EnhancedScript> m_EnhancedScripts = new ConcurrentDictionary<string, EnhancedScript>();
        //internal static Dictionary<string, EnhancedScript> EnhancedScripts { get { return EnhancedScript.ScriptList; } }

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

        static internal void ScriptChanged(object sender, FileSystemEventArgs e)
        {
            foreach (var script in EnhancedScript.Service.ScriptList())
            {
                if (script.Fullpath == e.FullPath)
                {
                    bool isRunning = script.IsRunning;

                    if (isRunning)
                        script.Stop();
                    script.Load();
                    script.LastModified = DateTime.MinValue;
                    if (isRunning)
                        script.Start();
                }
            }
        }

        // Autostart
        internal static void AutoStart()
        {
            foreach (EnhancedScript script in EnhancedScript.Service.ScriptList())
            {
                if (!script.IsRunning && script.AutoStart)
                    script.Start();
            }
        }
    }
}
