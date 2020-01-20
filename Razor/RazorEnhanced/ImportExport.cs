using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.IO.Compression;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Forms;

namespace RazorEnhanced
{
	public class ImportExport
	{
		////////////// AUTOLOOT START //////////////
		internal static void ImportAutoloot()
		{
			DataSet m_Dataset = new DataSet();
			DataTable m_DatasetTable = new DataTable();
			OpenFileDialog od = new OpenFileDialog
			{
				Filter = "Enhanced Razor Export|*.raz",
				Title = "Import Autoloot List",
				RestoreDirectory = true
			};

			if (od.ShowDialog() == DialogResult.OK)
			{
				if (File.Exists(od.FileName))
				{
					try
					{
						m_Dataset.RemotingFormat = SerializationFormat.Binary;
						m_Dataset.SchemaSerializationMode = SchemaSerializationMode.IncludeSchema;
						Stream stream = File.Open(od.FileName, FileMode.Open);
						GZipStream decompress = new GZipStream(stream, CompressionMode.Decompress);
						BinaryFormatter bin = new BinaryFormatter();
						m_Dataset = bin.Deserialize(decompress) as DataSet;
						decompress.Close();
						stream.Close();
					}
					catch
					{
						AutoLoot.AddLog("File is corrupted!");
						return;
					}
				}
				else
				{
					AutoLoot.AddLog("Unable to access file!");
					return;
				}
				if (m_Dataset.Tables.Contains("AUTOLOOT_ITEMS"))
				{
					m_DatasetTable = m_Dataset.Tables["AUTOLOOT_ITEMS"];
					if (m_DatasetTable.Rows.Count > 0)
					{
						if (RazorEnhanced.Settings.AutoLoot.ListExists(m_Dataset.Tables["AUTOLOOT_ITEMS"].Rows[0]["List"].ToString()))
							AutoLoot.AddLog("List: " + m_Dataset.Tables["AUTOLOOT_ITEMS"].Rows[0]["List"].ToString() + " already exist");
						else
						{
							AutoLoot.AddList(m_Dataset.Tables["AUTOLOOT_ITEMS"].Rows[0]["List"].ToString());
							List<RazorEnhanced.AutoLoot.AutoLootItem> itemlist = new List<AutoLoot.AutoLootItem>();
							foreach (DataRow row in m_Dataset.Tables["AUTOLOOT_ITEMS"].Rows)
							{
								itemlist.Add((RazorEnhanced.AutoLoot.AutoLootItem)row["Item"]);
							}
							RazorEnhanced.Settings.AutoLoot.ItemInsertFromImport(m_Dataset.Tables["AUTOLOOT_ITEMS"].Rows[0]["List"].ToString(), itemlist);
							RazorEnhanced.AutoLoot.InitGrid();
							AutoLoot.AddLog("List: " + m_Dataset.Tables["AUTOLOOT_ITEMS"].Rows[0]["List"].ToString() + " imported!");
						}
					}
					else
					{
						AutoLoot.AddLog("This list is empty!");
						return;
					}
				}
				else
				{
					AutoLoot.AddLog("This file not contain Autoloot data!");
					return;
				}
			}
			else
			{
				AutoLoot.AddLog("Import list cancelled.");
				return;
			}
		}

		internal static void ExportAutoloot(string listname)
		{
			SaveFileDialog sd = new SaveFileDialog
			{
				Filter = "Enhanced Razor Export|*.raz",
				Title = "Export Autoloot List",
				FileName = "AU." + listname + ".raz",
				RestoreDirectory = true
			};

			if (sd.ShowDialog() == DialogResult.OK)
			{
				DataSet m_Dataset = new DataSet();
				DataTable autoloot_items = new DataTable("AUTOLOOT_ITEMS");
				autoloot_items.Columns.Add("List", typeof(string));
				autoloot_items.Columns.Add("Item", typeof(RazorEnhanced.AutoLoot.AutoLootItem));
				m_Dataset.Tables.Add(autoloot_items);
				m_Dataset.AcceptChanges();

				List<AutoLoot.AutoLootItem> items = Settings.AutoLoot.ItemsRead(listname);

				foreach (RazorEnhanced.AutoLoot.AutoLootItem item in items)
				{
					DataRow row = m_Dataset.Tables["AUTOLOOT_ITEMS"].NewRow();
					row["List"] = listname;
					row["Item"] = item;
					m_Dataset.Tables["AUTOLOOT_ITEMS"].Rows.Add(row);
				}

				try
				{
					m_Dataset.RemotingFormat = SerializationFormat.Binary;
					m_Dataset.SchemaSerializationMode = SchemaSerializationMode.IncludeSchema;
					Stream stream = File.Create(sd.FileName);
					GZipStream compress = new GZipStream(stream, CompressionMode.Compress);
					BinaryFormatter bin = new BinaryFormatter();
					bin.Serialize(compress, m_Dataset);
					compress.Close();
					stream.Close();
					AutoLoot.AddLog("List: " + listname + " exported");
				}
				catch (Exception ex)
				{
					AutoLoot.AddLog("Export list fail");
					AutoLoot.AddLog(ex.ToString());
				}
			}
			else
			{
				AutoLoot.AddLog("Export list cancelled.");
				return;
			}
		}

		////////////// AUTOLOOT END //////////////

		////////////// SCAVENGER START //////////////
		internal static void ImportScavenger()
		{
			DataSet m_Dataset = new DataSet();
			DataTable m_DatasetTable = new DataTable();

			OpenFileDialog od = new OpenFileDialog
			{
				Filter = "Enhanced Razor Export|*.raz",
				Title = "Import Scavenger List",
				RestoreDirectory = true
			};

			if (od.ShowDialog() == DialogResult.OK)
			{
				if (File.Exists(od.FileName))
				{
					try
					{
						m_Dataset.RemotingFormat = SerializationFormat.Binary;
						m_Dataset.SchemaSerializationMode = SchemaSerializationMode.IncludeSchema;
						Stream stream = File.Open(od.FileName, FileMode.Open);
						GZipStream decompress = new GZipStream(stream, CompressionMode.Decompress);
						BinaryFormatter bin = new BinaryFormatter();
						m_Dataset = bin.Deserialize(decompress) as DataSet;
						decompress.Close();
						stream.Close();
					}
					catch
					{
						Scavenger.AddLog("File is corrupted!");
						return;
					}
				}
				else
				{
					Scavenger.AddLog("Unable to access file!");
					return;
				}
				if (m_Dataset.Tables.Contains("SCAVENGER_ITEMS"))
				{
					m_DatasetTable = m_Dataset.Tables["SCAVENGER_ITEMS"];
					if (m_DatasetTable.Rows.Count > 0)
					{
						if (RazorEnhanced.Settings.Scavenger.ListExists(m_Dataset.Tables["SCAVENGER_ITEMS"].Rows[0]["List"].ToString()))
							Scavenger.AddLog("List: " + m_Dataset.Tables["SCAVENGER_ITEMS"].Rows[0]["List"].ToString() + " already exist");
						else
						{
							Scavenger.AddList(m_Dataset.Tables["SCAVENGER_ITEMS"].Rows[0]["List"].ToString());
							List<RazorEnhanced.Scavenger.ScavengerItem> itemlist = new List<Scavenger.ScavengerItem>();
							foreach (DataRow row in m_Dataset.Tables["SCAVENGER_ITEMS"].Rows)
							{
								itemlist.Add((RazorEnhanced.Scavenger.ScavengerItem)row["Item"]);
							}
							RazorEnhanced.Settings.Scavenger.ItemInsertFromImport(m_Dataset.Tables["SCAVENGER_ITEMS"].Rows[0]["List"].ToString(), itemlist);
							RazorEnhanced.Scavenger.InitGrid();
							Scavenger.AddLog("List: " + m_Dataset.Tables["SCAVENGER_ITEMS"].Rows[0]["List"].ToString() + " imported!");
						}
					}
					else
					{
						Scavenger.AddLog("This list is empty!");
						return;
					}
				}
				else
				{
					Scavenger.AddLog("This file not contain Scavenger data!");
					return;
				}
			}
			else
			{
				Scavenger.AddLog("Import list cancelled.");
				return;
			}
		}

		internal static void ExportScavenger(string listname)
		{
			SaveFileDialog sd = new SaveFileDialog
			{
				Filter = "Enhanced Razor Export|*.raz",
				Title = "Export Scavenger List",
				FileName = "SC." + listname + ".raz",
				RestoreDirectory = true
			};

			if (sd.ShowDialog() == DialogResult.OK)
			{
				DataSet m_Dataset = new DataSet();
				DataTable scavenger_items = new DataTable("SCAVENGER_ITEMS");
				scavenger_items.Columns.Add("List", typeof(string));
				scavenger_items.Columns.Add("Item", typeof(RazorEnhanced.Scavenger.ScavengerItem));
				m_Dataset.Tables.Add(scavenger_items);
				m_Dataset.AcceptChanges();

				List<Scavenger.ScavengerItem> items = Settings.Scavenger.ItemsRead(listname);

				foreach (RazorEnhanced.Scavenger.ScavengerItem item in items)
				{
					DataRow row = m_Dataset.Tables["SCAVENGER_ITEMS"].NewRow();
					row["List"] = listname;
					row["Item"] = item;
					m_Dataset.Tables["SCAVENGER_ITEMS"].Rows.Add(row);
				}

				try
				{
					m_Dataset.RemotingFormat = SerializationFormat.Binary;
					m_Dataset.SchemaSerializationMode = SchemaSerializationMode.IncludeSchema;
					Stream stream = File.Create(sd.FileName);
					GZipStream compress = new GZipStream(stream, CompressionMode.Compress);
					BinaryFormatter bin = new BinaryFormatter();
					bin.Serialize(compress, m_Dataset);
					compress.Close();
					stream.Close();
					Scavenger.AddLog("List: " + listname + " exported");
				}
				catch (Exception ex)
				{
					Scavenger.AddLog("Export list fail");
					Scavenger.AddLog(ex.ToString());
				}
			}
			else
			{
				Scavenger.AddLog("Export list cancelled.");
				return;
			}
		}

		////////////// SCAVENGER END //////////////

		////////////// ORGANIZER START //////////////
		internal static void ImportOrganizer()
		{
			DataSet m_Dataset = new DataSet();
			DataTable m_DatasetTable = new DataTable();

			OpenFileDialog od = new OpenFileDialog
			{
				Filter = "Enhanced Razor Export|*.raz",
				Title = "Import Organizer List",
				RestoreDirectory = true
			};

			if (od.ShowDialog() == DialogResult.OK)
			{
				if (File.Exists(od.FileName))
				{
					try
					{
						m_Dataset.RemotingFormat = SerializationFormat.Binary;
						m_Dataset.SchemaSerializationMode = SchemaSerializationMode.IncludeSchema;
						Stream stream = File.Open(od.FileName, FileMode.Open);
						GZipStream decompress = new GZipStream(stream, CompressionMode.Decompress);
						BinaryFormatter bin = new BinaryFormatter();
						m_Dataset = bin.Deserialize(decompress) as DataSet;
						decompress.Close();
						stream.Close();
					}
					catch
					{
						Organizer.AddLog("File is corrupted!");
						return;
					}
				}
				else
				{
					Organizer.AddLog("Unable to access file!");
					return;
				}
				if (m_Dataset.Tables.Contains("ORGANIZER_ITEMS"))
				{
					m_DatasetTable = m_Dataset.Tables["ORGANIZER_ITEMS"];
					if (m_DatasetTable.Rows.Count > 0)
					{
						if (RazorEnhanced.Settings.Organizer.ListExists(m_Dataset.Tables["ORGANIZER_ITEMS"].Rows[0]["List"].ToString()))
							Organizer.AddLog("List: " + m_Dataset.Tables["ORGANIZER_ITEMS"].Rows[0]["List"].ToString() + " already exist");
						else
						{
							Organizer.AddList(m_Dataset.Tables["ORGANIZER_ITEMS"].Rows[0]["List"].ToString());
							List<RazorEnhanced.Organizer.OrganizerItem> itemlist = new List<Organizer.OrganizerItem>();
							foreach (DataRow row in m_Dataset.Tables["ORGANIZER_ITEMS"].Rows)
							{
								itemlist.Add((RazorEnhanced.Organizer.OrganizerItem)row["Item"]);
							}
							RazorEnhanced.Settings.Organizer.ItemInsertFromImport(m_Dataset.Tables["ORGANIZER_ITEMS"].Rows[0]["List"].ToString(), itemlist);
							RazorEnhanced.Organizer.InitGrid();
							Organizer.AddLog("List: " + m_Dataset.Tables["ORGANIZER_ITEMS"].Rows[0]["List"].ToString() + " imported!");
						}
					}
					else
					{
						Organizer.AddLog("This list is empty!");
						return;
					}
				}
				else
				{
					Organizer.AddLog("This file not contain Organizer data!");
					return;
				}
			}
			else
			{
				Organizer.AddLog("Import list cancelled.");
				return;
			}
		}

		internal static void ExportOrganizer(string listname)
		{
			SaveFileDialog sd = new SaveFileDialog
			{
				Filter = "Enhanced Razor Export|*.raz",
				Title = "Export Organizer List",
				FileName = "OR." + listname + ".raz",
				RestoreDirectory = true
			};

			if (sd.ShowDialog() == DialogResult.OK)
			{
				DataSet m_Dataset = new DataSet();
				DataTable organizer_items = new DataTable("ORGANIZER_ITEMS");
				organizer_items.Columns.Add("List", typeof(string));
				organizer_items.Columns.Add("Item", typeof(RazorEnhanced.Organizer.OrganizerItem));
				m_Dataset.Tables.Add(organizer_items);
				m_Dataset.AcceptChanges();

				List<Organizer.OrganizerItem> items = RazorEnhanced.Settings.Organizer.ItemsRead(listname);

				foreach (RazorEnhanced.Organizer.OrganizerItem item in items)
				{
					DataRow row = m_Dataset.Tables["ORGANIZER_ITEMS"].NewRow();
					row["List"] = listname;
					row["Item"] = item;
					m_Dataset.Tables["ORGANIZER_ITEMS"].Rows.Add(row);
				}

				try
				{
					m_Dataset.RemotingFormat = SerializationFormat.Binary;
					m_Dataset.SchemaSerializationMode = SchemaSerializationMode.IncludeSchema;
					Stream stream = File.Create(sd.FileName);
					GZipStream compress = new GZipStream(stream, CompressionMode.Compress);
					BinaryFormatter bin = new BinaryFormatter();
					bin.Serialize(compress, m_Dataset);
					compress.Close();
					stream.Close();
					Organizer.AddLog("List: " + listname + " exported");
				}
				catch (Exception ex)
				{
					Organizer.AddLog("Export list fail");
					Organizer.AddLog(ex.ToString());
					return;
				}
			}
			else
			{
				Organizer.AddLog("Export list cancelled.");
				return;
			}
		}

		////////////// ORGANIZER END //////////////

		////////////// SELL AGENT START //////////////
		internal static void ImportSell()
		{
			OpenFileDialog od = new OpenFileDialog
			{
				Filter = "Enhanced Razor Export|*.raz",
				Title = "Import Sell Agent List",
				RestoreDirectory = true
			};

			if (od.ShowDialog() == DialogResult.OK)
			{
				DataSet m_Dataset = new DataSet();
				DataTable m_DatasetTable = new DataTable();

				if (File.Exists(od.FileName))
				{
					try
					{
						m_Dataset.RemotingFormat = SerializationFormat.Binary;
						m_Dataset.SchemaSerializationMode = SchemaSerializationMode.IncludeSchema;
						Stream stream = File.Open(od.FileName, FileMode.Open);
						GZipStream decompress = new GZipStream(stream, CompressionMode.Decompress);
						BinaryFormatter bin = new BinaryFormatter();
						m_Dataset = bin.Deserialize(decompress) as DataSet;
						decompress.Close();
						stream.Close();
					}
					catch
					{
						SellAgent.AddLog("File is corrupted!");
						return;
					}
				}
				else
				{
					SellAgent.AddLog("Unable to access file!");
					return;
				}
				if (m_Dataset.Tables.Contains("SELL_ITEMS"))
				{
					m_DatasetTable = m_Dataset.Tables["SELL_ITEMS"];
					if (m_DatasetTable.Rows.Count > 0)
					{
						if (RazorEnhanced.Settings.SellAgent.ListExists(m_Dataset.Tables["SELL_ITEMS"].Rows[0]["List"].ToString()))
							SellAgent.AddLog("List: " + m_Dataset.Tables["SELL_ITEMS"].Rows[0]["List"].ToString() + " already exist");
						else
						{
							SellAgent.AddList(m_Dataset.Tables["SELL_ITEMS"].Rows[0]["List"].ToString());
							List<RazorEnhanced.SellAgent.SellAgentItem> itemlist = new List<SellAgent.SellAgentItem>();
							foreach (DataRow row in m_Dataset.Tables["SELL_ITEMS"].Rows)
							{
								itemlist.Add((RazorEnhanced.SellAgent.SellAgentItem)row["Item"]);
							}
							RazorEnhanced.Settings.SellAgent.ItemInsertFromImport(m_Dataset.Tables["SELL_ITEMS"].Rows[0]["List"].ToString(), itemlist);
							RazorEnhanced.SellAgent.InitGrid();
							SellAgent.AddLog("List: " + m_Dataset.Tables["SELL_ITEMS"].Rows[0]["List"].ToString() + " imported!");
						}
					}
					else
					{
						SellAgent.AddLog("This list is empty!");
						return;
					}
				}
				else
				{
					SellAgent.AddLog("This file not contain SellAgent data!");
					return;
				}
			}
			else
			{
				SellAgent.AddLog("Import list cancelled.");
				return;
			}
		}

		internal static void ExportSell(string listname)
		{
			SaveFileDialog sd = new SaveFileDialog
			{
				Filter = "Enhanced Razor Export|*.raz",
				Title = "Export Sell Agent List",
				FileName = "SA." + listname + ".raz",
				RestoreDirectory = true
			};

			if (sd.ShowDialog() == DialogResult.OK)
			{
				DataSet m_Dataset = new DataSet();
				DataTable sell_items = new DataTable("SELL_ITEMS");
				sell_items.Columns.Add("List", typeof(string));
				sell_items.Columns.Add("Item", typeof(RazorEnhanced.SellAgent.SellAgentItem));
				m_Dataset.Tables.Add(sell_items);
				m_Dataset.AcceptChanges();

				List<SellAgent.SellAgentItem> items = Settings.SellAgent.ItemsRead(listname);

				foreach (RazorEnhanced.SellAgent.SellAgentItem item in items)
				{
					DataRow row = m_Dataset.Tables["SELL_ITEMS"].NewRow();
					row["List"] = listname;
					row["Item"] = item;
					m_Dataset.Tables["SELL_ITEMS"].Rows.Add(row);
				}

				try
				{
					m_Dataset.RemotingFormat = SerializationFormat.Binary;
					m_Dataset.SchemaSerializationMode = SchemaSerializationMode.IncludeSchema;
					Stream stream = File.Create(sd.FileName);
					GZipStream compress = new GZipStream(stream, CompressionMode.Compress);
					BinaryFormatter bin = new BinaryFormatter();
					bin.Serialize(compress, m_Dataset);
					compress.Close();
					stream.Close();
					SellAgent.AddLog("List: " + listname + " exported");
				}
				catch (Exception ex)
				{
					SellAgent.AddLog("Export list fail");
					SellAgent.AddLog(ex.ToString());
					return;
				}
			}
			else
			{
				SellAgent.AddLog("Export list cancelled.");
				return;
			}
		}

		////////////// SELL AGENT END //////////////

		////////////// BUY AGENT START //////////////
		internal static void ImportBuy()
		{
			DataSet m_Dataset = new DataSet();
			DataTable m_DatasetTable = new DataTable();
			OpenFileDialog od = new OpenFileDialog
			{
				Filter = "Enhanced Razor Export|*.raz",
				Title = "Import Buy Agent List",
				RestoreDirectory = true
			};

			if (od.ShowDialog() == DialogResult.OK)
			{
				if (File.Exists(od.FileName))
				{
					try
					{
						m_Dataset.RemotingFormat = SerializationFormat.Binary;
						m_Dataset.SchemaSerializationMode = SchemaSerializationMode.IncludeSchema;
						Stream stream = File.Open(od.FileName, FileMode.Open);
						GZipStream decompress = new GZipStream(stream, CompressionMode.Decompress);
						BinaryFormatter bin = new BinaryFormatter();
						m_Dataset = bin.Deserialize(decompress) as DataSet;
						decompress.Close();
						stream.Close();
					}
					catch
					{
						BuyAgent.AddLog("File is corrupted!");
						return;
					}
				}
				else
				{
					BuyAgent.AddLog("Unable to access file!");
					return;
				}
				if (m_Dataset.Tables.Contains("BUY_ITEMS"))
				{
					m_DatasetTable = m_Dataset.Tables["BUY_ITEMS"];
					if (m_DatasetTable.Rows.Count > 0)
					{
						if (RazorEnhanced.Settings.BuyAgent.ListExists(m_Dataset.Tables["BUY_ITEMS"].Rows[0]["List"].ToString()))
							BuyAgent.AddLog("List: " + m_Dataset.Tables["BUY_ITEMS"].Rows[0]["List"].ToString() + " already exist");
						else
						{
							BuyAgent.AddList(m_Dataset.Tables["BUY_ITEMS"].Rows[0]["List"].ToString());
							List<RazorEnhanced.BuyAgent.BuyAgentItem> itemlist = new List<BuyAgent.BuyAgentItem>();
							foreach (DataRow row in m_Dataset.Tables["BUY_ITEMS"].Rows)
							{
								itemlist.Add((RazorEnhanced.BuyAgent.BuyAgentItem)row["Item"]);
							}
							RazorEnhanced.Settings.BuyAgent.ItemInsertFromImport(m_Dataset.Tables["BUY_ITEMS"].Rows[0]["List"].ToString(), itemlist);
							RazorEnhanced.BuyAgent.InitGrid();
							BuyAgent.AddLog("List: " + m_Dataset.Tables["BUY_ITEMS"].Rows[0]["List"].ToString() + " imported!");
						}
					}
					else
					{
						BuyAgent.AddLog("This list is empty!");
						return;
					}
				}
				else
				{
					BuyAgent.AddLog("This file not contain BuyAgent data!");
					return;
				}
			}
			else
			{
				BuyAgent.AddLog("Import list cancelled.");
				return;
			}
		}

		internal static void ExportBuy(string listname)
		{
			SaveFileDialog sd = new SaveFileDialog
			{
				Filter = "Enhanced Razor Export|*.raz",
				Title = "Export Buy Agent List",
				FileName = "BA." + listname + ".raz",
				RestoreDirectory = true
			};

			if (sd.ShowDialog() == DialogResult.OK)
			{
				DataSet m_Dataset = new DataSet();
				DataTable buy_items = new DataTable("BUY_ITEMS");
				buy_items.Columns.Add("List", typeof(string));
				buy_items.Columns.Add("Item", typeof(RazorEnhanced.BuyAgent.BuyAgentItem));
				m_Dataset.Tables.Add(buy_items);
				m_Dataset.AcceptChanges();

				List<BuyAgent.BuyAgentItem> items = Settings.BuyAgent.ItemsRead(listname);

				foreach (RazorEnhanced.BuyAgent.BuyAgentItem item in items)
				{
					DataRow row = m_Dataset.Tables["BUY_ITEMS"].NewRow();
					row["List"] = listname;
					row["Item"] = item;
					m_Dataset.Tables["BUY_ITEMS"].Rows.Add(row);
				}

				try
				{
					m_Dataset.RemotingFormat = SerializationFormat.Binary;
					m_Dataset.SchemaSerializationMode = SchemaSerializationMode.IncludeSchema;
					Stream stream = File.Create(sd.FileName);
					GZipStream compress = new GZipStream(stream, CompressionMode.Compress);
					BinaryFormatter bin = new BinaryFormatter();
					bin.Serialize(compress, m_Dataset);
					compress.Close();
					stream.Close();
					BuyAgent.AddLog("List: " + listname + " exported");
				}
				catch (Exception ex)
				{
					BuyAgent.AddLog("Export list fail");
					BuyAgent.AddLog(ex.ToString());
					return;
				}
			}
			else
			{
				BuyAgent.AddLog("Export list cancelled.");
				return;
			}
		}

		////////////// BUY AGENT END //////////////

		////////////// DRESS START //////////////
		internal static void ImportDress()
		{
			DataSet m_Dataset = new DataSet();
			DataTable m_DatasetTable = new DataTable();
			OpenFileDialog od = new OpenFileDialog
			{
				Filter = "Enhanced Razor Export|*.raz",
				Title = "Import Dress List",
				RestoreDirectory = true
			};

			if (od.ShowDialog() == DialogResult.OK)
			{
				if (File.Exists(od.FileName))
				{
					try
					{
						m_Dataset.RemotingFormat = SerializationFormat.Binary;
						m_Dataset.SchemaSerializationMode = SchemaSerializationMode.IncludeSchema;
						Stream stream = File.Open(od.FileName, FileMode.Open);
						GZipStream decompress = new GZipStream(stream, CompressionMode.Decompress);
						BinaryFormatter bin = new BinaryFormatter();
						m_Dataset = bin.Deserialize(decompress) as DataSet;
						decompress.Close();
						stream.Close();
					}
					catch
					{
						Dress.AddLog("File is corrupted!");
						return;
					}
				}
				else
				{
					Dress.AddLog("Unable to access file!");
					return;
				}
				if (m_Dataset.Tables.Contains("DESS_ITEMS"))
				{
					m_DatasetTable = m_Dataset.Tables["DESS_ITEMS"];
					if (m_DatasetTable.Rows.Count > 0)
					{
						if (RazorEnhanced.Settings.Dress.ListExists(m_Dataset.Tables["DESS_ITEMS"].Rows[0]["List"].ToString()))
							Dress.AddLog("List: " + m_Dataset.Tables["DESS_ITEMS"].Rows[0]["List"].ToString() + " already exist");
						else
						{
							Dress.AddList(m_Dataset.Tables["DESS_ITEMS"].Rows[0]["List"].ToString());
							List<RazorEnhanced.Dress.DressItemNew> itemlist = new List<Dress.DressItemNew>();
							foreach (DataRow row in m_Dataset.Tables["DESS_ITEMS"].Rows)
							{
								itemlist.Add((RazorEnhanced.Dress.DressItemNew)row["Item"]);
							}
							RazorEnhanced.Settings.Dress.ItemInsertFromImport(m_Dataset.Tables["DESS_ITEMS"].Rows[0]["List"].ToString(), itemlist);
							RazorEnhanced.Dress.RefreshItems();
							Dress.AddLog("List: " + m_Dataset.Tables["DESS_ITEMS"].Rows[0]["List"].ToString() + " imported!");
						}
					}
					else
					{
						Dress.AddLog("This list is empty!");
						return;
					}
				}
				else
				{
					Dress.AddLog("This file not contain Dress data!");
					return;
				}
			}
			else
			{
				Dress.AddLog("Import list cancelled.");
				return;
			}
		}

		internal static void ExportDress(string listname)
		{
			SaveFileDialog sd = new SaveFileDialog
			{
				Filter = "Enhanced Razor Export|*.raz",
				Title = "Export Dress List",
				FileName = "DR." + listname + ".raz",
				RestoreDirectory = true
			};

			if (sd.ShowDialog() == DialogResult.OK)
			{
				DataSet m_Dataset = new DataSet();
				DataTable dress_items = new DataTable("DRESS_ITEMS");
				dress_items.Columns.Add("List", typeof(string));
				dress_items.Columns.Add("Item", typeof(RazorEnhanced.Dress.DressItemNew));
				m_Dataset.Tables.Add(dress_items);
				m_Dataset.AcceptChanges();

				List<Dress.DressItemNew> items = Settings.Dress.ItemsRead(listname);

				foreach (RazorEnhanced.Dress.DressItemNew item in items)
				{
					DataRow row = m_Dataset.Tables["DRESS_ITEMS"].NewRow();
					row["List"] = listname;
					row["Item"] = item;
					m_Dataset.Tables["DRESS_ITEMS"].Rows.Add(row);
				}

				try
				{
					m_Dataset.RemotingFormat = SerializationFormat.Binary;
					m_Dataset.SchemaSerializationMode = SchemaSerializationMode.IncludeSchema;
					Stream stream = File.Create(sd.FileName);
					GZipStream compress = new GZipStream(stream, CompressionMode.Compress);
					BinaryFormatter bin = new BinaryFormatter();
					bin.Serialize(compress, m_Dataset);
					compress.Close();
					stream.Close();
					Dress.AddLog("List: " + listname + " exported");
				}
				catch (Exception ex)
				{
					Dress.AddLog("Export list fail");
					Dress.AddLog(ex.ToString());
					return;
				}
			}
			else
			{
				Dress.AddLog("Export list cancelled.");
				return;
			}
		}

		////////////// BUY AGENT END //////////////

		////////////// FRIENDS START //////////////
		internal static void ImportFriends()
		{
			DataSet m_Dataset = new DataSet();
			DataTable m_DatasetTable = new DataTable();
			OpenFileDialog od = new OpenFileDialog
			{
				Filter = "Enhanced Razor Export|*.raz",
				Title = "Import Friends List",
				RestoreDirectory = true
			};

			if (od.ShowDialog() == DialogResult.OK)
			{
				if (File.Exists(od.FileName))
				{
					try
					{
						m_Dataset.RemotingFormat = SerializationFormat.Binary;
						m_Dataset.SchemaSerializationMode = SchemaSerializationMode.IncludeSchema;
						Stream stream = File.Open(od.FileName, FileMode.Open);
						GZipStream decompress = new GZipStream(stream, CompressionMode.Decompress);
						BinaryFormatter bin = new BinaryFormatter();
						m_Dataset = bin.Deserialize(decompress) as DataSet;
						decompress.Close();
						stream.Close();
					}
					catch
					{
						Friend.AddLog("File is corrupted!");
						return;
					}
				}
				else
				{
					Friend.AddLog("Unable to access file!");
					return;
				}
				if (m_Dataset.Tables.Contains("FRIEND_PLAYERS"))
				{
					m_DatasetTable = m_Dataset.Tables["FRIEND_PLAYERS"];
					if (m_DatasetTable.Rows.Count > 0)
					{
						if (RazorEnhanced.Settings.Friend.ListExists(m_Dataset.Tables["FRIEND_PLAYERS"].Rows[0]["List"].ToString()))
							Friend.AddLog("List: " + m_Dataset.Tables["FRIEND_PLAYERS"].Rows[0]["List"].ToString() + " already exist");
						else
						{
							Friend.AddList(m_Dataset.Tables["FRIEND_PLAYERS"].Rows[0]["List"].ToString());
							List<RazorEnhanced.Friend.FriendPlayer> itemlist = new List<Friend.FriendPlayer>();
							foreach (DataRow row in m_Dataset.Tables["FRIEND_PLAYERS"].Rows)
							{
								itemlist.Add((RazorEnhanced.Friend.FriendPlayer)row["Item"]);
							}
							RazorEnhanced.Settings.Friend.PlayerInsertFromImport(m_Dataset.Tables["FRIEND_PLAYERS"].Rows[0]["List"].ToString(), itemlist);
							RazorEnhanced.Friend.RefreshPlayers();
							Friend.AddLog("List: " + m_Dataset.Tables["FRIEND_PLAYERS"].Rows[0]["List"].ToString() + " imported!");
						}
					}
					else
					{
						Friend.AddLog("This list is empty!");
						return;
					}
				}
				else
				{
					Friend.AddLog("This file not contain Restock data!");
					return;
				}
			}
			else
			{
				Friend.AddLog("Import list cancelled.");
				return;
			}
		}

		internal static void ExportFriends(string listname)
		{
			SaveFileDialog sd = new SaveFileDialog
			{
				Filter = "Enhanced Razor Export|*.raz",
				Title = "Export Friend List",
				FileName = "FL." + listname + ".raz",
				RestoreDirectory = true
			};

			if (sd.ShowDialog() == DialogResult.OK)
			{
				DataSet m_Dataset = new DataSet();
				DataTable restock_item = new DataTable("FRIEND_PLAYERS");
				restock_item.Columns.Add("List", typeof(string));
				restock_item.Columns.Add("Item", typeof(RazorEnhanced.Friend.FriendPlayer));
				m_Dataset.Tables.Add(restock_item);
				m_Dataset.AcceptChanges();

				RazorEnhanced.Settings.Friend.PlayersRead(listname, out List<Friend.FriendPlayer> players);

				foreach (RazorEnhanced.Friend.FriendPlayer player in players)
				{
					DataRow row = m_Dataset.Tables["FRIEND_PLAYERS"].NewRow();
					row["List"] = listname;
					row["Item"] = player;
					m_Dataset.Tables["FRIEND_PLAYERS"].Rows.Add(row);
				}

				try
				{
					m_Dataset.RemotingFormat = SerializationFormat.Binary;
					m_Dataset.SchemaSerializationMode = SchemaSerializationMode.IncludeSchema;
					Stream stream = File.Create(sd.FileName);
					GZipStream compress = new GZipStream(stream, CompressionMode.Compress);
					BinaryFormatter bin = new BinaryFormatter();
					bin.Serialize(compress, m_Dataset);
					compress.Close();
					stream.Close();
					Friend.AddLog("List: " + listname + " exported");
				}
				catch (Exception ex)
				{
					Friend.AddLog("Export list fail");
					Friend.AddLog(ex.ToString());
					return;
				}
			}
			else
			{
				Friend.AddLog("Export list cancelled.");
				return;
			}
		}

		////////////// FRIENDS END //////////////

		////////////// RESTOCK START //////////////
		internal static void ImportRestock()
		{
			DataSet m_Dataset = new DataSet();
			DataTable m_DatasetTable = new DataTable();
			OpenFileDialog od = new OpenFileDialog
			{
				Filter = "Enhanced Razor Export|*.raz",
				Title = "Import Restock List",
				RestoreDirectory = true
			};

			if (od.ShowDialog() == DialogResult.OK)
			{
				if (File.Exists(od.FileName))
				{
					try
					{
						m_Dataset.RemotingFormat = SerializationFormat.Binary;
						m_Dataset.SchemaSerializationMode = SchemaSerializationMode.IncludeSchema;
						Stream stream = File.Open(od.FileName, FileMode.Open);
						GZipStream decompress = new GZipStream(stream, CompressionMode.Decompress);
						BinaryFormatter bin = new BinaryFormatter();
						m_Dataset = bin.Deserialize(decompress) as DataSet;
						decompress.Close();
						stream.Close();
					}
					catch
					{
						Restock.AddLog("File is corrupted!");
						return;
					}
				}
				else
				{
					Restock.AddLog("Unable to access file!");
					return;
				}
				if (m_Dataset.Tables.Contains("RESTOCK_ITEMS"))
				{
					m_DatasetTable = m_Dataset.Tables["RESTOCK_ITEMS"];
					if (m_DatasetTable.Rows.Count > 0)
					{
						if (RazorEnhanced.Settings.Restock.ListExists(m_Dataset.Tables["RESTOCK_ITEMS"].Rows[0]["List"].ToString()))
							Restock.AddLog("List: " + m_Dataset.Tables["RESTOCK_ITEMS"].Rows[0]["List"].ToString() + " already exist");
						else
						{
							List<RazorEnhanced.Restock.RestockItem> itemlist = new List<Restock.RestockItem>();
							Restock.AddList(m_Dataset.Tables["RESTOCK_ITEMS"].Rows[0]["List"].ToString());
							foreach (DataRow row in m_Dataset.Tables["RESTOCK_ITEMS"].Rows)
							{
								itemlist.Add((RazorEnhanced.Restock.RestockItem)row["Item"]);
							}
							RazorEnhanced.Settings.Restock.ItemInsertFromImport(m_Dataset.Tables["RESTOCK_ITEMS"].Rows[0]["List"].ToString(), itemlist);
							RazorEnhanced.Restock.InitGrid();
							Restock.AddLog("List: " + m_Dataset.Tables["RESTOCK_ITEMS"].Rows[0]["List"].ToString() + " imported!");
						}
					}
					else
					{
						Restock.AddLog("This list is empty!");
						return;
					}
				}
				else
				{
					Restock.AddLog("This file not contain Friends data!");
					return;
				}
			}
			else
			{
				Restock.AddLog("Import list cancelled.");
				return;
			}
		}

		internal static void ExportRestock(string listname)
		{
			SaveFileDialog sd = new SaveFileDialog
			{
				Filter = "Enhanced Razor Export|*.raz",
				Title = "Export Restock List",
				FileName = "RR." + listname + ".raz",
				RestoreDirectory = true
			};

			if (sd.ShowDialog() == DialogResult.OK)
			{
				DataSet m_Dataset = new DataSet();
				DataTable friend_player = new DataTable("RESTOCK_ITEMS");
				friend_player.Columns.Add("List", typeof(string));
				friend_player.Columns.Add("Item", typeof(RazorEnhanced.Restock.RestockItem));
				m_Dataset.Tables.Add(friend_player);
				m_Dataset.AcceptChanges();

				List<Restock.RestockItem> items = Settings.Restock.ItemsRead(listname);

				foreach (RazorEnhanced.Restock.RestockItem item in items)
				{
					DataRow row = m_Dataset.Tables["RESTOCK_ITEMS"].NewRow();
					row["List"] = listname;
					row["Item"] = item;
					m_Dataset.Tables["RESTOCK_ITEMS"].Rows.Add(row);
				}

				try
				{
					m_Dataset.RemotingFormat = SerializationFormat.Binary;
					m_Dataset.SchemaSerializationMode = SchemaSerializationMode.IncludeSchema;
					Stream stream = File.Create(sd.FileName);
					GZipStream compress = new GZipStream(stream, CompressionMode.Compress);
					BinaryFormatter bin = new BinaryFormatter();
					bin.Serialize(compress, m_Dataset);
					compress.Close();
					stream.Close();
					Restock.AddLog("List: " + listname + " exported");
				}
				catch (Exception ex)
				{
					Restock.AddLog("Export list fail");
					Restock.AddLog(ex.ToString());
					return;
				}
			}
			else
			{
				Restock.AddLog("Export list cancelled.");
				return;
			}
		}

		////////////// RESTOCK END //////////////

		////////////// PROFILES START //////////////

		internal static void ExportProfiles(string profilename)
		{
			SaveFileDialog sd = new SaveFileDialog
			{
				Filter = "Enhanced Razor Export|*.raz",
				Title = "Export Profile",
				FileName = "PROF." + profilename + ".raz",
				RestoreDirectory = true
			};

			string oldprofilepath;
			if (RazorEnhanced.Profiles.LastUsed() == "default")
				oldprofilepath = Path.Combine(Assistant.Engine.RootPath, "Profiles", "RazorEnhanced.settings");
			else
				oldprofilepath = Path.Combine(Assistant.Engine.RootPath, "Profiles", "RazorEnhanced." + RazorEnhanced.Profiles.LastUsed() + ".settings");

			if (sd.ShowDialog() == DialogResult.OK)
			{
				if (File.Exists(oldprofilepath))
				{
					File.Copy(oldprofilepath, sd.FileName, true);
				}
				else
				{
					MessageBox.Show("Error during exporting profile!",
					"Enhanced Profiles",
					MessageBoxButtons.OK,
					MessageBoxIcon.Exclamation,
					MessageBoxDefaultButton.Button1);
				}
			}
		}

		internal static void ImportProfiles(string newprofilename, string oldprofilepath)
		{
			DataSet m_Dataset = new DataSet();
			DataTable m_DatasetTable = new DataTable();
			try
			{
				m_Dataset.RemotingFormat = SerializationFormat.Binary;
				m_Dataset.SchemaSerializationMode = SchemaSerializationMode.IncludeSchema;
				Stream stream = File.Open(oldprofilepath, FileMode.Open);
				GZipStream decompress = new GZipStream(stream, CompressionMode.Decompress);
				BinaryFormatter bin = new BinaryFormatter();
				m_Dataset = bin.Deserialize(decompress) as DataSet;
				decompress.Close();
				stream.Close();
			}
			catch
			{
				MessageBox.Show("File is corrupted!");
				return;
			}

			if (!m_Dataset.Tables.Contains("GENERAL"))
			{
				MessageBox.Show("This file not contain Profile data!");
				return;
			}
			else
			{
				File.Copy(oldprofilepath, Path.Combine(Assistant.Engine.RootPath, "Profiles", "RazorEnhanced." + newprofilename + ".settings"), true);
				RazorEnhanced.Profiles.Add(newprofilename);
				RazorEnhanced.Profiles.Refresh();
			}
		}

		////////////// PROFILES END //////////////

		////////////// GRAPHFILTER START //////////////
		internal static void ImportGraphFilter()
		{
			DataSet m_Dataset = new DataSet();
			DataTable m_DatasetTable = new DataTable();
			OpenFileDialog od = new OpenFileDialog
			{
				Filter = "Enhanced Razor Export|*.raz",
				Title = "Import Graph Filter",
				RestoreDirectory = true
			};

			if (od.ShowDialog() == DialogResult.OK)
			{
				if (File.Exists(od.FileName))
				{
					try
					{
						m_Dataset.RemotingFormat = SerializationFormat.Binary;
						m_Dataset.SchemaSerializationMode = SchemaSerializationMode.IncludeSchema;
						Stream stream = File.Open(od.FileName, FileMode.Open);
						GZipStream decompress = new GZipStream(stream, CompressionMode.Decompress);
						BinaryFormatter bin = new BinaryFormatter();
						m_Dataset = bin.Deserialize(decompress) as DataSet;
						decompress.Close();
						stream.Close();
					}
					catch
					{
						MessageBox.Show("File is corrupted!");
						return;
					}
				}
				else
				{
					MessageBox.Show("Unable to access file!");
					return;
				}

				if (m_Dataset.Tables.Contains("FILTER_GRAPH"))
				{
					m_DatasetTable = m_Dataset.Tables["FILTER_GRAPH"];
					if (m_DatasetTable.Rows.Count > 0)
					{
						Settings.GraphFilter.ClearList(); // Clear old data

						foreach (DataRow row in m_Dataset.Tables["FILTER_GRAPH"].Rows)
						{
							Filters.GraphChangeData d = (Filters.GraphChangeData)row["Graph"];
							Settings.GraphFilter.Insert(d.Selected, d.GraphReal, d.GraphNew, d.ColorNew);
						}

						Filters.InitGraphGrid();
					}
				}
				else
				{
					MessageBox.Show("This file not contain Graph Filter data!");
					return;
				}
			}
		}

		internal static void ExportGraphFilter()
		{
			SaveFileDialog sd = new SaveFileDialog
			{
				Filter = "Enhanced Razor Export|*.raz",
				Title = "Export Graph Filter List",
				FileName = "GraphFilter.raz",
				RestoreDirectory = true
			};

			if (sd.ShowDialog() == DialogResult.OK)
			{
				DataSet m_Dataset = new DataSet();
				DataTable filter_graph = new DataTable("FILTER_GRAPH");
				filter_graph.Columns.Add("Graph", typeof(Filters.GraphChangeData));
				m_Dataset.Tables.Add(filter_graph);
				m_Dataset.AcceptChanges();

				List<Filters.GraphChangeData> filters = Settings.GraphFilter.ReadAll();

				foreach (Filters.GraphChangeData filter in filters)
				{
					DataRow row = m_Dataset.Tables["FILTER_GRAPH"].NewRow();
					row["Graph"] = filter;
					m_Dataset.Tables["FILTER_GRAPH"].Rows.Add(row);
				}

				try
				{
					m_Dataset.RemotingFormat = SerializationFormat.Binary;
					m_Dataset.SchemaSerializationMode = SchemaSerializationMode.IncludeSchema;
					Stream stream = File.Create(sd.FileName);
					GZipStream compress = new GZipStream(stream, CompressionMode.Compress);
					BinaryFormatter bin = new BinaryFormatter();
					bin.Serialize(compress, m_Dataset);
					compress.Close();
					stream.Close();
				}
				catch (Exception ex)
				{
					MessageBox.Show(ex.ToString(), "Export Graph Filter fail");
					return;
				}
			}
		}

		////////////// GRAPHFILTER END //////////////


		////////////// TARGET START //////////////
		internal static void ImportTargetFilter(ListBox targetlistBox)
		{
			// import and export no longer needed
		}

		internal static void ExportTargetFilter(string name)
		{
		// import and export no longer needed
		}

		////////////// TARGET END //////////////
	}
}