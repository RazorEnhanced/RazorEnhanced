using RazorEnhanced;
using System;
using System.Windows.Forms;

namespace Assistant
{
	public partial class MainForm : System.Windows.Forms.Form
	{
		internal Button DPSMeterStartB { get { return DPSMeterStartButton; } }
		internal Button DPSMeterPauseB { get { return DPSMeterPauseButton; } }
		internal Button DPSMeterStopB { get { return DPSMeterStopButton; } }

		private void DPSMeterStartButton_Click(object sender, EventArgs e)
		{
			DpsMeterGridView.Rows.Clear();
			DPSMeter.Clear();
			DPSMeter.Enabled = DPSMeterStopButton.Enabled = DPSMeterPauseButton.Enabled = true;
			DPSMeterStartButton.Enabled = DpsMeterGridView.Enabled = false;
			DPSMeterStatusLabel.Text = "Collecting Data...";
			Misc.SendMessage("DPS METER: Collecting Data...", false);
		}

		private void DPSMeterStopButton_Click(object sender, EventArgs e)
		{
			DPSMeterStartButton.Enabled = DpsMeterGridView.Enabled = true;
			DPSMeterStopButton.Enabled = DPSMeter.Enabled = DPSMeterPauseButton.Enabled = false;
			DPSMeterStatusLabel.Text = "Idle";
			DPSMeter.ShowResult(DpsMeterGridView);
			Misc.SendMessage("DPS METER: Stop.", false);
		}

		private void DPSMeterClearButton_Click(object sender, EventArgs e)
		{
			DpsMeterGridView.Rows.Clear();
			DPSMeter.Clear();
		}

		private void DPSMeterPauseButton_Click(object sender, EventArgs e)
		{
			if (DPSMeter.Enabled)
			{
				DPSMeterStatusLabel.Text = "Pause";
				DPSMeter.Enabled = false;
				DPSMeterPauseButton.Text = "Resume";
				DPSMeter.ShowResult(DpsMeterGridView);
				DpsMeterGridView.Enabled = true;
				Misc.SendMessage("DPS METER: Pause.", false);
			}
			else
			{
				DpsMeterGridView.Rows.Clear();
				DPSMeterPauseButton.Text = "Pause";
				DPSMeterStatusLabel.Text = "Collecting Data...";
				DPSMeter.Enabled = true;
				DpsMeterGridView.Enabled = false;
				Misc.SendMessage("DPS METER: Collecting Data...", false);
			}
		}

		private void DPSMeterApplyFilterButton_Click(object sender, EventArgs e)
		{
			if (DPSMeter.Enabled)
				return;

			int max = -1;
			if (DPSmetermaxdamage.Text != string.Empty)
				max = Convert.ToInt32(DPSmetermaxdamage.Text);

			int min = -1;
			if (DPSmetermindamage.Text != string.Empty)
				min = Convert.ToInt32(DPSmetermindamage.Text);

			int serial = -1;
			if (DPSmeterserial.Text != string.Empty)
			{
				try
				{
					serial = Convert.ToInt32(DPSmeterserial.Text, 16);
				}
				catch { }
			}

			string name = null;
			if (DPSmetername.Text != string.Empty)
				name = DPSmetername.Text;

			DPSMeter.ShowResult(DpsMeterGridView, max, min, serial, name);
		}

		private void DPSMeterClearFilterButton_Click(object sender, EventArgs e)
		{
			DPSmetermaxdamage.Text = DPSmetermindamage.Text = DPSmeterserial.Text = DPSmetername.Text = string.Empty;
			DPSMeter.ShowResult(DpsMeterGridView, -1, -1, -1, null);
		}
	}
}
