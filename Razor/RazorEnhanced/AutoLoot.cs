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

    public class AutoLoot
    {
        private static int m_lootdelay;
        private static int m_maxrange;
        private static int m_autolootbag;
        private static bool m_noopencorpse;
        private static string m_autolootlist;
        private static Queue<int> m_IgnoreCorpseList = new Queue<int>();
        internal static volatile bool LockTable = false;

        public class AutoLootItem : ListAbleItem
        {
            public class Property
            {
                private string m_Name;
                [JsonProperty("Name")]
                public string Name { get { return m_Name; } }

                private int m_Minimum;
                [JsonProperty("Minimum")]
                public int Minimum { get { return m_Minimum; } }

                private int m_Maximum;
                [JsonProperty("Maximum")]
                public int Maximum { get { return m_Maximum; } }

                public Property(string name, int minimum, int maximum)
                {
                    m_Name = name;
                    m_Minimum = minimum;
                    m_Maximum = maximum;
                }
            }

            private string m_Name;
            public string Name { get { return m_Name; } }

            private int m_Graphics;
            public int Graphics { get { return m_Graphics; } }

            private int m_Color;
            public int Color { get { return m_Color; } }

            [JsonProperty("Selected")]
            internal bool Selected { get; set; }

            private List<Property> m_Properties;
            public List<Property> Properties { get { return m_Properties; } }

            public AutoLootItem(string name, int graphics, int color, bool selected, List<Property> properties)
            {
                m_Name = name;
                m_Graphics = graphics;
                m_Color = color;
                Selected = selected;
                m_Properties = properties;
            }
        }

        public class SerialToGrab
        {
            private int m_corpseserial;
            public int CorpseSerial { get { return m_corpseserial; } }

            private int m_itemserial;
            public int ItemSerial { get { return m_itemserial; } }

            public SerialToGrab(int itemserial, int corpseserial)
            {
                m_corpseserial = corpseserial;
                m_itemserial = itemserial;
            }
        }

        internal static ConcurrentQueue<SerialToGrab> SerialToGrabList = new ConcurrentQueue<SerialToGrab>();

        internal class AutoLootList
        {
            private string m_Description;
            internal string Description { get { return m_Description; } }

            private int m_Delay;
            internal int Delay { get { return m_Delay; } }

            private int m_Range;
            internal int Range { get { return m_Range; } }

            private int m_Bag;
            internal int Bag { get { return m_Bag; } }

            private bool m_Selected;
            [JsonProperty("Selected")]
            internal bool Selected { get { return m_Selected; } }

            private bool m_Noopencorpse;
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

            Assistant.Engine.MainWindow.AutoLootDataGridView.Rows.Clear();

            foreach (AutoLootList l in lists)
            {
                if (l.Selected)
                {
                    List<AutoLoot.AutoLootItem> items = Settings.AutoLoot.ItemsRead(l.Description);

                    foreach (AutoLootItem item in items)
                    {
                        string color = "All";
                        if (item.Color != -1)
                            color = "0x" + item.Color.ToString("X4");

                        string itemid = "All";
                        if (item.Graphics != -1)
                            itemid = "0x" + item.Graphics.ToString("X4");

                        Assistant.Engine.MainWindow.AutoLootDataGridView.Rows.Add(new object[] { item.Selected.ToString(), item.Name, itemid, color, item.Properties });
                    }

                    break;
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

                int color = 0;
                if ((string)row.Cells[3].Value == "All")
                    color = -1;
                else
                    color = Convert.ToInt32((string)row.Cells[3].Value, 16);

                bool.TryParse(row.Cells[0].Value.ToString(), out bool check);

                int itemID = 0;
                if ((string)row.Cells[2].Value == "All")
                    itemID = -1;
                else
                    itemID = Convert.ToInt32((string)row.Cells[2].Value, 16);

                if (row.Cells[4].Value != null)
                    Settings.AutoLoot.ItemInsert(Assistant.Engine.MainWindow.AutoLootListSelect.Text, new AutoLootItem((string)row.Cells[1].Value, itemID, color, check, (List<AutoLootItem.Property>)row.Cells[4].Value));
                else
                    Settings.AutoLoot.ItemInsert(Assistant.Engine.MainWindow.AutoLootListSelect.Text, new AutoLootItem((string)row.Cells[1].Value, itemID, color, check, new List<AutoLootItem.Property>()));
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

                int color = 0;
                if ((string)row.Cells[3].Value == "All")
                    color = -1;
                else
                    color = Convert.ToInt32((string)row.Cells[3].Value, 16);

                bool.TryParse(row.Cells[0].Value.ToString(), out bool check);

                if (row.Cells[4].Value != null)
                    Settings.AutoLoot.ItemInsert(newList, new AutoLootItem((string)row.Cells[1].Value, Convert.ToInt32((string)row.Cells[2].Value, 16), color, check, (List<AutoLootItem.Property>)row.Cells[4].Value));
                else
                    Settings.AutoLoot.ItemInsert(newList, new AutoLootItem((string)row.Cells[1].Value, Convert.ToInt32((string)row.Cells[2].Value, 16), color, check, new List<AutoLootItem.Property>()));
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
            Assistant.Engine.MainWindow.AutoLootDataGridView.Rows.Add(new object[] { "True", name, "0x" + graphics.ToString("X4"), "0x" + color.ToString("X4"), new List<AutoLootItem.Property>() });
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

                if (m_IgnoreCorpseList.Count > 100)
                    m_IgnoreCorpseList.Dequeue();
            }

        }
        internal static void Engine(List<AutoLootItem> autoLootList, int mseconds, Items.Filter filter)
        {
            List<Item> corpi = RazorEnhanced.Items.ApplyFilter(filter);

            if (World.Player != null && World.Player.IsGhost)
            {
                Thread.Sleep(2000);
                ResetIgnore();
                return;
            }

            foreach (RazorEnhanced.Item corpo in corpi)
            {
                RazorEnhanced.Item m_sharedcont = null;
                RazorEnhanced.Item m_OSIcont = null;

                RefreshCorpse(corpo);

                foreach (RazorEnhanced.Item oggettoContenuto in corpo.Contains)
                {
                    // Blocco shared
                    if (oggettoContenuto.ItemID == 0x0E75 && oggettoContenuto.Properties.Count > 0) // Attende l'arrivo delle props
                    {
                        if (oggettoContenuto.ItemID == 0x0E75 && oggettoContenuto.Properties[0].ToString() == "Instanced loot container")  // Rilevato backpack possibile shared loot verifico props UODREAMS
                        {
                            m_sharedcont = oggettoContenuto;
                            break;
                        }
                    }
                    if (oggettoContenuto.IsCorpse)  // Rilevato contenitore OSI
                    {
                        m_OSIcont = oggettoContenuto;
                        break;
                    }
                }

                RazorEnhanced.Item m_cont = null;

                if (m_sharedcont != null)
                    m_cont = m_sharedcont;
                else if (m_OSIcont != null)
                    m_cont = m_OSIcont;
                else
                    m_cont = corpo;

                foreach (AutoLootItem autoLootItem in autoLootList)
                {
                    if (!autoLootItem.Selected)
                        continue;

                    foreach (RazorEnhanced.Item oggettoContenuto in m_cont.Contains)
                    {
                        if (autoLootItem.Graphics == oggettoContenuto.ItemID || autoLootItem.Graphics == -1)  // match ALL id
                        {
                            if (autoLootItem.Color == oggettoContenuto.Hue || autoLootItem.Color == -1)
                            {
                                GrabItem(autoLootItem, oggettoContenuto, corpo.Serial);
                            }
                        }
                    }
                }
            }
        }

        internal static void GrabItem(AutoLootItem autoLoootItem, Item oggettoContenuto, int corpseserial)
        {
            foreach (SerialToGrab item in SerialToGrabList)
                if (item.ItemSerial == oggettoContenuto.Serial)
                    return;

            if (!oggettoContenuto.Movable || !oggettoContenuto.Visible)
                return;

            SerialToGrab data = new SerialToGrab(oggettoContenuto.Serial, corpseserial);

            if (autoLoootItem.Properties.Count > 0) // Item con props
            {
                Items.WaitForProps(oggettoContenuto, 1000);

                bool propsOk = false;
                foreach (AutoLootItem.Property props in autoLoootItem.Properties) // Scansione e verifica props
                {
                    int propsSuItemDaLootare = (int)RazorEnhanced.Items.GetPropValue(oggettoContenuto, props.Name);
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

        private static Items.Filter m_corpsefilter = new Items.Filter
        {
            Movable = -1,
            IsCorpse = 1,
            OnGround = 1,
            Enabled = true
        };

        internal static void AutoRun()
        {
            if (!Client.Running)
                return;

            if (World.Player == null)
                return;

            m_corpsefilter.RangeMax = m_maxrange;
            Engine(Settings.AutoLoot.ItemsRead(m_autolootlist), m_lootdelay, m_corpsefilter);
        }

        // Funzioni di controllo da script
        public static void ResetIgnore()
        {
            m_IgnoreCorpseList.Clear();
            AutoLoot.SerialToGrabList = new ConcurrentQueue<SerialToGrab>();
            DragDropManager.AutoLootSerialCorpseRefresh = new ConcurrentQueue<int>();
            Scavenger.ResetIgnore();
        }

        public static void RunOnce(string lootListName, int mseconds, Items.Filter filter)
        {
            if (Assistant.Engine.MainWindow.AutolootCheckBox.Checked == true)
            {
                Scripts.SendMessageScriptError("Script Error: Autoloot.Start: Autoloot already running");
                return;
            }
        List<AutoLootItem> autoLootList = Settings.AutoLoot.ItemsRead(lootListName);
        if (autoLootList.Count > 0)
            {
                Engine(autoLootList, mseconds, filter);
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

        static bool lootChangeMsgSent = false;
        public static uint GetLootBag()
        {
            // Check bag
            Assistant.Item bag = Assistant.World.FindItem(AutoLoot.AutoLootBag);
            if (bag != null)
            {
                if (bag.RootContainer != World.Player)
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

        public static void Start()
        {
            if (!Assistant.Client.Instance.AllowBit(FeatureBit.AutolootAgent))
            {
                Scripts.SendMessageScriptError("Autoloot Not Allowed!");
                return;
            }

            if (Assistant.Engine.MainWindow.AutolootCheckBox.Checked == true)
                Scripts.SendMessageScriptError("Script Error: Autoloot.Start: Autoloot already running");
            else
            {
                Assistant.Engine.MainWindow.SafeAction(s => s.AutolootCheckBox.Checked = true);
            }
        }

        public static void Stop()
        {
            if (Assistant.Engine.MainWindow.AutolootCheckBox.Checked == false)
                Scripts.SendMessageScriptError("Script Error: Autoloot.Stop: Autoloot already sleeping");
            else
                Assistant.Engine.MainWindow.SafeAction(s => s.AutolootCheckBox.Checked = false);
        }

        public static bool Status()
        {
            return Assistant.Engine.MainWindow.AutolootCheckBox.Checked;
        }

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
                    Assistant.Engine.MainWindow.SafeAction(s => s.AutoLootListSelect.SelectedIndex = s.AutoLootListSelect.Items.IndexOf(listName)); // Change list
                    Assistant.Engine.MainWindow.SafeAction(s => s.AutolootCheckBox.Checked = true);
                }
                else
                {
                    Assistant.Engine.MainWindow.SafeAction(s => s.AutoLootListSelect.SelectedIndex = s.AutoLootListSelect.Items.IndexOf(listName));  // Change List
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
        private static Assistant.Timer m_autostart = Assistant.Timer.DelayedCallback(TimeSpan.FromSeconds(3.0), new Assistant.TimerCallback(Start));

        internal static void LoginAutostart()
        {
            if (!Status())
            {
                m_autostart.Start();
            }
        }

    }
}
