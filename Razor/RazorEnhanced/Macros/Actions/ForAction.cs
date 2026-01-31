using RazorEnhanced;

namespace RazorEnhanced.Macros.Actions
{
    public class ForAction : MacroAction
    {
        public int Iterations { get; set; }
        public int CurrentIteration { get; set; } // Track current loop iteration

        public ForAction()
        {
            Iterations = 1;
            CurrentIteration = 0;
        }

        public ForAction(int iterations)
        {
            Iterations = iterations > 0 ? iterations : 1;
            CurrentIteration = 0;
        }

        public override string GetActionName() => "For";

        public override void Execute()
        {
            // Increment current iteration counter
            CurrentIteration++;
        }

        public override string Serialize()
        {
            return $"For|{Iterations}|{CurrentIteration}";
        }

        public override void Deserialize(string data)
        {
            var parts = data.Split('|');
            if (parts.Length >= 2)
            {
                if (int.TryParse(parts[1], out int iterations))
                {
                    Iterations = iterations > 0 ? iterations : 1;
                }
                else
                {
                    Iterations = 1;
                }

                // Load current iteration if available
                if (parts.Length >= 3 && int.TryParse(parts[2], out int currentIteration))
                {
                    CurrentIteration = currentIteration;
                }
                else
                {
                    CurrentIteration = 0;
                }
            }
        }

        public override bool IsValid()
        {
            return Iterations > 0;
        }

        public override int GetDelay()
        {
            return 0; // No delay for loop control
        }

        // Reset the loop counter
        public void Reset()
        {
            CurrentIteration = 0;
        }

        // Check if loop should continue
        public bool ShouldContinue()
        {
            return CurrentIteration < Iterations;
        }
    }
}