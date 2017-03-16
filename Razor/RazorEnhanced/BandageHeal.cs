using Assistant;
using System;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Collections.Generic;

namespace RazorEnhanced
{
	public class BandageHeal
	{
		private static bool m_AutoMode;

		internal static bool AutoMode
		{
			get { return m_AutoMode; }
			set { m_AutoMode = value; }
		}

		internal static void AddLog(string addlog)
		{
			if (!Assistant.Engine.Running)
				return;

			Assistant.Engine.MainWindow.BandageHealLogBox.Invoke(new Action(() => Assistant.Engine.MainWindow.BandageHealLogBox.Items.Add(addlog)));
			Assistant.Engine.MainWindow.BandageHealLogBox.Invoke(new Action(() => Assistant.Engine.MainWindow.BandageHealLogBox.SelectedIndex = Assistant.Engine.MainWindow.BandageHealLogBox.Items.Count - 1));
			if (Assistant.Engine.MainWindow.BandageHealLogBox.Items.Count > 300)
				Assistant.Engine.MainWindow.BandageHealLogBox.Invoke(new Action(() => Assistant.Engine.MainWindow.BandageHealLogBox.Items.Clear()));
		}

		internal static string TargetType
		{
			get
			{
				return (string)Assistant.Engine.MainWindow.BandageHealtargetComboBox.Invoke(new Func<string>(() => Assistant.Engine.MainWindow.BandageHealtargetComboBox.Text));
			}

			set
			{
				Assistant.Engine.MainWindow.BandageHealtargetComboBox.Invoke(new Action(() => Assistant.Engine.MainWindow.BandageHealtargetComboBox.Text = value));
			}
		}

		internal static int TargetSerial
		{
			get
			{
				int serial = 0;
				try
				{
					serial = Convert.ToInt32(Assistant.Engine.MainWindow.BandageHealtargetLabel.Text, 16);
				}
				catch
				{ }
				return serial;
			}

			set
			{
				Assistant.Engine.MainWindow.BandageHealtargetLabel.Invoke(new Action(() => Assistant.Engine.MainWindow.BandageHealtargetLabel.Text = "0x" + value.ToString("X8")));
			}
		}

		internal static bool CustomCheckBox
		{
			get
			{
				return (bool)Assistant.Engine.MainWindow.BandageHealcustomCheckBox.Invoke(new Func<bool>(() => Assistant.Engine.MainWindow.BandageHealcustomCheckBox.Checked));
			}

			set
			{
				Assistant.Engine.MainWindow.BandageHealcustomCheckBox.Invoke(new Action(() => Assistant.Engine.MainWindow.BandageHealcustomCheckBox.Checked = value));
			}
		}

		internal static int CustomID
		{
			get
			{
				int ID = 0;
				try
				{
					ID = Convert.ToInt32(Assistant.Engine.MainWindow.BandageHealcustomIDTextBox.Text, 16);
				}
				catch
				{ }
				return ID;
			}

			set
			{
				Assistant.Engine.MainWindow.BandageHealcustomIDTextBox.Invoke(new Action(() => Assistant.Engine.MainWindow.BandageHealcustomIDTextBox.Text = "0x" + value.ToString("X4")));
			}
		}

		internal static int CustomColor
		{
			get
			{
				int color = 0;
				try
				{
					color = Convert.ToInt32(Assistant.Engine.MainWindow.BandageHealcustomcolorTextBox.Text, 16);
				}
				catch
				{ }
				return color;
			}

			set
			{
				Assistant.Engine.MainWindow.BandageHealcustomcolorTextBox.Invoke(new Action(() => Assistant.Engine.MainWindow.BandageHealcustomcolorTextBox.Text = "0x" + value.ToString("X4")));
			}
		}

		internal static int CustomDelay
		{
			get
			{
				int delay = 1000;
				Assistant.Engine.MainWindow.BandageHealdelayTextBox.Invoke(new Action(() => Int32.TryParse(Assistant.Engine.MainWindow.BandageHealdelayTextBox.Text, out delay)));
				return delay;
			}

			set
			{
				Assistant.Engine.MainWindow.BandageHealdelayTextBox.Invoke(new Action(() => Assistant.Engine.MainWindow.BandageHealdelayTextBox.Text = value.ToString()));
			}
		}

		internal static int MaxRange
		{
			get
			{
				int range = 1;
				Assistant.Engine.MainWindow.BandageHealMaxRangeTextBox.Invoke(new Action(() => Int32.TryParse(Assistant.Engine.MainWindow.BandageHealMaxRangeTextBox.Text, out range)));
				return range;
			}

			set
			{
				Assistant.Engine.MainWindow.BandageHealMaxRangeTextBox.Invoke(new Action(() => Assistant.Engine.MainWindow.BandageHealMaxRangeTextBox.Text = value.ToString()));
			}
		}

		internal static bool DexFormula
		{
			get
			{
				return (bool)Assistant.Engine.MainWindow.BandageHealdexformulaCheckBox.Invoke(new Func<bool>(() => Assistant.Engine.MainWindow.BandageHealdexformulaCheckBox.Checked));
			}

			set
			{
				Assistant.Engine.MainWindow.BandageHealdexformulaCheckBox.Invoke(new Action(() => Assistant.Engine.MainWindow.BandageHealdexformulaCheckBox.Checked = value));
			}
		}

		internal static int HpLimit
		{
			get
			{
				int hplimit = 100;
				Assistant.Engine.MainWindow.BandageHealhpTextBox.Invoke(new Action(() => Int32.TryParse(Assistant.Engine.MainWindow.BandageHealhpTextBox.Text, out hplimit)));
				return hplimit;
			}

			set
			{
				Assistant.Engine.MainWindow.BandageHealhpTextBox.Invoke(new Action(() => Assistant.Engine.MainWindow.BandageHealhpTextBox.Text = value.ToString()));
			}
		}

		internal static bool PoisonBlock
		{
			get
			{
				return (bool)Assistant.Engine.MainWindow.BandageHealpoisonCheckBox.Invoke(new Func<bool>(() => Assistant.Engine.MainWindow.BandageHealpoisonCheckBox.Checked));
			}

			set
			{
				Assistant.Engine.MainWindow.BandageHealpoisonCheckBox.Invoke(new Action(() => Assistant.Engine.MainWindow.BandageHealpoisonCheckBox.Checked = value));
			}
		}

		internal static bool MortalBlock
		{
			get
			{
				return (bool)Assistant.Engine.MainWindow.BandageHealmortalCheckBox.Invoke(new Func<bool>(() => Assistant.Engine.MainWindow.BandageHealmortalCheckBox.Checked));
			}

			set
			{
				Assistant.Engine.MainWindow.BandageHealmortalCheckBox.Invoke(new Action(() => Assistant.Engine.MainWindow.BandageHealmortalCheckBox.Checked = value));
			}
		}

		internal static bool HiddenBlock
		{
			get
			{
				return (bool)Assistant.Engine.MainWindow.BandageHealhiddedCheckBox.Invoke(new Func<bool>(() => Assistant.Engine.MainWindow.BandageHealhiddedCheckBox.Checked));
			}

			set
			{
				Assistant.Engine.MainWindow.BandageHealhiddedCheckBox.Invoke(new Action(() => Assistant.Engine.MainWindow.BandageHealhiddedCheckBox.Checked = value));
			}
		}

		internal static bool ShowCountdown
		{
			get
			{
				return (bool)Assistant.Engine.MainWindow.BandageHealcountdownCheckBox.Invoke(new Func<bool>(() => Assistant.Engine.MainWindow.BandageHealcountdownCheckBox.Checked));
			}

			set
			{
				Assistant.Engine.MainWindow.BandageHealcountdownCheckBox.Invoke(new Action(() => Assistant.Engine.MainWindow.BandageHealcountdownCheckBox.Checked = value));
			}
		}

		internal static void LoadSettings()
		{
			ShowCountdown = RazorEnhanced.Settings.General.ReadBool("BandageHealcountdownCheckBox");
			string BandageHealtargetComboBox = RazorEnhanced.Settings.General.ReadString("BandageHealtargetComboBox");
			TargetSerial = RazorEnhanced.Settings.General.ReadInt("BandageHealtargetLabel");
			Assistant.Engine.MainWindow.BandageHealcustomIDTextBox.Enabled = Assistant.Engine.MainWindow.BandageHealcustomcolorTextBox.Enabled = RazorEnhanced.Settings.General.ReadBool("BandageHealcustomCheckBox");
			CustomID = RazorEnhanced.Settings.General.ReadInt("BandageHealcustomIDTextBox");
			CustomColor = RazorEnhanced.Settings.General.ReadInt("BandageHealcustomcolorTextBox");
			Assistant.Engine.MainWindow.BandageHealdelayTextBox.Enabled = RazorEnhanced.Settings.General.ReadBool("BandageHealdexformulaCheckBox");
			CustomDelay = RazorEnhanced.Settings.General.ReadInt("BandageHealdelayTextBox");
			HpLimit = RazorEnhanced.Settings.General.ReadInt("BandageHealhpTextBox");
			MaxRange = RazorEnhanced.Settings.General.ReadInt("BandageHealMaxRangeTextBox");
			PoisonBlock = RazorEnhanced.Settings.General.ReadBool("BandageHealpoisonCheckBox");
			MortalBlock = RazorEnhanced.Settings.General.ReadBool("BandageHealmortalCheckBox");
			HiddenBlock = RazorEnhanced.Settings.General.ReadBool("BandageHealhiddedCheckBox");

			Assistant.Engine.MainWindow.BandageHealtargetComboBox.Items.Clear();
			Assistant.Engine.MainWindow.BandageHealtargetComboBox.Items.Add("Self");
			Assistant.Engine.MainWindow.BandageHealtargetComboBox.Items.Add("Target");
			Assistant.Engine.MainWindow.BandageHealtargetComboBox.Items.Add("Friend");
			Assistant.Engine.MainWindow.BandageHealtargetComboBox.Text = RazorEnhanced.Settings.General.ReadString("BandageHealtargetComboBox");

			if (RazorEnhanced.Settings.General.ReadString("BandageHealtargetComboBox") == "Target")
			{
				Assistant.Engine.MainWindow.BandageHealsettargetButton.Enabled = true;
				Assistant.Engine.MainWindow.BandageHealtargetLabel.Enabled = true;
			}
			else
			{
				Assistant.Engine.MainWindow.BandageHealsettargetButton.Enabled = false;
				Assistant.Engine.MainWindow.BandageHealtargetLabel.Enabled = false;
			}
		}

		// Core

		internal static int EngineRun(Assistant.Mobile target)
		{
			if ((int)(target.Hits * 100 / (target.HitsMax == 0 ? (ushort)1 : target.HitsMax)) < HpLimit || target.Poisoned)       // Check HP se bendare o meno.
			{
				if (RazorEnhanced.Settings.General.ReadBool("BandageHealhiddedCheckBox"))
				{
					if (!World.Player.Visible)  // Esce se attivo blocco hidded
						return 0;
				}

				if (RazorEnhanced.Settings.General.ReadBool("BandageHealpoisonCheckBox"))
				{
					if (target.Poisoned) // Esce se attivo blocco poison
						return 0;
				}

				if (RazorEnhanced.Settings.General.ReadBool("BandageHealmortalCheckBox"))                // Esce se attivo blocco mortal
				{
					if (Player.BuffsExist("Mortal Strike"))
						return 0;
				}

				if (Targeting.HasTarget)
				{
					Target.Cancel();
					Thread.Sleep(100);
				}

				bool bandagefound = false;
				int bandageamount = 0;
				if (RazorEnhanced.Settings.General.ReadBool("BandageHealcustomCheckBox"))         // Se cerco bende custom
				{
					bandagefound = RazorEnhanced.Items.UseItemByID(RazorEnhanced.Settings.General.ReadInt("BandageHealcustomIDTextBox"), RazorEnhanced.Settings.General.ReadInt("BandageHealcustomcolorTextBox"));
					bandageamount = RazorEnhanced.Items.BackpackCount(RazorEnhanced.Settings.General.ReadInt("BandageHealcustomIDTextBox"), RazorEnhanced.Settings.General.ReadInt("BandageHealcustomcolorTextBox"));
					if (bandageamount == 0)
					{
						Player.HeadMessage(10, "Bandage not found");
						AddLog("Bandage not found");
					}
					else if (bandageamount < 11 && bandageamount > 0)
					{
						Player.HeadMessage(10, "Warning: Low bandage: " + bandageamount + " left");
						AddLog("Warning: Low bandage: " + bandageamount + " left");
					}
				}
				else
				{
					bandagefound = RazorEnhanced.Items.UseItemByID(0x0E21, -1);
					bandageamount = RazorEnhanced.Items.BackpackCount(0x0E21, -1);
					if (bandageamount == 0)
					{
						Player.HeadMessage(10, "Bandage not found");
						AddLog("Bandage not found");
					}
					else if (bandageamount < 11 && bandageamount > 0)
					{
						Player.HeadMessage(10, "Warning: Low bandage: " + bandageamount + " left");
						AddLog("Warning: Low bandage: " + bandageamount + " left");
					}
				}

				if (bandagefound)        // Cerca le bende
				{
					AddLog("Using bandage!");
					Target.WaitForTarget(1000);
					AddLog("Targetting: " + target.Serial.ToString());
					Assistant.Targeting.Target(target);

					if (RazorEnhanced.Settings.General.ReadBool("BandageHealdexformulaCheckBox"))
					{
						double delay = (11 - (Player.Dex - (Player.Dex % 10)) / 20) * 1000;         // Calcolo delay in MS
						if (RazorEnhanced.Settings.General.ReadBool("BandageHealcountdownCheckBox"))          // Se deve mostrare il cooldown
						{
							int second = 0;

							var delays = delay.ToString(CultureInfo.InvariantCulture).Split('.');
							int first = int.Parse(delays[0]);
							if (delays.Count() > 1)
								second = int.Parse(delays[1]);

							while (first > 0)
							{
								Player.HeadMessage(10, (first / 1000).ToString());
								AddLog("Delay counting....");
								first = first - 1000;
								Thread.Sleep(1000);
							}
							Thread.Sleep(second + 300);           // Pausa dei decimali rimasti
						}
						else
						{
							Thread.Sleep((Int32)delay + 300);
						}
					}
					else                // Se ho un delay custom
					{
						double delay = RazorEnhanced.Settings.General.ReadInt("BandageHealdelayTextBox");
						if (RazorEnhanced.Settings.General.ReadBool("BandageHealcountdownCheckBox"))          // Se deve mostrare il cooldown
						{
							double subdelay = delay / 1000;

							int second = 0;

							var delays = subdelay.ToString(CultureInfo.InvariantCulture).Split('.');
							int first = int.Parse(delays[0]);
							if (delays.Count() > 1)
								second = int.Parse(delays[1]);

							while (first > 0)
							{
								Player.HeadMessage(10, (first / 1000).ToString());
								AddLog("Delay counting....");
								first--;
								Thread.Sleep(1000);
							}
							Thread.Sleep(second + 300);           // Pausa dei decimali rimasti
						}
						else
						{
							Thread.Sleep((Int32)delay + 300);
						}
					}
				}
				else        // Fine bende
				{
					Thread.Sleep(5000);
				}
			}
			return 0;
		}

		internal static void AutoRun()
		{
			int exit = Int32.MinValue;

			if (World.Player == null)
				return;

			if (World.Player.IsGhost)
				return;

			Assistant.Mobile target = null;

			switch (RazorEnhanced.Settings.General.ReadString("BandageHealtargetComboBox"))
			{
				case "Self":
					target = World.Player;
					break;
				case "Target":
					target = Assistant.World.FindMobile(TargetSerial);
					break;
				case "Friend":
					RazorEnhanced.Mobiles.Filter targfilter = new Mobiles.Filter();
					targfilter.Enabled = true;
					targfilter.Friend = 1;
					targfilter.RangeMax = RazorEnhanced.Settings.General.ReadInt("BandageHealMaxRangeTextBox");
					Mobile targ = RazorEnhanced.Mobiles.Select(RazorEnhanced.Mobiles.ApplyFilter(targfilter), "Weakest");
					if (targ != null)
						target = Assistant.World.FindMobile(targ.Serial);
					break;
			}

			if (target == null)         // Verifica se il target è valido
				return;
			if (!Utility.InRange(new Assistant.Point2D(Assistant.World.Player.Position.X, Assistant.World.Player.Position.Y), new Assistant.Point2D(target.Position.X, target.Position.Y), RazorEnhanced.Settings.General.ReadInt("BandageHealMaxRangeTextBox"))) // Verifica distanza
				return;

			exit = EngineRun(target);
		}

		// Funzioni da script
		public static void Start()
		{
			if (!ClientCommunication.AllowBit(FeatureBit.AutoBandage))
			{
				Scripts.SendMessageScriptError("AutoBandage Not Allowed!");
				return;
			}


			if (Assistant.Engine.MainWindow.BandageHealenableCheckBox.Checked == true)
			{
				Scripts.SendMessageScriptError("Script Error: BandageHeal.Start: Bandage Heal already running");
			}
			else
				Assistant.Engine.MainWindow.BandageHealenableCheckBox.Invoke(new Action(() => Assistant.Engine.MainWindow.BandageHealenableCheckBox.Checked = true));
		}

		public static void Stop()
		{
			if (Assistant.Engine.MainWindow.BandageHealenableCheckBox.Checked == false)
			{
				Scripts.SendMessageScriptError("Script Error: BandageHeal.Stop: Bandage Heal already sleeping");
			}
			else
				Assistant.Engine.MainWindow.BandageHealenableCheckBox.Invoke(new Action(() => Assistant.Engine.MainWindow.BandageHealenableCheckBox.Checked = false));
		}

		public static bool Status()
		{
			return Assistant.Engine.MainWindow.BandageHealenableCheckBox.Checked;
		}
	}
}