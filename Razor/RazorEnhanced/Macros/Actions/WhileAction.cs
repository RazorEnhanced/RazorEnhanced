using Accord;
using System;
using System.Drawing;
using static RazorEnhanced.Macros.Actions.IfAction;

namespace RazorEnhanced.Macros.Actions
{
    public class WhileAction : MacroAction
    {
        public IfAction.ConditionType Type { get; set; }
        public IfAction.PlayerStatType StatType { get; set; }
        public IfAction.PlayerStatusType StatusType { get; set; }
        public IfAction.Operator Op { get; set; }
        public int Value { get; set; }
        public string ValueToken { get; set; }
        public bool BooleanValue { get; set; }
        public int Graphic { get; set; }
        public int Color { get; set; }
        public string SkillName { get; set; }
        public string BuffName { get; set; }
        public string PresetName { get; set; }
        public IfAction.InRangeMode RangeMode { get; set; }
        public int RangeSerial { get; set; }
        public int RangeGraphic { get; set; }
        public int RangeColor { get; set; }
        public IfAction.FindMode FindEntityMode { get; set; }
        public IfAction.FindLocation FindEntityLocation { get; set; }
        public int FindContainerSerial { get; set; }
        public int FindRange { get; set; }
        public bool FindStoreSerial { get; set; }

        public WhileAction() { }

        public WhileAction(
            IfAction.ConditionType type, IfAction.Operator op, int value, int graphic, int color,
            string skillName, string valueToken, bool booleanValue, string presetName, string buffName = "",
            IfAction.PlayerStatType statType = IfAction.PlayerStatType.HitPoints,
            IfAction.PlayerStatusType statusType = IfAction.PlayerStatusType.Poisoned,
            IfAction.InRangeMode rangeMode = IfAction.InRangeMode.LastTarget,
            int rangeSerial = 0, int rangeGraphic = 0, int rangeColor = -1,
            IfAction.FindMode findEntityMode = IfAction.FindMode.Item,
            IfAction.FindLocation findEntityLocation = IfAction.FindLocation.Backpack,
            int findContainerSerial = 0, int findRange = 2, bool findStoreSerial = false)
        {
            Type = type;
            StatType = statType;
            StatusType = statusType;
            Op = op;
            Value = value;
            Graphic = graphic;
            Color = color;
            SkillName = skillName ?? "";
            ValueToken = valueToken ?? "";
            BooleanValue = booleanValue;
            PresetName = presetName ?? "Custom";
            BuffName = buffName ?? "";
            RangeMode = rangeMode;
            RangeSerial = rangeSerial;
            RangeGraphic = rangeGraphic;
            RangeColor = rangeColor;
            FindEntityMode = findEntityMode;
            FindEntityLocation = findEntityLocation;
            FindContainerSerial = findContainerSerial;
            FindRange = findRange;
            FindStoreSerial = findStoreSerial;
        }

        public override string GetActionName() => "While";

        public override void Execute()
        {
            // No-op: loop logic handled by macro runner
        }

        public override int GetDelay() => 0;

        public override string Serialize()
        {
            return $"While|{Type}|{Op}|{Value}|{Graphic}|{Color}|{Escape(SkillName)}|{Escape(ValueToken)}|{BooleanValue}|{Escape(PresetName)}|{Escape(BuffName)}|{StatType}|{StatusType}|{RangeMode}|{RangeSerial}|{RangeGraphic}|{RangeColor}|{FindEntityMode}|{FindEntityLocation}|{FindContainerSerial}|{FindRange}|{FindStoreSerial}";
        }

        public override void Deserialize(string data)
        {
            var parts = data.Split('|');
            if (parts.Length >= 2 && Enum.TryParse(parts[1], out IfAction.ConditionType type))
                Type = type;
            if (parts.Length >= 3 && Enum.TryParse(parts[2], out IfAction.Operator op))
                Op = op;
            if (parts.Length >= 4 && int.TryParse(parts[3], out int value))
                Value = value;
            if (parts.Length >= 5 && int.TryParse(parts[4], out int graphic))
                Graphic = graphic;
            if (parts.Length >= 6 && int.TryParse(parts[5], out int color))
                Color = color;
            if (parts.Length >= 7)
                SkillName = Unescape(parts[6]);
            if (parts.Length >= 8)
                ValueToken = Unescape(parts[7]);
            if (parts.Length >= 9 && bool.TryParse(parts[8], out bool booleanValue))
                BooleanValue = booleanValue;
            if (parts.Length >= 10)
                PresetName = Unescape(parts[9]);
            if (parts.Length >= 11)
                BuffName = Unescape(parts[10]);
            if (parts.Length >= 12 && Enum.TryParse(parts[11], out IfAction.PlayerStatType statType))
                StatType = statType;
            if (parts.Length >= 13 && Enum.TryParse(parts[12], out IfAction.PlayerStatusType statusType))
                StatusType = statusType;
            if (parts.Length >= 14 && Enum.TryParse(parts[13], out IfAction.InRangeMode rangeMode))
                RangeMode = rangeMode;
            if (parts.Length >= 15 && int.TryParse(parts[14], out int rangeSerial))
                RangeSerial = rangeSerial;
            if (parts.Length >= 16 && int.TryParse(parts[15], out int rangeGraphic))
                RangeGraphic = rangeGraphic;
            if (parts.Length >= 17 && int.TryParse(parts[16], out int rangeColor))
                RangeColor = rangeColor;
            if (parts.Length >= 18 && Enum.TryParse(parts[17], out IfAction.FindMode findEntityMode))
                FindEntityMode = findEntityMode;
            if (parts.Length >= 19 && Enum.TryParse(parts[18], out IfAction.FindLocation findEntityLocation))
                FindEntityLocation = findEntityLocation;
            if (parts.Length >= 20 && int.TryParse(parts[19], out int findContainerSerial))
                FindContainerSerial = findContainerSerial;
            if (parts.Length >= 21 && int.TryParse(parts[20], out int findRange))
                FindRange = findRange;
            if (parts.Length >= 22 && bool.TryParse(parts[21], out bool findStoreSerial))
                FindStoreSerial = findStoreSerial;
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

        public override bool IsValid() => true;
    }
}