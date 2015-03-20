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
    public class Dress
    {
        [Serializable]
        public class DressItem
        {
            
        }
        internal static void AddLog(string addlog)
        {
            Engine.MainWindow.DressLogBox.Invoke(new Action(() => Engine.MainWindow.DressLogBox.Items.Add(addlog)));
            Engine.MainWindow.DressLogBox.Invoke(new Action(() => Engine.MainWindow.DressLogBox.SelectedIndex = Engine.MainWindow.DressLogBox.Items.Count - 1));
        }
    }
}
