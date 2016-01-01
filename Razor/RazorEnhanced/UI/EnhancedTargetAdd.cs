using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace RazorEnhanced.UI
{
	public partial class EnhancedTargetAdd : Form
	{
		private const string m_Title = "Enhanced Target Add";

		public EnhancedTargetAdd()
		{
			InitializeComponent();
			MaximizeBox = false;

			this.Text = m_Title; ;
		}

		private void EnhancedrgetAdd_Load(object sender, EventArgs e)
		{
			tAddBody.Text = "0x0000";
			tAddHue.Text = "0x0000";
			tRangeMax.Text = "-1";
			tRangeMin.Text = "-1";

			SelectorComboBox.Items.Add("Random");
			SelectorComboBox.Items.Add("Nearest");
			SelectorComboBox.Items.Add("Farthest");
			SelectorComboBox.Items.Add("Weakest");
			SelectorComboBox.Items.Add("Strongest");
			SelectorComboBox.Text = "Random";

			notocolorComboBox.Items.Add("Innocent");
			notocolorComboBox.Items.Add("Ally");
			notocolorComboBox.Items.Add("Can be attacked");
			notocolorComboBox.Items.Add("Criminal");
			notocolorComboBox.Items.Add("Enemy");
			notocolorComboBox.Items.Add("Murderer");
			notocolorComboBox.Items.Add("Invulnerable");
			notocolorComboBox.Text = "Criminal";
		}

		private void bodyaddButton_Click(object sender, EventArgs e)
		{
			int body = 0;
			try
			{
				body = Convert.ToInt32(tAddBody.Text, 16);
				if (body > 0)
				{
					if (!bodylistBox.Items.Contains("0x" + body.ToString("X4")))
					{
						bodylistBox.Items.Add("0x" + body.ToString("X4"));
					}
				}
			}
			catch
			{
				MessageBox.Show("Body ID is not valid.",
				"Body ID is not valid.",
				MessageBoxButtons.OK,
				MessageBoxIcon.Exclamation,
				MessageBoxDefaultButton.Button1);
				tAddBody.Text = "0x0000";
			}
		}

		private void hueaddButton_Click(object sender, EventArgs e)
		{
			int hue = 0;
			try
			{
				hue = Convert.ToInt32(tAddHue.Text, 16);
				if (!huelistBox.Items.Contains("0x" + hue.ToString("X4")))
				{
					huelistBox.Items.Add("0x" + hue.ToString("X4"));
				}
			}
			catch
			{
				MessageBox.Show("Hue number is not valid.",
				"Hue number is not valid.",
				MessageBoxButtons.OK,
				MessageBoxIcon.Exclamation,
				MessageBoxDefaultButton.Button1);
				tAddHue.Text = "0x0000";
			}
		}

		private void notoaddButton_Click(object sender, EventArgs e)
		{
			if (notocolorComboBox.Text != "")
			{
				if (!notolistBox.Items.Contains(notocolorComboBox.Text))
				{
					notolistBox.Items.Add(notocolorComboBox.Text);
				}
			}
		}

		private void bodyremoveButton_Click(object sender, EventArgs e)
		{
			if (bodylistBox.SelectedIndex >= 0)
				bodylistBox.Items.RemoveAt(bodylistBox.SelectedIndex);
		}

		private void hueremoveButton_Click(object sender, EventArgs e)
		{
			if (huelistBox.SelectedIndex >= 0)
				huelistBox.Items.RemoveAt(huelistBox.SelectedIndex);
		}

		private void notoremoveButton_Click(object sender, EventArgs e)
		{
			if (notolistBox.SelectedIndex >= 0)
				notolistBox.Items.RemoveAt(notolistBox.SelectedIndex);
		}

		private void razorButton2_Click(object sender, EventArgs e)
		{
			bool fail = false;
			string newtargetid = "";

			if (tTargetID.Text == "")
				fail = true;

			if (!Regex.IsMatch(tTargetID.Text, "^[a-zA-Z0-9_]+$"))
				fail = true;

			newtargetid = tTargetID.Text.ToLower();
			if (RazorEnhanced.Settings.Target.TargetExist(newtargetid))
				fail = true;

			if (fail)
			{
				MessageBox.Show("Invalid Target ID!",
				"Invalid Target ID!",
				MessageBoxButtons.OK,
				MessageBoxIcon.Exclamation,
				MessageBoxDefaultButton.Button1);
			}
			else
			{
				List<int> bodylist = new List<int>();
				List<int> huelist = new List<int>();
				List<byte> notolist = new List<byte>();
				int maxrange = -1;
				int minrange = -1;

				// body list
				foreach (var listBoxItem in bodylistBox.Items)
				{
					bodylist.Add(Convert.ToInt32(listBoxItem.ToString(), 16));
				}

				// hue list
				foreach (var listBoxItem in huelistBox.Items)
				{
					huelist.Add(Convert.ToInt32(listBoxItem.ToString(), 16));
				}

				// max range
				Int32.TryParse(tRangeMax.Text, out maxrange);
				if (maxrange < -1)
					maxrange = -1;

				// min range
				Int32.TryParse(tRangeMin.Text, out minrange);
				if (minrange < -1)
					minrange = -1;

				// notocolor
				foreach (var listBoxItem in notolistBox.Items)
				{
					notolist.Add(RazorEnhanced.TargetGUI.GetNotoByte(listBoxItem.ToString()));
				}

				// Genero filtro da salvare
				Mobiles.Filter filtertosave = new Mobiles.Filter();
				filtertosave.Enabled = true;
				filtertosave.Bodies = bodylist;
				filtertosave.Name = tName.Text;
				filtertosave.Hues = huelist;
				filtertosave.RangeMax = maxrange;
				filtertosave.RangeMin = minrange;

				if (poisonedBoth.Checked)
					filtertosave.Poisoned = -1;
				else if (poisonedOn.Checked)
					filtertosave.Poisoned = 1;
				else
					filtertosave.Poisoned = 0;

				if (blessedBoth.Checked)
					filtertosave.Blessed = -1;
				else if (blessedOn.Checked)
					filtertosave.Blessed = 1;
				else
					filtertosave.Blessed = 0;

				if (humanBoth.Checked)
					filtertosave.IsHuman = -1;
				else if (humanOn.Checked)
					filtertosave.IsHuman = 1;
				else
					filtertosave.IsHuman = 0;

				if (ghostBoth.Checked)
					filtertosave.IsGhost = -1;
				else if (ghostOn.Checked)
					filtertosave.IsGhost = 1;
				else
					filtertosave.IsGhost = 0;

				if (femaleBoth.Checked)
					filtertosave.Female = -1;
				else if (femaleOn.Checked)
					filtertosave.Female = 1;
				else
					filtertosave.Female = 0;

				if (warmodeBoth.Checked)
					filtertosave.Warmode = -1;
				else if (warmodeOn.Checked)
					filtertosave.Warmode = 1;
				else
					filtertosave.Warmode = 0;

				if (friendBoth.Checked)
					filtertosave.Friend = -1;
				else if (friendOn.Checked)
					filtertosave.Friend = 1;
				else
					filtertosave.Friend = 0;

				if (paralizedBoth.Checked)
					filtertosave.Paralized = -1;
				else if (paralizedOn.Checked)
					filtertosave.Paralized = 1;
				else
					filtertosave.Paralized = 0;

				filtertosave.Notorieties = notolist;

				// Genero struttura da salvare
				TargetGUI.TargetGUIObject targetguitosave = new TargetGUI.TargetGUIObject(SelectorComboBox.Text, filtertosave);

				// Salvo struttura
				RazorEnhanced.Settings.Target.TargetSave(newtargetid, targetguitosave, Keys.None, true);
				RazorEnhanced.TargetGUI.RefreshTarget();
				RazorEnhanced.HotKey.Init();

				this.Close();
			}
		}

		private void razorButton1_Click(object sender, EventArgs e)
		{
			this.Close();
		}
	}
}