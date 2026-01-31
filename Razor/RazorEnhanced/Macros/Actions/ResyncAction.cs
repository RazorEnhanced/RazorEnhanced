using System;

namespace RazorEnhanced.Macros.Actions
{
    public class ResyncAction : MacroAction
    {
        public override string GetActionName() => "Resync";

        public override void Execute()
        {
            Misc.Resync();
        }

        public override string Serialize()
        {
            return "Resync";
        }

        public override void Deserialize(string data)
        {
            // No parameters
        }
    }
}