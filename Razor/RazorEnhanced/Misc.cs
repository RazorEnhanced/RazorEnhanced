using Assistant;
using Assistant.UI;
using System;
using System.Collections.Generic;
using System.Media;
using System.Threading;
using System.Collections.Concurrent;
using System.Linq;
using System.Drawing;
using static RazorEnhanced.HotKey;
using System.Windows.Forms;

namespace RazorEnhanced
{
    public class Misc
    {
        // Bool per blocco packet in attesa di menu vecchi e vecchio gump response
        internal static bool BlockMenu = false;
        internal static bool BlockGump = false;

        public static void ClearDragQueue()
        {
            Assistant.DragDropManager.Clear();
        }


        public static void CloseBackpack()
        {
            RazorEnhanced.UoWarper.UODLLHandleClass = new RazorEnhanced.UoWarper.UO();

            if (!RazorEnhanced.UoWarper.UODLLHandleClass.Open())
            {
                while (!RazorEnhanced.UoWarper.UODLLHandleClass.Open())
                {
                    Thread.Sleep(50);
                }
            }
            RazorEnhanced.UoWarper.UODLLHandleClass.CloseBackpack();
        }
        // Container Experiment
        // Misc.NextContPosition(80, 80)
        public static void NextContPosition(int x, int y)
        {
            RazorEnhanced.UoWarper.UODLLHandleClass = new RazorEnhanced.UoWarper.UO();

            if (!RazorEnhanced.UoWarper.UODLLHandleClass.Open())
            {
                while (!RazorEnhanced.UoWarper.UODLLHandleClass.Open())
                {
                    Thread.Sleep(50);
                }
            }
            RazorEnhanced.UoWarper.UODLLHandleClass.NextContPos(x, y);
        }
        // p = Misc.GetContPosition()
        // Misc.SendMessage(p.X)
        // Misc.SendMessage(p.Y)
        public static Point GetContPosition()
        {
            RazorEnhanced.UoWarper.UODLLHandleClass = new RazorEnhanced.UoWarper.UO();

            if (!RazorEnhanced.UoWarper.UODLLHandleClass.Open())
            {
                while (!RazorEnhanced.UoWarper.UODLLHandleClass.Open())
                {
                    Thread.Sleep(50);
                }
            }
            Point p = RazorEnhanced.UoWarper.UODLLHandleClass.GetContPos();
            return p;
        }


        //General
        public static void Pause(int mseconds)
        {
            System.Threading.Thread.Sleep(mseconds);
        }

        public static void Resync()
        {
            Assistant.Client.Instance.SendToServer(new ResyncReq());
        }

        public static double DistanceSqrt(Point3D a, Point3D b)
        {
            double distance = Math.Sqrt(((a.X - b.X) ^ 2) + (a.Y - b.Y) ^ 2);
            return distance;
        }

        public static void SendToClient(string keys)
        {
            DLLImport.Win.SetForegroundWindow(Assistant.Client.Instance.GetWindowHandle());
            System.Windows.Forms.SendKeys.SendWait(keys);
        }

        // Sysmessage
        public static void SendMessage(int num)
        {
            SendMessage(num.ToString(), true);
        }

        public static void SendMessage(object obj)
        {
            SendMessage(obj.ToString(), true);
        }

        public static void SendMessage(uint num)
        {
            SendMessage(num.ToString(), true);
        }

        public static void SendMessage(bool msg)
        {
            SendMessage(msg.ToString(), true);
        }

        public static void SendMessage(double msg)
        {
            SendMessage(msg.ToString(), true);
        }

        public static void SendMessage(float num)
        {
            SendMessage(num.ToString(), true);
        }

        public static void SendMessage(int num, int color)
        {
            SendMessage(num.ToString(), color, true);
        }

        public static void SendMessage(object obj, int color)
        {
            SendMessage(obj.ToString(), color, true);
        }

        public static void SendMessage(uint num, int color)
        {
            SendMessage(num.ToString(), color, true);
        }

        public static void SendMessage(bool msg, int color)
        {
            SendMessage(msg.ToString(), color, true);
        }

        public static void SendMessage(double msg, int color)
        {
            SendMessage(msg.ToString(), color, true);
        }

        public static void SendMessage(string msg, bool wait = true)
        {
            SendMessage(msg, 945, wait);
        }

        public static string CurrentScriptDirectory()
        {
            string razorPath = System.IO.Path.Combine(Assistant.Engine.RootPath, "Scripts");
            return razorPath;
        }

        internal static void SendMessage(string msg, int color, bool wait) //Main function of sendmessage
        {
            if (Assistant.World.Player != null)
            {
                if (wait)
                    Assistant.Client.Instance.SendToClientWait(new UnicodeMessage(0xFFFFFFFF, -1, MessageType.Regular, color, 3, Language.CliLocName, "System", msg.ToString()));
                else
                    Assistant.Client.Instance.SendToClient(new UnicodeMessage(0xFFFFFFFF, -1, MessageType.Regular, color, 3, Language.CliLocName, "System", msg.ToString()));
            }
        }

        public static void Beep()
        {
            SystemSounds.Beep.Play();
        }

        // Login and logout
        public static void Disconnect()
        {
            Assistant.Client.Instance.SendToClient(new Disconnect());
        }

        public class Context
        {
            public int Response
            { get; set; }
            public string Entry
            { get; set; }
        }
        // Context Menu
        public static List<Context> WaitForContext(Mobile mob, int delay) // Delay in MS
        {
            return WaitForContext(mob.Serial, delay);
        }

        public static List<Context> WaitForContext(Item i, int delay) // Delay in MS
        {
            return WaitForContext(i.Serial, delay);
        }

        public static List<Context> WaitForContext(int ser, int delay) // Delay in MS
        {
            List<Context> retList = new List<Context>();
            Assistant.Client.Instance.SendToServerWait(new ContextMenuRequest(ser));
            int subdelay = delay;
            while (World.Player.HasContext != true && World.Player.ContextID != ser && subdelay > 0)
            {
                Thread.Sleep(2);
                subdelay -= 2;
            }
            UOEntity ent = null;
            Assistant.Serial menuOwner = new Assistant.Serial((uint)ser);
            if (menuOwner.IsMobile)
                ent = World.FindMobile(menuOwner);
            else if (menuOwner.IsItem)
                ent = World.FindItem(menuOwner);
            if (ent != null)
            {
                foreach (var entry in ent.ContextMenu)
                {
                    Context temp = new Context();
                    temp.Response = entry.Key;
                    temp.Entry = Language.GetString(entry.Value);
                    retList.Add(temp);
                }
            }
            return retList;
        }


        public static void ContextReply(int serial, int idx)
        {
            Assistant.Client.Instance.SendToServerWait(new ContextMenuResponse(serial, (ushort)idx));
            World.Player.HasContext = false;
            World.Player.ContextID = 0;
        }

        public static void ContextReply(Mobile mob, int idx)
        {
            ContextReply(mob.Serial, idx);
        }

        public static void ContextReply(Item item, int idx)
        {
            ContextReply(item.Serial, idx);
        }

        public static void ContextReply(Mobile mob, string menuname)
        {
            ContextReply(mob.Serial, menuname);
        }

        public static void ContextReply(Item item, string menuname)
        {
            ContextReply(item.Serial, menuname);
        }

        public static void ContextReply(int serial, string menuname)
        {
            int idx = -1;
            UOEntity e = World.FindItem(serial);
            if (e == null)
                e = World.FindMobile(serial);

            if (e != null)
            {
                foreach (KeyValuePair<ushort, int> menu in e.ContextMenu)
                {
                    if (Language.GetCliloc(menu.Value).ToLower() == menuname.ToLower())
                    {
                        idx = menu.Key;
                        break;
                    }
                }
                if (idx >= 0)
                {
                    Assistant.Client.Instance.SendToServerWait(new ContextMenuResponse(serial, (ushort)idx));
                    World.Player.HasContext = false;
                    World.Player.ContextID = 0;
                }
                else
                    Scripts.SendMessageScriptError("Script Error: ContextReply: Menu entry " + menuname + " not exist");
            }
            else
                Scripts.SendMessageScriptError("Script Error: ContextReply: Mobile or item not exit");

        }

        // Prompt Message Stuff
        public static void ResetPrompt()
        {
            World.Player.HasPrompt = false;
        }

        public static bool HasPrompt()
        {
            return World.Player.HasPrompt;
        }

        public static void WaitForPrompt(int delay) // Delay in MS
        {
            int subdelay = delay;
            while (!World.Player.HasPrompt && subdelay > 0)
            {
                Thread.Sleep(2);
                subdelay -= 2;
            }
        }

        public static void CancelPrompt()
        {
            Assistant.Client.Instance.SendToServerWait(new PromptResponse(World.Player.PromptSenderSerial, World.Player.PromptID, 0, Language.CliLocName, String.Empty));
            World.Player.HasPrompt = false;
        }

        public static void ResponsePrompt(string text)
        {
            Assistant.Client.Instance.SendToServerWait(new PromptResponse(World.Player.PromptSenderSerial, World.Player.PromptID, 1, Language.CliLocName, text));
            World.Player.HasPrompt = false;
        }

        public static void NoOperation()
        {
            return;
        }


        public static Point MouseLocation()
        {
            System.Drawing.Rectangle windowRect = Client.Instance.GetUoWindowPos();
            Point p = System.Windows.Forms.Cursor.Position;
            if (windowRect.X == -1 || windowRect.Y == -1)
            {
                return new Point(-1, -1);
            }
            p.X = p.X - windowRect.X;
            p.Y = p.Y - windowRect.Y;
            return p;
        }
        public static void MouseMove(int posX, int posY)
        {
            System.Drawing.Rectangle windowRect = Client.Instance.GetUoWindowPos();
            if (windowRect.X == -1 || windowRect.Y == -1)
            {
                return;
            }
            System.Drawing.Point thePoint = new System.Drawing.Point(posX + windowRect.X, posY + windowRect.Y);
            System.Windows.Forms.Cursor.Position = thePoint;
        }

        // Shared Script data
        private static ConcurrentDictionary<string, object> m_sharedscriptdata = new ConcurrentDictionary<string, object>();
        public static ConcurrentDictionary<string, object> SharedScriptData { get => m_sharedscriptdata; set => m_sharedscriptdata = value; }


        public static object ReadSharedValue(string name)
        {
            object data = 0;
            if (m_sharedscriptdata.ContainsKey(name))
                m_sharedscriptdata.TryGetValue(name, out data);
            return data;
        }

        public static void SetSharedValue(string name, object value)
        {
            m_sharedscriptdata.AddOrUpdate(name, value, (key, oldValue) => value);
        }
        public static void RemoveSharedValue(string name)
        {
            m_sharedscriptdata.TryRemove(name, out object data);
        }

        public static bool CheckSharedValue(string name)
        {
            if (m_sharedscriptdata.ContainsKey(name))
                return true;
            else
                return false;
        }

        // Ignore list
        private static List<int> m_serialignorelist = new List<int>();

        public static void IgnoreObject(Item i)
        {
            IgnoreObject(i.Serial);
        }
        public static void IgnoreObject(Mobile m)
        {
            IgnoreObject(m.Serial);
        }

        public static void IgnoreObject(int s)
        {
            if (m_serialignorelist.Contains(s)) // if already exist ignore
                return;

            m_serialignorelist.Add(s);
        }

        public static bool CheckIgnoreObject(Item i)
        {
            return CheckIgnoreObject(i.Serial);
        }

        public static bool CheckIgnoreObject(Mobile m)
        {
            return CheckIgnoreObject(m.Serial);
        }

        public static bool CheckIgnoreObject(int s)
        {
            for (int i = 0; i < m_serialignorelist.Count; i++)
            {
                if (m_serialignorelist[i] == s)
                    return true;
            }
            return false;
        }

        public static void ClearIgnore()
        {
            m_serialignorelist.Clear();
        }

        public static void UnIgnoreObject(int s)
        {
            for (int i = 0; i < m_serialignorelist.Count; ++i)
            {
                if (m_serialignorelist[i] == s)
                {
                    m_serialignorelist.RemoveAt(i);
                    break;
                }
            }
        }

        public static void UnIgnoreObject(Item i)
        {
            UnIgnoreObject(i.Serial);
        }
        public static void UnIgnoreObject(Mobile m)
        {
            UnIgnoreObject(m.Serial);
        }

        // Comandi Script per Menu Old

        public static bool HasMenu()
        {
            return World.Player.HasMenu;
        }

        public static void CloseMenu()
        {
            if (World.Player.HasMenu)
            {
                Assistant.Client.Instance.SendToServerWait(new MenuResponse(World.Player.CurrentMenuS, World.Player.CurrentMenuI, 0, 0, 0));
                World.Player.MenuEntry.Clear();
                World.Player.HasMenu = false;
            }
        }

        public static bool MenuContain(string submenu)
        {
            foreach (PlayerData.MenuItem menuentry in World.Player.MenuEntry)
            {
                if (menuentry.ModelText.Contains(submenu))
                {
                    return true;
                }
            }
            return false;
        }

        public static string GetMenuTitle()
        {
            if (World.Player.HasMenu)
            {
                return World.Player.MenuQuestionText;
            }
            return String.Empty;
        }

        public static void WaitForMenu(int delay) // Delay in MS
        {
            BlockMenu = true;
            int subdelay = delay;
            while (!World.Player.HasMenu && subdelay > 0)
            {
                Thread.Sleep(2);
                subdelay -= 2;
            }
            BlockMenu = false;
        }

        public static void MenuResponse(string submenu) // Delay in MS
        {
            int i = 1;
            foreach (PlayerData.MenuItem menuentry in World.Player.MenuEntry)
            {
                if (menuentry.ModelText.ToLower() == submenu.ToLower())
                {
                    Assistant.Client.Instance.SendToServerWait(new MenuResponse(World.Player.CurrentMenuS, World.Player.CurrentMenuI, (ushort)i, menuentry.ModelID, menuentry.ModelColor));
                    World.Player.MenuEntry.Clear();
                    World.Player.HasMenu = false;
                    return;
                }
                i++;
            }
            Assistant.Client.Instance.SendToServerWait(new MenuResponse(World.Player.CurrentMenuS, World.Player.CurrentMenuI, 0, 0, 0));
            World.Player.MenuEntry.Clear();
            World.Player.HasMenu = false;
            Scripts.SendMessageScriptError("MenuResponse Error: No menu name found");
        }

        // Comandi Query String

        public static bool HasQueryString()
        {
            return World.Player.HasQueryString;
        }

        public static void WaitForQueryString(int delay) // Delay in MS
        {
            BlockGump = true;
            int subdelay = delay;
            while (!World.Player.HasQueryString && subdelay > 0)
            {
                Thread.Sleep(2);
                subdelay -= 2;
            }
            BlockGump = false;
        }

        public static void QueryStringResponse(bool okcancel, string response) // Delay in MS
        {
            Assistant.Client.Instance.SendToServerWait(new StringQueryResponse(World.Player.QueryStringID, World.Player.QueryStringType, World.Player.QueryStringIndex, okcancel, response));
            World.Player.HasQueryString = false;
        }

        // Script function
        public static void ScriptRun(string scriptfile)
        {
            Scripts.EnhancedScript script = Scripts.Search(scriptfile);
            if (script != null)
            {
                script.Run = true;
            }
            else
                Scripts.SendMessageScriptError("ScriptRun: Script not exist");
        }

        public static void ScriptStop(string scriptfile)
        {
            Scripts.EnhancedScript script = Scripts.Search(scriptfile);
            if (script != null)
            {
                script.Run = false;
            }
            else
                Scripts.SendMessageScriptError("ScriptStop: Script not exist");
        }

        public static void ScriptStopAll()
        {
            foreach (RazorEnhanced.Scripts.EnhancedScript scriptdata in RazorEnhanced.Scripts.EnhancedScripts.Values.ToList())
            {
                scriptdata.Run = false;
            }
        }

        public static void CaptureNow()
        {
            ScreenCapManager.CaptureNow();
        }
        public class MapInfo
        {
            public MapInfo()
            {
                PinPosition = new RazorEnhanced.Point2D();
                MapOrigin = new RazorEnhanced.Point2D();
                MapEnd = new RazorEnhanced.Point2D();
            }
            public uint Serial;
            public RazorEnhanced.Point2D PinPosition;
            public RazorEnhanced.Point2D MapOrigin;
            public RazorEnhanced.Point2D MapEnd;
            public ushort Facet;
        }

        public static MapInfo GetMapInfo(uint serial)
        {
            MapInfo mapInfo = null;
            MapItem mapItem = World.FindItem(serial) as MapItem;
            if (mapItem != null)
            {
                mapInfo = new MapInfo();
                mapInfo.Serial = mapItem.Serial;
                mapInfo.PinPosition = new RazorEnhanced.Point2D(new Assistant.Point2D(mapItem.PinPosition.X * mapItem.Multiplier, mapItem.PinPosition.Y * mapItem.Multiplier));
                mapInfo.MapOrigin = mapItem.m_MapOrigin;
                mapInfo.MapEnd = mapItem.m_MapEnd;
                mapInfo.Facet = mapItem.m_Facet;
            }
            return mapInfo;
        }

        public static bool ScriptStatus(string scriptfile)
        {
            Scripts.EnhancedScript script = Scripts.Search(scriptfile);
            if (script != null)
            {
                return script.Run;
            }
            else
            {
                Scripts.SendMessageScriptError("ScriptStatus: Script not exist");
                return false;
            }
        }

        // Pet Rename
        public static void PetRename(int serial, string name)
        {
            Assistant.Client.Instance.SendToServerWait(new RenameRequest((uint)serial, name));
        }

        public static void PetRename(RazorEnhanced.Mobile mob, string name)
        {
            Assistant.Client.Instance.SendToServerWait(new RenameRequest((uint)mob.Serial, name));
        }

        // Lock stealth run
        public static void NoRunStealthToggle(bool enable)
        {
            Engine.MainWindow.SafeAction(s => s.ChkNoRunStealth.Checked = enable);
        }

        public static bool NoRunStealthStatus()
        {
            return Engine.MainWindow.ChkNoRunStealth.Checked;
        }

        public static void FocusUOWindow()
        {
            /* ShowWindow:
             * DOCS: https://docs.microsoft.com/en-us/windows/win32/api/winuser/nf-winuser-showwindow
             * SW_MAXIMIZE = 3    //Maximizes the specified window.
             * SW_MINIMIZE = 6    //Minimizes the specified window and activates the next top - level window in the Z order.
             * SW_RESTORE = 9  //Activates and displays the window.If the window is minimized or maximized, the system restores it to its original size and position. An application should specify this flag when restoring a minimized window.
             * SW_SHOW  = 5    //Activates the window and displays it in its current size and position.
             * SW_SHOWDEFAULT = 10    //Sets the show state based on the SW_ value specified in the STARTUPINFO structure passed to the CreateProcess function by the program that started the application.
             */

            if (DLLImport.Win.IsIconic(Assistant.Client.Instance.GetWindowHandle()))
            { // Minimized
                DLLImport.Win.ShowWindow(Assistant.Client.Instance.GetWindowHandle(), 9); // 9 -> restore
            }

            DLLImport.Win.SetForegroundWindow(Assistant.Client.Instance.GetWindowHandle());
        }

        public static string ShardName()
        {
            return World.ShardName;
        }

        /// <summary>
        /// Return a string containing list RE Python API list in JSON format.
        /// </summary>
        /// <param name="path">Default: RazorEnhanced.json</param>
        /// <param name="pretty">Default: True - Export a more readble version | False - Export a more compact version</param>
        public static void ExportPythonAPI(string path = null, bool pretty = true)
        {
            AutoDoc.ExportPythonAPI(path, pretty);
        }

        /// <summary>
        /// Returns the latest HotKeyEvent recorded by razor.
        /// The HotKeyEvent has 2 properties:
        /// hke.Key: enum System.Windows.Forms.Keys
        /// hke.Timestamp: double repesenting the UnixTimestamp, compatible with python's time.time()
        /// </summary>
        ///
        ///
        /// <example>
        /// last = Misc.LastHotKey()
        /// while True:
        ///     key_event = Misc.LastHotKey()
        ///     if last is None: last = ke
        ///     if key_event.Timestamp > last.Timestamp:
        ///         last = key_event
        ///         Player.HeadMessage(20,"HotKey: {}".format(key_event.HotKey))
        ///     Misc.Pause(1)
        /// </example>

        public static HotKeyEvent LastHotKey()
        {
            return HotKeyEvent.LastEvent;
        }
    }

}
