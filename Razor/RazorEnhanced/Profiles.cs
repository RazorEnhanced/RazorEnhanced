using Assistant;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Forms;

namespace RazorEnhanced
{
	internal class Profiles
	{
		private static string m_Save = "RazorEnhanced.profiles";
		private static DataSet m_Dataset;
		internal static DataSet Dataset { get { return m_Dataset; } }

		public class ProfilesData
		{
			private string m_Name;
			public string Name { get { return m_Name; } }

			private bool m_Last;
			internal bool Last { get { return m_Last; } }

			public ProfilesData(string name, bool last)
			{
				m_Name = name;
				m_Last = last;
			}
		}

		internal static void Load()
		{
			if (m_Dataset != null)
				return;

			m_Dataset = new DataSet();
			string filename = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), m_Save);

			if (File.Exists(filename))
			{
				try
				{
					Stream stream = File.Open(filename, FileMode.Open);
					m_Dataset.RemotingFormat = SerializationFormat.Binary;
					m_Dataset.SchemaSerializationMode = SchemaSerializationMode.IncludeSchema;
					GZipStream decompress = new GZipStream(stream, CompressionMode.Decompress);
					BinaryFormatter bin = new BinaryFormatter();
					m_Dataset = bin.Deserialize(decompress) as DataSet;
					decompress.Close();
					stream.Close();
					Settings.MakeBackup(m_Save);
				}
				catch
				{
                    MessageBox.Show("Error loading " + m_Save + ", Try to restore from backup!");
					Settings.RestoreBackup(m_Save);
					Load();
				}
			}
			else
			{
				// Profile
				DataTable profile = new DataTable("PROFILES");
				profile.Columns.Add("Name", typeof(string));
				profile.Columns.Add("Last", typeof(bool));
				profile.Columns.Add("PlayerName", typeof(string));
				profile.Columns.Add("PlayerSerial", typeof(int));

				DataRow profilerow = profile.NewRow();
				profilerow.ItemArray = new object[] { "default", true, "None", 0 };
				profile.Rows.Add(profilerow);

				m_Dataset.Tables.Add(profile);

				m_Dataset.AcceptChanges();
			}
		}

		// Funzioni di accesso al salvataggio
		internal static List<string> ReadAll()
		{
			List<string> profilelist = new List<string>();

			foreach (DataRow row in m_Dataset.Tables["PROFILES"].Rows)
			{
				profilelist.Add((string)row["Name"]);
			}

			return profilelist;
		}

		internal static string LastUsed()
		{
			foreach (DataRow row in m_Dataset.Tables["PROFILES"].Rows)
			{
				if ((bool)row["Last"])
					return (string)row["Name"];
			}

			return "default";
		}

		internal static void SetLast(string name)
		{
			foreach (DataRow row in m_Dataset.Tables["PROFILES"].Rows)
			{
				if ((string)row["Name"] == name)
				{
					row["Last"] = true;
				}
				else
					row["Last"] = false;
			}

			Save();
		}

		internal static void Add(string name)
		{
			DataRow row = m_Dataset.Tables["PROFILES"].NewRow();
			row["Name"] = (String)name;
			row["Last"] = (bool)true;
			row["PlayerName"] = (String)"None";
			row["PlayerSerial"] = (int)0;
			m_Dataset.Tables["PROFILES"].Rows.Add(row);

			Save();
		}

		internal static void Delete(string name)
		{
			foreach (DataRow row in m_Dataset.Tables["PROFILES"].Rows)
			{
				if ((string) row["Name"] != name)
					continue;

				row.Delete();
				break;
			}

			Save();
		}

		internal static bool Exist(string name)
		{
			return m_Dataset.Tables["PROFILES"].Rows.Cast<DataRow>().Any(row => (string) row["Name"] == name);
		}

		internal static string IsLinked(int serial)
		{
			return (from DataRow row in m_Dataset.Tables["PROFILES"].Rows where (int) row["PlayerSerial"] == serial select (string) row["Name"]).FirstOrDefault();
		}

		internal static string GetLinkName(string profilename)
		{
			return (from DataRow row in m_Dataset.Tables["PROFILES"].Rows where (string) row["Name"] == profilename select (string) row["PlayerName"]).FirstOrDefault();
		}

		internal static void Link(int serial, string profile, string playername)
		{
			foreach (DataRow row in m_Dataset.Tables["PROFILES"].Rows)  // Slinka se gia linkato
			{
				if ((int)row["PlayerSerial"] == serial)
				{
					row["PlayerSerial"] = 0;
					row["PlayerName"] = String.Empty;
				}
			}
			foreach (DataRow row in m_Dataset.Tables["PROFILES"].Rows)  // Linko nuovo profilo
			{
				if ((string)row["Name"] == profile)
				{
					row["PlayerSerial"] = serial;
					row["PlayerName"] = playername;
				}
			}
			Save();
		}

		internal static void UnLink(string profile)
		{
			foreach (DataRow row in m_Dataset.Tables["PROFILES"].Rows)
			{
				if ((string)row["Name"] == profile)
				{
					row["PlayerSerial"] = 0;
					row["PlayerName"] = "None";
				}
			}
			Save();
		}

		internal static void Rename(string oldname, string newname)
		{
			foreach (DataRow row in m_Dataset.Tables["PROFILES"].Rows)
			{
				if ((string)row["Name"] == oldname)
				{
					row["Name"] = newname;
					break;
				}
			}
			Save();
		}

		// Funzioni richiamate dalla gui

		internal static void Refresh()
		{
			Assistant.Engine.MainWindow.ProfilesComboBox.Items.Clear();
			List<string> profilelist = ReadAll();
			foreach (string profilename in profilelist)
			{
				Assistant.Engine.MainWindow.ProfilesComboBox.Items.Add(profilename);
			}

			Assistant.Engine.MainWindow.ProfilesComboBox.SelectedIndex = Assistant.Engine.MainWindow.ProfilesComboBox.Items.IndexOf(LastUsed());
		}

		internal static void ProfileChange(string name)
		{
			// Salvo parametri di uscita
			RazorEnhanced.Settings.General.SaveExitData();
		    PasswordMemory.ProfileChangeInit();
            Assistant.Engine.MainWindow.Initializing = true;

			// Stop timer update toolbar
			Assistant.TitleBar.Stop();

			// Stop forzato di tutti i thread agent
			if (Assistant.Engine.MainWindow.AutolootCheckBox.Checked == true)
				Assistant.Engine.MainWindow.AutolootCheckBox.Checked = false;

			if (Assistant.Engine.MainWindow.ScavengerCheckBox.Checked == true)
				Assistant.Engine.MainWindow.ScavengerCheckBox.Checked = false;

			if (Assistant.Engine.MainWindow.OrganizerStop.Enabled == true)
				Assistant.Engine.MainWindow.OrganizerStop.PerformClick();

			if (Assistant.Engine.MainWindow.BuyCheckBox.Checked == true)
				Assistant.Engine.MainWindow.BuyCheckBox.Checked = false;

			if (Assistant.Engine.MainWindow.SellCheckBox.Checked == true)
				Assistant.Engine.MainWindow.SellCheckBox.Checked = false;

			if (Assistant.Engine.MainWindow.DressStopButton.Enabled == true)
				Assistant.Engine.MainWindow.DressStopButton.PerformClick();

			if (Assistant.Engine.MainWindow.BandageHealenableCheckBox.Checked == true)
				Assistant.Engine.MainWindow.BandageHealenableCheckBox.Checked = false;

			if (Assistant.Engine.MainWindow.DressStopButton.Enabled == true)
				Assistant.Engine.MainWindow.DressStopButton.PerformClick();

			// Stop filtri
			if (Assistant.Engine.MainWindow.AutoCarverCheckBox.Enabled == true)
				Assistant.Engine.MainWindow.AutoCarverCheckBox.Checked = false;

			if (Assistant.Engine.MainWindow.MobFilterCheckBox.Enabled == true)
				Assistant.Engine.MainWindow.MobFilterCheckBox.Checked = false;

			// Svuoto logbox e reset select index
			Assistant.Engine.MainWindow.AutoLootLogBox.Items.Clear();
			AutoLoot.AddLog("Profile Changed!");

			Assistant.Engine.MainWindow.ScavengerLogBox.Items.Clear();
			Scavenger.AddLog("Profile Changed!");

			Assistant.Engine.MainWindow.OrganizerLogBox.Items.Clear();
			Organizer.AddLog("Profile Changed!");

			Assistant.Engine.MainWindow.SellLogBox.Items.Clear();
			SellAgent.AddLog("Profile Changed!");

			Assistant.Engine.MainWindow.BuyLogBox.Items.Clear();
			BuyAgent.AddLog("Profile Changed!");

			Assistant.Engine.MainWindow.DressLogBox.Items.Clear();
			Dress.AddLog("Profile Changed!");

			Assistant.Engine.MainWindow.FriendLogBox.Items.Clear();
			Friend.AddLog("Profile Changed!");

			Assistant.Engine.MainWindow.RestockLogBox.Items.Clear();
			Restock.AddLog("Profile Changed!");

			Assistant.Engine.MainWindow.BandageHealLogBox.Items.Clear();
			BandageHeal.AddLog("Profile Changed!");

			// Cambio file
			if (name == "default")
				RazorEnhanced.Settings.ProfileFiles = "RazorEnhanced.settings";
			else
				RazorEnhanced.Settings.ProfileFiles = "RazorEnhanced." + name + ".settings";

			// Rimuovo cache password e disabilito vecchi filtri
			Assistant.Filters.Filter.DisableAll();

			// Chiuto toolbar
			if (RazorEnhanced.ToolBar.ToolBarForm != null)
				RazorEnhanced.ToolBar.ToolBarForm.Close();

			// Chiuto toolbar
			if (RazorEnhanced.SpellGrid.SpellGridForm != null)
				RazorEnhanced.SpellGrid.SpellGridForm.Close();

			// Carico save profilo
			RazorEnhanced.Settings.Load();

			// Abilito patch UOMod
			UoMod.ProfileChange();

			// Refresh list 
			Assistant.Engine.MainWindow.AutoLootListSelect.SelectedIndex = -1;
			Assistant.Engine.MainWindow.ScavengerListSelect.SelectedIndex = -1;
			Assistant.Engine.MainWindow.OrganizerListSelect.SelectedIndex = -1;
			Assistant.Engine.MainWindow.SellListSelect.SelectedIndex = -1;
			Assistant.Engine.MainWindow.BuyListSelect.SelectedIndex = -1;
			Assistant.Engine.MainWindow.DressListSelect.SelectedIndex = -1;
			Assistant.Engine.MainWindow.FriendListSelect.SelectedIndex = -1;
			Assistant.Engine.MainWindow.RestockListSelect.SelectedIndex = -1;

			// Reinizzializzo razor
			Assistant.Engine.MainWindow.LoadSettings();

			// Apertura automatica toolbar se abilitata
			if (RazorEnhanced.Settings.General.ReadString("ToolBoxStyleComboBox") == "TitleBar")
				TitleBar.Start();
			else
			{
				// Riapro toollbar se le condizioni lo permettono
				if (RazorEnhanced.Settings.General.ReadBool("AutoopenToolBarCheckBox"))
					RazorEnhanced.ToolBar.Open();
			}

			// Riapro la spellgrid se le condizioni lo permettono
			if (RazorEnhanced.Settings.General.ReadBool("GridOpenLoginCheckBox"))
				RazorEnhanced.SpellGrid.Open();

			Assistant.Engine.MainWindow.Initializing = false;
			SetLast(name);

			PasswordMemory.ProfileChangeEnd();

			// Stop video recorder
			Assistant.MainForm.StopVideoRecorder();
		}

		internal static void Save()
		{
			try
			{
				m_Dataset.AcceptChanges();

				string filename = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), m_Save);

				m_Dataset.RemotingFormat = SerializationFormat.Binary;
				m_Dataset.SchemaSerializationMode = SchemaSerializationMode.IncludeSchema;
				Stream stream = File.Create(filename);
				GZipStream compress = new GZipStream(stream, CompressionMode.Compress);
				BinaryFormatter bin = new BinaryFormatter();
				bin.Serialize(compress, m_Dataset);
				compress.Close();
				stream.Close();
			}
			catch (Exception ex)
			{
				MessageBox.Show("Error writing " + m_Save + ": " + ex);
			}
		}
	}
}