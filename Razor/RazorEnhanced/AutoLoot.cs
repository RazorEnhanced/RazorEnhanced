using Assistant;
using Assistant.UI;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace RazorEnhanced
{
    public class ListAbleItem
    {
        [JsonProperty("List")]
        public string List { get; set; }

    };


    /// <summary>
    /// The Autoloot class allow to interact with the Autoloot Agent, via scripting.
    /// </summary>
    public class AutoLoot
    {
        private static int m_lootdelay;
        private static int m_maxrange;
        private static int m_autolootbag;
        private static bool m_noopencorpse;
        private static string m_autolootlist;
        private static readonly Queue<int> m_IgnoreCorpseList = new Queue<int>();
        internal static volatile bool LockTable = false;

        public class AutoLootItem : ListAbleItem
        {
            public class Property
            {
                private readonly string m_Name;
                [JsonProperty("Name")]
                public string Name { get { return m_Name; } }

                private readonly int m_Minimum;
                [JsonProperty("Minimum")]
                public int Minimum { get { return m_Minimum; } }

                private readonly int m_Maximum;
                [JsonProperty("Maximum")]
                public int Maximum { get { return m_Maximum; } }

                public Property(string name, int minimum, int maximum)
                {
                    m_Name = name;
                    m_Minimum = minimum;
                    m_Maximum = maximum;
                }
            }

            private readonly string m_Name;
            public string Name { get { return m_Name; } }

            private readonly int m_Graphics;
            public int Graphics { get { return m_Graphics; } }

            private readonly int m_Color;
            public int Color { get { return m_Color; } }

            [JsonProperty("LootBagOverride")]
            public int LootBagOverride
            { get; set; }

            [JsonProperty("Selected")]
            public bool Selected { get; set; }

            private readonly List<Property> m_Properties;
            public List<Property> Properties { get { return m_Properties; } }

            public AutoLootItem(string name, int graphics, int color, bool selected, int lootBag, List<Property> properties)
            {
                m_Name = name;
                m_Graphics = graphics;
                m_Color = color;
                Selected = selected;
                LootBagOverride = lootBag;
                m_Properties = properties;
            }
        }

        public class SerialToGrab
        {
            private readonly int m_corpseserial;
            public int CorpseSerial { get { return m_corpseserial; } }

            private readonly int m_itemserial;
            public int ItemSerial { get { return m_itemserial; } }

            public int DestContainerOverride { get; set; }

            public SerialToGrab(int itemserial, int corpseserial, int destContainerOverride)
            {
                m_corpseserial = corpseserial;
                m_itemserial = itemserial;
                DestContainerOverride = destContainerOverride;
            }
        }

        internal static ConcurrentQueue<SerialToGrab> SerialToGrabList = new ConcurrentQueue<SerialToGrab>();

        internal class AutoLootList
        {
            private readonly string m_Description;
            internal string Description { get { return m_Description; } }

            private readonly int m_Delay;
            internal int Delay { get { return m_Delay; } }

            private readonly int m_Range;
            internal int Range { get { return m_Range; } }

            private readonly int m_Bag;
            internal int Bag { get { return m_Bag; } }

            private readonly bool m_Selected;
            [JsonProperty("Selected")]
            internal bool Selected { get { return m_Selected; } }

            private readonly bool m_Noopencorpse;
            internal bool NoOpenCorpse { get { return m_Noopencorpse; } }

            public AutoLootList(string description, int delay, int bag, bool selected, bool noopencorpse, int range)
            {
                m_Description = description;
                m_Delay = delay;
                m_Bag = bag;
                m_Selected = selected;
                m_Noopencorpse = noopencorpse;
                m_Range = range;
            }
        }

        private static bool m_AutoMode;

        internal static bool AutoMode
        {
            get { return m_AutoMode; }
            set { m_AutoMode = value; }
        }

        internal static string ListName
        {
            get { return m_autolootlist; }
            set { m_autolootlist = value; }
        }

        internal static int MaxRange
        {
            get { return m_maxrange; }

            set
            {
                m_maxrange = value;
                Assistant.Engine.MainWindow.SafeAction(s => s.AutoLootTextBoxMaxRange.Text = value.ToString());
            }
        }

        internal static int AutoLootDelay
        {
            get { return m_lootdelay; }

            set
            {
                m_lootdelay = value;
                Assistant.Engine.MainWindow.SafeAction(s => s.AutolootLabelDelay.Text = value.ToString());
            }
        }

        internal static int AutoLootBag
        {
            get { return m_autolootbag; }

            set
            {
                m_autolootbag = value;
                Assistant.Engine.MainWindow.SafeAction(s => s.AutoLootContainerLabel.Text = "0x" + value.ToString("X8"));

            }
        }

        internal static bool NoOpenCorpse
        {
            get { return m_noopencorpse; }

            set
            {
                m_noopencorpse = value;
                Assistant.Engine.MainWindow.SafeAction(s => s.AutoLootNoOpenCheckBox.Checked = value);
            }
        }

        internal static void AddLog(string addlog)
        {
            if (!Client.Running)
                return;

            Assistant.Engine.MainWindow.SafeAction(s => s.AutoLootLogBox.Items.Add(addlog));
            Assistant.Engine.MainWindow.SafeAction(s => s.AutoLootLogBox.SelectedIndex = s.AutoLootLogBox.Items.Count - 1);
            if (Assistant.Engine.MainWindow.AutoLootLogBox.Items.Count > 300)
                Assistant.Engine.MainWindow.SafeAction(s => s.AutoLootLogBox.Items.Clear());
        }

        internal static void RefreshLists()
        {
            List<AutoLootList> lists = Settings.AutoLoot.ListsRead();

            AutoLootList selectedList = lists.FirstOrDefault(l => l.Selected);
            if (selectedList != null && selectedList.Description == Assistant.Engine.MainWindow.AutoLootListSelect.Text)
                return;

            Assistant.Engine.MainWindow.AutoLootListSelect.Items.Clear();
            foreach (AutoLootList l in lists)
            {
                Assistant.Engine.MainWindow.AutoLootListSelect.Items.Add(l.Description);

                if (l.Selected)
                {
                    Assistant.Engine.MainWindow.AutoLootListSelect.SelectedIndex = Assistant.Engine.MainWindow.AutoLootListSelect.Items.IndexOf(l.Description);
                    AutoLootDelay = l.Delay;
                    AutoLootBag = l.Bag;
                    NoOpenCorpse = l.NoOpenCorpse;
                    MaxRange = l.Range;
                    ListName = l.Description;
                }
            }
        }

        internal static void InitGrid()
        {
            List<AutoLootList> lists = Settings.AutoLoot.ListsRead();

            foreach (AutoLootList l in lists)
            {
                if (l.Selected)
                {
                    InitGrid(l.Description);
                    break;
                }
            }
        }

        internal static void InitGrid(string listName)
        {
            Assistant.Engine.MainWindow.AutoLootDataGridView.Rows.Clear();
                    Dictionary<int, List<AutoLoot.AutoLootItem>> items = Settings.AutoLoot.ItemsRead(listName);
                    foreach (KeyValuePair<int, List<AutoLoot.AutoLootItem>> entry in items)
                    {
                        foreach (AutoLootItem item in entry.Value)
                        {
                            string color = "All";
                            if (item.Color != -1)
                                color = "0x" + item.Color.ToString("X4");

                            string itemid = "All";
                            if (item.Graphics != -1)
                                itemid = "0x" + item.Graphics.ToString("X4");
                            string lootBag = "0x" + item.LootBagOverride.ToString("X4");

                            Assistant.Engine.MainWindow.AutoLootDataGridView.Rows.Add(new object[] { item.Selected.ToString(), item.Name, itemid, color, lootBag, item.Properties });
                        }
                    }
        }

        internal static void CopyTable()
        {
            LockTable = true;

            Settings.AutoLoot.ClearList(Assistant.Engine.MainWindow.AutoLootListSelect.Text); // Rimuove vecchi dati dal save

            foreach (DataGridViewRow row in Assistant.Engine.MainWindow.AutoLootDataGridView.Rows)
            {
                if (row.IsNewRow)
                    continue;

                int color;
                if ((string)row.Cells[3].Value == "All")
                    color = -1;
                else
                    color = Convert.ToInt32((string)row.Cells[3].Value, 16);

                bool.TryParse(row.Cells[0].Value.ToString(), out bool check);

                int itemID;
                if ((string)row.Cells[2].Value == "All")
                    itemID = -1;
                else
                    itemID = Convert.ToInt32((string)row.Cells[2].Value, 16);
                int lootbagOverride = Convert.ToInt32((string)row.Cells[4].Value, 16);


                if (row.Cells[5].Value != null)
                    Settings.AutoLoot.ItemInsert(Assistant.Engine.MainWindow.AutoLootListSelect.Text, new AutoLootItem((string)row.Cells[1].Value, itemID, color, check, lootbagOverride, (List<AutoLootItem.Property>)row.Cells[5].Value));
                else
                    Settings.AutoLoot.ItemInsert(Assistant.Engine.MainWindow.AutoLootListSelect.Text, new AutoLootItem((string)row.Cells[1].Value, itemID, color, check, lootbagOverride, new List<AutoLootItem.Property>()));
            }

            Settings.Save(); // Salvo dati
            LockTable = false;

        }

        internal static void AddList(string newList)
        {
            RazorEnhanced.Settings.AutoLoot.ListInsert(newList, RazorEnhanced.AutoLoot.AutoLootDelay, (int)0, RazorEnhanced.AutoLoot.NoOpenCorpse, RazorEnhanced.AutoLoot.MaxRange);

            RazorEnhanced.AutoLoot.RefreshLists();
            RazorEnhanced.AutoLoot.InitGrid();
        }

        internal static void CloneList(string newList)
        {
            RazorEnhanced.Settings.AutoLoot.ListInsert(newList, RazorEnhanced.AutoLoot.AutoLootDelay, RazorEnhanced.AutoLoot.AutoLootBag, RazorEnhanced.AutoLoot.NoOpenCorpse, RazorEnhanced.AutoLoot.MaxRange);

            foreach (DataGridViewRow row in Assistant.Engine.MainWindow.AutoLootDataGridView.Rows)
            {
                if (row.IsNewRow)
                    continue;

                int color;
                if ((string)row.Cells[3].Value == "All")
                    color = -1;
                else
                    color = Convert.ToInt32((string)row.Cells[3].Value, 16);

                bool.TryParse(row.Cells[0].Value.ToString(), out bool check);

                if (row.Cells[4].Value != null)
                    Settings.AutoLoot.ItemInsert(newList, new AutoLootItem((string)row.Cells[1].Value, Convert.ToInt32((string)row.Cells[2].Value, 16), color, check, Convert.ToInt32((string)row.Cells[4].Value, 16), (List<AutoLootItem.Property>)row.Cells[5].Value));
                else
                    Settings.AutoLoot.ItemInsert(newList, new AutoLootItem((string)row.Cells[1].Value, Convert.ToInt32((string)row.Cells[2].Value, 16), color, check, Convert.ToInt32((string)row.Cells[4].Value, 16), new List<AutoLootItem.Property>()));
            }

            Settings.Save(); // Salvo dati

            RazorEnhanced.AutoLoot.RefreshLists();
            RazorEnhanced.AutoLoot.InitGrid();
        }

        internal static void RemoveList(string list)
        {
            if (RazorEnhanced.Settings.AutoLoot.ListExists(list))
            {
                RazorEnhanced.Settings.AutoLoot.ListDelete(list);
            }

            RazorEnhanced.AutoLoot.RefreshLists();
            RazorEnhanced.AutoLoot.InitGrid();
        }

        internal static void AddItemToList(string name, int graphics, int color)
        {
            Assistant.Engine.MainWindow.AutoLootDataGridView.Rows.Add(new object[] { "True", name, "0x" + graphics.ToString("X4"), "0x" + color.ToString("X4"), "0x" + 0.ToString("X4"), new List<AutoLootItem.Property>() });
            CopyTable();
        }

        private static void RefreshCorpse(Item corpo)
        {
            if (!m_noopencorpse)
            {
                if (!m_IgnoreCorpseList.Contains(corpo.Serial))
                {
                    DragDropManager.AutoLootSerialCorpseRefresh.Enqueue(corpo.Serial);
                    m_IgnoreCorpseList.Enqueue(corpo.Serial);
                }

                if (m_IgnoreCorpseList.Count > 200)
                    m_IgnoreCorpseList.Dequeue();
            }

        }


        internal static List<Item> ItemizeAllItems(List<Item> container)
        {
            List<Item> items = new List<Item>();
            if (container == null) // not valid serial or container not found
            {
                return items;
            }
            if (container.Count == 0)
            {
                return items;
            }
            foreach (Item i in container)
            {
                if (i.IsContainer)
                {
                    List<Item> recursItems = ItemizeAllItems(i.Contains);
                    if (recursItems != null)
                        items.AddRange(recursItems);
                }
                items.Add(i);
            }
            return items; // Return empty list no items found
        }


        internal static void Engine(Dictionary<int, List<AutoLootItem>> autoLootList, int mseconds, Items.Filter filter)
        {
            //TODO: msecond it's unused
            List<Item> corpi = RazorEnhanced.Items.ApplyFilter(filter);

            if (World.Player != null && World.Player.IsGhost)
            {
                Thread.Sleep(2000);
                ResetIgnore();
                return;
            }

            foreach (RazorEnhanced.Item corpse in corpi)
            {
                RazorEnhanced.Item m_sharedcont = null;
                RazorEnhanced.Item m_OSIcont = null;

                RefreshCorpse(corpse);

                foreach (RazorEnhanced.Item item in corpse.Contains)
                {
                    // Blocco shared
                    if (item.ItemID == 0x0E75 && item.Properties.Count > 0) // Attende l'arrivo delle props
                    {
                        if (item.ItemID == 0x0E75 && item.Properties[0].ToString() == "Instanced loot container")  // Rilevato backpack possibile shared loot verifico props UODREAMS
                        {
                            m_sharedcont = item;
                            break;
                        }
                    }
                    if (item.IsCorpse)  // Rilevato contenitore OSI
                    {
                        m_OSIcont = item;
                        break;
                    }
                }

                RazorEnhanced.Item m_cont = null;

                if (m_sharedcont != null)
                    m_cont = m_sharedcont;
                else if (m_OSIcont != null)
                    m_cont = m_OSIcont;
                else
                    m_cont = corpse;

                // If container is empty move on
                if (m_cont.Contains.Count() == 0)
                {
                    continue;
                }

                // get all the all-ID lists
                List<AutoLootItem> matchAll = null;
                if (!autoLootList.TryGetValue(-1, out matchAll))
                {
                    matchAll = new List<AutoLootItem>();
                }
                
                foreach (RazorEnhanced.Item item in ItemizeAllItems(m_cont.Contains))
                {
                    if (! item.IsLootable )
                    {
                        continue;
                    }
                    // Check match all by graphics
                    foreach (AutoLootItem autoLootItem in matchAll)
                    {
                        if (autoLootItem.Selected)
                        {
                            if (autoLootItem.Color == item.Hue || autoLootItem.Color == -1)
                            {
                                GrabItem(autoLootItem, item, corpse.Serial);
                            }
                        }
                    }
                    // check if in dictionary by graphics
                    List<AutoLootItem> autoLootItemList2 = null;
                    if (autoLootList.TryGetValue(item.ItemID, out autoLootItemList2))
                    {
                        foreach (AutoLootItem autoLootItem2 in autoLootItemList2)
                        {
                            if (autoLootItem2.Selected)
                            {
                                if (autoLootItem2.Color == item.Hue || autoLootItem2.Color == -1)
                                {
                                    GrabItem(autoLootItem2, item, corpse.Serial);
                                }
                            }
                        }
                    }
                }
            }
        }

        internal static void GrabItem(AutoLootItem autoLoootItem, Item grabItem, int corpseserial)
        {
            foreach (SerialToGrab item in SerialToGrabList)
                if (item.ItemSerial == grabItem.Serial)
                    return;

            if (!grabItem.Movable || !grabItem.Visible)
                return;

            SerialToGrab data = new SerialToGrab(grabItem.Serial, corpseserial, autoLoootItem.LootBagOverride);

            if (autoLoootItem.Properties.Count > 0) // Item con props
            {
                Items.WaitForProps(grabItem, 1000);

                bool propsOk = false;
                foreach (AutoLootItem.Property props in autoLoootItem.Properties) // Scansione e verifica props
                {
                    int propsSuItemDaLootare = (int)RazorEnhanced.Items.GetPropValue(grabItem, props.Name);
                    if (propsSuItemDaLootare >= props.Minimum && propsSuItemDaLootare <= props.Maximum)
                    {
                        propsOk = true;
                    }
                    else
                    {
                        propsOk = false;
                        break; // alla prima fallita esce non ha senso controllare le altre
                    }
                }

                if (propsOk) // Tutte le props match OK
                    SerialToGrabList.Enqueue(data);
            }
            else // Item Senza props
                SerialToGrabList.Enqueue(data);
        }

        private static readonly Items.Filter m_corpsefilter = new Items.Filter
        {
            Movable = -1,
            IsCorpse = 1,
            OnGround = 1,
            IsDoor = -1,
            Enabled = true
        };

        internal static void AutoRun()
        {
            if (!Client.Running)
                return;

            if (World.Player == null)
                return;
            try
            {
                m_corpsefilter.RangeMax = m_maxrange;
                Engine(Settings.AutoLoot.ItemsRead(m_autolootlist), m_lootdelay, m_corpsefilter);
            }
            catch (Exception)
            {
                //  If anything goes wrong just continue on
            }
        }

        // Funzioni di controllo da script

        /// <summary>
        /// Reset the Autoloot ignore list.
        /// </summary>
        public static void ResetIgnore()
        {
            m_IgnoreCorpseList.Clear();
            AutoLoot.SerialToGrabList = new ConcurrentQueue<SerialToGrab>();
            DragDropManager.AutoLootSerialCorpseRefresh = new ConcurrentQueue<int>();
            Scavenger.ResetIgnore();
        }

        /// <summary>
        /// Start Autoloot with custom parameters.
        /// </summary>
        /// <param name="lootListName">Name of the Autoloot listfilter for search on ground.</param>
        /// <param name="millisec">Delay in milliseconds. (unused)</param>
        /// <param name="filter">Item filter</param>
        public static void RunOnce(string lootListName, int millisec, Items.Filter filter)
        {
            if (Assistant.Engine.MainWindow.AutolootCheckBox.Checked == true)
            {
                Scripts.SendMessageScriptError("Script Error: Autoloot.Start: Autoloot already running");
                return;
            }
        Dictionary<int, List<AutoLootItem>> autoLootList = Settings.AutoLoot.ItemsRead(lootListName);
        if (autoLootList.Count > 0)
            {
                Engine(autoLootList, millisec, filter);
                uint lootbag = GetLootBag();
                // at login, backpack is sometimes null
                if (lootbag != 0)
                {
                    DragDropManager.ProcessLootList(lootbag);
                }
            } else {
                Scripts.SendMessageScriptError("Script Error: Autoloot.RunOnce: list specified is empty or doesn't exist");
            }
        }

        

        /// <summary>
        /// Toggle "No Open Corpse" on/off. The change doesn't persist if you reopen razor.
        /// </summary>
        /// <param name="noOpen">True: "No Open Corpse" is active - False: otherwise</param>
        /// <returns>Previous value of "No Open Corpse"</returns>
        public static bool SetNoOpenCorpse(bool noOpen)
        {
            // Note: This can be controlled by script but does not persist the change.
            // Only if the check box is manually changed is the change persisted
            bool oldValue = m_noopencorpse;
            m_noopencorpse = noOpen;
            Assistant.Engine.MainWindow.SafeAction(s => s.AutoLootNoOpenCheckBox.Checked = noOpen);
            return oldValue;
        }


        /// <summary>
        /// Given an AutoLoot list name, return a list of AutoLootItem associated.
        /// </summary>
        /// <param name="lootListName">Name of the AutoLoot list.</param>
        /// <returns></returns>
        public static List<AutoLootItem> GetList(string lootListName, bool wantMinusOnes=false)
        {
            if (Settings.AutoLoot.ListExists(lootListName)) {
                List<AutoLootItem> retList = new List<AutoLootItem>();
                var lootDict = Settings.AutoLoot.ItemsRead(lootListName);
                foreach (KeyValuePair<int, List<AutoLootItem>> entry in lootDict)
                {
                    foreach (AutoLootItem lootItem in entry.Value)
                    {
                        if (!wantMinusOnes && lootItem.Graphics == -1)
                            continue;
                        retList.Add(lootItem);
                    }

                }
                return retList;
            }
            Misc.SendMessage("Autoloot: Invalid Loot List Name", 945, true);
            return null;
        }

        /// <summary>
        /// @nodoc
        /// </summary>
        static bool lootChangeMsgSent = false;

        /// <summary>
        /// Get current Autoloot destination container.
        /// </summary>
        /// <returns>Serial of the container.</returns>
        public static uint GetLootBag()
        {
            // Check bag
            Assistant.Item bag = Assistant.World.FindItem(AutoLoot.AutoLootBag);
            if (bag != null)
            {
                if (! bag.IsLootableTarget)
                {
                    if (!lootChangeMsgSent)
                    {
                        Misc.SendMessage("Autoloot: Invalid Bag, Switch to backpack", 945, true);
                        AddLog("Invalid Bag, Switch to backpack");
                        lootChangeMsgSent = true;
                    }
                    if (World.Player == null || World.Player.Backpack == null || World.Player.Backpack.Serial == null)
                    {
                        return 0;
                    }
                    return World.Player.Backpack.Serial.Value;
                }
            }
            else
            {
                if (!lootChangeMsgSent)
                {
                    Misc.SendMessage("Autoloot: Invalid Bag, Switch to backpack", 945, true);
                    AddLog("Invalid Bag, Switch to backpack");
                    lootChangeMsgSent = true;
                }
                if (World.Player == null || World.Player.Backpack == null || World.Player.Backpack.Serial == null)
                {
                    return 0;
                }
                return World.Player.Backpack.Serial.Value;
            }
            lootChangeMsgSent = false;
            return bag.Serial.Value;
        }

        /// <summary>
        /// Start the Autoloot Agent on the currently active list.
        /// </summary>
        public static void Start()
        {
            if (Assistant.Engine.MainWindow.AutolootCheckBox.Checked == true)
                Scripts.SendMessageScriptError("Script Error: Autoloot.Start: Autoloot already running");
            else
            {
                Assistant.Engine.MainWindow.SafeAction(s => s.AutolootCheckBox.Checked = true);
            }
        }

        /// <summary>
        /// Stop the Autoloot Agent.
        /// </summary>
        public static void Stop()
        {
            if (Assistant.Engine.MainWindow.AutolootCheckBox.Checked == false)
                Scripts.SendMessageScriptError("Script Error: Autoloot.Stop: Autoloot already sleeping");
            else
                Assistant.Engine.MainWindow.SafeAction(s => s.AutolootCheckBox.Checked = false);
        }

        /// <summary>
        /// Check Autoloot Agent status
        /// </summary>
        /// <returns>True: if the Autoloot is running - False: otherwise</returns>
        public static bool Status()
        {
            return Assistant.Engine.MainWindow.AutolootCheckBox.Checked;
        }

        /// <summary>
        /// Change the Autoloot's active list.
        /// </summary>
        /// <param name="listName">Name of an existing organizer list.</param>

        public static void ChangeList(string listName)
        {
            if (!UpdateListParam(listName))
            {
                Scripts.SendMessageScriptError("Script Error: Autoloot.ChangeList: Autoloot list: " + listName + " not exist");
            }
            else
            {
                m_autolootlist = listName;
                if (Assistant.Engine.MainWindow.AutolootCheckBox.Checked == true) // If it is running force stop list change and restart
                {
                    Assistant.Engine.MainWindow.SafeAction(s => s.AutolootCheckBox.Checked = false);
                    Assistant.Engine.MainWindow.SafeAction(s => { s.AutoLootListSelect.SelectedIndex = s.AutoLootListSelect.Items.IndexOf(listName); InitGrid(listName); }); // Change list
                    Assistant.Engine.MainWindow.SafeAction(s => s.AutolootCheckBox.Checked = true);
                }
                else
                {
                    Assistant.Engine.MainWindow.SafeAction(s => { s.AutoLootListSelect.SelectedIndex = s.AutoLootListSelect.Items.IndexOf(listName); InitGrid(listName); });  // Change List
                }
            }
        }

        internal static bool UpdateListParam(string listName)
        {
            if (Settings.AutoLoot.ListExists(listName))
            {
                Settings.AutoLoot.ListDetailsRead(listName, out int bag, out int delay, out bool noopen, out int range);
                AutoLoot.AutoLootBag = bag;
                AutoLoot.AutoLootDelay = delay;
                AutoLoot.MaxRange = range;
                AutoLoot.ListName = listName;
                AutoLoot.NoOpenCorpse = noopen;
                return true;
            }
            return false;
        }

        // Autostart al login
        private static readonly Assistant.Timer m_autostart = Assistant.Timer.DelayedCallback(TimeSpan.FromSeconds(3.0), new Assistant.TimerCallback(Start));

        internal static void LoginAutostart()
        {
            if (!Status())
            {
                m_autostart.Start();
            }
        }

    }
}
