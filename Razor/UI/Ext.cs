using System;
using System.Windows.Forms;

namespace Assistant.UI
{
    static class Ext
    {
        public static void SafeAction<TControl>(this TControl control, Action<TControl> action) where TControl : Control
        {
            if (control.InvokeRequired)
            {
                control.Invoke(action, control);
            }
            else
                action(control);
        }
    }
}
