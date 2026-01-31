using Assistant;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RazorEnhanced.Macros.Actions
{
    public class TargetAction : MacroAction
    {
        public enum TargetMode
        {
            Serial,      // Target specific serial
            Type,        // Target by graphic/color (with selector)
            Self,        // Target self
            Alias,       // Target by alias name
            Location,    // Target coordinates (ground/tile)
            LastTarget   // Target last targeted entity (uses 'last' alias)
        }

        public TargetMode Mode { get; set; }

        // Serial mode
        public int Serial { get; set; }

        // Type mode
        public int Graphic { get; set; }
        public int Color { get; set; }
        public string Selector { get; set; }  // Nearest, Farthest, Random

        // Alias mode
        public string AliasName { get; set; }

        // Location mode
        public int X { get; set; }
        public int Y { get; set; }
        public int Z { get; set; }

        public TargetAction()
        {
            Mode = TargetMode.Self;
            Serial = 0;
            Graphic = 0;
            Color = -1;
            Selector = "Nearest";
            AliasName = "";
            X = 0;
            Y = 0;
            Z = 0;
        }

        public TargetAction(TargetMode mode, int serial = 0, int graphic = 0, int color = -1,
            string selector = "Nearest", string aliasName = "", int x = 0, int y = 0, int z = 0)
        {
            Mode = mode;
            Serial = serial;
            Graphic = graphic;
            Color = color;
            Selector = selector ?? "Nearest";
            AliasName = aliasName ?? "";
            X = x;
            Y = y;
            Z = z;
        }

        public override string GetActionName() => "Target";

        public override void Execute()
        {
            switch (Mode)
            {
                case TargetMode.Self:
                    Target.Self();
                    break;

                case TargetMode.LastTarget:
                    Target.Last();
                    break;

                case TargetMode.Serial:
                    if (Serial != 0)
                    {
                        Target.TargetExecute(Serial);
                    }
                    else
                    {
                        Misc.SendMessage("Target: Invalid serial (0x00000000)", 33);
                    }
                    break;

                case TargetMode.Type:
                    if (Graphic != 0)
                    {
                        // Search both items and mobiles
                        var candidates = new List<(int serial, double distance)>();

                        // Search items
                        var filter = new Items.Filter
                        {
                            Graphics = new List<int> { Graphic },
                            OnGround = -1,
                            RangeMax = -1
                        };

                        if (Color != -1)
                        {
                            filter.Hues = new List<int> { Color };
                        }

                        var items = Items.ApplyFilter(filter);
                        foreach (var item in items)
                        {
                            double dist = Utility.Distance(Player.Position.X, Player.Position.Y, item.Position.X, item.Position.Y);
                            candidates.Add((item.Serial, dist));
                        }

                        // Search mobiles
                        var mobileFilter = new Mobiles.Filter
                        {
                            Bodies = new List<int> { Graphic },
                            RangeMax = -1
                        };

                        if (Color != -1)
                        {
                            mobileFilter.Hues = new List<int> { Color };
                        }

                        var mobiles = Mobiles.ApplyFilter(mobileFilter);
                        foreach (var mobile in mobiles)
                        {
                            double dist = Utility.Distance(Player.Position.X, Player.Position.Y, mobile.Position.X, mobile.Position.Y);
                            candidates.Add((mobile.Serial, dist));
                        }

                        if (candidates.Count > 0)
                        {
                            int targetSerial = 0;

                            // Apply selector
                            switch (Selector)
                            {
                                case "Nearest":
                                    targetSerial = candidates.OrderBy(c => c.distance).First().serial;
                                    break;

                                case "Farthest":
                                    targetSerial = candidates.OrderByDescending(c => c.distance).First().serial;
                                    break;

                                case "Random":
                                    var random = new Random();
                                    targetSerial = candidates[random.Next(candidates.Count)].serial;
                                    break;

                                default:
                                    targetSerial = candidates[0].serial;
                                    break;
                            }

                            if (targetSerial != 0)
                            {
                                Target.TargetExecute(targetSerial);
                            }
                        }
                        else
                        {
                            string colorInfo = Color == -1 ? "" : $" (Color: {Color})";
                            Misc.SendMessage($"Target: No items/mobiles of type 0x{Graphic:X4}{colorInfo} found", 33);
                        }
                    }
                    else
                    {
                        Misc.SendMessage("Target: Invalid graphic (0x0000)", 33);
                    }
                    break;

                case TargetMode.Alias:
                    if (!string.IsNullOrEmpty(AliasName))
                    {
                        if (!Misc.CheckSharedValue(AliasName.ToLower()))
                        {
                            Misc.SendMessage($"Target: Alias '{AliasName}' does not exist", 33);
                            return;
                        }

                        int aliasSerial = (int)Misc.ReadSharedValue(AliasName.ToLower());

                        if (aliasSerial != 0 && aliasSerial != -1)
                        {
                            Target.TargetExecute(aliasSerial);
                        }
                        else
                        {
                            Misc.SendMessage($"Target: Alias '{AliasName}' has invalid serial (0x{aliasSerial:X8})", 33);
                        }
                    }
                    else
                    {
                        Misc.SendMessage("Target: Alias name is empty", 33);
                    }
                    break;

                case TargetMode.Location:
                    Target.TargetExecute(X, Y, Z);
                    break;
            }
        }

        public override string Serialize()
        {
            return $"Target|{(int)Mode}|{Serial}|{Graphic}|{Color}|{Escape(Selector)}|{Escape(AliasName)}|{X}|{Y}|{Z}";
        }

        public override void Deserialize(string data)
        {
            var parts = data.Split('|');

            // Format: Target|Mode|Serial|Graphic|Color|Selector|AliasName|X|Y|Z
            if (parts.Length >= 2 && int.TryParse(parts[1], out int modeInt))
                Mode = (TargetMode)modeInt;
            if (parts.Length >= 3 && int.TryParse(parts[2], out int serial))
                Serial = serial;
            if (parts.Length >= 4 && int.TryParse(parts[3], out int graphic))
                Graphic = graphic;
            if (parts.Length >= 5 && int.TryParse(parts[4], out int color))
                Color = color;
            if (parts.Length >= 6)
                Selector = Unescape(parts[5]);
            if (parts.Length >= 7)
                AliasName = Unescape(parts[6]);
            if (parts.Length >= 8 && int.TryParse(parts[7], out int x))
                X = x;
            if (parts.Length >= 9 && int.TryParse(parts[8], out int y))
                Y = y;
            if (parts.Length >= 10 && int.TryParse(parts[9], out int z))
                Z = z;
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
            switch (Mode)
            {
                case TargetMode.Self:
                case TargetMode.LastTarget:
                case TargetMode.Location:
                    return true;
                case TargetMode.Serial:
                    return Serial != 0;
                case TargetMode.Type:
                    return Graphic != 0;
                case TargetMode.Alias:
                    return !string.IsNullOrWhiteSpace(AliasName);
                default:
                    return false;
            }
        }

        public override int GetDelay()
        {
            return 150;
        }
    }
}