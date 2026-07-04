using System;

namespace RazorEnhanced
{
    internal static class LocationPicker
    {
        private const uint PICKER_GID = 9909998;
        private const int GRID_SPACING = 50;
        private const int GRID_COLS = 41;

        private static Action<int, int> _callback;

        public static void Pick(Action<int, int> onPicked)
        {
            if (Assistant.World.Player == null)
                return;

            _callback = onPicked;

            var gd = Gumps.CreateGump(false, true, true, false);
            gd.gumpId = PICKER_GID;
            gd.serial = (uint)Player.Serial;
            gd.action = OnGumpResponse;

            Gumps.AddPage(ref gd, 0);
            Gumps.AddImageTiled(ref gd, 0, 0, 3000, 3000, 2624);
            Gumps.AddAlphaRegion(ref gd, 0, 0, 3000, 3000);
            Gumps.AddHtml(ref gd, 320, 215, 350, 85,
                @"Select a button where you would like the gump to open.", true, true);
            Gumps.AddButton(ref gd, 700, 230, 241, 242, 0, 1, 0);
            Gumps.AddButton(ref gd, 700, 260, 247, 248, 0, 1, 0);

            int buttonID = 1;
            for (int y = 0; y <= 1100; y += GRID_SPACING)
            {
                for (int x = 0; x <= 2000; x += GRID_SPACING)
                {
                    Gumps.AddButton(ref gd, x, y, 1210, 1209, buttonID++, 1, 0);
                }
            }

            Gumps.SendGump(gd, 1, 1);
        }

        private static void OnGumpResponse(Gumps.GumpData gd)
        {
            int buttonID = gd.buttonid;
            if (buttonID > 0)
            {
                int x = ((buttonID % GRID_COLS) - 1) * GRID_SPACING;
                int y = (buttonID / GRID_COLS) * GRID_SPACING;
                _callback?.Invoke(x, y);
            }
            _callback = null;
        }
    }
}
