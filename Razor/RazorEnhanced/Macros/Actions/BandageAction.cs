using System;

namespace RazorEnhanced.Macros.Actions
{
    [Serializable]
    public class BandageAction : MacroAction
    {
        public enum BandageTargetMode
        {
            Self,
            Serial,
            Alias
        }

        public BandageTargetMode TargetMode { get; set; }
        public int TargetSerial { get; set; }
        public string TargetAlias { get; set; }

        public BandageAction()
        {
            TargetMode = BandageTargetMode.Self;
            TargetSerial = 0;
            TargetAlias = string.Empty;
        }

        public BandageAction(int serial)
        {
            TargetMode = BandageTargetMode.Serial;
            TargetSerial = serial;
            TargetAlias = string.Empty;
        }

        public override string GetActionName() => "Bandage";

        /*
        public override string GetActionName()
        {
            return TargetMode switch
            {
                BandageTargetMode.Self => "Bandage Self",
                BandageTargetMode.Serial => $"Bandage Serial 0x{TargetSerial:X8}",
                BandageTargetMode.Alias => $"Bandage Alias '{TargetAlias}'",
                _ => "Bandage"
            };
        }
        */

        public override void Execute()
        {
            // Use BandageHeal agent text command if enabled and targeting self
            if (TargetMode == BandageTargetMode.Self && BandageHeal.SelfHealUseText)
            {
                string bandSelfCommand = BandageHeal.SelfHealUseTextSelfContent;
                if (string.IsNullOrEmpty(bandSelfCommand))
                    bandSelfCommand = "[bandself";
                Player.ChatSay(0, bandSelfCommand);
                return;
            }

            // Always use default bandage type for now
            Item bandage = Items.FindByID(0x0E21, -1, Player.Backpack.Serial);

            if (bandage == null)
            {
                Misc.SendMessage("No bandages found in backpack.", 33);
                return;
            }

            Items.UseItem(bandage);
            Misc.Pause(100);

            switch (TargetMode)
            {
                case BandageTargetMode.Self:
                    Target.Self();
                    break;
                case BandageTargetMode.Serial:
                    if (TargetSerial != 0)
                    {
                        Target.WaitForTarget(1000, false);
                        Target.TargetExecute(TargetSerial);
                    }
                    else
                    {
                        Misc.SendMessage("Bandage: Invalid serial (0x00000000)", 33);
                    }
                    break;
                case BandageTargetMode.Alias:
                    if (string.IsNullOrWhiteSpace(TargetAlias))
                    {
                        Misc.SendMessage("Bandage: Alias name is empty", 33);
                        return;
                    }

                    int aliasSerial = 0;
                    if (Misc.CheckSharedValue(TargetAlias.ToLower()))
                    {
                        object aliasValue = Misc.ReadSharedValue(TargetAlias.ToLower());
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
                            Misc.SendMessage($"Bandage: Invalid alias value for '{TargetAlias}'", 33);
                            return;
                        }
                    }
                    else
                    {
                        Misc.SendMessage($"Bandage: Alias '{TargetAlias}' not found", 33);
                        return;
                    }

                    if (aliasSerial != 0)
                    {
                        Target.WaitForTarget(1000, false);
                        Target.TargetExecute(aliasSerial);
                    }
                    else
                    {
                        Misc.SendMessage($"Bandage: Alias '{TargetAlias}' resolved to 0x00000000", 33);
                    }
                    break;
            }
        }

        public override int GetDelay() => 650;

        public override string Serialize()
        {
            // Format: Bandage|mode|serial|alias
            return $"Bandage|{(int)TargetMode}|{TargetSerial}|{Escape(TargetAlias)}";
        }

        public override void Deserialize(string data)
        {
            var parts = data.Split('|');
            if (parts.Length >= 2)
            {
                TargetMode = (BandageTargetMode)int.Parse(parts[1]);
                TargetSerial = parts.Length > 2 ? int.TryParse(parts[2], out int s) ? s : 0 : 0;
                TargetAlias = parts.Length > 3 ? Unescape(parts[3]) : string.Empty;
            }
        }

        public override bool IsValid()
        {
            if (TargetMode == BandageTargetMode.Self && BandageHeal.SelfHealUseText && BandageHeal.SelfHealIgnoreCount)
                return true;

            // Otherwise check for bandages in backpack
            var bandage = Items.FindByID(0x0E21, -1, Player.Backpack.Serial);
            return bandage != null;
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
    }
}