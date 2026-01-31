using System;
using System.Collections.Generic;

namespace RazorEnhanced.Macros.Actions
{
    public class CastSpellAction : MacroAction
    {
        public int SpellID { get; set; }
        public string SpellName { get; set; } // Optional: for UI and serialization
        public string TargetSerialOrAlias { get; set; } // Can be serial (hex/dec) or alias

        public CastSpellAction() { }

        public CastSpellAction(int spellId, string spellName = null, string targetSerialOrAlias = null)
        {
            SpellID = spellId;
            SpellName = spellName ?? GetSpellNameByID(spellId);
            TargetSerialOrAlias = targetSerialOrAlias ?? "";
        }

        public override string GetActionName() => "Cast Spell";

        public override void Execute()
        {
            string spellName = !string.IsNullOrWhiteSpace(SpellName)
                ? SpellName
                : GetSpellNameByID(SpellID);

            if (string.IsNullOrWhiteSpace(spellName))
                return;

            uint targetSerial = 0;
            if (!string.IsNullOrWhiteSpace(TargetSerialOrAlias))
            {
                targetSerial = ResolveSerialOrAlias(TargetSerialOrAlias);
            }

            if (targetSerial != 0)
            {
                Spells.Cast(spellName, targetSerial);
            }
            else
            {
                Spells.Cast(spellName);
            }
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

        public override int GetDelay() => 1500; // Default spell casting delay

        public override string Serialize()
        {
            string spellName = !string.IsNullOrWhiteSpace(SpellName)
                ? SpellName
                : GetSpellNameByID(SpellID);

            return $"CastSpell|{SpellID}|{Escape(spellName)}|{Escape(TargetSerialOrAlias)}";
        }

        public override void Deserialize(string data)
        {
            var parts = data.Split('|');
            if (parts.Length >= 2 && int.TryParse(parts[1], out int id))
                SpellID = id;
            SpellName = parts.Length >= 3 ? Unescape(parts[2]) : GetSpellNameByID(SpellID);
            TargetSerialOrAlias = parts.Length >= 4 ? Unescape(parts[3]) : "";
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

        public override bool IsValid()
        {
            var allSpells = GetAllSpellsForUI();
            return allSpells.ContainsValue(SpellID);
        }

        public static Dictionary<string, int> GetAllSpellsForUI()
        {
            var spellsType = typeof(RazorEnhanced.Spells);
            var field = spellsType.GetField("m_AllSpells",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);

            if (field != null)
            {
                var allSpells = field.GetValue(null) as Dictionary<string, int>;
                if (allSpells != null)
                {
                    return new Dictionary<string, int>(allSpells);
                }
            }
            return new Dictionary<string, int>();
        }

        public static string GetSpellNameByID(int spellID)
        {
            var allSpells = GetAllSpellsForUI();
            foreach (var kvp in allSpells)
            {
                if (kvp.Value == spellID)
                    return kvp.Key;
            }
            return $"Spell {spellID}";
        }
    }
}