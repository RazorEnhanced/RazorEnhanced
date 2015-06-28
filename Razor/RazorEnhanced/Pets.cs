using System;
using System.Media;
using System.Collections.Generic;
using Assistant;


namespace RazorEnhanced
{
    public class Pets
    {
        public static void AllFollow()
        {
            List<ushort> kw = new List<ushort> { 48, 232, 21, 161, 101 };
            ClientCommunication.SendToServer(new ClientUniMessage(Assistant.MessageType.Regular, RazorEnhanced.Settings.General.ReadInt("SpeechHue"), 3, Language.CliLocName, kw, "all guard"));
        }
        public static void AllFollowMe()
        {
            List<ushort> kw = new List<ushort> { 48, 232, 22, 49, 108 };
            ClientCommunication.SendToServer(new ClientUniMessage(Assistant.MessageType.Regular, RazorEnhanced.Settings.General.ReadInt("SpeechHue"), 3, Language.CliLocName, kw, "all follow me"));
        }
        public static void AllKill(int mseconds)
        {
            List<ushort> kw = new List<ushort> { 33, 93, 22, 128 };
            ClientCommunication.SendToServer(new ClientUniMessage(Assistant.MessageType.Regular, RazorEnhanced.Settings.General.ReadInt("SpeechHue"), 3, Language.CliLocName, kw, "all kill"));
        }
        public static void AllGuard(int mseconds)
        {
            List<ushort> kw = new List<ushort> { 64, 7, 21, 193, 102, 25, 144 };
            ClientCommunication.SendToServer(new ClientUniMessage(Assistant.MessageType.Regular, RazorEnhanced.Settings.General.ReadInt("SpeechHue"), 3, Language.CliLocName, kw, "all guard"));
        }
        public static void AllGuardMe(int mseconds)
        {
            List<ushort> kw = new List<ushort> { 48, 7, 22, 177, 153 };
            ClientCommunication.SendToServer(new ClientUniMessage(Assistant.MessageType.Regular, RazorEnhanced.Settings.General.ReadInt("SpeechHue"), 3, Language.CliLocName, kw, "all guard me"));
        }
        public static void AllStay(int mseconds)
        {
            List<ushort> kw = new List<ushort> { 33, 111, 23, 0 };
            ClientCommunication.SendToServer(new ClientUniMessage(Assistant.MessageType.Regular, RazorEnhanced.Settings.General.ReadInt("SpeechHue"), 3, Language.CliLocName, kw, "all stay"));
        }
        public static void AllStop(int mseconds)
        {
            List<ushort> kw = new List<ushort> { 33, 97, 22, 112 };
            ClientCommunication.SendToServer(new ClientUniMessage(Assistant.MessageType.Regular, RazorEnhanced.Settings.General.ReadInt("SpeechHue"), 3, Language.CliLocName, kw, "all stop"));
        }
        public static void AllCome(int mseconds)
        {
            List<ushort> kw = new List<ushort> { 33, 85, 22, 64 };
            ClientCommunication.SendToServer(new ClientUniMessage(Assistant.MessageType.Regular, RazorEnhanced.Settings.General.ReadInt("SpeechHue"), 3, Language.CliLocName, kw, "all come"));
        }
    }
}
