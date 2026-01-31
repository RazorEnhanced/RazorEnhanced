using Assistant;
using System;

namespace RazorEnhanced.Macros.Actions
{
    public class UseSkillAction : MacroAction
    {
        public string SkillName { get; set; }
        public string TargetSerialOrAlias { get; set; } // New property

        public UseSkillAction() { }

        public UseSkillAction(string skillName, string targetSerialOrAlias = "")
        {
            SkillName = skillName;
            TargetSerialOrAlias = targetSerialOrAlias ?? "";
        }

        public override string GetActionName() => "Use Skill";

        public override void Execute()
        {
            if (!string.IsNullOrWhiteSpace(TargetSerialOrAlias))
            {
                uint targetSerial = 0;
                targetSerial = ResolveSerialOrAlias(TargetSerialOrAlias);

                if (targetSerial != 0)
                {
                    Player.UseSkill(SkillName, (int)targetSerial);
                }
                else
                {
                    Misc.SendMessage("Error no valid serial to target.");
                    return;
                }
            }
            else
            {
                Player.UseSkill(SkillName);
            }
        }

        public override string Serialize()
        {
            return $"UseSkill|{Escape(SkillName)}|{Escape(TargetSerialOrAlias)}";
        }

        public override void Deserialize(string data)
        {
            var parts = data.Split('|');
            if (parts.Length >= 2)
            {
                SkillName = Unescape(parts[1]);
                TargetSerialOrAlias = parts.Length >= 3 ? Unescape(parts[2]) : "";
            }
        }

        // Add these helpers to the class:
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