using Assistant;
using RazorEnhanced.Macros;
using System;

namespace RazorEnhanced.Macros.Actions
{
    public class UseContextMenuAction : MacroAction
    {
        public string TargetSerialOrAlias { get; set; }
        public int MenuIndex { get; set; } // -1 means not used
        public string MenuName { get; set; } // null or empty means not used

        public UseContextMenuAction() { }

        public UseContextMenuAction(string targetSerialOrAlias, int menuIndex)
        {
            TargetSerialOrAlias = targetSerialOrAlias ?? "";
            MenuIndex = menuIndex;
            MenuName = null;
        }

        public UseContextMenuAction(string targetSerialOrAlias, string menuName)
        {
            TargetSerialOrAlias = targetSerialOrAlias ?? "";
            MenuIndex = -1;
            MenuName = menuName;
        }

        public override string GetActionName()
        {
            return "Use Context Menu";
        }

        public override void Execute()
        {
            uint targetSerial = 0;
            if (!string.IsNullOrWhiteSpace(TargetSerialOrAlias))
            {
                targetSerial = ResolveSerialOrAlias(TargetSerialOrAlias);

                if (targetSerial == 0)
                {
                    Misc.SendMessage("Error no valid serial to target.");
                    return;
                }
            }
            else
            {
                Misc.SendMessage("Error no target specified for context menu.");
                return;
            }

            Misc.WaitForContext((int)targetSerial, 1000);

            if (!string.IsNullOrEmpty(MenuName))
                Misc.ContextReply((int)targetSerial, MenuName);
            else
                Misc.ContextReply((int)targetSerial, MenuIndex);
        }

        public override string Serialize()
        {
            // Format: targetSerialOrAlias|menuIndex|menuName
            return $"{Escape(TargetSerialOrAlias ?? "")}|{MenuIndex}|{Escape(MenuName ?? "")}";
        }

        public override void Deserialize(string data)
        {
            var parts = data.Split('|');
            if (parts.Length >= 3)
            {
                TargetSerialOrAlias = Unescape(parts[0]);
                int.TryParse(parts[1], out int menuIndex);
                MenuIndex = menuIndex;
                MenuName = Unescape(parts[2]);
            }
        }

        private static string Escape(string value)
        {
            if (value == null) return "";
            return value.Replace("\\", "\\\\").Replace("|", "\\|");
        }

        private static string Unescape(string value)
        {
            if (value == null) return "";
            return value.Replace("\\|", "|").Replace("\\\\", "\\");
        }

        private uint ResolveSerialOrAlias(string serialOrAlias)
        {
            if (string.IsNullOrWhiteSpace(serialOrAlias))
                return 0;

            // Try parse as hex or decimal serial
            if (serialOrAlias.StartsWith("0x", StringComparison.OrdinalIgnoreCase))
            {
                if (uint.TryParse(serialOrAlias.Substring(2), System.Globalization.NumberStyles.HexNumber, null, out uint serial))
                    return serial;
            }
            else if (uint.TryParse(serialOrAlias, out uint serial))
            {
                return serial;
            }

            // Otherwise, treat as alias (SharedValue)
            string aliasKey = serialOrAlias.ToLower();
            if (Misc.CheckSharedValue(aliasKey))
            {
                object aliasValue = Misc.ReadSharedValue(aliasKey);
                if (aliasValue is uint uintVal)
                {
                    return uintVal;
                }
                else if (uint.TryParse(aliasValue.ToString(), out uint parsedVal))
                {
                    return parsedVal;
                }
            }
            return 0;
        }

    }
}