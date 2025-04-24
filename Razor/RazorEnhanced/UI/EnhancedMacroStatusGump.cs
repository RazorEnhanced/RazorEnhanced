using Accord.Imaging;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Scripting.Hosting;
using RazorEnhanced;
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace RazorEnhanced.UI
{
    public class EnhancedMacroStatusGump
    {
        static int Width;
        static int Height;
        static Gumps.GumpData gd;
        static System.Timers.Timer _timer = new System.Timers.Timer(1000);
        static bool isRunning = false;

        static EnhancedMacroStatusGump()
        {
            _timer = new System.Timers.Timer(500);
            _timer.Elapsed += OnTimedEvent;
            _timer.AutoReset = true;
            _timer.Enabled = true;
            _timer.Start();
        }

        public static void SetSize(string width, string height)
        {
            Width = Convert.ToInt32(width);
            Height = Convert.ToInt32(height);

            UpdateGump();
        }

        private static void OnTimedEvent(object sender, ElapsedEventArgs e)
        {
            ResponseGump();
        }

        public static void ResponseGump()
        {
            Gumps.GumpData gd = Gumps.GetGumpData(0x0fe00000);
            if (Assistant.World.Player != null && gd != null)
            {
                int idx = gd.buttonid;
                if (idx < 0) return;
                
                var script = EnhancedScriptService.Instance.ScriptList()[idx];
                script.Stop();
            }
        }
        public static void UpdateGump()
        {
            if (Assistant.World.Player != null && isRunning)
            {
                gd = Gumps.CreateGump(true, false, false, false);
                gd.gumpId = 0x0fe00000;
                gd.serial = (uint)0x0fe00000;

                Gumps.AddPage(ref gd, 0);
                Gumps.AddHtml(ref gd, 0, 0, Width, Height, 0, false, false);
                Gumps.AddAlphaRegion(ref gd, 0, 0, Width, Height);
                //Gumps.AddLabel(ref gd, 0, 0, 255, "Macro Status");
                Gumps.AddHtml(ref gd, 0, 0, Width, Height- 40, $"<BASEFONT face=Arial color=#FFFE91>Macro List</BASEFONT>\n", false, false);
                int step = 20;

                for (int i = 0; i < EnhancedScriptService.Instance.ScriptList().Count; i++)
                {
                    var script = EnhancedScriptService.Instance.ScriptList()[i];
                    if (!script.IsRunning)
                        continue;

                    string html = $"<BASEFONT face=Arial color=#FFFFFF>{script.Filename}</BASEFONT>\n";

                    if (script.Preload)
                    {
                        html = $"<BASEFONT face=Arial color=#FFFFFF><I>{script.Filename}</I></BASEFONT>\n";
                    }
                    Gumps.AddHtml(ref gd, 10, step, Width - 30, Height - 40, html, false, false);
                    Gumps.AddButton(ref gd, Width - 15, step + 3, 2104, 2103, i, 1, 0);

                    step += 20;
                }
                Gumps.SendGump(gd, 0, 0);
            }
        }

        public static void ShowGump()
        {
            isRunning = true;
            UpdateGump();
        }

        public static void CloseGump()
        {
            if (Assistant.World.Player != null)
                Gumps.CloseGump(0x0fe00000);
        }
    }
}
