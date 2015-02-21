using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using Assistant;
using System.IO;

namespace RazorEnhanced
{
	internal class Settings
	{
		internal static void Load()
		{
			if (File.Exists(Path.Combine(Directory.GetCurrentDirectory(), "RazorEnhanced.xml")))
			{
				try
				{
					DataSet doc = new DataSet();
					doc.ReadXml(Path.Combine(Directory.GetCurrentDirectory(), "RazorEnhanced.xml"));

					Engine.MainWindow.dataGridViewMacroNew.Rows.Clear();
					foreach (DataRow row in doc.Tables["macro"].Rows)
					{
						Engine.MainWindow.dataGridViewMacroNew.Rows.Add((bool)row["checked"], (bool)row["checked"], Assistant.Properties.Resources.yellow, "Idle");

					}
				}
				catch (Exception ex)
				{
					MessageBox.Show("Error loading RazorEnhanced.xml: " + ex);
				}
			}
		}

		internal static void Save()
		{
			try
			{
				DataSet doc = new DataSet();

				DataTable macro = new DataTable("macro");
				macro.Columns.Add("filename", typeof(string));
				macro.Columns.Add("checked", typeof(bool));
				foreach (DataGridViewRow data in Engine.MainWindow.dataGridViewMacroNew.Rows)
				{
					DataRow row = macro.NewRow();
					row["checked"] = data.Cells[0].Value;
					row["filename"] = data.Cells[1].Value;
					macro.Rows.Add(row);
				}
				doc.Tables.Add(macro);

				doc.WriteXml(Path.Combine(Directory.GetCurrentDirectory(), "RazorEnhanced.xml"));
			}
			catch (Exception ex)
			{
				MessageBox.Show("Error writing RazorEnhanced.xml: " + ex);
			}
		}
	}
}