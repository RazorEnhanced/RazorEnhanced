using Assistant;
using Assistant.UI;
using System;
using System.Collections.Generic;
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
            if (!Client.Running)
                return;

            Engine.MainWindow.SafeAction(s => s.BandageHealLogBox.Items.Add(addlog));
            Engine.MainWindow.SafeAction(s => s.BandageHealLogBox.SelectedIndex = s.BandageHealLogBox.Items.Count - 1);
            if (Engine.MainWindow.BandageHealLogBox.Items.Count > 300)
                Engine.MainWindow.SafeAction(s => s.BandageHealLogBox.Items.Clear());
        }

        internal static int TargetSerial
        {
            get
            {
                int serial = 0;
                try
                {
                    serial = Convert.ToInt32(Engine.MainWindow.BandageHealtargetLabel.Text, 16);
                }
                catch
                { }
                return serial;
            }

            set
            {
                Assistant.Engine.MainWindow.SafeAction(s => s.BandageHealtargetLabel.Text = "0x" + value.ToString("X8"));
            }
        }

        private static int m_customid;
        internal static int CustomID
        {
            get { return m_customid; }

            set
            {
                m_customid = value;
                Assistant.Engine.MainWindow.SafeAction(s => s.BandageHealcustomIDTextBox.Text = "0x" + value.ToString("X4"));
            }
        }

        private static int m_customcolor;
        internal static int CustomColor
        {
            get { return m_customcolor; }

            set
            {
                m_customcolor = value;
                Assistant.Engine.MainWindow.SafeAction(s => s.BandageHealcustomcolorTextBox.Text = "0x" + value.ToString("X4"));
            }
        }

        private static int m_customdelay;
        internal static int CustomDelay
        {
            get { return m_customdelay; }

            set
            {
                m_customdelay = value;
                Assistant.Engine.MainWindow.SafeAction(s => s.BandageHealdelayTextBox.Text = value.ToString());
            }
        }

        private static int m_maxrange;
        internal static int MaxRange
        {
            get { return m_maxrange; }

            set
            {
                m_maxrange = value;
                Assistant.Engine.MainWindow.SafeAction(s => s.BandageHealMaxRangeTextBox.Text = value.ToString());
            }
        }

        private static int m_hplimit;
        internal static int HpLimit
        {
            get { return m_hplimit; }

            set
            {
                m_hplimit = value;
                Assistant.Engine.MainWindow.SafeAction(s => s.BandageHealhpTextBox.Text = value.ToString());
            }
        }

        internal static bool PoisonBlock
        {
            get
            {
                return Engine.MainWindow.BandageHealpoisonCheckBox.Checked;
            }

            set
            {
                Assistant.Engine.MainWindow.SafeAction(s => s.BandageHealpoisonCheckBox.Checked = value);
            }
        }

        internal static bool MortalBlock
        {
            get
            {
                return Engine.MainWindow.BandageHealmortalCheckBox.Checked;
            }

            set
            {
                Assistant.Engine.MainWindow.SafeAction(s => s.BandageHealmortalCheckBox.Checked = value);
            }
        }

        internal static bool HiddenBlock
        {
            get
            {
                return Engine.MainWindow.BandageHealhiddedCheckBox.Checked;
            }

            set
            {
                Assistant.Engine.MainWindow.SafeAction(s => s.BandageHealhiddedCheckBox.Checked = value);
            }
        }

        internal static bool ShowCountdown
        {
            get
            {
                return Engine.MainWindow.BandageHealcountdownCheckBox.Checked;
            }

            set
            {
                Assistant.Engine.MainWindow.SafeAction(s => s.BandageHealcountdownCheckBox.Checked = value);
            }
        }


        internal static bool SelfHealUseText
        {
            get
            {
                return Assistant.Engine.MainWindow.BandageHealUseText.Checked;
            }

            set
            {
                Assistant.Engine.MainWindow.SafeAction(s => s.BandageHealUseText.Checked = value);
                Assistant.Engine.MainWindow.SafeAction(s => s.BandageHealUseTextSelfContent.Enabled = value);
                Assistant.Engine.MainWindow.SafeAction(s => s.BandageHealUseTextContent.Enabled = value);
                Assistant.Engine.MainWindow.SafeAction(s => s.BandageHealUseTarget.Enabled = !value);
                Assistant.Engine.MainWindow.SafeAction(s => s.BandageHealIgnoreCount.Enabled = value);
            }
        }
        internal static bool SelfHealIgnoreCount
        {
            get
            {
                return Assistant.Engine.MainWindow.BandageHealIgnoreCount.Checked;
            }

            set
            {
                Assistant.Engine.MainWindow.SafeAction(s => s.BandageHealIgnoreCount.Checked = value);
            }
        }
        internal static string SelfHealUseTextSelfContent
        {
            get
            {
                return Assistant.Engine.MainWindow.BandageHealUseTextSelfContent.Text;
            }

            set
            {
                Assistant.Engine.MainWindow.SafeAction(s => s.BandageHealUseTextSelfContent.Text = value);
            }
        }
        internal static string SelfHealUseTextContent
        {
            get
            {
                return Assistant.Engine.MainWindow.BandageHealUseTextContent.Text;
            }

            set
            {
                Assistant.Engine.MainWindow.SafeAction(s => s.BandageHealUseTextContent.Text = value);
            }
        }


        internal static bool UseTarget
        {
            get
            {
                return Assistant.Engine.MainWindow.BandageHealUseTarget.Checked;
            }

            set
            {
                Assistant.Engine.MainWindow.SafeAction(s => s.BandageHealUseTarget.Checked = value);
            }
        }

        internal static void LoadSettings()
        {
            ShowCountdown = Settings.General.ReadBool("BandageHealcountdownCheckBox");
            TargetSerial = Settings.General.ReadInt("BandageHealtargetLabel");
            Engine.MainWindow.BandageHealcustomCheckBox.Checked = Settings.General.ReadBool("BandageHealcustomCheckBox");
            CustomID = Settings.General.ReadInt("BandageHealcustomIDTextBox");
            CustomColor = Settings.General.ReadInt("BandageHealcustomcolorTextBox");
            Engine.MainWindow.BandageHealdexformulaCheckBox.Checked = Settings.General.ReadBool("BandageHealdexformulaCheckBox");
            Engine.MainWindow.BandageHealTimeWithBuf.Checked = Settings.General.ReadBool("BandageHealTimeWithBuf");
            CustomDelay = Settings.General.ReadInt("BandageHealdelayTextBox");
            HpLimit = Settings.General.ReadInt("BandageHealhpTextBox");
            MaxRange = Settings.General.ReadInt("BandageHealMaxRangeTextBox");
            PoisonBlock = Settings.General.ReadBool("BandageHealpoisonCheckBox");
            MortalBlock = Settings.General.ReadBool("BandageHealmortalCheckBox");
            HiddenBlock = Settings.General.ReadBool("BandageHealhiddedCheckBox");
            UseTarget = Settings.General.ReadBool("BandageHealUseTarget");
            SelfHealUseText = Settings.General.ReadBool("BandageHealUseText");
            SelfHealUseTextSelfContent = Settings.General.ReadString("BandageHealUseTextSelfContent");
            SelfHealUseTextContent = Settings.General.ReadString("BandageHealUseTextContent");
            SelfHealIgnoreCount = Settings.General.ReadBool("BandageHealIgnoreCount");

            Engine.MainWindow.BandageHealAutostartCheckBox.Checked = Settings.General.ReadBool("BandageHealAutostartCheckBox");

            Engine.MainWindow.BandageHealtargetComboBox.Items.Clear();
            Engine.MainWindow.BandageHealtargetComboBox.Items.Add("Self");
            Engine.MainWindow.BandageHealtargetComboBox.Items.Add("Target");
            Engine.MainWindow.BandageHealtargetComboBox.Items.Add("Friend");
            Engine.MainWindow.BandageHealtargetComboBox.Items.Add("Friend Or Self");
            Engine.MainWindow.BandageHealtargetComboBox.Text = Settings.General.ReadString("BandageHealtargetComboBox");

            if (Settings.General.ReadString("BandageHealtargetComboBox") == "Target")
            {
                Engine.MainWindow.BandageHealsettargetButton.Enabled = true;
                Engine.MainWindow.BandageHealtargetLabel.Enabled = true;
            }
            else
            {
                Engine.MainWindow.BandageHealsettargetButton.Enabled = false;
                Engine.MainWindow.BandageHealtargetLabel.Enabled = false;
            }


        }

        // Core

        internal static void EngineRun(Assistant.Mobile target)
        {
            if (target.Poisoned || (int)(target.Hits * 100 / (target.HitsMax == 0 ? (ushort)1 : target.HitsMax)) < m_hplimit)       // Check HP se bendare o meno.
            {
                if (RazorEnhanced.Settings.General.ReadBool("BandageHealhiddedCheckBox"))
                {
                    if (!World.Player.Visible)  // Esce se attivo blocco hidded
                        return;
                }

                if (RazorEnhanced.Settings.General.ReadBool("BandageHealpoisonCheckBox"))
                {
                    if (target.Poisoned) // Esce se attivo blocco poison
                        return;
                }

                if (RazorEnhanced.Settings.General.ReadBool("BandageHealmortalCheckBox"))                // Esce se attivo blocco mortal
                {
                    if (World.Player.Buffs.Contains(BuffIcon.MortalStrike) && (target.Serial == Player.Serial))
                        return;
                }

                Heal(target, true);
            }
        }


        internal static ManualResetEventSlim BandageFinish = new ManualResetEventSlim(true);

        internal static void BuffDebuff(PacketReader p, PacketHandlerEventArgs args)
        {
            UInt32 ser = p.ReadUInt32();
            UInt16 icon = p.ReadUInt16();
            UInt16 action = p.ReadUInt16();

            if (icon == (UInt16)BuffIcon.HealingSkill || icon == (UInt16)BuffIcon.Veterinary)
            {
                switch (action)
                {
                    case 0x01:
                        BandageFinish.Reset();
                        break;

                    case 0x0:
                        BandageFinish.Set();
                        break;
                }
            }
        }

        internal static bool CountActive = false;
        internal static void ShowCount(Assistant.Mobile target)
        {
            int seconds = 0;
            int maxCount = 10;
            while (CountActive && maxCount > 0)
            {
                Thread.Sleep(1000);
                seconds++;
                maxCount--;
                if (CountActive)
                    target.OverheadMessage(seconds.ToString());
            }
        }

        static internal Mutex HealMutex = new Mutex();
        static internal int NoBandageMsgCount = 0;
        internal static void Heal(Assistant.Mobile target, bool wait)
        {
            if (false == HealMutex.WaitOne(1))
                return;
            try
            {
                bool checkForBandage = true;
                if (SelfHealUseText && SelfHealIgnoreCount)
                    checkForBandage = false;

                Item bandage = null;
                if (checkForBandage)
                {
                    int bandageid = 0x0E21;
                    int bandagecolor = -1;

                    if (Settings.General.ReadBool("BandageHealcustomCheckBox"))         // se custom setto ID
                    {
                        bandageid = m_customid;
                        bandagecolor = m_customcolor;
                    }
                    bandage = SearchBandage(bandageid, bandagecolor); // Get serial bende

                    // Id base bende
                    // Conteggio bende
                    int bandageamount = 0;
                    if (bandage != null)
                        bandageamount = bandage.Amount; // RazorEnhanced.Items.BackpackCount(bandageid, bandagecolor);
                    if (bandageamount == 0)
                    {
                        if (NoBandageMsgCount <= 0)
                        {
                            Player.HeadMessage(10, "Bandage not found");
                            NoBandageMsgCount = 10;
                        }
                        NoBandageMsgCount--;
                        AddLog("Bandage not found");
                        Thread.Sleep(1000); // If no bandaids dont loop too quickly
                        return;
                    }

                    if (bandageamount < 11 && bandageamount > 1)    // don't warn on last bandaid to avoid constant message for everlasting bandage
                    {
                        Player.HeadMessage(10, "Warning: Low bandage: " + bandageamount + " left");
                        AddLog("Warning: Low bandage: " + bandageamount + " left");
                    }
                    AddLog("Using bandage (0x" + bandage.Serial.ToString("X8") + ") on Target (" + target.Name + " - " + target.Serial.ToString() + ")");
                }
                else
                {
                    AddLog("Using bandage on Target (" + target.Name + " - " + target.Serial.ToString() + ")");
                }


                System.Threading.Thread CountSeconds = null;


                if (SelfHealUseText)
                {
                    if (target.Serial == Player.Serial)
                    {
                        Player.ChatSay(0, SelfHealUseTextSelfContent);
                    }
                    else
                    {
                        Player.ChatSay(0, SelfHealUseTextContent);
                        Target.WaitForTarget(1000, true);
                        Target.TargetExecute(target.Serial);
                    }
                }
                else if (UseTarget) // Uso nuovo packet
                {
                    Items.UseItem(bandage.Serial);
                    Target.WaitForTarget(1000, true);
                    Target.TargetExecute(target.Serial);
                }
                else
                {
                    Items.UseItem(bandage.Serial, target.Serial, true);
                }
                if (!wait)
                    return;

                if (ShowCountdown)          // Se deve mostrare il cooldown
                {
                    CountActive = true;
                    CountSeconds = new System.Threading.Thread(() => ShowCount(target));
                    CountSeconds.Start();
                }
                //BandageFinish.Reset();

                if (RazorEnhanced.Settings.General.ReadBool("BandageHealdexformulaCheckBox"))
                {
                    BandageFinish.Reset(); // ensure its reset
                    double delay = (11 - (Player.Dex - (Player.Dex % 10)) / 20) * 1000;         // Calcolo delay in MS
                    if (delay < 1) // Limite per evitare che si vada in negativo
                        delay = 100;

                    BandageFinish.Wait((Int32)delay + 300);

                }
                else if (RazorEnhanced.Settings.General.ReadBool("BandageHealTimeWithBuf"))
                {
                    // This one doesn't reset because the heal should reset it
                    Thread.Sleep(100);
                    if (!BandageFinish.IsSet) // the bandage should have reset the bandage otherwise it must have gotten "you must wait"
                        BandageFinish.Wait(10000); // wait a max of 10 seconds, but buf should finish sooner
                }
                else                // Se ho un delay custom
                {
                    BandageFinish.Reset(); // ensure its reset
                    double delay = m_customdelay;
                    BandageFinish.Wait((Int32)delay + 300);
                }

                if (ShowCountdown)          // Se deve mostrare il cooldown
                {
                    CountActive = false;
                    CountSeconds.Abort();
                }
            }
            finally
            {
                HealMutex.ReleaseMutex();
            }
        }

        internal static void AutoRun()
        {
            if (!Client.Running)
                return;

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
                    {
                        RazorEnhanced.Mobiles.Filter targfilter = new Mobiles.Filter
                        {
                            Enabled = true,
                            Friend = 1,
                            RangeMax = m_maxrange
                        };
                        Mobile targ = RazorEnhanced.Mobiles.Select(RazorEnhanced.Mobiles.ApplyFilter(targfilter), "Weakest");
                        if (targ != null)
                            target = Assistant.World.FindMobile(targ.Serial);
                    }
                    break;
                case "Friend Or Self":
                    {
                        RazorEnhanced.Mobiles.Filter targfilter = new Mobiles.Filter
                        {
                            Enabled = true,
                            Friend = 1,
                            RangeMax = m_maxrange
                        };
                        List<Mobile> friends = RazorEnhanced.Mobiles.ApplyFilter(targfilter);
                        Mobile targ = RazorEnhanced.Mobiles.Select(friends, "Weakest");
                        if (targ == null)
                        {
                            target = World.Player;
                        }
                        else
                        {
                            int pct_life_friend = 100;
                            if (targ.HitsMax > 0)
                            {
                                pct_life_friend = 100 * targ.Hits / targ.HitsMax;
                            }

                            int pct_life_me;
                            {
                                pct_life_me = 100 * World.Player.Hits / World.Player.HitsMax;
                            }
                            if (pct_life_friend < pct_life_me)
                            {
                                target = Assistant.World.FindMobile(targ.Serial);
                            }
                            else
                            {
                                target = World.Player;
                            }
                        }
                    }
                    break;
            }

            if (target == null)         // Verifica se il target è valido
                return;
            if (!Utility.InRange(new Assistant.Point2D(Assistant.World.Player.Position.X, Assistant.World.Player.Position.Y), new Assistant.Point2D(target.Position.X, target.Position.Y), m_maxrange)) // Verifica distanza
                return;

            EngineRun(target);
        }

        // Funzioni da script

        /// <summary>
        /// Start BandageHeal Agent.
        /// </summary>
        public static void Start()
        {
            if (Assistant.Engine.MainWindow.BandageHealenableCheckBox.Checked == true)
            {
                Scripts.SendMessageScriptError("Script Error: BandageHeal.Start: Bandage Heal already running");
            }
            else
                Assistant.Engine.MainWindow.SafeAction(s => s.BandageHealenableCheckBox.Checked = true);
        }

        /// <summary>
        /// Stop BandageHeal Agent.
        /// </summary>
        public static void Stop()
        {
            if (Assistant.Engine.MainWindow.BandageHealenableCheckBox.Checked == false)
            {
                Scripts.SendMessageScriptError("Script Error: BandageHeal.Stop: Bandage Heal already sleeping");
            }
            else
                Assistant.Engine.MainWindow.SafeAction(s => s.BandageHealenableCheckBox.Checked = false);
        }

        /// <summary>
        /// Check BandageHeal Agent status, returns a bool value.
        /// </summary>
        /// <returns>True: is running - False: otherwise</returns>
        public static bool Status()
        {
            return Assistant.Engine.MainWindow.BandageHealenableCheckBox.Checked;
        }

        internal static Item SearchBandage(int itemid, int color)
        {
            // Genero filtro item
            Items.Filter itemFilter = new Items.Filter
            {
                Enabled = true
            };
            itemFilter.Graphics.Add(itemid);

            if (color != -1)
                itemFilter.Hues.Add(color);

            List<Item> containeritem = RazorEnhanced.Items.ApplyFilter(itemFilter);
            Assistant.Item belly = World.Player.GetItemOnLayer(Layer.Waist);

            foreach (Item found in containeritem)
            {
                if (found.RootContainer == World.Player.Backpack.Serial)
                {
                    return found;
                }
                if (belly != null && found.RootContainer == belly.Serial)
                {
                    return found;
                }

            }
            return null;
        }

        private static readonly Assistant.Timer m_autostart = Assistant.Timer.DelayedCallback(TimeSpan.FromSeconds(3.0), new Assistant.TimerCallback(Start));

        internal static void LoginAutostart()
        {
            if (!Status())
            {
                m_autostart.Start();
            }
        }
    }
}
