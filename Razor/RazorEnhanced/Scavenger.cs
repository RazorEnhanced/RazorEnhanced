using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assistant;
using System.Windows.Forms;
using System.Threading;
using System.Text.RegularExpressions;

namespace RazorEnhanced
{
    public class Scavenger
    {
        [Serializable]
        public class ScavengerItem
        {
            [Serializable]
            public class Property
            {
                private string m_Name;
                public string Name { get { return m_Name; } }

                private int m_Minimum;
                public int Minimum { get { return m_Minimum; } }

                private int m_Maximum;
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

            private List<Property> m_Properties;
            public List<Property> Properties { get { return m_Properties; } }

            public ScavengerItem(string name, int graphics, int color, List<Property> properties)
            {
                m_Name = name;
                m_Graphics = graphics;
                m_Color = color;
                m_Properties = properties;
            }
        }
        internal static RazorEnhanced.Item ScavengerBag
        {
            get
            {
                int serialBag = Convert.ToInt32(Assistant.Engine.MainWindow.ScavengerContainerLabel.Text, 16);

                if (serialBag == 0)
                {
                    serialBag = World.Player.Backpack.Serial;
                    return RazorEnhanced.Items.FindBySerial(World.Player.Backpack.Serial);
                }
                else
                {
                    Item bag = RazorEnhanced.Items.FindBySerial(serialBag);
                    if (bag.RootContainer != World.Player)
                        return RazorEnhanced.Items.FindBySerial(World.Player.Backpack.Serial);
                    else
                        return bag;
                }
            }
        }  
        internal static int ItemDragDelay
        {
            get
            {
                return Assistant.Engine.MainWindow.ScavengerDragDelay;
            }
        }

        internal static void AddLog(string addlog)
        {
            Assistant.Engine.MainWindow.ScavengerLogBox.Invoke(new Action(() => Assistant.Engine.MainWindow.ScavengerLogBox.Items.Add(addlog)));
            Assistant.Engine.MainWindow.ScavengerLogBox.Invoke(new Action(() => Assistant.Engine.MainWindow.ScavengerLogBox.SelectedIndex = Assistant.Engine.MainWindow.ScavengerLogBox.Items.Count - 1));
        }

        internal static void RefreshList(List<ScavengerItem> ScavengerItemList)
        {
            Assistant.Engine.MainWindow.ScavengerListView.Items.Clear();
            foreach (ScavengerItem item in ScavengerItemList)
            {
                ListViewItem listitem = new ListViewItem();
                listitem.SubItems.Add(item.Name);
                listitem.SubItems.Add("0x" + item.Graphics.ToString("X4"));
                if (item.Color == -1)
                    listitem.SubItems.Add("All");
                else
                    listitem.SubItems.Add("0x" + item.Color.ToString("X4"));
                Assistant.Engine.MainWindow.ScavengerListView.Items.Add(listitem);
            }
        }

        internal static void AddItemToList(string name, int graphics, int color, ListView scavengerListView, List<ScavengerItem> scavengerItemList)
        {
            List<ScavengerItem.Property> propsList = new List<ScavengerItem.Property>();
            scavengerItemList.Add(new ScavengerItem(name, graphics, color, propsList));
            RazorEnhanced.Settings.SaveScavengerItemList(Assistant.Engine.MainWindow.ScavengerListSelect.SelectedItem.ToString(), scavengerItemList);
            RazorEnhanced.Scavenger.RefreshList(scavengerItemList);
        }

        internal static void ModifyItemToList(string name, int graphics, int color, ListView scavengerListView, List<ScavengerItem> scavengerItemList, int indexToInsert)
        {
            List<ScavengerItem.Property> PropsList = scavengerItemList[indexToInsert].Properties;             // salva vecchie prop
            scavengerItemList.RemoveAt(indexToInsert);                                                       // rimuove
            scavengerItemList.Insert(indexToInsert, new ScavengerItem(name, graphics, color, PropsList));     // inserisce al posto di prima
            RazorEnhanced.Settings.SaveScavengerItemList(Assistant.Engine.MainWindow.ScavengerListSelect.SelectedItem.ToString(), scavengerItemList);
            RazorEnhanced.Scavenger.RefreshList(scavengerItemList);
        }

        internal static void RefreshPropListView(ListView scavengerListViewProp, List<ScavengerItem> scavengerItemList, int indexToInsert)
        {
            scavengerListViewProp.Items.Clear();
            List<ScavengerItem.Property> PropsList = scavengerItemList[indexToInsert].Properties;             // legge props correnti
            foreach (ScavengerItem.Property props in PropsList)
            {
                ListViewItem listitem = new ListViewItem();
                listitem.SubItems.Add(props.Name);
                listitem.SubItems.Add(props.Minimum.ToString());
                listitem.SubItems.Add(props.Maximum.ToString());
                scavengerListViewProp.Items.Add(listitem);
            }
            RazorEnhanced.Settings.SaveScavengerItemList(Assistant.Engine.MainWindow.ScavengerListSelect.SelectedItem.ToString(), scavengerItemList);
        }

        internal static void InsertPropToItem(string name, int graphics, int Color, ListView scavengerListViewProp, List<ScavengerItem> scavengerItemList, int indexToInsert, string propName, int propMin, int propMax)
        {
            scavengerListViewProp.Items.Clear();
            List<ScavengerItem.Property> PropsToAdd = new List<ScavengerItem.Property>();
            scavengerItemList[indexToInsert].Properties.Add(new ScavengerItem.Property(propName, propMin, propMax));
            RazorEnhanced.Settings.SaveScavengerItemList(Assistant.Engine.MainWindow.ScavengerListSelect.SelectedItem.ToString(), scavengerItemList);
        }

        private static bool m_Auto;
        internal static bool Auto
        {
            get { return m_Auto; }
            set { m_Auto = value; }
        }


        internal static int Engine(List<ScavengerItem> scavengerItemList, int mseconds, Items.Filter filter)
        {
            List<Item> itemsOnGround = RazorEnhanced.Items.ApplyFilter(filter);

            foreach (RazorEnhanced.Item itemGround in itemsOnGround)
            {
                if (World.Player.Weight - 20 > World.Player.MaxWeight)      // Controllo peso
                {
                    RazorEnhanced.Scavenger.AddLog("- Max weight reached, Wait untill free some space");
                    RazorEnhanced.Misc.SendMessage("SCAVENGER: Max weight reached, Wait untill free some space");
                    return -1;
                }
                foreach (ScavengerItem scavengerItem in scavengerItemList)
                {
                    if (scavengerItem.Color == -1)          // Colore ALL
                    {
                        if (itemGround.ItemID == scavengerItem.Graphics)
                        {
                            GrabItem(scavengerItem, itemGround, mseconds);
                        }
                    }
                    else
                    {
                        if (itemGround.ItemID == scavengerItem.Graphics && itemGround.Hue == scavengerItem.Color)
                        {
                            GrabItem(scavengerItem, itemGround, mseconds);
                        }
                    }
                }
                
            }

            return 0;
        }

        internal static void GrabItem(ScavengerItem scavengerItem, Item itemGround, int mseconds)
        {
            if (Utility.DistanceSqrt(new Assistant.Point2D(Assistant.World.Player.Position.X, Assistant.World.Player.Position.Y), new Assistant.Point2D(itemGround.Position.X, itemGround.Position.Y)) <= 3)
            {
                if (scavengerItem.Properties.Count > 0) // Item con props
                {
                    RazorEnhanced.Scavenger.AddLog("- Item Match found scan props");

                    bool propsOK = false;
                    foreach (ScavengerItem.Property props in scavengerItem.Properties) // Scansione e verifica props
                    {
                        int PropsSuItemDaLootare = RazorEnhanced.Items.GetPropByString(itemGround, props.Name);
                        if (PropsSuItemDaLootare >= props.Minimum && PropsSuItemDaLootare <= props.Maximum)
                        {
                            propsOK = true;
                        }
                        else
                        {
                            propsOK = false;
                            break; // alla prima fallita esce non ha senso controllare le altre
                        }
                    }

                    if (propsOK) // Tutte le props match OK
                    {
                        RazorEnhanced.Scavenger.AddLog("- Item Match found (0x" + itemGround.Serial.ToString("X8") + ") ... Grabbing");
                        RazorEnhanced.Items.Move(itemGround, RazorEnhanced.Scavenger.ScavengerBag, 0);
                        Thread.Sleep(mseconds);
                    }
                    else
                    {
                        RazorEnhanced.Scavenger.AddLog("- Props Match fail!");
                    }
                }
                else // Item Senza props     
                {
                    RazorEnhanced.Scavenger.AddLog("- Item Match found (0x" + itemGround.Serial.ToString("X8") + ") ... Grabbing");
                    RazorEnhanced.Items.Move(itemGround, RazorEnhanced.Scavenger.ScavengerBag, 0); 
                    Thread.Sleep(mseconds);
                }
            }
        }


        internal static void Engine()
        {
            int exit = Int32.MinValue;

            // Genero filtro item
            Items.Filter itemFilter = new Items.Filter();
            itemFilter.RangeMax = 2;
            itemFilter.Movable = true;
            itemFilter.OnGround = true;
            itemFilter.Enabled = true;

            exit = Engine(Assistant.Engine.MainWindow.ScavengerItemList, Assistant.Engine.MainWindow.ScavengerDragDelay, itemFilter);
        }

        // Funzioni da script
        public static int RunOnce(List<ScavengerItem> scavengerList, int mseconds, Items.Filter filter)
        {
            int exit = Int32.MinValue;

            if (Assistant.Engine.MainWindow.ScavengerCheckBox.Checked == true)
            {
                Misc.SendMessage("Script Error: Scavenger.Start: Scavenger already running");
            }
            else
            {
                exit = Engine(scavengerList, mseconds, filter);
            }

            return exit;
        }

        public static void Start()
        {
            if (Assistant.Engine.MainWindow.ScavengerCheckBox.Checked == true)
                Misc.SendMessage("Script Error: Scavenger.Start: Scavenger already running");
            else
                Assistant.Engine.MainWindow.ScavengerCheckBox.Invoke(new Action(() => Assistant.Engine.MainWindow.ScavengerCheckBox.Checked = true));
        }

        public static void Stop()
        {
            if (Assistant.Engine.MainWindow.ScavengerCheckBox.Checked == false)
                Misc.SendMessage("Script Error: Scavenger.Stop: Scavenger already sleeping");
            else
                Assistant.Engine.MainWindow.ScavengerCheckBox.Invoke(new Action(() => Assistant.Engine.MainWindow.ScavengerCheckBox.Checked = false));
        }

        public static bool Status()
        {
            return Assistant.Engine.MainWindow.ScavengerCheckBox.Checked;
        }

        public static void ChangeList(string nomelista)
        {
            bool ListaOK = false;
            for (int i = 0; i < Assistant.Engine.MainWindow.ScavengerListSelect.Items.Count; i++)
            {
                if (nomelista == Assistant.Engine.MainWindow.ScavengerListSelect.GetItemText(Assistant.Engine.MainWindow.ScavengerListSelect.Items[i]))
                    ListaOK = true;
            }
            if (!ListaOK)
                Misc.SendMessage("Script Error: Scavenger.ChangeList: Scavenger list: " + nomelista + " not exist");
            else
            {
                if (Assistant.Engine.MainWindow.ScavengerCheckBox.Checked == true) // Se è in esecuzione forza stop cambio lista e restart
                {
                    Assistant.Engine.MainWindow.ScavengerCheckBox.Invoke(new Action(() => Assistant.Engine.MainWindow.ScavengerCheckBox.Checked = false));
                    Assistant.Engine.MainWindow.ScavengerListSelect.Invoke(new Action(() => Assistant.Engine.MainWindow.ScavengerListSelect.SelectedIndex = Assistant.Engine.MainWindow.ScavengerListSelect.Items.IndexOf(nomelista)));  // cambio lista
                    Assistant.Engine.MainWindow.ScavengerCheckBox.Invoke(new Action(() => Assistant.Engine.MainWindow.ScavengerCheckBox.Checked = true));
                }
                else
                {
                    Assistant.Engine.MainWindow.ScavengerListSelect.Invoke(new Action(() => Assistant.Engine.MainWindow.ScavengerListSelect.SelectedIndex = Assistant.Engine.MainWindow.ScavengerListSelect.Items.IndexOf(nomelista)));  // cambio lista
                }
            }
        }
    }
}
