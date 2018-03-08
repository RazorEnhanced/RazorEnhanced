using Assistant;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Forms;
using System.IO;

namespace RazorEnhanced
{
	public class Filters
	{
		////////////////////////////////////////////////////////////////
		/////////////////// START - FLAG HIGHLIGHT /////////////////////
		////////////////////////////////////////////////////////////////

		internal static void ProcessMessage(Assistant.Mobile m)
		{
			if (m.Serial == World.Player.Serial)      // Skip Self
				return;

			if (Assistant.Engine.MainWindow.FlagsHighlightCheckBox.Checked)
			{
				if (m.Poisoned)
					RazorEnhanced.Mobiles.MessageNoWait(m.Serial, 10, "[Poisoned]");
				if (m.IsGhost)
				{
					if (m.PropsUpdated)
						RazorEnhanced.Mobiles.MessageNoWait(m.Serial, 10, "[Dead]");
				}
				if (m.Paralized)
					RazorEnhanced.Mobiles.MessageNoWait(m.Serial, 10, "[Paralized]");
				if (m.Blessed)
					RazorEnhanced.Mobiles.MessageNoWait(m.Serial, 10, "[Mortalled]");
			}

			if (Assistant.Engine.MainWindow.HighlightTargetCheckBox.Checked)
			{
				if (Targeting.IsLastTarget(m))
					RazorEnhanced.Mobiles.MessageNoWait(m.Serial, 10, "*[Target]*");
			}
		}

		////////////////////////////////////////////////////////////////
		//////////////////// END - FLAG HIGHLIGHT //////////////////////
		////////////////////////////////////////////////////////////////


		////////////////////////////////////////////////////////////////
		/////////////////// START - GRAPH FILTER ///////////////////////
		////////////////////////////////////////////////////////////////
		[Serializable]
		public class GraphChangeData
		{
			private bool m_Selected;
			public bool Selected { get { return m_Selected; } }

			private int m_GraphReal;
			public int GraphReal { get { return m_GraphReal; } }

			private int m_GraphNew;
			public int GraphNew { get { return m_GraphNew; } }

			public GraphChangeData(bool selected, int graphreal, int graphnew)
			{
				m_Selected = selected;
				m_GraphReal = graphreal;
				m_GraphNew = graphnew;
			}
		}

		internal static void RefreshLists()
		{
			List<RazorEnhanced.Filters.GraphChangeData> graphdatas = Settings.GraphFilter.ReadAll();

			Assistant.Engine.MainWindow.MobFilterlistView.Items.Clear();

			foreach (RazorEnhanced.Filters.GraphChangeData graphdata in graphdatas)
			{
				ListViewItem listitem = new ListViewItem
				{
					Checked = graphdata.Selected
				};

				listitem.SubItems.Add("0x" + graphdata.GraphReal.ToString("X4"));
				listitem.SubItems.Add("0x" + graphdata.GraphNew.ToString("X4"));

				Assistant.Engine.MainWindow.MobFilterlistView.Items.Add(listitem);
			}
		}

		internal static void UpdateSelectedItems(int i)
		{
			List<RazorEnhanced.Filters.GraphChangeData> graphdatas = Settings.GraphFilter.ReadAll();

			if (graphdatas.Count != Assistant.Engine.MainWindow.MobFilterlistView.Items.Count)
			{
				return;
			}

			ListViewItem lvi = Assistant.Engine.MainWindow.MobFilterlistView.Items[i];
			GraphChangeData old = graphdatas[i];

			if (lvi != null && old != null)
			{
				GraphChangeData graph = new GraphChangeData(lvi.Checked, old.GraphReal, old.GraphNew);
				RazorEnhanced.Settings.GraphFilter.Replace(i, graph);
			}
		}

		////////////////////////////////////////////////////////////////
		/////////////////// END - GRAPH FILTER /////////////////////////
		////////////////////////////////////////////////////////////////


		////////////////////////////////////////////////////////////////
		///////////////////// START - AUTOCARVER ///////////////////////
		////////////////////////////////////////////////////////////////

		private static Queue<int> m_IgnoreCutCorpiQueue = new Queue<int>();
		private static bool m_AutoCarver;
		private static int m_carverblade;

		internal static int AutoCarverBlade
		{
			get { return m_carverblade; }

			set
			{
				m_carverblade = value;
				Assistant.Engine.MainWindow.AutoCarverBladeLabel.Invoke(new Action(() => Assistant.Engine.MainWindow.AutoCarverBladeLabel.Text = "0x" + value.ToString("X8")));
			}
		}

		internal static bool AutoCarver
		{
			get { return m_AutoCarver; }
			set { m_AutoCarver = value; }
		}

		internal static void AutoCarverEngine(Items.Filter filter)
		{
			if (!Assistant.Engine.Running)
				return;

			if (World.Player == null)       // Esce se non loggato
				return;

			if (m_carverblade == 0)       // Esce in caso di errore lettura blade
				return;

			List<Item> corpi = RazorEnhanced.Items.ApplyFilter(filter);

			foreach (RazorEnhanced.Item corpo in corpi)
			{
				if (!m_IgnoreCutCorpiQueue.Contains(corpo.Serial))
				{
					DragDropManager.CorpseToCutSerial.Enqueue(corpo.Serial);
					m_IgnoreCutCorpiQueue.Enqueue(corpo.Serial);
				}
			}
		}

		private static Items.Filter m_corpsefilter = new Items.Filter
		{
			RangeMax = 3,
			Movable = false,
			IsCorpse = 1,
			OnGround = 1,
			Enabled = true
		};

		internal static void CarveAutoRun()
		{
			AutoCarverEngine(m_corpsefilter);
		}

		////////////////////////////////////////////////////////////////
		/////////////////////// END - AUTOCARVER ///////////////////////
		////////////////////////////////////////////////////////////////

		////////////////////////////////////////////////////////////////
		///////////////////// START - BONE CUTTER //////////////////////
		////////////////////////////////////////////////////////////////

		private static bool m_BoneCutter;
		private static int m_bonecutterblade;

		internal static int BoneCutterBlade
		{
			get { return m_bonecutterblade; }

			set
			{
				m_bonecutterblade = value;
				Assistant.Engine.MainWindow.BoneBladeLabel.Invoke(new Action(() => Assistant.Engine.MainWindow.BoneBladeLabel.Text = "0x" + value.ToString("X8")));
			}
		}

		internal static bool BoneCutter
		{
			get { return m_BoneCutter; }
			set { m_BoneCutter = value; }
		}

		internal static void BoneCutterEngine(Items.Filter filter)
		{
			if (!Assistant.Engine.Running)
				return;

			if (World.Player == null)       // Esce se non loggato
				return;

			if (m_bonecutterblade == 0)       // Esce in caso di errore lettura blade
				return;

			List<Item> bones = RazorEnhanced.Items.ApplyFilter(filter);

			foreach (RazorEnhanced.Item bone in bones)
			{
				Target.Cancel();
				if (Items.FindBySerial(BoneCutterBlade) != null)
				{
					Items.UseItem(Items.FindBySerial(BoneCutterBlade));
					Target.WaitForTarget(1000, true);
					Target.TargetExecute(bone.Serial);
					Thread.Sleep(RazorEnhanced.Settings.General.ReadInt("ObjectDelay"));
				}
			}
		}

		private static Items.Filter m_bonefilter = new Items.Filter
		{
			Graphics = new List<int> { 0x3968, 0x0ECA, 0x0ECB, 0x0ECC, 0x0ECD, 0x0ECE, 0x0ECF, 0x0ED0, 0x0ED1, 0x0ED2 },
			RangeMax = 1,
			Movable = false,
			IsCorpse = -1,
			OnGround = 1,
			Enabled = true
		};

		internal static void BoneCutterRun()
		{
			if (ClientCommunication.ServerEncrypted)
				m_bonefilter.Movable = true;

			BoneCutterEngine(m_bonefilter);
		}

		////////////////////////////////////////////////////////////////
		/////////////////////// END - BONE CUTTER //////////////////////
		////////////////////////////////////////////////////////////////

		////////////////////////////////////////////////////////////////
		///////////////////// START - AUTO REMOUNT /////////////////////
		////////////////////////////////////////////////////////////////
		private static bool m_AutoModeRemount;
		private static int m_autoremountdelay;
		private static int m_autoremountedelay;
		private static int m_autoremountserial;

		internal static int AutoRemountDelay
		{
			get { return m_autoremountdelay; }

			set
			{
				m_autoremountdelay = value;
				Assistant.Engine.MainWindow.RemountDelay.Invoke(new Action(() => Assistant.Engine.MainWindow.RemountDelay.Text = value.ToString()));
			}
		}

		internal static int AutoRemountEDelay
		{
			get { return m_autoremountedelay; }

			set
			{
				m_autoremountedelay = value;
				Assistant.Engine.MainWindow.RemountEDelay.Invoke(new Action(() => Assistant.Engine.MainWindow.RemountEDelay.Text = value.ToString()));
			}
		}

		internal static int AutoRemountSerial
		{
			get { return m_autoremountserial; }

			set
			{
				m_autoremountserial = value;
				Assistant.Engine.MainWindow.RemountSerialLabel.Invoke(new Action(() => Assistant.Engine.MainWindow.RemountSerialLabel.Text = "0x" + value.ToString("X8")));
			}
		}

		internal static bool AutoModeRemount
		{
			get { return m_AutoModeRemount; }
			set { m_AutoModeRemount = value; }
		}

		internal static void RemountAutoRun()
		{
			if (World.Player == null)
				return;

			if (m_autoremountserial == 0)
				return;

			if (World.Player.IsGhost)
				return;

			if (World.Player.GetItemOnLayer(Layer.Mount) != null)   // Gia su mount
				return;

			Assistant.Item etheralMount = Assistant.World.FindItem(m_autoremountserial);
			if (etheralMount != null && etheralMount.Serial.IsItem)
			{
				Items.UseItem(m_autoremountserial);
				Thread.Sleep(m_autoremountedelay);
			}
			else
			{
				Assistant.Mobile mount = Assistant.World.FindMobile(m_autoremountserial);
				if (mount != null && mount.Serial.IsMobile)
				{
					RazorEnhanced.Mobiles.UseMobile(m_autoremountserial);
					Thread.Sleep(m_autoremountdelay);
				}
			}
		}

		////////////////////////////////////////////////////////////////
		/////////////////////// END - AUTO REMOUNT /////////////////////
		////////////////////////////////////////////////////////////////


		////////////////////////////////////////////////////////////////
		///////////////////// START - FLAG COLOR ///////////////////////
		////////////////////////////////////////////////////////////////

		// COLORI FLAG HIGHLIGHT //
		internal enum HighLightColor : ushort
		{
			Poison = 0x0042,
			Paralized = 0x013C,
			Mortal = 0x002E,
			BloodOath = 0x0038
		}

		private static List<Assistant.Layer> m_colorized_layer = new List<Layer>
		{
			Layer.Invalid,
			Layer.FirstValid,
			Layer.RightHand,
			Layer.LeftHand,
			Layer.Shoes,
			Layer.Pants,
			Layer.Shirt,
			Layer.Head,
			Layer.Neck,
			Layer.Gloves,
			Layer.InnerTorso,
			Layer.MiddleTorso,
			Layer.Arms,
			Layer.Cloak,
			Layer.OuterTorso,
			Layer.OuterLegs,
			Layer.InnerLegs,
			Layer.LastUserValid,
			Layer.Mount,
			Layer.LastValid,
		};

		internal static void Decolorize(Assistant.Mobile m)
		{
			foreach (Assistant.Layer l in m_colorized_layer)
			{
				Assistant.Item i = m.GetItemOnLayer(l);
				if (i == null)
					continue;

				ClientCommunication.SendToClient(new EquipmentItem(i, i.Hue, m.Serial));
			}
		}
		internal static void ApplyColor(Assistant.Mobile m, bool isbloodoath = false)
		{
			int color = 0;
			if (m.Poisoned)
				color = (int)HighLightColor.Poison;
			else if (m.Paralized)
				color = (int)HighLightColor.Paralized;
			else if (m.Blessed) // Mortal
				color = (int)HighLightColor.Mortal;
			else if (isbloodoath)
				color = (int)HighLightColor.BloodOath;
			else
			{
				Decolorize(m);
				return;
			}

			// Apply color for valid flag
			foreach (Assistant.Layer l in m_colorized_layer)
			{
				Assistant.Item i = m.GetItemOnLayer(l);
				if (i == null)
					continue;

				ClientCommunication.SendToClient(new EquipmentItem(i, (ushort)color, m.Serial));
			}
		}

		internal static Packet MobileColorize(Packet p, Assistant.Mobile m)
		{
			int ltHue = RazorEnhanced.Settings.General.ReadInt("LTHilight");
			if (ltHue != 0 && Targeting.IsLastTarget(m))
				p = RewriteColorAndFlag(p, (ushort)ltHue);

			else
			{
				// Blocco Color Highlight flag
				if (RazorEnhanced.Settings.General.ReadBool("ColorFlagsHighlightCheckBox"))
				{
					if (m.Poisoned)
						p = RewriteColorAndFlag(p, (ushort)HighLightColor.Poison);

					else if (m.Paralized)
						p = RewriteColorAndFlag(p, (ushort)HighLightColor.Paralized);

					else if (m.Blessed) // Mortal
						p = RewriteColorAndFlag(p, (ushort)HighLightColor.Mortal);
				}
			}
			return p;
		}

		internal static Packet MobileIncomingItemColorize(Packet p, Assistant.Mobile m, bool newmobincoming, Assistant.Item item = null)
		{
			int ltHue = Settings.General.ReadInt("LTHilight");
			if (newmobincoming)
			{
				if (ltHue != 0 && Targeting.IsLastTarget(m))
					p = RewriteColor(p, (ushort)ltHue);
				else
				{
					// Blocco Color Highlight flag
					if (Settings.General.ReadBool("ColorFlagsHighlightCheckBox"))
					{
						if (m.Poisoned)
							p = RewriteColor(p, (ushort)HighLightColor.Poison);

						else if (m.Paralized)
							p = RewriteColor(p, (ushort)HighLightColor.Paralized);

						else if (m.Blessed) // Mortal
							p = RewriteColor(p, (ushort)HighLightColor.Mortal);
					}
				}
			}
			else
			{
				if (ltHue != 0 && Targeting.IsLastTarget(m))
				{
					ClientCommunication.SendToClient(new EquipmentItem(item, (ushort)(ltHue & 16383), m.Serial));
				}
				else
				{
					int color = 0;
					if (m.Poisoned)
						color = (int)HighLightColor.Poison;
					else if (m.Paralized)
						color = (int)HighLightColor.Paralized;
					else if (m.Blessed) // Mortal
						color = (int)HighLightColor.Mortal;

					if (color != 0)
						ClientCommunication.SendToClient(new EquipmentItem(item, (ushort)color, m.Serial));
				}
			}
			return p;
		}

		internal static Packet EquipmentUpdateColorize(Packet p, Assistant.Item i)
		{
			int ltHue = Settings.General.ReadInt("LTHilight");
			if (ltHue != 0 && Targeting.IsLastTarget(i.Container as Assistant.Mobile))
				p = RewriteColor(p, (ushort)ltHue);
			else
			{
				// Blocco Color Highlight flag
				if (Settings.General.ReadBool("ColorFlagsHighlightCheckBox"))
				{
					if ((i.Container as Assistant.Mobile) != null && (i.Container as Assistant.Mobile).Poisoned)
						p = RewriteColor(p, (ushort)HighLightColor.Poison);

					else if ((i.Container as Assistant.Mobile) != null && (i.Container as Assistant.Mobile).Paralized)
						p = RewriteColor(p, (ushort)HighLightColor.Paralized);

					else if ((i.Container as Assistant.Mobile) != null && (i.Container as Assistant.Mobile).Blessed) // Mortal
						p = RewriteColor(p, (ushort)HighLightColor.Mortal);
				}
			}
			return p;
		}

		private static Packet RewriteColor(Packet p, ushort color)
		{
			p.Seek(-2, SeekOrigin.Current);
			p.Write(color);
			return p;
		}
		private static Packet RewriteColorAndFlag(Packet p, ushort color)
		{
			p.Seek(-3, SeekOrigin.Current);
			p.Write((short)color);
			p.Seek(+1, SeekOrigin.Current);
			return p;
		}


		////////////////////////////////////////////////////////////////
		///////////////////// END -  FLAG COLOR ////////////////////////
		////////////////////////////////////////////////////////////////


		//////////////// Load settings ////////////////
		internal static void LoadSettings()
		{
			Engine.MainWindow.HighlightTargetCheckBox.Checked = Settings.General.ReadBool("HighlightTargetCheckBox");
			Engine.MainWindow.FlagsHighlightCheckBox.Checked = Settings.General.ReadBool("FlagsHighlightCheckBox");
			Engine.MainWindow.ShowStaticFieldCheckBox.Checked = Settings.General.ReadBool("ShowStaticFieldCheckBox");
			Engine.MainWindow.BlockTradeRequestCheckBox.Checked = Settings.General.ReadBool("BlockTradeRequestCheckBox");
			Engine.MainWindow.BlockPartyInviteCheckBox.Checked = Settings.General.ReadBool("BlockPartyInviteCheckBox");
			Engine.MainWindow.MobFilterCheckBox.Checked = Settings.General.ReadBool("MobFilterCheckBox");
			Engine.MainWindow.AutoCarverCheckBox.Checked = Settings.General.ReadBool("AutoCarverCheckBox");
			Engine.MainWindow.BoneCutterCheckBox.Checked = Settings.General.ReadBool("BoneCutterCheckBox");
			AutoCarverBlade = Settings.General.ReadInt("AutoCarverBladeLabel");
			BoneCutterBlade = Settings.General.ReadInt("BoneBladeLabel");
			Engine.MainWindow.RemountCheckbox.Checked = Settings.General.ReadBool("RemountCheckbox");
			Engine.MainWindow.ShowHeadTargetCheckBox.Checked = Settings.General.ReadBool("ShowHeadTargetCheckBox");
			Engine.MainWindow.BlockHealPoisonCheckBox.Checked = Settings.General.ReadBool("BlockHealPoison");
			Engine.MainWindow.BlockChivalryHealCheckBox.Checked = Settings.General.ReadBool("BlockChivalryHealCheckBox");
			Engine.MainWindow.BlockBigHealCheckBox.Checked = Settings.General.ReadBool("BlockBigHealCheckBox");
			Engine.MainWindow.BlockMiniHealCheckBox.Checked = Settings.General.ReadBool("BlockMiniHealCheckBox");
			Engine.MainWindow.ColorFlagsHighlightCheckBox.Checked = Settings.General.ReadBool("ColorFlagsHighlightCheckBox");
			Engine.MainWindow.ShowMessageFieldCheckBox.Checked = Settings.General.ReadBool("ShowMessageFieldCheckBox");
			Engine.MainWindow.ShowAgentMessageCheckBox.Checked = Settings.General.ReadBool("ShowAgentMessageCheckBox");
			Engine.MainWindow.ColorFlagsSelfHighlightCheckBox.Checked = Settings.General.ReadBool("ColorFlagsSelfHighlightCheckBox");
			AutoRemountDelay = Settings.General.ReadInt("MountDelay");
			AutoRemountEDelay = Settings.General.ReadInt("EMountDelay");
			AutoRemountSerial = Settings.General.ReadInt("MountSerial");

			RefreshLists();
		}
	}
}