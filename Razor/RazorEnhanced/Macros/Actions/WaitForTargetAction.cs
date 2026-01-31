using System;

namespace RazorEnhanced.Macros.Actions
{
    public class WaitForTargetAction : MacroAction
    {
        public int Timeout { get; set; }

        public WaitForTargetAction()
        {
            Timeout = 5000; // 5 seconds default
        }

        public WaitForTargetAction(int timeout)
        {
            Timeout = timeout;
        }

        public override string GetActionName() => "Wait for Target";

        public override void Execute()
        {
            Target.WaitForTarget(Timeout);
        }

        public override string Serialize()
        {
            return $"WaitForTarget|{Timeout}";
        }

        public override void Deserialize(string data)
        {
            var parts = data.Split('|');
            if (parts.Length >= 2)
            {
                int.TryParse(parts[1], out int timeout);
                Timeout = timeout;
            }
        }
    }
}