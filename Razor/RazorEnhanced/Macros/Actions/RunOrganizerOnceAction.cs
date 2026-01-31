using System;

namespace RazorEnhanced.Macros.Actions
{
    public class RunOrganizerOnceAction : MacroAction
    {
        public string OrganizerName { get; set; }
        public int SourceBag { get; set; }
        public int DestinationBag { get; set; }
        public int DragDelay { get; set; }

        public RunOrganizerOnceAction()
        {
            OrganizerName = "";
            SourceBag = -1;  // -1 = use organizer list default
            DestinationBag = -1;  // -1 = use organizer list default
            DragDelay = -1;  // -1 = use organizer list default
        }

        public RunOrganizerOnceAction(string organizerName, int sourceBag = -1, int destinationBag = -1, int dragDelay = -1)
        {
            OrganizerName = organizerName ?? "";
            SourceBag = sourceBag;
            DestinationBag = destinationBag;
            DragDelay = dragDelay;
        }

        public override string GetActionName() => "Run Organizer Once";

        public override void Execute()
        {
            if (string.IsNullOrWhiteSpace(OrganizerName))
            {
                Misc.SendMessage("Run Organizer Once: Organizer name is empty", 33);
                return;
            }

            // Check if organizer list exists
            if (!Settings.Organizer.ListExists(OrganizerName))
            {
                Misc.SendMessage($"Run Organizer Once: Organizer list '{OrganizerName}' not found", 33);
                return;
            }

            // Run organizer (this blocks until complete)
            Organizer.RunOnce(OrganizerName, SourceBag, DestinationBag, DragDelay);
        }

        public override string Serialize()
        {
            return $"RunOrganizerOnce|{Escape(OrganizerName)}|{SourceBag}|{DestinationBag}|{DragDelay}";
        }

        public override void Deserialize(string data)
        {
            var parts = data.Split('|');

            // Format: RunOrganizerOnce|OrganizerName|SourceBag|DestinationBag|DragDelay
            if (parts.Length >= 2)
            {
                OrganizerName = Unescape(parts[1]);
            }
            if (parts.Length >= 3 && int.TryParse(parts[2], out int sourceBag))
            {
                SourceBag = sourceBag;
            }
            if (parts.Length >= 4 && int.TryParse(parts[3], out int destBag))
            {
                DestinationBag = destBag;
            }
            if (parts.Length >= 5 && int.TryParse(parts[4], out int delay))
            {
                DragDelay = delay;
            }
        }

        // Add these helpers to the class:
        private static string Escape(string value)
        {
            if (value == null) return "";
            return value.Replace("\\", "\\\\").Replace("|", "\\|");
        }

        private static string Unescape(string value)
        {
            if (value == null) return "";
            return value.Replace("\\|", "|").Replace("\\\\", "\\");
        }

        public override bool IsValid()
        {
            return !string.IsNullOrWhiteSpace(OrganizerName);
        }

        public override int GetDelay()
        {
            return 0; // No additional delay - the organizer handles its own delays
        }
    }
}