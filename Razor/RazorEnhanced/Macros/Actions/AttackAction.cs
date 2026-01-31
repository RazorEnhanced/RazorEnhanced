using Assistant;
using AutoUpdaterDotNET;
using RazorEnhanced;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RazorEnhanced.Macros.Actions
{
    public class AttackAction : MacroAction
    {
        public enum AttackMode
        {
            LastTarget,
            Serial,
            Alias,
            Nearest,
            Farthest,
            ByType 
        }

        public enum NotorietyFilter
        {
            Any,           // No filter
            Blue,          // Innocent
            Green,         // Friend/Guild
            Grey,          // Neutral
            GreyAggro,     // Criminal
            Orange,        // Attacker
            Red,           // Murderer
            Yellow,        // Invulnerable
            Friendly,      // Blue + Green
            NonFriendly    // Orange + Red + Grey + GreyAggro
        }

        public AttackMode Mode { get; set; }
        public int Serial { get; set; }
        public string AliasName { get; set; }
        public NotorietyFilter Notoriety { get; set; }
        public int Range { get; set; }

        // NEW: Properties for ByType mode
        public int Graphic { get; set; }
        public int Color { get; set; }
        public string Selector { get; set; }

        public AttackAction()
        {
            Mode = AttackMode.LastTarget;
            Serial = 0;
            AliasName = "";
            Notoriety = NotorietyFilter.Any;
            Range = -1;
            Graphic = 0;
            Color = -1;
            Selector = "Nearest";
        }

        public AttackAction(AttackMode mode, int serial = 0, string aliasName = "",
            NotorietyFilter notoriety = NotorietyFilter.Any, int range = -1,
            int graphic = 0, int color = -1, string selector = "Nearest")
        {
            Mode = mode;
            Serial = serial;
            AliasName = aliasName ?? "";
            Notoriety = notoriety;
            Range = range;
            Graphic = graphic;
            Color = color;
            Selector = selector ?? "Nearest";
        }

        public override string GetActionName() => "Attack";

        public override void Execute()
        {
            int targetSerial = 0;

            switch (Mode)
            {
                case AttackMode.LastTarget:
                    Player.AttackLast();
                    return;

                case AttackMode.Serial:
                    if (Serial == 0)
                    {
                        Misc.SendMessage($"Attack: Invalid serial (0x00000000)", 33);
                        return;
                    }
                    targetSerial = Serial;
                    break;

                case AttackMode.Alias:
                    if (string.IsNullOrWhiteSpace(AliasName))
                    {
                        Misc.SendMessage($"Attack: Alias name is empty", 33);
                        return;
                    }

                    // Try to get the serial from SharedValue
                    if (Misc.CheckSharedValue(AliasName.ToLower()))
                    {
                        object aliasValue = Misc.ReadSharedValue(AliasName.ToLower());
                        if (aliasValue is uint uintVal)
                        {
                            targetSerial = (int)uintVal;
                        }
                        else if (uint.TryParse(aliasValue.ToString(), out uint parsedVal))
                        {
                            targetSerial = (int)parsedVal;
                        }
                        else
                        {
                            Misc.SendMessage($"Attack: Invalid alias value for '{AliasName}'", 33);
                            return;
                        }
                    }
                    else
                    {
                        Misc.SendMessage($"Attack Entity: Alias '{AliasName}' not found", 33);
                        return;
                    }
                    break;

                case AttackMode.Nearest:
                case AttackMode.Farthest:
                    {
                        // Get all mobiles in range
                        var filter = new Mobiles.Filter
                        {
                            RangeMax = Range > 0 ? Range : -1
                        };

                        var mobiles = Mobiles.ApplyFilter(filter);

                        if (mobiles.Count == 0)
                        {
                            Misc.SendMessage($"Attack Entity: No mobiles found", 33);
                            return;
                        }

                        // Filter by notoriety
                        var filteredMobiles = FilterByNotoriety(mobiles, Notoriety);

                        if (filteredMobiles.Count == 0)
                        {
                            Misc.SendMessage($"Attack Entity: No mobiles matching notoriety filter", 33);
                            return;
                        }

                        // Sort by distance and pick nearest or farthest
                        Mobile targetMobile = null;
                        if (Mode == AttackMode.Nearest)
                        {
                            // Find nearest
                            int minDistance = int.MaxValue;
                            foreach (var mobile in filteredMobiles)
                            {
                                int distance = Utility.Distance(World.Player.Position.X, World.Player.Position.Y,
                                    mobile.Position.X, mobile.Position.Y);
                                if (distance < minDistance)
                                {
                                    minDistance = distance;
                                    targetMobile = mobile;
                                }
                            }
                        }
                        else // Farthest
                        {
                            // Find farthest
                            int maxDistance = -1;
                            foreach (var mobile in filteredMobiles)
                            {
                                int distance = Utility.Distance(World.Player.Position.X, World.Player.Position.Y,
                                    mobile.Position.X, mobile.Position.Y);
                                if (distance > maxDistance)
                                {
                                    maxDistance = distance;
                                    targetMobile = mobile;
                                }
                            }
                        }

                        if (targetMobile != null)
                        {
                            targetSerial = targetMobile.Serial;
                        }
                        else
                        {
                            Misc.SendMessage($"Attack Entity: Could not find target mobile", 33);
                            return;
                        }
                    }
                    break;

                case AttackMode.ByType:
                    {
                        // Get all mobiles in range
                        var filter = new Mobiles.Filter
                        {
                            RangeMax = Range > 0 ? Range : -1,
                            Bodies = new List<int> { Graphic }
                        };

                        // Add color filter if not -1 (any color)
                        if (Color != -1)
                        {
                            filter.Hues = new List<int> { Color };
                        }

                        var mobiles = Mobiles.ApplyFilter(filter);

                        if (mobiles.Count == 0)
                        {
                            Misc.SendMessage($"Attack Entity: No mobiles found matching type 0x{Graphic:X4}", 33);
                            return;
                        }

                        Mobile targetMobile = null;

                        switch (Selector)
                        {
                            case "Nearest":
                                // Find nearest
                                int minDistance = int.MaxValue;
                                foreach (var mobile in mobiles)
                                {
                                    // Skip self
                                    if (mobile.Serial == World.Player.Serial)
                                        continue;

                                    int distance = Utility.Distance(World.Player.Position.X, World.Player.Position.Y,
                                        mobile.Position.X, mobile.Position.Y);
                                    if (distance < minDistance)
                                    {
                                        minDistance = distance;
                                        targetMobile = mobile;
                                    }
                                }
                                break;

                            case "Farthest":
                                // Find farthest
                                int maxDistance = -1;
                                foreach (var mobile in mobiles)
                                {
                                    // Skip self
                                    if (mobile.Serial == World.Player.Serial)
                                        continue;

                                    int distance = Utility.Distance(World.Player.Position.X, World.Player.Position.Y,
                                        mobile.Position.X, mobile.Position.Y);
                                    if (distance > maxDistance)
                                    {
                                        maxDistance = distance;
                                        targetMobile = mobile;
                                    }
                                }
                                break;

                            case "Random":
                                // Pick random from list (excluding self)
                                var validMobiles = mobiles.Where(m => m.Serial != World.Player.Serial).ToList();
                                if (validMobiles.Count > 0)
                                {
                                    var random = new Random();
                                    targetMobile = validMobiles[random.Next(validMobiles.Count)];
                                }
                                break;
                        }

                        if (targetMobile != null)
                        {
                            targetSerial = targetMobile.Serial;
                        }
                        else
                        {
                            Misc.SendMessage($"Attack Entity: Could not find target mobile", 33);
                            return;
                        }
                    }
                    break;
            }

            // Attack the target serial
            if (targetSerial != 0)
            {
                var mobile = Mobiles.FindBySerial(targetSerial);
                if (mobile != null)
                {
                    Player.Attack(targetSerial);
                }
                else
                {
                    Misc.SendMessage($"Attack Entity: Mobile 0x{targetSerial:X8} not found", 33);
                }
            }
        }

        private List<Mobile> FilterByNotoriety(List<Mobile> mobiles, NotorietyFilter filter)
        {
            if (filter == NotorietyFilter.Any)
                return mobiles;

            var filtered = new List<Mobile>();

            foreach (var mobile in mobiles)
            {
                // Skip self
                if (mobile.Serial == World.Player.Serial)
                    continue;

                bool match = false;

                switch (filter)
                {
                    case NotorietyFilter.Blue:
                        match = mobile.Notoriety == 1; // Innocent
                        break;

                    case NotorietyFilter.Green:
                        match = mobile.Notoriety == 2; // Friend/Guild
                        break;

                    case NotorietyFilter.Grey:
                        match = mobile.Notoriety == 3; // Neutral
                        break;

                    case NotorietyFilter.GreyAggro:
                        match = mobile.Notoriety == 4; // Criminal
                        break;

                    case NotorietyFilter.Orange:
                        match = mobile.Notoriety == 5; // Attacker
                        break;

                    case NotorietyFilter.Red:
                        match = mobile.Notoriety == 6; // Murderer
                        break;

                    case NotorietyFilter.Yellow:
                        match = mobile.Notoriety == 7; // Invulnerable
                        break;

                    case NotorietyFilter.Friendly:
                        match = mobile.Notoriety == 1 || mobile.Notoriety == 2; // Blue or Green
                        break;

                    case NotorietyFilter.NonFriendly:
                        match = mobile.Notoriety == 3 || mobile.Notoriety == 4 ||
                               mobile.Notoriety == 5 || mobile.Notoriety == 6; // Grey, GreyAggro, Orange, Red
                        break;
                }

                if (match)
                    filtered.Add(mobile);
            }

            return filtered;
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

        public override string Serialize()
        {
            return $"AttackEntity|{Mode}|{Serial}|{Escape(AliasName)}|{Notoriety}|{Range}|{Graphic}|{Color}|{Escape(Selector)}";
        }

        public override void Deserialize(string data)
        {
            var parts = data.Split('|');

            // Format: AttackEntity|Mode|Serial|AliasName|Notoriety|Range|Graphic|Color|Selector
            if (parts.Length >= 2 && Enum.TryParse(parts[1], out AttackMode mode))
                Mode = mode;
            if (parts.Length >= 3 && int.TryParse(parts[2], out int serial))
                Serial = serial;
            if (parts.Length >= 4)
                AliasName = Unescape(parts[3]);
            if (parts.Length >= 5 && Enum.TryParse(parts[4], out NotorietyFilter notoriety))
                Notoriety = notoriety;
            if (parts.Length >= 6 && int.TryParse(parts[5], out int range))
                Range = range;
            if (parts.Length >= 7 && int.TryParse(parts[6], out int graphic))
                Graphic = graphic;
            if (parts.Length >= 8 && int.TryParse(parts[7], out int color))
                Color = color;
            if (parts.Length >= 9)
                Selector = Unescape(parts[8]);
        }

        public override bool IsValid()
        {
            return true;
        }

        public override int GetDelay()
        {
            return 250; // Small delay for attack command
        }
    }
}