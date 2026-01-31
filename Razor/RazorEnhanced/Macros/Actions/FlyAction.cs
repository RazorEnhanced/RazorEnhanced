using RazorEnhanced;

namespace RazorEnhanced.Macros.Actions
{
    public class FlyAction : MacroAction
    {
        public bool Flying { get; set; }

        public FlyAction()
        {
            Flying = true;
        }

        public FlyAction(bool flying)
        {
            Flying = flying;
        }

        public override string GetActionName() => "Fly";

        public override void Execute()
        {
            Player.Fly(Flying);
        }

        public override string Serialize()
        {
            return $"Fly|{Flying}";
        }

        public override void Deserialize(string data)
        {
            var parts = data.Split('|');
            if (parts.Length >= 2)
            {
                bool.TryParse(parts[1], out bool flying);
                Flying = flying;
            }
            else
            {
                Flying = true; // Default to flying on
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