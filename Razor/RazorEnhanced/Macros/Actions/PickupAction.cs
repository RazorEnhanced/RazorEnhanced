using Assistant;
using System;

namespace RazorEnhanced.Macros.Actions
{
    public class PickUpAction : MacroAction
    {
        public int Serial { get; set; }
        public int Amount { get; set; }

        public PickUpAction() { }

        public PickUpAction(uint serial, ushort amount)
        {
            Serial = (int)serial;
            Amount = amount;
        }

        public PickUpAction(int serial, int amount)
        {
            Serial = serial;
            Amount = amount;
        }

        public override string GetActionName() => "Pick Up";

        public override void Execute()
        {
            if (Serial != 0)
            {
                var item = World.FindItem((int)Serial);
                if (item == null)
                {
                    Misc.SendMessage("Cannot find item to pick up.", 33);
                    return;
                }
                Assistant.DragDropManager.Drag(item, Amount);
                Misc.Pause(100); // Wait for item to move
            }
        }

        public override int GetDelay() => 650; // Default UO item movement delay

        public override string Serialize()
        {
            return $"PickUp|0x{Serial:X8}|{Amount}";
        }

        public override void Deserialize(string data)
        {
            var parts = data.Split('|');
            if (parts.Length >= 3)
            {
                string hexValue = parts[1].Replace("0x", "");
                Serial = Convert.ToInt32(hexValue, 16);
                int.TryParse(parts[2], out int amt);
                Amount = amt;
            }
        }

        public override bool IsValid()
        {
            return Serial != 0 && Items.FindBySerial(Serial) != null && Player.Backpack != null;
        }
    }
}