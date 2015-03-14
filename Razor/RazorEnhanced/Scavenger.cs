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
                    serialBag = World.Player.Backpack.Serial;

                return RazorEnhanced.Items.FindBySerial(serialBag);
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
            Assistant.Engine.MainWindow.AutoLootListView.Items.Clear();
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
    }
}
