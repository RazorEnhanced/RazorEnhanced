using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Xml;
using System.IO;
using System.Linq;
using System.Data;
using Assistant;


namespace RazorEnhanced
{
	public class Macro
	{
        public static void LoadList(ListView a)
        {
            if (File.Exists(Path.Combine(Config.GetUserDirectory(), "enhancedmacro.xml")))
            {
                try
                {
                    DataSet doc = new DataSet();
                    doc.ReadXml(Path.Combine(Config.GetUserDirectory(), "enhancedmacro.xml"));
                    ListViewItem item;
                    foreach (DataRow EM in doc.Tables["List"].Rows)
                    {
                        item = new ListViewItem(new string[] { EM["Hfilename"].ToString(), EM["Hstatus"].ToString() });
                        if (EM["check"].ToString() == "1")
                        {
                            item.Checked = true;
                        }
                        else
                        {
                            item.Checked = false;
                        }
                        a.Items.Add(item);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error to load enhancedmacro.xml: " + ex);
                }
            }
            else
            {
                File.Create(Path.Combine(Config.GetUserDirectory(), "enhancedmacro.xml"));
            }

        }

        public static void SaveList(ListView a)
        {
            XmlWriterSettings settings = new XmlWriterSettings() { Indent = true, IndentChars = "  " };
            try
            {
                using (XmlWriter doc = XmlWriter.Create(Path.Combine(Config.GetUserDirectory(), "enhancedmacro.xml")))
                {
                    doc.WriteStartDocument();
                    doc.WriteStartElement("EnanchedMacro");
                    foreach (ListViewItem one in a.Items)
                    {
                        doc.WriteStartElement("List");
                        doc.WriteElementString("Hfilename", one.SubItems[0].Text);
                        doc.WriteElementString("Hstatus", "Idle");
                        if (one.Checked)
                            doc.WriteElementString("check", "1");
                        else
                            doc.WriteElementString("check", "0");
                        doc.WriteEndElement();
                    }
                    doc.WriteEndElement();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error to write enhancedmacro.xml: " + ex);
            }
        }
	}
}
