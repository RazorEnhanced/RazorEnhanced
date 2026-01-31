using System;

namespace RazorEnhanced.Macros.Actions
{
    public class WaitForGumpAction : MacroAction
    {
        public int Timeout { get; set; }
        public uint GumpID { get; set; }

        public WaitForGumpAction()
        {
            Timeout = 5000; // 5 seconds default
            GumpID = 0;
        }

        public WaitForGumpAction(uint gumpId, int timeout)
        {
            GumpID = gumpId;
            Timeout = timeout;
        }

        public override string GetActionName() => "Wait for Gump";

        public override void Execute()
        {
            if (GumpID != 0)
            {
                Gumps.WaitForGump(GumpID, Timeout);
            }
            else
            {
                Gumps.WaitForGump(0, Timeout);
            }
        }

        public override string Serialize()
        {
            return $"WaitForGump|{GumpID}|{Timeout}";
        }

        public override void Deserialize(string data)
        {
            var parts = data.Split('|');
            if (parts.Length >= 3)
            {
                uint.TryParse(parts[1], out uint gumpId);
                int.TryParse(parts[2], out int timeout);
                GumpID = gumpId;
                Timeout = timeout;
            }
        }
    }
}