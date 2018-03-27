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
		/////////////////// START - STATIC FIELD ///////////////////////
		const ushort WallStaticID = 0x28A8;
		const ushort WallStaticIDStone = 0x0750;

		internal enum WallColor : ushort
		{
			Stone = 0x3B1,
			Fire = 0x0845,
			Poison = 0x016A,
			Paralyze = 0x00DA,
			Energy = 0x0125
		}
		internal static bool MakeWallStatic(Assistant.Item wall)
		{
			switch (wall.ItemID)
			{
				case 0x0080:
				case 0x0082:
					wall.ItemID = WallStaticIDStone;
					wall.Hue = (ushort)WallColor.Stone;
					ClientCommunication.SendToClient(new WorldItem(wall));
					if (Engine.MainWindow.ShowMessageFieldCheckBox.Checked)
						ClientCommunication.SendToClient(new UnicodeMessage(wall.Serial, wall.ItemID, MessageType.Regular, (ushort)WallColor.Stone, 3, Language.CliLocName, wall.Name, "[Wall Of Stone]"));
					return true;
				case 0x3996:
				case 0x398C:
					wall.ItemID = WallStaticID;
					wall.Hue = (ushort)WallColor.Fire;
					ClientCommunication.SendToClient(new WorldItem(wall));
					if (Engine.MainWindow.ShowMessageFieldCheckBox.Checked)
						ClientCommunication.SendToClient(new UnicodeMessage(wall.Serial, wall.ItemID, MessageType.Regular, (ushort)WallColor.Fire, 3, Language.CliLocName, wall.Name, "[Fire Field]"));
					return true;
				case 0x3915:
				case 0x3920:
				case 0x3922:
					wall.ItemID = WallStaticID;
					wall.Hue = (ushort)WallColor.Poison;
					ClientCommunication.SendToClient(new WorldItem(wall));
					if (Engine.MainWindow.ShowMessageFieldCheckBox.Checked)
						ClientCommunication.SendToClient(new UnicodeMessage(wall.Serial, wall.ItemID, MessageType.Regular, (ushort)WallColor.Poison, 3, Language.CliLocName, wall.Name, "[Poison Field]"));
					return true;
				case 0x3967:
				case 0x3979:
					wall.ItemID = WallStaticID;
					wall.Hue = (ushort)WallColor.Paralyze;
					ClientCommunication.SendToClient(new WorldItem(wall));
					if (Engine.MainWindow.ShowMessageFieldCheckBox.Checked)
						ClientCommunication.SendToClient(new UnicodeMessage(wall.Serial, wall.ItemID, MessageType.Regular, (ushort)WallColor.Paralyze, 3, Language.CliLocName, wall.Name, "[Paralyze Field]"));
					return true;
				case 0x3946:
				case 0x3956:
					wall.ItemID = WallStaticID;
					wall.Hue = (ushort)WallColor.Energy;
					ClientCommunication.SendToClient(new WorldItem(wall));
					if (Engine.MainWindow.ShowMessageFieldCheckBox.Checked)
						ClientCommunication.SendToClient(new UnicodeMessage(wall.Serial, wall.ItemID, MessageType.Regular, (ushort)WallColor.Energy, 3, Language.CliLocName, wall.Name, "[Energy Field]"));
					return true;
				default:
					return false;
			}
		}
		/////////////////// END - STATIC FIELD /////////////////////////


		/////////////////// START - FLAG HIGHLIGHT /////////////////////
		internal static void ProcessMessage(Assistant.Mobile m)
		{
			if (m.Serial == World.Player.Serial)      // Skip Self
				return;

			if (Engine.MainWindow.FlagsHighlightCheckBox.Checked)
			{
				if (m.Poisoned)
					Mobiles.Message(m.Serial, 10, "[Poisoned]", false);
				if (m.IsGhost)
				{
					if (m.PropsUpdated)
						Mobiles.Message(m.Serial, 10, "[Dead]", false);
				}
				if (m.Paralized)
					Mobiles.Message(m.Serial, 10, "[Paralized]", false);
				if (m.Blessed)
					Mobiles.Message(m.Serial, 10, "[Mortalled]", false);
			}

		/*	if (Engine.MainWindow.HighlightTargetCheckBox.Checked)
			{
				if (Targeting.IsLastTarget(m))
					Mobiles.Message(m.Serial, 10, "*[Target]*", false);
			}*/
		}
		//////////////////// END - FLAG HIGHLIGHT //////////////////////


		/////////////////// START - GRAPH FILTER ///////////////////////
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
			List<GraphChangeData> graphdatas = Settings.GraphFilter.ReadAll();

			Engine.MainWindow.MobFilterlistView.Items.Clear();

			foreach (GraphChangeData graphdata in graphdatas)
			{
				ListViewItem listitem = new ListViewItem
				{
					Checked = graphdata.Selected
				};

				listitem.SubItems.Add("0x" + graphdata.GraphReal.ToString("X4"));
				listitem.SubItems.Add("0x" + graphdata.GraphNew.ToString("X4"));

				Engine.MainWindow.MobFilterlistView.Items.Add(listitem);
			}
		}

		internal static void UpdateSelectedItems(int i)
		{
			List<GraphChangeData> graphdatas = Settings.GraphFilter.ReadAll();

			if (graphdatas.Count != Engine.MainWindow.MobFilterlistView.Items.Count)
			{
				return;
			}

			ListViewItem lvi = Engine.MainWindow.MobFilterlistView.Items[i];
			GraphChangeData old = graphdatas[i];

			if (lvi != null && old != null)
			{
				GraphChangeData graph = new GraphChangeData(lvi.Checked, old.GraphReal, old.GraphNew);
				Settings.GraphFilter.Replace(i, graph);
			}
		}

		internal static Packet GraphChange(Packet p, ushort body)
		{
			List<GraphChangeData> graphdatas = Settings.GraphFilter.ReadAll();
			foreach (GraphChangeData graphdata in graphdatas)
			{
				if (body != graphdata.GraphReal)
					continue;

				p.Seek(-2, SeekOrigin.Current);
				p.Write((ushort)(graphdata.GraphNew));
				break;
			}
			return p;
		}
		/////////////////// END - GRAPH FILTER /////////////////////////


		///////////////////// START - AUTOCARVER ///////////////////////
		private static Queue<int> m_IgnoreCutCorpiQueue = new Queue<int>();
		private static bool m_AutoCarver;
		private static int m_carverblade;

		internal static int AutoCarverBlade
		{
			get { return m_carverblade; }

			set
			{
				m_carverblade = value;
				Engine.MainWindow.AutoCarverBladeLabel.Invoke(new Action(() => Engine.MainWindow.AutoCarverBladeLabel.Text = "0x" + value.ToString("X8")));
			}
		}

		internal static bool AutoCarver
		{
			get { return m_AutoCarver; }
			set { m_AutoCarver = value; }
		}

		internal static void AutoCarverEngine(Items.Filter filter)
		{
			if (!Engine.Running)
				return;

			if (World.Player == null)       // Esce se non loggato
				return;

			if (m_carverblade == 0)       // Esce in caso di errore lettura blade
				return;

			List<Item> corpi = Items.ApplyFilter(filter);

			foreach (Item corpo in corpi)
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
		/////////////////////// END - AUTOCARVER ///////////////////////


		///////////////////// START - BONE CUTTER //////////////////////
		private static bool m_BoneCutter;
		private static int m_bonecutterblade;

		internal static int BoneCutterBlade
		{
			get { return m_bonecutterblade; }

			set
			{
				m_bonecutterblade = value;
				Engine.MainWindow.BoneBladeLabel.Invoke(new Action(() => Engine.MainWindow.BoneBladeLabel.Text = "0x" + value.ToString("X8")));
			}
		}

		internal static bool BoneCutter
		{
			get { return m_BoneCutter; }
			set { m_BoneCutter = value; }
		}

		internal static void BoneCutterEngine(Items.Filter filter)
		{
			if (!Engine.Running)
				return;

			if (World.Player == null)       // Esce se non loggato
				return;

			if (m_bonecutterblade == 0)       // Esce in caso di errore lettura blade
				return;

			List<Item> bones = Items.ApplyFilter(filter);

			foreach (Item bone in bones)
			{
				Target.Cancel();
				if (Items.FindBySerial(BoneCutterBlade) != null)
				{
					Items.UseItem(Items.FindBySerial(BoneCutterBlade));
					Target.WaitForTarget(1000, true);
					Target.TargetExecute(bone.Serial);
					Thread.Sleep(Settings.General.ReadInt("ObjectDelay"));
				}
			}
		}

		private static Items.Filter m_bonefilter = new Items.Filter
		{
			Graphics = new List<int> { 0x0ECA, 0x0ECB, 0x0ECC, 0x0ECD, 0x0ECE, 0x0ECF, 0x0ED0, 0x0ED1, 0x0ED2 },
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
		/////////////////////// END - BONE CUTTER //////////////////////


		///////////////////// START - AUTO REMOUNT /////////////////////
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
				Engine.MainWindow.RemountDelay.Invoke(new Action(() => Assistant.Engine.MainWindow.RemountDelay.Text = value.ToString()));
			}
		}

		internal static int AutoRemountEDelay
		{
			get { return m_autoremountedelay; }

			set
			{
				m_autoremountedelay = value;
				Engine.MainWindow.RemountEDelay.Invoke(new Action(() => Assistant.Engine.MainWindow.RemountEDelay.Text = value.ToString()));
			}
		}

		internal static int AutoRemountSerial
		{
			get { return m_autoremountserial; }

			set
			{
				m_autoremountserial = value;
				Engine.MainWindow.RemountSerialLabel.Invoke(new Action(() => Assistant.Engine.MainWindow.RemountSerialLabel.Text = "0x" + value.ToString("X8")));
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
					Mobiles.UseMobile(m_autoremountserial);
					Thread.Sleep(m_autoremountdelay);
				}
			}
		}
		/////////////////////// END - AUTO REMOUNT /////////////////////


		///////////////////// START - FLAG COLOR ///////////////////////
		internal enum HighLightColor : ushort
		{
			Poison = 0x0042,
			Paralized = 0x013C,
			Mortal = 0x002E,
			BloodOath = 0x0026
		}

		private static List<Assistant.Layer> m_colorized_layer = new List<Layer>
		{
			Layer.Backpack,
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
			Layer.Hair
		};

		internal static void Decolorize(Assistant.Mobile m)
		{
			if (m.IsGhost) // Non eseguire azione se fantasma
				return;

			foreach (Layer l in m_colorized_layer)
			{
				Assistant.Item i = m.GetItemOnLayer(l);
				if (i == null)
					continue;

				if (i.ItemID == 0x204E && i.Hue == 0x08FD) // Death Shround
					i.ItemID = 0x1F03;

				ClientCommunication.SendToClient(new EquipmentItem(i, i.Hue, m.Serial));
			}
		}
		internal static void ApplyColor(Assistant.Mobile m)
		{
			if (m.IsGhost) // Non eseguire azione se fantasma
				return;

			int color = 0;
			if (m.Poisoned)
				color = (int)HighLightColor.Poison;
			else if (m.Paralized)
				color = (int)HighLightColor.Paralized;
			else if (m.Blessed) // Mortal
				color = (int)HighLightColor.Mortal;
			else if (m == World.Player && Player.BuffsExist("Bload Oath (curse)"))
				color = (int)HighLightColor.BloodOath;
			else
			{
				Decolorize(m);
				return;
			}

			// Apply color for valid flag
			foreach (Layer l in m_colorized_layer)
			{
				Assistant.Item i = m.GetItemOnLayer(l);
				if (i == null)
					continue;

				ClientCommunication.SendToClient(new EquipmentItem(i, (ushort)color, m.Serial));
			}
		}

		internal static Packet MobileColorize(Packet p, Assistant.Mobile m)
		{
			if (m.IsGhost) // Non eseguire azione se fantasma
				return p;

			int ltHue = Engine.MainWindow.LTHilight;
			if (ltHue != 0 && Targeting.IsLastTarget(m))
				p = RewriteColorAndFlag(p, (ushort)ltHue);

			else
			{
				// Blocco Color Highlight flag
				if ((m != World.Player && Engine.MainWindow.ColorFlagsHighlightCheckBox.Checked) || (m == World.Player && Engine.MainWindow.ColorFlagsSelfHighlightCheckBox.Checked))
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
			int ltHue = Engine.MainWindow.LTHilight;
			if (newmobincoming)
			{
				if (ltHue != 0 && Targeting.IsLastTarget(m))
					p = RewriteColor(p, (ushort)ltHue);
				else
				{
					// Blocco Color Highlight flag
					if ((m != World.Player && Engine.MainWindow.ColorFlagsHighlightCheckBox.Checked) || (m == World.Player && Engine.MainWindow.ColorFlagsSelfHighlightCheckBox.Checked))
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
				if (m.IsGhost) // Non eseguire azione se fantasma
					return p;

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
			int ltHue = Engine.MainWindow.LTHilight;
			if (ltHue != 0 && Targeting.IsLastTarget(i.Container as Assistant.Mobile))
				p = RewriteColor(p, (ushort)ltHue);
			else
			{
				Assistant.Mobile m = (i.Container as Assistant.Mobile);
				if (m != null && !m.IsGhost)
				{
					// Blocco Color Highlight flag
					if (Engine.MainWindow.ColorFlagsHighlightCheckBox.Checked || Engine.MainWindow.ColorFlagsSelfHighlightCheckBox.Checked)
					{
						if (m.Poisoned)
							p = RewriteColor(p, (ushort)HighLightColor.Poison);

						else if (m.Paralized)
							p = RewriteColor(p, (ushort)HighLightColor.Paralized);

						else if (m.Blessed) // Mortal
							p = RewriteColor(p, (ushort)HighLightColor.Mortal);

						else if (m == World.Player && Player.BuffsExist("Bload Oath (curse)"))
							p = RewriteColor(p, (ushort)HighLightColor.BloodOath);
					}
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
		///////////////////// END -  FLAG COLOR ////////////////////////


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