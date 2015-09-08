using System;
using System.Media;
using System.Collections.Generic;
using Assistant;


namespace RazorEnhanced
{
    public class Pets
    {
        public static void Stable(string text)
        {
            List<ushort> kw = new List<ushort> { 16, 8 };
            ClientCommunication.SendToServer(new ClientUniMessage(Assistant.MessageType.Regular, RazorEnhanced.Settings.General.ReadInt("SpeechHue"), 3, Language.CliLocName, kw, text));
        }

        public static void Claim(string text)
        {
            List<ushort> kw = new List<ushort> { 16, 9 };
            ClientCommunication.SendToServer(new ClientUniMessage(Assistant.MessageType.Regular, RazorEnhanced.Settings.General.ReadInt("SpeechHue"), 3, Language.CliLocName, kw, text));
        }

        public static void Command(string command)
        {
            List<ushort> kw = new List<ushort>();

            switch (command)
            {
                case "All Come":
                    {
                        kw = new List<ushort> { 33, 85, 22, 64 };
                        ClientCommunication.SendToServer(new ClientUniMessage(Assistant.MessageType.Regular, RazorEnhanced.Settings.General.ReadInt("SpeechHue"), 3, Language.CliLocName, kw, "all come"));
                        break;
                    }
                case "All Follow Me":
                    {
                        kw = new List<ushort> { 48, 232, 22, 49, 108 };
                        ClientCommunication.SendToServer(new ClientUniMessage(Assistant.MessageType.Regular, RazorEnhanced.Settings.General.ReadInt("SpeechHue"), 3, Language.CliLocName, kw, "all follow me"));
                        break;
                    }
                case "All Follow":
                    {
                        kw = new List<ushort> { 48, 232, 21, 161, 101 };
                        ClientCommunication.SendToServer(new ClientUniMessage(Assistant.MessageType.Regular, RazorEnhanced.Settings.General.ReadInt("SpeechHue"), 3, Language.CliLocName, kw, "all follow"));
                        break;
                    }
                case "All Guard Me":
                    {
                        kw = new List<ushort> { 48, 7, 22, 177, 153 };
                        ClientCommunication.SendToServer(new ClientUniMessage(Assistant.MessageType.Regular, RazorEnhanced.Settings.General.ReadInt("SpeechHue"), 3, Language.CliLocName, kw, "all guard me"));
                        break;
                    }
                case "All Guard":
                    {
                        kw = new List<ushort> { 64, 7, 21, 193, 102, 25, 144 };
                        ClientCommunication.SendToServer(new ClientUniMessage(Assistant.MessageType.Regular, RazorEnhanced.Settings.General.ReadInt("SpeechHue"), 3, Language.CliLocName, kw, "all guard"));
                        break;
                    }
                case "All Kill":
                    {
                        kw = new List<ushort> { 33, 93, 22, 128 };
                        ClientCommunication.SendToServer(new ClientUniMessage(Assistant.MessageType.Regular, RazorEnhanced.Settings.General.ReadInt("SpeechHue"), 3, Language.CliLocName, kw, "all kill"));
                        break;
                    }
                case "All Stay":
                    {
                        kw = new List<ushort> { 33, 111, 23, 0 };
                        ClientCommunication.SendToServer(new ClientUniMessage(Assistant.MessageType.Regular, RazorEnhanced.Settings.General.ReadInt("SpeechHue"), 3, Language.CliLocName, kw, "all stay"));
                        break;
                    }
                case "All Stop":
                    {
                        kw = new List<ushort> { 33, 97, 22, 112 };
                        ClientCommunication.SendToServer(new ClientUniMessage(Assistant.MessageType.Regular, RazorEnhanced.Settings.General.ReadInt("SpeechHue"), 3, Language.CliLocName, kw, "all stop"));
                    }
                    break;
                default:
                    break;
            }
        }  
    }
}
