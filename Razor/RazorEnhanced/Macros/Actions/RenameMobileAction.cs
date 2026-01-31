using System;

namespace RazorEnhanced.Macros.Actions
{
    public class RenameMobileAction : MacroAction
    {
        public int Serial { get; set; }
        public string Name { get; set; }

        public RenameMobileAction() { }

        public RenameMobileAction(int serial, string name)
        {
            Serial = serial;
            Name = name;
        }

        public override string GetActionName() => "Rename Mobile";

        public override void Execute()
        {
            if (Serial != 0 && !string.IsNullOrEmpty(Name))
            {
                Misc.PetRename(Serial, Name);
            }
        }

        public override int GetDelay() => 500;

        public override string Serialize()
        {
            return $"RenameMobile|0x{Serial:X8}|{Escape(Name)}";
        }

        public override void Deserialize(string data)
        {
            var parts = data.Split('|');
            if (parts.Length >= 3)
            {
                string hexSerial = parts[1].Replace("0x", "");
                Serial = Convert.ToInt32(hexSerial, 16);
                Name = Unescape(parts[2]);
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
            return Serial != 0 && !string.IsNullOrEmpty(Name);
        }
    }
}