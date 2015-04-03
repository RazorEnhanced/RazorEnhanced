using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.IO;


namespace RazorEnhanced.UI
{
	public partial class EnhancedBuyAgentAddList : Form
	{
		private const string m_Title = "Enhanced Buy Add Item List";

		public EnhancedBuyAgentAddList()
		{
			InitializeComponent();
			MaximizeBox = false;

			this.Text = m_Title;
		}

		private void buycloseItemList_Click(object sender, EventArgs e)
		{
			this.Close();
		}

		private void buyaddItemList_Click(object sender, EventArgs e)
		{
			bool fail = false;
			string newList = "";

			if (buyagentListToAdd.Text == "")
				fail = true;

			if (!Regex.IsMatch(buyagentListToAdd.Text, "^[a-zA-Z0-9_]+$"))
				fail = true;

			newList = buyagentListToAdd.Text.ToLower();
			for (int i = 0; i < Assistant.Engine.MainWindow.BuyListSelect.Items.Count; i++)
			{
				if (newList == Assistant.Engine.MainWindow.BuyListSelect.GetItemText(Assistant.Engine.MainWindow.BuyListSelect.Items[i]))
					fail = true;
			}

			if (fail)
			{
				MessageBox.Show("Invalid list name!",
				"Invalid list name!",
				MessageBoxButtons.OK,
				MessageBoxIcon.Exclamation,
				MessageBoxDefaultButton.Button1);
				fail = true;
			}
			else
			{
				BuyAgent.AddList(newList);
				this.Close();
			}
		}

		private void EnhancedSellAgentAddItemList_Load(object sender, EventArgs e)
		{
		}
	}
}
