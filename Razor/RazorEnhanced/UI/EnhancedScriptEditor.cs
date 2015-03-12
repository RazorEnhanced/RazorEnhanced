using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ScintillaNET;
using System.IO;
using Assistant;
using RazorEnhanced;

namespace RazorEnhanced.UI
{
	public partial class EnhancedScriptEditor : Form
	{
		private const string m_Title = "Enhanced Script Editor";

		private int m_Line = -1;
		private string m_Classname = "";

		public EnhancedScriptEditor()
		{
			InitializeComponent();
			this.Text = m_Title;

		}

		private void scintillScriptEditor_TextChanged(object sender, EventArgs e)
		{
		}

		private void toolStripButtonPlay_Click(object sender, EventArgs e)
		{
			toolStripStatusLabelScript.Text = "RunMode: Run";
		}

		private void toolStripButtonNextLine_Click(object sender, EventArgs e)
		{
			scintillaScriptEditor.Caret.HighlightCurrentLine = true;
			toolStripStatusLabelScript.Text = "RunMode: Next Line";
		}

		private void toolStripButtonStepOver_Click(object sender, EventArgs e)
		{
			scintillaScriptEditor.Caret.HighlightCurrentLine = true;

			toolStripStatusLabelScript.Text = "RunMode: Step Over";
		}

		private void toolStripTraceInto_Click(object sender, EventArgs e)
		{

			scintillaScriptEditor.Caret.HighlightCurrentLine = true;
			toolStripStatusLabelScript.Text = "RunMode: Trace Into";
		}

		private void toolStripButtonStop_Click(object sender, EventArgs e)
		{
			scintillaScriptEditor.Caret.HighlightCurrentLine = false;

			toolStripStatusLabelScript.Text = "";
		}

		private void toolStripTextBoxEvaluate_TextChanged(object sender, EventArgs e)
		{
			if (toolStripTextBoxEvaluate.Text != null && toolStripTextBoxEvaluate.Text != "")
			{
				string msg = "EXPRESSION: " + toolStripTextBoxEvaluate.Text + " - EVALUATE: ";
				try
				{
				}
				catch (Exception ex)
				{
					toolStripStatusLabelScript.Text = msg + ex.Message;
				}
			}
		}

		private void toolStripButtonAddBreakpoint_Click(object sender, EventArgs e)
		{
			int line = scintillaScriptEditor.Caret.LineNumber;

			Marker lineHighlighter = scintillaScriptEditor.Markers[1];
			lineHighlighter.BackColor = System.Drawing.Color.Red;
			lineHighlighter.Symbol = MarkerSymbol.Background;
			scintillaScriptEditor.Lines[line].AddMarker(lineHighlighter);
			m_Line = line;
		}

		private void toolStripButtonRemoveBreakpoints_Click(object sender, EventArgs e)
		{
			scintillaScriptEditor.Lines[m_Line].DeleteAllMarkers();
			m_Line = -1;
		}

		private void toolStripButtonOpen_Click(object sender, EventArgs e)
		{
			OpenFileDialog open = new OpenFileDialog();
			open.Filter = "Script Files|*.cs";

			if (open.ShowDialog() == DialogResult.OK)
			{
				m_Classname = Path.GetFileNameWithoutExtension(open.FileName);
				this.Text = m_Title + " - " + m_Classname + ".cs";
				scintillaScriptEditor.Text = System.IO.File.ReadAllText(open.FileName);
			}
		}

		private void toolStripButtonSave_Click(object sender, EventArgs e)
		{
			SaveFileDialog save = new SaveFileDialog();
			save.Filter = "Script Files|*.cs";

			if (save.ShowDialog() == DialogResult.OK)
			{
				m_Classname = Path.GetFileNameWithoutExtension(save.FileName);
				this.Text = m_Title + " - " + m_Classname + ".cs";
				System.IO.File.WriteAllText(save.FileName, scintillaScriptEditor.Text);
			}
		}

		private void toolStripButtonClose_Click(object sender, EventArgs e)
		{
			if (MessageBox.Show("Save current file?", "WARNING", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == System.Windows.Forms.DialogResult.Yes)
			{
				SaveFileDialog save = new SaveFileDialog();
				save.Filter = "Script Files|*.cs";

				if (save.ShowDialog() == DialogResult.OK)
				{
					System.IO.File.WriteAllText(save.FileName, scintillaScriptEditor.Text);
				}
			}

			scintillaScriptEditor.Text = "";
			m_Classname = "";
			this.Text = m_Title;
		}

		private void toolStripButtonInspect_Click(object sender, EventArgs e)
		{
			Targeting.OneTimeTarget(new Targeting.TargetResponseCallback(InspectItemTarget_Callback));
		}

		private void InspectItemTarget_Callback(bool loc, Assistant.Serial serial, Assistant.Point3D pt, ushort itemid)
		{

			Assistant.Item assistantItem = Assistant.World.FindItem(serial);
			if (assistantItem != null && assistantItem.Serial.IsItem)
			{
				EnhancedItemInspector inspector = new EnhancedItemInspector(assistantItem);
				inspector.TopMost = true;
				inspector.Show();
			}

			else
			{
				Assistant.Mobile assistantMobile = Assistant.World.FindMobile(serial);
				if (assistantMobile != null && assistantMobile.Serial.IsMobile)
				{
					EnhancedMobileInspector inspector = new EnhancedMobileInspector(assistantMobile);
					inspector.TopMost = true;
					inspector.Show();
				}
			}
		}

		private void EnhancedScriptEditor_Load(object sender, EventArgs e)
		{
			scintillaScriptEditor.Margins[0].Width = 20;
		}
	}
}
