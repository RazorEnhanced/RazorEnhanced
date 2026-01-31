using Assistant;
using RazorEnhanced.Macros;
using System;
using static IronPython.Modules._ast;

namespace RazorEnhanced.Macros.Actions
{
    [Serializable]
    public class MovementAction : MacroAction
    {
        public enum MovementType
        {
            Walk,
            Run,
            Pathfind
        }

        public enum PathfindMode
        {
            Coordinates,
            Serial,
            Alias
        }

        public MovementType Type { get; set; }

        // For Walk/Run
        public string Direction { get; set; } // "North", "South", etc.

        // For Pathfind
        public PathfindMode Mode { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public int Z { get; set; }
        public int Serial { get; set; }
        public string AliasName { get; set; }



        public MovementAction()
        {
            Type = MovementType.Walk;
            Direction = "North";
            Mode = PathfindMode.Coordinates;
            X = 0;
            Y = 0;
            Z = 0;
            Serial = 0;
            AliasName = string.Empty;
        }

        // Walk/Run constructor
        public MovementAction(MovementType type, string direction)
        {
            Type = type;
            Direction = direction ?? "North";
            Mode = PathfindMode.Coordinates;
            X = 0;
            Y = 0;
            Z = 0;
            Serial = 0;
            AliasName = string.Empty;
        }

        public MovementAction(MovementType type, byte directionByte)
        {
            Type = type;
            Direction = ConvertDirectionToString(directionByte);
        }

        // Pathfind constructor
        public MovementAction(PathfindMode mode, int x, int y, int z, int serial, string aliasName)
        {
            Type = MovementType.Pathfind;
            Direction = string.Empty;
            Mode = mode;
            X = x;
            Y = y;
            Z = z;
            Serial = serial;
            AliasName = aliasName ?? string.Empty;
        }

        public override string GetActionName()
        {
            return "Movement";
        }

        public override string Serialize()
        {
            // Format: Movement|type|direction|mode|x|y|z|serial|aliasName
            return $"Movement|{(int)Type}|{Escape(Direction)}|{(int)Mode}|{X}|{Y}|{Z}|{Serial}|{Escape(AliasName)}";
        }

        public override void Deserialize(string data)
        {
            var parts = data.Split('|');
            if (parts.Length >= 2)
            {
                Type = (MovementType)int.Parse(parts[1]);
                Direction = parts.Length > 2 ? Unescape(parts[2]) : "North";
                Mode = parts.Length > 3 ? (PathfindMode)int.Parse(parts[3]) : PathfindMode.Coordinates;
                X = parts.Length > 4 ? int.TryParse(parts[4], out int x) ? x : 0 : 0;
                Y = parts.Length > 5 ? int.TryParse(parts[5], out int y) ? y : 0 : 0;
                Z = parts.Length > 6 ? int.TryParse(parts[6], out int z) ? z : 0 : 0;
                Serial = parts.Length > 7 ? int.TryParse(parts[7], out int serial) ? serial : 0 : 0;
                AliasName = parts.Length > 8 ? Unescape(parts[8]) : string.Empty;
            }
        }

        public override void Execute()
        {
            switch (Type)
            {
                case MovementType.Walk:
                    Player.Walk(Direction);
                    break;
                case MovementType.Run:
                    Player.Run(Direction);
                    break;
                case MovementType.Pathfind:
                    int targetX = 0;
                    int targetY = 0;
                    int targetZ = 0;

                    switch (Mode)
                    {
                        case PathfindMode.Coordinates:
                            targetX = X;
                            targetY = Y;
                            targetZ = Z;
                            break;

                        case PathfindMode.Serial:
                            if (Serial == 0)
                            {
                                Misc.SendMessage($"Pathfind: Invalid serial (0x00000000)", 33);
                                return;
                            }

                            // Try to find as mobile first
                            var mobile = Mobiles.FindBySerial(Serial);
                            if (mobile != null)
                            {
                                targetX = mobile.Position.X;
                                targetY = mobile.Position.Y;
                                targetZ = mobile.Position.Z;
                            }
                            else
                            {
                                // Try to find as item
                                var item = Items.FindBySerial(Serial);
                                if (item != null)
                                {
                                    targetX = item.Position.X;
                                    targetY = item.Position.Y;
                                    targetZ = item.Position.Z;
                                }
                                else
                                {
                                    Misc.SendMessage($"Pathfind: Entity 0x{Serial:X8} not found", 33);
                                    return;
                                }
                            }
                            break;

                        case PathfindMode.Alias:
                            if (string.IsNullOrWhiteSpace(AliasName))
                            {
                                Misc.SendMessage($"Pathfind: Alias name is empty", 33);
                                return;
                            }

                            // Try to get the serial from SharedValue
                            int aliasSerial = 0;
                            if (Misc.CheckSharedValue(AliasName.ToLower()))
                            {
                                object aliasValue = Misc.ReadSharedValue(AliasName.ToLower());
                                if (aliasValue is uint uintVal)
                                {
                                    aliasSerial = (int)uintVal;
                                }
                                else if (uint.TryParse(aliasValue.ToString(), out uint parsedVal))
                                {
                                    aliasSerial = (int)parsedVal;
                                }
                                else
                                {
                                    Misc.SendMessage($"Pathfind: Invalid alias value for '{AliasName}'", 33);
                                    return;
                                }
                            }
                            else
                            {
                                Misc.SendMessage($"Pathfind: Alias '{AliasName}' not found", 33);
                                return;
                            }

                            // Find the entity from alias serial
                            var aliasMobile = Mobiles.FindBySerial(aliasSerial);
                            if (aliasMobile != null)
                            {
                                targetX = aliasMobile.Position.X;
                                targetY = aliasMobile.Position.Y;
                                targetZ = aliasMobile.Position.Z;
                            }
                            else
                            {
                                var aliasItem = Items.FindBySerial(aliasSerial);
                                if (aliasItem != null)
                                {
                                    targetX = aliasItem.Position.X;
                                    targetY = aliasItem.Position.Y;
                                    targetZ = aliasItem.Position.Z;
                                }
                                else
                                {
                                    Misc.SendMessage($"Pathfind: Entity from alias '{AliasName}' (0x{aliasSerial:X8}) not found", 33);
                                    return;
                                }
                            }
                            break;
                    }

                    // Execute the pathfind
                    if (targetX == 0 && targetY == 0)
                    {
                        Misc.SendMessage($"Pathfind: Invalid coordinates (0, 0)", 33);
                        return;
                    }

                    PathFinding.PathFindTo(targetX, targetY, targetZ);
                    break;
            }
        }

        // Escaping helpers for serialization
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

        private static string ConvertDirectionToString(byte direction)
        {
            return direction switch
            {
                0 => "North",
                1 => "Northeast",
                2 => "East",
                3 => "Southeast",
                4 => "South",
                5 => "Southwest",
                6 => "West",
                7 => "Northwest",
                _ => "North"
            };
        }
    }
}