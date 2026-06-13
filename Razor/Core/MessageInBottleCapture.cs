#region license
// Razor: An Ultima Online Assistant
// Copyright (c) 2022 Razor Development Community on GitHub <https://github.com/markdwags/Razor>
// 
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <http://www.gnu.org/licenses/>.
#endregion

using RazorEnhanced;
using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Assistant.Core
{
    public class MessageInBottleCapture
    {
        private static readonly string[] _mibGumpLayout =
        {
            "{ page 0 }{ resizepic 0 40 2520 350 300 }{ xmfhtmlgump 30 80 285 160 1018326 1 1 }{ htmlgump 35 240 230 20 0 0 0 }{ button 35 265 4005 4007 1 0 0 }{ xmfhtmlgump 70 265 100 20 1011036 0 0 }",
            "{ page 0 }{ htmlgump 200 140 230 20 0 0 0 }{ resizepic 0 40 2520 350 300 }{ xmfhtmlgump 30 80 285 160 1018326 1 1 }{ htmlgump 35 240 230 20 1 0 0 }{ button 35 265 4005 4007 1 0 0 }{ xmfhtmlgump 70 265 100 20 1011036 0 0 }"
        };

        public static bool IsMibGump(string layout)
        {
            return _mibGumpLayout.Any(
                gumpLayout => layout.IndexOf(gumpLayout, StringComparison.OrdinalIgnoreCase) != -1);
        }

        public static void CaptureMibCoordinates(string coords)
        {
            string mibCapturePath = RazorEnhanced.Settings.General.ReadString("MibCapturePath");
            string mibLog;
            if (!string.IsNullOrEmpty(mibCapturePath))
                mibLog = Path.Combine(mibCapturePath, "MIBCapture.csv");
            else
                mibLog = Path.Combine(Assistant.Engine.RootPath, "MIBCapture.csv");

            // 130°15'N,63°16'W

            int xAxis = 0;
            int yAxis = 0;

            // Auto-detect format: if contains '|', treat as x|y, else as latitude/longitude
            if (coords.Contains("|"))
            {
                // 0   1 2
                // MIB|x|y
                string[] mibCoords = coords.Split('|');
                if (mibCoords.Length >= 3)
                {
                    int.TryParse(mibCoords[1], out xAxis);
                    int.TryParse(mibCoords[2], out yAxis);
                }
            }
            else
            {
                ConvertCoords(coords, ref xAxis, ref yAxis);
            }

            string entry = $"{xAxis},{yAxis},{World.Player.Map},mib,mib,red,3";

            bool alreadyExists = false;
            if (File.Exists(mibLog))
            {
                string existingContent = File.ReadAllText(mibLog);
                alreadyExists = existingContent.Contains(entry);
            }

            if (!alreadyExists)
            {
                using (StreamWriter sw = File.AppendText(mibLog))
                {
                    sw.WriteLine(entry);
                }

                World.Player.SendMessage(MsgLevel.Force, $"MIB Captured: {xAxis},{yAxis}");
            }
        }


        public static string GetMibCoordinatesLabel(string coords)
        {
            int xAxis = 0;
            int yAxis = 0;

            if (coords.Contains("|"))
            {
                string[] mibCoords = coords.Split('|');
                if (mibCoords.Length >= 3)
                {
                    int.TryParse(mibCoords[1], out xAxis);
                    int.TryParse(mibCoords[2], out yAxis);
                }
            }
            else
            {
                ConvertCoords(coords, ref xAxis, ref yAxis);
            }

            return $"[Location: {xAxis}, {yAxis}]";
        }

        private static readonly Regex _coordPattern = new Regex(
            @"(\d+)°(\d+)'([NS])\s*,\s*(\d+)°(\d+)'([EW])",
            RegexOptions.Compiled);

        private static void ConvertCoords(string coords, ref int xAxis, ref int yAxis)
        {
            Match match = _coordPattern.Match(coords);
            if (!match.Success)
                return;

            string yCoord = match.Groups[1].Value + "°" + match.Groups[2].Value + "'" + match.Groups[3].Value;
            string xCoord = match.Groups[4].Value + "°" + match.Groups[5].Value + "'" + match.Groups[6].Value;

            // Calc Y first
            string[] ySplit = yCoord.Split('°');
            double yDegree = Convert.ToDouble(ySplit[0]);
            double yMinute = Convert.ToDouble(ySplit[1].Substring(0, ySplit[1].IndexOf("'", StringComparison.Ordinal)));

            if (yCoord.Substring(yCoord.Length - 1).Equals("N"))
            {
                yAxis = (int)(1624 - (yMinute / 60) * (4096.0 / 360) - yDegree * (4096.0 / 360));
            }
            else
            {
                yAxis = (int)(1624 + (yMinute / 60) * (4096.0 / 360) + yDegree * (4096.0 / 360));
            }

            // Calc X next
            string[] xSplit = xCoord.Split('°');
            double xDegree = Convert.ToDouble(xSplit[0]);
            double xMinute = Convert.ToDouble(xSplit[1].Substring(0, xSplit[1].IndexOf("'", StringComparison.Ordinal)));

            if (xCoord.Substring(xCoord.Length - 1).Equals("W"))
            {
                xAxis = (int)(1323 - (xMinute / 60) * (5120.0 / 360) - xDegree * (5120.0 / 360));
            }
            else
            {
                xAxis = (int)(1323 + (xMinute / 60) * (5120.0 / 360) + xDegree * (5120.0 / 360));
            }

            // Normalize values outside of map range.
            if (xAxis < 0)
                xAxis += 5120;
            else if (xAxis > 5120)
                xAxis -= 5120;

            if (yAxis < 0)
                yAxis += 4096;
            else if (yAxis > 4096)
                yAxis -= 4096;
        }
    }
}