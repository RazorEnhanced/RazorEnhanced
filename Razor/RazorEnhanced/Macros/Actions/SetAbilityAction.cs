using System;

namespace RazorEnhanced.Macros.Actions
{
    public class SetAbilityAction : MacroAction
    {
        public string Ability { get; set; } // "Primary", "Secondary", "Stun", "Disarm"

        public SetAbilityAction() { }

        public SetAbilityAction(string ability)
        {
            Ability = ability;
        }

        public override string GetActionName() => "Set Ability";

        public override void Execute()
        {
            switch (Ability?.ToLower())
            {
                case "primary":
                    Player.WeaponPrimarySA();
                    break;
                case "secondary":
                    Player.WeaponSecondarySA();
                    break;
                case "stun":
                    Player.WeaponStunSA();
                    break;
                case "disarm":
                    Player.WeaponDisarmSA();
                    break;
                case "clear":
                    Player.WeaponClearSA();
                    break;
            }
        }

        public override string Serialize()
        {
            return $"SetAbility|{Escape(Ability)}";
        }

        public override void Deserialize(string data)
        {
            var parts = data.Split('|');
            if (parts.Length >= 2)
            {
                Ability = Unescape(parts[1]);
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


    }
}