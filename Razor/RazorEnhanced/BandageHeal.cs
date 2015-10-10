using Assistant;
using System;
using System.Globalization;
using System.Linq;
using System.Threading;

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
			if (Assistant.Engine.Running)
			{
				Assistant.Engine.MainWindow.BandageHealLogBox.Invoke(new Action(() => Assistant.Engine.MainWindow.BandageHealLogBox.Items.Add(addlog)));
				Assistant.Engine.MainWindow.BandageHealLogBox.Invoke(new Action(() => Assistant.Engine.MainWindow.BandageHealLogBox.SelectedIndex = Assistant.Engine.MainWindow.BandageHealLogBox.Items.Count - 1));
				if (Assistant.Engine.MainWindow.BandageHealLogBox.Items.Count > 300)
					Assistant.Engine.MainWindow.BandageHealLogBox.Invoke(new Action(() => Assistant.Engine.MainWindow.BandageHealLogBox.Items.Clear()));
			}
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
			bool BandageHealcountdownCheckBox = RazorEnhanced.Settings.General.ReadBool("BandageHealcountdownCheckBox");
			string BandageHealtargetComboBox = RazorEnhanced.Settings.General.ReadString("BandageHealtargetComboBox");
			int BandageHealtargetLabel = RazorEnhanced.Settings.General.ReadInt("BandageHealtargetLabel");
			bool BandageHealcustomCheckBox = RazorEnhanced.Settings.General.ReadBool("BandageHealcustomCheckBox");
			int BandageHealcustomIDTextBox = RazorEnhanced.Settings.General.ReadInt("BandageHealcustomIDTextBox");
			int BandageHealcustomcolorTextBox = RazorEnhanced.Settings.General.ReadInt("BandageHealcustomcolorTextBox");
			bool BandageHealdexformulaCheckBox = RazorEnhanced.Settings.General.ReadBool("BandageHealdexformulaCheckBox");
			int BandageHealdelayTextBox = RazorEnhanced.Settings.General.ReadInt("BandageHealdelayTextBox");
			int BandageHealhpTextBox = RazorEnhanced.Settings.General.ReadInt("BandageHealhpTextBox");
			bool BandageHealpoisonCheckBox = RazorEnhanced.Settings.General.ReadBool("BandageHealpoisonCheckBox");
			bool BandageHealmortalCheckBox = RazorEnhanced.Settings.General.ReadBool("BandageHealmortalCheckBox");
			bool BandageHealhiddedCheckBox = RazorEnhanced.Settings.General.ReadBool("BandageHealhiddedCheckBox");

			Assistant.Engine.MainWindow.BandageHealtargetComboBox.Items.Clear();
			Assistant.Engine.MainWindow.BandageHealtargetComboBox.Items.Add("Self");
			Assistant.Engine.MainWindow.BandageHealtargetComboBox.Items.Add("Target");

			ShowCountdown = BandageHealcountdownCheckBox;
			HiddenBlock = BandageHealhiddedCheckBox;
			MortalBlock = BandageHealmortalCheckBox;
			PoisonBlock = BandageHealpoisonCheckBox;
			HpLimit = BandageHealhpTextBox;
			CustomDelay = BandageHealdelayTextBox;
			DexFormula = BandageHealdexformulaCheckBox;
			if (DexFormula)
				Assistant.Engine.MainWindow.BandageHealdelayTextBox.Enabled = false;
			else
				Assistant.Engine.MainWindow.BandageHealdelayTextBox.Enabled = true;

			CustomColor = BandageHealcustomcolorTextBox;
			CustomID = BandageHealcustomIDTextBox;
			CustomCheckBox = BandageHealcustomCheckBox;
			if (CustomCheckBox)
			{
				Assistant.Engine.MainWindow.BandageHealcustomIDTextBox.Enabled = true;
				Assistant.Engine.MainWindow.BandageHealcustomcolorTextBox.Enabled = true;
			}
			else
			{
				Assistant.Engine.MainWindow.BandageHealcustomIDTextBox.Enabled = false;
				Assistant.Engine.MainWindow.BandageHealcustomcolorTextBox.Enabled = false;
			}

			TargetSerial = BandageHealtargetLabel;
			TargetType = BandageHealtargetComboBox;
			if (TargetType == "Target")
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

				int serialbende = FindBandage();
				if (serialbende != 0)        // Cerca le bende
				{
					if (Targeting.HasTarget)
					{
						Target.Cancel();
						Thread.Sleep(100);
					}

					Assistant.ClientCommunication.SendToServer(new DoubleClick((Assistant.Serial)serialbende));
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
					Player.HeadMessage(10, "Bandage not found");
					AddLog("Bandage not found");
					Thread.Sleep(5000);
				}
			}
			return 0;
		}

		internal static int FindBandage()
		{
			if (RazorEnhanced.Settings.General.ReadBool("BandageHealcustomCheckBox"))         // Se cerco bende custom
			{
				foreach (Assistant.Item iteminzaino in Assistant.World.Player.Backpack.Contains)
				{
					if (iteminzaino.ItemID == RazorEnhanced.Settings.General.ReadInt("BandageHealcustomIDTextBox") && iteminzaino.Hue == RazorEnhanced.Settings.General.ReadInt("BandageHealcustomcolorTextBox"))
					{
						if (iteminzaino.Amount < 11)
						{
							Player.HeadMessage(10, "Warning: Low bandage: " + iteminzaino.Amount + " left");
							AddLog("Warning: Low bandage: " + iteminzaino.Amount + " left");
						}
						return iteminzaino.Serial;
					}
				}
			}
			else
			{
				foreach (Assistant.Item iteminzaino in Assistant.World.Player.Backpack.Contains)
				{
					if (iteminzaino.ItemID == 0x0E21)
					{
						if (iteminzaino.Amount < 11)
						{
							Player.HeadMessage(10, "Warning: Low bandage: " + iteminzaino.Amount + " left");
							AddLog("Warning: Low bandage: " + iteminzaino.Amount + " left");
						}
						return iteminzaino.Serial;
					}
				}
			}
			return 0;
		}

		internal static void Engine()
		{
			int exit = Int32.MinValue;

			if (World.Player == null)
				return;

			if (World.Player.IsGhost)
				return;

			Assistant.Mobile target;

			if (RazorEnhanced.Settings.General.ReadString("BandageHealtargetComboBox") == "Self")
			{
				target = World.Player;
			}
			else
			{
				target = Assistant.World.FindMobile(TargetSerial);
				if (target == null)         // Verifica se il target è valido
					return;
				if (!Utility.InRange(new Assistant.Point2D(Assistant.World.Player.Position.X, Assistant.World.Player.Position.Y), new Assistant.Point2D(target.Position.X, target.Position.Y), 1)) // Verifica distanza
					return;
			}

			exit = EngineRun(target);
		}

		// Funzioni da script
		public static void Start()
		{
			if (Assistant.Engine.MainWindow.BandageHealenableCheckBox.Checked == true)
				Misc.SendMessage("Script Error: BandageHeal.Start: Bandage Heal already running");
			else
				Assistant.Engine.MainWindow.BandageHealenableCheckBox.Invoke(new Action(() => Assistant.Engine.MainWindow.BandageHealenableCheckBox.Checked = true));
		}

		public static void Stop()
		{
			if (Assistant.Engine.MainWindow.BandageHealenableCheckBox.Checked == false)
				Misc.SendMessage("Script Error: BandageHeal.Stop: Bandage Heal already sleeping");
			else
				Assistant.Engine.MainWindow.BandageHealenableCheckBox.Invoke(new Action(() => Assistant.Engine.MainWindow.BandageHealenableCheckBox.Checked = false));
		}

		public static bool Status()
		{
			return Assistant.Engine.MainWindow.BandageHealenableCheckBox.Checked;
		}
	}
}
