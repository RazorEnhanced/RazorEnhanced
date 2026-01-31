using Assistant;

namespace RazorEnhanced.Macros.Actions
{
    public class DisconnectAction : MacroAction
    {
        public override string GetActionName() => "Disconnect";

        public override void Execute()
        {
            Misc.Disconnect();
        }

        public override string Serialize() => "Disconnect";

        public override void Deserialize(string data) { }

        public override bool IsValid() => true;

        public override int GetDelay() => 0;
    }
}