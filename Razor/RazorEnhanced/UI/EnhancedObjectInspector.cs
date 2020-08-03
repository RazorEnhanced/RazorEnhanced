using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace RazorEnhanced.UI
{
	public partial class EnhancedObjectInspector : Form
	{
		public EnhancedObjectInspector()
		{
			InitializeComponent();
		}

		private void EnhancedObjectInspector_Load(object sender, EventArgs e)
		{
			RefreshShared();
		}

		private void close_Click(object sender, EventArgs e)
		{
			this.Close();
		}

		private void refreshtimer_Tick(object sender, EventArgs e)
		{
			if (tabControl1.SelectedTab == objecttabPage)
				RefreshShared();
			else
				RefreshTimers();

		}

		private void RefreshShared()
		{
			sharedobjectGridView.Rows.Clear();

			foreach (var o in Misc.SharedScriptData)
			{
				if (o.Value is Item)
					sharedobjectGridView.Rows.Add(new object[] { o.Key, "0x " + ((Item)o.Value).Serial.ToString("X8") });
				if (o.Value is Mobile)
					sharedobjectGridView.Rows.Add(new object[] { o.Key, "0x " + ((Mobile)o.Value).Serial.ToString("X8") });
				else
				{
					if (int.TryParse(o.Value.ToString(), out int n))
						sharedobjectGridView.Rows.Add(new object[] { o.Key, "0x " + n.ToString("X8") });
					else
						sharedobjectGridView.Rows.Add(new object[] { o.Key, o.Value.ToString() });

				}
			}
		}

		private void RefreshTimers()
		{
			timerGridView.Rows.Clear();
			RazorEnhanced.AutoLoot.AddLog("call");

			foreach (var t in Timer.Timers)
			{
				timerGridView.Rows.Add(new object[] { t.Key, "0x " + ((ScriptTimer)t.Value).Interval.ToString() });
			}
		}
	}
}
