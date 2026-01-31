using Assistant;
using RazorEnhanced.Macros;
using System;
using Ultima;

namespace RazorEnhanced.Macros.Actions
{
    [Serializable]
    public class MessagingAction : MacroAction
    {
        public enum MessageType
        {
            Say,
            Yell,
            Whisper,
            Emote,
            Overhead,
            System,
            General,
            Guild,
            Alliance,
            Party
        }

        public MessageType Type { get; set; }
        public string Message { get; set; }
        public int Hue { get; set; }
        public string TargetSerialOrAlias { get; set; } // Serial (hex/dec) or alias, just like CastSpellAction

        public MessagingAction()
        {
            Type = MessageType.Say;
            Message = string.Empty;
            Hue = 0;
            TargetSerialOrAlias = string.Empty;
        }

        public MessagingAction(MessageType type, string message, int hue = 0, string targetSerialOrAlias = "")
        {
            Type = type;
            Message = message;
            Hue = hue;
            TargetSerialOrAlias = targetSerialOrAlias ?? string.Empty;
        }

        public override string GetActionName()
        {
            return "Messaging";
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
            // Format: Messaging|type|message|hue|targetSerialOrAlias
            return $"Messaging|{(int)Type}|{Escape(Message)}|{Hue}|{Escape(TargetSerialOrAlias)}";
        }

        public override void Deserialize(string data)
        {
            // Split on |, but handle escaped pipes
            var parts = new System.Collections.Generic.List<string>();
            var current = "";
            bool escape = false;
            for (int i = 0; i < data.Length; i++)
            {
                char c = data[i];
                if (escape)
                {
                    current += c;
                    escape = false;
                }
                else if (c == '\\')
                {
                    escape = true;
                }
                else if (c == '|')
                {
                    parts.Add(current);
                    current = "";
                }
                else
                {
                    current += c;
                }
            }
            parts.Add(current);

            // parts[0] is "Messaging"
            if (parts.Count >= 3)
            {
                Type = (MessageType)int.Parse(parts[1]);
                Message = Unescape(parts[2]);
                Hue = parts.Count > 3 ? int.TryParse(parts[3], out int hue) ? hue : 0 : 0;
                TargetSerialOrAlias = parts.Count > 4 ? Unescape(parts[4]) : string.Empty;
            }
        }

        public override void Execute()
        {
            int hue = 1153;
            switch (Type)
            {
                case MessageType.Say:
                    Player.ChatSay(Hue > 0 ? Hue : hue, Message);
                    break;
                case MessageType.Yell:
                    Player.ChatYell(Hue > 0 ? Hue : hue, Message);
                    break;
                case MessageType.Whisper:
                    Player.ChatWhisper(Hue > 0 ? Hue : hue, Message);
                    break;
                case MessageType.Emote:
                    Player.ChatEmote(Hue > 0 ? Hue : hue, Message);
                    break;
                case MessageType.Overhead:
                    uint resolvedSerial = ResolveSerialOrAlias(TargetSerialOrAlias);
                    if (resolvedSerial != 0)
                    {
                        var mobile = Mobiles.FindBySerial((int)resolvedSerial);
                        if (mobile != null)
                        {
                            Mobiles.Message((int)resolvedSerial, Hue > 0 ? Hue : hue, Message, false);
                        }
                        else
                        {
                            var item = Items.FindBySerial((int)resolvedSerial);
                            if (item != null)
                            {
                                Items.Message((int)resolvedSerial, Hue > 0 ? Hue : hue, Message);
                            }
                            else
                                Player.HeadMessage(Hue > 0 ? Hue : hue, Message);
                        }
                    }
                    else
                        Player.HeadMessage(Hue > 0 ? Hue : hue, Message);
                    break;
                case MessageType.System:
                    Misc.SendMessage(Message, Hue > 0 ? Hue : hue);
                    break;
                case MessageType.General:
                    Player.ChatChannel(Message);
                    break;
                case MessageType.Guild:
                    Player.ChatGuild(Message);
                    break;
                case MessageType.Alliance:
                    Player.ChatAlliance(Message);
                    break;
                case MessageType.Party:
                    Player.ChatParty(Message);
                    break;
            }
        }

        private uint ResolveSerialOrAlias(string serialOrAlias)
        {
            if (string.IsNullOrWhiteSpace(serialOrAlias))
                return 0;

            // Try parse as hex or decimal serial
            if (serialOrAlias.StartsWith("0x", StringComparison.OrdinalIgnoreCase))
            {
                if (uint.TryParse(serialOrAlias.Substring(2), System.Globalization.NumberStyles.HexNumber, null, out uint serial))
                    return serial;
            }
            else if (uint.TryParse(serialOrAlias, out uint serial))
            {
                return serial;
            }

            // Otherwise, treat as alias (SharedValue)
            string aliasKey = serialOrAlias.ToLower();
            if (Misc.CheckSharedValue(aliasKey))
            {
                object aliasValue = Misc.ReadSharedValue(aliasKey);
                if (aliasValue is uint uintVal)
                {
                    return uintVal;
                }
                else if (uint.TryParse(aliasValue.ToString(), out uint parsedVal))
                {
                    return parsedVal;
                }
            }
            return 0;
        }
    }
}