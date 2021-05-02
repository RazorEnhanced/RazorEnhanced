//C#
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;

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
            // Method #1
            Items.Filter filter = new Items.Filter();
            filter.Enabled = false;
            List<Item> itemList = Items.ApplyFilter(filter);

            string path = "d:\\";
            Log("Saving into path: " + path);

            if (itemList.Count > 0)
            {
                Bitmap bitmap1 = itemList[0].Image;
                bitmap1.Save(path + "myImage_1.jpg", ImageFormat.Jpeg);
            }

            // Method #2 : 0x0EFA = spellbook, #1196 = yellowish
            Bitmap bitmap2 = Items.GetImage(0x0EFA, 1196);
            bitmap2.Save(path + "myImage_2.jpg", ImageFormat.Jpeg);

            Log("Finished saving 2 bitmaps.");
        }
    }
}