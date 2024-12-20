using System;
using System.Windows.Forms;

namespace RazorEnhanced.UI
{
    public partial class EnhancedDressAddUndressLayer : Form
    {
        private const string m_Title = "Enhanced Dress Add Clear Layer";

        public EnhancedDressAddUndressLayer()
        {
            InitializeComponent();
            MaximizeBox = false;
            Text = m_Title;
        }

        private void bClose_Click(object sender, EventArgs e)
        {
            RazorEnhanced.Dress.InitGrid();
            Close();
        }

        private void bAddItem_Click(object sender, EventArgs e)
        {
            if (layerlist.Text != String.Empty)
            {
                Dress.DressItemNew toinsert = new("UNDRESS", (Assistant.Layer)layerlist.SelectedItem, 0, true);
                Settings.Dress.ItemInsertByLayer(Assistant.Engine.MainWindow.DressListSelect.Text, toinsert);
                Dress.InitGrid();
                Close();
            }
        }

        private void EnhancedDressAddUndressLayer_Load(object sender, EventArgs e)
        {
            layerlist.DataSource = Dress.LayerList;
        }
    }
}