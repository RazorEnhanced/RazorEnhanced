using RazorEnhanced;
using RazorEnhanced.Macros;
using System;

namespace RazorEnhanced.Macros.Actions
{
    public class ArmDisarmAction : MacroAction
    {
        public string Mode { get; set; } // "Arm" or "Disarm"
        public int ItemSerial { get; set; }
        public string Hand { get; set; }

        public ArmDisarmAction() { }
        public ArmDisarmAction(string mode, int itemSerial, string hand)
        {
            Mode = mode;
            ItemSerial = itemSerial;
            Hand = hand;
        }

        public override string GetActionName() => "Arm/Disarm";

        public override void Execute()
        {
            if (string.Equals(Mode, "Arm", StringComparison.OrdinalIgnoreCase))
            {
                Player.EquipItem(ItemSerial);
            }
            else if (string.Equals(Mode, "Disarm", StringComparison.OrdinalIgnoreCase))
            {
                if (string.Equals(Hand, "Both", StringComparison.OrdinalIgnoreCase))
                {
                    Player.UnEquipItemByLayer("LeftHand");
                    Player.UnEquipItemByLayer("RightHand");
                }
                else
                {
                    Player.UnEquipItemByLayer(Hand + "Hand");
                }
            }
        }

        public override string Serialize() => $"{Mode}|{ItemSerial}|{Hand}";
        public override void Deserialize(string data)
        {
            var parts = data.Split('|');
            if (parts.Length == 3)
            {
                Mode = parts[0];
                int.TryParse(parts[1], out int serial);
                ItemSerial = serial;
                Hand = parts[2];
            }
        }
    }
}