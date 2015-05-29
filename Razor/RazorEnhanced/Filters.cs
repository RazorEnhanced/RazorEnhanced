using System;
using System.IO;
using System.Collections.Generic;
using Assistant;
using System.Windows.Forms;
using System.Threading;
using RazorEnhanced;

namespace RazorEnhanced
{
	public class Filters
	{
        internal static int BoneCutterBlade
        {
            get
            {
                try
                {
                    int serialblade = Convert.ToInt32(Assistant.Engine.MainWindow.BoneBladeLabel.Text, 16);
                    if (serialblade == 0)
                    {
                        return 0;
                    }

                    Item blade = RazorEnhanced.Items.FindBySerial(serialblade);
                    if (blade != null && blade.RootContainer == World.Player)
                       return blade.Serial;
                    else
                        return 0;
                }
                catch 
                {
                    return 0;
                }
            }

            set
            {
                Assistant.Engine.MainWindow.BoneBladeLabel.Invoke(new Action(() => Assistant.Engine.MainWindow.BoneBladeLabel.Text = "0x" + value.ToString("X8")));
            }
        }

        internal static int AutoCarverBlade
        {
            get
            {
                try
                {
                    int serialblade = Convert.ToInt32(Assistant.Engine.MainWindow.AutoCarverBladeLabel.Text, 16);
                    if (serialblade == 0)
                        return 0;

                    Item blade = RazorEnhanced.Items.FindBySerial(serialblade);
                    if (blade != null && blade.RootContainer == World.Player)
                        return blade.Serial;
                    else
                        return 0;
                }
                catch
                {
                    return 0;
                }
            }

            set
            {
                Assistant.Engine.MainWindow.AutoCarverBladeLabel.Invoke(new Action(() => Assistant.Engine.MainWindow.AutoCarverBladeLabel.Text = "0x" + value.ToString("X8")));
            }
        }

        internal static void ProcessMessage(Assistant.Mobile m)
        {
            if (m.Serial == World.Player.Serial)      // Skip Self
                return;

            if (Assistant.Engine.MainWindow.FlagsHighlightCheckBox.Checked)
            {
                if (m.Poisoned)
                    RazorEnhanced.Mobiles.Message(m.Serial, 10, "[Poisoned]");
                if (m.IsGhost)
                    RazorEnhanced.Mobiles.Message(m.Serial, 10, "[Dead]");
            }

            if (Assistant.Engine.MainWindow.HighlightTargetCheckBox.Checked)
            {
                if (Targeting.IsLastTarget(m))
                    RazorEnhanced.Mobiles.Message(m.Serial, 10, "*[Target]*");
            }

        }


        //////////////// AUTOCARVER START ////////////////

        private static Queue<Item> m_IgnoreCorpiQueue = new Queue<Item>();
        private static bool m_AutoCarver;

        internal static bool AutoCarver
        {
            get { return m_AutoCarver; }
            set { m_AutoCarver = value; }
        }

        internal static int AutoCarverEngine(Items.Filter filter)
        {
            bool giaTagliato = false;
            if (World.Player == null)       // Esce se non loggato
                return 0;

            if (AutoCarverBlade == 0)       // Esce in caso di errore lettura blade
                return 0;


            List<Item> corpi = RazorEnhanced.Items.ApplyFilter(filter);

            foreach (RazorEnhanced.Item corpo in corpi)
            {
                foreach (RazorEnhanced.Item corpoIgnorato in m_IgnoreCorpiQueue)
                {
                    if (corpoIgnorato.Serial == corpo.Serial)
                        giaTagliato = true;
                }
                if (!giaTagliato)
                {
                    Thread.Sleep(200);
                    Items.UseItem(Items.FindBySerial(AutoCarverBlade));
                    Target.WaitForTarget(1000);
                    Target.TargetExecute(corpo.Serial);
                    Items.Message(corpo.Serial, 10, "*Cutting*");
                    
                    m_IgnoreCorpiQueue.Enqueue(corpo);
                    if (m_IgnoreCorpiQueue.Count > 50)
                        m_IgnoreCorpiQueue.Dequeue();
                    Thread.Sleep(200);
                }
            }
            return 0;
        }


        internal static void AutoCarverEngine()
        {
            int exit = Int32.MinValue;

            // Genero filtro per corpi
            Items.Filter corpseFilter = new Items.Filter();
            corpseFilter.RangeMax = 3;
            corpseFilter.Movable = false;
            corpseFilter.IsCorpse = true;
            corpseFilter.OnGround = true;
            corpseFilter.Enabled = true;

            exit = AutoCarverEngine(corpseFilter);
        }

        //////////////// AUTOCARVER STOP ////////////////

        //////////////// Load settings ////////////////
        internal static void LoadSettings()
        {
            bool HighlightTargetCheckBox = false;
            bool FlagsHighlightCheckBox = false;
            bool ShowStaticFieldCheckBox = false;
            bool BlockTradeRequestCheckBox = false;
            bool BlockPartyInviteCheckBox = false;
            bool MobFilterCheckBox = false;
            bool AutoCarverCheckBox = false;
            bool BoneCutterCheckBox = false;
            int AutoCarverBladeLabel = 0;
            int BoneBladeLabel = 0;

            RazorEnhanced.Settings.General.EnhancedFilterLoadAll(out HighlightTargetCheckBox, out FlagsHighlightCheckBox, out ShowStaticFieldCheckBox, out BlockTradeRequestCheckBox, out BlockPartyInviteCheckBox, out MobFilterCheckBox, out AutoCarverCheckBox, out BoneCutterCheckBox, out AutoCarverBladeLabel, out BoneBladeLabel);

            Assistant.Engine.MainWindow.HighlightTargetCheckBox.Checked = HighlightTargetCheckBox;
            Assistant.Engine.MainWindow.FlagsHighlightCheckBox.Checked = FlagsHighlightCheckBox;
            Assistant.Engine.MainWindow.ShowStaticFieldCheckBox.Checked = ShowStaticFieldCheckBox;
            Assistant.Engine.MainWindow.BlockTradeRequestCheckBox.Checked = BlockTradeRequestCheckBox;
            Assistant.Engine.MainWindow.BlockPartyInviteCheckBox.Checked = BlockPartyInviteCheckBox;
            Assistant.Engine.MainWindow.MobFilterCheckBox.Checked = MobFilterCheckBox;
            Assistant.Engine.MainWindow.AutoCarverCheckBox.Checked = AutoCarverCheckBox;
            Assistant.Engine.MainWindow.BoneCutterCheckBox.Checked = BoneCutterCheckBox;
            Assistant.Engine.MainWindow.AutoCarverBladeLabel.Text = AutoCarverBladeLabel.ToString("X8");
            Assistant.Engine.MainWindow.BoneBladeLabel.Text = BoneBladeLabel.ToString("X8");
        }
	}
}
