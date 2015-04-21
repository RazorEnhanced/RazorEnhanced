using System;
using System.Linq;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.IO.Compression;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Forms;
using Assistant;

namespace RazorEnhanced
{
    public class ImportExport
    {
        ////////////// AUTOLOOT START //////////////
        internal static void ImportAutoloot()
        {
            DataSet m_Dataset = new DataSet();
            DataTable m_DatasetTable = new DataTable();
            OpenFileDialog od = new OpenFileDialog();
            od.Filter = "Enhanced Razor Export|*.raz";
            od.Title = "Import Autoloot List";
            od.RestoreDirectory = true;

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
                        decompress.Dispose();
                        stream.Close();
                        stream.Dispose();
                    }
                    catch
                    {
                        AutoLoot.AddLog("File is corrupted!");
                    }
                }
                else
                {
                    AutoLoot.AddLog("Unable to access file!");
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
                            foreach (DataRow row in m_Dataset.Tables["AUTOLOOT_ITEMS"].Rows)
                            {
                                RazorEnhanced.Settings.AutoLoot.ItemInsert((string)row["List"], (RazorEnhanced.AutoLoot.AutoLootItem)row["Item"]);
                            }
                            RazorEnhanced.AutoLoot.RefreshItems();
                            AutoLoot.AddLog("List: " + m_Dataset.Tables["AUTOLOOT_ITEMS"].Rows[0]["List"].ToString() + " imported!");
                        }
                    }
                    else
                    {
                        AutoLoot.AddLog("This list is empty!");
                    }
                }
                else
                {
                    AutoLoot.AddLog("This file not contain Autoloot data!");
                }
            }
            else
            {
                AutoLoot.AddLog("Import list cancelled.");
            }

        }
        internal static void ExportAutoloot(string listname)
        {
            SaveFileDialog sd = new SaveFileDialog();
            sd.Filter = "Enhanced Razor Export|*.raz";
            sd.Title = "Export Autoloot List";
            sd.FileName = "AU." + listname + ".raz";
            sd.RestoreDirectory = true;

            if (sd.ShowDialog() == DialogResult.OK)
            {
                DataSet m_Dataset = new DataSet();
                DataTable autoloot_items = new DataTable("AUTOLOOT_ITEMS");
                autoloot_items.Columns.Add("List", typeof(string));
                autoloot_items.Columns.Add("Item", typeof(RazorEnhanced.AutoLoot.AutoLootItem));
                m_Dataset.Tables.Add(autoloot_items);
                m_Dataset.AcceptChanges();

                List<AutoLoot.AutoLootItem> items;
                RazorEnhanced.Settings.AutoLoot.ItemsRead(listname, out items);

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
                    compress.Dispose();
                    stream.Close();
                    stream.Dispose();
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
            }
        }
        ////////////// AUTOLOOT END //////////////

        ////////////// SCAVENGER START //////////////
        internal static void ImportScavenger()
        {
            DataSet m_Dataset = new DataSet();
            DataTable m_DatasetTable = new DataTable();

            OpenFileDialog od = new OpenFileDialog();
            od.Filter = "Enhanced Razor Export|*.raz";
            od.Title = "Import Scavenger List";
            od.RestoreDirectory = true;

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
                        decompress.Dispose();
                        stream.Close();
                        stream.Dispose();
                    }
                    catch
                    {
                        Scavenger.AddLog("File is corrupted!");
                    }
                }
                else
                {
                    Scavenger.AddLog("Unable to access file!");
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
                            foreach (DataRow row in m_Dataset.Tables["SCAVENGER_ITEMS"].Rows)
                            {
                                RazorEnhanced.Settings.Scavenger.ItemInsert((string)row["List"], (RazorEnhanced.Scavenger.ScavengerItem)row["Item"]);
                            }
                            RazorEnhanced.Scavenger.RefreshItems();
                            Scavenger.AddLog("List: " + m_Dataset.Tables["SCAVENGER_ITEMS"].Rows[0]["List"].ToString() + " imported!");
                        }
                    }
                    else
                    {
                        Scavenger.AddLog("This list is empty!");
                    }
                }
                else
                {
                    Scavenger.AddLog("This file not contain Scavenger data!");
                }
            }
            else
            {
                Scavenger.AddLog("Import list cancelled.");
            }
        }
        internal static void ExportScavenger(string listname)
        {
            SaveFileDialog sd = new SaveFileDialog();
            sd.Filter = "Enhanced Razor Export|*.raz";
            sd.Title = "Export Scavenger List";
            sd.FileName = "SC." + listname + ".raz";
            sd.RestoreDirectory = true;

            if (sd.ShowDialog() == DialogResult.OK)
            {
                DataSet m_Dataset = new DataSet();
                DataTable scavenger_items = new DataTable("SCAVENGER_ITEMS");
                scavenger_items.Columns.Add("List", typeof(string));
                scavenger_items.Columns.Add("Item", typeof(RazorEnhanced.Scavenger.ScavengerItem));
                m_Dataset.Tables.Add(scavenger_items);
                m_Dataset.AcceptChanges();

                List<Scavenger.ScavengerItem> items;
                RazorEnhanced.Settings.Scavenger.ItemsRead(listname, out items);

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
                    compress.Dispose();
                    stream.Close();
                    stream.Dispose();
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
            }

        }
        ////////////// SCAVENGER END //////////////

        ////////////// ORGANIZER START //////////////
        internal static void ImportOrganizer()
        {
            DataSet m_Dataset = new DataSet();
            DataTable m_DatasetTable = new DataTable();

            OpenFileDialog od = new OpenFileDialog();
            od.Filter = "Enhanced Razor Export|*.raz";
            od.Title = "Import Organizer List";
            od.RestoreDirectory = true;

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
                        decompress.Dispose();
                        stream.Close();
                        stream.Dispose();
                    }
                    catch
                    {
                        Organizer.AddLog("File is corrupted!");
                    }
                }
                else
                {
                    Organizer.AddLog("Unable to access file!");
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
                            foreach (DataRow row in m_Dataset.Tables["ORGANIZER_ITEMS"].Rows)
                            {
                                RazorEnhanced.Settings.Organizer.ItemInsert((string)row["List"], (RazorEnhanced.Organizer.OrganizerItem)row["Item"]);
                            }
                            RazorEnhanced.Organizer.RefreshItems();
                            Organizer.AddLog("List: " + m_Dataset.Tables["ORGANIZER_ITEMS"].Rows[0]["List"].ToString() + " imported!");
                        }
                    }
                    else
                    {
                        Organizer.AddLog("This list is empty!");
                    }
                }
                else
                {
                    Organizer.AddLog("This file not contain Organizer data!");
                }
            }
            else
            {
                Organizer.AddLog("Import list cancelled.");
            }

        }
        internal static void ExportOrganizer(string listname)
        {
            SaveFileDialog sd = new SaveFileDialog();
            sd.Filter = "Enhanced Razor Export|*.raz";
            sd.Title = "Export Organizer List";
            sd.FileName = "OR." + listname + ".raz";
            sd.RestoreDirectory = true;

            if (sd.ShowDialog() == DialogResult.OK)
            {
                DataSet m_Dataset = new DataSet();
                DataTable organizer_items = new DataTable("ORGANIZER_ITEMS");
                organizer_items.Columns.Add("List", typeof(string));
                organizer_items.Columns.Add("Item", typeof(RazorEnhanced.Organizer.OrganizerItem));
                m_Dataset.Tables.Add(organizer_items);
                m_Dataset.AcceptChanges();

                List<Organizer.OrganizerItem> items;
                RazorEnhanced.Settings.Organizer.ItemsRead(listname, out items);

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
                    compress.Dispose();
                    stream.Close();
                    stream.Dispose();
                    Organizer.AddLog("List: " + listname + " exported");
                }
                catch (Exception ex)
                {
                    Organizer.AddLog("Export list fail");
                    Organizer.AddLog(ex.ToString());
                }
            }
            else
            {
                Organizer.AddLog("Export list cancelled.");
            }
        }
        ////////////// ORGANIZER END //////////////

        ////////////// SELL AGENT START //////////////
        internal static void ImportSell()
        {
            OpenFileDialog od = new OpenFileDialog();
            od.Filter = "Enhanced Razor Export|*.raz";
            od.Title = "Import Sell Agent List";
            od.RestoreDirectory = true;

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
                        decompress.Dispose();
                        stream.Close();
                        stream.Dispose();
                    }
                    catch
                    {
                        SellAgent.AddLog("File is corrupted!");
                    }
                }
                else
                {
                    SellAgent.AddLog("Unable to access file!");
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
                            foreach (DataRow row in m_Dataset.Tables["SELL_ITEMS"].Rows)
                            {
                                RazorEnhanced.Settings.SellAgent.ItemInsert((string)row["List"], (RazorEnhanced.SellAgent.SellAgentItem)row["Item"]);
                            }
                            RazorEnhanced.SellAgent.RefreshItems();
                            SellAgent.AddLog("List: " + m_Dataset.Tables["SELL_ITEMS"].Rows[0]["List"].ToString() + " imported!");
                        }
                    }
                    else
                    {
                        SellAgent.AddLog("This list is empty!");
                    }
                }
                else
                {
                    SellAgent.AddLog("This file not contain SellAgent data!");
                }
            }
            else
            {
                SellAgent.AddLog("Import list cancelled.");
            }
        }
        internal static void ExportSell(string listname)
        {
            SaveFileDialog sd = new SaveFileDialog();
            sd.Filter = "Enhanced Razor Export|*.raz";
            sd.Title = "Export Sell Agent List";
            sd.FileName = "SA." + listname + ".raz";
            sd.RestoreDirectory = true;

            if (sd.ShowDialog() == DialogResult.OK)
            {
                DataSet m_Dataset = new DataSet();
                DataTable sell_items = new DataTable("SELL_ITEMS");
                sell_items.Columns.Add("List", typeof(string));
                sell_items.Columns.Add("Item", typeof(RazorEnhanced.SellAgent.SellAgentItem));
                m_Dataset.Tables.Add(sell_items);
                m_Dataset.AcceptChanges();

                List<SellAgent.SellAgentItem> items;
                RazorEnhanced.Settings.SellAgent.ItemsRead(listname, out items);

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
                    compress.Dispose();
                    stream.Close();
                    stream.Dispose();
                    SellAgent.AddLog("List: " + listname + " exported");
                }
                catch (Exception ex)
                {
                    SellAgent.AddLog("Export list fail");
                    SellAgent.AddLog(ex.ToString());
                }
            }
            else
            {
                SellAgent.AddLog("Export list cancelled.");
            }
        }
        ////////////// SELL AGENT END //////////////

        ////////////// BUY AGENT START //////////////
        internal static void ImportBuy()
        {
            DataSet m_Dataset = new DataSet();
            DataTable m_DatasetTable = new DataTable();
            OpenFileDialog od = new OpenFileDialog();
            od.Filter = "Enhanced Razor Export|*.raz";
            od.Title = "Import Buy Agent List";
            od.RestoreDirectory = true;

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
                        decompress.Dispose();
                        stream.Close();
                        stream.Dispose();
                    }
                    catch
                    {
                        BuyAgent.AddLog("File is corrupted!");
                    }
                }
                else
                {
                    BuyAgent.AddLog("Unable to access file!");
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
                            foreach (DataRow row in m_Dataset.Tables["BUY_ITEMS"].Rows)
                            {
                                RazorEnhanced.Settings.BuyAgent.ItemInsert((string)row["List"], (RazorEnhanced.BuyAgent.BuyAgentItem)row["Item"]);
                            }
                            RazorEnhanced.BuyAgent.RefreshItems();
                            BuyAgent.AddLog("List: " + m_Dataset.Tables["BUY_ITEMS"].Rows[0]["List"].ToString() + " imported!");
                        }
                    }
                    else
                    {
                        BuyAgent.AddLog("This list is empty!");
                    }
                }
                else
                {
                    BuyAgent.AddLog("This file not contain BuyAgent data!");
                }
            }
            else
            {
                BuyAgent.AddLog("Import list cancelled.");
            }
        }
        internal static void ExportBuy(string listname)
        {
            SaveFileDialog sd = new SaveFileDialog();
            sd.Filter = "Enhanced Razor Export|*.raz";
            sd.Title = "Export Buy Agent List";
            sd.FileName = "BA." + listname + ".raz";
            sd.RestoreDirectory = true;

            if (sd.ShowDialog() == DialogResult.OK)
            {
                DataSet m_Dataset = new DataSet();
                DataTable buy_items = new DataTable("BUY_ITEMS");
                buy_items.Columns.Add("List", typeof(string));
                buy_items.Columns.Add("Item", typeof(RazorEnhanced.BuyAgent.BuyAgentItem));
                m_Dataset.Tables.Add(buy_items);
                m_Dataset.AcceptChanges();

                List<BuyAgent.BuyAgentItem> items;
                RazorEnhanced.Settings.BuyAgent.ItemsRead(listname, out items);

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
                    compress.Dispose();
                    stream.Close();
                    stream.Dispose();
                    BuyAgent.AddLog("List: " + listname + " exported");
                }
                catch (Exception ex)
                {
                    BuyAgent.AddLog("Export list fail");
                    BuyAgent.AddLog(ex.ToString());
                }
            }
            else
            {
                BuyAgent.AddLog("Export list cancelled.");
            }

        }
        ////////////// BUY AGENT END //////////////

        ////////////// DRESS START //////////////
        internal static void ImportDress()
        {
            DataSet m_Dataset = new DataSet();
            DataTable m_DatasetTable = new DataTable();
            OpenFileDialog od = new OpenFileDialog();
            od.Filter = "Enhanced Razor Export|*.raz";
            od.Title = "Import Dress List";
            od.RestoreDirectory = true;

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
                        decompress.Dispose();
                        stream.Close();
                        stream.Dispose();
                    }
                    catch
                    {
                        Dress.AddLog("File is corrupted!");
                    }
                }
                else
                {
                    Dress.AddLog("Unable to access file!");
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
                            foreach (DataRow row in m_Dataset.Tables["DESS_ITEMS"].Rows)
                            {
                                RazorEnhanced.Settings.Dress.ItemInsert((string)row["List"], (RazorEnhanced.Dress.DressItem)row["Item"]);
                            }
                            RazorEnhanced.Dress.RefreshItems();
                            Dress.AddLog("List: " + m_Dataset.Tables["DESS_ITEMS"].Rows[0]["List"].ToString() + " imported!");
                        }
                    }
                    else
                    {
                        Dress.AddLog("This list is empty!");
                    }
                }
                else
                {
                    Dress.AddLog("This file not contain Dress data!");
                }
            }
            else
            {
                Dress.AddLog("Import list cancelled.");
            }
        }
        internal static void ExportDress(string listname)
        {
            SaveFileDialog sd = new SaveFileDialog();
            sd.Filter = "Enhanced Razor Export|*.raz";
            sd.Title = "Export Dress List";
            sd.FileName = "DR." + listname + ".raz";
            sd.RestoreDirectory = true;

            if (sd.ShowDialog() == DialogResult.OK)
            {
                DataSet m_Dataset = new DataSet();
                DataTable dress_items = new DataTable("DRESS_ITEMS");
                dress_items.Columns.Add("List", typeof(string));
                dress_items.Columns.Add("Item", typeof(RazorEnhanced.Dress.DressItem));
                m_Dataset.Tables.Add(dress_items);
                m_Dataset.AcceptChanges();

                List<Dress.DressItem> items;
                RazorEnhanced.Settings.Dress.ItemsRead(listname, out items);

                foreach (RazorEnhanced.Dress.DressItem item in items)
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
                    compress.Dispose();
                    stream.Close();
                    stream.Dispose();
                    Dress.AddLog("List: " + listname + " exported");
                }
                catch (Exception ex)
                {
                    Dress.AddLog("Export list fail");
                    Dress.AddLog(ex.ToString());
                }
            }
            else
            {
                Dress.AddLog("Export list cancelled.");
            }

        }
        ////////////// BUY AGENT END //////////////
    }
}
