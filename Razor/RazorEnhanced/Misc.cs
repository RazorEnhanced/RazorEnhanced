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
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;

namespace RazorEnhanced
{
    /// <summary>
    /// The Misc class contains general purpose functions of common use.
    /// </summary>
    public class Misc
    {
        // Bool per blocco packet in attesa di menu vecchi e vecchio gump response
        internal static bool BlockMenu = false;
        internal static bool BlockGump = false;

        /// <summary>
        /// Clear the Drag-n-Drop queue.
        /// </summary>
        public static void ClearDragQueue()
        {
            Assistant.DragDropManager.Clear();
        }

        /// <summary>
        /// Prompt the user with a Target. Open the inspector for the selected target.
        /// </summary>
        public static void Inspect()
        {
            Assistant.Targeting.OneTimeTarget(true, new Assistant.Targeting.TargetResponseCallback(Assistant.Commands.GetInfoTarget_Callback));
        }

        /// <summary>
        /// Close the backpack. 
        /// (OSI client only, no ClassicUO)
        /// </summary>
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
        /// <summary>
        /// @nodoc
        /// </summary>
        public static void Test(uint target, bool display, ushort mx, ushort my)
        {
            // Misc.Test(Player.Serial, True, 6928, 1028)
            var ta = new TrackingArrow(target, display, mx, my);
            Client.Instance.SendToClient(ta);
        }

        internal static bool isSubDirectoryOf(string candidate, string other)
        {
            var isChild = false;
            try
            {
                var candidateInfo = new DirectoryInfo(candidate.ToLower());
                var otherInfo = new DirectoryInfo(other.ToLower());

                while (candidateInfo.Parent != null)
                {
                    if (candidateInfo.Parent.FullName == otherInfo.FullName)
                    {
                        isChild = true;
                        break;
                    }
                    else candidateInfo = candidateInfo.Parent;
                }
            }
            catch (Exception error)
            {
                //var message = String.Format("Unable to check directories {0} and {1}: {2}", candidate, other, error);
                //Trace.WriteLine(message);
            }

            return isChild;
        }
        internal static bool validateFilename(string fileName)
        {
            string dirName = Path.GetDirectoryName(fileName).ToLower();
            string suffix = Path.GetExtension(fileName).ToLower();
            switch (suffix)
            {
                case ".data":
                case ".xml":
                case ".map":
                case ".csv":
                    break;
                default:
                    return false;
            }
            List<string> validPaths = Assistant.Client.Instance.ValidFileLocations();
            foreach (string path in validPaths)
            {
                if (isSubDirectoryOf(dirName, path))
                    return true;
            }
            
            return false;
        }
        /// <summary>
        /// Allows creation and append of a file within RE ValidLocations.
        /// For OSI/RE this is only the RE directory / sub-directories
        /// For CUO/RE this is only CUO or RE directory / sub-directories
        /// The filename MUST end in a limited file suffix list
        /// </summary>
        public static bool AppendToFile(string fileName, string lineOfData)
        {
            if (validateFilename(fileName))
            {
                TextWriter writer = new StreamWriter(fileName, true);
                writer.WriteLine(lineOfData);
                writer.Close();
                return true;
            }
            return false;
        }

        /// <summary>
        /// Allows creation and append of a file within RE ValidLocations.
        /// For OSI/RE this is only the RE directory / sub-directories
        /// For CUO/RE this is only CUO or RE directory / sub-directories
        /// The filename MUST end in a limited file suffix list
        /// Checks to see if an identical line is already in the file, and does not add if it exists
        /// </summary>
        public static bool AppendNotDupToFile(string fileName, string lineOfData)
        {
            if (validateFilename(fileName))
            {
                if (File.Exists(fileName))
                {
                    List<string> lines = System.IO.File.ReadLines(fileName).ToList();
                    if (lines.Contains(lineOfData))
                        return true;
                }
                TextWriter writer = new StreamWriter(fileName, true);
                writer.WriteLine(lineOfData);
                writer.Close();
                return true;
            }
            return false;
        }

        /// <summary>
        /// Allows removal of a line in a file within RE ValidLocations.
        /// For OSI/RE this is only the RE directory / sub-directories
        /// For CUO/RE this is only CUO or RE directory / sub-directories
        /// The filename MUST end in a limited file suffix list
        /// Checks to see if an identical line is in the file, and if it exists, it is removed and file written
        /// </summary>
        public static bool RemoveLineInFile(string fileName, string lineOfData)
        {
            if (validateFilename(fileName))
            {
                if (File.Exists(fileName))
                {
                    List<string> lines = System.IO.File.ReadLines(fileName).ToList();
                    if (lines.Contains(lineOfData))
                    {
                        lines.RemoveAll(item => item == lineOfData);
                        System.IO.File.WriteAllLines(fileName, lines);
                    }
                }
                return true;
            }
            return false;
        }


        /// <summary>
        /// Allows deletion of a file within RE ValidLocations.
        /// For OSI/RE this is only the RE directory / sub-directories
        /// For CUO/RE this is only CUO or RE directory / sub-directories
        /// The filename MUST end in a limited file suffix list
        /// </summary>
        public static bool DeleteFile(string fileName)
        {
            if (validateFilename(fileName))
            {
                if (File.Exists(fileName))
                {
                    File.Delete(fileName);
                }
                return true;
            }
            return false;
        }


        /// <summary>
        /// Open the backpack. 
        /// (OSI client only, no ClassicUO)
        /// </summary>
        public static void OpenPaperdoll()
        {
            RazorEnhanced.UoWarper.UODLLHandleClass = new RazorEnhanced.UoWarper.UO();

            if (!RazorEnhanced.UoWarper.UODLLHandleClass.Open())
            {
                while (!RazorEnhanced.UoWarper.UODLLHandleClass.Open())
                {
                    Thread.Sleep(50);
                }
            }
            RazorEnhanced.UoWarper.UODLLHandleClass.OpenPaperdoll();
        }


        // Container Experiment
        // Misc.NextContPosition(80, 80)
        /// <summary>
        /// Return the X,Y of the next container, relative to the game window.
        /// (OSI client only, no ClassicUO)
        /// </summary>
        /// <param name="x">X coordinate.</param>
        /// <param name="y">Y coordinate.</param>
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
        /// <summary>
        /// Get the position of the currently active Gump/Container.
        /// (OSI client only, no ClassicUO)
        /// </summary>
        /// <returns>Return X,Y coordinates as a Point2D</returns>
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

        // IsItem 
        /// <summary>
        /// Determine if the serial is an item
        /// </summary>
        /// <param name="serial"> Serial number of object to test is Item</param>
        /// <returns>Return True - is an Item False - is not an item</returns>
        public static bool IsItem(System.UInt32  serial)
        {
            Assistant.Serial anObject = new Assistant.Serial(serial);
            return anObject.IsItem;
        }

        // IsMobile
        /// <summary>
        /// Determine if the serial is a mobile
        /// </summary>
        /// <param name="serial"> Serial number of object to test is Mobile</param>
        /// <returns>Return True - is a mobile False - is not a mobile</returns>
        public static bool IsMobile(System.UInt32 serial)
        {
            Assistant.Serial anObject = new Assistant.Serial(serial);
            return anObject.IsMobile;
        }

        //Change Profile
        /// <summary>
        /// Allow the scripted loading of a profile
        /// </summary>
        /// <param name="profileName">Name of profile to load</param>
        public static void ChangeProfile(string profileName)
        {
            Engine.MainWindow.SafeAction(s => s.changeProfile(profileName));
        }

        //General
        /// <summary>
        /// Pause the script for a given amount of time.
        /// </summary>
        /// <param name="millisec">Pause duration, in milliseconds.</param>
        public static void Pause(int millisec)
        {
            System.Threading.Thread.Sleep(millisec);
        }

        /// <summary>
        /// Trigger a client ReSync.
        /// </summary>
        public static void Resync()
        {
            Assistant.Client.Instance.SendToServer(new ResyncReq());
        }

        /// <summary>
        /// Compute the distance between 2 Point3D using pythagorian.
        /// </summary>
        /// <param name="point_a">First coordinates.</param>
        /// <param name="point_b">Second coordinates.</param>
        /// <returns></returns>
        public static double DistanceSqrt(Point3D point_a, Point3D point_b)
        {
            double distance = Math.Sqrt(((point_a.X - point_b.X) ^ 2) + (point_a.Y - point_b.Y) ^ 2);
            return distance;
        }

        /// <summary>
        /// Returns the UO distance between the 2 sets of co-ordinates.
        /// </summary>
        /// <param name="X1">X co-ordinate of first place.</param>
        /// <param name="Y1">Y co-ordinate of first place.</param>
        /// <param name="X2">X co-ordinate of second place.</param>
        /// <param name="Y2">Y co-ordinate of second place.</param>
        public static int Distance(int X1, int Y1, int X2, int Y2)
        {
            return Utility.Distance(X1, Y1, X2, Y2);
        }
        
        /// <summary>
        /// Send to the client a list of keystrokes. Can contain control characters: 
        /// - Send Control+Key: ctrl+u: ^u
        /// - Send ENTER: {Enter}
        /// Note: some keys don't work with ClassicUO (es: {Enter} )
        /// </summary>
        /// <param name="keys">List of keys.</param>
        public static void SendToClient(string keys)
        {
            DLLImport.Win.SetForegroundWindow(Assistant.Client.Instance.GetWindowHandle());
            System.Windows.Forms.SendKeys.SendWait(keys);
        }

        // Sysmessage

        /// <summary>
        /// Send a message to the client.
        /// </summary>
        /// <param name="msg">The object to print.</param>
        /// <param name="color">Color of the message.</param>
        /// <param name="wait">True: Wait for confimation. - False: Returns instatnly.</param>
        public static void SendMessage(string msg, int color, bool wait) //Main function of sendmessage
        {
            if (Assistant.World.Player != null)
            {
                if (wait)
                    Assistant.Client.Instance.SendToClientWait(new UnicodeMessage(0xFFFFFFFF, -1, MessageType.Regular, color, 3, Language.CliLocName, "System", msg.ToString()));
                else
                    Assistant.Client.Instance.SendToClient(new UnicodeMessage(0xFFFFFFFF, -1, MessageType.Regular, color, 3, Language.CliLocName, "System", msg.ToString()));
            }
        }

        public static void SendMessage(object obj, int color)
        {
            SendMessage(obj.ToString(), color, true);
        }

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


        /// <summary>
        /// Get the full path to the main Razor Enhanced folder.
        /// This path maybe be different from the Python starting folder when RE is loaded as plugin (ex: ClassicUO)
        /// </summary>
        /// <returns>Path as text</returns>
        public static string RazorDirectory()
        {
            return Assistant.Engine.RootPath;
        }

        /// <summary>
        /// @nodoc
        /// @rename: Misc.ScriptDirectory() 
        /// </summary>
        public static string CurrentScriptDirectory()
        {
            return ScriptDirectory();
        }

        /// <summary>
        /// Get the full path to the Scripts Directory.
        /// </summary>
        /// <returns>Full path to the Scripts Directory.</returns>
        /// 
        public static string ScriptDirectory()
        {
            return Path.Combine( RazorDirectory(), "Scripts");
        }

        /// <summary>
        /// Get the full path to the Config Directory. 
        /// </summary>
        /// <returns>Full path to the Scripts Directory.</returns>
        /// 
        public static string ConfigDirectory()
        {
            return Path.Combine( RazorDirectory(), "Config");
        }

        /// <summary>
        /// Get the full path to the Config Directory. 
        /// </summary>
        /// <returns>Full path to the Config Directory.</returns>
        /// 
        public static string DataDirectory()
        {
            return Path.Combine( RazorDirectory(), "Data");
        }
        


        /// <summary>
        /// Play Beep system sound.
        /// </summary>
        public static void Beep()
        {
            SystemSounds.Beep.Play();
        }

        // Login and logout
        /// <summary>
        /// Force client to disconnect.
        /// </summary>
        public static void Disconnect()
        {
            Assistant.Client.Instance.SendToServerWait(new Disconnect());

            // Dalamar:
            // Notify the server for disconnection, mimiking manual logout operations.
            // CUO doesn't support Logout via packet ( their method for handling the disconnect packet is empty )
            // So we handshake directly with the server
            if (!Assistant.Client.IsOSI) { 
                Assistant.Client.Instance.SendToServerWait(new LogoffNotification()); // Unnecessary, but mimic the normal packet flow of a client.
                Assistant.Client.Instance.SendToServerWait(new ClosedStatusGump());

                Misc.SendToClient("{ENTER}");
            }
        }


        // Context Menu

        /// <summary>
        /// The Context class holds information about a single entry in the Context Menu.
        /// </summary>
        public class Context
        {
            public int Response
            { get; set; }
            public string Entry
            { get; set; }
        }


        /// <summary>
        /// Open and click the option of Context menu, given the serial of Mobile or Item, via packets.
        /// </summary>
        /// <param name="serial">Serial of the Item or Mobile.</param>
        /// <param name="choice">Option as Text or integer.</param>
        /// <param name="delay">Maximum wait for the action to complete.</param>
        /// <returns>True: Optiona selected succesfully - False: otherwise.</returns>
        public static bool UseContextMenu(int serial, string choice, int delay)
        {     // Delay in MS
            choice = choice.Trim().ToLower();
            var menuList = WaitForContext(serial,delay,false);
            var menuOption = menuList.Where(context => context.Entry.Trim().ToLower() == choice);
            if (menuOption.Count() == 0) {
                return false;
            }
            ContextReply(serial, menuOption.First().Response);
            return true;
        }


        /// <summary>
        /// Return the List entry of a Context menu, of Mobile or Item objects.
        /// The function will ask the server for the List and wait for a maximum amount of time.
        /// </summary>
        /// <param name="serial">Serial of the entity.</param>
        /// <param name="delay">Maximum wait.</param>
        /// <param name="showContext">Show context menu in-game. (default: True)</param>
        /// <returns>A List of Context objects.</returns>
        public static List<Context> WaitForContext(int serial, int delay, bool showContext=false) // Delay in MS
        {
            if (!showContext)
            {
                var HideUntilMax = Math.Min(10000, Math.Max(1000, delay)); // at least 1 sec, but nmo more than 10 seconds
                Assistant.PacketHandlers.HideContextUntil = DateTime.Now.AddMilliseconds(HideUntilMax);
            }
            List<Context> retList = new List<Context>();
            Assistant.Client.Instance.SendToServerWait(new ContextMenuRequest(serial));
            int subdelay = delay;
            while (World.Player.HasContext != true && World.Player.ContextID != serial && subdelay > 0)
            {
                Thread.Sleep(2);
                subdelay -= 2;
            }
            UOEntity ent = null;
            Assistant.Serial menuOwner = new Assistant.Serial((uint)serial);
            if (menuOwner.IsMobile)
                ent = World.FindMobile(menuOwner);
            else if (menuOwner.IsItem)
                ent = World.FindItem(menuOwner);
            if (ent != null)
            {
                foreach (var entry in ent.ContextMenu)
                {
                    Context temp = new Context
                    {
                        Response = entry.Key,
                        Entry = Language.GetString(entry.Value)
                    };
                    retList.Add(temp);
                }
            }
            return retList;
        }

        /// <param name="mob">Entity as Item object.</param>
        /// <param name="delay">max time to wait for context</param>
        /// <param name="showContext"></param>
        public static List<Context> WaitForContext(Mobile mob, int delay, bool showContext= false) // Delay in MS
        {
            return WaitForContext(mob.Serial, delay, showContext);
        }

        /// <param name="itm">Entity as Item object.</param>
        /// <param name="delay">max time to wait for context</param>
        /// <param name="showContext"></param>
        public static List<Context> WaitForContext(Item itm, int delay, bool showContext = false) // Delay in MS
        {
            return WaitForContext(itm.Serial, delay, showContext);
        }

        /// <summary>
        /// Respond to a context menu on mobile or item. Menu ID is base zero, or can use string of menu text.
        /// </summary>
        /// <param name="serial">Serial of the Entity</param>
        /// <param name="respone_num">Poition of the option in the menu. Starts from 0.</param>
        public static void ContextReply(int serial, int respone_num)
        {
            Assistant.Client.Instance.SendToServerWait(new ContextMenuResponse(serial, (ushort)respone_num));
            World.Player.HasContext = false;
            World.Player.ContextID = 0;
        }

        /// <param name="serial">serial number of the item to get a context menu from</param>
        /// <param name="menu_name">Name of the Entry as wirtten in-game.</param>
        public static void ContextReply(int serial, string menu_name)
        {
            int idx = -1;
            UOEntity e = World.FindItem(serial);
            if (e == null)
                e = World.FindMobile(serial);

            if (e != null)
            {
                foreach (KeyValuePair<ushort, int> menu in e.ContextMenu)
                {
                    if (Language.GetCliloc(menu.Value).ToLower() == menu_name.ToLower())
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
                    Scripts.SendMessageScriptError("Script Error: ContextReply: Menu entry " + menu_name + " not exist");
            }
            else
                Scripts.SendMessageScriptError("Script Error: ContextReply: Mobile or item not exit");
        }

        public static void ContextReply(Mobile mob, int menu_num)
        {
            ContextReply(mob.Serial, menu_num);
        }

        public static void ContextReply(Item itm, int menu_num)
        {
            ContextReply(itm.Serial, menu_num);
        }

        public static void ContextReply(Mobile mob, string menu_name)
        {
            ContextReply(mob.Serial, menu_name);
        }

        public static void ContextReply(Item itm, string menu_name)
        {
            ContextReply(itm.Serial, menu_name);
        }

        // Prompt Message Stuff
        /// <summary>
        /// Reset a prompt response.
        /// </summary>
        public static void ResetPrompt()
        {
            World.Player.HasPrompt = false;
        }

        /// <summary>
        /// Check if have a prompt request.
        /// </summary>
        /// <returns>True: there is a prompt - False: otherwise</returns>
        public static bool HasPrompt()
        {
            return World.Player.HasPrompt;
        }

        /// <summary>
        /// Wait for a prompt for a maximum amount of time.
        /// </summary>
        /// <param name="delay">Maximum wait time.</param>
        /// <returns>True: Prompt is present - False: otherwise</returns>
        public static bool WaitForPrompt(int delay) // Delay in MS
        {
            int subdelay = delay;
            while (!World.Player.HasPrompt && subdelay > 0)
            {
                Thread.Sleep(2);
                subdelay -= 2;
            }
            return World.Player.HasPrompt;
        }

        /// <summary>
        /// Cancel a prompt request.
        /// </summary>
        public static void CancelPrompt()
        {
            Assistant.Client.Instance.SendToServerWait(new PromptResponse(World.Player.PromptSenderSerial, World.Player.PromptID, 0, Language.CliLocName, String.Empty));
            World.Player.HasPrompt = false;
        }

        /// <summary>
        /// Response a prompt request. Often used to rename runes and similar.
        /// </summary>
        /// <param name="text">Text of the response.</param>
        public static void ResponsePrompt(string text)
        {
            Assistant.Client.Instance.SendToServerWait(new PromptResponse(World.Player.PromptSenderSerial, World.Player.PromptID, 1, Language.CliLocName, text));
            World.Player.HasPrompt = false;
        }

        /// <summary>
        /// Just do nothing and enjot the present moment.
        /// </summary>
        public static void NoOperation()
        {
            return;
        }


        /// <summary>
        /// Returns a point with the X and Y coordinates of the mouse relative to the UO Window
        /// </summary>
        /// <returns>Return X,Y coords as Point object.</returns>
        public static Point MouseLocation()
        {
            System.Drawing.Rectangle windowRect = Client.Instance.GetUoWindowPos();
            Point p = System.Windows.Forms.Cursor.Position;
            if (windowRect.X == -1 || windowRect.Y == -1)
            {
                return new Point(-1, -1);
            }
            p.X -= windowRect.X;
            p.Y -= windowRect.Y;
            return p;
        }
        /// <summary>
        /// Moves the mouse pointer to the position X,Y relative to the UO window
        /// </summary>
        /// <param name="posX">X screen coordinate.</param>
        /// <param name="posY">Y screen coordinate.</param>
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

        /// <summary>@nodoc</summary>
        public static ConcurrentDictionary<string, object> SharedScriptData { get => m_sharedscriptdata; set => m_sharedscriptdata = value; }


        /// <summary>
        /// Get a Shared Value, if value not exist return null.
        /// Shared values are accessible by every script.
        /// </summary>
        /// <param name="name">Name of the value.</param>
        /// <returns>The stored object.</returns>
        public static object ReadSharedValue(string name)
        {
            object data = 0;
            if (m_sharedscriptdata.ContainsKey(name))
                m_sharedscriptdata.TryGetValue(name, out data);
            return data;
        }

        /// <summary>
        /// Set a Shared Value by specific name, if value exist he repalce value.
        /// Shared values are accessible by every script.
        /// </summary>
        /// <param name="name">Name of the value.</param>
        /// <param name="value">Value to set.</param>
        public static void SetSharedValue(string name, object value)
        {
            m_sharedscriptdata.AddOrUpdate(name, value, (key, oldValue) => value);
        }

        /// <summary>
        /// Remove a Shared Value.
        /// </summary>
        /// <param name="name">Name of the value.</param>
        public static void RemoveSharedValue(string name)
        {
            m_sharedscriptdata.TryRemove(name, out _);
        }

        /// <summary>
        /// Check if a shared value exixts.
        /// </summary>
        /// <param name="name">Name of the value.</param>
        /// <returns>True: Shared value exists - False: otherwise.</returns>
        public static bool CheckSharedValue(string name)
        {
            if (m_sharedscriptdata.ContainsKey(name))
                return true;
            else
                return false;
        }

        // Ignore list
        private static readonly List<int> m_serialignorelist = new List<int>();

        /// <summary>
        /// Add an entiry to the ignore list. Can ignore Serial, Items or Mobiles.
        /// </summary>
        /// <param name="serial">Serial to ignore.</param>
        public static void IgnoreObject(int serial)
        {
            if (m_serialignorelist.Contains(serial)) // if already exist ignore
                return;

            m_serialignorelist.Add(serial);
        }

        /// <param name="itm">Item to ignore</param>
        public static void IgnoreObject(Item itm)
        {
            IgnoreObject(itm.Serial);
        }
        /// <param name="mob">Mobile to ignore</param>
        public static void IgnoreObject(Mobile mob)
        {
            IgnoreObject(mob.Serial);
        }


        /// <summary>
        /// Check object from ignore list, return true if present. Can check Serial, Items or Mobiles
        /// </summary>
        /// <param name="serial">Serial to check.</param>
        /// <returns>True: Object is ignored - False: otherwise.</returns>
        public static bool CheckIgnoreObject(int serial)
        {
            for (int i = 0; i < m_serialignorelist.Count; i++)
            {
                if (m_serialignorelist[i] == serial)
                    return true;
            }
            return false;
        }

        /// <param name="itm">Item to check</param>
        public static bool CheckIgnoreObject(Item itm)
        {
            return CheckIgnoreObject(itm.Serial);
        }

        /// <param name="mob">Mobile to check</param>
        public static bool CheckIgnoreObject(Mobile mob)
        {
            return CheckIgnoreObject(mob.Serial);
        }


        /// <summary>
        /// Clear ignore list from all object
        /// </summary>
        public static void ClearIgnore()
        {
            m_serialignorelist.Clear();
        }

        /// <summary>
        /// Remove object from ignore list. Can remove serial, items or mobiles
        /// </summary>
        /// <param name="serial">Serial to unignore.</param>
        public static void UnIgnoreObject(int serial)
        {
            for (int i = 0; i < m_serialignorelist.Count; ++i)
            {
                if (m_serialignorelist[i] == serial)
                {
                    m_serialignorelist.RemoveAt(i);
                    break;
                }
            }
        }

        /// <param name="itm">Item to unignore.</param>
        public static void UnIgnoreObject(Item itm)
        {
            UnIgnoreObject(itm.Serial);
        }

        /// <param name="mob">Item to unignore</param>
        public static void UnIgnoreObject(Mobile mob)
        {
            UnIgnoreObject(mob.Serial);
        }

        // Comandi Script per Menu Old

        /// <summary>
        /// Check if an Old Menu is open.
        /// </summary>
        /// <returns>True: is open - False: otherwise</returns>
        public static bool HasMenu()
        {
            return World.Player.HasMenu;
        }

        /// <summary>
        /// Close opened Old Menu.
        /// </summary>
        public static void CloseMenu()
        {
            if (World.Player.HasMenu)
            {
                Assistant.Client.Instance.SendToServerWait(new MenuResponse(World.Player.CurrentMenuS, World.Player.CurrentMenuI, 0, 0, 0));
                World.Player.MenuEntry.Clear();
                World.Player.HasMenu = false;
            }
        }

        /// <summary>
        /// Search in open Old Menu if contains a specific text.
        /// </summary>
        /// <param name="text">Text to search.</param>
        /// <returns>True: Text found - False: otherwise.</returns>
        public static bool MenuContain(string text)
        {
            foreach (PlayerData.MenuItem menuentry in World.Player.MenuEntry)
            {
                if (menuentry.ModelText.Contains(text))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Get the title of title for open Old Menu.
        /// </summary>
        /// <returns>Text of the title.</returns>
        public static string GetMenuTitle()
        {
            if (World.Player.HasMenu)
            {
                return World.Player.MenuQuestionText;
            }
            return String.Empty;
        }

        /// <summary>
        /// Pause script until server send an Old Menu, for a maximum amount of time.
        /// </summary>
        /// <param name="delay">Maximum wait, in milliseconds.</param>
        /// <returns>True: if the Old Menu is open - False: otherwise.</returns>
        public static bool WaitForMenu(int delay) // Delay in MS
        {
            BlockMenu = true;
            int subdelay = delay;
            while (!World.Player.HasMenu && subdelay > 0)
            {
                Thread.Sleep(2);
                subdelay -= 2;
            }
            BlockMenu = false;

            return World.Player.HasMenu;
        }

        /// <summary>
        /// Perform a menu response by subitem name. If item not exist close menu.
        /// </summary>
        /// <param name="text">Name of subitem to respond.</param>
        public static void MenuResponse(string text) // Delay in MS
        {
            int i = 1;
            foreach (PlayerData.MenuItem menuentry in World.Player.MenuEntry)
            {
                if (menuentry.ModelText.ToLower() == text.ToLower())
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

        /// <summary>
        /// Check if a have a query string menu opened, return true or false.
        /// </summary>
        /// <returns>True: Has quesy - False: otherwise.</returns>
        public static bool HasQueryString()
        {
            return World.Player.HasQueryString;
        }

        /// <summary>
        /// Pause script until server send query string request, for a maximum amount of time.
        /// </summary>
        /// <param name="delay">Maximum wait, in milliseconds.</param>
        /// <returns>True: if player has a query - False: otherwise.</returns>
        public static bool WaitForQueryString(int delay) // Delay in MS
        {
            BlockGump = true;
            int subdelay = delay;
            while (!World.Player.HasQueryString && subdelay > 0)
            {
                Thread.Sleep(2);
                subdelay -= 2;
            }
            BlockGump = false;

            return World.Player.HasQueryString;
        }

        /// <summary>
        /// Perform a query string response by ok or cancel button and specific response string.
        /// </summary>
        /// <param name="okcancel">OK Button</param>
        /// <param name="response">Cancel Button</param>
        public static void QueryStringResponse(bool okcancel, string response) // Delay in MS
        {
            Assistant.Client.Instance.SendToServerWait(new StringQueryResponse(World.Player.QueryStringID, World.Player.QueryStringType, World.Player.QueryStringIndex, okcancel, response));
            World.Player.HasQueryString = false;
        }

        // Script function
        /// <summary>
        /// Run a script by file name, Script must be present in script grid.
        /// </summary>
        /// <param name="scriptfile">Name of the script.</param>
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

        /// <summary>
        /// Stop a script by file name, Script must be present in script grid.
        /// </summary>
        /// <param name="scriptfile">Name of the script.</param>
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

        /// <summary>
        /// Stop all script running.
        /// </summary>
        public static void ScriptStopAll()
        {
            foreach (RazorEnhanced.Scripts.EnhancedScript scriptdata in RazorEnhanced.Scripts.EnhancedScripts.Values.ToList())
            {
                scriptdata.Run = false;
            }
        }

        /// <summary>
        /// Get status of script if running or not, Script must be present in script grid.
        /// </summary>
        /// <param name="scriptfile"></param>
        /// <returns>True: Script is running - False: otherwise.</returns>
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


        /// <summary>
        /// Creates a snapshot of the current UO window.
        /// </summary>
        public static void CaptureNow()
        {
            ScreenCapManager.CaptureNow();
        }


        /// <summary>
        /// The MapInfo class is used to store information about the Map location.
        /// </summary>
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

        /// <summary>
        /// Get MapInfo about a Mobile or Item using the serial
        /// </summary>
        /// <param name="serial">Serial of the object.</param>
        /// <returns>A MapInfo object.</returns>
        public static MapInfo GetMapInfo(uint serial)
        {
            MapInfo mapInfo = new MapInfo
            {
                Serial = serial,
                PinPosition = RazorEnhanced.Point2D.Zero,
                MapOrigin = RazorEnhanced.Point2D.Zero,
                MapEnd = RazorEnhanced.Point2D.Zero,
                Facet = 0
            };

            if (MapItem.MapItemHistory.ContainsKey(serial)) {
                MapItem theItem = MapItem.MapItemHistory[serial];
                mapInfo = new MapInfo
                {
                    Serial = serial,
                    PinPosition = theItem.PinPosition,
                    MapOrigin = theItem.MapOrigin,
                    MapEnd = theItem.MapEnd,
                    Facet = theItem.Facet
                };
            }
            return mapInfo;
        }



        // Pet Rename
        /// <summary>
        /// Rename a specific pet.
        /// </summary>
        /// <param name="serial">Serial of the pet.</param>
        /// <param name="name">New name to set.</param>
        public static void PetRename(int serial, string name)
        {
            Assistant.Client.Instance.SendToServerWait(new RenameRequest((uint)serial, name));
        }

        /// <param name="mob">Mobile object representing the pet.</param>
        /// <param name="name">name to assign to the pet</param>
        public static void PetRename(RazorEnhanced.Mobile mob, string name)
        {
            Assistant.Client.Instance.SendToServerWait(new RenameRequest((uint)mob.Serial, name));
        }

        // Lock stealth run
        /// <summary>
        /// Set "No Run When Stealth" via scripting. Changes via scripting are not persistents.
        /// </summary>
        /// <param name="enable">True: enable the option.</param>
        public static void NoRunStealthToggle(bool enable)
        {
            Engine.MainWindow.SafeAction(s => s.ChkNoRunStealth.Checked = enable);
        }

        /// <summary>
        /// Get the status of "No Run When Stealth" via scripting.
        /// </summary>
        /// <returns>True: Open is active - False: otherwise.</returns>
        public static bool NoRunStealthStatus()
        {
            return Engine.MainWindow.ChkNoRunStealth.Checked;
        }

        /// <summary>
        /// Set UoClient window in focus or restore if minimized.
        /// </summary>
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

        /// <summary>
        /// Get the name of the shard.
        /// </summary>
        /// <returns>Name of the shard</returns>
        public static string ShardName()
        {
            return World.ShardName;
        }

        /// <summary>
        /// Return a string containing list RE Python API list in JSON format.
        /// </summary>
        /// <param name="path">Name of the output file. (default: Config/AutoComplete.json )</param>
        /// <param name="pretty">Print a readable JSON. (default: True )</param>
        public static void ExportPythonAPI(string path = null, bool pretty = true)
        {
            AutoDocIO.ExportPythonAPI(path, pretty);
        }

        /// <summary>
        /// Returns the latest HotKey recorded by razor as HotKeyEvent object.
        /// </summary>
        public static HotKeyEvent LastHotKey()
        {
            return HotKeyEvent.LastEvent;
        }

        
        /// <summary>
        /// Enable or disable the Seasons filter forcing a specific season
        /// Season filter state will be saved on logout but not the season flag that will be recovered.
        /// </summary>
        /// <param name="enable">True: enable seasons filter</param>
        /// <param name="seasonFlag">
        ///     0: Spring (default fallback)
        ///     1: Summer
        ///     2: Fall
        ///     3: Winter
        ///     4: Desolation
        /// </param>
        public static void FilterSeason(Boolean enable, uint seasonFlag)
        {
            System.Collections.ArrayList filters = Assistant.Filters.Filter.List;
            System.Windows.Forms.CheckState checkState;

            checkState = (enable == true) ? System.Windows.Forms.CheckState.Checked : System.Windows.Forms.CheckState.Unchecked;

            int cnt = 0;
            foreach (var filter in filters)
            {
                if (filter is Assistant.Filters.SeasonFilter seasons)
                {
                    World.Player.ForcedSeason = (byte)seasonFlag;
                    // Setting the Checked box on the Seasons Filter enabling or disabling the Filter
                    System.Windows.Forms.CheckedListBox checkbox = Engine.MainWindow.Controls["tabs"].Controls["generalTab"].Controls["groupBox1"].Controls["filters"] as System.Windows.Forms.CheckedListBox;
                    Client.Instance.ForceSendToClient(new SeasonChange(World.Player.ForcedSeason, true));
                    checkbox.SetItemCheckState(cnt, checkState);
                    break;
                }
                cnt++;
            }
        }



        /*
         Dalamar: it is theoretically possible to click the window without moving the Cursor.
         should be done via SendMessage
         see more: https://docs.microsoft.com/en-us/windows/win32/api/winuser/nf-winuser-sendmessage



        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern bool SendMessage(IntPtr hWnd, uint Msg, int wParam, int lParam);


        var coords = ((ypos << 16) | xpos);
        SendMessage(window, WM_LBUTTONDOWN, 0, coords);
        SendMessage(window, WM_LBUTTONUP, 0, coords);

        internal const int WM_LBUTTONDOWN = 0x0201;
        internal const int WM_LBUTTONUP = 0x0202;
       */

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        internal static extern bool GetWindowRect(IntPtr hWnd, ref Rectangle lpRect);

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        internal static extern bool ClientToScreen(IntPtr hWnd, ref Point lpPoint);

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern bool SetCursorPos(int x, int y);

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        internal static extern bool GetCursorPos(ref Point lpPoint);

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        internal static extern void mouse_event(int dwFlags, int dx, int dy, int cButtons, int dwExtraInfo);

        
        internal const int MOUSEEVENTF_ABSOLUTE = 0x8000;
        internal const int MOUSEEVENTF_LEFTDOWN = 0x0002;
        internal const int MOUSEEVENTF_LEFTUP = 0x0004;
        internal const int MOUSEEVENTF_RIGHTDOWN = 0x0008;
        internal const int MOUSEEVENTF_RIGHTUP = 0x0010;

        //This simulates a left mouse click
        /// <summary>
        /// Perform a phisical left click on the window using Windows API.
        /// Is possible to use abolute Screen Coordinates by setting clientCoords=False.
        /// </summary>
        /// <param name="xpos">X click coordinate.</param>
        /// <param name="ypos">Y click coordinate.</param>
        /// <param name="clientCoords">True: Client coordinates.- False:Screen coordinates (default: True, client).</param>
        public static void LeftMouseClick(int xpos, int ypos, bool clientCoords = true)
        {
            var window = Assistant.Client.Instance.GetWindowHandle();
            Point old_point = new Point();
            GetCursorPos(ref old_point);
            
            if (clientCoords) { 
                Point pnt = new Point { X = xpos, Y = ypos };
                ClientToScreen(window, ref pnt);
                xpos = pnt.X;
                ypos = pnt.Y;
            }

            SetCursorPos(xpos, ypos);
            mouse_event(MOUSEEVENTF_LEFTDOWN, xpos, ypos, 0, 0);
            mouse_event(MOUSEEVENTF_LEFTUP, xpos, ypos, 0, 0);
            SetCursorPos(old_point.X, old_point.Y);


        }

        /// <summary>
        /// Perform a phisical Right click on the window.
        /// </summary>
        /// <param name="xpos">X click coordinate.</param>
        /// <param name="ypos">Y click coordinate.</param>
        /// <param name="clientCoords">True: Client coordinates - False: Screen coordinates (default: True, client).</param>
        public static void RightMouseClick(int xpos, int ypos, bool clientCoords = true)
        {
            Point old_point = new Point();
            GetCursorPos(ref old_point);

            if (clientCoords)
            {
                Point pnt = new Point { X = xpos, Y = ypos };
                var window = Assistant.Client.Instance.GetWindowHandle();
                ClientToScreen(window, ref pnt);
                xpos = pnt.X;
                ypos = pnt.Y;      
            }
            SetCursorPos(xpos, ypos);
            mouse_event(MOUSEEVENTF_RIGHTDOWN, xpos, ypos, 0, 0);
            mouse_event(MOUSEEVENTF_RIGHTUP, xpos, ypos, 0, 0);
            SetCursorPos(old_point.X, old_point.Y);
        }

        /// <summary>
        /// Get a Rectangle representing the window size.
        /// See also: https://docs.microsoft.com/dotnet/api/system.drawing.rectangle
        /// </summary>
        /// <returns>Rectangle object. Properties: X, Y, Width, Height.</returns>
        public static Rectangle GetWindowSize() {
            return Client.Instance.GetUoWindowPos();
        }




    }

}
                                                              
