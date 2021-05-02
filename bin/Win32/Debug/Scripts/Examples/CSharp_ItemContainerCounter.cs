//C#
using System;
using System.Collections.Generic;

namespace RazorEnhanced
{
    public class Script
    {
        private void Log(object messageString)
        {
            Misc.SendMessage(messageString, 201);
        }

        public void Run()
        {
            Items.Filter filter = new Items.Filter();
            filter.Enabled = false;
            List<Item> itemList = Items.ApplyFilter(filter);

            int containerCount = 0;
            foreach (Item item in itemList)
            {
                if (item.IsContainer == true)
                {
                    containerCount += 1;
                }
            }
            Log("There are " + containerCount + " containers near you!");
        }
    }
}