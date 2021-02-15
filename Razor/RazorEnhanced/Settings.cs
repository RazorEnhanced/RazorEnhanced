using Assistant;
using JsonData;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Forms;
using System.Web.Security;
using System.DirectoryServices;


namespace RazorEnhanced
{
	internal class Settings
	{
		// Versione progressiva della struttura dei salvataggi per successive modifiche
		private static int SettingVersion = 9;

		private static string m_profileName = null;

		private static DataSet m_Dataset;
		internal static DataSet Dataset
		{
			get { return m_Dataset; }
		}


		internal delegate DataTable initFN(string tableName);
		internal delegate DataTable loadFN(string filename, string tableName);
		internal delegate void saveFN(string filename, string tableName, DataTable targets);


		internal static Dictionary<string, initFN> initDict = new Dictionary<string, initFN>()
				{
					{ "AUTOLOOT_ITEMS", InitItems<RazorEnhanced.AutoLoot.AutoLootItem>},
					{ "AUTOLOOT_LISTS", InitAutoLootLists},
					{ "BUY_ITEMS", InitItems<RazorEnhanced.BuyAgent.BuyAgentItem> },
					{ "BUY_LISTS", InitBuyAgentLists },
					{ "DRESS_ITEMS", InitItems<RazorEnhanced.Dress.DressItemNew> },
					{ "DRESS_LISTS", InitDressingAgentLists },
					{ "FILTER_GRAPH", InitGraphChanges },
					{ "FRIEND_GUILDS", InitItems<RazorEnhanced.Friend.FriendGuild> },
					{ "FRIEND_LISTS", InitFriendsList },
					{ "FRIEND_PLAYERS", InitItems<RazorEnhanced.Friend.FriendPlayer> },
					{ "GENERAL", InitGeneralSettings },
					{ "HOTKEYS", InitHotKeys },
					{ "ORGANIZER_ITEMS", InitItems<RazorEnhanced.Organizer.OrganizerItem> },
					{ "ORGANIZER_LISTS", InitOrganizerLists },
					{ "PASSWORD", InitPasswords },
					{ "RESTOCK_ITEMS", InitItems<RazorEnhanced.Restock.RestockItem> },
					{ "RESTOCK_LISTS", InitRestockLists },
					{ "SCAVENGER_ITEMS", InitItems<RazorEnhanced.Scavenger.ScavengerItem> },
					{ "SCAVENGER_LISTS", InitScavengerLists },
					{ "SCRIPTING", InitScripting },
					{ "SELL_ITEMS", InitItems<RazorEnhanced.SellAgent.SellAgentItem> },
					{ "SELL_LISTS", InitSellAgentLists },
					{ "SPELLGRID_ITEMS", InitSpellGrid },
					{ "TARGETS", InitItems<TargetGUI> },
					{ "TOOLBAR_ITEMS", InitToolbarItems }
				};
		internal static Dictionary<string, loadFN> loadDict = new Dictionary<string, loadFN>()
				{
					{ "AUTOLOOT_ITEMS", LoadItems<RazorEnhanced.AutoLoot.AutoLootItem> },
					{ "BUY_ITEMS", LoadItems<RazorEnhanced.BuyAgent.BuyAgentItem> },
					{ "DRESS_ITEMS", LoadItems<RazorEnhanced.Dress.DressItemNew> },
					{ "FRIEND_GUILDS", LoadItems<RazorEnhanced.Friend.FriendGuild> },
					{ "FRIEND_PLAYERS", LoadItems<RazorEnhanced.Friend.FriendPlayer> },
					{ "ORGANIZER_ITEMS", LoadItems<RazorEnhanced.Organizer.OrganizerItem> },
					{ "PASSWORD", LoadPasswords },
					{ "RESTOCK_ITEMS", LoadItems<RazorEnhanced.Restock.RestockItem> },
					{ "SCAVENGER_ITEMS", LoadItems<RazorEnhanced.Scavenger.ScavengerItem> },
                    { "SCRIPTING", LoadScripting },
                    { "SELL_ITEMS", LoadItems<RazorEnhanced.SellAgent.SellAgentItem> },
					{ "TARGETS", LoadItems<TargetGUI> },
		};
		internal static Dictionary<string, saveFN> saveDict = new Dictionary<string, saveFN>()
				{
					{ "AUTOLOOT_ITEMS", SaveItems<RazorEnhanced.AutoLoot.AutoLootItem> },
					{ "BUY_ITEMS", SaveItems<RazorEnhanced.BuyAgent.BuyAgentItem> },
					{ "DRESS_ITEMS", SaveItems<RazorEnhanced.Dress.DressItemNew> },
					{ "FRIEND_GUILDS", SaveItems<RazorEnhanced.Friend.FriendGuild> },
					{ "FRIEND_PLAYERS", SaveItems<RazorEnhanced.Friend.FriendPlayer> },
					{ "ORGANIZER_ITEMS", SaveItems<RazorEnhanced.Organizer.OrganizerItem> },
					{ "PASSWORD", SavePasswords },
					{ "RESTOCK_ITEMS", SaveItems<RazorEnhanced.Restock.RestockItem> },
					{ "SCAVENGER_ITEMS", SaveItems<RazorEnhanced.Scavenger.ScavengerItem> },
                    { "SCRIPTING", SaveScripting },
					{ "SELL_ITEMS", SaveItems<RazorEnhanced.SellAgent.SellAgentItem> },
					{ "TARGETS", SaveItems<TargetGUI> },
		};

		internal static bool LoadExistingData(string profileName, bool try_recover = true)
		{
			string profileFilename = Path.Combine(Assistant.Engine.RootPath, "Profiles", profileName, "RazorEnhanced.settings");
			string serverFilename = Path.Combine(Assistant.Engine.RootPath, "Profiles", "RazorEnhanced");
			if (!File.Exists(profileFilename + ".GENERAL"))
			{
				return false;
			}

            string workingOnTableName = "None Yet";
			try
			{
				foreach (string tableName in initDict.Keys)
				{
                    workingOnTableName = tableName;
					// PASSWORD moved to server area, so have to check both
					if (File.Exists(profileFilename + "." + tableName) || File.Exists(serverFilename + "." + tableName))
					{
						DataTable temp = null;
						if (loadDict.ContainsKey(tableName))
						{
							temp = loadDict[tableName](profileFilename, tableName);
						}
						else
						{
							temp = Newtonsoft.Json.JsonConvert.DeserializeObject<DataTable>(File.ReadAllText(profileFilename + "." + tableName));
						}
						if (temp.Columns.Count == 0)
							if (initDict.ContainsKey(tableName))
							{
								temp = initDict[tableName](tableName);
							}
							else
							{
								// Something BAD
								throw new Exception("Table name to init function mismatched");
							}
						temp.TableName = tableName;
						m_Dataset.Tables.Add(temp);
					}
					else
					{
						if (initDict.ContainsKey(tableName))
						{
							DataTable temp = initDict[tableName](tableName);
							temp.TableName = tableName;
							m_Dataset.Tables.Add(temp);
						}
						else
						{
							// Something BAD
							throw new Exception("Table name to init function mismatched");
						}

					}
				}

				MakeBackup(profileName);
			}
			catch (Exception e)
			{
				if (try_recover == true)
				{
					MessageBox.Show("Error loading " + profileName + " {"+ workingOnTableName+"}"+", Trying to restore from backup");
					Settings.RestoreBackup(profileName);
					Load(profileName, false);
				}
				else
				{
					throw;
				}
			}

			// Version check, Permette update delle tabelle anche se gia esistenti
			DataRow versionrow = m_Dataset.Tables["GENERAL"].Rows[0];
			int currentVersion = 0;

			try
			{
				currentVersion = Convert.ToInt32(versionrow["SettingVersion"]);
			}
			catch
			{
				DataTable general = m_Dataset.Tables["GENERAL"];
				general.Columns.Add("SettingVersion", typeof(int));
				DataRow row = m_Dataset.Tables["GENERAL"].Rows[0];
				row["SettingVersion"] = 1;
				currentVersion = 1;
			}

			UpdateVersion(currentVersion);
			return true;

		}
        private static DataTable LoadScripting(string filename, string tableName)
        {
            List<RazorEnhanced.Scripts.ScriptItem> scriptItems = Newtonsoft.Json.JsonConvert.DeserializeObject<List<RazorEnhanced.Scripts.ScriptItem>>(File.ReadAllText(filename + "." + tableName));
            DataTable temp = initDict[tableName](tableName);
            foreach (RazorEnhanced.Scripts.ScriptItem item in scriptItems)
            {
                string fullpath = Path.Combine(Assistant.Engine.RootPath, "Scripts", item.Filename);
                if (File.Exists(fullpath))
                {
                    DataRow row = temp.NewRow();
                    row["Filename"] = item.Filename;
                    //row["Flag"]  UNUSED
                    row["Status"] = item.Status;
                    row["Loop"] = item.Loop;
                    row["Wait"] = item.Wait;
                    row["HotKey"] = item.Hotkey;
                    row["HotKeyPass"] = item.HotKeyPass;
                    row["AutoStart"] = item.AutoStart;
                    temp.Rows.Add(row);
                }
                else
                {
                    // drop it from scripts as it doesn't exist
                }

        }
            return temp;
        }
            internal static DataTable InitItems<T>(string tableName) where T : ListAbleItem
		{
			DataTable items = new DataTable(tableName);
			items.Columns.Add("List", typeof(string));
			items.Columns.Add("Item", typeof(T));
			return (items);
		}
		private static DataTable LoadItems<T>(string filename, string tableName) where T : ListAbleItem
		{
			List<T> items = Newtonsoft.Json.JsonConvert.DeserializeObject<List<T>>(File.ReadAllText(filename + "." + tableName));
			DataTable temp = initDict[tableName](tableName);
			foreach (T item in items)
			{
				DataRow row = temp.NewRow();
				row["List"] = item.List;
				row["Item"] = item;
				temp.Rows.Add(row);
			}
			return temp;
		}

        //
        private static void SaveScripting(string filename, string tableName, DataTable targets)
        {
            List<RazorEnhanced.Scripts.ScriptItem> items = new List<RazorEnhanced.Scripts.ScriptItem>();
            foreach (DataRow row in targets.Rows)
            {
                RazorEnhanced.Scripts.ScriptItem item = new RazorEnhanced.Scripts.ScriptItem();
                item.Filename = Convert.ToString(row["Filename"]);
                item.Status = Convert.ToString(row["Status"]);
                item.Loop = Convert.ToBoolean(row["Loop"]);
                item.Wait = Convert.ToBoolean(row["Wait"]);
                item.Hotkey = (System.Windows.Forms.Keys)(row["HotKey"]);
                item.HotKeyPass = Convert.ToBoolean(row["HotKeyPass"]);
                item.AutoStart = Convert.ToBoolean(row["AutoStart"]);
                items.Add(item);
            }
            string xml = Newtonsoft.Json.JsonConvert.SerializeObject(items, Newtonsoft.Json.Formatting.Indented);
            File.WriteAllText(filename + "." + tableName, xml);
        }
        private static void SaveItems<T>(string filename, string tableName, DataTable targets) where T : ListAbleItem
		{
			List<T> items = new List<T>();
			foreach (DataRow row in targets.Rows)
			{
				T item = (T)row["Item"];
				if (row["List"] is System.DBNull)
				{
					item.List = "None";
				}
				else {
					item.List = (string)row["List"];
				}
				items.Add(item);
			}

			string xml = Newtonsoft.Json.JsonConvert.SerializeObject(items, Newtonsoft.Json.Formatting.Indented);
			//	File.WriteAllText(filename + '.' + table.TableName, Newtonsoft.Json.JsonConvert.SerializeObject(targets, Newtonsoft.Json.Formatting.Indented));
			File.WriteAllText(filename + "." + tableName, xml);

		}
		///

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
            else {
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
			catch (Exception e)
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
			catch (Exception e)
			{
				return "";
			}
		}
		internal static DataTable InitPasswords(string tableName)
		{
			DataTable password = new DataTable(tableName);
			password.Columns.Add("IP", typeof(string));
			password.Columns.Add("User", typeof(string));
			password.Columns.Add("Password", typeof(string));

			return password;
		}
		private class PasswordStorage {
			public string IP;
			public string User;
			public string Password;
		}

		//string pw = Security.FingerPrint.Value();
		internal static DataTable LoadPasswords(string in_profile, string tableName)
		{
			string filename = Path.Combine(Assistant.Engine.RootPath, "Profiles", "RazorEnhanced." + tableName.ToLower());

			// ensure we load passwords from top level of profile
			// all this cleanup code can go away by June of 2020
			if (!File.Exists(filename))
			{
				// This maybe be a pre 0.7.2 directory so I can maybe move the file from profile
				string tempName = in_profile + "." + tableName;
				if (File.Exists(tempName))
				{
					File.Move(tempName, filename);
				}

			}
			foreach (string subDir in Directory.GetDirectories(Path.Combine(Assistant.Engine.RootPath, "Profiles")))
			{
				string passwordFilename = Path.Combine(subDir, "RazorEnhanced.settings.PASSWORD");
				if (File.Exists(passwordFilename))
				{
					File.Delete(passwordFilename);
				}
			}

			List <PasswordStorage> items = Newtonsoft.Json.JsonConvert.DeserializeObject<List<PasswordStorage>>(File.ReadAllText(filename));
			DataTable temp = initDict[tableName](tableName);
			foreach (PasswordStorage item in items)
			{
				DataRow row = temp.NewRow();
				row["IP"] = item.IP;
				row["User"] = item.User;
				row["Password"] = Unprotect(item.Password);
				temp.Rows.Add(row);
			}
			return temp;
		}
		internal static void SavePasswords(string not_used, string tableName, DataTable table)
		{
			// ensure we save passwords to top level of profile
			string filename = Path.Combine(Assistant.Engine.RootPath, "Profiles", "RazorEnhanced");

			List<PasswordStorage> items = new List<PasswordStorage>();
			foreach (DataRow row in table.Rows)
			{
				PasswordStorage item = new PasswordStorage();
				item.IP = (string)row["IP"];
				item.User = (string)row["User"];
				item.Password = Protect((string)row["Password"]);
				items.Add(item);
			}
			File.WriteAllText(filename + '.' + table.TableName.ToLower(), Newtonsoft.Json.JsonConvert.SerializeObject(items, Newtonsoft.Json.Formatting.Indented));


			// ----------- SAVE PASSWORD ----------
			//DataTable password = new DataTable(tableName);
			//password.Columns.Add("IP", typeof(string));
			//password.Columns.Add("User", typeof(string));
			//password.Columns.Add("Password", typeof(string));
			//return password;
		}

		// Scripting
		internal static DataTable InitScripting(string tableName)
		{
			// Scripting
			DataTable scripting = new DataTable(tableName);
			scripting.Columns.Add("Filename", typeof(string));
			scripting.Columns.Add("Flag", typeof(Bitmap));    // note appears unused
			scripting.Columns.Add("Status", typeof(string));
			scripting.Columns.Add("Loop", typeof(bool));
			scripting.Columns.Add("Wait", typeof(bool));
			scripting.Columns.Add("HotKey", typeof(Keys));
			scripting.Columns.Add("HotKeyPass", typeof(bool));
			scripting.Columns.Add("AutoStart", typeof(bool));
			return scripting;

		}

		internal static DataTable InitAutoLootLists(string tableName)
		{
			// -------- AUTOLOOT ------------
			DataTable autoloot_lists = new DataTable(tableName);
			autoloot_lists.Columns.Add("Description", typeof(string));
			autoloot_lists.Columns.Add("Delay", typeof(int));
			autoloot_lists.Columns.Add("Range", typeof(int));
			autoloot_lists.Columns.Add("Bag", typeof(int));
			autoloot_lists.Columns.Add("Selected", typeof(bool));
			autoloot_lists.Columns.Add("NoOpenCorpse", typeof(bool));
			return (autoloot_lists);
		}


		internal static DataTable InitScavengerLists(string tableName)
		{
			// ----------- SCAVENGER ----------
			DataTable scavenger_lists = new DataTable(tableName);
			scavenger_lists.Columns.Add("Description", typeof(string));
			scavenger_lists.Columns.Add("Delay", typeof(int));
			scavenger_lists.Columns.Add("Range", typeof(int));
			scavenger_lists.Columns.Add("Bag", typeof(int));
			scavenger_lists.Columns.Add("Selected", typeof(bool));
			return scavenger_lists;
		}
		internal static DataTable InitOrganizerLists(string tableName)
		{
			// ----------- ORGANIZER ----------
			DataTable organizer_lists = new DataTable(tableName);
			organizer_lists.Columns.Add("Description", typeof(string));
			organizer_lists.Columns.Add("Delay", typeof(int));
			organizer_lists.Columns.Add("Source", typeof(int));
			organizer_lists.Columns.Add("Destination", typeof(int));
			organizer_lists.Columns.Add("Selected", typeof(bool));
			return organizer_lists;
		}

		internal static DataTable InitSellAgentLists(string tableName)
		{               // ----------- SELL AGENT ----------
			DataTable sell_lists = new DataTable(tableName);
			sell_lists.Columns.Add("Description", typeof(string));
			sell_lists.Columns.Add("Bag", typeof(int));
			sell_lists.Columns.Add("Selected", typeof(bool));
			return sell_lists;
		}
		internal static DataTable InitBuyAgentLists(string tableName)
		{               // ----------- BUY AGENT ----------
			DataTable buy_lists = new DataTable(tableName);
			buy_lists.Columns.Add("Description", typeof(string));
			buy_lists.Columns.Add("CompareName", typeof(bool));
			buy_lists.Columns.Add("Selected", typeof(bool));
			return buy_lists;
		}
		internal static DataTable InitDressingAgentLists(string tableName)
		{
			// ----------- DRESS ----------
			DataTable dress_lists = new DataTable(tableName);
			dress_lists.Columns.Add("Description", typeof(string));
			dress_lists.Columns.Add("Bag", typeof(int));
			dress_lists.Columns.Add("Delay", typeof(int));
			dress_lists.Columns.Add("Conflict", typeof(bool));
			dress_lists.Columns.Add("Selected", typeof(bool));
			dress_lists.Columns.Add("HotKey", typeof(Keys));
			dress_lists.Columns.Add("HotKeyPass", typeof(bool));
			return dress_lists;
		}
		internal static DataTable InitDressingAgent(string tableName)
		{
			DataTable dress_items = new DataTable(tableName);
			dress_items.Columns.Add("List", typeof(string));
			dress_items.Columns.Add("Item", typeof(RazorEnhanced.Dress.DressItemNew));
			return dress_items;

		}
		internal static DataTable InitFriendsList(string tableName)
		{
			// ----------- FRIEND ----------
			DataTable friend_lists = new DataTable(tableName);
			friend_lists.Columns.Add("Description", typeof(string));
			friend_lists.Columns.Add("IncludeParty", typeof(bool));
			friend_lists.Columns.Add("PreventAttack", typeof(bool));
			friend_lists.Columns.Add("AutoacceptParty", typeof(bool));
			friend_lists.Columns.Add("SLFrinedCheckBox", typeof(bool));
			friend_lists.Columns.Add("TBFrinedCheckBox", typeof(bool));
			friend_lists.Columns.Add("COMFrinedCheckBox", typeof(bool));
			friend_lists.Columns.Add("MINFrinedCheckBox", typeof(bool));
			friend_lists.Columns.Add("Selected", typeof(bool));
			return friend_lists;
		}
		internal static DataTable InitPlayerFriends(string tableName)
		{
			DataTable friend_player = new DataTable(tableName);
			friend_player.Columns.Add("List", typeof(string));
			friend_player.Columns.Add("Item", typeof(RazorEnhanced.Friend.FriendPlayer));
			return friend_player;
		}
		internal static DataTable InitGuildFriends(string tableName)
		{
			DataTable friend_guild = new DataTable(tableName);
			friend_guild.Columns.Add("List", typeof(string));
			friend_guild.Columns.Add("Item", typeof(RazorEnhanced.Friend.FriendGuild));
			return friend_guild;

		}
		internal static DataTable InitRestockLists(string tableName)
		{
			// ----------- RESTOCK ----------
			DataTable restock_lists = new DataTable(tableName);
			restock_lists.Columns.Add("Description", typeof(string));
			restock_lists.Columns.Add("Delay", typeof(int));
			restock_lists.Columns.Add("Source", typeof(int));
			restock_lists.Columns.Add("Destination", typeof(int));
			restock_lists.Columns.Add("Selected", typeof(bool));
			return restock_lists;
		}
		internal static DataTable InitRestock(string tableName)
		{
			DataTable restock_items = new DataTable(tableName);
			restock_items.Columns.Add("List", typeof(string));
			restock_items.Columns.Add("Item", typeof(RazorEnhanced.Restock.RestockItem));
			return restock_items;

		}
		internal static DataTable InitGraphChanges(string tableName)
		{
			// ----------- FILTER GRAPH CHANGE ----------
			DataTable filter_graph = new DataTable(tableName);
			filter_graph.Columns.Add("Selected", typeof(bool));
			filter_graph.Columns.Add("GraphReal", typeof(int));
			filter_graph.Columns.Add("GraphNew", typeof(int));
			filter_graph.Columns.Add("ColorNew", typeof(int));
			return filter_graph;

		}
		internal static DataTable InitToolbarItems(string tableName)
		{
			// ----------- TOOLBAR ITEM ----------
			DataTable toolbar_items = new DataTable(tableName);
			toolbar_items.Columns.Add("Name", typeof(string));
			toolbar_items.Columns.Add("Graphics", typeof(int));
			toolbar_items.Columns.Add("Color", typeof(int));
			toolbar_items.Columns.Add("Warning", typeof(bool));
			toolbar_items.Columns.Add("WarningLimit", typeof(int));


			for (int i = 0; i < 60; i++)  // Popolo di slot vuoti al primo avvio
			{
				DataRow emptytoolbar = toolbar_items.NewRow();
				//RazorEnhanced.ToolBar.ToolBarItem emptyitem = new RazorEnhanced.ToolBar.ToolBarItem("Empty", 0x0000, 0x0000, false, 0);
				emptytoolbar.ItemArray = new object[] { "Empty", 0x0000, 0x0000, false, 0 };
				toolbar_items.Rows.Add(emptytoolbar);
			}
			return toolbar_items;

		}
		internal static DataTable InitSpellGrid(string tableName)
		{
			// ----------- SPELLGRID ITEM ----------
			DataTable spellgrid_items = new DataTable(tableName);
			spellgrid_items.Columns.Add("Group", typeof(string));
			spellgrid_items.Columns.Add("Spell", typeof(string));
			spellgrid_items.Columns.Add("Color", typeof(string));

			for (int i = 0; i < 100; i++)  // Popolo di slot vuoti al primo avvio
			{
				DataRow emptygrid = spellgrid_items.NewRow();
				//RazorEnhanced.SpellGrid.SpellGridItem emptyitem = new RazorEnhanced.SpellGrid.SpellGridItem("Empty", "Empty", Color.Transparent, Color.Transparent);
				//RazorEnhanced.SpellGrid.SpellGridItem item = new RazorEnhanced.SpellGrid.SpellGridItem(group, spell, border, Color.Transparent);
				emptygrid.ItemArray = new object[] { "Empty", "Empty", Color.Transparent };
				spellgrid_items.Rows.Add(emptygrid);
			}
			return spellgrid_items;
		}


		internal static DataTable InitHotKeys(string tableName)
		{
			// ----------- HOTKEYS ----------
			DataTable hotkey = new DataTable(tableName);
			hotkey.Columns.Add("Group", typeof(string));
			hotkey.Columns.Add("Name", typeof(string));
			hotkey.Columns.Add("Key", typeof(Keys));
			hotkey.Columns.Add("Pass", typeof(bool));

			// Parametri primo avvio HotKey
			DataRow hotkeyrow = hotkey.NewRow();
			hotkeyrow.ItemArray = new object[] { "General", "Resync", Keys.None, true };
			hotkey.Rows.Add(hotkeyrow);

			hotkeyrow = hotkey.NewRow();
			hotkeyrow.ItemArray = new object[] { "General", "Take Screen Shot", Keys.None, true };
			hotkey.Rows.Add(hotkeyrow);

			hotkeyrow = hotkey.NewRow();
			hotkeyrow.ItemArray = new object[] { "General", "Start Video Record", Keys.None, true };
			hotkey.Rows.Add(hotkeyrow);

			hotkeyrow = hotkey.NewRow();
			hotkeyrow.ItemArray = new object[] { "General", "Stop Video Record", Keys.None, true };
			hotkey.Rows.Add(hotkeyrow);

			hotkeyrow = hotkey.NewRow();
			hotkeyrow.ItemArray = new object[] { "General", "Ping Server", Keys.None, true };
			hotkey.Rows.Add(hotkeyrow);

			hotkeyrow = hotkey.NewRow();
			hotkeyrow.ItemArray = new object[] { "General", "Accept Party", Keys.None, true };
			hotkey.Rows.Add(hotkeyrow);

			hotkeyrow = hotkey.NewRow();
			hotkeyrow.ItemArray = new object[] { "General", "Decline Party", Keys.None, true };
			hotkey.Rows.Add(hotkeyrow);

			hotkeyrow = hotkey.NewRow();
			hotkeyrow.ItemArray = new object[] { "General", "DPS Meter Start", Keys.None, true };
			hotkey.Rows.Add(hotkeyrow);

			hotkeyrow = hotkey.NewRow();
			hotkeyrow.ItemArray = new object[] { "General", "DPS Meter Pause / Resume", Keys.None, true };
			hotkey.Rows.Add(hotkeyrow);

			hotkeyrow = hotkey.NewRow();
			hotkeyrow.ItemArray = new object[] { "General", "DPS Meter Pause", Keys.None, true };
			hotkey.Rows.Add(hotkeyrow);

			hotkeyrow = hotkey.NewRow();
			hotkeyrow.ItemArray = new object[] { "General", "No Run Stealth ON/OFF", Keys.None, true };
			hotkey.Rows.Add(hotkeyrow);

			hotkeyrow = hotkey.NewRow();
			hotkeyrow.ItemArray = new object[] { "General", "Open Enhanced Map", Keys.None, true };
			hotkey.Rows.Add(hotkeyrow);

			hotkeyrow = hotkey.NewRow();
			hotkeyrow.ItemArray = new object[] { "General", "Inspect Item/Ground", Keys.None, true };
			hotkey.Rows.Add(hotkeyrow);

			hotkeyrow = hotkey.NewRow();
			hotkeyrow.ItemArray = new object[] { "Actions", "Grab Item", Keys.None, true };
			hotkey.Rows.Add(hotkeyrow);

			hotkeyrow = hotkey.NewRow();
			hotkeyrow.ItemArray = new object[] { "Actions", "Drop Item", Keys.None, true };
			hotkey.Rows.Add(hotkeyrow);

			hotkeyrow = hotkey.NewRow();
			hotkeyrow.ItemArray = new object[] { "Actions", "Fly ON/OFF", Keys.None, true };
			hotkey.Rows.Add(hotkeyrow);

			hotkeyrow = hotkey.NewRow();
			hotkeyrow.ItemArray = new object[] { "Actions", "Hide Item", Keys.None, true };
			hotkey.Rows.Add(hotkeyrow);

			hotkeyrow = hotkey.NewRow();
			hotkeyrow.ItemArray = new object[] { "Use", "Last Item", Keys.None, true };
			hotkey.Rows.Add(hotkeyrow);

			hotkeyrow = hotkey.NewRow();
			hotkeyrow.ItemArray = new object[] { "Use", "Left Hand", Keys.None, true };
			hotkey.Rows.Add(hotkeyrow);

			hotkeyrow = hotkey.NewRow();
			hotkeyrow.ItemArray = new object[] { "Use", "Right Hand", Keys.None, true };
			hotkey.Rows.Add(hotkeyrow);

			hotkeyrow = hotkey.NewRow();
			hotkeyrow.ItemArray = new object[] { "Show Names", "All", Keys.None, true };
			hotkey.Rows.Add(hotkeyrow);

			hotkeyrow = hotkey.NewRow();
			hotkeyrow.ItemArray = new object[] { "Show Names", "Corpses", Keys.None, true };
			hotkey.Rows.Add(hotkeyrow);

			hotkeyrow = hotkey.NewRow();
			hotkeyrow.ItemArray = new object[] { "Show Names", "Mobiles", Keys.None, true };
			hotkey.Rows.Add(hotkeyrow);

			hotkeyrow = hotkey.NewRow();
			hotkeyrow.ItemArray = new object[] { "Show Names", "Items", Keys.None, true };
			hotkey.Rows.Add(hotkeyrow);

			hotkeyrow = hotkey.NewRow();
			hotkeyrow.ItemArray = new object[] { "Pet Commands", "All Come", Keys.None, true };
			hotkey.Rows.Add(hotkeyrow);

			hotkeyrow = hotkey.NewRow();
			hotkeyrow.ItemArray = new object[] { "Pet Commands", "All Follow Me", Keys.None, true };
			hotkey.Rows.Add(hotkeyrow);

			hotkeyrow = hotkey.NewRow();
			hotkeyrow.ItemArray = new object[] { "Pet Commands", "All Follow", Keys.None, true };
			hotkey.Rows.Add(hotkeyrow);

			hotkeyrow = hotkey.NewRow();
			hotkeyrow.ItemArray = new object[] { "Pet Commands", "All Guard Me", Keys.None, true };
			hotkey.Rows.Add(hotkeyrow);

			hotkeyrow = hotkey.NewRow();
			hotkeyrow.ItemArray = new object[] { "Pet Commands", "All Guard", Keys.None, true };
			hotkey.Rows.Add(hotkeyrow);

			hotkeyrow = hotkey.NewRow();
			hotkeyrow.ItemArray = new object[] { "Pet Commands", "All Kill", Keys.None, true };
			hotkey.Rows.Add(hotkeyrow);

			hotkeyrow = hotkey.NewRow();
			hotkeyrow.ItemArray = new object[] { "Pet Commands", "All Stay", Keys.None, true };
			hotkey.Rows.Add(hotkeyrow);

			hotkeyrow = hotkey.NewRow();
			hotkeyrow.ItemArray = new object[] { "Pet Commands", "All Stop", Keys.None, true };
			hotkey.Rows.Add(hotkeyrow);

			hotkeyrow = hotkey.NewRow();
			hotkeyrow.ItemArray = new object[] { "Pet Commands", "Mount", Keys.None, true };
			hotkey.Rows.Add(hotkeyrow);

			hotkeyrow = hotkey.NewRow();
			hotkeyrow.ItemArray = new object[] { "Pet Commands", "Dismount", Keys.None, true };
			hotkey.Rows.Add(hotkeyrow);

			hotkeyrow = hotkey.NewRow();
			hotkeyrow.ItemArray = new object[] { "Pet Commands", "Mount / Dismount", Keys.None, true };
			hotkey.Rows.Add(hotkeyrow);

			// Autoloot agent
			hotkeyrow = hotkey.NewRow();
			hotkeyrow.ItemArray = new object[] { "AgentAutoloot", "Autoloot Trigger ON/OFF", Keys.None, true };
			hotkey.Rows.Add(hotkeyrow);

			hotkeyrow = hotkey.NewRow();
			hotkeyrow.ItemArray = new object[] { "AgentAutoloot", "Autoloot Set Bag", Keys.None, true };
			hotkey.Rows.Add(hotkeyrow);

			hotkeyrow = hotkey.NewRow();
			hotkeyrow.ItemArray = new object[] { "AgentAutoloot", "Autoloot Add Item", Keys.None, true };
			hotkey.Rows.Add(hotkeyrow);

			// Scavenger agent
			hotkeyrow = hotkey.NewRow();
			hotkeyrow.ItemArray = new object[] { "AgentScavenger", "Scavenger Trigger ON/OFF", Keys.None, true };
			hotkey.Rows.Add(hotkeyrow);

			hotkeyrow = hotkey.NewRow();
			hotkeyrow.ItemArray = new object[] { "AgentScavenger", "Scavenger Set Bag", Keys.None, true };
			hotkey.Rows.Add(hotkeyrow);

			hotkeyrow = hotkey.NewRow();
			hotkeyrow.ItemArray = new object[] { "AgentScavenger", "Scavenger Add Item", Keys.None, true };
			hotkey.Rows.Add(hotkeyrow);

			// Organizer Agent
			hotkeyrow = hotkey.NewRow();
			hotkeyrow.ItemArray = new object[] { "AgentOrganizer", "Organizer Start", Keys.None, true };
			hotkey.Rows.Add(hotkeyrow);

			hotkeyrow = hotkey.NewRow();
			hotkeyrow.ItemArray = new object[] { "AgentOrganizer", "Organizer Stop", Keys.None, true };
			hotkey.Rows.Add(hotkeyrow);

			hotkeyrow = hotkey.NewRow();
			hotkeyrow.ItemArray = new object[] { "AgentOrganizer", "Organizer Set Soruce Bag", Keys.None, true };
			hotkey.Rows.Add(hotkeyrow);

			hotkeyrow = hotkey.NewRow();
			hotkeyrow.ItemArray = new object[] { "AgentOrganizer", "Organizer Set Destination Bag", Keys.None, true };
			hotkey.Rows.Add(hotkeyrow);

			hotkeyrow = hotkey.NewRow();
			hotkeyrow.ItemArray = new object[] { "AgentOrganizer", "Organizer Add Item", Keys.None, true };
			hotkey.Rows.Add(hotkeyrow);

			// Sell Agent
			hotkeyrow = hotkey.NewRow();
			hotkeyrow.ItemArray = new object[] { "AgentSell", "Sell Trigger ON/OFF", Keys.None, true };
			hotkey.Rows.Add(hotkeyrow);

			hotkeyrow = hotkey.NewRow();
			hotkeyrow.ItemArray = new object[] { "AgentSell", "Sell Set Soruce Bag", Keys.None, true };
			hotkey.Rows.Add(hotkeyrow);

			// Buy Agent
			hotkeyrow = hotkey.NewRow();
			hotkeyrow.ItemArray = new object[] { "AgentBuy", "Buy Trigger ON/OFF", Keys.None, true };
			hotkey.Rows.Add(hotkeyrow);

			// Dress agent
			hotkeyrow = hotkey.NewRow();
			hotkeyrow.ItemArray = new object[] { "AgentDress", "Dress Start", Keys.None, true };
			hotkey.Rows.Add(hotkeyrow);

			hotkeyrow = hotkey.NewRow();
			hotkeyrow.ItemArray = new object[] { "AgentDress", "Undress Start", Keys.None, true };
			hotkey.Rows.Add(hotkeyrow);

			hotkeyrow = hotkey.NewRow();
			hotkeyrow.ItemArray = new object[] { "AgentDress", "Dress / Undress Stop", Keys.None, true };
			hotkey.Rows.Add(hotkeyrow);

			// Restock agent
			hotkeyrow = hotkey.NewRow();
			hotkeyrow.ItemArray = new object[] { "AgentRestock", "Restock Start", Keys.None, true };
			hotkey.Rows.Add(hotkeyrow);

			hotkeyrow = hotkey.NewRow();
			hotkeyrow.ItemArray = new object[] { "AgentRestock", "Restock Stop", Keys.None, true };
			hotkey.Rows.Add(hotkeyrow);

			hotkeyrow = hotkey.NewRow();
			hotkeyrow.ItemArray = new object[] { "AgentRestock", "Restock Set Soruce Bag", Keys.None, true };
			hotkey.Rows.Add(hotkeyrow);

			hotkeyrow = hotkey.NewRow();
			hotkeyrow.ItemArray = new object[] { "AgentRestock", "Restock Set Destination Bag", Keys.None, true };
			hotkey.Rows.Add(hotkeyrow);

			hotkeyrow = hotkey.NewRow();
			hotkeyrow.ItemArray = new object[] { "AgentRestock", "Restock Add Item", Keys.None, true };
			hotkey.Rows.Add(hotkeyrow);

			// Bandage Heal agent
			hotkeyrow = hotkey.NewRow();
			hotkeyrow.ItemArray = new object[] { "AgentBandage", "Bandage Heal Trigger ON/OFF", Keys.None, true };
			hotkey.Rows.Add(hotkeyrow);

			// BoneCutter Agent
			hotkeyrow = hotkey.NewRow();
			hotkeyrow.ItemArray = new object[] { "AgentBoneCutter", "Bone Cutter Trigger ON/OFF", Keys.None, true };
			hotkey.Rows.Add(hotkeyrow);

			hotkeyrow = hotkey.NewRow();
			hotkeyrow.ItemArray = new object[] { "AgentBoneCutter", "Bone Cutter Set Blade", Keys.None, true };
			hotkey.Rows.Add(hotkeyrow);

			// AutoCarver Agent
			hotkeyrow = hotkey.NewRow();
			hotkeyrow.ItemArray = new object[] { "AgentAutoCarver", "Auto Carver Trigger ON/OFF", Keys.None, true };
			hotkey.Rows.Add(hotkeyrow);

			hotkeyrow = hotkey.NewRow();
			hotkeyrow.ItemArray = new object[] { "AgentAutoCarver", "Auto Carver Set Blade", Keys.None, true };
			hotkey.Rows.Add(hotkeyrow);

			// AutoRemount Agent
			hotkeyrow = hotkey.NewRow();
			hotkeyrow.ItemArray = new object[] { "AgentAutoRemount", "Auto Remount Trigger ON/OFF", Keys.None, true };
			hotkey.Rows.Add(hotkeyrow);

			hotkeyrow = hotkey.NewRow();
			hotkeyrow.ItemArray = new object[] { "AgentAutoRemount", "Auto Remount Set Mount", Keys.None, true };
			hotkey.Rows.Add(hotkeyrow);

			// Graphics Filter
			hotkeyrow = hotkey.NewRow();
			hotkeyrow.ItemArray = new object[] { "AgentGraphFilter", "Graphic Filter Trigger ON/OFF", Keys.None, true };
			hotkey.Rows.Add(hotkeyrow);

			// Friend Agent
			hotkeyrow = hotkey.NewRow();
			hotkeyrow.ItemArray = new object[] { "AgentFriend", "Add Friend", Keys.None, true };
			hotkey.Rows.Add(hotkeyrow);


			hotkeyrow = hotkey.NewRow();
			hotkeyrow.ItemArray = new object[] { "Abilities", "Primary", Keys.None, true };
			hotkey.Rows.Add(hotkeyrow);

			hotkeyrow = hotkey.NewRow();
			hotkeyrow.ItemArray = new object[] { "Abilities", "Secondary", Keys.None, true };
			hotkey.Rows.Add(hotkeyrow);

			hotkeyrow = hotkey.NewRow();
			hotkeyrow.ItemArray = new object[] { "Abilities", "Cancel", Keys.None, true };
			hotkey.Rows.Add(hotkeyrow);

			hotkeyrow = hotkey.NewRow();
			hotkeyrow.ItemArray = new object[] { "Abilities", "Stun", Keys.None, true };
			hotkey.Rows.Add(hotkeyrow);

			hotkeyrow = hotkey.NewRow();
			hotkeyrow.ItemArray = new object[] { "Abilities", "Disarm", Keys.None, true };
			hotkey.Rows.Add(hotkeyrow);

			hotkeyrow = hotkey.NewRow();
			hotkeyrow.ItemArray = new object[] { "Abilities", "Primary ON/OFF", Keys.None, true };
			hotkey.Rows.Add(hotkeyrow);

			hotkeyrow = hotkey.NewRow();
			hotkeyrow.ItemArray = new object[] { "Abilities", "Secondary ON/OFF", Keys.None, true };
			hotkey.Rows.Add(hotkeyrow);

			hotkeyrow = hotkey.NewRow();
			hotkeyrow.ItemArray = new object[] { "Attack", "Attack Last Target", Keys.None, true };
			hotkey.Rows.Add(hotkeyrow);

			hotkeyrow = hotkey.NewRow();
			hotkeyrow.ItemArray = new object[] { "Attack", "Attack Last", Keys.None, true };
			hotkey.Rows.Add(hotkeyrow);

			hotkeyrow = hotkey.NewRow();
			hotkeyrow.ItemArray = new object[] { "Attack", "WarMode ON/OFF", Keys.None, true };
			hotkey.Rows.Add(hotkeyrow);

			hotkeyrow = hotkey.NewRow();
			hotkeyrow.ItemArray = new object[] { "Bandage", "Self", Keys.None, true };
			hotkey.Rows.Add(hotkeyrow);

			hotkeyrow = hotkey.NewRow();
			hotkeyrow.ItemArray = new object[] { "Bandage", "Last", Keys.None, true };
			hotkey.Rows.Add(hotkeyrow);

			hotkeyrow = hotkey.NewRow();
			hotkeyrow.ItemArray = new object[] { "Bandage", "Use Only", Keys.None, true };
			hotkey.Rows.Add(hotkeyrow);

			hotkeyrow = hotkey.NewRow();
			hotkeyrow.ItemArray = new object[] { "Potions", "Potion Agility", Keys.None, true };
			hotkey.Rows.Add(hotkeyrow);

			hotkeyrow = hotkey.NewRow();
			hotkeyrow.ItemArray = new object[] { "Potions", "Potion Cure", Keys.None, true };
			hotkey.Rows.Add(hotkeyrow);

			hotkeyrow = hotkey.NewRow();
			hotkeyrow.ItemArray = new object[] { "Potions", "Potion Explosion", Keys.None, true };
			hotkey.Rows.Add(hotkeyrow);

			hotkeyrow = hotkey.NewRow();
			hotkeyrow.ItemArray = new object[] { "Potions", "Potion Heal", Keys.None, true };
			hotkey.Rows.Add(hotkeyrow);

			hotkeyrow = hotkey.NewRow();
			hotkeyrow.ItemArray = new object[] { "Potions", "Potion Refresh", Keys.None, true };
			hotkey.Rows.Add(hotkeyrow);

			hotkeyrow = hotkey.NewRow();
			hotkeyrow.ItemArray = new object[] { "Potions", "Potion Strength", Keys.None, true };
			hotkey.Rows.Add(hotkeyrow);

			hotkeyrow = hotkey.NewRow();
			hotkeyrow.ItemArray = new object[] { "Potions", "Potion Nightsight", Keys.None, true };
			hotkey.Rows.Add(hotkeyrow);

			hotkeyrow = hotkey.NewRow();
			hotkeyrow.ItemArray = new object[] { "Potions", "Potion Shatter", Keys.None, true };
			hotkey.Rows.Add(hotkeyrow);

			hotkeyrow = hotkey.NewRow();
			hotkeyrow.ItemArray = new object[] { "Potions", "Potion Parasitic", Keys.None, true };
			hotkey.Rows.Add(hotkeyrow);

			hotkeyrow = hotkey.NewRow();
			hotkeyrow.ItemArray = new object[] { "Potions", "Potion Supernova", Keys.None, true };
			hotkey.Rows.Add(hotkeyrow);

			hotkeyrow = hotkey.NewRow();
			hotkeyrow.ItemArray = new object[] { "Potions", "Potion Confusion Blast", Keys.None, true };
			hotkey.Rows.Add(hotkeyrow);

			hotkeyrow = hotkey.NewRow();
			hotkeyrow.ItemArray = new object[] { "Potions", "Potion Conflagration", Keys.None, true };
			hotkey.Rows.Add(hotkeyrow);

			hotkeyrow = hotkey.NewRow();
			hotkeyrow.ItemArray = new object[] { "Potions", "Potion Invisibility", Keys.None, true };
			hotkey.Rows.Add(hotkeyrow);

			hotkeyrow = hotkey.NewRow();
			hotkeyrow.ItemArray = new object[] { "Potions", "Potion Exploding Tar", Keys.None, true };
			hotkey.Rows.Add(hotkeyrow);

			hotkeyrow = hotkey.NewRow();
			hotkeyrow.ItemArray = new object[] { "Potions", "Fear Essence", Keys.None, true };
			hotkey.Rows.Add(hotkeyrow);

			hotkeyrow = hotkey.NewRow();
			hotkeyrow.ItemArray = new object[] { "Potions", "Darkglow Poison", Keys.None, true };
			hotkey.Rows.Add(hotkeyrow);

			hotkeyrow = hotkey.NewRow();
			hotkeyrow.ItemArray = new object[] { "Potions", "Kurak Ambusher's Essence", Keys.None, true };
			hotkey.Rows.Add(hotkeyrow);

			hotkeyrow = hotkey.NewRow();
			hotkeyrow.ItemArray = new object[] { "Potions", "Potion Sakkhra Prophylaxis", Keys.None, true };
			hotkey.Rows.Add(hotkeyrow);

			hotkeyrow = hotkey.NewRow();
			hotkeyrow.ItemArray = new object[] { "Potions", "Jukari Burn Poultice", Keys.None, true };
			hotkey.Rows.Add(hotkeyrow);

			hotkeyrow = hotkey.NewRow();
			hotkeyrow.ItemArray = new object[] { "Potions", "Barako Draft Of Might", Keys.None, true };
			hotkey.Rows.Add(hotkeyrow);

			hotkeyrow = hotkey.NewRow();
			hotkeyrow.ItemArray = new object[] { "Potions", "Urali Trance Tonic", Keys.None, true };
			hotkey.Rows.Add(hotkeyrow);

			hotkeyrow = hotkey.NewRow();
			hotkeyrow.ItemArray = new object[] { "Other", "Enchanted Apple", Keys.None, true };
			hotkey.Rows.Add(hotkeyrow);

			hotkeyrow = hotkey.NewRow();
			hotkeyrow.ItemArray = new object[] { "Other", "Orange Petals", Keys.None, true };
			hotkey.Rows.Add(hotkeyrow);

			hotkeyrow = hotkey.NewRow();
			hotkeyrow.ItemArray = new object[] { "Other", "Wrath Grapes", Keys.None, true };
			hotkey.Rows.Add(hotkeyrow);

			hotkeyrow = hotkey.NewRow();
			hotkeyrow.ItemArray = new object[] { "Other", "Rose Of Trinsic", Keys.None, true };
			hotkey.Rows.Add(hotkeyrow);

			hotkeyrow = hotkey.NewRow();
			hotkeyrow.ItemArray = new object[] { "Other", "Smoke Bomb", Keys.None, true };
			hotkey.Rows.Add(hotkeyrow);

			hotkeyrow = hotkey.NewRow();
			hotkeyrow.ItemArray = new object[] { "Other", "Spell Stone", Keys.None, true };
			hotkey.Rows.Add(hotkeyrow);

			hotkeyrow = hotkey.NewRow();
			hotkeyrow.ItemArray = new object[] { "Other", "Healing Stone", Keys.None, true };
			hotkey.Rows.Add(hotkeyrow);

			hotkeyrow = hotkey.NewRow();
			hotkeyrow.ItemArray = new object[] { "Other", "Pouch", Keys.None, true };
			hotkey.Rows.Add(hotkeyrow);

			hotkeyrow = hotkey.NewRow();
			hotkeyrow.ItemArray = new object[] { "Hands", "Clear Left", Keys.None, true };
			hotkey.Rows.Add(hotkeyrow);

			hotkeyrow = hotkey.NewRow();
			hotkeyrow.ItemArray = new object[] { "Hands", "Clear Right", Keys.None, true };
			hotkey.Rows.Add(hotkeyrow);

			hotkeyrow = hotkey.NewRow();
			hotkeyrow.ItemArray = new object[] { "Hands", "Equip Right", Keys.None, true };
			hotkey.Rows.Add(hotkeyrow);

			hotkeyrow = hotkey.NewRow();
			hotkeyrow.ItemArray = new object[] { "Hands", "Equip Left", Keys.None, true };
			hotkey.Rows.Add(hotkeyrow);

			hotkeyrow = hotkey.NewRow();
			hotkeyrow.ItemArray = new object[] { "Hands", "Toggle Right", Keys.None, true };
			hotkey.Rows.Add(hotkeyrow);

			hotkeyrow = hotkey.NewRow();
			hotkeyrow.ItemArray = new object[] { "Hands", "Toggle Left", Keys.None, true };
			hotkey.Rows.Add(hotkeyrow);

			hotkeyrow = hotkey.NewRow();
			hotkeyrow.ItemArray = new object[] { "Equip Wands", "Wand Clumsy", Keys.None, true };
			hotkey.Rows.Add(hotkeyrow);

			hotkeyrow = hotkey.NewRow();
			hotkeyrow.ItemArray = new object[] { "Equip Wands", "Wand Identidication", Keys.None, true };
			hotkey.Rows.Add(hotkeyrow);

			hotkeyrow = hotkey.NewRow();
			hotkeyrow.ItemArray = new object[] { "Equip Wands", "Wand Heal", Keys.None, true };
			hotkey.Rows.Add(hotkeyrow);

			hotkeyrow = hotkey.NewRow();
			hotkeyrow.ItemArray = new object[] { "Equip Wands", "Wand Feebleming", Keys.None, true };
			hotkey.Rows.Add(hotkeyrow);

			hotkeyrow = hotkey.NewRow();
			hotkeyrow.ItemArray = new object[] { "Equip Wands", "Wand Weakness", Keys.None, true };
			hotkey.Rows.Add(hotkeyrow);

			hotkeyrow = hotkey.NewRow();
			hotkeyrow.ItemArray = new object[] { "Equip Wands", "Wand Magic Arrow", Keys.None, true };
			hotkey.Rows.Add(hotkeyrow);

			hotkeyrow = hotkey.NewRow();
			hotkeyrow.ItemArray = new object[] { "Equip Wands", "Wand Harm", Keys.None, true };
			hotkey.Rows.Add(hotkeyrow);

			hotkeyrow = hotkey.NewRow();
			hotkeyrow.ItemArray = new object[] { "Equip Wands", "Wand Fireball", Keys.None, true };
			hotkey.Rows.Add(hotkeyrow);

			hotkeyrow = hotkey.NewRow();
			hotkeyrow.ItemArray = new object[] { "Equip Wands", "Wand Greater Heal", Keys.None, true };
			hotkey.Rows.Add(hotkeyrow);

			hotkeyrow = hotkey.NewRow();
			hotkeyrow.ItemArray = new object[] { "Equip Wands", "Wand Lightning", Keys.None, true };
			hotkey.Rows.Add(hotkeyrow);

			hotkeyrow = hotkey.NewRow();
			hotkeyrow.ItemArray = new object[] { "Equip Wands", "Wand Mana Drain", Keys.None, true };
			hotkey.Rows.Add(hotkeyrow);

			hotkeyrow = hotkey.NewRow();
			hotkeyrow.ItemArray = new object[] { "Skills", "Last Used", Keys.None, true };
			hotkey.Rows.Add(hotkeyrow);

			hotkeyrow = hotkey.NewRow();
			hotkeyrow.ItemArray = new object[] { "Skills", "Animal Lore", Keys.None, true };
			hotkey.Rows.Add(hotkeyrow);

			hotkeyrow = hotkey.NewRow();
			hotkeyrow.ItemArray = new object[] { "Skills", "Item ID", Keys.None, true };
			hotkey.Rows.Add(hotkeyrow);

			hotkeyrow = hotkey.NewRow();
			hotkeyrow.ItemArray = new object[] { "Skills", "Arms Lore", Keys.None, true };
			hotkey.Rows.Add(hotkeyrow);

			hotkeyrow = hotkey.NewRow();
			hotkeyrow.ItemArray = new object[] { "Skills", "Begging", Keys.None, true };
			hotkey.Rows.Add(hotkeyrow);

			hotkeyrow = hotkey.NewRow();
			hotkeyrow.ItemArray = new object[] { "Skills", "Peacemaking", Keys.None, true };
			hotkey.Rows.Add(hotkeyrow);

			hotkeyrow = hotkey.NewRow();
			hotkeyrow.ItemArray = new object[] { "Skills", "Cartography", Keys.None, true };
			hotkey.Rows.Add(hotkeyrow);

			hotkeyrow = hotkey.NewRow();
			hotkeyrow.ItemArray = new object[] { "Skills", "Detect Hidden", Keys.None, true };
			hotkey.Rows.Add(hotkeyrow);

			hotkeyrow = hotkey.NewRow();
			hotkeyrow.ItemArray = new object[] { "Skills", "Eval Int", Keys.None, true };
			hotkey.Rows.Add(hotkeyrow);

			hotkeyrow = hotkey.NewRow();
			hotkeyrow.ItemArray = new object[] { "Skills", "Forensics", Keys.None, true };
			hotkey.Rows.Add(hotkeyrow);

			hotkeyrow = hotkey.NewRow();
			hotkeyrow.ItemArray = new object[] { "Skills", "Hiding", Keys.None, true };
			hotkey.Rows.Add(hotkeyrow);

			hotkeyrow = hotkey.NewRow();
			hotkeyrow.ItemArray = new object[] { "Skills", "Provocation", Keys.None, true };
			hotkey.Rows.Add(hotkeyrow);

			hotkeyrow = hotkey.NewRow();
			hotkeyrow.ItemArray = new object[] { "Skills", "Spirit Speak", Keys.None, true };
			hotkey.Rows.Add(hotkeyrow);

			hotkeyrow = hotkey.NewRow();
			hotkeyrow.ItemArray = new object[] { "Skills", "Stealing", Keys.None, true };
			hotkey.Rows.Add(hotkeyrow);

			hotkeyrow = hotkey.NewRow();
			hotkeyrow.ItemArray = new object[] { "Skills", "Animal Taming", Keys.None, true };
			hotkey.Rows.Add(hotkeyrow);

			hotkeyrow = hotkey.NewRow();
			hotkeyrow.ItemArray = new object[] { "Skills", "Taste ID", Keys.None, true };
			hotkey.Rows.Add(hotkeyrow);

			hotkeyrow = hotkey.NewRow();
			hotkeyrow.ItemArray = new object[] { "Skills", "Tracking", Keys.None, true };
			hotkey.Rows.Add(hotkeyrow);

			hotkeyrow = hotkey.NewRow();
			hotkeyrow.ItemArray = new object[] { "Skills", "Meditation", Keys.None, true };
			hotkey.Rows.Add(hotkeyrow);

			hotkeyrow = hotkey.NewRow();
			hotkeyrow.ItemArray = new object[] { "Skills", "Stealth", Keys.None, true };
			hotkey.Rows.Add(hotkeyrow);

			hotkeyrow = hotkey.NewRow();
			hotkeyrow.ItemArray = new object[] { "Skills", "RemoveTrap", Keys.None, true };
			hotkey.Rows.Add(hotkeyrow);

			hotkeyrow = hotkey.NewRow();
			hotkeyrow.ItemArray = new object[] { "Skills", "Inscribe", Keys.None, true };
			hotkey.Rows.Add(hotkeyrow);

			hotkeyrow = hotkey.NewRow();
			hotkeyrow.ItemArray = new object[] { "Skills", "Anatomy", Keys.None, true };
			hotkey.Rows.Add(hotkeyrow);

			hotkeyrow = hotkey.NewRow();
			hotkeyrow.ItemArray = new object[] { "Skills", "Discordance", Keys.None, true };
			hotkey.Rows.Add(hotkeyrow);

			hotkeyrow = hotkey.NewRow();
			hotkeyrow.ItemArray = new object[] { "Skills", "Imbuing", Keys.None, true };
			hotkey.Rows.Add(hotkeyrow);

			hotkeyrow = hotkey.NewRow();
			hotkeyrow.ItemArray = new object[] { "SpellsAgent", "Mini Heal", Keys.None, true };
			hotkey.Rows.Add(hotkeyrow);

			hotkeyrow = hotkey.NewRow();
			hotkeyrow.ItemArray = new object[] { "SpellsAgent", "Big Heal", Keys.None, true };
			hotkey.Rows.Add(hotkeyrow);

			hotkeyrow = hotkey.NewRow();
			hotkeyrow.ItemArray = new object[] { "SpellsAgent", "Chivarly Heal", Keys.None, true };
			hotkey.Rows.Add(hotkeyrow);

			hotkeyrow = hotkey.NewRow();
			hotkeyrow.ItemArray = new object[] { "SpellsAgent", "Interrupt", Keys.None, true };
			hotkey.Rows.Add(hotkeyrow);

			hotkeyrow = hotkey.NewRow();
			hotkeyrow.ItemArray = new object[] { "SpellsAgent", "Last Spell", Keys.None, true };
			hotkey.Rows.Add(hotkeyrow);

            hotkeyrow = hotkey.NewRow();
            hotkeyrow.ItemArray = new object[] { "SpellsAgent", "Last Spell Last Target", Keys.None, true };
            hotkey.Rows.Add(hotkeyrow);

            hotkeyrow = hotkey.NewRow();
			hotkeyrow.ItemArray = new object[] { "SpellsMagery", "Clumsy", Keys.None, true };
			hotkey.Rows.Add(hotkeyrow);

			hotkeyrow = hotkey.NewRow();
			hotkeyrow.ItemArray = new object[] { "SpellsMagery", "Create Food", Keys.None, true };
			hotkey.Rows.Add(hotkeyrow);

			hotkeyrow = hotkey.NewRow();
			hotkeyrow.ItemArray = new object[] { "SpellsMagery", "Feeblemind", Keys.None, true };
			hotkey.Rows.Add(hotkeyrow);

			hotkeyrow = hotkey.NewRow();
			hotkeyrow.ItemArray = new object[] { "SpellsMagery", "Heal", Keys.None, true };
			hotkey.Rows.Add(hotkeyrow);

			hotkeyrow = hotkey.NewRow();
			hotkeyrow.ItemArray = new object[] { "SpellsMagery", "Magic Arrow", Keys.None, true };
			hotkey.Rows.Add(hotkeyrow);

			hotkeyrow = hotkey.NewRow();
			hotkeyrow.ItemArray = new object[] { "SpellsMagery", "Night Sight", Keys.None, true };
			hotkey.Rows.Add(hotkeyrow);

			hotkeyrow = hotkey.NewRow();
			hotkeyrow.ItemArray = new object[] { "SpellsMagery", "Reactive Armor", Keys.None, true };
			hotkey.Rows.Add(hotkeyrow);

			hotkeyrow = hotkey.NewRow();
			hotkeyrow.ItemArray = new object[] { "SpellsMagery", "Weaken", Keys.None, true };
			hotkey.Rows.Add(hotkeyrow);

			hotkeyrow = hotkey.NewRow();
			hotkeyrow.ItemArray = new object[] { "SpellsMagery", "Agility", Keys.None, true };
			hotkey.Rows.Add(hotkeyrow);

			hotkeyrow = hotkey.NewRow();
			hotkeyrow.ItemArray = new object[] { "SpellsMagery", "Cunning", Keys.None, true };
			hotkey.Rows.Add(hotkeyrow);

			hotkeyrow = hotkey.NewRow();
			hotkeyrow.ItemArray = new object[] { "SpellsMagery", "Cure", Keys.None, true };
			hotkey.Rows.Add(hotkeyrow);

			hotkeyrow = hotkey.NewRow();
			hotkeyrow.ItemArray = new object[] { "SpellsMagery", "Harm", Keys.None, true };
			hotkey.Rows.Add(hotkeyrow);

			hotkeyrow = hotkey.NewRow();
			hotkeyrow.ItemArray = new object[] { "SpellsMagery", "Magic Trap", Keys.None, true };
			hotkey.Rows.Add(hotkeyrow);

			hotkeyrow = hotkey.NewRow();
			hotkeyrow.ItemArray = new object[] { "SpellsMagery", "Magic Untrap", Keys.None, true };
			hotkey.Rows.Add(hotkeyrow);

			hotkeyrow = hotkey.NewRow();
			hotkeyrow.ItemArray = new object[] { "SpellsMagery", "Protection", Keys.None, true };
			hotkey.Rows.Add(hotkeyrow);

			hotkeyrow = hotkey.NewRow();
			hotkeyrow.ItemArray = new object[] { "SpellsMagery", "Strength", Keys.None, true };
			hotkey.Rows.Add(hotkeyrow);

			hotkeyrow = hotkey.NewRow();
			hotkeyrow.ItemArray = new object[] { "SpellsMagery", "Bless", Keys.None, true };
			hotkey.Rows.Add(hotkeyrow);

			hotkeyrow = hotkey.NewRow();
			hotkeyrow.ItemArray = new object[] { "SpellsMagery", "Fireball", Keys.None, true };
			hotkey.Rows.Add(hotkeyrow);

			hotkeyrow = hotkey.NewRow();
			hotkeyrow.ItemArray = new object[] { "SpellsMagery", "Magic Lock", Keys.None, true };
			hotkey.Rows.Add(hotkeyrow);

			hotkeyrow = hotkey.NewRow();
			hotkeyrow.ItemArray = new object[] { "SpellsMagery", "Poison", Keys.None, true };
			hotkey.Rows.Add(hotkeyrow);

			hotkeyrow = hotkey.NewRow();
			hotkeyrow.ItemArray = new object[] { "SpellsMagery", "Telekinesis", Keys.None, true };
			hotkey.Rows.Add(hotkeyrow);

			hotkeyrow = hotkey.NewRow();
			hotkeyrow.ItemArray = new object[] { "SpellsMagery", "Teleport", Keys.None, true };
			hotkey.Rows.Add(hotkeyrow);

			hotkeyrow = hotkey.NewRow();
			hotkeyrow.ItemArray = new object[] { "SpellsMagery", "Unlock", Keys.None, true };
			hotkey.Rows.Add(hotkeyrow);

			hotkeyrow = hotkey.NewRow();
			hotkeyrow.ItemArray = new object[] { "SpellsMagery", "Wall of Stone", Keys.None, true };
			hotkey.Rows.Add(hotkeyrow);

			hotkeyrow = hotkey.NewRow();
			hotkeyrow.ItemArray = new object[] { "SpellsMagery", "Arch Cure", Keys.None, true };
			hotkey.Rows.Add(hotkeyrow);

			hotkeyrow = hotkey.NewRow();
			hotkeyrow.ItemArray = new object[] { "SpellsMagery", "Arch Protection", Keys.None, true };
			hotkey.Rows.Add(hotkeyrow);

			hotkeyrow = hotkey.NewRow();
			hotkeyrow.ItemArray = new object[] { "SpellsMagery", "Curse", Keys.None, true };
			hotkey.Rows.Add(hotkeyrow);

			hotkeyrow = hotkey.NewRow();
			hotkeyrow.ItemArray = new object[] { "SpellsMagery", "Fire Field", Keys.None, true };
			hotkey.Rows.Add(hotkeyrow);

			hotkeyrow = hotkey.NewRow();
			hotkeyrow.ItemArray = new object[] { "SpellsMagery", "Greater Heal", Keys.None, true };
			hotkey.Rows.Add(hotkeyrow);

			hotkeyrow = hotkey.NewRow();
			hotkeyrow.ItemArray = new object[] { "SpellsMagery", "Lightning", Keys.None, true };
			hotkey.Rows.Add(hotkeyrow);

			hotkeyrow = hotkey.NewRow();
			hotkeyrow.ItemArray = new object[] { "SpellsMagery", "Mana Drain", Keys.None, true };
			hotkey.Rows.Add(hotkeyrow);

			hotkeyrow = hotkey.NewRow();
			hotkeyrow.ItemArray = new object[] { "SpellsMagery", "Recall", Keys.None, true };
			hotkey.Rows.Add(hotkeyrow);

			hotkeyrow = hotkey.NewRow();
			hotkeyrow.ItemArray = new object[] { "SpellsMagery", "Blade Spirits", Keys.None, true };
			hotkey.Rows.Add(hotkeyrow);

			hotkeyrow = hotkey.NewRow();
			hotkeyrow.ItemArray = new object[] { "SpellsMagery", "Dispel Field", Keys.None, true };
			hotkey.Rows.Add(hotkeyrow);

			hotkeyrow = hotkey.NewRow();
			hotkeyrow.ItemArray = new object[] { "SpellsMagery", "Incognito", Keys.None, true };
			hotkey.Rows.Add(hotkeyrow);

			hotkeyrow = hotkey.NewRow();
			hotkeyrow.ItemArray = new object[] { "SpellsMagery", "Magic Reflection", Keys.None, true };
			hotkey.Rows.Add(hotkeyrow);

			hotkeyrow = hotkey.NewRow();
			hotkeyrow.ItemArray = new object[] { "SpellsMagery", "Mind Blast", Keys.None, true };
			hotkey.Rows.Add(hotkeyrow);

			hotkeyrow = hotkey.NewRow();
			hotkeyrow.ItemArray = new object[] { "SpellsMagery", "Paralyze", Keys.None, true };
			hotkey.Rows.Add(hotkeyrow);

			hotkeyrow = hotkey.NewRow();
			hotkeyrow.ItemArray = new object[] { "SpellsMagery", "Poison Field", Keys.None, true };
			hotkey.Rows.Add(hotkeyrow);

			hotkeyrow = hotkey.NewRow();
			hotkeyrow.ItemArray = new object[] { "SpellsMagery", "Summon Creature", Keys.None, true };
			hotkey.Rows.Add(hotkeyrow);

			hotkeyrow = hotkey.NewRow();
			hotkeyrow.ItemArray = new object[] { "SpellsMagery", "Dispel", Keys.None, true };
			hotkey.Rows.Add(hotkeyrow);

			hotkeyrow = hotkey.NewRow();
			hotkeyrow.ItemArray = new object[] { "SpellsMagery", "Energy Bolt", Keys.None, true };
			hotkey.Rows.Add(hotkeyrow);

			hotkeyrow = hotkey.NewRow();
			hotkeyrow.ItemArray = new object[] { "SpellsMagery", "Explosion", Keys.None, true };
			hotkey.Rows.Add(hotkeyrow);

			hotkeyrow = hotkey.NewRow();
			hotkeyrow.ItemArray = new object[] { "SpellsMagery", "Invisibility", Keys.None, true };
			hotkey.Rows.Add(hotkeyrow);

			hotkeyrow = hotkey.NewRow();
			hotkeyrow.ItemArray = new object[] { "SpellsMagery", "Mark", Keys.None, true };
			hotkey.Rows.Add(hotkeyrow);

			hotkeyrow = hotkey.NewRow();
			hotkeyrow.ItemArray = new object[] { "SpellsMagery", "Mass Curse", Keys.None, true };
			hotkey.Rows.Add(hotkeyrow);

			hotkeyrow = hotkey.NewRow();
			hotkeyrow.ItemArray = new object[] { "SpellsMagery", "Paralyze Field", Keys.None, true };
			hotkey.Rows.Add(hotkeyrow);

			hotkeyrow = hotkey.NewRow();
			hotkeyrow.ItemArray = new object[] { "SpellsMagery", "Reveal", Keys.None, true };
			hotkey.Rows.Add(hotkeyrow);

			hotkeyrow = hotkey.NewRow();
			hotkeyrow.ItemArray = new object[] { "SpellsMagery", "Chain Lightning", Keys.None, true };
			hotkey.Rows.Add(hotkeyrow);

			hotkeyrow = hotkey.NewRow();
			hotkeyrow.ItemArray = new object[] { "SpellsMagery", "Energy Field", Keys.None, true };
			hotkey.Rows.Add(hotkeyrow);

			hotkeyrow = hotkey.NewRow();
			hotkeyrow.ItemArray = new object[] { "SpellsMagery", "Flamestrike", Keys.None, true };
			hotkey.Rows.Add(hotkeyrow);

			hotkeyrow = hotkey.NewRow();
			hotkeyrow.ItemArray = new object[] { "SpellsMagery", "Gate Travel", Keys.None, true };
			hotkey.Rows.Add(hotkeyrow);

			hotkeyrow = hotkey.NewRow();
			hotkeyrow.ItemArray = new object[] { "SpellsMagery", "Mana Vampire", Keys.None, true };
			hotkey.Rows.Add(hotkeyrow);

			hotkeyrow = hotkey.NewRow();
			hotkeyrow.ItemArray = new object[] { "SpellsMagery", "Mass Dispel", Keys.None, true };
			hotkey.Rows.Add(hotkeyrow);

			hotkeyrow = hotkey.NewRow();
			hotkeyrow.ItemArray = new object[] { "SpellsMagery", "Meteor Swarm", Keys.None, true };
			hotkey.Rows.Add(hotkeyrow);

			hotkeyrow = hotkey.NewRow();
			hotkeyrow.ItemArray = new object[] { "SpellsMagery", "Polymorph", Keys.None, true };
			hotkey.Rows.Add(hotkeyrow);

			hotkeyrow = hotkey.NewRow();
			hotkeyrow.ItemArray = new object[] { "SpellsMagery", "Earthquake", Keys.None, true };
			hotkey.Rows.Add(hotkeyrow);

			hotkeyrow = hotkey.NewRow();
			hotkeyrow.ItemArray = new object[] { "SpellsMagery", "Energy Vortex", Keys.None, true };
			hotkey.Rows.Add(hotkeyrow);

			hotkeyrow = hotkey.NewRow();
			hotkeyrow.ItemArray = new object[] { "SpellsMagery", "Resurrection", Keys.None, true };
			hotkey.Rows.Add(hotkeyrow);

			hotkeyrow = hotkey.NewRow();
			hotkeyrow.ItemArray = new object[] { "SpellsMagery", "Summon Air Elemental", Keys.None, true };
			hotkey.Rows.Add(hotkeyrow);

			hotkeyrow = hotkey.NewRow();
			hotkeyrow.ItemArray = new object[] { "SpellsMagery", "Summon Daemon", Keys.None, true };
			hotkey.Rows.Add(hotkeyrow);

			hotkeyrow = hotkey.NewRow();
			hotkeyrow.ItemArray = new object[] { "SpellsMagery", "Summon Earth Elemental", Keys.None, true };
			hotkey.Rows.Add(hotkeyrow);

			hotkeyrow = hotkey.NewRow();
			hotkeyrow.ItemArray = new object[] { "SpellsMagery", "Summon Fire Elemental", Keys.None, true };
			hotkey.Rows.Add(hotkeyrow);

			hotkeyrow = hotkey.NewRow();
			hotkeyrow.ItemArray = new object[] { "SpellsMagery", "Summon Water Elemental", Keys.None, true };
			hotkey.Rows.Add(hotkeyrow);

			hotkeyrow = hotkey.NewRow();
			hotkeyrow.ItemArray = new object[] { "SpellsNecro", "Animate Dead", Keys.None, true };
			hotkey.Rows.Add(hotkeyrow);

			hotkeyrow = hotkey.NewRow();
			hotkeyrow.ItemArray = new object[] { "SpellsNecro", "Blood Oath", Keys.None, true };
			hotkey.Rows.Add(hotkeyrow);

			hotkeyrow = hotkey.NewRow();
			hotkeyrow.ItemArray = new object[] { "SpellsNecro", "Corpse Skin", Keys.None, true };
			hotkey.Rows.Add(hotkeyrow);

			hotkeyrow = hotkey.NewRow();
			hotkeyrow.ItemArray = new object[] { "SpellsNecro", "Curse Weapon", Keys.None, true };
			hotkey.Rows.Add(hotkeyrow);

			hotkeyrow = hotkey.NewRow();
			hotkeyrow.ItemArray = new object[] { "SpellsNecro", "Evil Omen", Keys.None, true };
			hotkey.Rows.Add(hotkeyrow);

			hotkeyrow = hotkey.NewRow();
			hotkeyrow.ItemArray = new object[] { "SpellsNecro", "Horrific Beast", Keys.None, true };
			hotkey.Rows.Add(hotkeyrow);

			hotkeyrow = hotkey.NewRow();
			hotkeyrow.ItemArray = new object[] { "SpellsNecro", "Lich Form", Keys.None, true };
			hotkey.Rows.Add(hotkeyrow);

			hotkeyrow = hotkey.NewRow();
			hotkeyrow.ItemArray = new object[] { "SpellsNecro", "Mind Rot", Keys.None, true };
			hotkey.Rows.Add(hotkeyrow);

			hotkeyrow = hotkey.NewRow();
			hotkeyrow.ItemArray = new object[] { "SpellsNecro", "Pain Spike", Keys.None, true };
			hotkey.Rows.Add(hotkeyrow);

			hotkeyrow = hotkey.NewRow();
			hotkeyrow.ItemArray = new object[] { "SpellsNecro", "Poison Strike", Keys.None, true };
			hotkey.Rows.Add(hotkeyrow);

			hotkeyrow = hotkey.NewRow();
			hotkeyrow.ItemArray = new object[] { "SpellsNecro", "Strangle", Keys.None, true };
			hotkey.Rows.Add(hotkeyrow);

			hotkeyrow = hotkey.NewRow();
			hotkeyrow.ItemArray = new object[] { "SpellsNecro", "Summon Familiar", Keys.None, true };
			hotkey.Rows.Add(hotkeyrow);

			hotkeyrow = hotkey.NewRow();
			hotkeyrow.ItemArray = new object[] { "SpellsNecro", "Vampiric Embrace", Keys.None, true };
			hotkey.Rows.Add(hotkeyrow);

			hotkeyrow = hotkey.NewRow();
			hotkeyrow.ItemArray = new object[] { "SpellsNecro", "Vengeful Spirit", Keys.None, true };
			hotkey.Rows.Add(hotkeyrow);

			hotkeyrow = hotkey.NewRow();
			hotkeyrow.ItemArray = new object[] { "SpellsNecro", "Wither", Keys.None, true };
			hotkey.Rows.Add(hotkeyrow);

			hotkeyrow = hotkey.NewRow();
			hotkeyrow.ItemArray = new object[] { "SpellsNecro", "Wraith Form", Keys.None, true };
			hotkey.Rows.Add(hotkeyrow);

			hotkeyrow = hotkey.NewRow();
			hotkeyrow.ItemArray = new object[] { "SpellsNecro", "Exorcism", Keys.None, true };
			hotkey.Rows.Add(hotkeyrow);

			hotkeyrow = hotkey.NewRow();
			hotkeyrow.ItemArray = new object[] { "SpellsBushido", "Honorable Execution", Keys.None, true };
			hotkey.Rows.Add(hotkeyrow);

			hotkeyrow = hotkey.NewRow();
			hotkeyrow.ItemArray = new object[] { "SpellsBushido", "Confidence", Keys.None, true };
			hotkey.Rows.Add(hotkeyrow);

			hotkeyrow = hotkey.NewRow();
			hotkeyrow.ItemArray = new object[] { "SpellsBushido", "Evasion", Keys.None, true };
			hotkey.Rows.Add(hotkeyrow);

			hotkeyrow = hotkey.NewRow();
			hotkeyrow.ItemArray = new object[] { "SpellsBushido", "Counter Attack", Keys.None, true };
			hotkey.Rows.Add(hotkeyrow);

			hotkeyrow = hotkey.NewRow();
			hotkeyrow.ItemArray = new object[] { "SpellsBushido", "Lightning Strike", Keys.None, true };
			hotkey.Rows.Add(hotkeyrow);

			hotkeyrow = hotkey.NewRow();
			hotkeyrow.ItemArray = new object[] { "SpellsBushido", "Momentum Strike", Keys.None, true };
			hotkey.Rows.Add(hotkeyrow);

			hotkeyrow = hotkey.NewRow();
			hotkeyrow.ItemArray = new object[] { "SpellsNinjitsu", "Focus Attack", Keys.None, true };
			hotkey.Rows.Add(hotkeyrow);

			hotkeyrow = hotkey.NewRow();
			hotkeyrow.ItemArray = new object[] { "SpellsNinjitsu", "Death Strike", Keys.None, true };
			hotkey.Rows.Add(hotkeyrow);

			hotkeyrow = hotkey.NewRow();
			hotkeyrow.ItemArray = new object[] { "SpellsNinjitsu", "Animal Form", Keys.None, true };
			hotkey.Rows.Add(hotkeyrow);

			hotkeyrow = hotkey.NewRow();
			hotkeyrow.ItemArray = new object[] { "SpellsNinjitsu", "Ki Attack", Keys.None, true };
			hotkey.Rows.Add(hotkeyrow);

			hotkeyrow = hotkey.NewRow();
			hotkeyrow.ItemArray = new object[] { "SpellsNinjitsu", "Surprise Attack", Keys.None, true };
			hotkey.Rows.Add(hotkeyrow);

			hotkeyrow = hotkey.NewRow();
			hotkeyrow.ItemArray = new object[] { "SpellsNinjitsu", "Backstab", Keys.None, true };
			hotkey.Rows.Add(hotkeyrow);

			hotkeyrow = hotkey.NewRow();
			hotkeyrow.ItemArray = new object[] { "SpellsNinjitsu", "Shadow jump", Keys.None, true };
			hotkey.Rows.Add(hotkeyrow);

			hotkeyrow = hotkey.NewRow();
			hotkeyrow.ItemArray = new object[] { "SpellsNinjitsu", "Mirror Image", Keys.None, true };
			hotkey.Rows.Add(hotkeyrow);

			hotkeyrow = hotkey.NewRow();
			hotkeyrow.ItemArray = new object[] { "SpellsSpellweaving", "Arcane Circle", Keys.None, true };
			hotkey.Rows.Add(hotkeyrow);

			hotkeyrow = hotkey.NewRow();
			hotkeyrow.ItemArray = new object[] { "SpellsSpellweaving", "Gift Of Renewal", Keys.None, true };
			hotkey.Rows.Add(hotkeyrow);

			hotkeyrow = hotkey.NewRow();
			hotkeyrow.ItemArray = new object[] { "SpellsSpellweaving", "Immolating Weapon", Keys.None, true };
			hotkey.Rows.Add(hotkeyrow);

			hotkeyrow = hotkey.NewRow();
			hotkeyrow.ItemArray = new object[] { "SpellsSpellweaving", "Thunderstorm", Keys.None, true };
			hotkey.Rows.Add(hotkeyrow);

			hotkeyrow = hotkey.NewRow();
			hotkeyrow.ItemArray = new object[] { "SpellsSpellweaving", "Natures Fury", Keys.None, true };
			hotkey.Rows.Add(hotkeyrow);

			hotkeyrow = hotkey.NewRow();
			hotkeyrow.ItemArray = new object[] { "SpellsSpellweaving", "Summon Fey", Keys.None, true };
			hotkey.Rows.Add(hotkeyrow);

			hotkeyrow = hotkey.NewRow();
			hotkeyrow.ItemArray = new object[] { "SpellsSpellweaving", "Summoniend", Keys.None, true };
			hotkey.Rows.Add(hotkeyrow);

			hotkeyrow = hotkey.NewRow();
			hotkeyrow.ItemArray = new object[] { "SpellsSpellweaving", "Reaper Form", Keys.None, true };
			hotkey.Rows.Add(hotkeyrow);

			hotkeyrow = hotkey.NewRow();
			hotkeyrow.ItemArray = new object[] { "SpellsSpellweaving", "Wildfire", Keys.None, true };
			hotkey.Rows.Add(hotkeyrow);

			hotkeyrow = hotkey.NewRow();
			hotkeyrow.ItemArray = new object[] { "SpellsSpellweaving", "Essence Of Wind", Keys.None, true };
			hotkey.Rows.Add(hotkeyrow);

			hotkeyrow = hotkey.NewRow();
			hotkeyrow.ItemArray = new object[] { "SpellsSpellweaving", "Dryad Allure", Keys.None, true };
			hotkey.Rows.Add(hotkeyrow);

			hotkeyrow = hotkey.NewRow();
			hotkeyrow.ItemArray = new object[] { "SpellsSpellweaving", "Ethereal Voyage", Keys.None, true };
			hotkey.Rows.Add(hotkeyrow);

			hotkeyrow = hotkey.NewRow();
			hotkeyrow.ItemArray = new object[] { "SpellsSpellweaving", "Word Of Death", Keys.None, true };
			hotkey.Rows.Add(hotkeyrow);

			hotkeyrow = hotkey.NewRow();
			hotkeyrow.ItemArray = new object[] { "SpellsSpellweaving", "Gift Of Life", Keys.None, true };
			hotkey.Rows.Add(hotkeyrow);

			hotkeyrow = hotkey.NewRow();
			hotkeyrow.ItemArray = new object[] { "SpellsSpellweaving", "Arcane Empowerment", Keys.None, true };
			hotkey.Rows.Add(hotkeyrow);

			hotkeyrow = hotkey.NewRow();
			hotkeyrow.ItemArray = new object[] { "SpellsSpellweaving", "Attunement", Keys.None, true };
			hotkey.Rows.Add(hotkeyrow);

			hotkeyrow = hotkey.NewRow();
			hotkeyrow.ItemArray = new object[] { "SpellsMysticism", "Nether Bolt", Keys.None, true };
			hotkey.Rows.Add(hotkeyrow);

			hotkeyrow = hotkey.NewRow();
			hotkeyrow.ItemArray = new object[] { "SpellsMysticism", "Healing Stone", Keys.None, true };
			hotkey.Rows.Add(hotkeyrow);

			hotkeyrow = hotkey.NewRow();
			hotkeyrow.ItemArray = new object[] { "SpellsMysticism", "Purge Magic", Keys.None, true };
			hotkey.Rows.Add(hotkeyrow);

			hotkeyrow = hotkey.NewRow();
			hotkeyrow.ItemArray = new object[] { "SpellsMysticism", "Enchant", Keys.None, true };
			hotkey.Rows.Add(hotkeyrow);

			hotkeyrow = hotkey.NewRow();
			hotkeyrow.ItemArray = new object[] { "SpellsMysticism", "Sleep", Keys.None, true };
			hotkey.Rows.Add(hotkeyrow);

			hotkeyrow = hotkey.NewRow();
			hotkeyrow.ItemArray = new object[] { "SpellsMysticism", "Eagle Strike", Keys.None, true };
			hotkey.Rows.Add(hotkeyrow);

			hotkeyrow = hotkey.NewRow();
			hotkeyrow.ItemArray = new object[] { "SpellsMysticism", "Animated Weapon", Keys.None, true };
			hotkey.Rows.Add(hotkeyrow);

			hotkeyrow = hotkey.NewRow();
			hotkeyrow.ItemArray = new object[] { "SpellsMysticism", "Stone Form", Keys.None, true };
			hotkey.Rows.Add(hotkeyrow);

			hotkeyrow = hotkey.NewRow();
			hotkeyrow.ItemArray = new object[] { "SpellsMysticism", "Spell Trigger", Keys.None, true };
			hotkey.Rows.Add(hotkeyrow);

			hotkeyrow = hotkey.NewRow();
			hotkeyrow.ItemArray = new object[] { "SpellsMysticism", "Mass Sleep", Keys.None, true };
			hotkey.Rows.Add(hotkeyrow);

			hotkeyrow = hotkey.NewRow();
			hotkeyrow.ItemArray = new object[] { "SpellsMysticism", "Cleansing Winds", Keys.None, true };
			hotkey.Rows.Add(hotkeyrow);

			hotkeyrow = hotkey.NewRow();
			hotkeyrow.ItemArray = new object[] { "SpellsMysticism", "Bombard", Keys.None, true };
			hotkey.Rows.Add(hotkeyrow);

			hotkeyrow = hotkey.NewRow();
			hotkeyrow.ItemArray = new object[] { "SpellsMysticism", "Spell Plague", Keys.None, true };
			hotkey.Rows.Add(hotkeyrow);

			hotkeyrow = hotkey.NewRow();
			hotkeyrow.ItemArray = new object[] { "SpellsMysticism", "Hail Storm", Keys.None, true };
			hotkey.Rows.Add(hotkeyrow);

			hotkeyrow = hotkey.NewRow();
			hotkeyrow.ItemArray = new object[] { "SpellsMysticism", "Nether Cyclone", Keys.None, true };
			hotkey.Rows.Add(hotkeyrow);

			hotkeyrow = hotkey.NewRow();
			hotkeyrow.ItemArray = new object[] { "SpellsMysticism", "Rising Colossus", Keys.None, true };
			hotkey.Rows.Add(hotkeyrow);

			hotkeyrow = hotkey.NewRow();
			hotkeyrow.ItemArray = new object[] { "SpellsChivalry", "Cleanse By Fire", Keys.None, true };
			hotkey.Rows.Add(hotkeyrow);

			hotkeyrow = hotkey.NewRow();
			hotkeyrow.ItemArray = new object[] { "SpellsChivalry", "Close Wounds", Keys.None, true };
			hotkey.Rows.Add(hotkeyrow);

			hotkeyrow = hotkey.NewRow();
			hotkeyrow.ItemArray = new object[] { "SpellsChivalry", "Consecrate Weapon", Keys.None, true };
			hotkey.Rows.Add(hotkeyrow);

			hotkeyrow = hotkey.NewRow();
			hotkeyrow.ItemArray = new object[] { "SpellsChivalry", "Dispel Evil", Keys.None, true };
			hotkey.Rows.Add(hotkeyrow);

			hotkeyrow = hotkey.NewRow();
			hotkeyrow.ItemArray = new object[] { "SpellsChivalry", "Divine Fury", Keys.None, true };
			hotkey.Rows.Add(hotkeyrow);

			hotkeyrow = hotkey.NewRow();
			hotkeyrow.ItemArray = new object[] { "SpellsChivalry", "Enemy Of One", Keys.None, true };
			hotkey.Rows.Add(hotkeyrow);

			hotkeyrow = hotkey.NewRow();
			hotkeyrow.ItemArray = new object[] { "SpellsChivalry", "Holy Light", Keys.None, true };
			hotkey.Rows.Add(hotkeyrow);

			hotkeyrow = hotkey.NewRow();
			hotkeyrow.ItemArray = new object[] { "SpellsChivalry", "Noble Sacrifice", Keys.None, true };
			hotkey.Rows.Add(hotkeyrow);

			hotkeyrow = hotkey.NewRow();
			hotkeyrow.ItemArray = new object[] { "SpellsChivalry", "Remove Curse", Keys.None, true };
			hotkey.Rows.Add(hotkeyrow);

			hotkeyrow = hotkey.NewRow();
			hotkeyrow.ItemArray = new object[] { "SpellsChivalry", "Sacred Journey", Keys.None, true };
			hotkey.Rows.Add(hotkeyrow);

			hotkeyrow = hotkey.NewRow();
			hotkeyrow.ItemArray = new object[] { "SpellsMastery", "Inspire", Keys.None, true };
			hotkey.Rows.Add(hotkeyrow);

			hotkeyrow = hotkey.NewRow();
			hotkeyrow.ItemArray = new object[] { "SpellsMastery", "Invigorate", Keys.None, true };
			hotkey.Rows.Add(hotkeyrow);

			hotkeyrow = hotkey.NewRow();
			hotkeyrow.ItemArray = new object[] { "SpellsMastery", "Resilience", Keys.None, true };
			hotkey.Rows.Add(hotkeyrow);

			hotkeyrow = hotkey.NewRow();
			hotkeyrow.ItemArray = new object[] { "SpellsMastery", "Perseverance", Keys.None, true };
			hotkey.Rows.Add(hotkeyrow);

			hotkeyrow = hotkey.NewRow();
			hotkeyrow.ItemArray = new object[] { "SpellsMastery", "Tribulation", Keys.None, true };
			hotkey.Rows.Add(hotkeyrow);

			hotkeyrow = hotkey.NewRow();
			hotkeyrow.ItemArray = new object[] { "SpellsMastery", "Despair", Keys.None, true };
			hotkey.Rows.Add(hotkeyrow);

			hotkeyrow = hotkey.NewRow();
			hotkeyrow.ItemArray = new object[] { "SpellsMastery", "Death Ray", Keys.None, true };
			hotkey.Rows.Add(hotkeyrow);

			hotkeyrow = hotkey.NewRow();
			hotkeyrow.ItemArray = new object[] { "SpellsMastery", "Ethereal Blast", Keys.None, true };
			hotkey.Rows.Add(hotkeyrow);

			hotkeyrow = hotkey.NewRow();
			hotkeyrow.ItemArray = new object[] { "SpellsMastery", "Nether Blast", Keys.None, true };
			hotkey.Rows.Add(hotkeyrow);

			hotkeyrow = hotkey.NewRow();
			hotkeyrow.ItemArray = new object[] { "SpellsMastery", "Mystic Weapon", Keys.None, true };
			hotkey.Rows.Add(hotkeyrow);

			hotkeyrow = hotkey.NewRow();
			hotkeyrow.ItemArray = new object[] { "SpellsMastery", "Command Undead", Keys.None, true };
			hotkey.Rows.Add(hotkeyrow);

			hotkeyrow = hotkey.NewRow();
			hotkeyrow.ItemArray = new object[] { "SpellsMastery", "Mana Shield", Keys.None, true };
			hotkey.Rows.Add(hotkeyrow);

			hotkeyrow = hotkey.NewRow();
			hotkeyrow.ItemArray = new object[] { "SpellsMastery", "Summon Reaper", Keys.None, true };
			hotkey.Rows.Add(hotkeyrow);

			hotkeyrow = hotkey.NewRow();
			hotkeyrow.ItemArray = new object[] { "SpellsMastery", "Enchanted Summoning", Keys.None, true };
			hotkey.Rows.Add(hotkeyrow);

			hotkeyrow = hotkey.NewRow();
			hotkeyrow.ItemArray = new object[] { "SpellsMastery", "Anticipate Hit", Keys.None, true };
			hotkey.Rows.Add(hotkeyrow);

			hotkeyrow = hotkey.NewRow();
			hotkeyrow.ItemArray = new object[] { "SpellsMastery", "Warcry", Keys.None, true };
			hotkey.Rows.Add(hotkeyrow);

			hotkeyrow = hotkey.NewRow();
			hotkeyrow.ItemArray = new object[] { "SpellsMastery", "Intuition", Keys.None, true };
			hotkey.Rows.Add(hotkeyrow);

			hotkeyrow = hotkey.NewRow();
			hotkeyrow.ItemArray = new object[] { "SpellsMastery", "Rejuvenate", Keys.None, true };
			hotkey.Rows.Add(hotkeyrow);

			hotkeyrow = hotkey.NewRow();
			hotkeyrow.ItemArray = new object[] { "SpellsMastery", "Holy Fist", Keys.None, true };
			hotkey.Rows.Add(hotkeyrow);

			hotkeyrow = hotkey.NewRow();
			hotkeyrow.ItemArray = new object[] { "SpellsMastery", "Shadow", Keys.None, true };
			hotkey.Rows.Add(hotkeyrow);

			hotkeyrow = hotkey.NewRow();
			hotkeyrow.ItemArray = new object[] { "SpellsMastery", "White Tiger Form", Keys.None, true };
			hotkey.Rows.Add(hotkeyrow);

			hotkeyrow = hotkey.NewRow();
			hotkeyrow.ItemArray = new object[] { "SpellsMastery", "Flaming Shot", Keys.None, true };
			hotkey.Rows.Add(hotkeyrow);

			hotkeyrow = hotkey.NewRow();
			hotkeyrow.ItemArray = new object[] { "SpellsMastery", "Playing The Odds", Keys.None, true };
			hotkey.Rows.Add(hotkeyrow);

			hotkeyrow = hotkey.NewRow();
			hotkeyrow.ItemArray = new object[] { "SpellsMastery", "Thrust", Keys.None, true };
			hotkey.Rows.Add(hotkeyrow);

			hotkeyrow = hotkey.NewRow();
			hotkeyrow.ItemArray = new object[] { "SpellsMastery", "Pierce", Keys.None, true };
			hotkey.Rows.Add(hotkeyrow);

			hotkeyrow = hotkey.NewRow();
			hotkeyrow.ItemArray = new object[] { "SpellsMastery", "Stagger", Keys.None, true };
			hotkey.Rows.Add(hotkeyrow);

			hotkeyrow = hotkey.NewRow();
			hotkeyrow.ItemArray = new object[] { "SpellsMastery", "Toughness", Keys.None, true };
			hotkey.Rows.Add(hotkeyrow);

			hotkeyrow = hotkey.NewRow();
			hotkeyrow.ItemArray = new object[] { "SpellsMastery", "Onslaught", Keys.None, true };
			hotkey.Rows.Add(hotkeyrow);

			hotkeyrow = hotkey.NewRow();
			hotkeyrow.ItemArray = new object[] { "SpellsMastery", "Focused Eye", Keys.None, true };
			hotkey.Rows.Add(hotkeyrow);

			hotkeyrow = hotkey.NewRow();
			hotkeyrow.ItemArray = new object[] { "SpellsMastery", "Elemental Fury", Keys.None, true };
			hotkey.Rows.Add(hotkeyrow);

			hotkeyrow = hotkey.NewRow();
			hotkeyrow.ItemArray = new object[] { "SpellsMastery", "Called Shot", Keys.None, true };
			hotkey.Rows.Add(hotkeyrow);

			hotkeyrow = hotkey.NewRow();
			hotkeyrow.ItemArray = new object[] { "SpellsMastery", "Saving Throw", Keys.None, true };
			hotkey.Rows.Add(hotkeyrow);

			hotkeyrow = hotkey.NewRow();
			hotkeyrow.ItemArray = new object[] { "SpellsMastery", "Shield Bash", Keys.None, true };
			hotkey.Rows.Add(hotkeyrow);

			hotkeyrow = hotkey.NewRow();
			hotkeyrow.ItemArray = new object[] { "SpellsMastery", "Bodyguard", Keys.None, true };
			hotkey.Rows.Add(hotkeyrow);

			hotkeyrow = hotkey.NewRow();
			hotkeyrow.ItemArray = new object[] { "SpellsMastery", "Heighten Senses", Keys.None, true };
			hotkey.Rows.Add(hotkeyrow);

			hotkeyrow = hotkey.NewRow();
			hotkeyrow.ItemArray = new object[] { "SpellsMastery", "Tolerance", Keys.None, true };
			hotkey.Rows.Add(hotkeyrow);

			hotkeyrow = hotkey.NewRow();
			hotkeyrow.ItemArray = new object[] { "SpellsMastery", "Injected Strike", Keys.None, true };
			hotkey.Rows.Add(hotkeyrow);

			hotkeyrow = hotkey.NewRow();
			hotkeyrow.ItemArray = new object[] { "SpellsMastery", "Potency", Keys.None, true };
			hotkey.Rows.Add(hotkeyrow);

			hotkeyrow = hotkey.NewRow();
			hotkeyrow.ItemArray = new object[] { "SpellsMastery", "Rampage", Keys.None, true };
			hotkey.Rows.Add(hotkeyrow);

			hotkeyrow = hotkey.NewRow();
			hotkeyrow.ItemArray = new object[] { "SpellsMastery", "Fists Of Fury", Keys.None, true };
			hotkey.Rows.Add(hotkeyrow);

			hotkeyrow = hotkey.NewRow();
			hotkeyrow.ItemArray = new object[] { "SpellsMastery", "Knockout", Keys.None, true };
			hotkey.Rows.Add(hotkeyrow);

			hotkeyrow = hotkey.NewRow();
			hotkeyrow.ItemArray = new object[] { "SpellsMastery", "Whispering", Keys.None, true };
			hotkey.Rows.Add(hotkeyrow);

			hotkeyrow = hotkey.NewRow();
			hotkeyrow.ItemArray = new object[] { "SpellsMastery", "Combat Training", Keys.None, true };
			hotkey.Rows.Add(hotkeyrow);

			hotkeyrow = hotkey.NewRow();
			hotkeyrow.ItemArray = new object[] { "SpellsMastery", "Boarding", Keys.None, true };
			hotkey.Rows.Add(hotkeyrow);

            initCleric(hotkey);

            initDruid(hotkey);

            hotkeyrow = hotkey.NewRow();
			hotkeyrow.ItemArray = new object[] { "UseVirtue", "Honor", Keys.None, true };
			hotkey.Rows.Add(hotkeyrow);

			hotkeyrow = hotkey.NewRow();
			hotkeyrow.ItemArray = new object[] { "UseVirtue", "Sacrifice", Keys.None, true };
			hotkey.Rows.Add(hotkeyrow);

			hotkeyrow = hotkey.NewRow();
			hotkeyrow.ItemArray = new object[] { "UseVirtue", "Compassion", Keys.None, true };
			hotkey.Rows.Add(hotkeyrow);

			hotkeyrow = hotkey.NewRow();
			hotkeyrow.ItemArray = new object[] { "UseVirtue", "Valor", Keys.None, true };
			hotkey.Rows.Add(hotkeyrow);

			hotkeyrow = hotkey.NewRow();
			hotkeyrow.ItemArray = new object[] { "UseVirtue", "Honesty", Keys.None, true };
			hotkey.Rows.Add(hotkeyrow);

			hotkeyrow = hotkey.NewRow();
			hotkeyrow.ItemArray = new object[] { "UseVirtue", "Humility", Keys.None, true };
			hotkey.Rows.Add(hotkeyrow);

			hotkeyrow = hotkey.NewRow();
			hotkeyrow.ItemArray = new object[] { "UseVirtue", "Justice", Keys.None, true };
			hotkey.Rows.Add(hotkeyrow);

			hotkeyrow = hotkey.NewRow();
			hotkeyrow.ItemArray = new object[] { "Target", "Target Self", Keys.None, true };
			hotkey.Rows.Add(hotkeyrow);

			hotkeyrow = hotkey.NewRow();
			hotkeyrow.ItemArray = new object[] { "Target", "Target Last", Keys.None, true };
			hotkey.Rows.Add(hotkeyrow);

			hotkeyrow = hotkey.NewRow();
			hotkeyrow.ItemArray = new object[] { "Target", "Target Cancel", Keys.None, true };
			hotkey.Rows.Add(hotkeyrow);

			hotkeyrow = hotkey.NewRow();
			hotkeyrow.ItemArray = new object[] { "Target", "Target Self Queued", Keys.None, true };
			hotkey.Rows.Add(hotkeyrow);

			hotkeyrow = hotkey.NewRow();
			hotkeyrow.ItemArray = new object[] { "Target", "Target Last Queued", Keys.None, true };
			hotkey.Rows.Add(hotkeyrow);

			hotkeyrow = hotkey.NewRow();
			hotkeyrow.ItemArray = new object[] { "Target", "Clear Target Queue", Keys.None, true };
			hotkey.Rows.Add(hotkeyrow);

			hotkeyrow = hotkey.NewRow();
			hotkeyrow.ItemArray = new object[] { "Target", "Clear Last Target", Keys.None, true };
			hotkey.Rows.Add(hotkeyrow);

			hotkeyrow = hotkey.NewRow();
			hotkeyrow.ItemArray = new object[] { "Target", "Clear Last and Queue", Keys.None, true };
			hotkey.Rows.Add(hotkeyrow);

			hotkeyrow = hotkey.NewRow();
			hotkeyrow.ItemArray = new object[] { "Target", "Set Last", Keys.None, true };
			hotkey.Rows.Add(hotkeyrow);

			hotkeyrow = hotkey.NewRow();
			hotkeyrow.ItemArray = new object[] { "Script", "Stop All", Keys.None, true };
			hotkey.Rows.Add(hotkeyrow);

			return hotkey;


		}
        internal static void initCleric(DataTable hotkey)
        {
            DataRow hotkeyrow = null;

            hotkeyrow = hotkey.NewRow();
            hotkeyrow.ItemArray = new object[] { "SpellsCleric", "Angelic Faith", Keys.None, true };
            hotkey.Rows.Add(hotkeyrow);
            hotkeyrow = hotkey.NewRow();
            hotkeyrow.ItemArray = new object[] { "SpellsCleric", "Banish Evil", Keys.None, true };
            hotkey.Rows.Add(hotkeyrow);
            hotkeyrow = hotkey.NewRow();
            hotkeyrow.ItemArray = new object[] { "SpellsCleric", "Dampen Spirit", Keys.None, true };
            hotkey.Rows.Add(hotkeyrow);
            hotkeyrow = hotkey.NewRow();
            hotkeyrow.ItemArray = new object[] { "SpellsCleric", "Divine Focus", Keys.None, true };
            hotkey.Rows.Add(hotkeyrow);
            hotkeyrow = hotkey.NewRow();
            hotkeyrow.ItemArray = new object[] { "SpellsCleric", "Hammer of Faith", Keys.None, true };
            hotkey.Rows.Add(hotkeyrow);
            hotkeyrow = hotkey.NewRow();
            hotkeyrow.ItemArray = new object[] { "SpellsCleric", "Purge", Keys.None, true };
            hotkey.Rows.Add(hotkeyrow);
            hotkeyrow = hotkey.NewRow();
            hotkeyrow.ItemArray = new object[] { "SpellsCleric", "Restoration", Keys.None, true };
            hotkey.Rows.Add(hotkeyrow);
            hotkeyrow = hotkey.NewRow();
            hotkeyrow.ItemArray = new object[] { "SpellsCleric", "Sacred Boon", Keys.None, true };
            hotkey.Rows.Add(hotkeyrow);
            hotkeyrow = hotkey.NewRow();
            hotkeyrow.ItemArray = new object[] { "SpellsCleric", "Sacrifice", Keys.None, true };
            hotkey.Rows.Add(hotkeyrow);
            hotkeyrow = hotkey.NewRow();
            hotkeyrow.ItemArray = new object[] { "SpellsCleric", "Smite", Keys.None, true };
            hotkey.Rows.Add(hotkeyrow);
            hotkeyrow = hotkey.NewRow();
            hotkeyrow.ItemArray = new object[] { "SpellsCleric", "Touch of Life", Keys.None, true };
            hotkey.Rows.Add(hotkeyrow);
            hotkeyrow = hotkey.NewRow();
            hotkeyrow.ItemArray = new object[] { "SpellsCleric", "Trial by Fire", Keys.None, true };
            hotkey.Rows.Add(hotkeyrow);
        }
        internal static void initDruid(DataTable hotkey)
        {
            DataRow hotkeyrow = null;

            hotkeyrow = hotkey.NewRow();
            hotkeyrow.ItemArray = new object[] { "SpellsDruid", "Leaf whirlwind", Keys.None, true };
            hotkey.Rows.Add(hotkeyrow);
            hotkeyrow = hotkey.NewRow();
            hotkeyrow.ItemArray = new object[] { "SpellsDruid", "Hollow Reed", Keys.None, true };
            hotkey.Rows.Add(hotkeyrow);
            hotkeyrow = hotkey.NewRow();
            hotkeyrow.ItemArray = new object[] { "SpellsDruid", "Pack of Beasts", Keys.None, true };
            hotkey.Rows.Add(hotkeyrow);
            hotkeyrow = hotkey.NewRow();
            hotkeyrow.ItemArray = new object[] { "SpellsDruid", "Spring of Life", Keys.None, true };
            hotkey.Rows.Add(hotkeyrow);
            hotkeyrow = hotkey.NewRow();
            hotkeyrow.ItemArray = new object[] { "SpellsDruid", "Grasping Roots", Keys.None, true };
            hotkey.Rows.Add(hotkeyrow);
            hotkeyrow = hotkey.NewRow();
            hotkeyrow.ItemArray = new object[] { "SpellsDruid", "Blend with Forest", Keys.None, true };
            hotkey.Rows.Add(hotkeyrow);
            hotkeyrow = hotkey.NewRow();
            hotkeyrow.ItemArray = new object[] { "SpellsDruid", "Swarm of Insects", Keys.None, true };
            hotkey.Rows.Add(hotkeyrow);
            hotkeyrow = hotkey.NewRow();
            hotkeyrow.ItemArray = new object[] { "SpellsDruid", "Volcanic Eruption", Keys.None, true };
            hotkey.Rows.Add(hotkeyrow);
            hotkeyrow = hotkey.NewRow();
            hotkeyrow.ItemArray = new object[] { "SpellsDruid", "Summon Familiar", Keys.None, true };
            hotkey.Rows.Add(hotkeyrow);
            hotkeyrow = hotkey.NewRow();
            hotkeyrow.ItemArray = new object[] { "SpellsDruid", "Stone Circle", Keys.None, true };
            hotkey.Rows.Add(hotkeyrow);
            hotkeyrow = hotkey.NewRow();
            hotkeyrow.ItemArray = new object[] { "SpellsDruid", "Enchanted Grove", Keys.None, true };
            hotkey.Rows.Add(hotkeyrow);
            hotkeyrow = hotkey.NewRow();
            hotkeyrow.ItemArray = new object[] { "SpellsDruid", "Lure Stone", Keys.None, true };
            hotkey.Rows.Add(hotkeyrow);
            hotkeyrow = hotkey.NewRow();
            hotkeyrow.ItemArray = new object[] { "SpellsDruid", "Hurricane", Keys.None, true };
            hotkey.Rows.Add(hotkeyrow);
            hotkeyrow = hotkey.NewRow();
            hotkeyrow.ItemArray = new object[] { "SpellsDruid", "Natures Passage", Keys.None, true };
            hotkey.Rows.Add(hotkeyrow);
            hotkeyrow = hotkey.NewRow();
            hotkeyrow.ItemArray = new object[] { "SpellsDruid", "Mushroom Gateway", Keys.None, true };
            hotkey.Rows.Add(hotkeyrow);
            hotkeyrow = hotkey.NewRow();
            hotkeyrow.ItemArray = new object[] { "SpellsDruid", "Restorative Soil", Keys.None, true };
            hotkey.Rows.Add(hotkeyrow);
            hotkeyrow = hotkey.NewRow();
            hotkeyrow.ItemArray = new object[] { "SpellsDruid", "Shield of Earth", Keys.None, true };
            hotkey.Rows.Add(hotkeyrow);
        }

        internal static DataTable InitGeneralSettings(string tableName)
		{
			// ----------- GENERAL SETTINGS ----------
			DataTable general = new DataTable(tableName);

			// Parametri Tab (Agent --> Heal)
			general.Columns.Add("BandageHealcountdownCheckBox", typeof(bool));
			general.Columns.Add("BandageHealtargetComboBox", typeof(string));
			general.Columns.Add("BandageHealtargetLabel", typeof(int));
			general.Columns.Add("BandageHealcustomCheckBox", typeof(bool));
			general.Columns.Add("BandageHealcustomIDTextBox", typeof(int));
			general.Columns.Add("BandageHealcustomcolorTextBox", typeof(int));
			general.Columns.Add("BandageHealdexformulaCheckBox", typeof(bool));
			general.Columns.Add("BandageHealdelayTextBox", typeof(int));
			general.Columns.Add("BandageHealhpTextBox", typeof(int));
			general.Columns.Add("BandageHealpoisonCheckBox", typeof(bool));
			general.Columns.Add("BandageHealmortalCheckBox", typeof(bool));
			general.Columns.Add("BandageHealhiddedCheckBox", typeof(bool));
			general.Columns.Add("BandageHealMaxRangeTextBox", typeof(int));
			general.Columns.Add("BandageHealUseTarget", typeof(bool));
            general.Columns.Add("BandageHealUseText", typeof(bool));
            general.Columns.Add("BandageHealUseTextContent", typeof(string));
            general.Columns.Add("BandageHealUseTextSelfContent", typeof(string));


            // Parametri Tab (Enhanced Filters)
            general.Columns.Add("HighlightTargetCheckBox", typeof(bool));
			general.Columns.Add("FlagsHighlightCheckBox", typeof(bool));
			general.Columns.Add("ShowStaticFieldCheckBox", typeof(bool));
			general.Columns.Add("BlockTradeRequestCheckBox", typeof(bool));
			general.Columns.Add("BlockPartyInviteCheckBox", typeof(bool));
			general.Columns.Add("MobFilterCheckBox", typeof(bool));
			general.Columns.Add("AutoCarverCheckBox", typeof(bool));
			general.Columns.Add("BoneCutterCheckBox", typeof(bool));
			general.Columns.Add("AutoCarverBladeLabel", typeof(int));
			general.Columns.Add("BoneBladeLabel", typeof(int));
			general.Columns.Add("ShowHeadTargetCheckBox", typeof(bool));
			general.Columns.Add("ColorFlagsHighlightCheckBox", typeof(bool));
			general.Columns.Add("BlockMiniHealCheckBox", typeof(bool));
			general.Columns.Add("BlockBigHealCheckBox", typeof(bool));
			general.Columns.Add("BlockChivalryHealCheckBox", typeof(bool));
			general.Columns.Add("ShowMessageFieldCheckBox", typeof(bool));
			general.Columns.Add("ShowAgentMessageCheckBox", typeof(bool));
			general.Columns.Add("ColorFlagsSelfHighlightCheckBox", typeof(bool));

			// Parametri Tab (Enhanced ToolBar)
			general.Columns.Add("LockToolBarCheckBox", typeof(bool));
			general.Columns.Add("AutoopenToolBarCheckBox", typeof(bool));
			general.Columns.Add("PosXToolBar", typeof(int));
			general.Columns.Add("PosYToolBar", typeof(int));
			general.Columns.Add("ToolBoxSlotsTextBox", typeof(int));
			general.Columns.Add("ToolBoxSizeComboBox", typeof(string));
			general.Columns.Add("ToolBoxStyleComboBox", typeof(string));
			general.Columns.Add("ShowFollowerToolBarCheckBox", typeof(bool));
			general.Columns.Add("ShowWeightToolBarCheckBox", typeof(bool));
			general.Columns.Add("ShowManaToolBarCheckBox", typeof(bool));
			general.Columns.Add("ShowStaminaToolBarCheckBox", typeof(bool));
			general.Columns.Add("ShowHitsToolBarCheckBox", typeof(bool));
            general.Columns.Add("ShowTitheToolBarCheckBox", typeof(bool));
            general.Columns.Add("ToolBarOpacity", typeof(int));

			// Parametri Tab (Enhanced Grid)
			general.Columns.Add("LockGridCheckBox", typeof(bool));
			general.Columns.Add("GridOpenLoginCheckBox", typeof(bool));
			general.Columns.Add("PosXGrid", typeof(int));
			general.Columns.Add("PosYGrid", typeof(int));
			general.Columns.Add("GridVSlot", typeof(int));
			general.Columns.Add("GridHSlot", typeof(int));
			general.Columns.Add("GridOpacity", typeof(int));

			// Parametri Tab (Screenshot)
			general.Columns.Add("CapPath", typeof(string));
			general.Columns.Add("ImageFormat", typeof(string));
			general.Columns.Add("CapFullScreen", typeof(bool));
			general.Columns.Add("CapTimeStamp", typeof(bool));
			general.Columns.Add("AutoCap", typeof(bool));

			// General Tab (Filter Sounds)
			general.Columns.Add(Convert.ToString((int)LocString.BardMusic), typeof(bool));
			general.Columns.Add(Convert.ToString((int)LocString.BirdSounds), typeof(bool));
			general.Columns.Add(Convert.ToString((int)LocString.BullSounds), typeof(bool));
			general.Columns.Add(Convert.ToString((int)LocString.CatSounds), typeof(bool));
			general.Columns.Add(Convert.ToString((int)LocString.ChickenSounds), typeof(bool));
			general.Columns.Add(Convert.ToString((int)LocString.CyclopTitanSounds), typeof(bool));
			general.Columns.Add(Convert.ToString((int)LocString.DeathStatus), typeof(bool));
			general.Columns.Add(Convert.ToString((int)LocString.DeerSounds), typeof(bool));
			general.Columns.Add(Convert.ToString((int)LocString.DogSounds), typeof(bool));
			general.Columns.Add(Convert.ToString((int)LocString.DragonSounds), typeof(bool));
			general.Columns.Add(Convert.ToString((int)LocString.FizzleSound), typeof(bool));
			general.Columns.Add(Convert.ToString((int)LocString.HorseSounds), typeof(bool));
			general.Columns.Add(Convert.ToString((int)LocString.LightFilter), typeof(bool));
			general.Columns.Add(Convert.ToString((int)LocString.PackSound), typeof(bool));
			general.Columns.Add(Convert.ToString((int)LocString.SS_Sound), typeof(bool));
			general.Columns.Add(Convert.ToString((int)LocString.SheepSounds), typeof(bool));
			general.Columns.Add(Convert.ToString((int)LocString.StaffOnlyItems), typeof(bool));
			general.Columns.Add(Convert.ToString((int)LocString.StaffOnlyNpcs), typeof(bool));
			general.Columns.Add(Convert.ToString((int)LocString.VetRewardGump), typeof(bool));
			general.Columns.Add(Convert.ToString((int)LocString.Weather), typeof(bool));

			// Parametri Tab (General)
			general.Columns.Add("SmartCPU", typeof(bool));
			general.Columns.Add("AlwaysOnTop", typeof(bool));
			general.Columns.Add("RememberPwds", typeof(bool));
			general.Columns.Add("Systray", typeof(bool));
			general.Columns.Add("ForceSizeEnabled", typeof(bool));
			general.Columns.Add("ForceSizeX", typeof(int));
			general.Columns.Add("ForceSizeY", typeof(int));
			general.Columns.Add("ClientPrio", typeof(string));
			general.Columns.Add("Opacity", typeof(int));
			general.Columns.Add("WindowX", typeof(int));
			general.Columns.Add("WindowY", typeof(int));
			general.Columns.Add("NotShowLauncher", typeof(bool));

			// Parametri Tab (Skill)
			general.Columns.Add("DisplaySkillChanges", typeof(bool));
			general.Columns.Add("SkillListAsc", typeof(bool));
			general.Columns.Add("SkillListCol", typeof(int));

			// Parametri Tab (Options)
			general.Columns.Add("ActionStatusMsg", typeof(bool));
			general.Columns.Add("QueueActions", typeof(bool));
			general.Columns.Add("ObjectDelay", typeof(int));
			general.Columns.Add("SmartLastTarget", typeof(bool));
			general.Columns.Add("RangeCheckLT", typeof(bool));
			general.Columns.Add("LTRange", typeof(int));
			general.Columns.Add("LastTargTextFlags", typeof(bool));
			general.Columns.Add("ShowHealth", typeof(bool));
			general.Columns.Add("HealthFmt", typeof(string));
			general.Columns.Add("ShowPartyStats", typeof(bool));
			general.Columns.Add("OldStatBar", typeof(bool));
			general.Columns.Add("QueueTargets", typeof(bool));
			general.Columns.Add("BlockDismount", typeof(bool));
			general.Columns.Add("AutoStack", typeof(bool));
			general.Columns.Add("AutoOpenCorpses", typeof(bool));
            general.Columns.Add("AllowHiddenLooting", typeof(bool));
            general.Columns.Add("CorpseRange", typeof(int));
			general.Columns.Add("FilterSpam", typeof(bool));
			general.Columns.Add("FilterSnoopMsg", typeof(bool));
			general.Columns.Add("ShowMobNames", typeof(bool));
			general.Columns.Add("Negotiate", typeof(bool));
			general.Columns.Add("ShowCorpseNames", typeof(bool));
			general.Columns.Add("CountStealthSteps", typeof(bool));
			general.Columns.Add("AlwaysStealth", typeof(bool));
			general.Columns.Add("AutoOpenDoors", typeof(bool));
			general.Columns.Add("SpellUnequip", typeof(bool));
			general.Columns.Add("PotionEquip", typeof(bool));
			general.Columns.Add("ForceSpeechHue", typeof(bool));
			general.Columns.Add("ForceSpellHue", typeof(bool));
			general.Columns.Add("SpellFormat", typeof(string));
			general.Columns.Add("MessageLevel", typeof(int));
			general.Columns.Add("HiddedAutoOpenDoors", typeof(bool));
			general.Columns.Add("UO3DEquipUnEquip", typeof(bool));
			general.Columns.Add("ChkNoRunStealth", typeof(bool));
			general.Columns.Add("FilterPoison", typeof(bool));
			general.Columns.Add("EnhancedMapPath", typeof(string));
			general.Columns.Add("FilterNPC", typeof(bool));

            // Parametri Tab (Options -> Hues)
            general.Columns.Add("LTHilight", typeof(int));
			general.Columns.Add("NeutralSpellHue", typeof(int));
			general.Columns.Add("HarmfulSpellHue", typeof(int));
			general.Columns.Add("BeneficialSpellHue", typeof(int));
			general.Columns.Add("SpeechHue", typeof(int));
			general.Columns.Add("ExemptColor", typeof(int));
			general.Columns.Add("WarningColor", typeof(int));
			general.Columns.Add("SysColor", typeof(int));

			// Parametri Tab (HotKey)
			general.Columns.Add("HotKeyEnable", typeof(bool));
			general.Columns.Add("HotKeyMasterKey", typeof(Keys));

			// Parametri Interni
			general.Columns.Add("PartyStatFmt", typeof(string));
			general.Columns.Add("ForcePort", typeof(int));
			general.Columns.Add("ForceIP", typeof(string));
			general.Columns.Add("BlockHealPoison", typeof(bool));
			general.Columns.Add("AutoSearch", typeof(bool));
			general.Columns.Add("NoSearchPouches", typeof(bool));
            general.Columns.Add("DruidClericPackets", typeof(bool));

            // Parametri Mappa
            general.Columns.Add("MapX", typeof(int));
			general.Columns.Add("MapY", typeof(int));
			general.Columns.Add("MapW", typeof(int));
			general.Columns.Add("MapH", typeof(int));

			// Parametri Enhanced Map
			general.Columns.Add("MapOpenOnLoginCheckBox", typeof(bool));
			general.Columns.Add("MapAutoConnectCheckBox", typeof(bool));
			general.Columns.Add("MapHpBarCheckBox", typeof(bool));
			general.Columns.Add("MapStaminaBarCheckBox", typeof(bool));
			general.Columns.Add("MapManaBarCheckBox", typeof(bool));
			general.Columns.Add("MapDeathPointCheckBox", typeof(bool));
			general.Columns.Add("MapPanicCheckBox", typeof(bool));
			general.Columns.Add("MapPartyMemberCheckBox", typeof(bool));
			general.Columns.Add("MapGuildCheckBox", typeof(bool));
			general.Columns.Add("MapServerCheckBox", typeof(bool));
			general.Columns.Add("MapChatCheckBox", typeof(bool));
			general.Columns.Add("MapChatPrefixTextBox", typeof(string));
			general.Columns.Add("MapAutoOpenChatCheckBox", typeof(bool));
			general.Columns.Add("MapChatColor", typeof(int));

			general.Columns.Add("MapServerAddressTextBox", typeof(string));
			general.Columns.Add("MapServerPortTextBox", typeof(string));
			general.Columns.Add("MapLinkUsernameTextBox", typeof(string));
			general.Columns.Add("MapLinkPasswordTextBox", typeof(string));

			// Setting Version
			general.Columns.Add("SettingVersion", typeof(int));

			// Parametri AutoRemount
			general.Columns.Add("MountSerial", typeof(int));
			general.Columns.Add("MountDelay", typeof(int));
			general.Columns.Add("EMountDelay", typeof(int));
			general.Columns.Add("RemountCheckbox", typeof(bool));

			// Parametri UoMod
			general.Columns.Add("UoModFPS", typeof(bool));
			general.Columns.Add("UoModPaperdoll", typeof(bool));
			general.Columns.Add("UoModSound", typeof(bool));

			// Parametri Video Recorder
			general.Columns.Add("VideoPath", typeof(string));
			general.Columns.Add("VideoFPS", typeof(int));
			general.Columns.Add("VideoResolution", typeof(string));
			general.Columns.Add("VideoFormat", typeof(int));
			general.Columns.Add("VideoCompression", typeof(int));
			general.Columns.Add("VideoFlipV", typeof(bool));
			general.Columns.Add("VideoFlipH", typeof(bool));
			general.Columns.Add("VideoTimestamp", typeof(bool));

			// Parametri finestra script
			general.Columns.Add("ShowScriptMessageCheckBox", typeof(bool));
			general.Columns.Add("ScriptErrorLog", typeof(bool));
			general.Columns.Add("ScriptStartStopMessage", typeof(bool));

			// Parametri AgentAutostart
			general.Columns.Add("ScavengerAutostartCheckBox", typeof(bool));
			general.Columns.Add("AutolootAutostartCheckBox", typeof(bool));
			general.Columns.Add("BandageHealAutostartCheckBox", typeof(bool));

			// Composizione Parematri base primo avvio
			object[] generalstartparam = new object[] {
                    // Parametri primo avvio per tab agent Bandage heal
                    false, "Self", 0, false, 0, 0, false, 1000, 100, false, false, false, 1, true, false, "[band", "[bandself",

                    // Parametri primo avvio per tab Enhanced Filters
                    false, false, false, false, false, false, false, false, 0, 0, false, false, false, false, false, true, true, false,

                    // Parametri primo avvio per tab Enhanced ToolBar
                    false, false, 10, 10, 2, "Big", "Vertical", true, true, true, true, true, false, 100,

                    // Parametri primo avvio per tab Enhanced Grid
                    false, false, 10, 10, 2, 2, 100,

                    // Parametri primo avvio per tab screenshot
                    Assistant.Engine.RootPath, "jpg", false, false, false,

                    // Sound Filters
                    false, false, false, false, false, false, false, false, false, false, false, false, false, false, false,
					false, false, false, false, false,

                    // Parametri primo avvio tab general
                    false, false, false, false, false, 800, 600, "Normal", 100, 400, 400, false,

                    // Parametri primo avvio tab skill
                    false, false, -1,

                    // Parametri primo avvio tab Options
                    false, false, 600,
					false, false, 12,
					false, false, "[{0}%]",
					false, false, false,
					false, false, false, false, 2,
					false, false, false, false, false, false, false, false, false,
					false, false, false, @"{power} [{spell}]", 0, false, false, false, false, string.Empty, false,

                    // Parametri primo avvio tab Options -> Hues
                    (int)0, (int)0x03B1, (int)0x0025, (int)0x0005, (int)0x03B1, (int)0x0480, (int)0x0025, (int)0x03B1,

                    // Parametri primo avvio tab HotKey
                    true, Keys.None,

                    // Parametri primo avvio interni
                    "[{0}% / {1}%]", 0, String.Empty, false, false, true, false,

                     // Parametri primo avvio Mappa
                     200,200,200,200,

                     // Parametri primo avvio enchanced map
                     false, false, true, false, false, true, true, true, true, true, false, "--", false, 0, "0.0.0.0", "0", String.Empty, String.Empty,

                     // Versione Corrente
                     SettingVersion,

                     // Parametri AutoRemount
                     0, 1000, 1000, false,

                     // Parametri UoMod
                     false, false, false,

					 // Parametri Video Recorder
                     Assistant.Engine.RootPath, 25, "Full Size", 1, 100, false, false, false,

					 // Parametri finestra script
                     true, false, false,

					 // Parametri AgentAutostart
                     false, false, false
				};

            DataRow generalsettings = general.NewRow();
            generalsettings.ItemArray = generalstartparam;
            general.Rows.Add(generalsettings);

            return general;

		}



		internal static void Load(string profileName, bool try_recover=true)
		{
			// Parametri di razor
			//if (profileName == "default")
			//	RazorEnhanced.Settings.ProfileFiles = "RazorEnhanced.settings";
			//else
				//RazorEnhanced.Settings.ProfileFiles = "RazorEnhanced." + RazorEnhanced.Profiles.LastUsed() + ".settings";

			if (m_Dataset != null)
				m_Dataset.Clear();

			m_profileName = profileName;
			m_Dataset = new DataSet();

			if (! LoadExistingData(profileName, try_recover))
			{
				m_Dataset.Tables.Add(initDict["SCRIPTING"]("SCRIPTING"));

				m_Dataset.Tables.Add(initDict["AUTOLOOT_ITEMS"]("AUTOLOOT_ITEMS"));
				m_Dataset.Tables.Add(initDict["AUTOLOOT_LISTS"]("AUTOLOOT_LISTS"));

				m_Dataset.Tables.Add(initDict["SCAVENGER_LISTS"]("SCAVENGER_LISTS"));
				m_Dataset.Tables.Add(initDict["SCAVENGER_ITEMS"]("SCAVENGER_ITEMS"));

				m_Dataset.Tables.Add(initDict["ORGANIZER_ITEMS"]("ORGANIZER_ITEMS"));
				m_Dataset.Tables.Add(initDict["ORGANIZER_LISTS"]("ORGANIZER_LISTS"));

				m_Dataset.Tables.Add(initDict["SELL_ITEMS"]("SELL_ITEMS"));
				m_Dataset.Tables.Add(initDict["SELL_LISTS"]("SELL_LISTS"));

				m_Dataset.Tables.Add(initDict["BUY_ITEMS"]("BUY_ITEMS"));
				m_Dataset.Tables.Add(initDict["BUY_LISTS"]("BUY_LISTS"));

				m_Dataset.Tables.Add(initDict["DRESS_ITEMS"]("DRESS_ITEMS"));
				m_Dataset.Tables.Add(initDict["DRESS_LISTS"]("DRESS_LISTS"));

				m_Dataset.Tables.Add(initDict["FRIEND_PLAYERS"]("FRIEND_PLAYERS"));
				m_Dataset.Tables.Add(initDict["FRIEND_GUILDS"]("FRIEND_GUILDS"));
				m_Dataset.Tables.Add(initDict["FRIEND_LISTS"]("FRIEND_LISTS"));

				m_Dataset.Tables.Add(initDict["RESTOCK_ITEMS"]("RESTOCK_ITEMS"));
				m_Dataset.Tables.Add(initDict["RESTOCK_LISTS"]("RESTOCK_LISTS"));

				m_Dataset.Tables.Add(initDict["TARGETS"]("TARGETS"));
				m_Dataset.Tables.Add(initDict["FILTER_GRAPH"]("FILTER_GRAPH"));
				m_Dataset.Tables.Add(initDict["TOOLBAR_ITEMS"]("TOOLBAR_ITEMS"));
				m_Dataset.Tables.Add(initDict["SPELLGRID_ITEMS"]("SPELLGRID_ITEMS"));
				m_Dataset.Tables.Add(initDict["PASSWORD"]("PASSWORD"));
				m_Dataset.Tables.Add(initDict["HOTKEYS"]("HOTKEYS"));
				m_Dataset.Tables.Add(initDict["GENERAL"]("GENERAL"));

				m_Dataset.AcceptChanges();
			}
		}


		// ------------- AUTOLOOT -----------------
		internal class AutoLoot
		{
			internal static bool ListExists(string description)
			{
				return m_Dataset.Tables["AUTOLOOT_LISTS"].Rows.Cast<DataRow>().Any(row => ((string) row["Description"]).ToLower() == description.ToLower());
			}

			internal static void ListInsert(string description, int delay, int bag, bool noopencorpse, int maxrange)
			{
				foreach (DataRow row in m_Dataset.Tables["AUTOLOOT_LISTS"].Rows)
				{
					row["Selected"] = false;
				}

				DataRow newRow = m_Dataset.Tables["AUTOLOOT_LISTS"].NewRow();
				newRow["Description"] = description;
				newRow["Delay"] = delay;
				newRow["Bag"] = bag;
				newRow["Selected"] = true;
				newRow["NoOpenCorpse"] = noopencorpse;
				newRow["Range"] = maxrange;
				m_Dataset.Tables["AUTOLOOT_LISTS"].Rows.Add(newRow);

				Save();
			}

			internal static void ListUpdate(string description, int delay, int bag, bool selected, bool noopencorpse, int maxrange)
			{
				bool found = m_Dataset.Tables["AUTOLOOT_LISTS"].Rows.Cast<DataRow>().Any(row => (string) row["Description"] == description);

				if (found)
				{
					if (selected)
					{
						foreach (DataRow row in m_Dataset.Tables["AUTOLOOT_LISTS"].Rows)
						{
							row["Selected"] = false;
						}
					}

					foreach (DataRow row in m_Dataset.Tables["AUTOLOOT_LISTS"].Rows)
					{
						if ((string)row["Description"] == description)
						{
							row["Delay"] = delay;
							row["Bag"] = bag;
							row["Selected"] = selected;
							row["NoOpenCorpse"] = noopencorpse;
							row["Range"] = maxrange;
							break;
						}
					}

					Save();
				}
			}

			internal static void ClearList(string list)
			{
				for (int i = m_Dataset.Tables["AUTOLOOT_ITEMS"].Rows.Count - 1; i >= 0; i--)
				{
					DataRow row = m_Dataset.Tables["AUTOLOOT_ITEMS"].Rows[i];
                    if (row.RowState != DataRowState.Deleted && (string)row["List"] == list)
						    row.Delete();
				}
			}

			internal static void ListDelete(string description)
			{
				ClearList(description);

				for (int i = m_Dataset.Tables["AUTOLOOT_LISTS"].Rows.Count - 1; i >= 0; i--)
				{
					DataRow row = m_Dataset.Tables["AUTOLOOT_LISTS"].Rows[i];
					if ((string)row["Description"] == description)
					{
						row.Delete();
						break;
					}
					row["Selected"] = false;
				}

				Save();
			}

			internal static List<RazorEnhanced.AutoLoot.AutoLootList> ListsRead()
			{
				List<RazorEnhanced.AutoLoot.AutoLootList> lists = new List<RazorEnhanced.AutoLoot.AutoLootList>();

				foreach (DataRow row in m_Dataset.Tables["AUTOLOOT_LISTS"].Rows)
				{
					string description = (string)row["Description"];
					int delay = Convert.ToInt32(row["Delay"]);
					int bag = Convert.ToInt32(row["Bag"]);
					bool selected = (bool)row["Selected"];
					bool noopencorspe = (bool)row["NoOpenCorpse"];
					int range = Convert.ToInt32(row["Range"]);

					RazorEnhanced.AutoLoot.AutoLootList list = new RazorEnhanced.AutoLoot.AutoLootList(description, delay, bag, selected, noopencorspe, range);
					lists.Add(list);
				}

				return lists;
			}

			internal static void ItemInsert(string list, RazorEnhanced.AutoLoot.AutoLootItem item)
			{
				DataRow row = m_Dataset.Tables["AUTOLOOT_ITEMS"].NewRow();
				row["List"] = list;
				row["Item"] = item;
				m_Dataset.Tables["AUTOLOOT_ITEMS"].Rows.Add(row);
			}

			internal static void ItemInsertFromImport(string list, List<RazorEnhanced.AutoLoot.AutoLootItem> itemlist)
			{
				foreach (RazorEnhanced.AutoLoot.AutoLootItem item in itemlist)
				{
					DataRow row = m_Dataset.Tables["AUTOLOOT_ITEMS"].NewRow();
					row["List"] = list;
					row["Item"] = item;
					m_Dataset.Tables["AUTOLOOT_ITEMS"].Rows.Add(row);
				}
				Save();
			}

			internal static Dictionary<int, List<RazorEnhanced.AutoLoot.AutoLootItem>> ItemsRead(string list)
			{
                Dictionary<int, List<RazorEnhanced.AutoLoot.AutoLootItem>> lootList = new Dictionary<int, List<RazorEnhanced.AutoLoot.AutoLootItem>>();

                if (RazorEnhanced.AutoLoot.LockTable)
					return lootList;

				if (ListExists(list))
				{
					foreach (DataRow row in m_Dataset.Tables["AUTOLOOT_ITEMS"].Rows)
					{
                        if (row.RowState != DataRowState.Deleted && row.RowState != DataRowState.Detached && (string)row["List"] == list)
                        {
                            RazorEnhanced.AutoLoot.AutoLootItem autoLootItem = ((RazorEnhanced.AutoLoot.AutoLootItem)row["Item"]);
                            List<RazorEnhanced.AutoLoot.AutoLootItem> autoLootItems = null;
                            if (lootList.TryGetValue(autoLootItem.Graphics, out autoLootItems))
                            {
                                autoLootItems.Add(autoLootItem);
                                lootList[autoLootItem.Graphics] = autoLootItems;

                            }
                            else
                            {
                                autoLootItems = new List<RazorEnhanced.AutoLoot.AutoLootItem>();
                                autoLootItems.Add(autoLootItem);
                                lootList.Add(autoLootItem.Graphics, autoLootItems);
                            }
                        }
                    }
				}
                return lootList;
            }

			internal static void ListDetailsRead(string listname, out int bag, out int delay, out bool noopencorpse, out int range)
			{
				bag = 0;
				delay = 0;
				range = 0;
				noopencorpse = false;

                foreach (DataRow row in m_Dataset.Tables["AUTOLOOT_LISTS"].Rows)
				{
					if ((string)row["Description"] == listname)
					{
						bag = Convert.ToInt32(row["Bag"]);
						delay = Convert.ToInt32(row["Delay"]);
						noopencorpse = (bool)row["NoOpenCorpse"];
						range = Convert.ToInt32(row["Range"]);
					}
				}
			}
		}

		// ------------- AUTOLOOT END-----------------

		// ------------- SCAVENGER -----------------
		internal class Scavenger
		{
			internal static bool ListExists(string description)
			{
                try
                {
                    return m_Dataset.Tables["SCAVENGER_LISTS"].Rows.Cast<DataRow>().Any(row => ((string)row["Description"]).ToLower() == description.ToLower());
                }
                catch (Exception ex)
                {
                    return false;
                }
			}

			internal static void ListInsert(string description, int delay, int bag, int range)
			{
				foreach (DataRow row in m_Dataset.Tables["SCAVENGER_LISTS"].Rows)
				{
					row["Selected"] = false;
				}

				DataRow newRow = m_Dataset.Tables["SCAVENGER_LISTS"].NewRow();
				newRow["Description"] = description;
				newRow["Delay"] = delay;
				newRow["Bag"] = bag;
				newRow["Selected"] = true;
				newRow["Range"] = range;
				m_Dataset.Tables["SCAVENGER_LISTS"].Rows.Add(newRow);

				Save();
			}

			internal static void ListUpdate(string description, int delay, int bag, bool selected, int range)
			{
				bool found = m_Dataset.Tables["SCAVENGER_LISTS"].Rows.Cast<DataRow>().Any(row => (string) row["Description"] == description);

				if (found)
				{
					if (selected)
					{
						foreach (DataRow row in m_Dataset.Tables["SCAVENGER_LISTS"].Rows)
						{
							row["Selected"] = false;
						}
					}

					foreach (DataRow row in m_Dataset.Tables["SCAVENGER_LISTS"].Rows)
					{
						if ((string)row["Description"] == description)
						{
							row["Delay"] = delay;
							row["Bag"] = bag;
							row["Selected"] = selected;
							row["Range"] = range;
							break;
						}
					}

					Save();
				}
			}

			internal static void ClearList(string list)
			{
				for (int i = m_Dataset.Tables["SCAVENGER_ITEMS"].Rows.Count - 1; i >= 0; i--)
				{
					DataRow row = m_Dataset.Tables["SCAVENGER_ITEMS"].Rows[i];
					if (row.RowState != DataRowState.Deleted)
					{
						if ((string)row["List"] == list)
							row.Delete();
					}
				}
			}

			internal static void ListDelete(string description)
			{
				ClearList(description);

				for (int i = m_Dataset.Tables["SCAVENGER_LISTS"].Rows.Count - 1; i >= 0; i--)
				{
					DataRow row = m_Dataset.Tables["SCAVENGER_LISTS"].Rows[i];
					if ((string)row["Description"] == description)
					{
						row.Delete();
						break;
					}
					row["Selected"] = false;
				}

				Save();
			}

			internal static List<RazorEnhanced.Scavenger.ScavengerList> ListsRead()
			{
				List<RazorEnhanced.Scavenger.ScavengerList> lists = new List<RazorEnhanced.Scavenger.ScavengerList>();

				foreach (DataRow row in m_Dataset.Tables["SCAVENGER_LISTS"].Rows)
				{
					string description = (string)row["Description"];
					int delay = Convert.ToInt32(row["Delay"]);
					int bag = Convert.ToInt32(row["Bag"]);
					bool selected = (bool)row["Selected"];
					int range = Convert.ToInt32(row["Range"]);

					RazorEnhanced.Scavenger.ScavengerList list = new RazorEnhanced.Scavenger.ScavengerList(description, delay, bag, selected, range);
					lists.Add(list);
				}

				return lists;
			}

			internal static void ItemInsert(string list, RazorEnhanced.Scavenger.ScavengerItem item)
			{
				DataRow row = m_Dataset.Tables["SCAVENGER_ITEMS"].NewRow();
				row["List"] = list;
				row["Item"] = item;
				m_Dataset.Tables["SCAVENGER_ITEMS"].Rows.Add(row);
			}

			internal static void ItemInsertFromImport(string list, List<RazorEnhanced.Scavenger.ScavengerItem> itemlist)
			{
				foreach (RazorEnhanced.Scavenger.ScavengerItem item in itemlist)
				{
					DataRow row = m_Dataset.Tables["SCAVENGER_ITEMS"].NewRow();
					row["List"] = list;
					row["Item"] = item;
					m_Dataset.Tables["SCAVENGER_ITEMS"].Rows.Add(row);
				}
				Save();
			}

			internal static List<RazorEnhanced.Scavenger.ScavengerItem> ItemsRead(string list)
			{
				List<RazorEnhanced.Scavenger.ScavengerItem> items = new List<RazorEnhanced.Scavenger.ScavengerItem>();

				if (RazorEnhanced.Scavenger.LockTable)
					return items;

				if (ListExists(list))
				{
					foreach (DataRow row in m_Dataset.Tables["SCAVENGER_ITEMS"].Rows)
					{
						if (row.RowState != DataRowState.Deleted && row.RowState != DataRowState.Detached && (string)row["List"] == list)
							items.Add((RazorEnhanced.Scavenger.ScavengerItem)row["Item"]);
					}
				}

				return items;
            }

			internal static void ListDetailsRead(string listname, out int bag, out int delay, out int range)
			{
				bag = 0;
				delay = 0;
				range = 0;
				foreach (DataRow row in m_Dataset.Tables["SCAVENGER_LISTS"].Rows)
				{
					if ((string)row["Description"] == listname)
					{
						bag = Convert.ToInt32(row["Bag"]);
						delay = Convert.ToInt32(row["Delay"]);
						range = Convert.ToInt32(row["Range"]);
					}
				}
			}
		}

		// ------------- SCAVENGER END-----------------

		// ------------- ORGANIZER -----------------

		internal class Organizer
		{
			internal static bool ListExists(string description)
			{
				return m_Dataset.Tables["ORGANIZER_LISTS"].Rows.Cast<DataRow>().Any(row => ((string) row["Description"]).ToLower() == description.ToLower());
			}

			internal static void ListInsert(string description, int delay, int source, int destination)
			{
				foreach (DataRow row in m_Dataset.Tables["ORGANIZER_LISTS"].Rows)
				{
					row["Selected"] = false;
				}

				DataRow newRow = m_Dataset.Tables["ORGANIZER_LISTS"].NewRow();
				newRow["Description"] = description;
				newRow["Delay"] = delay;
				newRow["Source"] = source;
				newRow["Destination"] = destination;
				newRow["Selected"] = true;
				m_Dataset.Tables["ORGANIZER_LISTS"].Rows.Add(newRow);

				Save();
			}

			internal static void ListUpdate(string description, int delay, int source, int destination, bool selected)
			{
				bool found = m_Dataset.Tables["ORGANIZER_LISTS"].Rows.Cast<DataRow>().Any(row => (string) row["Description"] == description);

				if (found)
				{
					if (selected)
					{
						foreach (DataRow row in m_Dataset.Tables["ORGANIZER_LISTS"].Rows)
						{
							row["Selected"] = false;
						}
					}

					foreach (DataRow row in m_Dataset.Tables["ORGANIZER_LISTS"].Rows)
					{
						if ((string)row["Description"] == description)
						{
							row["Delay"] = delay;
							row["Source"] = source;
							row["Destination"] = destination;
							row["Selected"] = selected;
							break;
						}
					}

					Save();
				}
			}

			internal static void ClearList(string list)
			{
				for (int i = m_Dataset.Tables["ORGANIZER_ITEMS"].Rows.Count - 1; i >= 0; i--)
				{
					DataRow row = m_Dataset.Tables["ORGANIZER_ITEMS"].Rows[i];
					if ((string)row["List"] == list)
						row.Delete();
				}
			}

			internal static void ListDelete(string description)
			{
				ClearList(description);

                for (int i = m_Dataset.Tables["ORGANIZER_LISTS"].Rows.Count - 1; i >= 0; i--)
				{
					DataRow row = m_Dataset.Tables["ORGANIZER_LISTS"].Rows[i];
					if ((string)row["Description"] == description)
					{
						row.Delete();
						break;
					}
					row["Selected"] = false;
				}

				Save();
			}

			internal static List<RazorEnhanced.Organizer.OrganizerList> ListsRead()
			{
				List<RazorEnhanced.Organizer.OrganizerList> lists = new List<RazorEnhanced.Organizer.OrganizerList>();

				foreach (DataRow row in m_Dataset.Tables["ORGANIZER_LISTS"].Rows)
				{
					string description = (string)row["Description"];
					int delay = Convert.ToInt32(row["Delay"]);
					int source = Convert.ToInt32(row["Source"]);
					int destination = Convert.ToInt32(row["Destination"]);
					bool selected = (bool)row["Selected"];

					RazorEnhanced.Organizer.OrganizerList list = new RazorEnhanced.Organizer.OrganizerList(description, delay, source, destination, selected);
					lists.Add(list);
				}

				return lists;
			}

			internal static void ItemInsert(string list, RazorEnhanced.Organizer.OrganizerItem item)
			{
				DataRow row = m_Dataset.Tables["ORGANIZER_ITEMS"].NewRow();
				row["List"] = list;
				row["Item"] = item;
				m_Dataset.Tables["ORGANIZER_ITEMS"].Rows.Add(row);
			}

			internal static void ItemInsertFromImport(string list, List<RazorEnhanced.Organizer.OrganizerItem> itemlist)
			{
				foreach (RazorEnhanced.Organizer.OrganizerItem item in itemlist)
				{
					DataRow row = m_Dataset.Tables["ORGANIZER_ITEMS"].NewRow();
					row["List"] = list;
					row["Item"] = item;
					m_Dataset.Tables["ORGANIZER_ITEMS"].Rows.Add(row);
				}
				Save();
			}

			internal static List<RazorEnhanced.Organizer.OrganizerItem> ItemsRead(string list)
			{
				List<RazorEnhanced.Organizer.OrganizerItem> items = new List<RazorEnhanced.Organizer.OrganizerItem>();

				if (ListExists(list))
				{
					items.AddRange(from DataRow row in m_Dataset.Tables["ORGANIZER_ITEMS"].Rows where (string) row["List"] == list select (RazorEnhanced.Organizer.OrganizerItem) row["Item"]);
				}

				return items;
            }

			internal static void ListDetailsRead(string listname, out int bags, out int bagd, out int delay)
			{
				bags = 0;
				bagd = 0;
				delay = 0;
				foreach (DataRow row in m_Dataset.Tables["ORGANIZER_LISTS"].Rows)
				{
					if ((string)row["Description"] == listname)
					{
						bags = Convert.ToInt32(row["Source"]);
						bagd = Convert.ToInt32(row["Destination"]);
						delay = Convert.ToInt32(row["Delay"]);
					}
				}
			}
		}

		// ------------- ORGANIZER END-----------------

		// ------------- SELL AGENT ----------------

		internal class SellAgent
		{
			internal static bool ListExists(string description)
			{
				return m_Dataset.Tables["SELL_LISTS"].Rows.Cast<DataRow>().Any(row => ((string) row["Description"]).ToLower() == description.ToLower());
			}

			internal static void ListInsert(string description, int bag)
			{
				foreach (DataRow row in m_Dataset.Tables["SELL_LISTS"].Rows)
				{
					row["Selected"] = false;
				}

				DataRow newRow = m_Dataset.Tables["SELL_LISTS"].NewRow();
				newRow["Description"] = description;
				newRow["Bag"] = bag;
				newRow["Selected"] = true;
				m_Dataset.Tables["SELL_LISTS"].Rows.Add(newRow);

				Save();
			}

			internal static void ListUpdate(string description, int bag, bool selected)
			{
				bool found = m_Dataset.Tables["SELL_LISTS"].Rows.Cast<DataRow>().Any(row => (string) row["Description"] == description);

				if (found)
				{
					if (selected)
					{
						foreach (DataRow row in m_Dataset.Tables["SELL_LISTS"].Rows)
						{
							row["Selected"] = false;
						}
					}

					foreach (DataRow row in m_Dataset.Tables["SELL_LISTS"].Rows)
					{
						if ((string)row["Description"] == description)
						{
							row["Bag"] = bag;
							row["Selected"] = selected;
							break;
						}
					}

					Save();
				}
			}

			internal static void ListDelete(string description)
			{
				ClearList(description);

				for (int i = m_Dataset.Tables["SELL_LISTS"].Rows.Count - 1; i >= 0; i--)
				{
					DataRow row = m_Dataset.Tables["SELL_LISTS"].Rows[i];
					if ((string)row["Description"] == description)
					{
						row.Delete();
						break;
					}
					row["Selected"] = false;
				}

				Save();
			}

			internal static List<RazorEnhanced.SellAgent.SellAgentList> ListsRead()
			{
				List<RazorEnhanced.SellAgent.SellAgentList> lists = new List<RazorEnhanced.SellAgent.SellAgentList>();

				foreach (DataRow row in m_Dataset.Tables["SELL_LISTS"].Rows)
				{
					string description = (string)row["Description"];
					int bag = Convert.ToInt32(row["Bag"]);
					bool selected = (bool)row["Selected"];

					RazorEnhanced.SellAgent.SellAgentList list = new RazorEnhanced.SellAgent.SellAgentList(description, bag, selected);
					lists.Add(list);
				}

				return lists;
			}

			internal static int BagRead(string listname)
			{
				return (from DataRow row in m_Dataset.Tables["SELL_LISTS"].Rows where (string) row["Description"] == listname select Convert.ToInt32(row["Bag"])).FirstOrDefault();
			}

			internal static void ItemInsert(string list, RazorEnhanced.SellAgent.SellAgentItem item)
			{
				DataRow row = m_Dataset.Tables["SELL_ITEMS"].NewRow();
				row["List"] = list;
				row["Item"] = item;
				m_Dataset.Tables["SELL_ITEMS"].Rows.Add(row);
			}

			internal static void ItemInsertFromImport(string list, List<RazorEnhanced.SellAgent.SellAgentItem> itemlist)
			{
				foreach (RazorEnhanced.SellAgent.SellAgentItem item in itemlist)
				{
					DataRow row = m_Dataset.Tables["SELL_ITEMS"].NewRow();
					row["List"] = list;
					row["Item"] = item;
					m_Dataset.Tables["SELL_ITEMS"].Rows.Add(row);
				}
				Save();
			}

			internal static void ClearList(string list)
			{
				for (int i = m_Dataset.Tables["SELL_ITEMS"].Rows.Count - 1; i >= 0; i--)
				{
					DataRow row = m_Dataset.Tables["SELL_ITEMS"].Rows[i];
					if ((string)row["List"] == list)
						row.Delete();
				}
			}

			internal static List<RazorEnhanced.SellAgent.SellAgentItem> ItemsRead(string list)
			{
				List<RazorEnhanced.SellAgent.SellAgentItem> items = new List<RazorEnhanced.SellAgent.SellAgentItem>();

				if (ListExists(list))
				{
					foreach (DataRow row in m_Dataset.Tables["SELL_ITEMS"].Rows)
					{
						if ((string)row["List"] == list)
						{
							items.Add((RazorEnhanced.SellAgent.SellAgentItem)row["Item"]);
						}
					}
				}
				return items;
            }
		}

		// ------------- SELL AGENT END-----------------

		// ------------- BUY AGENT ----------------

		internal class BuyAgent
		{
			internal static bool ListExists(string description)
			{
				return m_Dataset.Tables["BUY_LISTS"].Rows.Cast<DataRow>().Any(row => ((string) row["Description"]).ToLower() == description.ToLower());
			}

			internal static void ListInsert(string description)
			{
				foreach (DataRow row in m_Dataset.Tables["BUY_LISTS"].Rows)
				{
					row["Selected"] = false;
				}

				DataRow newRow = m_Dataset.Tables["BUY_LISTS"].NewRow();
				newRow["Description"] = description;
				newRow["CompareName"] = false;
				newRow["Selected"] = true;
				m_Dataset.Tables["BUY_LISTS"].Rows.Add(newRow);

				Save();
			}

			internal static void ListUpdate(string description, bool comparename, bool selected)
			{
				bool found = m_Dataset.Tables["BUY_LISTS"].Rows.Cast<DataRow>().Any(row => (string) row["Description"] == description);

				if (found)
				{
					if (selected)
					{
						foreach (DataRow row in m_Dataset.Tables["BUY_LISTS"].Rows)
						{
							row["Selected"] = false;
						}
					}

					foreach (DataRow row in m_Dataset.Tables["BUY_LISTS"].Rows)
					{
						if ((string)row["Description"] == description)
						{
							row["Selected"] = selected;
							row["CompareName"] = comparename;
							break;
						}
					}

					Save();
				}
			}

			internal static bool CompareNameRead(string listname)
			{
				return (from DataRow row in m_Dataset.Tables["BUY_LISTS"].Rows where (string)row["Description"] == listname select Convert.ToBoolean(row["CompareName"])).FirstOrDefault();
			}

			internal static void ClearList(string list)
			{
				for (int i = m_Dataset.Tables["BUY_ITEMS"].Rows.Count - 1; i >= 0; i--)
				{
					DataRow row = m_Dataset.Tables["BUY_ITEMS"].Rows[i];
					if ((string)row["List"] == list)
						row.Delete();
				}
			}

			internal static void ListDelete(string description)
			{
				ClearList(description);

				for (int i = m_Dataset.Tables["BUY_LISTS"].Rows.Count - 1; i >= 0; i--)
				{
					DataRow row = m_Dataset.Tables["BUY_LISTS"].Rows[i];
					if ((string)row["Description"] == description)
					{
						row.Delete();
						break;
					}
					row["Selected"] = false;
				}

				Save();
			}

			internal static List<RazorEnhanced.BuyAgent.BuyAgentList> ListsRead()
			{
				return (from DataRow row in m_Dataset.Tables["BUY_LISTS"].Rows let description = (string) row["Description"] let comparename = (bool)row["CompareName"] let selected = (bool) row["Selected"] select new RazorEnhanced.BuyAgent.BuyAgentList(description, comparename, selected)).ToList();
			}

			internal static void ItemInsert(string list, RazorEnhanced.BuyAgent.BuyAgentItem item)
			{
				DataRow row = m_Dataset.Tables["BUY_ITEMS"].NewRow();
				row["List"] = list;
				row["Item"] = item;
				m_Dataset.Tables["BUY_ITEMS"].Rows.Add(row);
			}

			internal static void ItemInsertFromImport(string list, List<RazorEnhanced.BuyAgent.BuyAgentItem> itemlist)
			{
				foreach (RazorEnhanced.BuyAgent.BuyAgentItem item in itemlist)
				{
					DataRow row = m_Dataset.Tables["BUY_ITEMS"].NewRow();
					row["List"] = list;
					row["Item"] = item;
					m_Dataset.Tables["BUY_ITEMS"].Rows.Add(row);
				}
				Save();
			}

			internal static List<RazorEnhanced.BuyAgent.BuyAgentItem> ItemsRead(string list)
			{
				List<RazorEnhanced.BuyAgent.BuyAgentItem> items = new List<RazorEnhanced.BuyAgent.BuyAgentItem>();

				if (ListExists(list))
				{
					items.AddRange(from DataRow row in m_Dataset.Tables["BUY_ITEMS"].Rows where (string) row["List"] == list select (RazorEnhanced.BuyAgent.BuyAgentItem) row["Item"]);
				}

				return items;
			}
		}

		// ------------- BUY AGENT END-----------------

		// ------------- DRESS ----------------

		internal class Dress
		{
			internal static bool ListExists(string description)
			{
				return m_Dataset.Tables["DRESS_LISTS"].Rows.Cast<DataRow>().Any(row => ((string) row["Description"]).ToLower() == description.ToLower());
			}

			internal static void ListInsert(string description, int delay, int bag, bool conflict)
			{
				foreach (DataRow row in m_Dataset.Tables["DRESS_LISTS"].Rows)
				{
					row["Selected"] = false;
				}

				DataRow newRow = m_Dataset.Tables["DRESS_LISTS"].NewRow();
				newRow["Description"] = description;
				newRow["Delay"] = delay;
				newRow["Bag"] = bag;
				newRow["Conflict"] = conflict;
				newRow["Selected"] = true;
				newRow["HotKey"] = Keys.None;
				newRow["HotKeyPass"] = true;
				m_Dataset.Tables["DRESS_LISTS"].Rows.Add(newRow);

				Save();
			}

			internal static void ListUpdate(string description, int delay, int bag, bool conflict, bool selected)
			{
				bool found = m_Dataset.Tables["DRESS_LISTS"].Rows.Cast<DataRow>().Any(row => (string) row["Description"] == description);

				if (found)
				{
					if (selected)
					{
						foreach (DataRow row in m_Dataset.Tables["DRESS_LISTS"].Rows)
						{
							row["Selected"] = false;
						}
					}

					foreach (DataRow row in m_Dataset.Tables["DRESS_LISTS"].Rows)
					{
						if ((string)row["Description"] == description)
						{
							row["Delay"] = delay;
							row["Bag"] = bag;
							row["Conflict"] = conflict;
							row["Selected"] = selected;
							break;
						}
					}

					Save();
				}
			}

			internal static void ClearList(string list)
			{
				for (int i = m_Dataset.Tables["DRESS_ITEMS"].Rows.Count - 1; i >= 0; i--)
				{
					DataRow row = m_Dataset.Tables["DRESS_ITEMS"].Rows[i];
					if ((string)row["List"] == list)
						row.Delete();
					m_Dataset.Tables["DRESS_ITEMS"].AcceptChanges();
				}
			}

			internal static void ListDelete(string description)
			{
				ClearList(description);

				for (int i = m_Dataset.Tables["DRESS_LISTS"].Rows.Count - 1; i >= 0; i--)
				{
					DataRow row = m_Dataset.Tables["DRESS_LISTS"].Rows[i];
					if ((string)row["Description"] == description)
					{
						row.Delete();
						break;
					}
					row["Selected"] = false;
				}

				Save();
			}

			internal static List<RazorEnhanced.Dress.DressList> ListsRead()
			{
				List<RazorEnhanced.Dress.DressList> lists = new List<RazorEnhanced.Dress.DressList>();

				foreach (DataRow row in m_Dataset.Tables["DRESS_LISTS"].Rows)
				{
					string description = (string)row["Description"];
					int delay = Convert.ToInt32(row["Delay"]);
					int bag = Convert.ToInt32(row["Bag"]);
					bool conflict = (bool)row["Conflict"];
					bool selected = (bool)row["Selected"];

					RazorEnhanced.Dress.DressList list = new RazorEnhanced.Dress.DressList(description, delay, bag, conflict, selected);
					lists.Add(list);
				}

				return lists;
			}

			internal static List<RazorEnhanced.Dress.DressItemNew> ItemsRead(string list)
			{
				List<RazorEnhanced.Dress.DressItemNew> items = new List<RazorEnhanced.Dress.DressItemNew>();

				if (ListExists(list))
				{
					items.AddRange(from DataRow row in m_Dataset.Tables["DRESS_ITEMS"].Rows where (string) row["List"] == list select (RazorEnhanced.Dress.DressItemNew) row["Item"]);
				}

				return items;
            }

			internal static void ListDetailsRead(string listname, out int bag, out int delay, out bool conflict)
			{
				bag = 0;
				delay = 0;
				conflict = false;
				foreach (DataRow row in m_Dataset.Tables["DRESS_LISTS"].Rows)
				{
					if ((string)row["Description"] == listname)
					{
						bag = Convert.ToInt32(row["Bag"]);
						delay = Convert.ToInt32(row["Delay"]);
						conflict = (bool)row["Conflict"];
					}
				}
			}

			internal static void ItemClear(string list)
			{
				if (ListExists(list))
				{
					foreach (DataRow row in m_Dataset.Tables["DRESS_ITEMS"].Rows)
					{
						if ((string)row["List"] == list)
							row.Delete();
					}
				}
				Save();
			}

			internal static void ItemInsert(string list, RazorEnhanced.Dress.DressItemNew item)
			{
				DataRow row = m_Dataset.Tables["DRESS_ITEMS"].NewRow();
				row["List"] = list;
				row["Item"] = item;
				m_Dataset.Tables["DRESS_ITEMS"].Rows.Add(row);

				Save();
			}

			internal static void ItemInsertFromImport(string list, List<RazorEnhanced.Dress.DressItemNew> itemlist)
			{
				foreach (RazorEnhanced.Dress.DressItemNew item in itemlist)
				{
					DataRow row = m_Dataset.Tables["DRESS_ITEMS"].NewRow();
					row["List"] = list;
					row["Item"] = item;
					m_Dataset.Tables["DRESS_ITEMS"].Rows.Add(row);
				}
				Save();
			}

			internal static void ItemDelete(string list, RazorEnhanced.Dress.DressItemNew item)
			{
				for (int i = m_Dataset.Tables["DRESS_ITEMS"].Rows.Count - 1; i >= 0; i--)
				{
					DataRow row = m_Dataset.Tables["DRESS_ITEMS"].Rows[i];
					if ((string)row["List"] == list && (RazorEnhanced.Dress.DressItemNew)row["Item"] == item)
					{
						row.Delete();
						break;
					}
				}

				Save();
			}

			internal static void ItemReplace(string list, int index, RazorEnhanced.Dress.DressItemNew item)
			{
				int count = -1;
				foreach (DataRow row in m_Dataset.Tables["DRESS_ITEMS"].Rows)
				{
					if ((string)row["List"] == list)
					{
						count++;
						if (count == index)
						{
							row["Item"] = item;
						}
					}
				}
				Save();
			}

			internal static void ItemInsertByLayer(string list, RazorEnhanced.Dress.DressItemNew item)
			{
				bool found = false;
				foreach (DataRow row in m_Dataset.Tables["DRESS_ITEMS"].Rows)
				{
					if ((string)row["List"] == list)
					{
						RazorEnhanced.Dress.DressItemNew itemtoscan;
						itemtoscan = (RazorEnhanced.Dress.DressItemNew)row["Item"];
						if (itemtoscan.Layer == item.Layer)
						{
							RazorEnhanced.Dress.AddLog("Item replaced");
							row["Item"] = item;
							found = true;
						}
					}
				}
				if (!found)
				{
					RazorEnhanced.Dress.AddLog("New item added");
					ItemInsert(list, item);
				}
				Save();
			}
		}

		// ------------- DRESS END-----------------

		// ------------- FRIEND START -----------------

		internal class Friend
		{
			internal static bool ListExists(string description)
			{
				return m_Dataset.Tables["FRIEND_LISTS"].Rows.Cast<DataRow>().Any(row => ((string) row["Description"]).ToLower() == description.ToLower());
			}

			internal static void ListInsert(string description, bool includeparty, bool preventattack, bool autoacceptparty, bool slfriend, bool tbfriend, bool comfriend, bool minfriend)
			{
				foreach (DataRow row in m_Dataset.Tables["FRIEND_LISTS"].Rows)
				{
					row["Selected"] = false;
				}

				DataRow newRow = m_Dataset.Tables["FRIEND_LISTS"].NewRow();
				newRow["Description"] = description;
				newRow["IncludeParty"] = includeparty;
				newRow["PreventAttack"] = preventattack;
				newRow["AutoacceptParty"] = autoacceptparty;
				newRow["SLFrinedCheckBox"] = slfriend;
				newRow["TBFrinedCheckBox"] = tbfriend;
				newRow["COMFrinedCheckBox"] = comfriend;
				newRow["MINFrinedCheckBox"] = minfriend;
				newRow["Selected"] = true;
				m_Dataset.Tables["FRIEND_LISTS"].Rows.Add(newRow);

				Save();
			}

			internal static void ListUpdate(string description, bool includeparty, bool preventattack, bool autoacceptparty, bool slfriend, bool tbfriend, bool comfriend, bool minfriend, bool selected)
			{
				bool found = m_Dataset.Tables["FRIEND_LISTS"].Rows.Cast<DataRow>().Any(row => (string) row["Description"] == description);

				if (found)
				{
					if (selected)
					{
						foreach (DataRow row in m_Dataset.Tables["FRIEND_LISTS"].Rows)
						{
							row["Selected"] = false;
						}
					}

					foreach (DataRow row in m_Dataset.Tables["FRIEND_LISTS"].Rows)
					{
						if ((string)row["Description"] == description)
						{
							row["Description"] = description;
							row["IncludeParty"] = includeparty;
							row["PreventAttack"] = preventattack;
							row["AutoacceptParty"] = autoacceptparty;
							row["SLFrinedCheckBox"] = slfriend;
							row["TBFrinedCheckBox"] = tbfriend;
							row["COMFrinedCheckBox"] = comfriend;
							row["MINFrinedCheckBox"] = minfriend;
							row["Selected"] = selected;
							break;
						}
					}

					Save();
				}
			}

			internal static void ListDelete(string description)
			{
				for (int i = m_Dataset.Tables["FRIEND_PLAYERS"].Rows.Count - 1; i >= 0; i--)
				{
					DataRow row = m_Dataset.Tables["FRIEND_PLAYERS"].Rows[i];
					if ((string)row["List"] == description)
					{
						row.Delete();
					}
				}

				for (int i = m_Dataset.Tables["FRIEND_LISTS"].Rows.Count - 1; i >= 0; i--)
				{
					DataRow row = m_Dataset.Tables["FRIEND_LISTS"].Rows[i];
					if ((string)row["Description"] == description)
					{
						row.Delete();
						break;
					}
					row["Selected"] = false;
				}
				Save();
			}

			internal static List<RazorEnhanced.Friend.FriendList> ListsRead()
			{
				List<RazorEnhanced.Friend.FriendList> lists = new List<RazorEnhanced.Friend.FriendList>();

				foreach (DataRow row in m_Dataset.Tables["FRIEND_LISTS"].Rows)
				{
					string description = (string)row["Description"];
					bool includeparty = (bool)row["IncludeParty"];
					bool preventattack = (bool)row["PreventAttack"];
					bool autoacceptparty = (bool)row["AutoacceptParty"];

					bool slfriend = (bool)row["SLFrinedCheckBox"];
					bool tbfriend = (bool)row["TBFrinedCheckBox"];
					bool comfriend = (bool)row["COMFrinedCheckBox"];
					bool minfriend = (bool)row["MINFrinedCheckBox"];

					bool selected = (bool)row["Selected"];

					RazorEnhanced.Friend.FriendList list = new RazorEnhanced.Friend.FriendList(description, autoacceptparty, preventattack, includeparty, slfriend, tbfriend, comfriend, minfriend, selected);
					lists.Add(list);
				}

				return lists;
			}

			internal static bool PlayerExists(string list, RazorEnhanced.Friend.FriendPlayer player)
			{
				foreach (DataRow row in m_Dataset.Tables["FRIEND_PLAYERS"].Rows)
				{
					RazorEnhanced.Friend.FriendPlayer listPlayer = (RazorEnhanced.Friend.FriendPlayer)row["Item"];
					if ((listPlayer.List == list) && (listPlayer.Serial == player.Serial))
					{
						return true;
					}
				}
				return false;

				//return (from DataRow row in m_Dataset.Tables["FRIEND_PLAYERS"].Rows let dacercare = (RazorEnhanced.Friend.FriendPlayer) row["Item"] where (string) row["List"] == list && dacercare.Serial == player.Serial select row).Any();
			}

			internal static bool GuildExists(string list, string guild)
			{
				foreach (DataRow row in m_Dataset.Tables["FRIEND_GUILDS"].Rows)
				{
					RazorEnhanced.Friend.FriendGuild f_guild = (RazorEnhanced.Friend.FriendGuild)row["Item"];
					if ((f_guild.List == list) && (f_guild.Name == guild))
					{
						return true;
					}
				}
				return false;
				// (from DataRow row in m_Dataset.Tables["FRIEND_GUILDS"].Rows let dacercare = (RazorEnhanced.Friend.FriendGuild) row["Item"] where (string) row["List"] == list && dacercare.Name == guild select row).Any();
			}

			internal static void PlayerInsert(string list, RazorEnhanced.Friend.FriendPlayer player)
			{
				DataRow row = m_Dataset.Tables["FRIEND_PLAYERS"].NewRow();
				row["List"] = list;
				player.List = list;
				row["Item"] = player;
				m_Dataset.Tables["FRIEND_PLAYERS"].Rows.Add(row);

				Save();
			}

			internal static void GuildInsert(string list, RazorEnhanced.Friend.FriendGuild guild)
			{
				DataRow row = m_Dataset.Tables["FRIEND_GUILDS"].NewRow();
				row["List"] = list;
				guild.List = list;
				row["Item"] = guild;
				m_Dataset.Tables["FRIEND_GUILDS"].Rows.Add(row);

				Save();
			}

			internal static void PlayerInsertFromImport(string list, List<RazorEnhanced.Friend.FriendPlayer> playerlist)
			{
				foreach (RazorEnhanced.Friend.FriendPlayer player in playerlist)
				{
					DataRow row = m_Dataset.Tables["FRIEND_PLAYERS"].NewRow();
					row["List"] = list;
					player.List = list;
					row["Item"] = player;
					m_Dataset.Tables["FRIEND_PLAYERS"].Rows.Add(row);
				}
				Save();
			}

			internal static void GuildInsertFromImport(string list, List<RazorEnhanced.Friend.FriendGuild> guilds)
			{
				foreach (RazorEnhanced.Friend.FriendGuild guild in guilds)
				{
					DataRow row = m_Dataset.Tables["FRIEND_GUILDS"].NewRow();
					row["List"] = list;
					guild.List = list;
					row["Item"] = guild;
					m_Dataset.Tables["FRIEND_GUILDS"].Rows.Add(row);
				}
				Save();
			}

			internal static void PlayerReplace(string list, int index, RazorEnhanced.Friend.FriendPlayer player)
			{
				int count = -1;
				foreach (DataRow row in m_Dataset.Tables["FRIEND_PLAYERS"].Rows)
				{
					if ((string)row["List"] == list)
					{
						count++;
						if (count == index)
						{
							row["Item"] = player;
						}
					}
				}

				Save();
			}

			internal static void GuildReplace(string list, int index, RazorEnhanced.Friend.FriendGuild guild)
			{
				int count = -1;
				foreach (DataRow row in m_Dataset.Tables["FRIEND_GUILDS"].Rows)
				{
					if ((string)row["List"] == list)
					{
						count++;
						if (count == index)
						{
							row["Item"] = guild;
						}
					}
				}

				Save();
			}

			internal static void PlayerDelete(string list, RazorEnhanced.Friend.FriendPlayer player)
			{
				for (int i = m_Dataset.Tables["FRIEND_PLAYERS"].Rows.Count - 1; i >= 0; i--)
				{
					DataRow row = m_Dataset.Tables["FRIEND_PLAYERS"].Rows[i];
					if ((string)row["List"] == list && (RazorEnhanced.Friend.FriendPlayer)row["Item"] == player)
					{
						row.Delete();
						break;
					}
				}

				Save();
			}

			internal static void GuildDelete(string list, RazorEnhanced.Friend.FriendGuild guild)
			{
				for (int i = m_Dataset.Tables["FRIEND_GUILDS"].Rows.Count - 1; i >= 0; i--)
				{
					DataRow row = m_Dataset.Tables["FRIEND_GUILDS"].Rows[i];
					if ((string)row["List"] == list && (RazorEnhanced.Friend.FriendGuild)row["Item"] == guild)
					{
						row.Delete();
						break;
					}
				}

				Save();
			}

			internal static void PlayersRead(string list, out List<RazorEnhanced.Friend.FriendPlayer> players)
			{
				players = new List<RazorEnhanced.Friend.FriendPlayer>();

				if (ListExists(list))
				{
					foreach (DataRow row in m_Dataset.Tables["FRIEND_PLAYERS"].Rows)
					{
						RazorEnhanced.Friend.FriendPlayer player = (RazorEnhanced.Friend.FriendPlayer)row["Item"];
						if (player.List == list)
						{
							players.Add(player);
						}
					}
// players.AddRange(from DataRow row in m_Dataset.Tables["FRIEND_PLAYERS"].Rows where (string) row["List"] == list select (RazorEnhanced.Friend.FriendPlayer) row["Item"]);
				}
			}

			internal static void GuildRead(string list, out List<RazorEnhanced.Friend.FriendGuild> guilds)
			{
				guilds = new List<RazorEnhanced.Friend.FriendGuild>();
				foreach (DataRow row in m_Dataset.Tables["FRIEND_GUILDS"].Rows)
				{
					RazorEnhanced.Friend.FriendGuild guild = (RazorEnhanced.Friend.FriendGuild)row["Item"];
					if (guild.List == list)
					{
						guilds.Add(guild);
					}
				}
				//if (ListExists(list))
				//{
				//	guilds.AddRange(from DataRow row in m_Dataset.Tables["FRIEND_GUILDS"].Rows where (string) row["List"] == list select (RazorEnhanced.Friend.FriendGuild) row["Item"]);
				//}
			}

			internal static void ListDetailsRead(string listname, out bool includeparty, out bool preventattack, out bool autoacceptparty, out bool slfriend, out bool tbfriend, out bool comfriend, out bool minfriend)
			{
				includeparty = false;
				preventattack = false;
				autoacceptparty = false;
				slfriend = false;
				tbfriend = false;
				comfriend = false;
				minfriend = false;

				foreach (DataRow row in m_Dataset.Tables["FRIEND_LISTS"].Rows)
				{
					if ((string) row["Description"] != listname)
						continue;

					includeparty = (bool)row["IncludeParty"];
					preventattack = (bool)row["PreventAttack"];
					autoacceptparty = (bool)row["AutoacceptParty"];
					slfriend = (bool)row["SLFrinedCheckBox"];
					tbfriend = (bool)row["TBFrinedCheckBox"];
					comfriend = (bool)row["COMFrinedCheckBox"];
					minfriend = (bool)row["MINFrinedCheckBox"];
				}
			}
		}

		// ------------- FRIEND END-----------------

		// ------------- RESTOCK  -----------------

		internal class Restock
		{
			internal static bool ListExists(string description)
			{
				return m_Dataset.Tables["RESTOCK_LISTS"].Rows.Cast<DataRow>().Any(row => ((string) row["Description"]).ToLower() == description.ToLower());
			}

			internal static void ListInsert(string description, int delay, int source, int destination)
			{
				foreach (DataRow row in m_Dataset.Tables["RESTOCK_LISTS"].Rows)
				{
					row["Selected"] = false;
				}

				DataRow newRow = m_Dataset.Tables["RESTOCK_LISTS"].NewRow();
				newRow["Description"] = description;
				newRow["Delay"] = delay;
				newRow["Source"] = source;
				newRow["Destination"] = destination;
				newRow["Selected"] = true;
				m_Dataset.Tables["RESTOCK_LISTS"].Rows.Add(newRow);

				Save();
			}

			internal static void ListUpdate(string description, int delay, int source, int destination, bool selected)
			{
				bool found = m_Dataset.Tables["RESTOCK_LISTS"].Rows.Cast<DataRow>().Any(row => (string) row["Description"] == description);

				if (found)
				{
					if (selected)
					{
						foreach (DataRow row in m_Dataset.Tables["RESTOCK_LISTS"].Rows)
						{
							row["Selected"] = false;
						}
					}

					foreach (DataRow row in m_Dataset.Tables["RESTOCK_LISTS"].Rows)
					{
						if ((string)row["Description"] == description)
						{
							row["Delay"] = delay;
							row["Source"] = source;
							row["Destination"] = destination;
							row["Selected"] = selected;
							break;
						}
					}

					Save();
				}
			}

			internal static void ClearList(string list)
			{
				for (int i = m_Dataset.Tables["RESTOCK_ITEMS"].Rows.Count - 1; i >= 0; i--)
				{
					DataRow row = m_Dataset.Tables["RESTOCK_ITEMS"].Rows[i];
					if ((string)row["List"] == list)
						row.Delete();
				}
			}

			internal static void ListDelete(string description)
			{
				ClearList(description);

				for (int i = m_Dataset.Tables["RESTOCK_LISTS"].Rows.Count - 1; i >= 0; i--)
				{
					DataRow row = m_Dataset.Tables["RESTOCK_LISTS"].Rows[i];
					if ((string)row["Description"] == description)
					{
						row.Delete();
						break;
					}
					row["Selected"] = false;
				}

				Save();
			}

			internal static List<RazorEnhanced.Restock.RestockList> ListsRead()
			{
				List<RazorEnhanced.Restock.RestockList> lists = new List<RazorEnhanced.Restock.RestockList>();

				foreach (DataRow row in m_Dataset.Tables["RESTOCK_LISTS"].Rows)
				{
					string description = (string)row["Description"];
					int delay = Convert.ToInt32(row["Delay"]);
					int source = Convert.ToInt32(row["Source"]);
					int destination = Convert.ToInt32(row["Destination"]);
					bool selected = (bool)row["Selected"];

					lists.Add(new RazorEnhanced.Restock.RestockList(description, delay, source, destination, selected));
				}
				return lists;
			}

			internal static void ItemInsert(string list, RazorEnhanced.Restock.RestockItem item)
			{
				DataRow row = m_Dataset.Tables["RESTOCK_ITEMS"].NewRow();
				row["List"] = list;
				row["Item"] = item;
				m_Dataset.Tables["RESTOCK_ITEMS"].Rows.Add(row);
			}

			internal static void ItemInsertFromImport(string list, List<RazorEnhanced.Restock.RestockItem> itemlist)
			{
				foreach (RazorEnhanced.Restock.RestockItem item in itemlist)
				{
					DataRow row = m_Dataset.Tables["RESTOCK_ITEMS"].NewRow();
					row["List"] = list;
					row["Item"] = item;
					m_Dataset.Tables["RESTOCK_ITEMS"].Rows.Add(row);
				}
				Save();
			}

			internal static List<RazorEnhanced.Restock.RestockItem> ItemsRead(string list)
			{
				List<RazorEnhanced.Restock.RestockItem> items = new List<RazorEnhanced.Restock.RestockItem>();

				if (ListExists(list))
				{
					foreach (DataRow row in m_Dataset.Tables["RESTOCK_ITEMS"].Rows)
					{
						if ((string)row["List"] == list)
						{
							items.Add((RazorEnhanced.Restock.RestockItem)row["Item"]);
						}
					}
				}

				return items;
            }

			internal static void ListDetailsRead(string listname, out int bags, out int bagd, out int delay)
			{
				bags = 0;
				bagd = 0;
				delay = 0;
				foreach (DataRow row in m_Dataset.Tables["RESTOCK_LISTS"].Rows)
				{
					if ((string)row["Description"] == listname)
					{
						bags = Convert.ToInt32(row["Source"]);
						bagd = Convert.ToInt32(row["Destination"]);
						delay = Convert.ToInt32(row["Delay"]);
					}
				}
			}
		}

		// ------------- RESTOCK END-----------------

		// ------------- GRAPH FILTER  -----------------

		internal class GraphFilter
		{
			internal static void ClearList()
			{
				for (int i = m_Dataset.Tables["FILTER_GRAPH"].Rows.Count - 1; i >= 0; i--)
				{
					DataRow row = m_Dataset.Tables["FILTER_GRAPH"].Rows[i];
					row.Delete();
				}
			}

			internal static List<RazorEnhanced.Filters.GraphChangeData> ReadAll()
			{
				List<RazorEnhanced.Filters.GraphChangeData> retList = new List<RazorEnhanced.Filters.GraphChangeData>();
				foreach (DataRow row in m_Dataset.Tables["FILTER_GRAPH"].Rows)
				{
					RazorEnhanced.Filters.GraphChangeData graphdata = new RazorEnhanced.Filters.GraphChangeData(
						(bool)row["Selected"], Convert.ToInt32(row["GraphReal"]), Convert.ToInt32(row["GraphNew"]), Convert.ToInt32(row["ColorNew"]));
					retList.Add(graphdata);
				}
				//from DataRow row in m_Dataset.Tables["FILTER_GRAPH"].Rows select (RazorEnhanced.Filters.GraphChangeData) row["Graph"]).ToList();
				return retList;
			}

			internal static void Insert(bool enabled, int graphreal, int graphnew, int colornew)
			{
				RazorEnhanced.Filters.GraphChangeData graphdata = new RazorEnhanced.Filters.GraphChangeData(enabled, graphreal, graphnew, colornew);

				DataRow row = m_Dataset.Tables["FILTER_GRAPH"].NewRow();
				//row["Graph"] = graphdata;
				row["Selected"] = enabled;
				row["GraphReal"] = graphreal;
				row["GraphNew"] = graphnew;
				row["ColorNew"] = colornew;
				m_Dataset.Tables["FILTER_GRAPH"].Rows.Add(row);
				m_Dataset.AcceptChanges();


			}
		}

		// ------------- GRAPH FILTER END-----------------

		// ------------- TARGET SETTINGS START -----------------
		internal class Target
		{
			internal static List<string> ReadAllShortCut()
			{
				List<string> all = new List<string>();
				foreach (DataRow row in m_Dataset.Tables["TARGETS"].Rows)
				{
					TargetGUI theTarget = (TargetGUI)row["Item"];
					string name = theTarget.Name;
					all.Add(name);
				}

				return all;
			}

			internal static void TargetReplace(string targetid, TargetGUI target)
			{
				for (int i=0; i < m_Dataset.Tables["TARGETS"].Rows.Count; i++)
				{
					DataRow row = m_Dataset.Tables["TARGETS"].Rows[i];
					TargetGUI theTarget = (TargetGUI)row["Item"];
					string name = theTarget.Name;
					if (name == targetid)
					{
                        target.HotKey = theTarget.HotKey;
                        target.HotKeyPass = theTarget.HotKeyPass;
                        row.Delete();
						//m_Dataset.Tables["TARGETS"].Rows.Add(target);
						row = m_Dataset.Tables["TARGETS"].NewRow();
						row["Item"] = target;
						m_Dataset.Tables["TARGETS"].Rows.Add(row);
						break;
					}
				}
				Save();
			}

			internal static bool TargetExist(string targetid)
			{
				foreach (DataRow row in m_Dataset.Tables["TARGETS"].Rows)
				{
					TargetGUI theTarget = (TargetGUI)row["Item"];
					if (theTarget.Name == targetid)
					{
						return true;
					}
					//return m_Dataset.Tables["TARGETS"].Rows.Cast<DataRow>().Any(row => (string) row["Name"] == targetid);
				}
				return false;
			}

			internal static void TargetAdd(string targetid, TargetGUI target, Keys k, bool pass)
			{
				target.Name = targetid;
				target.HotKey = k;
				target.HotKeyPass = pass;
				if (TargetExist(targetid))
					TargetDelete(targetid);
				DataRow row = m_Dataset.Tables["TARGETS"].NewRow();
				row["Item"] = target;
				m_Dataset.Tables["TARGETS"].Rows.Add(row);
				Save();
			}

			internal static void TargetDelete(string targetid)
			{
				if (TargetExist(targetid))
				{
					foreach (DataRow row in m_Dataset.Tables["TARGETS"].Rows)
					{
						TargetGUI theTarget = (TargetGUI)row["Item"];
						if (theTarget.Name == targetid)
						{
							row.Delete();
							break;
						}
					}
				}
				Save();
			}

			public static TargetGUI TargetRead(string targetid)
			{
				foreach (DataRow row in m_Dataset.Tables["TARGETS"].Rows)
				{
					TargetGUI theTarget = (TargetGUI)row["Item"];
					if (theTarget.Name == targetid)
					{
						return theTarget;
					}
					//return (from DataRow row in m_Dataset.Tables["TARGETS"].Rows where (string) row["Name"] == targetid select (TargetGUI) row["TargetGUI"]).FirstOrDefault();
				}
				return null;
			}
		}

		// ------------- TARGET SETTINGS END -----------------

		// ------------- TOOLBAR -----------------
		internal class Toolbar
		{
			internal static List<RazorEnhanced.ToolBar.ToolBarItem> ReadItems()
			{
				List<RazorEnhanced.ToolBar.ToolBarItem> retList = new List<RazorEnhanced.ToolBar.ToolBarItem>();
				foreach (DataRow row in m_Dataset.Tables["TOOLBAR_ITEMS"].Rows)  // .Count || index == -1) //out of range
				{
					string name = (string)row["Name"];
					int graphic = Convert.ToInt32(row["Graphics"]);
					int color = Convert.ToInt32(row["Color"]);
					bool warn = Convert.ToBoolean(row["Warning"]);
					int limit = Convert.ToInt32(row["WarningLimit"]);
					//RazorEnhanced.ToolBar.ToolBarItem item = new RazorEnhanced.ToolBar.ToolBarItem(name, graphic, color, warn, limit);
					retList.Add(new RazorEnhanced.ToolBar.ToolBarItem(name, graphic, color, warn, limit));
				}
				return retList;
			}

			internal static RazorEnhanced.ToolBar.ToolBarItem ReadSelectedItem(int index)
			{
				DataRow row = m_Dataset.Tables["TOOLBAR_ITEMS"].Rows[index];
				string name = (string)row["Name"];
				int graphic = Convert.ToInt32(row["Graphics"]);
				int color = Convert.ToInt32(row["Color"]);
				bool warn = Convert.ToBoolean(row["Warning"]);
				int limit = Convert.ToInt32(row["WarningLimit"]);

				RazorEnhanced.ToolBar.ToolBarItem item = new RazorEnhanced.ToolBar.ToolBarItem(name, graphic, color, warn, limit);
				return item;
			}

			internal static void UpdateItem(int index, string name, string graphics, string color, bool warning, string warninglimit)
			{
				int convgraphics = 0;
				int convcolor = 0;
				int convwarninglimit = 0;

				try
				{
					convgraphics = Convert.ToInt32(graphics, 16);
				}
				catch
				{ }

				if (color == "-1")
				{
					convcolor = -1;
				}
				else
				{
					try
					{
						convcolor = Convert.ToInt32(color, 16);
					}
					catch
					{ }
				}

				try
				{
					convwarninglimit = Convert.ToInt32(warninglimit);
				}
				catch
				{ }

				//RazorEnhanced.ToolBar.ToolBarItem item = new RazorEnhanced.ToolBar.ToolBarItem(name, convgraphics, convcolor, warning, convwarninglimit);

				if (index >= m_Dataset.Tables["TOOLBAR_ITEMS"].Rows.Count || index == -1) //out of range
					return;

				DataRow row = m_Dataset.Tables["TOOLBAR_ITEMS"].Rows[index];
				row["Name"] = name;
				row["Graphics"] = convgraphics;
				row["Color"] = convcolor;
				row["Warning"] = warning;
				row["WarningLimit"] = convwarninglimit;
				Save();
			}
		}

		// ------------- TOOLBAR END -----------------

		// ------------- TOOLBAR -----------------
		internal class SpellGrid
		{
			internal static List<RazorEnhanced.SpellGrid.SpellGridItem> ReadItems()
			{
				List<RazorEnhanced.SpellGrid.SpellGridItem> griditem = new List<RazorEnhanced.SpellGrid.SpellGridItem>();

				for (int i = 0; i < m_Dataset.Tables["SPELLGRID_ITEMS"].Rows.Count; i++)
				{
					DataRow row = m_Dataset.Tables["SPELLGRID_ITEMS"].Rows[i];
					if (row.RowState != DataRowState.Deleted)
					{
						//string group = (string)row["Group"];
						//string _row = (string)row["Spell"];
						//string color = (string)row["Color"];
						//Color c = Color.FromName((string)row["Color"]);

						RazorEnhanced.SpellGrid.SpellGridItem temp = new RazorEnhanced.SpellGrid.SpellGridItem((string)row["Group"], (string)row["Spell"], Color.FromName((string)row["Color"]), Color.FromName((string)row["Color"]));
						griditem.Add(temp);
					}
				}

				return griditem;
			}

			internal static RazorEnhanced.SpellGrid.SpellGridItem ReadSelectedItem(int index)
			{
				DataRow row = m_Dataset.Tables["SPELLGRID_ITEMS"].Rows[index];
				RazorEnhanced.SpellGrid.SpellGridItem temp = new RazorEnhanced.SpellGrid.SpellGridItem((string)row["Group"], (string)row["Spell"], Color.FromName((string)row["Color"]), Color.FromName((string)row["Color"]));
				return temp;
				//return (RazorEnhanced.SpellGrid.SpellGridItem)m_Dataset.Tables["SPELLGRID_ITEMS"].Rows[index];
			}

			internal static void UpdateItem(int index, string group, string spell, Color border)
			{
				//RazorEnhanced.SpellGrid.SpellGridItem item = new RazorEnhanced.SpellGrid.SpellGridItem(group, spell, border, Color.Transparent);
				DataRow row = m_Dataset.Tables["SPELLGRID_ITEMS"].Rows[index];
				row["Group"] = group;
				row["Spell"] = spell;
				row["Color"] = border;
				Save();
			}
		}

		// ------------- TOOLBAR END -----------------

		// ------------- PASSWORD START -----------------
		internal class Password
		{
			internal static void AddUpdateUser(string user, string password, string IP)
			{
				bool found = false;

				foreach (DataRow row in m_Dataset.Tables["PASSWORD"].Rows)  // Cerco e aggiorno se esiste
				{
					if ((string)row["User"] == user && (string)row["IP"] == IP)
					{
						row["Password"] = password;
						found = true;
                        break;
					}
				}

				if (!found)
				{
					DataRow newRow = m_Dataset.Tables["PASSWORD"].NewRow();
					newRow["IP"] = IP;
					newRow["User"] = user;
					newRow["Password"] = password;
					m_Dataset.Tables["PASSWORD"].Rows.Add(newRow);
				}
				Save();
			}

			internal static string GetPassword(string user, string IP)
			{
				foreach (DataRow row in m_Dataset.Tables["PASSWORD"].Rows)  // Cerco
				{
					if ((string)row["User"] == user && (string)row["IP"] == IP)
					{
						return (string)row["Password"];
					}
				}

				return String.Empty;
			}

			internal static void InsertAll(List<PasswordMemory.PasswordData> pdatalist)
			{
				m_Dataset.Tables["PASSWORD"].Rows.Clear();

				foreach (PasswordMemory.PasswordData pdata in pdatalist)
				{
					DataRow newRow = m_Dataset.Tables["PASSWORD"].NewRow();
					newRow["IP"] = pdata.IP;
					newRow["User"] = pdata.User;
					newRow["Password"] = pdata.Password;
					m_Dataset.Tables["PASSWORD"].Rows.Add(newRow);
				}
				Save();
			}


			internal static List<Assistant.PasswordMemory.PasswordData> RealAll()
			{
				List<Assistant.PasswordMemory.PasswordData> dataOut = new List<Assistant.PasswordMemory.PasswordData>();

				foreach (DataRow row in m_Dataset.Tables["PASSWORD"].Rows)
				{
					string ip = (string)row["IP"];
					string user = (string)row["User"];
					string password = (string)row["Password"];

					Assistant.PasswordMemory.PasswordData data = new Assistant.PasswordMemory.PasswordData(ip, user, password);
					dataOut.Add(data);
				}
				return dataOut;
			}
		}

		// ------------- PASSWORD END -----------------

		// ------------- HOTKEYS START -----------------
		internal class HotKey
		{
			internal static List<RazorEnhanced.HotKey.HotKeyData> ReadGroup(string gname)
			{
				List<RazorEnhanced.HotKey.HotKeyData> keydataOut = new List<RazorEnhanced.HotKey.HotKeyData>();

				foreach (DataRow row in m_Dataset.Tables["HOTKEYS"].Rows)
				{
					if ((string)row["Group"] == gname)
					{
						string name = (string)row["Name"];
						int key = Convert.ToInt32(row["Key"]);
						keydataOut.Add(new RazorEnhanced.HotKey.HotKeyData(name, (Keys)key));
					}
				}
				return keydataOut;
			}

			internal static List<RazorEnhanced.HotKey.HotKeyData> ReadTarget()
			{
				List<RazorEnhanced.HotKey.HotKeyData> retList = new List<RazorEnhanced.HotKey.HotKeyData>();
				foreach (DataRow row in m_Dataset.Tables["TARGETS"].Rows)
				{
					TargetGUI theTarget = (TargetGUI)row["Item"];
					string name = theTarget.Name;
					Keys key = theTarget.HotKey;
					RazorEnhanced.HotKey.HotKeyData aKey = new RazorEnhanced.HotKey.HotKeyData(name, key);
					retList.Add(aKey);
				}

				//return (from DataRow row in m_Dataset.Tables["TARGETS"].Rows let name = (string) row.ItemArray[0].Name let key = (Keys)row.ItemArray[0].HotKey select new RazorEnhanced.HotKey.HotKeyData(name, key)).ToList();
				return retList;
			}

			internal static List<RazorEnhanced.HotKey.HotKeyData> ReadScript()
			{
				return (from DataRow row in m_Dataset.Tables["SCRIPTING"].Rows let name = (string) row["Filename"] let key = (Keys)Convert.ToInt32(row["HotKey"]) select new RazorEnhanced.HotKey.HotKeyData(name, key)).ToList();
			}

			internal static List<RazorEnhanced.HotKey.HotKeyData> ReadDress()
			{
				return (from DataRow row in m_Dataset.Tables["DRESS_LISTS"].Rows let name = (string) row["Description"] let key = (Keys)Convert.ToInt32(row["HotKey"]) select new RazorEnhanced.HotKey.HotKeyData(name, key)).ToList();
			}

			internal static void UpdateKey(string name, Keys key, bool passkey)
			{
				foreach (DataRow row in m_Dataset.Tables["HOTKEYS"].Rows)
				{
					if ((string)row["Name"] == name)
					{
						row["Key"] = key;
						row["Pass"] = passkey;
						break;
					}
				}

				Save();
			}

			internal static void UpdateTargetKey(string name, Keys key, bool passkey)
			{
				foreach (DataRow row in m_Dataset.Tables["TARGETS"].Rows)
				{
					TargetGUI theTarget = (TargetGUI)row["Item"];
					if (theTarget.Name == name)
					{
						theTarget.HotKey = key;
						theTarget.HotKeyPass = passkey;
						break;
					}
				}

				Save();
			}

			internal static void UpdateScriptKey(string name, Keys key, bool passkey)
			{
				foreach (DataRow row in m_Dataset.Tables["SCRIPTING"].Rows)
				{
					if ((string)row["Filename"] == name)
					{
						row["HotKey"] = key;
						row["HotKeyPass"] = passkey;
						break;
					}
				}

				Save();
			}

			internal static void UpdateDressKey(string name, Keys key, bool passkey)
			{
				foreach (DataRow row in m_Dataset.Tables["DRESS_LISTS"].Rows)
				{
					if ((string)row["Description"] == name)
					{
						row["HotKey"] = key;
						row["HotKeyPass"] = passkey;
						break;
					}
				}

				Save();
			}

			internal static void UnassignKey(Keys key)
			{
				if (RazorEnhanced.Settings.General.ReadKey("HotKeyMasterKey") == key)
				{
					RazorEnhanced.Settings.General.WriteKey("HotKeyMasterKey", Keys.None);
					Assistant.Engine.MainWindow.HotKeyKeyMasterLabel.Text = "ON/OFF Key: " + RazorEnhanced.HotKey.KeyString(RazorEnhanced.HotKey.MasterKey);
				}

				foreach (DataRow row in m_Dataset.Tables["HOTKEYS"].Rows)
				{
					if ((Keys)Convert.ToInt32(row["Key"]) == key)
					{
						row["Key"] = Keys.None;
						row["Pass"] = true;
					}
				}

				foreach (DataRow row in m_Dataset.Tables["TARGETS"].Rows)
				{
					TargetGUI theTarget = (TargetGUI)row["Item"];
					if (theTarget.HotKey == key)
					{
						theTarget.HotKey = Keys.None;
						theTarget.HotKeyPass = true;
					}
				}

				foreach (DataRow row in m_Dataset.Tables["SCRIPTING"].Rows)
				{
					if ((Keys)Convert.ToInt32(row["HotKey"]) == key)
					{
						row["HotKey"] = Keys.None;
						row["HotKeyPass"] = true;
					}
				}

				foreach (DataRow row in m_Dataset.Tables["DRESS_LISTS"].Rows)
				{
					if ((Keys)Convert.ToInt32(row["HotKey"]) == key)
					{
						row["HotKey"] = Keys.None;
						row["HotKeyPass"] = true;
					}
				}

				Save();
			}

			internal static bool AssignedKey(Keys key)
			{
				if (m_Dataset.Tables["HOTKEYS"].Rows.Cast<DataRow>().Any(row => (Keys)Convert.ToInt32(row["Key"]) == key))
				{
					return true;
				}


				foreach (DataRow row in m_Dataset.Tables["TARGETS"].Rows)
				{
					TargetGUI theTarget = (TargetGUI)row["Item"];
					if (theTarget.HotKey == key)
						return true;
				}


				if (m_Dataset.Tables["SCRIPTING"].Rows.Cast<DataRow>().Any(row => (Keys)Convert.ToInt32(row["HotKey"]) == key))
				{
					return true;
				}

				if (m_Dataset.Tables["DRESS_LISTS"].Rows.Cast<DataRow>().Any(row => (Keys)Convert.ToInt32(row["HotKey"]) == key))
				{
					return true;
				}

				if (RazorEnhanced.Settings.General.ReadKey("HotKeyMasterKey") == key)
					return true;

				return false;
			}

			internal static void FindKeyGui(string name, out Keys outkey, out bool outpasskey)
			{
				Keys key = Keys.None;
				bool passkey = true;
				bool found = false;

				foreach (DataRow row in m_Dataset.Tables["HOTKEYS"].Rows)
				{
					if ((string)row["Name"] == name)
					{
						key = (Keys)Convert.ToInt32(row["Key"]);
						passkey = (bool)row["Pass"];
						found = true;
						break;
					}
				}

				if (!found)
					foreach (DataRow row in m_Dataset.Tables["TARGETS"].Rows)
					{
						TargetGUI target = (TargetGUI)row["Item"];
						if (target.Name == name)
						{
							key = target.HotKey;
							passkey = target.HotKeyPass;
							found = true;
							break;
						}
					}

				if (!found)
					foreach (DataRow row in m_Dataset.Tables["SCRIPTING"].Rows)
					{
						if ((string)row["Filename"] == name)
						{
							key = (Keys)Convert.ToInt32(row["HotKey"]);
							passkey = (bool)row["HotKeyPass"];
							found = true;
							break;
						}
					}

				if (!found)
					foreach (DataRow row in m_Dataset.Tables["DRESS_LISTS"].Rows)
					{
						if ((string)row["Description"] == name)
						{
							key = (Keys)Convert.ToInt32(row["HotKey"]);
							passkey = (bool)row["HotKeyPass"];
							found = true;
							break;
						}
					}

				outkey = key;
				outpasskey = passkey;
			}

			internal static string FindString(Keys key)
			{
				return (from DataRow row in m_Dataset.Tables["HOTKEYS"].Rows where (Keys) Convert.ToInt32(row["Key"]) == key select (String) row["Name"]).FirstOrDefault();
			}

			internal static string FindTargetString(Keys key)
			{

				foreach (DataRow row in m_Dataset.Tables["TARGETS"].Rows)
				{
					TargetGUI theTarget = (TargetGUI)row["Item"];
					if (theTarget.HotKey == key)
						return theTarget.Name;
				}
				return "";
			}

			internal static void FindTargetData(string name, out Keys k, out bool pass)
			{
				k = Keys.None;
				pass = true;

				foreach (DataRow row in m_Dataset.Tables["TARGETS"].Rows)
				{
					TargetGUI theTarget = (TargetGUI)row["Item"];
					if (theTarget.Name == name)
					{
						k = theTarget.HotKey;
						pass = theTarget.HotKeyPass;
					}
				}
			}

			internal static string FindScript(Keys key)
			{
				return (from DataRow row in m_Dataset.Tables["SCRIPTING"].Rows where (Keys)Convert.ToInt32(row["HotKey"]) == key select (String) row["Filename"]).FirstOrDefault();
			}

			internal static string FindDress(Keys key)
			{
				return (from DataRow row in m_Dataset.Tables["DRESS_LISTS"].Rows where (Keys)Convert.ToInt32(row["HotKey"]) == key select (String) row["Description"]).FirstOrDefault();
			}

			internal static void FindGroup(Keys key, out string outgroup, out bool outpass)
			{
				string group = String.Empty;
				bool pass = true;
				bool found = false;

				foreach (DataRow row in m_Dataset.Tables["HOTKEYS"].Rows)
				{
					if ((Keys)Convert.ToInt32(row["Key"]) == key)
					{
						group = (String)row["Group"];
						pass = (bool)row["Pass"];
						found = true;
						break;
					}
				}

				if (!found)
					foreach (DataRow row in m_Dataset.Tables["TARGETS"].Rows)
					{
						TargetGUI theTarget = (TargetGUI)row["Item"];
						if (theTarget.HotKey == key)
						{
							group = "TList";
							pass = theTarget.HotKeyPass;
							found = true;
							break;
						}
					}

				if (!found)
					foreach (DataRow row in m_Dataset.Tables["SCRIPTING"].Rows)
					{
						if ((Keys)Convert.ToInt32(row["HotKey"]) == key)
						{
							group = "SList";
							pass = (bool)row["HotKeyPass"];
							found = true;
							break;
						}
					}

				if (!found)
					foreach (DataRow row in m_Dataset.Tables["DRESS_LISTS"].Rows)
					{
						if ((Keys)Convert.ToInt32(row["HotKey"]) == key)
						{
							group = "DList";
							pass = (bool)row["HotKeyPass"];
							found = true;
							break;
						}
					}

				outgroup = group;
				outpass = pass;
			}
		}

		// ------------- HOTKEYS END -----------------

		// ------------- GENERAL SETTINGS START -----------------
		internal class General
		{
			internal static bool ReadBool(string name)
			{
				if (m_Dataset != null && m_Dataset.Tables["GENERAL"].Columns.Contains(name))
				{
					DataRow row = m_Dataset.Tables["GENERAL"].Rows[0];
					return (bool)row[name];
				}
				return false;
			}

			internal static void WriteBool(string name, bool value)
			{
				if (m_Dataset != null && m_Dataset.Tables["GENERAL"].Rows.Count > 0)
				{
					DataRow row = m_Dataset.Tables["GENERAL"].Rows[0];
					if (!m_Dataset.Tables["GENERAL"].Columns.Contains(name))
					{
						// Bug where I forgot to initialize a sound
						m_Dataset.Tables["GENERAL"].Columns.Add(name, typeof(bool));
					}

					row[name] = value;
					Save();
				}
			}

			internal static void WriteBoolNoSave(string name, bool value)
			{
				if (m_Dataset != null && m_Dataset.Tables["GENERAL"].Rows.Count > 0)
				{
					DataRow row = m_Dataset.Tables["GENERAL"].Rows[0];
					if (!m_Dataset.Tables["GENERAL"].Columns.Contains(name))
					{
						// Bug where I forgot to initialize a sound
						m_Dataset.Tables["GENERAL"].Columns.Add(name, typeof(bool));
					}

					row[name] = value;
					//Save();
				}
			}

			internal static string ReadString(string name)
			{
				if (m_Dataset != null && m_Dataset.Tables["GENERAL"].Columns.Contains(name))
				{
					DataRow row = m_Dataset.Tables["GENERAL"].Rows[0];
					return (string)row[name];
				}

				return String.Empty;
			}

			internal static void WriteString(string name, string value)
			{
				if (m_Dataset != null && m_Dataset.Tables["GENERAL"].Rows.Count > 0)
				{
					DataRow row = m_Dataset.Tables["GENERAL"].Rows[0];
					row[name] = value;
					Save();
				}
			}

			internal static int ReadInt(string name)
			{
				if (m_Dataset != null && m_Dataset.Tables["GENERAL"].Columns.Contains(name))
				{
					DataRow row = m_Dataset.Tables["GENERAL"].Rows[0];
					return Convert.ToInt32(row[name]);
				}

				return 1;
			}

			internal static void WriteInt(string name, int value)
			{
				if (m_Dataset != null && m_Dataset.Tables["GENERAL"].Rows.Count > 0)
				{
					DataRow row = m_Dataset.Tables["GENERAL"].Rows[0];
					row[name] = value;
					Save();
				}
			}

			internal static Keys ReadKey(string name)
			{
				if (m_Dataset != null && m_Dataset.Tables["GENERAL"].Columns.Contains(name))
				{
					DataRow row = m_Dataset.Tables["GENERAL"].Rows[0];
					var key = row[name];
					return (System.Windows.Forms.Keys)Convert.ToInt32(key);
				}

				return Keys.None;
			}

			internal static void WriteKey(string name, Keys k)
			{
				if (m_Dataset != null && m_Dataset.Tables["GENERAL"].Rows.Count > 0)
				{
					DataRow row = m_Dataset.Tables["GENERAL"].Rows[0];
					row[name] = k;
					Save();
				}
			}

			internal static void SaveExitData()
			{
				if (Assistant.Engine.GridX > 0)
					WriteInt("PosXGrid", Assistant.Engine.GridX);

				if (Assistant.Engine.GridY > 0)
					WriteInt("PosYGrid", Assistant.Engine.GridY);

				if (Assistant.Engine.ToolBarX > 0)
					WriteInt("PosXToolBar", Assistant.Engine.ToolBarX);

				if (Assistant.Engine.ToolBarY > 0)
					WriteInt("PosYToolBar", Assistant.Engine.ToolBarY);

				if (Assistant.Engine.MainWindowX > 0)
					WriteInt("WindowX", Assistant.Engine.MainWindowX);

				if (Assistant.Engine.MainWindowY > 0)
					WriteInt("WindowY", Assistant.Engine.MainWindowY);
			}
		}

		// ------------- GENERAL SETTINGS END -----------------

		internal static void Save(bool force=false)
		{
			if (!force)
				if (Engine.MainWindow != null)
				{
					if (Assistant.Engine.MainWindow.Initializing)
						return;
				}

			try
			{
				string dir = Path.Combine(Assistant.Engine.RootPath, "Profiles", m_profileName);
				System.IO.Directory.CreateDirectory(dir);
				string filename = Path.Combine(dir, "RazorEnhanced.settings");
				m_Dataset.AcceptChanges();
				foreach (DataTable table in m_Dataset.Tables)
				{

					if (saveDict.ContainsKey(table.TableName))
					{
						saveDict[table.TableName](filename, table.TableName, table);
					}
					else
					{
						File.WriteAllText(filename + '.' + table.TableName, Newtonsoft.Json.JsonConvert.SerializeObject(table, Newtonsoft.Json.Formatting.Indented));
					}
				}

				//File.WriteAllText(filename, Newtonsoft.Json.JsonConvert.SerializeObject(m_Dataset, Newtonsoft.Json.Formatting.Indented));
			}
			catch (Exception ex)
			{
				MessageBox.Show("Error writing " + m_profileName + ": " + ex);
			}
		}

		// Funzione per cambiare la struttura dei save in caso di modifiche senza dover cancellare e rifare da 0
		internal static void UpdateVersion(int previousVersion)
		{
            int realVersion = previousVersion;

            if (realVersion == 1)
            {
                DataTable hotkey = m_Dataset.Tables["HOTKEYS"];
                initCleric(hotkey);
                initDruid(hotkey);
                realVersion = 2;
                General.WriteInt("SettingVersion", realVersion);
            }
            if (realVersion == 2)
            {
                DataTable general = m_Dataset.Tables["GENERAL"];
                general.Columns.Add("BandageHealUseText", typeof(bool));
                general.Columns.Add("BandageHealUseTextContent", typeof(string));
                Settings.General.WriteBool("BandageHealUseText", false);
                Settings.General.WriteString("BandageHealUseTextContent", "[band");
                realVersion = 3;
                General.WriteInt("SettingVersion", realVersion);
            }
            if (realVersion == 3)
            {
                DataTable general = m_Dataset.Tables["GENERAL"];
                general.Columns.Add("BandageHealUseTextSelfContent", typeof(string));
                Settings.General.WriteString("BandageHealUseTextSelfContent", "[bandself");
                realVersion = 4;
                General.WriteInt("SettingVersion", realVersion);
            }

            if (realVersion == 4)
            {
                DataTable hotkey = m_Dataset.Tables["HOTKEYS"];
                DataRow hotkeyrow = hotkey.NewRow();
                hotkeyrow.ItemArray = new object[] { "SpellsAgent", "Last Spell Last Target", Keys.None, true };
                hotkey.Rows.Add(hotkeyrow);
                realVersion = 5;
                General.WriteInt("SettingVersion", realVersion);
            }
            if (realVersion == 5)
            {
                DataTable general = m_Dataset.Tables["General"];
                general.Columns.Add("AllowHiddenLooting", typeof(bool));
                RazorEnhanced.Settings.General.WriteBool("AllowHiddenLooting", false);
                realVersion = 6;
                General.WriteInt("SettingVersion", realVersion);
            }
            if (realVersion == 6)
            {
                DataTable general = m_Dataset.Tables["General"];
                general.Columns.Add("DruidClericPackets", typeof(bool));
                RazorEnhanced.Settings.General.WriteBool("DruidClericPackets", false);
                realVersion = 7;
                General.WriteInt("SettingVersion", realVersion);
            }
			if (realVersion == 7)
			{
				DataTable buylist = m_Dataset.Tables["BUY_LISTS"];
                // quick fix for crash people were having, have not traced down root cause
                if (!buylist.Columns.Contains("CompareName"))
                {
                    buylist.Columns.Add("CompareName", typeof(bool));
                }
				foreach (DataRow row in m_Dataset.Tables["BUY_LISTS"].Rows)
				{
					row["CompareName"] = false;
				}
				realVersion = 8;
				General.WriteInt("SettingVersion", realVersion);
			}
            if (realVersion == 8)
            {
                DataTable general = m_Dataset.Tables["General"];
                general.Columns.Add("ShowTitheToolBarCheckBox", typeof(bool));
                RazorEnhanced.Settings.General.WriteBool("ShowTitheToolBarCheckBox", false);
                realVersion = 9;
                General.WriteInt("SettingVersion", realVersion);
            }


            Save(true);
		}



		// *************************************************************************
		// **************************** BACKUP SETTINGS ****************************
		// *************************************************************************

		internal static void MakeBackup(string profileName)
		{
			string backupDir = Path.Combine(Assistant.Engine.RootPath, "Backup", profileName);
			System.IO.Directory.CreateDirectory(backupDir);

			string src = Path.Combine(Assistant.Engine.RootPath, "Profiles", profileName);

			try
			{
				DirectoryInfo d = new DirectoryInfo(src);
				FileInfo[] Files = d.GetFiles("*");
				foreach (FileInfo file in Files)
				{
					File.Copy(file.FullName, Path.Combine(backupDir, file.Name), true);
				}

			}
			catch
			{
				MessageBox.Show("BackUp of: " + profileName + " Impossible to restore!");
				Environment.Exit(0);
			}

		}

		internal static void RestoreBackup(string profileName)
		{
			string backupDir = Path.Combine(Assistant.Engine.RootPath, "Backup", profileName);

			if (!Directory.Exists(backupDir))
			{
				MessageBox.Show("BackUp folder not exist! Can't restore: " + profileName);
				Environment.Exit(0);
			}

			try
			{
				DirectoryInfo d = new DirectoryInfo(backupDir);
				FileInfo[] Files = d.GetFiles("RazorEnhanced.settings.*");
				foreach (FileInfo file in Files)
				{
					string dest = Path.Combine(Assistant.Engine.RootPath, "Profiles", profileName);
					File.Copy(file.FullName, Path.Combine(dest, file.Name), true);
				}

			}
			catch
			{
				MessageBox.Show("BackUp of: " + profileName + " Impossible to restore!");
				Environment.Exit(0);
			}
		}
	}
}
