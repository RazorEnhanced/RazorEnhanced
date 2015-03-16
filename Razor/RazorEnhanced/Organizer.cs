using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using Assistant;
using System.Windows.Forms;
using System.Threading;
using System.Text.RegularExpressions;

namespace RazorEnhanced
{
  public class Organizer
	{
		[Serializable]
		public class OrganizerItem
		{
			
			private string m_Name;
			public string Name { get { return m_Name; } }

			private int m_Graphics;
			public int Graphics { get { return m_Graphics; } }

			private int m_Color;
			public int Color { get { return m_Color; } }

            private int m_amount;
            public int Amount { get { return m_amount; } }
			
            public OrganizerItem(string name, int graphics, int color, int amount)
			{
				m_Name = name;
				m_Graphics = graphics;
				m_Color = color;
                m_amount = amount;
			}
		}

        internal static int ItemDragDelay
        {
            get
            {
                return Assistant.Engine.MainWindow.OrganizerDragDelay;
            }
        }
        internal static void AddLog(string addlog)
        {
            Assistant.Engine.MainWindow.OrganizerLogBox.Invoke(new Action(() => Assistant.Engine.MainWindow.OrganizerLogBox.Items.Add(addlog)));
            Assistant.Engine.MainWindow.OrganizerLogBox.Invoke(new Action(() => Assistant.Engine.MainWindow.OrganizerLogBox.SelectedIndex = Assistant.Engine.MainWindow.OrganizerLogBox.Items.Count - 1));
        }
        internal static void RefreshList(List<OrganizerItem> OrganizerItemList)
        {
            Assistant.Engine.MainWindow.OrganizerListView.Items.Clear();
            foreach (OrganizerItem item in OrganizerItemList)
            {
                ListViewItem listitem = new ListViewItem();
                listitem.SubItems.Add(item.Name);
                listitem.SubItems.Add("0x" + item.Graphics.ToString("X4"));
                if (item.Color == -1)
                    listitem.SubItems.Add("All");
                else
                    listitem.SubItems.Add("0x" + item.Color.ToString("X4"));
                if (item.Amount == -1)
                    listitem.SubItems.Add("All");
                else
                    listitem.SubItems.Add(item.Amount.ToString());

                Assistant.Engine.MainWindow.OrganizerListView.Items.Add(listitem);
            }
        }
        internal static void ModifyItemToList(string name, int graphics, int color, int amount, ListView oranizerListView, List<OrganizerItem> organizerItemList, int indexToInsert)
        {
            organizerItemList.RemoveAt(indexToInsert);                                                       // rimuove
            organizerItemList.Insert(indexToInsert, new OrganizerItem(name, graphics, color, amount));     // inserisce al posto di prima
            RazorEnhanced.Settings.SaveOrganizerItemList(Assistant.Engine.MainWindow.OrganzierListSelect.SelectedItem.ToString(), organizerItemList, Assistant.Engine.MainWindow.OrganizerSourceLabel.Text, Assistant.Engine.MainWindow.OrganizerDestinationLabel.Text);
            RazorEnhanced.Organizer.RefreshList(organizerItemList);
        }
        internal static void AddItemToList(string name, int graphics, int color, int amount, ListView OrganizerlistView, List<OrganizerItem> OrganizerItemList)
        {
            OrganizerItemList.Add(new OrganizerItem(name, graphics, color, amount));
            RazorEnhanced.Settings.SaveOrganizerItemList(Assistant.Engine.MainWindow.OrganzierListSelect.SelectedItem.ToString(), OrganizerItemList, Assistant.Engine.MainWindow.OrganizerSourceLabel.Text, Assistant.Engine.MainWindow.OrganizerDestinationLabel.Text);
            RazorEnhanced.Organizer.RefreshList(OrganizerItemList);
        }
    }
}
