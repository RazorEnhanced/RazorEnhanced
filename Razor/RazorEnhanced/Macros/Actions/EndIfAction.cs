using System;

namespace RazorEnhanced.Macros.Actions
{
    public class EndIfAction : MacroAction
    {
        public EndIfAction()
        {
        }

        public override string GetActionName() => "EndIf";

        public override void Execute()
        {
            // EndIf just marks the end of a conditional block
            // Execution logic is handled by the Macro executor
        }

        public override string Serialize()
        {
            return "EndIf";
        }

        public override void Deserialize(string data)
        {
            // No parameters
        }

        public override bool IsValid()
        {
            return true;
        }

        public override int GetDelay()
        {
            return 0;
        }
    }
}