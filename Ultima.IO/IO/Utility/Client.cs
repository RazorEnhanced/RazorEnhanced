using System;
using System.IO;
using Ultima.IO;
using static Ultima.Constants;

namespace Ultima.Data
{
    internal class InvalidClientVersion : Exception
    {
        public InvalidClientVersion(string msg) : base(msg)
        {
        }
    }

    internal class InvalidClientDirectory : Exception
    {
        public InvalidClientDirectory(string msg) : base(msg)
        {
        }
    }
    internal static class Client
    {
        internal static NLog.Logger Log = NLog.LogManager.GetCurrentClassLogger();
        public static void Load(string clientPath)
        {
            Log.Trace($"Ultima Online installation folder: {clientPath}");
            if (!Directory.Exists(clientPath))
            {
                Log.Error("Invalid client directory: " + clientPath);
                ShowErrorMessage(string.Format("ClientPathIsNotAValidUODirectory {0}", clientPath));

                throw new InvalidClientDirectory($"'{clientPath}' is not a valid directory");
            }
            ClientFolder = clientPath;
            Log.Trace($"Ultima Online installation folder: {clientPath}");

            Log.Trace("Loading files...");
            ClientPath = Path.Combine(clientPath, "client.exe");

            string clientVersionText;
            bool parsedVersion = ClientVersionHelper.TryParseFromFile(ClientPath, out clientVersionText);
            if (parsedVersion)
            {
                ClientVersion clientVersion;
                bool valid = ClientVersionHelper.IsClientVersionValid(clientVersionText, out clientVersion);
                if (!valid)
                    throw new InvalidClientVersion($"Invalid client version: '{clientVersionText}'");
                Version = clientVersion;
            }
            else
                throw new InvalidClientDirectory($"'{clientPath}' is not a valid directory");

            Log.Trace($"Found a valid client.exe [{clientVersionText} - {Version}]");
            
            IsUOPInstallation = Version >= ClientVersion.CV_7000 && File.Exists(UOFileManager.GetUOFilePath("MainMisc.uop"));
            Protocol = ClientFlags.CF_T2A;

            if (Version >= ClientVersion.CV_200)
            {
                Protocol |= ClientFlags.CF_RE;
            }

            if (Version >= ClientVersion.CV_300)
            {
                Protocol |= ClientFlags.CF_TD;
            }

            if (Version >= ClientVersion.CV_308)
            {
                Protocol |= ClientFlags.CF_LBR;
            }

            if (Version >= ClientVersion.CV_308Z)
            {
                Protocol |= ClientFlags.CF_AOS;
            }

            if (Version >= ClientVersion.CV_405A)
            {
                Protocol |= ClientFlags.CF_SE;
            }

            if (Version >= ClientVersion.CV_60144)
            {
                Protocol |= ClientFlags.CF_SA;
            }

            Log.Trace($"Client path: '{clientPath}'");
            Log.Trace($"Client version: {Version}");
            Log.Trace($"Protocol: {Protocol}");
            Log.Trace("UOP? " + (IsUOPInstallation ? "yes" : "no"));

            UOFileManager.Load();            

            //ATTENTION: you will need to enable ALSO ultimalive server-side, or this code will have absolutely no effect!
            //UltimaLive.Enable();
            //PacketsTable.AdjustPacketSizeByVersion(Version);

        }
        public static ClientVersion Version
        { get; private set; }
        public static Ultima.Constants.ClientFlags Protocol { get; set; }
        public static string ClientPath { get; set; }
        public static string ClientFolder { get; set; }

        public static bool IsUOPInstallation { get; private set; }
        public static bool UseUOPGumps { get; set; }

        public static string Language { get; set; } = "en-US";

        public static void ShowErrorMessage(string msg)
        {
           //MessageBox.Show(msg, "ERROR!", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

    }
}