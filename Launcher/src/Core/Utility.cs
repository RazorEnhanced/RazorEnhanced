using System;
using System.IO;
using System.Text;
using System.Linq;
using System.Windows.Forms;
using System.Net;
using System.DirectoryServices;

namespace Assistant
{
    internal class Utility
    {
        private static readonly Random m_Random = new Random();

        internal static int Random(int min, int max)
        {
            return m_Random.Next(max - min + 1) + min;
        }

        internal static int Random(int num)
        {
            return m_Random.Next(num);
        }

        internal static System.Net.IPAddress Resolve(string addr)
        {
            IPAddress ipAddr = IPAddress.None;

            if (string.IsNullOrEmpty(addr))
                return ipAddr;

            if (!IPAddress.TryParse(addr, out ipAddr))
            {
                try
                {
                    IPHostEntry iphe = Dns.GetHostEntry(addr);

                    if (iphe.AddressList.Length > 0)
                        ipAddr = iphe.AddressList[iphe.AddressList.Length - 1];
                }
                catch
                {
                }
            }

            return ipAddr;
        }


        internal static string CapitalizeAllWords(string str)
        {
            if (str == null || str == string.Empty)
                return string.Empty;
            if (str.Length == 1)
                return char.ToUpper(str[0]).ToString();

            StringBuilder sb = new StringBuilder();
            bool capitalizeNext = true;
            for (int i = 0; i < str.Length; i++)
            {
                if (capitalizeNext)
                    sb.Append(char.ToUpper(str[i]));
                else
                    sb.Append(str[i]);
                capitalizeNext = (" .,;!".Contains(str[i]));
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
                StringBuilder bytes = new StringBuilder(49);
                StringBuilder chars = new StringBuilder(16);

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
                StringBuilder bytes = new StringBuilder(49);
                StringBuilder chars = new StringBuilder(rem);

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

        internal static unsafe void FormatBuffer(TextWriter output, byte* buff, int length)
        {
            output.WriteLine("        0  1  2  3  4  5  6  7   8  9  A  B  C  D  E  F");
            output.WriteLine("       -- -- -- -- -- -- -- --  -- -- -- -- -- -- -- --");

            int byteIndex = 0;

            int whole = length >> 4;
            int rem = length & 0xF;

            for (int i = 0; i < whole; ++i, byteIndex += 16)
            {
                StringBuilder bytes = new StringBuilder(49);
                StringBuilder chars = new StringBuilder(16);

                for (int j = 0; j < 16; ++j)
                {
                    int c = *buff++;

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
                StringBuilder bytes = new StringBuilder(49);
                StringBuilder chars = new StringBuilder(rem);

                for (int j = 0; j < 16; ++j)
                {
                    if (j < rem)
                    {
                        int c = *buff++;

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

        private static readonly char[] pathChars = new char[] { '\\', '/' };

        internal static string PathDisplayStr(string path, int maxLen)
        {
            if (path == null || path.Length <= maxLen || path.Length < 5)
                return path;

            int first = (maxLen - 3) / 2;
            int last = path.LastIndexOfAny(pathChars);
            if (last == -1 || last < maxLen / 4)
                last = path.Length - first;
            first = maxLen - last - 3;
            if (first < 0)
                first = 1;
            if (last < first)
                last = first;

            return String.Format("{0}...{1}", path.Substring(0, first), path.Substring(last));
        }

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

        public static class StringCipher
        {
            // This constant is used to determine the keysize of the encryption algorithm in bits.
            // We divide this by 8 within the code below to get the equivalent number of bytes.
            private const int Keysize = 256;

            // This constant determines the number of iterations for the password bytes generation function.
            private const int DerivationIterations = 1000;

            public static string Encrypt(string plainText, string passPhrase)
            {
                // Salt and IV is randomly generated each time, but is preprended to encrypted cipher text
                // so that the same Salt and IV values can be used when decrypting.
                var saltStringBytes = Generate256BitsOfRandomEntropy();
                var ivStringBytes = Generate256BitsOfRandomEntropy();
                var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
                using (var password = new System.Security.Cryptography.Rfc2898DeriveBytes(passPhrase, saltStringBytes, DerivationIterations))
                {
                    var keyBytes = password.GetBytes(Keysize / 8);
                    using (var symmetricKey = new System.Security.Cryptography.RijndaelManaged())
                    {
                        symmetricKey.BlockSize = 256;
                        symmetricKey.Mode = System.Security.Cryptography.CipherMode.CBC;
                        symmetricKey.Padding = System.Security.Cryptography.PaddingMode.PKCS7;
                        using (var encryptor = symmetricKey.CreateEncryptor(keyBytes, ivStringBytes))
                        {
                            using (var memoryStream = new MemoryStream())
                            {
                                using (var cryptoStream = new System.Security.Cryptography.CryptoStream(memoryStream, encryptor, System.Security.Cryptography.CryptoStreamMode.Write))
                                {
                                    cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);
                                    cryptoStream.FlushFinalBlock();
                                    // Create the final bytes as a concatenation of the random salt bytes, the random iv bytes and the cipher bytes.
                                    var cipherTextBytes = saltStringBytes;
                                    cipherTextBytes = cipherTextBytes.Concat(ivStringBytes).ToArray();
                                    cipherTextBytes = cipherTextBytes.Concat(memoryStream.ToArray()).ToArray();
                                    memoryStream.Close();
                                    cryptoStream.Close();
                                    return Convert.ToBase64String(cipherTextBytes);
                                }
                            }
                        }
                    }
                }
            }

            public static string Decrypt(string cipherText, string passPhrase)
            {
                // Get the complete stream of bytes that represent:
                // [32 bytes of Salt] + [32 bytes of IV] + [n bytes of CipherText]
                var cipherTextBytesWithSaltAndIv = Convert.FromBase64String(cipherText);
                // Get the saltbytes by extracting the first 32 bytes from the supplied cipherText bytes.
                var saltStringBytes = cipherTextBytesWithSaltAndIv.Take(Keysize / 8).ToArray();
                // Get the IV bytes by extracting the next 32 bytes from the supplied cipherText bytes.
                var ivStringBytes = cipherTextBytesWithSaltAndIv.Skip(Keysize / 8).Take(Keysize / 8).ToArray();
                // Get the actual cipher text bytes by removing the first 64 bytes from the cipherText string.
                var cipherTextBytes = cipherTextBytesWithSaltAndIv.Skip((Keysize / 8) * 2).Take(cipherTextBytesWithSaltAndIv.Length - ((Keysize / 8) * 2)).ToArray();

                using (var password = new System.Security.Cryptography.Rfc2898DeriveBytes(passPhrase, saltStringBytes, DerivationIterations))
                {
                    var keyBytes = password.GetBytes(Keysize / 8);
                    using (var symmetricKey = new System.Security.Cryptography.RijndaelManaged())
                    {
                        symmetricKey.BlockSize = 256;
                        symmetricKey.Mode = System.Security.Cryptography.CipherMode.CBC;
                        symmetricKey.Padding = System.Security.Cryptography.PaddingMode.PKCS7;
                        using (var decryptor = symmetricKey.CreateDecryptor(keyBytes, ivStringBytes))
                        {
                            using (var memoryStream = new MemoryStream(cipherTextBytes))
                            {
                                using (var cryptoStream = new System.Security.Cryptography.CryptoStream(memoryStream, decryptor, System.Security.Cryptography.CryptoStreamMode.Read))
                                {
                                    var plainTextBytes = new byte[cipherTextBytes.Length];
                                    var decryptedByteCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);
                                    memoryStream.Close();
                                    cryptoStream.Close();
                                    return System.Text.Encoding.UTF8.GetString(plainTextBytes, 0, decryptedByteCount);
                                }
                            }
                        }
                    }
                }
            }

            private static byte[] Generate256BitsOfRandomEntropy()
            {
                var randomBytes = new byte[32]; // 32 Bytes will give us 256 bits.
                using (var rngCsp = new System.Security.Cryptography.RNGCryptoServiceProvider())
                {
                    // Fill the array with cryptographically secure random bytes.
                    rngCsp.GetBytes(randomBytes);
                }
                return randomBytes;
            }
        }
        internal static bool IsLinux
        {
            get
            {
                try
                {
                    using (Microsoft.Win32.RegistryKey localKey = Microsoft.Win32.RegistryKey.OpenBaseKey(Microsoft.Win32.RegistryHive.LocalMachine, Microsoft.Win32.RegistryView.Registry64))
                    {
                        using (Microsoft.Win32.RegistryKey key = localKey.OpenSubKey("Software\\Wine\\Drives"))
                        {
                            if (key != null)
                            {
                                return true;
                            }
                        }
                    }
                }
                catch (Exception)
                {
                }
                return false;
            }
        }

        public static string GetComputerSid()
        {
            if (IsLinux)
            {
                try
                {
                    using (Microsoft.Win32.RegistryKey localKey = Microsoft.Win32.RegistryKey.OpenBaseKey(Microsoft.Win32.RegistryHive.LocalMachine, Microsoft.Win32.RegistryView.Registry64))
                    {
                        using (Microsoft.Win32.RegistryKey key = localKey.OpenSubKey("Software\\Microsoft\\Cryptography"))
                        {
                            {
                                if (key != null)
                                {
                                    var o = key.GetValue("MachineGuid");
                                    if (o != null)
                                    {
                                        return o.ToString();
                                    }
                                }
                            }
                        }
                    }
                }
                catch (Exception)
                { }
            }
            else
            {
                System.Security.Principal.SecurityIdentifier sid = new System.Security.Principal.SecurityIdentifier((byte[])new DirectoryEntry(string.Format("WinNT://{0},Computer", Environment.MachineName)).Children.Cast<DirectoryEntry>().First().InvokeGet("objectSID"), 0).AccountDomainSid;
                return sid.ToString();
            }
            return "Some crap I made up112.45678-234523";
        }
        internal static string key = GetComputerSid();
        // Passwords
        public static string Protect(string text)
        {
            if (string.IsNullOrEmpty(text))
                return "";
            try
            {
                return StringCipher.Encrypt(text, key);
            }
            catch (Exception)
            {
                return "";
            }
        }

        public static string Unprotect(string text)
        {
            if (string.IsNullOrEmpty(text))
                return "";
            try
            {
                return StringCipher.Decrypt(text, key);
            }
            catch (Exception)
            {
                return "";
            }
        }




    }
}
