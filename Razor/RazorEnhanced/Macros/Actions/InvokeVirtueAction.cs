using System;

namespace RazorEnhanced.Macros.Actions
{
    public class InvokeVirtueAction : MacroAction
    {
        public string VirtueName { get; set; }

        public InvokeVirtueAction() { }

        public InvokeVirtueAction(string virtueName)
        {
            VirtueName = virtueName;
        }

        public override string GetActionName() => "Invoke Virtue";

        public override void Execute()
        {
            if (!string.IsNullOrEmpty(VirtueName))
            {
                Player.InvokeVirtue(VirtueName);
            }
        }

        public override string Serialize()
        {
            return $"InvokeVirtue|{VirtueName}";
        }

        public override void Deserialize(string data)
        {
            var parts = data.Split('|');
            if (parts.Length >= 2)
            {
                VirtueName = parts[1];
            }
        }

        public override bool IsValid()
        {
            return !string.IsNullOrEmpty(VirtueName);
        }
    }
}