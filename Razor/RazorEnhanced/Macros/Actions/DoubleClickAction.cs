using Assistant;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RazorEnhanced.Macros.Actions
{
    public class DoubleClickAction : MacroAction
    {
        public enum DoubleClickMode
        {
            Serial,      // Double-click specific serial (item or mobile)
            Type,        // Double-click by graphic/color (with selector)
            Self,        // Double-click self
            Alias,       // Double-click by alias name
            LastTarget   // Double-click last target
        }

        public DoubleClickMode Mode { get; set; }

        // Serial mode
        public int Serial { get; set; }

        // Type mode
        public int Graphic { get; set; }
        public int Color { get; set; }
        public string Selector { get; set; }  // Nearest, Farthest, Random

        // Alias mode
        public string AliasName { get; set; }

        public DoubleClickAction()
        {
            Mode = DoubleClickMode.Serial;
            Serial = 0;
            Graphic = 0;
            Color = -1;
            Selector = "Nearest";
            AliasName = "";
        }

        public DoubleClickAction(DoubleClickMode mode, int serial = 0, int graphic = 0, int color = -1,
            string selector = "Nearest", string aliasName = "")
        {
            Mode = mode;
            Serial = serial;
            Graphic = graphic;
            Color = color;
            Selector = selector ?? "Nearest";
            AliasName = aliasName ?? "";
        }

        // Legacy constructor for backward compatibility with old macros
        public DoubleClickAction(uint serial) : this(DoubleClickMode.Serial, (int)serial)
        {
        }

        public override string GetActionName() => "Double-Click";

        public override void Execute()
        {
            switch (Mode)
            {
                case DoubleClickMode.Self:
                    if (World.Player != null)
                    {
                        Client.Instance.SendToServerWait(new DoubleClick(World.Player.Serial));
                    }
                    else
                    {
                        Misc.SendMessage("Double Click: Player not found", 33);
                    }
                    break;

                case DoubleClickMode.LastTarget:
                    // Get last target serial from shared values
                    if (Misc.CheckSharedValue("last"))
                    {
                        int lastSerial = (int)Misc.ReadSharedValue("last");
                        if (lastSerial != 0 && lastSerial != -1)
                        {
                            UseEntityBySerial(lastSerial);
                        }
                        else
                        {
                            Misc.SendMessage("Double Click: Last target is invalid", 33);
                        }
                    }
                    else
                    {
                        Misc.SendMessage("Double Click: No last target set", 33);
                    }
                    break;

                case DoubleClickMode.Serial:
                    if (Serial != 0)
                    {
                        UseEntityBySerial(Serial);
                    }
                    else
                    {
                        Misc.SendMessage("Double Click: Invalid serial (0x00000000)", 33);
                    }
                    break;

                case DoubleClickMode.Type:
                    if (Graphic != 0)
                    {
                        // Search both items and mobiles
                        var candidates = new List<(int serial, double distance)>();

                        // Search items
                        var itemFilter = new Items.Filter
                        {
                            Graphics = new List<int> { Graphic },
                            OnGround = -1,  // Search everywhere
                            RangeMax = -1
                        };

                        if (Color != -1)
                        {
                            itemFilter.Hues = new List<int> { Color };
                        }

                        var items = Items.ApplyFilter(itemFilter);
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
                                UseEntityBySerial(targetSerial);
                            }
                        }
                        else
                        {
                            string colorInfo = Color == -1 ? "" : $" (Color: {Color})";
                            Misc.SendMessage($"Double Click: No items/mobiles of type 0x{Graphic:X4}{colorInfo} found", 33);
                        }
                    }
                    else
                    {
                        Misc.SendMessage("Double Click: Invalid graphic (0x0000)", 33);
                    }
                    break;

                case DoubleClickMode.Alias:
                    if (!string.IsNullOrEmpty(AliasName))
                    {
                        if (!Misc.CheckSharedValue(AliasName.ToLower()))
                        {
                            Misc.SendMessage($"Double Click: Alias '{AliasName}' does not exist", 33);
                            return;
                        }

                        int aliasSerial = (int)Misc.ReadSharedValue(AliasName.ToLower());

                        if (aliasSerial != 0 && aliasSerial != -1)
                        {
                            UseEntityBySerial(aliasSerial);
                        }
                        else
                        {
                            Misc.SendMessage($"Double Click: Alias '{AliasName}' has invalid serial (0x{aliasSerial:X8})", 33);
                        }
                    }
                    else
                    {
                        Misc.SendMessage("Double Click: Alias name is empty", 33);
                    }
                    break;
            }
        }

        // Add this helper method to properly handle both items and mobiles
        private void UseEntityBySerial(int serial)
        {
            // Check if it's a mobile first
            var mobile = Mobiles.FindBySerial(serial);
            if (mobile != null)
            {
                // Use packet for mobiles
                Client.Instance.SendToServerWait(new DoubleClick(serial));
                return;
            }

            // Check if it's an item
            var item = Items.FindBySerial(serial);
            if (item != null)
            {
                // Use Items.UseItem for items (handles container logic)
                Items.UseItem(serial);
                return;
            }

            // Serial not found in world - try anyway (might be valid but not visible)
            Client.Instance.SendToServerWait(new DoubleClick(serial));
        }

        public override string Serialize()
        {
            return $"DoubleClick|{(int)Mode}|{Serial}|{Graphic}|{Color}|{Escape(Selector)}|{Escape(AliasName)}";
        }

        public override void Deserialize(string data)
        {
            var parts = data.Split('|');
            if (parts.Length >= 2 && int.TryParse(parts[1], out int modeInt))
                Mode = (DoubleClickMode)modeInt;
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
        }


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
                case DoubleClickMode.Self:
                case DoubleClickMode.LastTarget:
                    return true;
                case DoubleClickMode.Serial:
                    return Serial != 0;
                case DoubleClickMode.Type:
                    return Graphic != 0;
                case DoubleClickMode.Alias:
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