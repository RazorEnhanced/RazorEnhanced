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
using PaxScript.Net;
using System.IO;
using Assistant;


namespace RazorEnhanced.UI
{
	public partial class EnhancedScriptEditor : Form
	{
		private const string m_Title = "Enhanced Script Editor";

		private static PaxScripter m_PaxScripterEnhanced;
		private int m_Line = -1;
		private string m_Classname = "";

		public EnhancedScriptEditor()
		{
			InitializeComponent();
			this.Text = m_Title;
			m_PaxScripterEnhanced = new PaxScripter(this.components);
			m_PaxScripterEnhanced.OnChangeState += new ChangeStateHandler(this.paxScripterEnhanced_OnChangeState);
		}

		private void scintillScriptEditor_TextChanged(object sender, EventArgs e)
		{
			m_PaxScripterEnhanced.Reset();
			m_PaxScripterEnhanced.AddModule("1");
			m_PaxScripterEnhanced.AddCode("1", scintillScriptEditor.Text);
		}

		private void paxScripterEnhanced_OnChangeState(PaxScripter sender, ChangeStateEventArgs e)
		{
			if (e.NewState == ScripterState.Error)
			{
				string msg = "SCRIPT ERRORS:" + Environment.NewLine;
				foreach (ScriptError err in m_PaxScripterEnhanced.Error_List)
				{
					msg += "Line: " + err.LineNumber.ToString() + " - Error: " + err.Message + Environment.NewLine;
				}
				MessageBox.Show(msg);
			}
			else if (e.OldState == ScripterState.Init)
			{
				m_PaxScripterEnhanced.AddModule("1");
				m_PaxScripterEnhanced.AddCode("1", scintillScriptEditor.Text);
			}

		}

		private static void InvokeMethod(string classname, string method, RunMode mode)
		{
			try
			{
				m_PaxScripterEnhanced.Invoke(mode, null, "RazorEnhanced." + classname + "." + method);
			}
			catch
			{
			}
		}

		private void toolStripButtonPlay_Click(object sender, EventArgs e)
		{
			InvokeMethod(m_Classname, "Run", RunMode.Run);

			if (m_PaxScripterEnhanced.HasErrors)
			{
				m_PaxScripterEnhanced.Reset();
				toolStripStatusLabelScript.Text = "";
			}
			else
			{
				toolStripStatusLabelScript.Text = "RunMode: Run";
			}
		}

		private void toolStripButtonNextLine_Click(object sender, EventArgs e)
		{
			InvokeMethod(m_Classname, "Run", RunMode.NextLine);

			int line = m_PaxScripterEnhanced.CurrentLineNumber / 2;
			scintillScriptEditor.Caret.HighlightCurrentLine = true;
			scintillScriptEditor.Caret.LineNumber = line;

			toolStripStatusLabelScript.Text = "RunMode: Next Line";
		}

		private void toolStripButtonStepOver_Click(object sender, EventArgs e)
		{
			InvokeMethod(m_Classname, "Run", RunMode.StepOver);

			int line = m_PaxScripterEnhanced.CurrentLineNumber / 2;
			scintillScriptEditor.Caret.HighlightCurrentLine = true;
			scintillScriptEditor.Caret.LineNumber = line;

			toolStripStatusLabelScript.Text = "RunMode: Step Over";
		}

		private void toolStripTraceInto_Click(object sender, EventArgs e)
		{
			InvokeMethod(m_Classname, "Run", RunMode.TraceInto);

			int line = m_PaxScripterEnhanced.CurrentLineNumber / 2;
			scintillScriptEditor.Caret.HighlightCurrentLine = true;
			scintillScriptEditor.Caret.LineNumber = line;

			toolStripStatusLabelScript.Text = "RunMode: Trace Into";
		}

		private void toolStripButtonStop_Click(object sender, EventArgs e)
		{
			m_PaxScripterEnhanced.Reset();
			scintillScriptEditor.Caret.HighlightCurrentLine = false;
			toolStripStatusLabelScript.Text = "";
		}

		private void toolStripTextBoxEvaluate_TextChanged(object sender, EventArgs e)
		{
			if (toolStripTextBoxEvaluate.Text != null && toolStripTextBoxEvaluate.Text != "")
			{
				string msg = "EXPRESSION: " + toolStripTextBoxEvaluate.Text + " - EVALUATE: ";
				try
				{
					toolStripStatusLabelScript.Text = msg + m_PaxScripterEnhanced.Eval(toolStripTextBoxEvaluate.Text).ToString();
				}
				catch (Exception ex)
				{
					toolStripStatusLabelScript.Text = msg + ex.Message;
				}
			}
		}

		private void toolStripButtonAddBreakpoint_Click(object sender, EventArgs e)
		{
			m_PaxScripterEnhanced.RemoveAllBreakpoints();

			int line = scintillScriptEditor.Caret.LineNumber;

			Marker lineHighlighter = scintillScriptEditor.Markers[1];
			lineHighlighter.BackColor = System.Drawing.Color.Red;
			lineHighlighter.Symbol = MarkerSymbol.Background;
			scintillScriptEditor.Lines[line].AddMarker(lineHighlighter);
			m_Line = line;

			m_PaxScripterEnhanced.AddBreakpoint("1", line);
		}

		private void toolStripButtonRemoveBreakpoints_Click(object sender, EventArgs e)
		{
			m_PaxScripterEnhanced.RemoveAllBreakpoints();
			scintillScriptEditor.Lines[m_Line].DeleteAllMarkers();
			m_Line = -1;
		}

		private void toolStripButtonOpen_Click(object sender, EventArgs e)
		{
			OpenFileDialog open = new OpenFileDialog();
			open.Filter = "Script Files|*.cs";

			if (open.ShowDialog() == DialogResult.OK)
			{
				scintillScriptEditor.Text = System.IO.File.ReadAllText(open.FileName);
				m_Classname = Path.GetFileNameWithoutExtension(open.FileName);
				this.Text = m_Title + " " + m_Classname + ".cs";
			}
		}

		private void toolStripButtonSave_Click(object sender, EventArgs e)
		{
			SaveFileDialog save = new SaveFileDialog();
			save.Filter = "Script Files|*.cs";

			if (save.ShowDialog() == DialogResult.OK)
			{
				System.IO.File.WriteAllText(save.FileName, scintillScriptEditor.Text);
				m_Classname = Path.GetFileNameWithoutExtension(save.FileName);
				this.Text = m_Title + " " + m_Classname + ".cs";
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
					System.IO.File.WriteAllText(save.FileName, scintillScriptEditor.Text);
				}
			}

			scintillScriptEditor.Text = "";
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
	}
}
