using Assistant;
using System;
using System.Collections.Generic;
using System.IO;

namespace RazorEnhanced.Macros.Actions
{
    [Serializable]
    public class TargetResourceAction : MacroAction
    {
        public int ToolType { get; set; }
        public int ToolColor { get; set; }
        public int ResourceNumber { get; set; }

        public static readonly Dictionary<string, int> ResourcePresets = new()
        {
            { "Ore", 0 },
            { "Sand", 1 },
            { "Wood", 2 },
            { "Graves", 3 },
            { "Red Mushrooms", 4 }
        };

        public TargetResourceAction()
        {
            ToolType = 0x0F39; // Default: shovel
            ToolColor = -1;
            ResourceNumber = 0; // Default: Ore
        }

        public TargetResourceAction(int toolType, int toolColor, int resourceNumber)
        {
            ToolType = toolType;
            ToolColor = toolColor;
            ResourceNumber = resourceNumber;
        }

        public override string GetActionName()
        {
            return "Target Resource";
        }

        public override void Execute()
        {
            // Find tool by type/color in backpack
            var tool = Items.FindByID(ToolType, ToolColor, Player.Backpack.Serial);
            if (tool == null)
            {
                tool = Player.GetItemOnLayer("RightHand");
                if (tool == null || tool.TypeID.Value != ToolType)
                {
                    tool = Player.GetItemOnLayer("LeftHand");

                    if (tool == null || tool.TypeID.Value != ToolType)
                    {
                        Misc.SendMessage($"No tool found in backpack or equipped (type 0x{ToolType:X4}, color {(ToolColor == -1 ? "Any" : $"0x{ToolColor:X4}")})", 33);
                        return;
                    }
                }
            }

            Target.TargetResource(tool.Serial, ResourceNumber);
        }

        public override int GetDelay() => 250;

        public override string Serialize()
        {
            // Format: TargetResource|tooltype|toolcolor|resourcenumber
            return $"TargetResource|{ToolType}|{ToolColor}|{ResourceNumber}";
        }

        public override void Deserialize(string data)
        {
            var parts = data.Split('|');
            if (parts.Length >= 4)
            {
                int.TryParse(parts[1], out int toolType);
                int.TryParse(parts[2], out int toolColor);
                int.TryParse(parts[3], out int resourceNumber);
                ToolType = toolType;
                ToolColor = toolColor;
                ResourceNumber = resourceNumber;
            }
        }

        public override bool IsValid()
        {
            var tool = Items.FindByID(ToolType, ToolColor, Player.Backpack.Serial);
            return tool != null;
        }
    }
}