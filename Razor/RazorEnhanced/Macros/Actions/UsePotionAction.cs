using System;
using System.Collections.Generic;

namespace RazorEnhanced.Macros.Actions
{
    public class UsePotionAction : MacroAction
    {
        public string PotionType { get; set; }

        private static readonly Dictionary<string, int> PotionGraphics = new Dictionary<string, int>
        {
            { "Heal", 0x0F0C },
            { "Cure", 0x0F07 },
            { "Refresh", 0x0F0B },
            { "Agility", 0x0F08 },
            { "Strength", 0x0F09 },
            { "Poison", 0x0F0A },
            { "Explosion", 0x0F0D }
        };

        public UsePotionAction() { }

        public UsePotionAction(string potionType)
        {
            PotionType = potionType;
        }

        public override string GetActionName() => "Use Potion";

        public override void Execute()
        {
            if (PotionGraphics.TryGetValue(PotionType, out int graphic))
            {
                Items.UseItemByID(graphic, -1);
            }
        }

        public override int GetDelay() => 350;

        public override string Serialize()
        {
            return $"UsePotion|{Escape(PotionType)}";
        }

        public override void Deserialize(string data)
        {
            var parts = data.Split('|');
            if (parts.Length >= 2)
            {
                PotionType = Unescape(parts[1]);
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

        public override bool IsValid()
        {
            return PotionGraphics.ContainsKey(PotionType);
        }
    }
}