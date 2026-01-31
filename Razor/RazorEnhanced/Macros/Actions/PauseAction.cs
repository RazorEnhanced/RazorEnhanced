using System;

namespace RazorEnhanced.Macros.Actions
{
    public class PauseAction : MacroAction
    {
        public int Milliseconds { get; set; }

        public PauseAction()
        {
            Milliseconds = 1000; // Default 1 second
        }

        public PauseAction(int milliseconds)
        {
            Milliseconds = milliseconds;
        }

        public override string GetActionName() => "Pause";

        public override void Execute()
        {
            // Use RazorEnhanced Misc.Pause() for proper timing
            Misc.Pause(Milliseconds);
        }

        public override int GetDelay() => 0; // No additional delay - Misc.Pause() handles it

        public override string Serialize()
        {
            return $"Pause|{Milliseconds}";
        }

        public override void Deserialize(string data)
        {
            var parts = data.Split('|');
            if (parts.Length >= 2)
            {
                if (int.TryParse(parts[1], out int ms))
                {
                    Milliseconds = ms;
                }
                else
                {
                    Milliseconds = 1000;
                }
            }
        }

        public override bool IsValid()
        {
            return Milliseconds > 0;
        }

        public override string ToString()
        {
            return $"Pause {Milliseconds}ms";
        }
    }
}