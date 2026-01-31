using RazorEnhanced;

namespace RazorEnhanced.Macros.Actions
{
    public class EndForAction : MacroAction
    {
        public EndForAction()
        {
        }

        public override string GetActionName() => "EndFor";

        public override void Execute()
        {
            // Execution logic is handled by Macro.cs
            // This action marks the end of a For loop
        }

        public override string Serialize()
        {
            return "EndFor";
        }

        public override void Deserialize(string data)
        {
            // No data to deserialize
        }

        public override bool IsValid()
        {
            return true;
        }

        public override int GetDelay()
        {
            return 0; // No delay for loop control
        }
    }
}