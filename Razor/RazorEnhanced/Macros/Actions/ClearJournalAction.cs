using System;

namespace RazorEnhanced.Macros.Actions
{
    public class ClearJournalAction : MacroAction
    {
        public override string GetActionName() => "Clear Journal";

        public override void Execute()
        {
            // Clear the global journal
            if (Journal.GlobalJournal != null)
            {
                Journal.GlobalJournal.Clear();
            }
        }

        public override int GetDelay() => 0; // No delay needed

        public override string Serialize()
        {
            return "ClearJournal";
        }

        public override void Deserialize(string data)
        {
            // No parameters
        }

        public override bool IsValid()
        {
            return true; // Always valid
        }
    }
}