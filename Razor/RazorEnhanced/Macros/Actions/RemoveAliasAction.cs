using System;

namespace RazorEnhanced.Macros.Actions
{
    public class RemoveAliasAction : MacroAction
    {
        public string AliasName { get; set; }

        public RemoveAliasAction()
        {
            AliasName = "myalias";
        }

        public RemoveAliasAction(string aliasName)
        {
            AliasName = aliasName ?? "myalias";
        }

        public override string GetActionName() => "Remove Alias";

        public override void Execute()
        {
            if (string.IsNullOrWhiteSpace(AliasName))
                return;

            // Remove the alias using SharedValue
            if (Misc.CheckSharedValue(AliasName.ToLower()))
            {
                Misc.RemoveSharedValue(AliasName.ToLower());
            }
        }

        public override string Serialize()
        {
            return $"RemoveAlias|{Escape(AliasName)}";
        }

        public override void Deserialize(string data)
        {
            var parts = data.Split('|');
            if (parts.Length >= 2)
            {
                AliasName = Unescape(parts[1]);
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
            return !string.IsNullOrWhiteSpace(AliasName);
        }

        public override int GetDelay()
        {
            return 0;
        }
    }
}