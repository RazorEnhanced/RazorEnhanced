using System;
using System.IO;
using System.Collections.Generic;
using System.Windows.Forms;

namespace RazorEnhanced
{
    internal class AutoLoot
    {
        internal static void RefreshList(ListView AutolootlistView, List<RazorEnhanced.Items.AutoLootItem> AutoLootItemList)
        {
            AutolootlistView.Items.Clear();
            foreach (RazorEnhanced.Items.AutoLootItem item in AutoLootItemList)
            {
                ListViewItem listitem = new ListViewItem();
                listitem.SubItems.Add(item.Name);
                listitem.SubItems.Add("0x" + item.Graphics.ToString("X4"));
                listitem.SubItems.Add("0x" + item.Color.ToString("X4"));
                AutolootlistView.Items.Add(listitem);
            }
        }
        internal static void AddItemToList(string Name, int Graphycs, int Color, ListView AutolootlistView, List<RazorEnhanced.Items.AutoLootItem> AutoLootItemList)
        {
            List<RazorEnhanced.Items.AutoLootItem.Property> PropsList = new List<RazorEnhanced.Items.AutoLootItem.Property>();
            AutoLootItemList.Add(new RazorEnhanced.Items.AutoLootItem(Name, Graphycs, Color, PropsList));
            RazorEnhanced.AutoLoot.RefreshList(AutolootlistView, AutoLootItemList);
        }

    }

}