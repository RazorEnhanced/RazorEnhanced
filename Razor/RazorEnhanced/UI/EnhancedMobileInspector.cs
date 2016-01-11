using System;
using System.Windows.Forms;
using System.Threading;

namespace RazorEnhanced.UI
{
	internal partial class EnhancedMobileInspector : Form
	{
		private const string m_Title = "Enhanced Mobile Inspect";
		private Thread m_ProcessInfo;
		private Assistant.Mobile m_mobile;
		private Form m_inspectform;

		internal EnhancedMobileInspector(Assistant.Mobile mobileTarg)
		{
			InitializeComponent();
			m_mobile = mobileTarg;
			MaximizeBox = false;
		}

		private void ProcessInfoThread()
		{
			int attrib = 0;

			attrib = GetAttribute("Fire Resist");
            if (attrib > 0)
				AddAttributesToList("Fire Resist: " + attrib);

			attrib = GetAttribute("Cold Resist");
			if (attrib > 0)
				AddAttributesToList("Cold Resist: " + attrib);

			attrib = GetAttribute("Poison Resist");
			if (attrib > 0)
				AddAttributesToList("Poison Resist: " + attrib);

			attrib = GetAttribute("Energy Resist");
			if (attrib > 0)
				AddAttributesToList("Energy Resist: " + attrib);

			attrib = GetAttribute("Physical Resist");
			if (attrib > 0)
				AddAttributesToList("Physical Resist: " + attrib);

			attrib = GetAttribute("Swing Speed Increase");
			if (attrib > 0)
				AddAttributesToList("Luck: " + attrib);

			attrib = GetAttribute("Swing Speed Increase");
			if (attrib > 0)
				AddAttributesToList("Swing Speed Increase: " + attrib);

			attrib = GetAttribute("Damage Chance Increase");
			if (attrib > 0)
				AddAttributesToList("Damage Chance Increase: " + attrib);

			attrib = GetAttribute("Damage Increase");
			if (attrib > 0)
				AddAttributesToList("Damage Increase: " + attrib);

			attrib = GetAttribute("Hit Fireball");
			if (attrib > 0)
				AddAttributesToList("Hit Fireball: " + attrib);

			attrib = GetAttribute("Hit Chance Increase");
			if (attrib > 0)
				AddAttributesToList("Hit Chance Increase: " + attrib);

			attrib = GetAttribute("Mage Armor");
			if (attrib > 0)
				AddAttributesToList("Mage Armor: " + attrib);

			attrib = GetAttribute("Lower Reagent Cost");
			if (attrib > 0)
				AddAttributesToList("Lower Reagent Cost: " + attrib);

			attrib = GetAttribute("Hit Point Increase");
			if (attrib > 0)
				AddAttributesToList("Hit Point Increase: " + attrib);

			attrib = GetAttribute("Hit Points Regeneration");
			if (attrib > 0)
				AddAttributesToList("Hit Points Regeneration: " + attrib);

			attrib = GetAttribute("Stamina Regeneration");
			if (attrib > 0)
				AddAttributesToList("Stamina Regeneration: " + attrib);

			attrib = GetAttribute("Mana Regeneration");
			if (attrib > 0)
				AddAttributesToList("Mana Regeneration: " + attrib);

			attrib = GetAttribute("Reflect Physical Damage");
			if (attrib > 0)
				AddAttributesToList("Reflect Physical Damage: " + attrib);

			attrib = GetAttribute("Enhance Potions");
			if (attrib > 0)
				AddAttributesToList("Enhance Potions: " + attrib);

			attrib = GetAttribute("Defense Chance Increase");
			if (attrib > 0)
				AddAttributesToList("Defense Chance Increase: " + attrib);

			attrib = GetAttribute("Spell Damage Increase");
			if (attrib > 0)
				AddAttributesToList("Spell Damage Increase: " + attrib);

			attrib = GetAttribute("Faster Cast Recovery");
			if (attrib > 0)
				AddAttributesToList("Faster Cast Recovery: " + attrib);

			attrib = GetAttribute("Faster Casting");
			if (attrib > 0)
				AddAttributesToList("Faster Casting: " + attrib);

			attrib = GetAttribute("Lower Mana Cost");
			if (attrib > 0)
				AddAttributesToList("Lower Mana Cost: " + attrib);

			attrib = GetAttribute("Strength Increase");
			if (attrib > 0)
				AddAttributesToList("Strength Increase: " + attrib);

			attrib = GetAttribute("Dexterity Increase");
			if (attrib > 0)
				AddAttributesToList("Dexterity Increase: " + attrib);

			attrib = GetAttribute("Dexterity Bonus");
			if (attrib > 0)
				AddAttributesToList("Dexterity Bonus: " + attrib);

			attrib = GetAttribute("Intelligence Bonus");
			if (attrib > 0)
				AddAttributesToList("Intelligence Bonus: " + attrib);

			attrib = GetAttribute("Strength Bonus");
			if (attrib > 0)
				AddAttributesToList("Strength Bonus: " + attrib);

			attrib = GetAttribute("Intelligence Increase");
			if (attrib > 0)
				AddAttributesToList("Intelligence Increase: " + attrib);

			attrib = GetAttribute("Hit Points Increase");
			if (attrib > 0)
				AddAttributesToList("Hit Points Increase: " + attrib);

			attrib = GetAttribute("Stamina Increase");
			if (attrib > 0)
				AddAttributesToList("Stamina Increase: " + attrib);

			attrib = GetAttribute("Mana Increase");
			if (attrib > 0)
				AddAttributesToList("Mana Increase: " + attrib);

			attrib = GetAttribute("Maximum Hit Points Increase");
			if (attrib > 0)
				AddAttributesToList("Maximum Hit PointsIncrease: " + attrib);

			attrib = GetAttribute("Maximum Stamina Increase");
			if (attrib > 0)
				AddAttributesToList("Maximum Stamina Increase: " + attrib);

			attrib = GetAttribute("Maximum Mana Increase");
			if (attrib > 0)
				AddAttributesToList("Maximum Mana Increase: " + attrib);

			attrib = GetAttribute("Self Repair");
			if (attrib > 0)
				AddAttributesToList("Self Repair: " + attrib);

			attrib = GetAttribute("Insured");
			if (attrib > 0)
				AddAttributesToList("Insured: " + attrib);
		}

		private void AddAttributesToList(string value)
		{
			try
			{
				if (Assistant.Engine.Running)
				{
					listBoxAttributes.Invoke(new Action(() => listBoxAttributes.Items.Add(value)));
				}
			}
			catch (Exception ex)
			{ MessageBox.Show(ex.ToString()); }


		}

		private int GetAttribute(string attributename)
		{
			int attributevalue = 0;

			Assistant.Item itemtocheck = m_mobile.GetItemOnLayer(Assistant.Layer.Arms);
			if (itemtocheck != null)
			{
				if (!itemtocheck.PropsUpdated)
				{
					RazorEnhanced.Items.WaitForProps(itemtocheck.Serial, 1000);
					Thread.Sleep(50);
                }
				attributevalue = attributevalue + RazorEnhanced.Items.GetPropValue(itemtocheck.Serial, attributename);
            }

			itemtocheck = m_mobile.GetItemOnLayer(Assistant.Layer.Bracelet);
			if (itemtocheck != null)
			{
				if (!itemtocheck.PropsUpdated)
				{
					RazorEnhanced.Items.WaitForProps(itemtocheck.Serial, 1000);
					Thread.Sleep(50);
				}
				attributevalue = attributevalue + RazorEnhanced.Items.GetPropValue(itemtocheck.Serial, attributename);
			}

			itemtocheck = m_mobile.GetItemOnLayer(Assistant.Layer.Cloak);
			if (itemtocheck != null)
			{
				if (!itemtocheck.PropsUpdated)
				{
					RazorEnhanced.Items.WaitForProps(itemtocheck.Serial, 1000);
					Thread.Sleep(50);
				}
				attributevalue = attributevalue + RazorEnhanced.Items.GetPropValue(itemtocheck.Serial, attributename);
			}

			itemtocheck = m_mobile.GetItemOnLayer(Assistant.Layer.Earrings);
			if (itemtocheck != null)
			{
				if (!itemtocheck.PropsUpdated)
				{
					RazorEnhanced.Items.WaitForProps(itemtocheck.Serial, 1000);
					Thread.Sleep(50);
				}
				attributevalue = attributevalue + RazorEnhanced.Items.GetPropValue(itemtocheck.Serial, attributename);
			}

			itemtocheck = m_mobile.GetItemOnLayer(Assistant.Layer.Gloves);
			if (itemtocheck != null)
			{
				if (!itemtocheck.PropsUpdated)
				{
					RazorEnhanced.Items.WaitForProps(itemtocheck.Serial, 1000);
					Thread.Sleep(50);
				}
				attributevalue = attributevalue + RazorEnhanced.Items.GetPropValue(itemtocheck.Serial, attributename);

			}

			itemtocheck = m_mobile.GetItemOnLayer(Assistant.Layer.Head);
			if (itemtocheck != null)
			{
				if (!itemtocheck.PropsUpdated)
				{
					RazorEnhanced.Items.WaitForProps(itemtocheck.Serial, 1000);
				}
				attributevalue = attributevalue + RazorEnhanced.Items.GetPropValue(itemtocheck.Serial, attributename);
			}

			itemtocheck = m_mobile.GetItemOnLayer(Assistant.Layer.InnerLegs);
			if (itemtocheck != null)
			{
				if (!itemtocheck.PropsUpdated)
				{
					RazorEnhanced.Items.WaitForProps(itemtocheck.Serial, 1000);
					Thread.Sleep(50);
				}
				attributevalue = attributevalue + RazorEnhanced.Items.GetPropValue(itemtocheck.Serial, attributename);
			}

			itemtocheck = m_mobile.GetItemOnLayer(Assistant.Layer.InnerTorso);
			if (itemtocheck != null)
			{
				if (!itemtocheck.PropsUpdated)
				{
					RazorEnhanced.Items.WaitForProps(itemtocheck.Serial, 1000);
					Thread.Sleep(50);
				}
				attributevalue = attributevalue + RazorEnhanced.Items.GetPropValue(itemtocheck.Serial, attributename);
			}

			itemtocheck = m_mobile.GetItemOnLayer(Assistant.Layer.LeftHand);
			if (itemtocheck != null)
			{
				if (!itemtocheck.PropsUpdated)
				{
					RazorEnhanced.Items.WaitForProps(itemtocheck.Serial, 1000);
					Thread.Sleep(50);
				}
				attributevalue = attributevalue + RazorEnhanced.Items.GetPropValue(itemtocheck.Serial, attributename);
			}

			itemtocheck = m_mobile.GetItemOnLayer(Assistant.Layer.MiddleTorso);
			if (itemtocheck != null)
			{
				if (!itemtocheck.PropsUpdated)
				{
					RazorEnhanced.Items.WaitForProps(itemtocheck.Serial, 1000);
					Thread.Sleep(50);
				}
				attributevalue = attributevalue + RazorEnhanced.Items.GetPropValue(itemtocheck.Serial, attributename);
			}

			itemtocheck = m_mobile.GetItemOnLayer(Assistant.Layer.Neck);
			if (itemtocheck != null)
			{
				if (!itemtocheck.PropsUpdated)
				{
					RazorEnhanced.Items.WaitForProps(itemtocheck.Serial, 1000);
					Thread.Sleep(50);
					
				}
				attributevalue = attributevalue + RazorEnhanced.Items.GetPropValue(itemtocheck.Serial, attributename);
			}

			itemtocheck = m_mobile.GetItemOnLayer(Assistant.Layer.OuterLegs);
			if (itemtocheck != null)
			{
				if (!itemtocheck.PropsUpdated)
				{
					RazorEnhanced.Items.WaitForProps(itemtocheck.Serial, 1000);
					Thread.Sleep(50);
				}
				attributevalue = attributevalue + RazorEnhanced.Items.GetPropValue(itemtocheck.Serial, attributename);
			}

			itemtocheck = m_mobile.GetItemOnLayer(Assistant.Layer.OuterTorso);
			if (itemtocheck != null)
			{
				if (!itemtocheck.PropsUpdated)
				{
					RazorEnhanced.Items.WaitForProps(itemtocheck.Serial, 1000);
					Thread.Sleep(50);
				}
				attributevalue = attributevalue + RazorEnhanced.Items.GetPropValue(itemtocheck.Serial, attributename);
			}

			itemtocheck = m_mobile.GetItemOnLayer(Assistant.Layer.Pants);
			if (itemtocheck != null)
			{ 
				if (!itemtocheck.PropsUpdated)
				{
					RazorEnhanced.Items.WaitForProps(itemtocheck.Serial, 1000);
					Thread.Sleep(50);
				}
				attributevalue = attributevalue + RazorEnhanced.Items.GetPropValue(itemtocheck.Serial, attributename);
			}

			itemtocheck = m_mobile.GetItemOnLayer(Assistant.Layer.RightHand);
			if (itemtocheck != null)
			{
				if (!itemtocheck.PropsUpdated)
				{
					RazorEnhanced.Items.WaitForProps(itemtocheck.Serial, 1000);
					Thread.Sleep(50);
				}
				attributevalue = attributevalue + RazorEnhanced.Items.GetPropValue(itemtocheck.Serial, attributename);
			}

			itemtocheck = m_mobile.GetItemOnLayer(Assistant.Layer.Ring);
			if (itemtocheck != null)
			{
				if (!itemtocheck.PropsUpdated)
				{
					RazorEnhanced.Items.WaitForProps(itemtocheck.Serial, 1000);
					Thread.Sleep(50);
				}
				attributevalue = attributevalue + RazorEnhanced.Items.GetPropValue(itemtocheck.Serial, attributename);

			}

			itemtocheck = m_mobile.GetItemOnLayer(Assistant.Layer.Shirt);
			if (itemtocheck != null)
			{
				if (!itemtocheck.PropsUpdated)
				{
					RazorEnhanced.Items.WaitForProps(itemtocheck.Serial, 1000);
					Thread.Sleep(50);
				}
				attributevalue = attributevalue + RazorEnhanced.Items.GetPropValue(itemtocheck.Serial, attributename);
			}

			itemtocheck = m_mobile.GetItemOnLayer(Assistant.Layer.Shoes);
			if (itemtocheck != null)
			{
				if (!itemtocheck.PropsUpdated)
				{
					RazorEnhanced.Items.WaitForProps(itemtocheck.Serial, 1000);
					Thread.Sleep(50);
				}
				attributevalue = attributevalue + RazorEnhanced.Items.GetPropValue(itemtocheck.Serial, attributename);
			}

			itemtocheck = m_mobile.GetItemOnLayer(Assistant.Layer.Unused_x9);
			if (itemtocheck != null)
			{
				if (!itemtocheck.PropsUpdated)
				{
					RazorEnhanced.Items.WaitForProps(itemtocheck.Serial, 1000);
					Thread.Sleep(50);
				}
				attributevalue = attributevalue + RazorEnhanced.Items.GetPropValue(itemtocheck.Serial, attributename);
			}

			itemtocheck = m_mobile.GetItemOnLayer(Assistant.Layer.Unused_xF);
			if (itemtocheck != null)
			{ 
				if (!itemtocheck.PropsUpdated)
				{
					RazorEnhanced.Items.WaitForProps(itemtocheck.Serial, 1000);					
					Thread.Sleep(50);
				}
				attributevalue = attributevalue + RazorEnhanced.Items.GetPropValue(itemtocheck.Serial, attributename);
			}

			itemtocheck = m_mobile.GetItemOnLayer(Assistant.Layer.Waist);
			if (itemtocheck != null)
				{
				if (!itemtocheck.PropsUpdated)
				{
					RazorEnhanced.Items.WaitForProps(itemtocheck.Serial, 1000);
					Thread.Sleep(50);
				}
				attributevalue = attributevalue + RazorEnhanced.Items.GetPropValue(itemtocheck.Serial, attributename);
			}

			return attributevalue;
        }

		private void razorButton1_Click(object sender, EventArgs e)
		{
			try
			{
				m_ProcessInfo.Abort();
			}
			catch { }

			this.Close();
		}

		private void bNameCopy_Click(object sender, EventArgs e)
		{
			Clipboard.SetText(lName.Text);
		}

		private void bSerialCopy_Click(object sender, EventArgs e)
		{
			Clipboard.SetText(lSerial.Text);
		}

		private void bItemIdCopy_Click(object sender, EventArgs e)
		{
			Clipboard.SetText(lMobileID.Text);
		}

		private void bColorCopy_Click(object sender, EventArgs e)
		{
			Clipboard.SetText(lColor.Text);
		}

		private void bPositionCopy_Click(object sender, EventArgs e)
		{
			Clipboard.SetText(lPosition.Text);
		}

		private void bContainerCopy_Click(object sender, EventArgs e)
		{
			Clipboard.SetText(lSex.Text);
		}

		private void bRContainerCopy_Click(object sender, EventArgs e)
		{
			Clipboard.SetText(lHits.Text);
		}

		private void bAmountCopy_Click(object sender, EventArgs e)
		{
			Clipboard.SetText(lMaxHits.Text);
		}

		private void bLayerCopy_Click(object sender, EventArgs e)
		{
			Clipboard.SetText(lNotoriety.Text);
		}

		private void bOwnedCopy_Click(object sender, EventArgs e)
		{
			Clipboard.SetText(lDirection.Text);
		}

		private void EnhancedMobileInspector_FormClosing(object sender, FormClosingEventArgs e)
		{
			try
			{
				m_ProcessInfo.Abort();
			}
			catch { }
		}

		private void EnhancedMobileInspector_Load(object sender, EventArgs e)
		{
			// general
			lName.Text = m_mobile.Name.ToString();
			lSerial.Text = "0x" + m_mobile.Serial.Value.ToString("X8");
			lMobileID.Text = "0x" + m_mobile.Body.ToString("X4");
			lColor.Text = m_mobile.Hue.ToString();
			lPosition.Text = m_mobile.Position.ToString();
			// Details
			if (m_mobile.Female)
				lSex.Text = "Female";
			else
				lSex.Text = "Male";
			lHits.Text = m_mobile.Hits.ToString();
			lMaxHits.Text = m_mobile.Hits.ToString();
			lNotoriety.Text = m_mobile.Notoriety.ToString();

			switch (m_mobile.Direction & Assistant.Direction.Mask)
			{
				case Assistant.Direction.North: lDirection.Text = "North"; break;
				case Assistant.Direction.South: lDirection.Text = "South"; break;
				case Assistant.Direction.West: lDirection.Text = "West"; break;
				case Assistant.Direction.East: lDirection.Text = "East"; break;
				case Assistant.Direction.Right: lDirection.Text = "Right"; break;
				case Assistant.Direction.Left: lDirection.Text = "Left"; break;
				case Assistant.Direction.Down: lDirection.Text = "Down"; break;
				case Assistant.Direction.Up: lDirection.Text = "Up"; break;
				default: lDirection.Text = "Undefined"; break;
			}

			if (m_mobile.Poisoned)
				lFlagPoisoned.Text = "Yes";
			else
				lFlagPoisoned.Text = "No";

			if (m_mobile.Warmode)
				lFlagWar.Text = "Yes";
			else
				lFlagWar.Text = "No";

			if (m_mobile.Visible)
				lFlagHidden.Text = "No";
			else
				lFlagHidden.Text = "Yes";

			if (m_mobile.IsGhost)
				lFlagGhost.Text = "Yes";
			else
				lFlagGhost.Text = "No";

			if (m_mobile.Blessed)     // Yellow Hits
				lFlagBlessed.Text = "Yes";
			else
				lFlagBlessed.Text = "No";

			if (m_mobile.Paralized)
				lFlagParalized.Text = "Yes";
			else
				lFlagParalized.Text = "No";

			for (int i = 0; i < m_mobile.ObjPropList.Content.Count; i++)
			{
				Assistant.ObjectPropertyList.OPLEntry ent = m_mobile.ObjPropList.Content[i];
				if (i == 0)
					lName.Text = ent.ToString();
				else
				{
					string content = ent.ToString();
					listBoxAttributes.Items.Add(content);
				}
			}

			if (m_mobile.ObjPropList.Content.Count == 0)
			{
				lName.Text = m_mobile.Name.ToString();
			}


			if (m_mobile == Assistant.World.Player)
			{
				listBoxAttributes.Items.Add("");
				listBoxAttributes.Items.Add("Weight: " + Assistant.World.Player.Weight);
				if (Assistant.World.Player.Expansion >= 3)
				{
					listBoxAttributes.Items.Add("Stat Cap: " + Assistant.World.Player.StatCap);
					listBoxAttributes.Items.Add("Followers: " + Assistant.World.Player.Followers);
					listBoxAttributes.Items.Add("Max Followers: " + Assistant.World.Player.FollowersMax);

					if (Assistant.World.Player.Expansion >= 4)
					{
						listBoxAttributes.Items.Add("Damage Minimum: " + Assistant.World.Player.DamageMin);
						listBoxAttributes.Items.Add("Damage Maximum: " + Assistant.World.Player.DamageMax);
						listBoxAttributes.Items.Add("Tithing points: " + Assistant.World.Player.Tithe);

						if (Assistant.World.Player.Expansion >= 5)
						{
							switch (Assistant.World.Player.Race)
							{
								case 1:
									listBoxAttributes.Items.Add("Race: Human");
									break;
								case 2:
									listBoxAttributes.Items.Add("Race: Elf");
									break;
								case 3:
									listBoxAttributes.Items.Add("Race: Gargoyle");
									break;
							}

							listBoxAttributes.Items.Add("Max Weight: " + Assistant.World.Player.MaxWeight);

							if (Assistant.World.Player.Expansion >= 6)
							{
								m_ProcessInfo = new Thread(ProcessInfoThread);
								m_ProcessInfo.Start();
							}
						}
					}
				}
			}
			else
			{
				m_ProcessInfo = new Thread(ProcessInfoThread);
				m_ProcessInfo.Start();
			}
		}
	}
}