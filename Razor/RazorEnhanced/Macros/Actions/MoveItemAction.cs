using RazorEnhanced;
using System;

namespace RazorEnhanced.Macros.Actions
{
    public class MoveItemAction : MacroAction
    {
        public enum MoveTargetType
        {
            Entity, // Move to container or mobile
            Ground  // Move to ground coordinates
        }

        public MoveTargetType TargetType { get; set; }

        // Accepts either a serial (int) or an alias (string)
        public string ItemSerialOrAlias { get; set; }
        public int Amount { get; set; }
        public string TargetSerialOrAlias { get; set; } // For Entity
        public int X { get; set; } // Used for both container (optional) and ground
        public int Y { get; set; } // Used for both container (optional) and ground
        public int Z { get; set; } // Only for ground

        public MoveItemAction()
        {
            TargetType = MoveTargetType.Entity;
            ItemSerialOrAlias = "";
            Amount = 0;
            TargetSerialOrAlias = "";
            X = -1;
            Y = -1;
            Z = 0;
        }

        public MoveItemAction(MoveTargetType targetType, string itemSerialOrAlias, int amount, string targetSerialOrAlias, int x, int y, int z)
        {
            TargetType = targetType;
            ItemSerialOrAlias = itemSerialOrAlias;
            Amount = amount;
            TargetSerialOrAlias = targetSerialOrAlias;
            X = x;
            Y = y;
            Z = z;
        }

        public override string GetActionName() => "Move Item";

        public override void Execute()
        {
            int itemSerial = ResolveSerialOrAlias(ItemSerialOrAlias, "item");
            if (itemSerial == 0)
            {
                Misc.SendMessage($"MoveItemAction: Could not resolve item '{ItemSerialOrAlias}'", 33);
                return;
            }

            if (TargetType == MoveTargetType.Entity)
            {
                int targetSerial = ResolveSerialOrAlias(TargetSerialOrAlias, "target");
                if (targetSerial == 0)
                {
                    Misc.SendMessage($"MoveItemAction: Could not resolve target '{TargetSerialOrAlias}'", 33);
                    return;
                }

                if (X > 0 && Y > 0)
                    Items.Move(itemSerial, targetSerial, Amount, X, Y);
                else
                    Items.Move(itemSerial, targetSerial, Amount);
            }
            else // Ground
            {
                Items.MoveOnGround(itemSerial, Amount, X, Y, Z);
            }
        }

        // This matches the alias resolution logic from AttackAction
        private int ResolveSerialOrAlias(string serialOrAlias, string context)
        {
            if (string.IsNullOrWhiteSpace(serialOrAlias))
                return 0;

            // Try parse as hex or decimal serial
            if (serialOrAlias.StartsWith("0x", StringComparison.OrdinalIgnoreCase))
            {
                if (int.TryParse(serialOrAlias.Substring(2), System.Globalization.NumberStyles.HexNumber, null, out int serial))
                    return serial;
            }
            else if (int.TryParse(serialOrAlias, out int serial))
            {
                return serial;
            }

            // Otherwise, treat as alias and resolve via shared value (like AttackAction)
            string aliasKey = serialOrAlias.ToLower();
            if (Misc.CheckSharedValue(aliasKey))
            {
                object aliasValue = Misc.ReadSharedValue(aliasKey);
                if (aliasValue is uint uintVal)
                {
                    return (int)uintVal;
                }
                else if (uint.TryParse(aliasValue.ToString(), out uint parsedVal))
                {
                    return (int)parsedVal;
                }
                else
                {
                    Misc.SendMessage($"MoveItemAction: Invalid alias value for '{serialOrAlias}' ({context})", 33);
                    return 0;
                }
            }
            else
            {
                Misc.SendMessage($"MoveItemAction: Alias '{serialOrAlias}' not found ({context})", 33);
                return 0;
            }
        }

        public override int GetDelay() => 250; // Standard macro delay

        public override string Serialize()
        {
            return $"MoveItem|{TargetType}|{Escape(ItemSerialOrAlias)}|{Amount}|{Escape(TargetSerialOrAlias)}|{X}|{Y}|{Z}";
        }

        public override void Deserialize(string data)
        {
            var parts = data.Split('|');
            if (parts.Length < 8) return;

            Enum.TryParse(parts[1], out MoveTargetType targetType);
            TargetType = targetType;
            ItemSerialOrAlias = Unescape(parts[2]);
            int.TryParse(parts[3], out int amount);
            Amount = amount;
            TargetSerialOrAlias = Unescape(parts[4]);
            int.TryParse(parts[5], out int x);
            X = x;
            int.TryParse(parts[6], out int y);
            Y = y;
            int.TryParse(parts[7], out int z);
            Z = z;
        }

        private string Escape(string value)
        {
            return value?.Replace("|", "%7C") ?? "";
        }

        private string Unescape(string value)
        {
            return value?.Replace("%7C", "|") ?? "";
        }
    }
}