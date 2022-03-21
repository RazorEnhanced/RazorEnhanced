using Assistant.Filters;
using RazorEnhanced;
using RazorEnhanced.UI;
using System;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;

namespace Assistant
{
	public partial class MainForm : System.Windows.Forms.Form
	{
		internal CheckBox ChkNoRunStealth { get { return chknorunStealth; } }
        internal CheckBox ChkStealth { get { return chkStealth; } }
		internal CheckBox FilterPoison { get { return filterPoison; } }
        internal CheckBox FilterNPC { get { return filterNPC; } }
        internal CheckBox ShowHealthOH { get { return showHealthOH; } }
		internal CheckBox AutoOpenDoors { get { return autoOpenDoors; } }
		internal CheckBox ShowCorpseNames { get { return incomingCorpse; } }
		internal CheckBox FilterSnoopMsg { get { return filterSnoop; } }
		internal CheckBox FilterSpam { get { return spamFilter; } }
		internal CheckBox ForceSpeechHue { get { return chkForceSpeechHue; } }
		internal DataGridView GraphFilterDataGrid { get { return graphfilterdatagrid; } }
		internal DataGridView JournalFilterDataGrid { get { return journalfilterdatagrid; } }
		internal DataGridView JournalList { get { return journalList; } }
		internal CheckedListBox JournalTextSelection { get { return journalTextSelection; } }
		internal TextBox JournalFilterString { get { return journalFilterString; } }
		internal CheckBox ShowMobNames { get { return incomingMob; } }
		internal CheckBox LastTargTextFlags { get { return showtargtext; } }
		internal CheckBox SmartLastTarget { get { return smartLT; } }

		// Colori override
		internal int SysColor = 0;
		internal int WarningColor = 0;
		internal int SpeechHue = 0;
		internal int LTHilight = 0;
		internal int BeneficialSpellHue = 0;
		internal int HarmfulSpellHue = 0;
		internal int NeutralSpellHue = 0;

		private void queueTargets_CheckedChanged(object sender, System.EventArgs e)
		{
			if (queueTargets.Focused)
				RazorEnhanced.Settings.General.WriteBool("QueueTargets", queueTargets.Checked);
		}

		private void chkForceSpeechHue_CheckedChanged(object sender, System.EventArgs e)
		{
			setSpeechHue.Enabled = chkForceSpeechHue.Checked;
			if (chkForceSpeechHue.Focused)
				RazorEnhanced.Settings.General.WriteBool("ForceSpeechHue", chkForceSpeechHue.Checked);
		}

		private void lthilight_CheckedChanged(object sender, System.EventArgs e)
		{
			if (!(setLTHilight.Enabled = lthilight.Checked))
			{
				Settings.General.WriteInt("LTHilight", 0);
				LTHilight = 0;
				Assistant.Client.Instance.SetCustomNotoHue(0);
				lthilight.BackColor = SystemColors.Control;
				lthilight.ForeColor = SystemColors.ControlText;
			}
		}

		private void chkForceSpellHue_CheckedChanged(object sender, System.EventArgs e)
		{
			if (chkForceSpellHue.Checked)
			{
				setBeneHue.Enabled = setHarmHue.Enabled = setNeuHue.Enabled = true;
				RazorEnhanced.Settings.General.WriteBool("ForceSpellHue", true);
			}
			else
			{
				setBeneHue.Enabled = setHarmHue.Enabled = setNeuHue.Enabled = false;
				RazorEnhanced.Settings.General.WriteBool("ForceSpellHue", false);
			}
		}

		private void txtSpellFormat_TextChanged(object sender, System.EventArgs e)
		{
			if (txtSpellFormat.Focused)
				RazorEnhanced.Settings.General.WriteString("SpellFormat", txtSpellFormat.Text.Trim());
		}

		private void InitPreviewHue(Control ctrl, string cfg)
		{
			int hueIdx = RazorEnhanced.Settings.General.ReadInt(cfg);
			if (hueIdx > 0 && hueIdx < 3000)
			{
				ctrl.BackColor = Ultima.Hues.GetHue(hueIdx - 1).GetColor(HueEntry.TextHueIDX);
			}
			else
			{
				ctrl.BackColor = SystemColors.Control;
			}

			ctrl.ForeColor = (ctrl.BackColor.GetBrightness() < 0.35 ? System.Drawing.Color.White : System.Drawing.Color.Black);

			switch (cfg)
			{
				case "SysColor":
					SysColor = hueIdx;
					break;
				case "WarningColor":
					WarningColor = hueIdx;
					break;
				case "SpeechHue":
					SpeechHue = hueIdx;
					break;
				case "LTHilight":
					LTHilight = hueIdx;
					break;
				case "BeneficialSpellHue":
					BeneficialSpellHue = hueIdx;
					break;
				case "HarmfulSpellHue":
					HarmfulSpellHue = hueIdx;
					break;
				case "NeutralSpellHue":
					NeutralSpellHue = hueIdx;
					break;
			}
		}

		private bool SetHue(Control ctrl, string cfg)
		{
			HueEntry h = new HueEntry(RazorEnhanced.Settings.General.ReadInt(cfg));

			if (h.ShowDialog(this) == DialogResult.OK)
			{
				int hueIdx = h.Hue;
				RazorEnhanced.Settings.General.WriteInt(cfg, hueIdx);

				switch (cfg)
				{
					case "SysColor":
						SysColor = hueIdx;
						break;
					case "WarningColor":
						WarningColor = hueIdx;
						break;
					case "SpeechHue":
						SpeechHue = hueIdx;
						break;
					case "LTHilight":
						LTHilight = hueIdx;
						break;
					case "BeneficialSpellHue":
						BeneficialSpellHue = hueIdx;
						break;
					case "HarmfulSpellHue":
						HarmfulSpellHue = hueIdx;
						break;
					case "NeutralSpellHue":
						NeutralSpellHue = hueIdx;
						break;
				}

				if (hueIdx > 0 && hueIdx < 3000)
				{
					ctrl.BackColor = Ultima.Hues.GetHue(hueIdx - 1).GetColor(HueEntry.TextHueIDX);
				}
				else
				{
					ctrl.BackColor = System.Drawing.Color.White;
				}

				ctrl.ForeColor = (ctrl.BackColor.GetBrightness() < 0.35 ? System.Drawing.Color.White : System.Drawing.Color.Black);

				return true;
			}
			else
			{
				return false;
			}
		}

		private void setMsgHue_Click(object sender, System.EventArgs e)
		{
			SetHue(lblMsgHue, "SysColor");
		}

		private void setWarnHue_Click(object sender, System.EventArgs e)
		{
			SetHue(lblWarnHue, "WarningColor");
		}

		private void setSpeechHue_Click(object sender, System.EventArgs e)
		{
			SetHue(chkForceSpeechHue, "SpeechHue");
		}

		private void setLTHilight_Click(object sender, System.EventArgs e)
		{
			if (SetHue(lthilight, "LTHilight"))
				Assistant.Client.Instance.SetCustomNotoHue(LTHilight);
		}

		private void setBeneHue_Click(object sender, System.EventArgs e)
		{
			SetHue(lblBeneHue, "BeneficialSpellHue");
		}

		private void setHarmHue_Click(object sender, System.EventArgs e)
		{
			SetHue(lblHarmHue, "HarmfulSpellHue");
		}

		private void setNeuHue_Click(object sender, System.EventArgs e)
		{
			SetHue(lblNeuHue, "NeutralSpellHue");
		}

		private void QueueActions_CheckedChanged(object sender, System.EventArgs e)
		{
			if (QueueActions.Focused)
				RazorEnhanced.Settings.General.WriteBool("QueueActions", QueueActions.Checked);
		}

		private void txtObjDelay_TextChanged(object sender, System.EventArgs e)
		{
			if (txtObjDelay.Focused)
				RazorEnhanced.Settings.General.WriteInt("ObjectDelay", Utility.ToInt32(txtObjDelay.Text.Trim(), 500));
		}

		private void chkStealth_CheckedChanged(object sender, System.EventArgs e)
		{
			if (chkStealth.Focused)
				RazorEnhanced.Settings.General.WriteBool("CountStealthSteps", chkStealth.Checked);
		}

		private void druidClericPackets_CheckedChanged(object sender, System.EventArgs e)
		{
			if (druidClericPackets.Focused)
				RazorEnhanced.Settings.General.WriteBool("DruidClericPackets", druidClericPackets.Checked);
		}


		private void setpathmapbutton_Click(object sender, EventArgs e)
		{
			openmaplocation.RestoreDirectory = true;
			if (openmaplocation.ShowDialog(this) == DialogResult.OK)
			{
				enhancedmappathTextBox.Text = openmaplocation.FileName;
				Settings.General.WriteString("EnhancedMapPath", openmaplocation.FileName);
			}
		}

		private void dispDelta_CheckedChanged(object sender, System.EventArgs e)
		{
			if (dispDelta.Focused)
				RazorEnhanced.Settings.General.WriteBool("DisplaySkillChanges", dispDelta.Checked);
		}

		private void openCorpses_CheckedChanged(object sender, System.EventArgs e)
		{
			if (openCorpses.Focused)
				RazorEnhanced.Settings.General.WriteBool("AutoOpenCorpses", openCorpses.Checked);
			corpseRange.Enabled = openCorpses.Checked;
		}

		private void corpseRange_TextChanged(object sender, System.EventArgs e)
		{
			if (corpseRange.Focused)
				RazorEnhanced.Settings.General.WriteInt("CorpseRange", Utility.ToInt32(corpseRange.Text, 2));
		}

		private static readonly char[] m_InvalidNameChars = new char[] { '/', '\\', ';', '?', ':', '*' };

		private void spamFilter_CheckedChanged(object sender, System.EventArgs e)
		{
			if (spamFilter.Focused)
				RazorEnhanced.Settings.General.WriteBool("FilterSpam", spamFilter.Checked);
		}

		private void spellUnequip_CheckedChanged(object sender, System.EventArgs e)
		{
			if (spellUnequip.Focused)
				RazorEnhanced.Settings.General.WriteBool("SpellUnequip", spellUnequip.Checked);
		}

		private void rangeCheckLT_CheckedChanged(object sender, System.EventArgs e)
		{
			if (rangeCheckLT.Focused)
				RazorEnhanced.Settings.General.WriteBool("RangeCheckLT", rangeCheckLT.Checked);
			ltRange.Enabled = rangeCheckLT.Checked;
		}

		private void ltRange_TextChanged(object sender, System.EventArgs e)
		{
			if (ltRange.Focused)
				RazorEnhanced.Settings.General.WriteInt("LTRange", Utility.ToInt32(ltRange.Text, 11));
		}

		private void filterPoison_CheckedChanged(object sender, EventArgs e)
		{
			if (filterPoison.Focused)
				RazorEnhanced.Settings.General.WriteBool("FilterPoison", filterPoison.Checked);
		}

		private void filterNPC_CheckedChanged(object sender, EventArgs e)
		{
			if (filterNPC.Focused)
				RazorEnhanced.Settings.General.WriteBool("FilterNPC", filterNPC.Checked);
		}

		private void filterSnoop_CheckedChanged(object sender, System.EventArgs e)
		{
			if (filterSnoop.Focused)
				RazorEnhanced.Settings.General.WriteBool("FilterSnoopMsg", filterSnoop.Checked);
		}

		private void preAOSstatbar_CheckedChanged(object sender, System.EventArgs e)
		{
			if (preAOSstatbar.Focused)
				RazorEnhanced.Settings.General.WriteBool("OldStatBar", preAOSstatbar.Checked);

			Assistant.Client.Instance.RequestStatbarPatch(preAOSstatbar.Checked);
			if (World.Player != null && !m_Initializing)
				MessageBox.Show(this, "Close and re-open your status bar for the change to take effect.", "Status Window Note", MessageBoxButtons.OK, MessageBoxIcon.Information);
		}

		private void smartLT_CheckedChanged(object sender, System.EventArgs e)
		{
			if (smartLT.Focused)
				RazorEnhanced.Settings.General.WriteBool("SmartLastTarget", smartLT.Checked);
		}

		private void showtargtext_CheckedChanged(object sender, System.EventArgs e)
		{
			if (showtargtext.Focused)
				RazorEnhanced.Settings.General.WriteBool("LastTargTextFlags", showtargtext.Checked);
		}

		//private void smartCPU_CheckedChanged(object sender, System.EventArgs e)
		//{
		//	if (smartCPU.Focused)
		//		RazorEnhanced.Settings.General.WriteBool("SmartCPU", smartCPU.Checked);
		//		Assistant.Client.Instance.SetSmartCPU(smartCPU.Checked);
		//}

		private void blockDis_CheckedChanged(object sender, System.EventArgs e)
		{
			if (blockDis.Focused)
				RazorEnhanced.Settings.General.WriteBool("BlockDismount", blockDis.Checked);
		}

		private void autoOpenDoors_CheckedChanged(object sender, System.EventArgs e)
		{
			if (autoOpenDoors.Focused)
				RazorEnhanced.Settings.General.WriteBool("AutoOpenDoors", autoOpenDoors.Checked);
			if (!Client.IsOSI)
			{
				PropertyInfo getCurProfile = ClassicUOClient.CUOAssembly?.GetType("ClassicUO.Configuration.ProfileManager")?.GetProperty("CurrentProfile", BindingFlags.Public | BindingFlags.Static);
				if (getCurProfile != null)
				{
					var profile = getCurProfile.GetValue(null, null);
					if (profile != null)
					{
						PropertyInfo ProfileClass = ClassicUOClient.CUOAssembly?.GetType("ClassicUO.Configuration.Profile")?.GetProperty("AutoOpenDoors", BindingFlags.Public | BindingFlags.Instance);
						if (ProfileClass != null)
						{
							ProfileClass.SetValue(profile, autoOpenDoors.Checked, null);
						}
					}
				}				
			}

			hiddedAutoOpenDoors.Enabled = autoOpenDoors.Checked;
		}


		private void hiddedAutoOpenDoors_CheckedChanged(object sender, EventArgs e)
		{
			if (hiddedAutoOpenDoors.Focused)
				RazorEnhanced.Settings.General.WriteBool("HiddedAutoOpenDoors", hiddedAutoOpenDoors.Checked);
		}

		private void msglvl_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			if (msglvl.Focused)
				RazorEnhanced.Settings.General.WriteInt("MessageLevel", msglvl.SelectedIndex);
		}

		private readonly Timer m_ResizeTimer = Timer.DelayedCallback(TimeSpan.FromSeconds(3.0), new TimerCallback(ForceSize));

		internal static void ForceSize()
		{
			if (Client.IsOSI)
			{
				int x, y;

				if (RazorEnhanced.Settings.General.ReadBool("ForceSizeEnabled"))
				{
					x = RazorEnhanced.Settings.General.ReadInt("ForceSizeX");
					y = RazorEnhanced.Settings.General.ReadInt("ForceSizeY");

					if (x > 100 && x < 4000 && y > 100 && y < 4000)
						Assistant.Client.Instance.SetGameSize(x, y);
					else
						MessageBox.Show(Engine.MainWindow, Language.GetString(LocString.ForceSizeBad), "Bad Size", MessageBoxButtons.OK, MessageBoxIcon.Stop);
				}
				else
				{
					Assistant.Client.Instance.SetGameSize(0, 0);
				}

				ShowVideoGump();
			}
		}

		private static void ShowVideoGump()
		{
			if (World.Player != null)
			{
				Gumps.GumpData gd = Gumps.CreateGump(false, true, true, false);
				gd.gumpId = 0x9994df;
				gd.serial = (uint)Player.Serial;

				int x = 100;
				int y = 150;
				Gumps.AddImageTiled(ref gd, x, y, 300, 100, 0x4df);
				Gumps.AddLabel(ref gd, x + 50, y + 30, 0, @"Open Options and click Okay.");
				Gumps.AddLabel(ref gd, x + 50, y + 60, 0, @"This will clear up any abnormality");
				Gumps.SendGump(gd, 0, 0);
			}
		}

		private void gameSize_CheckedChanged(object sender, System.EventArgs e)
		{
			if (Client.IsOSI)
			{
				if (gameSize.Focused)
					RazorEnhanced.Settings.General.WriteBool("ForceSizeEnabled", gameSize.Checked);

				if (Assistant.Client.IsOSI)
				{
					forceSizeX.Enabled = forceSizeY.Enabled = gameSize.Checked;

					if (gameSize.Checked)
					{
						int x = Utility.ToInt32(forceSizeX.Text, 800);
						int y = Utility.ToInt32(forceSizeY.Text, 600);

						if (x < 100 || y < 100 || x > 4000 || y > 4000)
							MessageBox.Show(this, Language.GetString(LocString.ForceSizeBad), "Bad Size", MessageBoxButtons.OK, MessageBoxIcon.Stop);
						else
							Assistant.Client.Instance.SetGameSize(x, y);
					}
					else
					{
						Assistant.Client.Instance.SetGameSize(0, 0);
					}
				}
				else
				{
					forceSizeX.Enabled = forceSizeY.Enabled = gameSize.Enabled = gameSize.Checked = false;
				}

				ShowVideoGump();
			}
		}

		private void forceSizeX_TextChanged(object sender, System.EventArgs e)
		{
			int x = Utility.ToInt32(forceSizeX.Text, 600);
			if (forceSizeX.Focused && x >= 100 && x <= 3000)
			{
				RazorEnhanced.Settings.General.WriteInt("ForceSizeX", x);
				m_ResizeTimer.Stop();
				m_ResizeTimer.Start();
			}
		}

		private void notshowlauncher_CheckedChanged(object sender, EventArgs e)
		{
			if (notshowlauncher.Focused)
				RazorEnhanced.Settings.General.WriteBool("NotShowLauncher", notshowlauncher.Checked);
		}

		private void forceSizeY_TextChanged(object sender, System.EventArgs e)
		{
			int y = Utility.ToInt32(forceSizeY.Text, 600);
			if (forceSizeY.Focused && y >= 100 && y <= 3000)
			{
				RazorEnhanced.Settings.General.WriteInt("ForceSizeY", y);
				m_ResizeTimer.Stop();
				m_ResizeTimer.Start();
			}
		}

		private void potionEquip_CheckedChanged(object sender, System.EventArgs e)
		{
			if (potionEquip.Focused)
				RazorEnhanced.Settings.General.WriteBool("PotionEquip", potionEquip.Checked);
		}

		private void chknorunStealth_CheckedChanged(object sender, EventArgs e)
		{
			if (chknorunStealth.Focused)
				RazorEnhanced.Settings.General.WriteBool("ChkNoRunStealth", chknorunStealth.Checked);
		}

		private void autosearchcontainers_CheckedChanged(object sender, EventArgs e)
		{
			if (autosearchcontainers.Focused)
				RazorEnhanced.Settings.General.WriteBool("AutoSearch", autosearchcontainers.Checked);
		}

		private void nosearchpouches_CheckedChanged(object sender, EventArgs e)
		{
			if (nosearchpouches.Focused)
				RazorEnhanced.Settings.General.WriteBool("NoSearchPouches", nosearchpouches.Checked);
		}

		private void showHealthOH_CheckedChanged(object sender, System.EventArgs e)
		{
			if (showHealthOH.Focused)
				RazorEnhanced.Settings.General.WriteBool("ShowHealth", showHealthOH.Checked);
			healthFmt.Enabled = showHealthOH.Checked;
		}

        private void healthFmt_TextChanged(object sender, System.EventArgs e)
        {
            if (healthFmt.Focused)
                RazorEnhanced.Settings.General.WriteString("HealthFmt", healthFmt.Text);
        }

        private void journalFilter_TextChanged(object sender, System.EventArgs e)
        {
            RazorEnhanced.Settings.General.WriteString("JournalFilterText", journalFilterString.Text);

            System.Data.DataView dv = (System.Data.DataView)JournalList.DataSource;
            try
            {
                string[] allWords = JournalFilterString.Text.ToLower().Split(' ');
                if (allWords.Length > 0)
                {
                    string assembleFilter = "";
                    int lastCount = allWords.Length;
                    foreach (string word in allWords)
                    {
                        lastCount -= 1;
                        string trimmedWord = word.Trim();
                        if (trimmedWord.Length > 0)
                        {
                            assembleFilter += String.Format("text like '*{0}*'", trimmedWord);
                            if (lastCount > 0)
                                assembleFilter += " or ";
                        }
                    }
                    dv.RowFilter = assembleFilter;

                }
                else
                {
                    dv.RowFilter = "";
                }
            }
            catch (Exception)
            {
                dv.RowFilter = "";
            }
        }

		private void chkPartyOverhead_CheckedChanged(object sender, System.EventArgs e)
		{
			if (chkPartyOverhead.Focused)
				RazorEnhanced.Settings.General.WriteBool("ShowPartyStats", chkPartyOverhead.Checked);
		}

		private void rememberPwds_CheckedChanged(object sender, System.EventArgs e)
		{
			if (rememberPwds.Focused)
				RazorEnhanced.Settings.General.WriteBool("RememberPwds", rememberPwds.Checked);
		}

		private void actionStatusMsg_CheckedChanged(object sender, System.EventArgs e)
		{
			if (actionStatusMsg.Focused)
				RazorEnhanced.Settings.General.WriteBool("ActionStatusMsg", actionStatusMsg.Checked);
		}

		private void autoStackRes_CheckedChanged(object sender, System.EventArgs e)
		{
			if (autoStackRes.Focused)
				RazorEnhanced.Settings.General.WriteBool("AutoStack", autoStackRes.Checked);
		}

		private void clientPrio_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			string str = (string)clientPrio.SelectedItem;

			if (clientPrio.Focused)
				RazorEnhanced.Settings.General.WriteString("ClientPrio", str);

			try
			{
				if (Client.IsOSI)
		 			Assistant.Client.Instance.ClientProcess.PriorityClass = (System.Diagnostics.ProcessPriorityClass)Enum.Parse(typeof(System.Diagnostics.ProcessPriorityClass), str, true);
			}
			catch
			{
			}
		}

		private void incomingMob_CheckedChanged(object sender, System.EventArgs e)
		{
			if (incomingMob.Focused)
				RazorEnhanced.Settings.General.WriteBool("ShowMobNames", incomingMob.Checked);
		}

		private void incomingCorpse_CheckedChanged(object sender, System.EventArgs e)
		{
			if (incomingCorpse.Focused)
				RazorEnhanced.Settings.General.WriteBool("ShowCorpseNames", incomingCorpse.Checked);
		}

		private void alwaysTop_CheckedChanged(object sender, System.EventArgs e)
		{
			if (alwaysTop.Focused)
			{
				RazorEnhanced.Settings.General.WriteBool("AlwaysOnTop", alwaysTop.Checked);
				this.TopMost = alwaysTop.Checked;
			}
		}

		private void opacity_Scroll(object sender, System.EventArgs e)
		{
			int o = opacity.Value;

			if (opacity.Focused)
				RazorEnhanced.Settings.General.WriteInt("Opacity", o);

			opacityLabel.Text = String.Format("Opacity: {0}%", o);
			this.Opacity = ((double)o) / 100.0;
		}


        private void smartCPU_CheckedChanged(object sender, System.EventArgs e)
        {
            Assistant.Client.Instance.SmartCpuChecked = this.smartCPU.Checked;
        }

    private void taskbar_CheckedChanged(object sender, System.EventArgs e)
		{
			if (taskbar.Focused)
			{
				if (taskbar.Checked)
				{
					systray.Checked = false;
					RazorEnhanced.Settings.General.WriteBool("Systray", false);
					if (!this.ShowInTaskbar)
						MessageBox.Show(this, Language.GetString(LocString.NextRestart), "Notice", MessageBoxButtons.OK, MessageBoxIcon.Information);
				}
			}
		}

		private void systray_CheckedChanged(object sender, System.EventArgs e)
		{
			if (systray.Focused)
			{
				if (systray.Checked)
				{
					taskbar.Checked = false;
					RazorEnhanced.Settings.General.WriteBool("Systray", true);
					if (this.ShowInTaskbar)
						MessageBox.Show(this, Language.GetString(LocString.NextRestart), "Notice", MessageBoxButtons.OK, MessageBoxIcon.Information);
				}
			}
		}

		private void OnFilterCheck(object sender, System.Windows.Forms.ItemCheckEventArgs e)
		{
			((Filters.Filter)filters.Items[e.Index]).OnCheckChanged(e.NewValue);
		}

        private void OnJournalFilterCheck(object sender, System.Windows.Forms.ItemCheckEventArgs e)
        {
            string changed = (string)JournalTextSelection.Items[e.Index];
            RazorEnhanced.Settings.General.WriteBool("Journal" + changed, e.NewValue == CheckState.Checked);
        }

    }
}
