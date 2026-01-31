using System;

namespace RazorEnhanced.Macros.Actions
{
    public class EndWhileAction : MacroAction
    {
        public override string GetActionName() => "EndWhile";
        public override void Execute() { }
        public override int GetDelay() => 0;
        public override string Serialize() => "EndWhile";
        public override void Deserialize(string data) { }
        public override bool IsValid() => true;
    }
}