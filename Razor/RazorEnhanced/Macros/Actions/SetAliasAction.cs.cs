using System;

namespace RazorEnhanced.Macros.Actions
{
    public class SetAliasAction : MacroAction
    {
        public string AliasName { get; set; }
        public int Serial { get; set; }
        public bool UseFoundSerial { get; set; }

        public SetAliasAction()
        {
            AliasName = "myalias";
            Serial = 0;
            UseFoundSerial = false;
        }

        public SetAliasAction(string aliasName, int serial, bool useFoundSerial)
        {
            AliasName = aliasName ?? "myalias";
            Serial = serial;
            UseFoundSerial = useFoundSerial;
        }

        public override string GetActionName() => "Set Alias";

        public override void Execute()
        {
            if (string.IsNullOrWhiteSpace(AliasName))
                return;

            uint serialToSet;

            if (UseFoundSerial)
            {
                // Read from 'findfound' shared value
                if (Misc.CheckSharedValue("findfound"))
                {
                    object foundValue = Misc.ReadSharedValue("findfound");
                    if (foundValue is uint uintVal)
                    {
                        serialToSet = uintVal;
                    }
                    else if (uint.TryParse(foundValue.ToString(), out uint parsedVal))
                    {
                        serialToSet = parsedVal;
                    }
                    else
                    {
                        Misc.SendMessage($"Failed to read 'findfound' serial", 33);
                        return;
                    }
                }
                else
                {
                    Misc.SendMessage($"'findfound' alias not set", 33);
                    return;
                }
            }
            else
            {
                serialToSet = (uint)Serial;
            }

            // Set the alias using SharedValue (compatible with UOSteam)
            Misc.SetSharedValue(AliasName.ToLower(), serialToSet);

            // Also set 'found' to 'findfound' if the alias is 'found'
            if (AliasName.ToLower() == "found")
            {
                Misc.SetSharedValue("findfound", serialToSet);
            }
        }

        public override string Serialize()
        {
            return $"SetAlias|{Escape(AliasName)}|{Serial}|{UseFoundSerial}";
        }

        public override void Deserialize(string data)
        {
            var parts = data.Split('|');

            if (parts.Length >= 2)
            {
                AliasName = Unescape(parts[1]);
            }
            if (parts.Length >= 3 && int.TryParse(parts[2], out int serial))
            {
                Serial = serial;
            }
            if (parts.Length >= 4 && bool.TryParse(parts[3], out bool useFoundSerial))
            {
                UseFoundSerial = useFoundSerial;
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
            return !string.IsNullOrWhiteSpace(AliasName);
        }

        public override int GetDelay()
        {
            return 0;
        }
    }
}