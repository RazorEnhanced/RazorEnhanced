using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Windows.Forms;
//using System.Linq;


namespace Assistant
{
    internal class Utility
    {
        internal static NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        //internal static void DebugLog(string message, [CallerFilePath] string codePath, [CallerLineNumber], int lineNumber, [CallerMemberName] string memberName)
        //{
        //    Logger.Debug(member)
        //}

        private static readonly Random m_Random = new();

        internal static int Random(int min, int max)
        {
            return m_Random.Next(max - min + 1) + min;
        }

        internal static int Random(int num)
        {
            return m_Random.Next(num);
        }

        internal static bool InRange(IPoint2D from, IPoint2D to, int range)
        {
            return (to.X >= (from.X - range))
                && (to.X <= (from.X + range))
                && (to.Y >= (from.Y - range))
                && (to.Y <= (from.Y + range));
        }

        internal static int Distance(int fx, int fy, int tx, int ty)
        {
            int xDelta = Math.Abs(fx - tx);
            int yDelta = Math.Abs(fy - ty);

            return (xDelta > yDelta ? xDelta : yDelta);
        }

        internal static int Distance(IPoint2D from, IPoint2D to)
        {
            int xDelta = Math.Abs(from.X - to.X);
            int yDelta = Math.Abs(from.Y - to.Y);

            return (xDelta > yDelta ? xDelta : yDelta);
        }

        internal static double DistanceSqrt(IPoint2D from, IPoint2D to)
        {
            float xDelta = Math.Abs(from.X - to.X);
            float yDelta = Math.Abs(from.Y - to.Y);

            return Math.Sqrt((xDelta * xDelta) + (yDelta * yDelta));
        }

        internal static void Offset(Direction d, ref int x, ref int y)
        {
            switch (d & Direction.mask)
            {
                case Direction.north: --y; break;
                case Direction.south: ++y; break;
                case Direction.west: --x; break;
                case Direction.east: ++x; break;
                case Direction.right: ++x; --y; break;
                case Direction.left: --x; ++y; break;
                case Direction.down: ++x; ++y; break;
                case Direction.up: --x; --y; break;
            }
        }

        internal static string CapitalizeAllWords(string str)
        {
            if (str == null || str == string.Empty)
                return string.Empty;
            if (str.Length == 1)
                return char.ToUpper(str[0]).ToString();

            StringBuilder sb = new();
            bool capitalizeNext = true;
            for (int i = 0; i < str.Length; i++)
            {
                if (capitalizeNext)
                    sb.Append(char.ToUpper(str[i]));
                else
                    sb.Append(str[i]);
                //capitalizeNext = (" .,;!".Contains(str[i].ToString()));
                capitalizeNext = " .,;!".IndexOf(str[i]) != -1;
            }
            return sb.ToString();
        }
        internal static void FormatBuffer(TextWriter output, Stream input, int length)
        {
            output.WriteLine("        0  1  2  3  4  5  6  7   8  9  A  B  C  D  E  F");
            output.WriteLine("       -- -- -- -- -- -- -- --  -- -- -- -- -- -- -- --");

            int byteIndex = 0;

            int whole = length >> 4;
            int rem = length & 0xF;

            for (int i = 0; i < whole; ++i, byteIndex += 16)
            {
                StringBuilder bytes = new(49);
                StringBuilder chars = new(16);

                for (int j = 0; j < 16; ++j)
                {
                    int c = input.ReadByte();

                    bytes.Append(c.ToString("X2"));

                    if (j != 7)
                    {
                        bytes.Append(' ');
                    }
                    else
                    {
                        bytes.Append("  ");
                    }

                    if (c >= 0x20 && c < 0x80)
                    {
                        chars.Append((char)c);
                    }
                    else
                    {
                        chars.Append('.');
                    }
                }

                output.Write(byteIndex.ToString("X4"));
                output.Write("   ");
                output.Write(bytes.ToString());
                output.Write("  ");
                output.WriteLine(chars.ToString());
            }

            if (rem != 0)
            {
                StringBuilder bytes = new(49);
                StringBuilder chars = new(rem);

                for (int j = 0; j < 16; ++j)
                {
                    if (j < rem)
                    {
                        int c = input.ReadByte();

                        bytes.Append(c.ToString("X2"));

                        if (j != 7)
                        {
                            bytes.Append(' ');
                        }
                        else
                        {
                            bytes.Append("  ");
                        }

                        if (c >= 0x20 && c < 0x80)
                        {
                            chars.Append((char)c);
                        }
                        else
                        {
                            chars.Append('.');
                        }
                    }
                    else
                    {
                        bytes.Append("   ");
                    }
                }

                output.Write(byteIndex.ToString("X4"));
                output.Write("   ");
                output.Write(bytes.ToString());
                if (rem <= 8)
                    output.Write("   ");
                else
                    output.Write("  ");
                output.WriteLine(chars.ToString());
            }
        }

        //private static readonly char[] pathChars = new char[] { '\\', '/' };

        internal static string FormatSize(long size)
        {
            if (size < 1024) // 1 K
                return String.Format("{0:#,##0} B", size);
            else if (size < 1048576) // 1 M
                return String.Format("{0:#,###.0} KB", size / 1024.0);
            else
                return String.Format("{0:#,###.0} MB", size / 1048576.0);
        }

        internal static string FormatTime(int sec)
        {
            int m = sec / 60;
            int h = m / 60;
            m %= 60;
            return String.Format("{0:#0}:{1:00}:{2:00}", h, m, sec % 60);
        }

        internal static string FormatTimeMS(int ms)
        {
            int s = ms / 1000;
            int m = s / 60;
            int h = m / 60;

            ms %= 1000;
            s %= 60;
            m %= 60;

            if (h > 0 || m > 55)
                return String.Format("{0:#0}:{1:00}:{2:00}.{3:000}", h, m, s, ms);
            else
                return String.Format("{0:00}:{1:00}.{2:000}", m, s, ms);
        }

        // Datagrid
        private static readonly int m_maxvalue = 65535;
        internal static string FormatDatagridAmountCell(DataGridViewCell cell, bool allowall)
        {
            if (cell.Value == null)
                return "0";

            if (cell.Value.ToString() == "-1" && allowall)
            {
                return "All";
            }
            else
            {
                Int32.TryParse(cell.Value.ToString(), out int amount);

                if (amount < 0 || amount > 9999)
                    amount = 9999;

                return amount.ToString();
            }
        }

        internal static string FormatDatagridItemIDCell(DataGridViewCell cell)
        {
            int itemid = m_maxvalue;
            if (cell.Value != null && !cell.Value.ToString().Contains("-"))
            {
                try
                {
                    itemid = Convert.ToInt32((string)cell.Value, 16);
                }
                catch { }

                if (itemid > m_maxvalue)
                    itemid = m_maxvalue;
            }
            return "0x" + itemid.ToString("X4");
        }

        internal static string FormatDatagridItemIDCellAutoLoot(DataGridViewCell cell)
        {
            int itemid = m_maxvalue;

            if (cell.Value == null)
                return "0x" + m_maxvalue.ToString("X4");

            if ((cell.Value.ToString() == "All")
                ||
                (cell.Value.ToString().Contains("-"))
                )
            {
                return "All";
            }

            try
            {
                itemid = Convert.ToInt32((string)cell.Value, 16);
            }
            catch { }

            if (itemid > m_maxvalue)
                itemid = m_maxvalue;

            if (itemid == -1)
            {
                return "All";
            }

            return "0x" + itemid.ToString("X4");
        }

        internal static string FormatDatagridColorCell(DataGridViewCell cell)
        {
            int color = m_maxvalue;
            if (cell.Value == null)
                return "0x" + m_maxvalue.ToString("X4");

            if ((cell.Value.ToString() == "All")
                ||
                (cell.Value.ToString().Contains("-"))
                )
            {
                return "All";
            }

            try
            {
                color = Convert.ToInt32((string)cell.Value, 16);
            }
            catch { }

            if (color > m_maxvalue)
                color = m_maxvalue;

            if (color == -1)
            {
                return "All";
            }

            return "0x" + color.ToString("X4");
        }

        internal static string FormatDatagridColorGraphCell(DataGridViewCell cell)
        {
            int color = m_maxvalue;
            if (cell.Value == null)
                return "0x" + m_maxvalue.ToString("X4");

            if (cell.Value.ToString() == "-1")
            {
                return "No Change";
            }
            else
            {
                if (!cell.Value.ToString().Contains("-"))
                {
                    try
                    {
                        color = Convert.ToInt32((string)cell.Value, 16);
                    }
                    catch { }

                    if (color > m_maxvalue)
                        color = m_maxvalue;
                }
                return "0x" + color.ToString("X4");
            }
        }

        internal static int ToInt32(string str, int def)
        {
            if (str == null)
                return def;

            try
            {
                if (str.Length > 2 && str.Substring(0, 2).ToLower() == "0x")
                    return Convert.ToInt32(str.Substring(2), 16);
                else
                    return Convert.ToInt32(str);
            }
            catch
            {
                return def;
            }
        }

        internal static void ClipBoardCopy(string txt)
        {
            if (string.IsNullOrEmpty(txt))
                return;

            try
            {
                Clipboard.SetText(txt);
            }
            catch { }
        }


        /// <summary>
        /// Delays execution until the specified condition is met.
        /// </summary>
        /// <param name="condition">The lambda conditional that must be met to end the delay.</param>
        /// <param name="delay">The maximum time to delay.</param>
        /// <returns>true if condition was satisfied, false if max delay time expired</returns>
        public static bool DelayUntil(Func<bool> condition, int delay)
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();
            while (condition() == false)
            {
                Thread.Sleep(2);
                if (watch.ElapsedMilliseconds >= delay)
                    break;
                var elapsedMs2 = watch.ElapsedMilliseconds;
            }
            watch.Stop();
            var elapsedMs = watch.ElapsedMilliseconds;

            return condition();
        }

        public static string GetCaseInsensitiveFilePath(string filePath)
        {
            string directory = Path.GetDirectoryName(filePath);
            string fileName = Path.GetFileName(filePath);

            if (string.IsNullOrEmpty(directory) || string.IsNullOrEmpty(fileName))
            {
                throw new ArgumentException("Invalid file path");
            }

            try
            {
                foreach (var f in Directory.GetFiles(directory))
                {
                    if (string.Equals(Path.GetFileName(f), fileName, StringComparison.OrdinalIgnoreCase))
                    {
                        return f;
                    }
                }

                throw new FileNotFoundException($"File not found: {filePath}");
            }
            catch (Exception ex)
            {
                throw new Exception($"Error accessing directory: {ex.Message}");
            }
        }
    }
}
