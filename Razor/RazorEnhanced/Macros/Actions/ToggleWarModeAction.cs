using RazorEnhanced;

namespace RazorEnhanced.Macros.Actions
{
    public class ToggleWarModeAction : MacroAction
    {
        public bool WarMode { get; set; }

        public ToggleWarModeAction()
        {
            WarMode = true;
        }

        public ToggleWarModeAction(bool warMode)
        {
            WarMode = warMode;
        }

        public override string GetActionName() => "Toggle War Mode";

        public override void Execute()
        {
            Player.SetWarMode(WarMode);
        }

        public override string Serialize()
        {
            return $"ToggleWarMode|{WarMode}";
        }

        public override void Deserialize(string data)
        {
            var parts = data.Split('|');
            if (parts.Length >= 2)
            {
                bool.TryParse(parts[1], out bool warMode);
                WarMode = warMode;
            }
            else
            {
                WarMode = true; // Default to war mode on
            }
        }

        public override bool IsValid()
        {
            return true;
        }

        public override int GetDelay()
        {
            return 250; // Small delay for state change
        }
    }
}