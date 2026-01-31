using System;

namespace RazorEnhanced.Macros.Actions
{
    public class DropAction : MacroAction
    {
        public int Serial { get; set; }
        public int Container { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public int Z { get; set; }
        public int Amount { get; set; }

        public DropAction() { }

        public DropAction(uint serial, uint container, ushort x, ushort y, sbyte z, int amount = 0)
        {
            Serial = (int)serial;
            Container = unchecked((int)container); // Preserve 0xFFFFFFFF
            X = (short)x; // Cast to signed to handle 0xFFFF as -1
            Y = (short)y;
            Z = z;
            Amount = amount; // Now captures the actual amount being moved
        }

        public DropAction(int serial, int container, int x, int y, int z, int amount = 0)
        {
            Serial = serial;
            Container = container;
            X = x;
            Y = y;
            Z = z;
            Amount = amount;
        }

        public override string GetActionName() => "Drop";

        public override void Execute()
        {
            if (Serial == 0) return;

            var item = Items.FindBySerial(Serial);
            if (item == null)
            {
                Misc.SendMessage($"Item 0x{Serial:X8} not found", 33);
                return;
            }

            // Determine amount to move (0 means all)
            int amountToMove = (Amount <= 0) ? item.Amount : Math.Min(Amount, item.Amount);

            // Match ScriptRecorder pattern: if dest != 0xFFFFFFFF, it's a container drop
            if (Container != unchecked((int)0xFFFFFFFF))
            {
                // Dropping into a container
                if (X == -1 && Y == -1)
                {
                    // Auto-position in container
                    Items.Move(Serial, Container, amountToMove);
                }
                else
                {
                    // Specific position in container
                    Items.Move(Serial, Container, amountToMove, X, Y);
                }
            }
            else
            {
                // Dropping on ground (0xFFFFFFFF)
                Items.MoveOnGround(Serial, amountToMove, X, Y, Z);
                //Items.DropItemGroundSelf(Serial, amountToMove);
            }
        }


        public override int GetDelay() => 650; // Standard item movement delay

        public override string Serialize()
        {
            return $"Drop|0x{Serial:X8}|0x{Container:X8}|{X}|{Y}|{Z}|{Amount}";
        }

        public override void Deserialize(string data)
        {
            var parts = data.Split('|');
            if (parts.Length >= 6)
            {
                string hexSerial = parts[1].Replace("0x", "");
                string hexContainer = parts[2].Replace("0x", "");
                Serial = Convert.ToInt32(hexSerial, 16);
                Container = Convert.ToInt32(hexContainer, 16);
                int.TryParse(parts[3], out int x);
                int.TryParse(parts[4], out int y);
                int.TryParse(parts[5], out int z);
                X = x;
                Y = y;
                Z = z;

                // Amount is optional for backwards compatibility
                if (parts.Length >= 7)
                {
                    int.TryParse(parts[6], out int amt);
                    Amount = amt;
                }
            }
        }

        public override bool IsValid()
        {
            return Serial != 0;
        }
    }
}