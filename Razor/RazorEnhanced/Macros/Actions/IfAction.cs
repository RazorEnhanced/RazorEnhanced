using Assistant;
using System;
using static IronPython.Modules._ast;

namespace RazorEnhanced.Macros.Actions
{
    public class IfAction : MacroAction
    {
        public enum ConditionType
        {
            // Player stats (use Operator for comparison)
            PlayerStats,

            // Player status (boolean checks - use BooleanValue for true/false)
            PlayerStatus,

            // Item/Target checks
            Find,
            InRange,
            TargetExists,
            Count,

            // Skill checks (use Operator for comparison)
            Skill,

            // Text/Buff checks
            InJournal,
            BuffExists
        }

        public enum PlayerStatType
        {
            HitPoints,
            Mana,
            Stamina,
            Weight,
            Str,
            Dex,
            Int
        }

        public enum PlayerStatusType
        {
            Poisoned,
            Paralyzed,
            Hidden,
            Mounted,
            IsAlive,
            RightHandEquipped,
            LeftHandEquipped
        }

        public enum Operator
        {
            GreaterThan,
            LessThan,
            Equal,
            GreaterOrEqual,
            LessOrEqual,
            NotEqual
        }

        public enum InRangeMode
        {
            LastTarget,
            Serial,
            ItemType,
            MobileType
        }

        public enum FindMode
        {
            Item,
            Mobile
        }

        public enum FindLocation
        {
            Backpack,
            Container,
            Ground
        }

        public ConditionType Type { get; set; }
        public PlayerStatType StatType { get; set; }
        public PlayerStatusType StatusType { get; set; }
        public Operator Op { get; set; }
        public int Value { get; set; }
        public string ValueToken { get; set; }
        public bool BooleanValue { get; set; }
        public bool FindStoreSerial { get; set; }
        public int Graphic { get; set; }
        public int Color { get; set; }
        public string SkillName { get; set; }
        public string BuffName { get; set; }
        public string PresetName { get; set; }
        public InRangeMode RangeMode { get; set; }
        public int RangeSerial { get; set; }
        public int RangeGraphic { get; set; }
        public int RangeColor { get; set; }

        // New Find properties
        public FindMode FindEntityMode { get; set; }
        public FindLocation FindEntityLocation { get; set; }
        public int FindContainerSerial { get; set; }
        public int FindRange { get; set; }

        private bool m_ConditionResult = false;
        public bool ConditionResult => m_ConditionResult;

        public IfAction()
        {
            Type = ConditionType.PlayerStats;
            StatType = PlayerStatType.HitPoints;
            StatusType = PlayerStatusType.Poisoned;
            Op = Operator.LessThan;
            Value = 50;
            ValueToken = "";
            BooleanValue = true;
            Graphic = 0;
            Color = -1;
            SkillName = "";
            PresetName = "";
            BuffName = "";
            RangeMode = InRangeMode.LastTarget;
            RangeSerial = 0;
            RangeGraphic = 0;
            RangeColor = -1;
            FindEntityMode = FindMode.Item;
            FindEntityLocation = FindLocation.Backpack;
            FindContainerSerial = 0;
            FindRange = 2;
            FindStoreSerial = false; // ADD THIS
        }

        // Update the parameterized constructor (around line 140)
        public IfAction(ConditionType type, Operator op, int value, int graphic, int color, string skillName, string valueToken, bool booleanValue, string presetName, string buffName = "", PlayerStatType statType = PlayerStatType.HitPoints, PlayerStatusType statusType = PlayerStatusType.Poisoned, InRangeMode rangeMode = InRangeMode.LastTarget, int rangeSerial = 0, int rangeGraphic = 0, int rangeColor = -1, FindMode findEntityMode = FindMode.Item, FindLocation findEntityLocation = FindLocation.Backpack, int findContainerSerial = 0, int findRange = 2, bool findStoreSerial = false) // ADD THIS PARAMETER
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
            FindStoreSerial = findStoreSerial; // ADD THIS
        }

        public override string GetActionName() => "If";

        public override void Execute()
        {
            m_ConditionResult = EvaluateCondition();
        }

        private int ResolveValue()
        {
            if (string.IsNullOrEmpty(ValueToken))
                return Value;

            if (World.Player == null)
                return Value;

            switch (ValueToken.ToLower())
            {
                case "{maxhp}":
                    return World.Player.HitsMax;
                case "{maxstam}":
                    return World.Player.StamMax;
                case "{maxmana}":
                    return World.Player.ManaMax;
                default:
                    return Value;
            }
        }

        private int GetPlayerStatValue(PlayerStatType statType)
        {
            if (World.Player == null)
                return 0;

            switch (statType)
            {
                case PlayerStatType.HitPoints:
                    return World.Player.Hits;
                case PlayerStatType.Mana:
                    return World.Player.Mana;
                case PlayerStatType.Stamina:
                    return World.Player.Stam;
                case PlayerStatType.Weight:
                    return World.Player.Weight;
                case PlayerStatType.Str:
                    return World.Player.Str;
                case PlayerStatType.Dex:
                    return World.Player.Dex;
                case PlayerStatType.Int:
                    return World.Player.Int;
                default:
                    return 0;
            }
        }

        private bool GetPlayerStatusValue(PlayerStatusType statusType)
        {
            if (World.Player == null)
                return false;

            switch (statusType)
            {
                case PlayerStatusType.Poisoned:
                    return World.Player.Poisoned;
                case PlayerStatusType.Paralyzed:
                    return World.Player.Paralized;
                case PlayerStatusType.Hidden:
                    return !World.Player.Visible;
                case PlayerStatusType.Mounted:
                    return World.Player.Mounted;
                case PlayerStatusType.IsAlive:
                    return !World.Player.IsGhost;
                case PlayerStatusType.RightHandEquipped:
                    {
                        var rightHand = World.Player.GetItemOnLayer(Layer.RightHand);
                        return rightHand != null && rightHand.Serial.IsItem;
                    }
                case PlayerStatusType.LeftHandEquipped:
                    {
                        var leftHand = World.Player.GetItemOnLayer(Layer.LeftHand);
                        return leftHand != null && leftHand.Serial.IsItem;
                    }
                default:
                    return false;
            }
        }

        private bool EvaluateFind()
        {
            if (FindEntityMode == FindMode.Item)
            {
                // Search for items
                switch (FindEntityLocation)
                {
                    case FindLocation.Backpack:
                        {
                            if (World.Player == null || World.Player.Backpack == null)
                                return false;

                            var item = Items.FindByID(Graphic, Color, Player.Backpack.Serial, true);

                            // Store serial if found and option enabled
                            if (item != null && FindStoreSerial)
                            {
                                Misc.SetSharedValue("findfound", (uint)item.Serial);
                            }

                            return item != null;
                        }

                    case FindLocation.Container:
                        {
                            if (FindContainerSerial == 0)
                                return false;

                            var item = Items.FindByID(Graphic, Color, FindContainerSerial, true);

                            // Store serial if found and option enabled
                            if (item != null && FindStoreSerial)
                            {
                                Misc.SetSharedValue("findfound", (uint)item.Serial);
                            }

                            return item != null;
                        }

                    case FindLocation.Ground:
                        {
                            var filter = new Items.Filter
                            {
                                Graphics = new System.Collections.Generic.List<int> { Graphic },
                                RangeMax = FindRange,
                                OnGround = 1
                            };

                            if (Color != -1)
                                filter.Hues = new System.Collections.Generic.List<int> { Color };

                            var items = Items.ApplyFilter(filter);

                            // Store serial if found and option enabled
                            if (items.Count > 0 && FindStoreSerial)
                            {
                                // Store the first (nearest) item found
                                Misc.SetSharedValue("findfound", (uint)items[0].Serial);
                            }

                            return items.Count > 0;
                        }

                    default:
                        return false;
                }
            }
            else // FindEntityMode == FindMode.Mobile
            {
                // Search for mobiles (always on ground within range)
                var filter = new Mobiles.Filter
                {
                    Bodies = new System.Collections.Generic.List<int> { Graphic },
                    RangeMax = FindRange
                };

                if (Color != -1)
                    filter.Hues = new System.Collections.Generic.List<int> { Color };

                var mobiles = Mobiles.ApplyFilter(filter);

                // Store serial if found and option enabled
                if (mobiles.Count > 0 && FindStoreSerial)
                {
                    // Store the first (nearest) mobile found
                    Misc.SetSharedValue("findfound", (uint)mobiles[0].Serial);
                }

                return mobiles.Count > 0;
            }
        }

        private bool EvaluateCondition()
        {
            if (World.Player == null)
                return false;

            int resolvedValue = ResolveValue();

            switch (Type)
            {
                case ConditionType.PlayerStats:
                    return CompareValues(GetPlayerStatValue(StatType), resolvedValue);

                case ConditionType.PlayerStatus:
                    return GetPlayerStatusValue(StatusType) == BooleanValue;

                case ConditionType.Find:
                    return EvaluateFind();

                case ConditionType.Count:
                    {
                        int count = Items.BackpackCount(Graphic, Color);
                        return CompareValues(count, resolvedValue);
                    }

                case ConditionType.InRange:
                    {
                        int targetSerial = 0;

                        // Determine which serial to check based on mode
                        switch (RangeMode)
                        {
                            case InRangeMode.LastTarget:
                                if (Targeting.GetLastTarger == 0)
                                    return false;
                                targetSerial = (int)Targeting.GetLastTarger;
                                break;

                            case InRangeMode.Serial:
                                targetSerial = RangeSerial;
                                break;

                            case InRangeMode.ItemType:
                                {
                                    // Search for item by type
                                    var filter = new Items.Filter
                                    {
                                        Graphics = new System.Collections.Generic.List<int> { RangeGraphic },
                                        RangeMax = resolvedValue,
                                        OnGround = 1
                                    };
                                    if (RangeColor != -1)
                                        filter.Hues = new System.Collections.Generic.List<int> { RangeColor };

                                    var items = Items.ApplyFilter(filter);
                                    if (items.Count > 0)
                                    {
                                        // Found at least one matching item in range
                                        return true;
                                    }
                                    return false;
                                }

                            case InRangeMode.MobileType:
                                {
                                    // Search for mobile by type
                                    var filter = new Mobiles.Filter
                                    {
                                        Bodies = new System.Collections.Generic.List<int> { RangeGraphic },
                                        RangeMax = resolvedValue
                                    };
                                    if (RangeColor != -1)
                                        filter.Hues = new System.Collections.Generic.List<int> { RangeColor };

                                    var mobiles = Mobiles.ApplyFilter(filter);
                                    if (mobiles.Count > 0)
                                    {
                                        // Found at least one matching mobile in range
                                        return true;
                                    }
                                    return false;
                                }
                        }

                        if (targetSerial == 0)
                            return false;

                        // Check distance for Last Target or Serial modes
                        var mobile = Mobiles.FindBySerial(targetSerial);
                        if (mobile != null)
                        {
                            int distance = Utility.Distance(World.Player.Position.X, World.Player.Position.Y,
                                mobile.Position.X, mobile.Position.Y);
                            return CompareValues(distance, resolvedValue);
                        }

                        // Check if it's an item instead
                        var itemEntity = Items.FindBySerial(targetSerial);
                        if (itemEntity != null)
                        {
                            int distance = Utility.Distance(World.Player.Position.X, World.Player.Position.Y,
                                itemEntity.Position.X, itemEntity.Position.Y);
                            return CompareValues(distance, resolvedValue);
                        }

                        return false;
                    }

                case ConditionType.TargetExists:
                    return Target.HasTarget() == BooleanValue;

                case ConditionType.Skill:
                    double skillValue = Player.GetSkillValue(SkillName);
                    return CompareValues((int)skillValue, resolvedValue);

                case ConditionType.InJournal:
                    {
                        if (string.IsNullOrEmpty(ValueToken))
                            return false;

                        bool found = Journal.GlobalJournal.Search(ValueToken);
                        return found == BooleanValue;
                    }

                case ConditionType.BuffExists:
                    bool buffActive = Player.BuffsExist(BuffName, true);
                    return BooleanValue ? buffActive : !buffActive;

                default:
                    return false;
            }
        }

        private bool CompareValues(int actualValue, int compareValue)
        {
            switch (Op)
            {
                case Operator.GreaterThan:
                    return actualValue > compareValue;
                case Operator.LessThan:
                    return actualValue < compareValue;
                case Operator.Equal:
                    return actualValue == compareValue;
                case Operator.GreaterOrEqual:
                    return actualValue >= compareValue;
                case Operator.LessOrEqual:
                    return actualValue <= compareValue;
                case Operator.NotEqual:
                    return actualValue != compareValue;
                default:
                    return false;
            }
        }

        public override string Serialize()
        {
            return $"If|{Type}|{Op}|{Value}|{Graphic}|{Color}|{Escape(SkillName)}|{Escape(ValueToken)}|{BooleanValue}|{Escape(PresetName)}|{Escape(BuffName)}|{StatType}|{StatusType}|{RangeMode}|{RangeSerial}|{RangeGraphic}|{RangeColor}|{FindEntityMode}|{FindEntityLocation}|{FindContainerSerial}|{FindRange}|{FindStoreSerial}";
        }

        public override void Deserialize(string data)
        {
            var parts = data.Split('|');

            if (parts.Length >= 2 && Enum.TryParse(parts[1], out ConditionType type))
                Type = type;
            if (parts.Length >= 3 && Enum.TryParse(parts[2], out Operator op))
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
            if (parts.Length >= 12 && Enum.TryParse(parts[11], out PlayerStatType statType))
                StatType = statType;
            if (parts.Length >= 13 && Enum.TryParse(parts[12], out PlayerStatusType statusType))
                StatusType = statusType;
            if (parts.Length >= 14 && Enum.TryParse(parts[13], out InRangeMode rangeMode))
                RangeMode = rangeMode;
            if (parts.Length >= 15 && int.TryParse(parts[14], out int rangeSerial))
                RangeSerial = rangeSerial;
            if (parts.Length >= 16 && int.TryParse(parts[15], out int rangeGraphic))
                RangeGraphic = rangeGraphic;
            if (parts.Length >= 17 && int.TryParse(parts[16], out int rangeColor))
                RangeColor = rangeColor;
            if (parts.Length >= 18 && Enum.TryParse(parts[17], out FindMode findEntityMode))
                FindEntityMode = findEntityMode;
            if (parts.Length >= 19 && Enum.TryParse(parts[18], out FindLocation findEntityLocation))
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

        public override bool IsValid()
        {
            return true;
        }

        public override int GetDelay()
        {
            return 0;
        }
    }
}