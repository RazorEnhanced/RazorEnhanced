using System;

namespace RazorEnhanced.Macros.Actions
{
    public class ElseAction : MacroAction
    {
        public ElseAction()
        {
        }

        public override string GetActionName() => "Else";

        public override void Execute()
        {
            // The Else action itself doesn't execute - it's handled by the If/ElseIf logic
            // The MacroExecutor should skip to EndIf when an If or ElseIf succeeds
        }

        public override string Serialize()
        {
            return "else";
        }

        public override void Deserialize(string data)
        {
            // Else has no parameters
        }
    }
}