using System;
using System.Collections.Generic;
using System.Linq;

namespace RazorEnhanced.Macros.Actions
{
    public class GumpResponseAction : MacroAction
    {
        public uint GumpID { get; set; }
        public int ButtonID { get; set; }
        public List<int> Switches { get; set; }
        public List<int> TextIDs { get; set; }
        public List<string> TextEntries { get; set; }

        public GumpResponseAction()
        {
            Switches = new List<int>();
            TextIDs = new List<int>();
            TextEntries = new List<string>();
        }

        public GumpResponseAction(uint gumpId, int buttonId)
        {
            GumpID = gumpId;
            ButtonID = buttonId;
            Switches = new List<int>();
            TextIDs = new List<int>();
            TextEntries = new List<string>();
        }

        public GumpResponseAction(uint gumpId, int buttonId, List<int> switches, List<int> textIds, List<string> textEntries)
        {
            GumpID = gumpId;
            ButtonID = buttonId;
            Switches = switches ?? new List<int>();
            TextIDs = textIds ?? new List<int>();
            TextEntries = textEntries ?? new List<string>();
        }

        public override string GetActionName() => "Gump Response";

        public override void Execute()
        {
            // Check if this is a simple button press or advanced response
            if (Switches.Count == 0 && TextIDs.Count == 0 && TextEntries.Count == 0)
            {
                // Simple button press
                Gumps.SendAction(GumpID, ButtonID);
            }
            else
            {
                // Advanced response with switches and text entries
                // SendAdvancedAction expects List<int> and List<string>, not arrays!
                Gumps.SendAdvancedAction(
                    GumpID,
                    ButtonID,
                    Switches,      // List<int> not array
                    TextIDs,       // List<int> not array
                    TextEntries    // List<string> not array
                );
            }
        }

        public override int GetDelay() => 250; // Small delay for gump response

        public override string Serialize()
        {
            // Format: GumpResponse|GumpID|ButtonID|Switches|TextIDs|TextEntries
            string switchesStr = Switches.Count > 0 ? string.Join(",", Switches) : "";
            string textIdsStr = TextIDs.Count > 0 ? string.Join(",", TextIDs) : "";
            string textEntriesStr = TextEntries.Count > 0 ? string.Join("~", TextEntries.Select(t => t.Replace("|", "&#124;").Replace("~", "&#126;"))) : "";

            return $"GumpResponse|0x{GumpID:X8}|{ButtonID}|{switchesStr}|{textIdsStr}|{textEntriesStr}";
        }

        public override void Deserialize(string data)
        {
            var parts = data.Split('|');
            if (parts.Length >= 3)
            {
                string hexGumpId = parts[1].Replace("0x", "");
                GumpID = Convert.ToUInt32(hexGumpId, 16);
                int.TryParse(parts[2], out int buttonId);
                ButtonID = buttonId;

                // Parse switches
                if (parts.Length >= 4 && !string.IsNullOrEmpty(parts[3]))
                {
                    Switches = parts[3].Split(',').Select(s => int.Parse(s)).ToList();
                }
                else
                {
                    Switches = new List<int>();
                }

                // Parse text IDs
                if (parts.Length >= 5 && !string.IsNullOrEmpty(parts[4]))
                {
                    TextIDs = parts[4].Split(',').Select(s => int.Parse(s)).ToList();
                }
                else
                {
                    TextIDs = new List<int>();
                }

                // Parse text entries
                if (parts.Length >= 6 && !string.IsNullOrEmpty(parts[5]))
                {
                    TextEntries = parts[5].Split('~')
                        .Select(t => t.Replace("&#124;", "|").Replace("&#126;", "~"))
                        .ToList();
                }
                else
                {
                    TextEntries = new List<string>();
                }
            }
        }
    }
}