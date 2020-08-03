using System;
using System.Windows.Forms;
using System.Globalization;
using System.Collections.Generic;
using System.Drawing;
using Assistant;
using Ultima;

namespace RazorEnhanced.UI
{
	internal partial class EnhancedStaticInspector : Form
	{
		private Assistant.Point3D m_loc;

		internal EnhancedStaticInspector(Assistant.Point3D loc)
		{
			InitializeComponent();
			m_loc = loc;
			MaximizeBox = false;
		}

		private void razorButton1_Click(object sender, EventArgs e)
		{
			this.Close();
		}

		private List<Statics.TileInfo> m_static = new List<Statics.TileInfo>();
		private void EnhancedStaticInspector_Load(object sender, EventArgs e)
		{
			// Land data
			Statics.TileInfo land = Statics.GetStaticsLandInfo(m_loc.X, m_loc.Y, Player.Map);
			lPosition.Text = m_loc.ToString();
			lLandZ.Text = land.StaticZ.ToString();
			lLandHue.Text = land.StaticHue.ToString("X4");
			lLandID.Text = "0x" + land.StaticID.ToString("X4");
			ipLandImg.BackgroundImage = Ultima.Textures.GetTexture(land.StaticID);

			try // Necessary if some fail in Ultima.dll
			{
				// Land Flag
				lLandFlagNone.Text = (TileData.LandTable[land.StaticID].Flags == TileFlag.None) ? "Yes" : "No";
				lLandFlagNone.ForeColor = (TileData.LandTable[land.StaticID].Flags == TileFlag.None) ? Color.Green : Color.Red;

				lLandFlagTranslucent.Text = ((TileData.LandTable[land.StaticID].Flags & TileFlag.Translucent) != 0) ? "Yes" : "No";
				lLandFlagTranslucent.ForeColor = ((TileData.LandTable[land.StaticID].Flags & TileFlag.Translucent) != 0) ? Color.Green : Color.Red;

				lLandFlagWall.Text = ((TileData.LandTable[land.StaticID].Flags & TileFlag.Wall) != 0) ? "Yes" : "No";
				lLandFlagWall.ForeColor = ((TileData.LandTable[land.StaticID].Flags & TileFlag.Wall) != 0) ? Color.Green : Color.Red;

				lLandFlagDamaging.Text = ((TileData.LandTable[land.StaticID].Flags & TileFlag.Damaging) != 0) ? "Yes" : "No";
				lLandFlagDamaging.ForeColor = ((TileData.LandTable[land.StaticID].Flags & TileFlag.Damaging) != 0) ? Color.Green : Color.Red;

				lLandFlagImpassable.Text = ((TileData.LandTable[land.StaticID].Flags & TileFlag.Impassable) != 0) ? "Yes" : "No";
				lLandFlagImpassable.ForeColor = ((TileData.LandTable[land.StaticID].Flags & TileFlag.Impassable) != 0) ? Color.Green : Color.Red;

				lLandFlagSurface.Text = ((TileData.LandTable[land.StaticID].Flags & TileFlag.Surface) != 0) ? "Yes" : "No";
				lLandFlagSurface.ForeColor = ((TileData.LandTable[land.StaticID].Flags & TileFlag.Surface) != 0) ? Color.Green : Color.Red;

				lLandFlagBridge.Text = ((TileData.LandTable[land.StaticID].Flags & TileFlag.Bridge) != 0) ? "Yes" : "No";
				lLandFlagBridge.ForeColor = ((TileData.LandTable[land.StaticID].Flags & TileFlag.Bridge) != 0) ? Color.Green : Color.Red;

				lLandFlagWindow.Text = ((TileData.LandTable[land.StaticID].Flags & TileFlag.Window) != 0) ? "Yes" : "No";
				lLandFlagWindow.ForeColor = ((TileData.LandTable[land.StaticID].Flags & TileFlag.Window) != 0) ? Color.Green : Color.Red;

				lLandFlagNoShoot.Text = ((TileData.LandTable[land.StaticID].Flags & TileFlag.NoShoot) != 0) ? "Yes" : "No";
				lLandFlagNoShoot.ForeColor = ((TileData.LandTable[land.StaticID].Flags & TileFlag.NoShoot) != 0) ? Color.Green : Color.Red;

				lLandFlagFoliage.Text = ((TileData.LandTable[land.StaticID].Flags & TileFlag.Foliage) != 0) ? "Yes" : "No";
				lLandFlagFoliage.ForeColor = ((TileData.LandTable[land.StaticID].Flags & TileFlag.Foliage) != 0) ? Color.Green : Color.Red;

				lLandFlagHoverOver.Text = ((TileData.LandTable[land.StaticID].Flags & TileFlag.HoverOver) != 0) ? "Yes" : "No";
				lLandFlagHoverOver.ForeColor = ((TileData.LandTable[land.StaticID].Flags & TileFlag.HoverOver) != 0) ? Color.Green : Color.Red;

				lLandFlagRoof.Text = ((TileData.LandTable[land.StaticID].Flags & TileFlag.Roof) != 0) ? "Yes" : "No";
				lLandFlagRoof.ForeColor = ((TileData.LandTable[land.StaticID].Flags & TileFlag.Roof) != 0) ? Color.Green : Color.Red;

				lLandFlagDoor.Text = ((TileData.LandTable[land.StaticID].Flags & TileFlag.Door) != 0) ? "Yes" : "No";
				lLandFlagDoor.ForeColor = ((TileData.LandTable[land.StaticID].Flags & TileFlag.Door) != 0) ? Color.Green : Color.Red;

				lLandFlagWet.Text = ((TileData.LandTable[land.StaticID].Flags & TileFlag.Wet) != 0) ? "Yes" : "No";
				lLandFlagWet.ForeColor = ((TileData.LandTable[land.StaticID].Flags & TileFlag.Wet) != 0) ? Color.Green : Color.Red;
			}
			catch { }
			// Static Data
			m_static = Statics.GetStaticsTileInfo(m_loc.X, m_loc.Y, Player.Map);
			if (m_static.Count == 0)
			{
				listBoxStatic.SelectedIndexChanged -= listBoxStatic_SelectedIndexChanged;
				listBoxStatic.Items.Add("No static here!");
				groupBoxStaticDetails.Enabled = groupBoxStaticFlag.Enabled = groupBoxStaticList.Enabled = false;
			}
			else
			{
				foreach (Statics.TileInfo s in m_static)
				{
					listBoxStatic.Items.Add("0x" + s.StaticID.ToString("X4"));
				}
				listBoxStatic.SelectedIndex = 0;
			}

		}

		private void listBoxStatic_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (listBoxStatic.SelectedItem == null)
				return;

			int id = FromHex(listBoxStatic.SelectedItem.ToString());
			foreach (Statics.TileInfo s in m_static)
			{
				if (s.StaticID == id)
				{
					lStaticID.Text = "0x" + s.StaticID.ToString("X4");
					lStaticHue.Text = "0x" + s.StaticHue.ToString("X4");
					lStaticZ.Text = s.StaticZ.ToString();
					// Immagine
					Bitmap staticimage = Ultima.Art.GetStatic(s.StaticID);
					{
						if (staticimage != null && s.StaticHue > 0)
						{
							int hue = s.StaticHue;
							bool onlyHueGrayPixels = (hue & 0x8000) != 0;
							hue = (hue & 0x3FFF) - 1;
							Ultima.Hue m_hue = Ultima.Hues.GetHue(hue);
							m_hue.ApplyTo(staticimage, onlyHueGrayPixels);
						}
						ipStaticImg.BackgroundImage = staticimage;
					}

					// Static Flag
					lStaticFlagNone.Text = (TileData.ItemTable[s.StaticID].Flags == TileFlag.None) ? "Yes" : "No";
					lStaticFlagNone.ForeColor = (TileData.ItemTable[s.StaticID].Flags == TileFlag.None) ? Color.Green : Color.Red;

					lStaticFlagTranslucent.Text = ((TileData.ItemTable[s.StaticID].Flags & TileFlag.Translucent) != 0) ? "Yes" : "No";
					lStaticFlagTranslucent.ForeColor = ((TileData.ItemTable[s.StaticID].Flags & TileFlag.Translucent) != 0) ? Color.Green : Color.Red;

					lStaticFlagWall.Text = ((TileData.ItemTable[s.StaticID].Flags & TileFlag.Wall) != 0) ? "Yes" : "No";
					lStaticFlagWall.ForeColor = ((TileData.ItemTable[s.StaticID].Flags & TileFlag.Wall) != 0) ? Color.Green : Color.Red;

					lStaticFlagDamaging.Text = ((TileData.ItemTable[s.StaticID].Flags & TileFlag.Damaging) != 0) ? "Yes" : "No";
					lStaticFlagDamaging.ForeColor = ((TileData.ItemTable[s.StaticID].Flags & TileFlag.Damaging) != 0) ? Color.Green : Color.Red;

					lStaticFlagImpassable.Text = ((TileData.ItemTable[s.StaticID].Flags & TileFlag.Impassable) != 0) ? "Yes" : "No";
					lStaticFlagImpassable.ForeColor = ((TileData.ItemTable[s.StaticID].Flags & TileFlag.Impassable) != 0) ? Color.Green : Color.Red;

					lStaticFlagSurface.Text = ((TileData.ItemTable[s.StaticID].Flags & TileFlag.Surface) != 0) ? "Yes" : "No";
					lStaticFlagSurface.ForeColor = ((TileData.ItemTable[s.StaticID].Flags & TileFlag.Surface) != 0) ? Color.Green : Color.Red;

					lStaticFlagBridge.Text = ((TileData.ItemTable[s.StaticID].Flags & TileFlag.Bridge) != 0) ? "Yes" : "No";
					lStaticFlagBridge.ForeColor = ((TileData.ItemTable[s.StaticID].Flags & TileFlag.Bridge) != 0) ? Color.Green : Color.Red;

					lStaticFlagWindow.Text = ((TileData.ItemTable[s.StaticID].Flags & TileFlag.Window) != 0) ? "Yes" : "No";
					lStaticFlagWindow.ForeColor = ((TileData.ItemTable[s.StaticID].Flags & TileFlag.Window) != 0) ? Color.Green : Color.Red;

					lStaticFlagNoShot.Text = ((TileData.ItemTable[s.StaticID].Flags & TileFlag.NoShoot) != 0) ? "Yes" : "No";
					lStaticFlagNoShot.ForeColor = ((TileData.ItemTable[s.StaticID].Flags & TileFlag.NoShoot) != 0) ? Color.Green : Color.Red;

					lStaticFlagFoliage.Text = ((TileData.ItemTable[s.StaticID].Flags & TileFlag.Foliage) != 0) ? "Yes" : "No";
					lStaticFlagFoliage.ForeColor = ((TileData.ItemTable[s.StaticID].Flags & TileFlag.Foliage) != 0) ? Color.Green : Color.Red;

					lStaticFlagHoverOver.Text = ((TileData.ItemTable[s.StaticID].Flags & TileFlag.HoverOver) != 0) ? "Yes" : "No";
					lStaticFlagHoverOver.ForeColor = ((TileData.ItemTable[s.StaticID].Flags & TileFlag.HoverOver) != 0) ? Color.Green : Color.Red;

					lStaticFlagRoof.Text = ((TileData.ItemTable[s.StaticID].Flags & TileFlag.Roof) != 0) ? "Yes" : "No";
					lStaticFlagRoof.ForeColor = ((TileData.ItemTable[s.StaticID].Flags & TileFlag.Roof) != 0) ? Color.Green : Color.Red;

					lStaticFlagDoor.Text = ((TileData.ItemTable[s.StaticID].Flags & TileFlag.Door) != 0) ? "Yes" : "No";
					lStaticFlagDoor.ForeColor = ((TileData.ItemTable[s.StaticID].Flags & TileFlag.Door) != 0) ? Color.Green : Color.Red;

					lStaticFlagWet.Text = ((TileData.ItemTable[s.StaticID].Flags & TileFlag.Wet) != 0) ? "Yes" : "No";
					lStaticFlagWet.ForeColor = ((TileData.ItemTable[s.StaticID].Flags & TileFlag.Wet) != 0) ? Color.Green : Color.Red;

					break;
				}
			}
		}
		private int FromHex(string value)
		{
			// strip the leading 0x
			if (value.StartsWith("0x", StringComparison.OrdinalIgnoreCase))
			{
				value = value.Substring(2);
			}
			return Int32.Parse(value, NumberStyles.HexNumber);
		}

		private void bPositionCopy_Click(object sender, EventArgs e)
		{
			Utility.ClipBoardCopy(lPosition.Text);
		}

		private void bLandID_Click(object sender, EventArgs e)
		{
			Utility.ClipBoardCopy(lLandID.Text);
		}

		private void bLandHue_Click(object sender, EventArgs e)
		{
			Utility.ClipBoardCopy(lLandHue.Text);
		}

		private void bLandZ_Click(object sender, EventArgs e)
		{
			Utility.ClipBoardCopy(lLandZ.Text);
		}

		private void bStaticID_Click(object sender, EventArgs e)
		{
			Utility.ClipBoardCopy(lStaticID.Text);
		}

		private void bStaticHue_Click(object sender, EventArgs e)
		{
			Utility.ClipBoardCopy(lStaticHue.Text);
		}

		private void bStaticZ_Click(object sender, EventArgs e)
		{
			Utility.ClipBoardCopy(lStaticZ.Text);
		}
	}
}