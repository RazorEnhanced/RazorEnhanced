using Assistant;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Windows.Forms;
using System.Diagnostics;
using Assistant.UI;
using System;

namespace RazorEnhanced
{
	public enum ModKeys : int
	{
		None = 0,
		Alt = 0x0001,
		Control = 0x0002,
		Shift = 0x0004
	}

    public class HotKeyEvent
    {
        public static HotKeyEvent LastEvent;

        public static DateTime UnixTimeBegin = new DateTime(1970, 1, 1);
        public Keys HotKey;
        public double Timestamp;

        public static HotKeyEvent AddEvent(Keys k) {
            LastEvent = new HotKeyEvent(k);
            return LastEvent;
        }

        public HotKeyEvent(Keys key)
        {
            this.HotKey = key;
            this.Timestamp = DateTime.Now.Subtract(UnixTimeBegin).TotalSeconds;
        }

    }

    internal class HotKey
    {
        

        public class HotKeyData
        {
            private string m_Name;
            public string Name { get { return m_Name; } }

            private Keys m_Key;
            public Keys Key { get { return m_Key; } }

            public HotKeyData(string name, Keys key)
            {
                m_Name = name;
                m_Key = key;
            }
        }

        
        

		internal static Keys NormalKey { get { return m_key; } set { m_key = value; } }
		private static Keys m_key;
		internal static Keys MasterKey { get { return m_masterkey; } set { m_masterkey = value; } }
		private static Keys m_masterkey;

		internal static void OnMouse(int button, int wheel)
		{
            if (World.Player == null)
				return;

			switch (button)
			{
				case 0:
					{
						if (wheel == -1)
							KeyDown((Keys)501 | Control.ModifierKeys);
						else
							KeyDown((Keys)502 | Control.ModifierKeys);
						break;
					}
				case 1:
					KeyDown((Keys)500 | Control.ModifierKeys);
					break;

				case 2:
					KeyDown((Keys)503 | Control.ModifierKeys);
					break;

				case 3:
					KeyDown((Keys)504 | Control.ModifierKeys);
					break;
			}
		}

		internal static bool OnKeyDown(int key, ModKeys mod)
		{
			Debug.WriteLine("key: 0x{0:X} mod: 0x{0:X} ", key, mod);
			Keys k = (Keys)key;
			if (mod.HasFlag(ModKeys.Control))
			{
				k |= Keys.Control;
			}
			if (mod.HasFlag(ModKeys.Alt))
			{
				k |= Keys.Alt;
			}
			if (mod.HasFlag(ModKeys.Shift))
			{
				k |= Keys.Shift;
			}
			return KeyDown(k);
		}

		internal static bool KeyDown(Keys k)
		{
            HotKeyEvent.AddEvent(k);

			Debug.WriteLine("KD Keys: 0x{0:X}", k);
			bool hotTextFocused = false;
			bool hotTextMasterFocused = false;
			Engine.MainWindow.SafeAction(s => hotTextFocused = s.HotKeyTextBox.Focused);
			Engine.MainWindow.SafeAction(s => hotTextMasterFocused = s.HotKeyKeyMasterTextBox.Focused);
			if (!hotTextFocused && !hotTextMasterFocused)
			{
				if (k == RazorEnhanced.Settings.General.ReadKey("HotKeyMasterKey"))         // Pressione master key abilita o disabilita
				{
					if (RazorEnhanced.Settings.General.ReadBool("HotKeyEnable"))
					{
						RazorEnhanced.Settings.General.WriteBool("HotKeyEnable", false);
						Engine.MainWindow.SafeAction(s => s.HotKeyStatusLabel.Text = "Status: Disable");
						if (World.Player != null)
							RazorEnhanced.Misc.SendMessage("HotKey: DISABLED", 37, false);
					}
					else
					{
						Engine.MainWindow.SafeAction(s => s.HotKeyStatusLabel.Text = "Status: Enable");
						RazorEnhanced.Settings.General.WriteBool("HotKeyEnable", true);
						if (World.Player != null)
							RazorEnhanced.Misc.SendMessage("HotKey: ENABLED", 168, false);
					}
				}
			}

			if (hotTextFocused)                // In caso di assegnazione hotKey normale
			{
				m_key = k;
				//Engine.MainWindow.HotKeyTextBox.Text = KeyString(k);
				Engine.MainWindow.SafeAction(s => s.HotKeyTextBox.Text = KeyString(k));
				return false;
			}
			else if (hotTextMasterFocused)                // In caso di assegnazione hotKey primaria
			{
				m_masterkey = k;
				//Engine.MainWindow.HotKeyKeyMasterTextBox.Text = KeyString(k);
				Engine.MainWindow.SafeAction(s => s.HotKeyKeyMasterTextBox.Text = KeyString(k));
				return false;
			}
			else    // Esecuzine reale
			{
				if (World.Player != null && World.Player.WalkScriptRequest >= 1 && (k == Keys.Up || k == Keys.Down || k == Keys.Left || k == Keys.Right
					|| k == Keys.PageUp || k == Keys.PageDown || k == Keys.Home || k == Keys.End))
					return true; // Check for prevent new pathfind system trigger hotkey if assignet to move key

				if (World.Player != null && RazorEnhanced.Settings.General.ReadBool("HotKeyEnable"))
				{
					string group = string.Empty;
					RazorEnhanced.Settings.HotKey.FindGroup(k, out group, out bool passkey);
					ProcessGroup(group, k);
					return passkey;
				}
				return true;
			}
		}

		internal static string KeyString(Keys k)
		{
			switch (k)
			{
				case (Keys)500:
					return "Wheel Click";

				case (Keys)501:
					return "Wheel Down";

				case (Keys)502:
					return "Wheel Up";

				case (Keys)503:
					return "X Button 1";

				case (Keys)504:
					return "X Button 2";

				// Mouse piu tasti
				case (Keys)131572:
					return "Wheel Click, Control";

				case (Keys)131573:
					return "Wheel Down, Control";

				case (Keys)131574:
					return "Wheel Up, Control";

				case (Keys)131575:
					return "X Button 1, Control";

				case (Keys)131576:
					return "X Button 2, Control";

				case (Keys)66036:
					return "Wheel Click, Shift";

				case (Keys)66037:
					return "Wheel Down, Shift";

				case (Keys)66038:
					return "Wheel Up, Shift";

				case (Keys)66039:
					return "X Button 1, Shift";

				case (Keys)66040:
					return "X Button 2, Shift";

				case (Keys)262644:
					return "Wheel Click, Alt";

				case (Keys)262645:
					return "Wheel Down, Alt";

				case (Keys)262646:
					return "Wheel Up, Alt";

				case (Keys)262647:
					return "X Button 1, Alt";

				case (Keys)262648:
					return "X Button 2, Alt";

				default:
					return k.ToString();
			}
		}

		private static void ProcessGroup(string group, Keys k)
		{
			if (group != string.Empty)
			{
				switch (group)
				{
					case "General":
						ProcessGeneral(RazorEnhanced.Settings.HotKey.FindString(k));
						break;

					case "Actions":
						ProcessActions(RazorEnhanced.Settings.HotKey.FindString(k));
						break;

					case "Use":
						ProcessUse(RazorEnhanced.Settings.HotKey.FindString(k));
						break;

					case "Show Names":
						ProcessShowName(RazorEnhanced.Settings.HotKey.FindString(k));
						break;

					case "Pet Commands":
						ProcessPet(RazorEnhanced.Settings.HotKey.FindString(k));
						break;

					case "AgentAutoloot":
						ProcessAgentsAutoloot(RazorEnhanced.Settings.HotKey.FindString(k));
						break;

					case "AgentScavenger":
						ProcessAgentsScavenger(RazorEnhanced.Settings.HotKey.FindString(k));
						break;

					case "AgentOrganizer":
						ProcessAgentsOrganizer(RazorEnhanced.Settings.HotKey.FindString(k));
						break;

					case "AgentSell":
						ProcessAgentsSell(RazorEnhanced.Settings.HotKey.FindString(k));
						break;

					case "AgentBuy":
						ProcessAgentBuy(RazorEnhanced.Settings.HotKey.FindString(k));
						break;

					case "AgentDress":
						ProcessAgentDress(RazorEnhanced.Settings.HotKey.FindString(k));
						break;

					case "AgentRestock":
						ProcessAgentRestock(RazorEnhanced.Settings.HotKey.FindString(k));
						break;

					case "AgentBandage":
						ProcessAgentBandage(RazorEnhanced.Settings.HotKey.FindString(k));
						break;

					case "AgentBoneCutter":
						ProcessAgentBoneCutter(RazorEnhanced.Settings.HotKey.FindString(k));
						break;

					case "AgentAutoCarver":
						ProcessAgentAutoCarver(RazorEnhanced.Settings.HotKey.FindString(k));
						break;

					case "AgentAutoRemount":
						ProcessAgentAutoRemount(RazorEnhanced.Settings.HotKey.FindString(k));
						break;

					case "AgentGraphFilter":
						ProcessAgentGraphFilter(RazorEnhanced.Settings.HotKey.FindString(k));
						break;

					case "AgentFriend":
						ProcessAgentFriend(RazorEnhanced.Settings.HotKey.FindString(k));
						break;

					case "Abilities":
						ProcessAbilities(RazorEnhanced.Settings.HotKey.FindString(k));
						break;

					case "Attack":
						ProcessAttack(RazorEnhanced.Settings.HotKey.FindString(k));
						break;

					case "Bandage":
						ProcessBandage(RazorEnhanced.Settings.HotKey.FindString(k));
						break;

					case "Potions":
						ProcessPotions(RazorEnhanced.Settings.HotKey.FindString(k));
						break;

					case "Other":
						ProcessOther(RazorEnhanced.Settings.HotKey.FindString(k));
						break;

					case "Hands":
						ProcessHands(RazorEnhanced.Settings.HotKey.FindString(k));
						break;

					case "Equip Wands":
						ProcessEquipWands(RazorEnhanced.Settings.HotKey.FindString(k));
						break;

					case "Skills":
						ProcessSkills(RazorEnhanced.Settings.HotKey.FindString(k));
						break;

					case "SpellsAgent":
						ProcessSpellsAgent(RazorEnhanced.Settings.HotKey.FindString(k));
						break;

					case "SpellsMagery":
						RazorEnhanced.Spells.CastOnlyMagery(RazorEnhanced.Settings.HotKey.FindString(k), false);
						break;

					case "SpellsNecro":
						RazorEnhanced.Spells.CastOnlyNecro(RazorEnhanced.Settings.HotKey.FindString(k), false);
						break;

					case "SpellsBushido":
						RazorEnhanced.Spells.CastOnlyBushido(RazorEnhanced.Settings.HotKey.FindString(k), false);
						break;

					case "SpellsNinjitsu":
						RazorEnhanced.Spells.CastOnlyNinjitsu(RazorEnhanced.Settings.HotKey.FindString(k), false);
						break;

					case "SpellsSpellweaving":
						RazorEnhanced.Spells.CastOnlySpellweaving(RazorEnhanced.Settings.HotKey.FindString(k), false);
						break;

					case "SpellsMysticism":
						RazorEnhanced.Spells.CastOnlyMysticism(RazorEnhanced.Settings.HotKey.FindString(k), false);
						break;

					case "SpellsChivalry":
						RazorEnhanced.Spells.CastOnlyChivalry(RazorEnhanced.Settings.HotKey.FindString(k), false);
						break;

					case "SpellsMastery":
						RazorEnhanced.Spells.CastOnlyMastery(RazorEnhanced.Settings.HotKey.FindString(k), false);
						break;

                    case "SpellsCleric":
                        RazorEnhanced.Spells.CastOnlyCleric(RazorEnhanced.Settings.HotKey.FindString(k), false);
                        break;

                    case "SpellsDruid":
                        RazorEnhanced.Spells.CastOnlyDruid(RazorEnhanced.Settings.HotKey.FindString(k), false);
                        break;

                    case "Target":
						ProcessTarget(RazorEnhanced.Settings.HotKey.FindString(k));
						break;

					case "TList":
						RazorEnhanced.Target.SetLastTargetFromListHotKey(RazorEnhanced.Settings.HotKey.FindTargetString(k));
						break;

					case "Script":
						if (RazorEnhanced.Settings.HotKey.FindString(k) == "Stop All")
						{
							RazorEnhanced.Misc.SendMessage("Stopping all scripts...",33, false);
							foreach (RazorEnhanced.Scripts.EnhancedScript scriptdata in RazorEnhanced.Scripts.EnhancedScripts.Values.ToList())
							{
								scriptdata.Run = false;
							}
						}
						break;

					case "SList":
						string filename = RazorEnhanced.Settings.HotKey.FindScript(k);
						Scripts.EnhancedScript script = Scripts.Search(filename);
						if (script != null)
						{
							if (script.Loop)
							{
								if (script.IsUnstarted)
									script.Start();
								else
								{
									script.Stop();
									script.Reset();
								}
							}
							else
							{
								if (!script.Wait && script.IsRunning)
								{
									script.Stop();
									script.Reset();
								}
								else if (!script.IsRunning)
								{
									script.Start();
								}
							}
						}
						break;

					case "DList":
						string dresslist = RazorEnhanced.Settings.HotKey.FindDress(k);
						Dress.ChangeList(dresslist);
						Dress.DressFStart();
						break;

					case "UseVirtue":
						RazorEnhanced.Player.InvokeVirtue(RazorEnhanced.Settings.HotKey.FindString(k));
						break;

					default:
						break;
				}
			}
		}
		private static void ProcessAgentGraphFilter(string function)
		{
			switch (function)
			{
				case "Graphic Filter Trigger ON/OFF":
					if (Engine.MainWindow.MobFilterCheckBox.Checked)
						Engine.MainWindow.MobFilterCheckBox.Checked = false;
					else
						Engine.MainWindow.MobFilterCheckBox.Checked = true;
					break;

				default:
					break;
			}
		}

		private static void ProcessAgentFriend(string function)
		{
			switch (function)
			{
				case "Add Friend":
					Friend.AddFriendTarget();
					break;

				default:
					break;
			}
		}

		private static void ProcessAgentAutoRemount(string function)
		{
			switch (function)
			{
				case "Auto Remount Trigger ON/OFF":
					if (Engine.MainWindow.RemountCheckbox.Checked)
						Engine.MainWindow.RemountCheckbox.Checked = false;
					else
						Engine.MainWindow.RemountCheckbox.Checked = true;
					break;

				case "Auto Remount Set Mount":
					Engine.MainWindow.AutoRemountSetMount();
					break;

				default:
					break;
			}
		}

		private static void ProcessAgentAutoCarver(string function)
		{
			switch (function)
			{
				case "Auto Carver Trigger ON/OFF":
					if (Engine.MainWindow.AutoCarverCheckBox.Checked)
						Engine.MainWindow.AutoCarverCheckBox.Checked = false;
					else
						Engine.MainWindow.AutoCarverCheckBox.Checked = true;
					break;

				case "Auto Carver Set Blade":
					Engine.MainWindow.AutoCarverSetBlade();
					break;

				default:
					break;
			}
		}

		private static void ProcessAgentBoneCutter(string function)
		{
			switch (function)
			{
				case "Bone Cutter Trigger ON/OFF":
					if (Engine.MainWindow.BoneCutterCheckBox.Checked)
						Engine.MainWindow.BoneCutterCheckBox.Checked = false;
					else
						Engine.MainWindow.BoneCutterCheckBox.Checked = true;
					break;

				case "Bone Cutter Set Blade":
					Engine.MainWindow.BoneCutterSetBlade();
					break;

				default:
					break;
			}
		}

		private static void ProcessAgentRestock(string function)
		{
			switch (function)
			{
				case "Restock Start":
					Restock.FStart();
					break;

				case "Restock Stop":
					Restock.FStop();
					break;

				case "Restock Set Soruce Bag":
					Engine.MainWindow.RestockSetSource();
					break;

				case "Restock Set Destination Bag":
					Engine.MainWindow.RestockSetDestination();
					break;

				case "Restock Add Item":
					Engine.MainWindow.RestockAddItem();
					break;

				default:
					break;
			}
		}

		private static void ProcessAgentDress(string function)
		{
			switch (function)
			{
				case "Dress Start":
					Dress.DressFStart();
					break;

				case "Undress Start":
					Dress.UnDressFStart();
					break;

				case "Dress / Undress Stop":
					if (Dress.DressStatus())
						Dress.DressFStop();
					else
						Dress.UnDressFStop();
					break;

				default:
					break;
			}
		}

		private static void ProcessAgentBandage(string function)
		{
			switch (function)
			{
				case "Bandage Heal Trigger ON/OFF":
					if (BandageHeal.Status())
						BandageHeal.Stop();
					else
						BandageHeal.Start();
					break;

				default:
					break;
			}
		}

		private static void ProcessAgentBuy(string function)
		{
			switch (function)
			{
				case "Buy Trigger ON/OFF":
					if (BuyAgent.Status())
						BuyAgent.Disable();
					else
						BuyAgent.Enable();
					break;

				default:
					break;
			}
		}

		private static void ProcessAgentsSell(string function)
		{
			switch (function)
			{
				case "Sell Trigger ON/OFF":
					if (SellAgent.Status())
						SellAgent.Disable();
					else
						SellAgent.Enable();
					break;

				case "Sell Set Soruce Bag":
					Engine.MainWindow.SellAgentSetBag();
					break;

				default:
					break;
			}
		}

		private static void ProcessAgentsAutoloot(string function)
		{
			switch (function)
			{
				case "Autoloot Trigger ON/OFF":
					if (AutoLoot.Status())
						AutoLoot.Stop();
					else
						AutoLoot.Start();
					break;

				case "Autoloot Set Bag":
					Engine.MainWindow.AutolootSetBag();
					break;

				case "Autoloot Add Item":
					Engine.MainWindow.AutolootAddItem();
					break;

				default:
					break;
			}
		}

		private static void ProcessAgentsScavenger(string function)
		{
			switch (function)
			{
				case "Scavenger Trigger ON/OFF":
					if (Scavenger.Status())
						Scavenger.Stop();
					else
						Scavenger.Start();
					break;

				case "Scavenger Set Bag":
					Engine.MainWindow.ScavengerSetBag();
					break;

				case "Scavenger Add Item":
					Engine.MainWindow.ScavengerAddItem();
					break;

				default:
					break;
			}
		}

		private static void ProcessAgentsOrganizer(string function)
		{
			switch (function)
			{
				case "Organizer Start":
					Organizer.FStart();
					break;

				case "Organizer Stop":
					Organizer.FStop();
					break;

				case "Organizer Set Soruce Bag":
					Engine.MainWindow.OrganizerSetSource();
					break;

				case "Organizer Set Destination Bag":
					Engine.MainWindow.OrganizerSetDestination();
					break;

				case "Organizer Add Item":
					Engine.MainWindow.OrganizerAddItem();
					break;

				default:
					break;
			}
		}

		private static void ProcessGeneral(string function)
			{
			switch (function)
			{
				case "Resync":
					RazorEnhanced.Misc.Resync();
					break;

				case "Take Screen Shot":
					ScreenCapManager.CaptureNow();
					break;

				case "Start Video Record":
					Assistant.MainForm.StartVideoRecorder();
					break;

				case "Stop Video Record":
					Assistant.MainForm.StopVideoRecorder();
					break;

				case "Ping Server":
					Assistant.Commands.Ping(null);
					break;

				case "Accept Party":
					if (PacketHandlers.PartyLeader != Assistant.Serial.Zero)
					{
				 		Assistant.Client.Instance.SendToServer(new AcceptParty(PacketHandlers.PartyLeader));
						PacketHandlers.PartyLeader = Assistant.Serial.Zero;
					}
					break;

				case "Decline Party":
					if (PacketHandlers.PartyLeader != Assistant.Serial.Zero)
					{
				 		Assistant.Client.Instance.SendToServer(new DeclineParty(PacketHandlers.PartyLeader));
						PacketHandlers.PartyLeader = Assistant.Serial.Zero;
					}
					break;

				case "DPS Meter Start":
					DPSMeter.Start();
					break;

				case "DPS Meter Pause / Resume":
					DPSMeter.Pause();
					break;

				case "DPS Meter Stop":
					DPSMeter.Stop();
					break;

				case "Open Enhanced Map":
					string mappath = Settings.General.ReadString("EnhancedMapPath");
					if (!File.Exists(mappath))
						Misc.SendMessage("Enhanced Map path not correct or not accessible!", 33, false);
					else
					{
						Process[] processlist = Process.GetProcesses();

						foreach (Process process in processlist)
						{
							if (process.ProcessName == "EnhancedMap")
							{
								Misc.SendMessage("Enhanced Map already running", 33, false);
								return;
							}
						}

						ProcessStartInfo info = new ProcessStartInfo()
						{
							WorkingDirectory = Path.GetDirectoryName(mappath),
							FileName = mappath
						};
						Process.Start(info);
					}
					break;

				case "Inspect Item/Ground":
			 		Assistant.Client.Instance.ForceSendToClient(new UnicodeMessage(0xFFFFFFFF, -1, MessageType.Regular, 0x25, 3, Language.CliLocName, "System", "Target a player or item to open object inspect."));
					Targeting.OneTimeTarget(true, new Targeting.TargetResponseCallback(Commands.GetInfoTarget_Callback));
					break;

				default:
					break;
			}
		}

		private static void ProcessActions(string function)
		{
			switch (function)
			{
				case "Fly ON/OFF":
			 		Assistant.Client.Instance.SendToServer(new ToggleFly());
					break;

				case "Grab Item":
					RazorEnhanced.Misc.SendMessage("Target item to Grab.", false);
					Targeting.OneTimeTarget(new Targeting.TargetResponseCallback(grabitemTarget_Callback));
					break;

				case "Drop Item":
					RazorEnhanced.Misc.SendMessage("Target item to Drop at feet.", false);
					Targeting.OneTimeTarget(new Targeting.TargetResponseCallback(dropitemTarget_Callback));
					break;

				case "Hide Item":
					RazorEnhanced.Misc.SendMessage("Target item to Hide.", false);
					Targeting.OneTimeTarget(new Targeting.TargetResponseCallback(Commands.HideItem_Callback));
					break;

				default:
					break;
			}
		}

		private static void grabitemTarget_Callback(bool loc, Assistant.Serial serial, Assistant.Point3D pt, ushort itemid)
		{
			Assistant.Item itemtograb = Assistant.World.FindItem(serial);

			if (itemtograb != null && itemtograb.Serial.IsItem) // && OSI nothing was movable itemtograb.Movable)
			{
				Assistant.DragDropManager.DragDrop(itemtograb, itemtograb.Amount, World.Player.Backpack);
			}
			else
				RazorEnhanced.Misc.SendMessage("Invalid or inaccessible item.", false);
		}
		private static void setlastTarget_Callback(bool loc, Assistant.Serial serial, Assistant.Point3D pt, ushort itemid)
		{
			if (serial.IsValid)
				Target.SetLast(serial, false);
		}

		internal static void dropitemTarget_Callback(bool loc, Assistant.Serial serial, Assistant.Point3D pt, ushort itemid)
		{
			Assistant.Item itemtodrop = World.FindItem(serial);

			if (itemtodrop != null && itemtodrop.Movable)
			{
				Assistant.DragDropManager.DragDrop(itemtodrop, World.Player.Position, itemtodrop.Amount);
			}
			else
				RazorEnhanced.Misc.SendMessage("Invalid or inaccessible item.", false);
		}

		private static void ProcessUse(string function)
		{
			Assistant.Item item;
			switch (function)
			{
				case "Last Item":
					if (World.Player.LastObject != Assistant.Serial.Zero)
						Assistant.Client.Instance.SendToServer(new DoubleClick(World.Player.LastObject));
					break;

				case "Left Hand":
					item = World.Player.GetItemOnLayer(Layer.LeftHand);
					if (item != null)
						Assistant.Client.Instance.SendToServer(new DoubleClick(item.Serial));
					break;

				case "Right Hand":
					item = World.Player.GetItemOnLayer(Layer.RightHand);
					if (item != null)
						Assistant.Client.Instance.SendToServer(new DoubleClick(item.Serial));
					break;

				default:
					break;
			}
		}

		private static void ProcessShowName(string function)
		{
			switch (function)
			{
				case "All":
					foreach (Assistant.Mobile m in World.MobilesInRange())
					{
						if (m != World.Player)
					 		Assistant.Client.Instance.SendToServer(new SingleClick(m));

						if (Assistant.Engine.MainWindow.LastTargTextFlags.Checked)
							Targeting.CheckTextFlags(m);
					}
					foreach (Assistant.Item i in World.Items.Values)
					{
						if (i.IsCorpse)
					 		Assistant.Client.Instance.SendToServer(new SingleClick(i));
					}
					break;

				case "Corpses":
					foreach (Assistant.Item i in World.Items.Values)
					{
						if (i.IsCorpse)
					 		Assistant.Client.Instance.SendToServer(new SingleClick(i));
					}
					break;

				case "Mobiles":
					foreach (Assistant.Mobile m in World.MobilesInRange())
					{
						if (m != World.Player)
					 		Assistant.Client.Instance.SendToServer(new SingleClick(m));

						if (Assistant.Engine.MainWindow.LastTargTextFlags.Checked)
							Targeting.CheckTextFlags(m);
					}
					break;

				case "Items":
					foreach (Assistant.Item i in World.Items.Values)
					{
				 		Assistant.Client.Instance.SendToServer(new SingleClick(i));
					}
					break;

				default:
					break;
			}
		}

		private static void ProcessPet(string function)
		{

			if (function == "Mount")
			{
				if (Filters.AutoRemountSerial != 0)
					Assistant.Client.Instance.SendToServer(new DoubleClick(Filters.AutoRemountSerial));
			}
			else if (function == "Dismount")
			{
				Assistant.Client.Instance.SendToServer(new DoubleClick(World.Player.Serial));
			}
			else if (function == "Mount / Dismount")
			{
				if (Player.Mount == null)
					Assistant.Client.Instance.SendToServer(new DoubleClick(Filters.AutoRemountSerial));
				else
					Assistant.Client.Instance.SendToServer(new DoubleClick(World.Player.Serial));
			}
			else
			{
				RazorEnhanced.Player.ChatSay(Engine.MainWindow.SpeechHue, function);
			}
		}

		private static void ProcessAbilities(string function)
		{
			switch (function)
			{
				case "Primary":
					Assistant.SpecialMoves.SetPrimaryAbility(false);
					break;
				case "Secondary":
					Assistant.SpecialMoves.SetSecondaryAbility(false);
					break;
				case "Stun":
					Assistant.SpecialMoves.OnStun(false);
					break;
				case "Disarm":
					Assistant.SpecialMoves.OnDisarm(false);
					break;
				case "Cancel":
					Assistant.SpecialMoves.ClearAbilities(false);
					break;
				case "Primary ON/OFF":
					if (SpecialMoves.HasSecondary)
						Assistant.SpecialMoves.SetPrimaryAbility(false);
					else if (SpecialMoves.HasPrimary)
						Assistant.SpecialMoves.ClearAbilities(false);
					else
						Assistant.SpecialMoves.SetPrimaryAbility(false);
					break;
				case "Secondary ON/OFF":
					if (SpecialMoves.HasPrimary)
						Assistant.SpecialMoves.SetSecondaryAbility(false);
					else if (SpecialMoves.HasSecondary)
						Assistant.SpecialMoves.ClearAbilities(false);
					else
						Assistant.SpecialMoves.SetSecondaryAbility(false);
					break;
				default:
					break;
			}
		}

		private static void ProcessAttack(string function)
		{
			switch (function)
			{
				case "Attack Last Target":
					if (World.FindMobile(Targeting.GetLastTarger) != null)
					{
						Targeting.LastAttack = Targeting.GetLastTarger;
						Assistant.Client.Instance.SendToServer(new AttackReq(Targeting.GetLastTarger));
					}
					break;
				case "Attack Last":
					if (Targeting.LastAttack != 0)
						Assistant.Client.Instance.SendToServer(new AttackReq(Targeting.LastAttack));
					break;
				case "WarMode ON/OFF":
					SpecialMoves.ToggleWarPeace();
					break;
				default:
					break;
			}
		}

		private static void ProcessBandage(string function)
		{
			Assistant.Item pack = World.Player.Backpack;
			if (pack == null)
				return;

			switch (function)
			{
				case "Self":
					{
						int bandageserial = BandageHeal.SearchBandage(3617, -1);

						if (bandageserial == 0)
							World.Player.SendMessage(MsgLevel.Warning, LocString.NoBandages);
						else
						{
							if (Engine.ClientVersion.Major >= 7) // Uso nuovo packet
								Items.UseItem(bandageserial, World.Player.Serial, false);
							else // Vecchi client
							{
								Items.UseItem(bandageserial);
								Target.WaitForTarget(1000, true);
								Target.TargetExecute(World.Player.Serial);
							}
						}
					}
					break;

				case "Last":
					{
						int bandageserial = BandageHeal.SearchBandage(3617, -1);

						if (bandageserial == 0)
							World.Player.SendMessage(MsgLevel.Warning, LocString.NoBandages);
						else
						{
							if (Engine.ClientVersion.Major >= 7) // Uso nuovo packet
								Items.UseItem(bandageserial, Target.GetLast(), false);
							else // Vecchi client
							{
								Items.UseItem(bandageserial);
								Target.WaitForTarget(1000, true);
								Target.TargetExecute(Target.GetLast());
							}
						}
					}
					break;

				case "Use Only":
					if (!UseItemByIdHue(pack, 3617, -1))
						World.Player.SendMessage(MsgLevel.Warning, LocString.NoBandages);
					break;

				default:
					break;
			}
		}

		private static void ProcessPotions(string function)
		{
			Assistant.Item pack = World.Player.Backpack;
			switch (function)
			{
				case "Potion Agility":
					if (pack != null)
					{
						if (!UseItemByIdHue(pack, 3848, 0))
							World.Player.SendMessage(MsgLevel.Warning, "No potions left");
					}
					break;

				case "Potion Cure":
					if (pack != null)
					{
						if (!UseItemByIdHue(pack, 3847, 0))
							World.Player.SendMessage(MsgLevel.Warning, "No potions left");
					}
					break;

				case "Potion Explosion":
					if (pack != null)
					{
						if (!UseItemByIdHue(pack, 3853, 0))
							World.Player.SendMessage(MsgLevel.Warning, "No potions left");
					}
					break;

				case "Potion Heal":
					if (pack != null)
					{
						if (!UseItemByIdHue(pack, 3852, 0))
							World.Player.SendMessage(MsgLevel.Warning, "No potions left");
					}
					break;

				case "Potion Refresh":
					if (pack != null)
					{
						if (!UseItemByIdHue(pack, 3851, 0))
							World.Player.SendMessage(MsgLevel.Warning, "No potions left");
					}
					break;

				case "Potion Strength":
					if (pack != null)
					{
						if (!UseItemByIdHue(pack, 0x0F09, 0))
							World.Player.SendMessage(MsgLevel.Warning, "No potions left");
					}
					break;

				case "Potion Nightsight":
					if (pack != null)
					{
						if (!UseItemByIdHue(pack, 3846, 0))
							World.Player.SendMessage(MsgLevel.Warning, "No potions left");
					}
					break;

				case "Potion Shatter":
					if (pack != null)
					{
						if (!UseItemByIdHue(pack, 0x0F0D, 0x003C))
							World.Player.SendMessage(MsgLevel.Warning, "No potions left");
					}
					break;

				case "Potion Parasitic":
					if (pack != null)
					{
						if (!UseItemByIdHue(pack, 0x0F0A, 0x017C))
							World.Player.SendMessage(MsgLevel.Warning, "No potions left");
					}
					break;

				case "Potion Supernova":
					if (pack != null)
					{
						if (!UseItemByIdHue(pack, 0x0F09, 0x000D))
							World.Player.SendMessage(MsgLevel.Warning, "No potions left");
					}
					break;

				case "Potion Confusion Blast":
					if (pack != null)
					{
						if (!UseItemByIdHue(pack, 0x0F06, 0x048D))
							World.Player.SendMessage(MsgLevel.Warning, "No potions left");
					}
					break;

				case "Potion Conflagration":
					if (pack != null)
					{
						if (!UseItemByIdHue(pack, 0x0F06, 0x0489))
							World.Player.SendMessage(MsgLevel.Warning, "No potions left");
					}
					break;

				case "Potion Invisibility":
					if (pack != null)
					{
						if (!UseItemByIdHue(pack, 0x0F06, 0x0132))
							World.Player.SendMessage(MsgLevel.Warning, "No potions left");
					}
					break;

				case "Potion Exploding Tar":
					if (pack != null)
					{
						if (!UseItemByIdHue(pack, 0x0F0D, 0x0455))
							World.Player.SendMessage(MsgLevel.Warning, "No potions left");
					}
					break;

				case "Fear Essence":
					if (pack != null)
					{
						if (!UseItemByIdHue(pack, 0x0F0D, 0x0005))
							World.Player.SendMessage(MsgLevel.Warning, "No potions left");
					}
					break;

				case "Darkglow Poison":
					if (pack != null)
					{
						if (!UseItemByIdHue(pack, 0x0F0A, 0x0096))
							World.Player.SendMessage(MsgLevel.Warning, "No potions left");
					}
					break;

				case "Kurak Ambusher's Essence":
					if (pack != null)
					{
						if (!UseItemByIdHue(pack, 0x0F06, 0x04EC))
							World.Player.SendMessage(MsgLevel.Warning, "No potions left");
					}
					break;

				case "Potion Sakkhra Prophylaxis":
					if (pack != null)
					{
						if (!UseItemByIdHue(pack, 0x0F06, 0x09E3))
							World.Player.SendMessage(MsgLevel.Warning, "No potions left");
					}
					break;

				case "Jukari Burn Poultice":
					if (pack != null)
					{
						if (!UseItemByIdHue(pack, 0x0F06, 0x0AA7))
							World.Player.SendMessage(MsgLevel.Warning, "No potions left");
					}
					break;

				case "Barako Draft Of Might":
					if (pack != null)
					{
						if (!UseItemByIdHue(pack, 0x0F06, 0x0430))
							World.Player.SendMessage(MsgLevel.Warning, "No potions left");
					}
					break;

				case "Urali Trance Tonic":
					if (pack != null)
					{
						if (!UseItemByIdHue(pack, 0x0F06, 0x044A))
							World.Player.SendMessage(MsgLevel.Warning, "No potions left");
					}
					break;

				default:
					break;
			}
		}

		private static void ProcessOther(string function)
		{
			Assistant.Item pack = World.Player.Backpack;
			switch (function)
			{
				case "Enchanted Apple":
					if (pack != null)
					{
						if (!UseItemByIdHue(pack, 12248, 1160))
							World.Player.SendMessage(MsgLevel.Warning, "No item left");
					}
					break;

				case "Orange Petals":
					if (pack != null)
					{
						if (!UseItemByIdHue(pack, 0x1021, 0x002B))
							World.Player.SendMessage(MsgLevel.Warning, "No item left");
					}
					break;

				case "Wrath Grapes":
					if (pack != null)
					{
						if (!UseItemByIdHue(pack, 0x2FD7, 0x0482))
							World.Player.SendMessage(MsgLevel.Warning, "No item left");
					}
					break;

				case "Rose Of Trinsic":
					if (pack != null)
					{
						if (!UseItemByIdHue(pack, 0x234B, 0))
							World.Player.SendMessage(MsgLevel.Warning, "No item left");
					}
					break;

				case "Smoke Bomb":
					if (pack != null)
					{
						if (!UseItemByIdHue(pack, 0x2808, 0))
							World.Player.SendMessage(MsgLevel.Warning, "No item left");
					}
					break;

				case "Spell Stone":
					if (pack != null)
					{
						if (!UseItemByIdHue(pack, 0x4079, 0))
							World.Player.SendMessage(MsgLevel.Warning, "No item left");
					}
					break;

				case "Healing Stone":
					if (pack != null)
					{
						if (!UseItemByIdHue(pack, 0x4078, 0))
							World.Player.SendMessage(MsgLevel.Warning, "No item left");
					}
					break;

				case "Pouch":
					if (pack != null)
					{
						if (!UseItemByIdHue(pack, 0x09B0, -1))
						{
							if (!UseItemByIdHue(pack, 0x0E79, - 1))
								World.Player.SendMessage(MsgLevel.Warning, "No Pouch found");
						}
					}
					break;

				default:
					break;
			}
		}

		private static void ProcessHands(string function)
		{
			Assistant.Item i;
			switch (function)
			{
				case "Clear Left":
					if (World.Player.GetItemOnLayer(Layer.LeftHand) != null)
					{
						World.Player.LastWeaponLeft = World.Player.GetItemOnLayer(Layer.LeftHand).Serial;
						Player.UnEquipItemByLayer("LeftHand", false);
					}
					break;

				case "Clear Right":
					if (World.Player.GetItemOnLayer(Layer.RightHand) != null)
					{
						World.Player.LastWeaponRight = World.Player.GetItemOnLayer(Layer.RightHand).Serial;
						Player.UnEquipItemByLayer("RightHand", false);
					}
					break;

				case "Equip Right":
					if (World.Player.GetItemOnLayer(Layer.RightHand) != null) // Layer già occupato
						return;

					// arma a due mani equippata
					if (World.Player.GetItemOnLayer(Layer.LeftHand) != null && World.Player.GetItemOnLayer(Layer.LeftHand).IsTwoHanded)
						return;

					i = World.FindItem(World.Player.LastWeaponRight);
					if (i != null)
					{
				 		Assistant.Client.Instance.SendToServer(new LiftRequest(i.Serial, i.Amount));
				 		Assistant.Client.Instance.SendToServer(new EquipRequest(i.Serial, World.Player.Serial, i.Layer));
					}
					break;

				case "Equip Left":
					if (World.Player.GetItemOnLayer(Layer.LeftHand) != null) // Layer già occupato
						return;

					i = World.FindItem(World.Player.LastWeaponLeft);
					if (i != null)
					{
				 		Assistant.Client.Instance.SendToServer(new LiftRequest(i.Serial, i.Amount));
				 		Assistant.Client.Instance.SendToServer(new EquipRequest(i.Serial, World.Player.Serial, i.Layer));
					}
					break;

				case "Toggle Right":
					if (World.Player.GetItemOnLayer(Layer.RightHand) != null)
					{
						World.Player.LastWeaponRight = World.Player.GetItemOnLayer(Layer.RightHand).Serial;
						Player.UnEquipItemByLayer("RightHand", false);
					}
					else
					{
						// arma a due mani equippata
						if (World.Player.GetItemOnLayer(Layer.LeftHand) != null && World.Player.GetItemOnLayer(Layer.LeftHand).IsTwoHanded)
							return;

						i = World.FindItem(World.Player.LastWeaponRight);
						if (i != null)
						{
					 		Assistant.Client.Instance.SendToServer(new LiftRequest(i.Serial, i.Amount));
					 		Assistant.Client.Instance.SendToServer(new EquipRequest(i.Serial, World.Player.Serial, i.Layer));
						}
					}
					break;

				case "Toggle Left":
					if (World.Player.GetItemOnLayer(Layer.LeftHand) != null)
					{
						World.Player.LastWeaponLeft = World.Player.GetItemOnLayer(Layer.LeftHand).Serial;
						Player.UnEquipItemByLayer("LeftHand", false);
					}
					else
					{
						i = World.FindItem(World.Player.LastWeaponLeft);
						if (i != null)
						{
					 		Assistant.Client.Instance.SendToServer(new LiftRequest(i.Serial, i.Amount));
					 		Assistant.Client.Instance.SendToServer(new EquipRequest(i.Serial, World.Player.Serial, i.Layer));
						}
					}
					break;

				default:
					break;
			}
		}

		private static void ProcessEquipWands(string function)
		{
			switch (function)
			{
				default:
					World.Player.SendMessage("Da implementare");
					break;
			}
		}

		private static void ProcessSkills(string function)
		{
			if (function == "Last Used")
			{
				if (World.Player.LastSkill != -1)
				{
			 		Assistant.Client.Instance.SendToServer(new UseSkill(World.Player.LastSkill));
					if ((World.Player.LastSkill == (int)SkillName.Stealth && !World.Player.Visible) || World.Player.LastSkill == (int)SkillName.Hiding) // Trigger stealth step counter
						StealthSteps.Hide();
				}
			}
			else
			{
				RazorEnhanced.Player.UseSkillOnly(function,false);
			}
		}

		private static void ProcessSpellsAgent(string function)
		{
			switch (function)
			{
				case "Mini Heal":
					Assistant.Spell.MiniHealOrCureSelf();
					break;

				case "Big Heal":
					Assistant.Spell.HealOrCureSelf();
					break;

				case "Chivarly Heal":
					Assistant.Spell.HealOrCureSelfChiva();
					break;

				case "Interrupt":
					Assistant.Item item = Spells.FindUsedLayer();
					if (item != null)
					{
						Assistant.Point3D loc = Assistant.Point3D.MinusOne;
						Assistant.Client.Instance.SendToServer(new LiftRequest(item, 1));
						Assistant.Client.Instance.SendToServer(new EquipRequest(item.Serial, Assistant.World.Player, item.Layer)); // Equippa
					}
					break;

				case "Last Spell":
					Spells.CastLastSpellInternal(false);
					break;

                case "Last Spell Last Target":
                    Spells.CastLastSpellLastTarget();
                    break;


                default:
					break;
			}
		}

		private static void ProcessTarget(string function)
		{
			switch (function)
			{
				case "Target Self":
					Assistant.Targeting.TargetSelf();
					break;

				case "Target Last":
					Assistant.Targeting.LastTarget();
					break;

				case "Target Self Queued":
					Assistant.Client.Instance.SendToClient(new UnicodeMessage(World.Player.Serial, World.Player.Body, MessageType.Regular, 993, 3, Language.CliLocName, World.Player.Name, "Target \"self\" queued."));
					Assistant.Targeting.TargetSelf(true);
					break;

				case "Target Last Queued":
					Assistant.Client.Instance.SendToClient(new UnicodeMessage(World.Player.Serial, World.Player.Body, MessageType.Regular, 993, 3, Language.CliLocName, World.Player.Name, "Target \"last\" queued."));
					Assistant.Targeting.LastTarget(true);
					break;

				case "Target Cancel":
					//Assistant.Targeting.CancelClientTarget(false);
					Assistant.Targeting.CancelOneTimeTarget(false);
					break;

				case "Clear Target Queue":
					Assistant.Client.Instance.SendToClient(new UnicodeMessage(World.Player.Serial, World.Player.Body, MessageType.Regular, 993, 3, Language.CliLocName, World.Player.Name, "Target \"queue\" cleared."));
					Assistant.Targeting.ClearQueue();
					break;

				case "Clear Last Target":
					Assistant.Client.Instance.SendToClient(new UnicodeMessage(World.Player.Serial, World.Player.Body, MessageType.Regular, 993, 3, Language.CliLocName, World.Player.Name, "Target \"last\" cleared."));
					Assistant.Targeting.ClearLast();
                    break;

				case "Clear Last and Queue":
					Assistant.Client.Instance.SendToClient(new UnicodeMessage(World.Player.Serial, World.Player.Body, MessageType.Regular, 993, 3, Language.CliLocName, World.Player.Name, "Target \"last and queue\" cleared."));
					Assistant.Targeting.ClearLast();
					Assistant.Targeting.ClearQueue();
					break;

				case "Set Last":
					Targeting.OneTimeTarget(new Targeting.TargetResponseCallback(setlastTarget_Callback));
					break;

				default:
					break;
			}
		}

		internal static void Init()
		{
			// BLocco generico
			Engine.MainWindow.HotKeyKeyMasterLabel.Text = "ON/OFF Key: " + KeyString(RazorEnhanced.Settings.General.ReadKey("HotKeyMasterKey"));

			if (RazorEnhanced.Settings.General.ReadBool("HotKeyEnable"))
				Engine.MainWindow.HotKeyStatusLabel.Text = "Status: Enabled";
			else
				Engine.MainWindow.HotKeyStatusLabel.Text = "Status: Disabled";

			// Parametri lista
			Engine.MainWindow.HotKeyTreeView.Nodes.Clear();
			Engine.MainWindow.HotKeyTreeView.Nodes.Add("HotKeys");
			Engine.MainWindow.HotKeyTreeView.Nodes[0].Nodes.Add("General");

			// General
			List<HotKeyData> keylist = RazorEnhanced.Settings.HotKey.ReadGroup("General");
			foreach (HotKeyData keydata in keylist)
				Engine.MainWindow.HotKeyTreeView.Nodes[0].Nodes[0].Nodes.Add(GenerateNode(keydata));

			// Actions
			Engine.MainWindow.HotKeyTreeView.Nodes[0].Nodes.Add("Actions");
			keylist = RazorEnhanced.Settings.HotKey.ReadGroup("Actions");
			foreach (HotKeyData keydata in keylist)
				Engine.MainWindow.HotKeyTreeView.Nodes[0].Nodes[1].Nodes.Add(GenerateNode(keydata));

			// Actions -> Use
			Engine.MainWindow.HotKeyTreeView.Nodes[0].Nodes[1].Nodes.Add("Use");
			keylist = RazorEnhanced.Settings.HotKey.ReadGroup("Use");
			foreach (HotKeyData keydata in keylist)
				Engine.MainWindow.HotKeyTreeView.Nodes[0].Nodes[1].Nodes[4].Nodes.Add(GenerateNode(keydata));

			// Actions -> Show Names
			Engine.MainWindow.HotKeyTreeView.Nodes[0].Nodes[1].Nodes.Add("Show Names");
			keylist = RazorEnhanced.Settings.HotKey.ReadGroup("Show Names");
			foreach (HotKeyData keydata in keylist)
				Engine.MainWindow.HotKeyTreeView.Nodes[0].Nodes[1].Nodes[5].Nodes.Add(GenerateNode(keydata));

			// Actions -> Per Commands
			Engine.MainWindow.HotKeyTreeView.Nodes[0].Nodes[1].Nodes.Add("Pet Commands");
			keylist = RazorEnhanced.Settings.HotKey.ReadGroup("Pet Commands");
			foreach (HotKeyData keydata in keylist)
				Engine.MainWindow.HotKeyTreeView.Nodes[0].Nodes[1].Nodes[6].Nodes.Add(GenerateNode(keydata));

			// Agents
			Engine.MainWindow.HotKeyTreeView.Nodes[0].Nodes.Add("Agents");

			// Agent Autoloot
			Engine.MainWindow.HotKeyTreeView.Nodes[0].Nodes[2].Nodes.Add("Autoloot");
			keylist = RazorEnhanced.Settings.HotKey.ReadGroup("AgentAutoloot");
			foreach (HotKeyData keydata in keylist)
				Engine.MainWindow.HotKeyTreeView.Nodes[0].Nodes[2].Nodes[0].Nodes.Add(GenerateNode(keydata));

			// Agent Scavenger
			Engine.MainWindow.HotKeyTreeView.Nodes[0].Nodes[2].Nodes.Add("Scavenger");
			keylist = RazorEnhanced.Settings.HotKey.ReadGroup("AgentScavenger");
			foreach (HotKeyData keydata in keylist)
				Engine.MainWindow.HotKeyTreeView.Nodes[0].Nodes[2].Nodes[1].Nodes.Add(GenerateNode(keydata));

			// Organizer Agent
			Engine.MainWindow.HotKeyTreeView.Nodes[0].Nodes[2].Nodes.Add("Organizer");
			keylist = RazorEnhanced.Settings.HotKey.ReadGroup("AgentOrganizer");
			foreach (HotKeyData keydata in keylist)
				Engine.MainWindow.HotKeyTreeView.Nodes[0].Nodes[2].Nodes[2].Nodes.Add(GenerateNode(keydata));

			// Sell Agent
			Engine.MainWindow.HotKeyTreeView.Nodes[0].Nodes[2].Nodes.Add("Sell");
			keylist = RazorEnhanced.Settings.HotKey.ReadGroup("AgentSell");
			foreach (HotKeyData keydata in keylist)
				Engine.MainWindow.HotKeyTreeView.Nodes[0].Nodes[2].Nodes[3].Nodes.Add(GenerateNode(keydata));

			// Buy Agent
			Engine.MainWindow.HotKeyTreeView.Nodes[0].Nodes[2].Nodes.Add("Buy");
			keylist = RazorEnhanced.Settings.HotKey.ReadGroup("AgentBuy");
			foreach (HotKeyData keydata in keylist)
				Engine.MainWindow.HotKeyTreeView.Nodes[0].Nodes[2].Nodes[4].Nodes.Add(GenerateNode(keydata));

			// Dress Agent
			Engine.MainWindow.HotKeyTreeView.Nodes[0].Nodes[2].Nodes.Add("Dress");
			keylist = RazorEnhanced.Settings.HotKey.ReadGroup("AgentDress");
			foreach (HotKeyData keydata in keylist)
				Engine.MainWindow.HotKeyTreeView.Nodes[0].Nodes[2].Nodes[5].Nodes.Add(GenerateNode(keydata));

			// Agents -> Dress List
				Engine.MainWindow.HotKeyTreeView.Nodes[0].Nodes[2].Nodes[5].Nodes.Add("DList", "List");
				keylist = RazorEnhanced.Settings.HotKey.ReadDress();
				foreach (HotKeyData keydata in keylist)
					Engine.MainWindow.HotKeyTreeView.Nodes[0].Nodes[2].Nodes[5].Nodes[3].Nodes.Add(GenerateNode(keydata));

			// Restock Agent
			Engine.MainWindow.HotKeyTreeView.Nodes[0].Nodes[2].Nodes.Add("Restock");
			keylist = RazorEnhanced.Settings.HotKey.ReadGroup("AgentRestock");
			foreach (HotKeyData keydata in keylist)
				Engine.MainWindow.HotKeyTreeView.Nodes[0].Nodes[2].Nodes[6].Nodes.Add(GenerateNode(keydata));

			// Bandage Heal agent
			Engine.MainWindow.HotKeyTreeView.Nodes[0].Nodes[2].Nodes.Add("Bandage Heal");
			keylist = RazorEnhanced.Settings.HotKey.ReadGroup("AgentBandage");
			foreach (HotKeyData keydata in keylist)
				Engine.MainWindow.HotKeyTreeView.Nodes[0].Nodes[2].Nodes[7].Nodes.Add(GenerateNode(keydata));

			// BoneCutter agent
			Engine.MainWindow.HotKeyTreeView.Nodes[0].Nodes[2].Nodes.Add("Bone Cutter");
			keylist = RazorEnhanced.Settings.HotKey.ReadGroup("AgentBoneCutter");
			foreach (HotKeyData keydata in keylist)
				Engine.MainWindow.HotKeyTreeView.Nodes[0].Nodes[2].Nodes[8].Nodes.Add(GenerateNode(keydata));

			// AutoCarver agent
			Engine.MainWindow.HotKeyTreeView.Nodes[0].Nodes[2].Nodes.Add("Auto Carver");
			keylist = RazorEnhanced.Settings.HotKey.ReadGroup("AgentAutoCarver");
			foreach (HotKeyData keydata in keylist)
				Engine.MainWindow.HotKeyTreeView.Nodes[0].Nodes[2].Nodes[9].Nodes.Add(GenerateNode(keydata));

			// AutoRemount agent
			Engine.MainWindow.HotKeyTreeView.Nodes[0].Nodes[2].Nodes.Add("Auto Remount");
			keylist = RazorEnhanced.Settings.HotKey.ReadGroup("AgentAutoRemount");
			foreach (HotKeyData keydata in keylist)
				Engine.MainWindow.HotKeyTreeView.Nodes[0].Nodes[2].Nodes[10].Nodes.Add(GenerateNode(keydata));

			// AutoRemount agent
			Engine.MainWindow.HotKeyTreeView.Nodes[0].Nodes[2].Nodes.Add("Graphics Filter");
			keylist = RazorEnhanced.Settings.HotKey.ReadGroup("AgentGraphFilter");
			foreach (HotKeyData keydata in keylist)
				Engine.MainWindow.HotKeyTreeView.Nodes[0].Nodes[2].Nodes[11].Nodes.Add(GenerateNode(keydata));

			// Friend  agent
			Engine.MainWindow.HotKeyTreeView.Nodes[0].Nodes[2].Nodes.Add("Friend");
			keylist = RazorEnhanced.Settings.HotKey.ReadGroup("AgentFriend");
			foreach (HotKeyData keydata in keylist)
				Engine.MainWindow.HotKeyTreeView.Nodes[0].Nodes[2].Nodes[12].Nodes.Add(GenerateNode(keydata));


			// Combats
			Engine.MainWindow.HotKeyTreeView.Nodes[0].Nodes.Add("Combat");

			// Combat  --> Abilities
			Engine.MainWindow.HotKeyTreeView.Nodes[0].Nodes[3].Nodes.Add("Abilities");
			keylist = RazorEnhanced.Settings.HotKey.ReadGroup("Abilities");
			foreach (HotKeyData keydata in keylist)
				Engine.MainWindow.HotKeyTreeView.Nodes[0].Nodes[3].Nodes[0].Nodes.Add(GenerateNode(keydata));

			// Combat  --> Attack
			Engine.MainWindow.HotKeyTreeView.Nodes[0].Nodes[3].Nodes.Add("Attack");
			keylist = RazorEnhanced.Settings.HotKey.ReadGroup("Attack");
			foreach (HotKeyData keydata in keylist)
				Engine.MainWindow.HotKeyTreeView.Nodes[0].Nodes[3].Nodes[1].Nodes.Add(GenerateNode(keydata));

			// Combat  --> Bandage
			Engine.MainWindow.HotKeyTreeView.Nodes[0].Nodes[3].Nodes.Add("Bandage");
			keylist = RazorEnhanced.Settings.HotKey.ReadGroup("Bandage");
			foreach (HotKeyData keydata in keylist)
				Engine.MainWindow.HotKeyTreeView.Nodes[0].Nodes[3].Nodes[2].Nodes.Add(GenerateNode(keydata));

			// Combat  --> Consumable
			Engine.MainWindow.HotKeyTreeView.Nodes[0].Nodes[3].Nodes.Add("Consumable");

			// Combat  --> Consumable --> Potions
			Engine.MainWindow.HotKeyTreeView.Nodes[0].Nodes[3].Nodes[3].Nodes.Add("Potions");
			keylist = RazorEnhanced.Settings.HotKey.ReadGroup("Potions");
			foreach (HotKeyData keydata in keylist)
				Engine.MainWindow.HotKeyTreeView.Nodes[0].Nodes[3].Nodes[3].Nodes[0].Nodes.Add(GenerateNode(keydata));

			// Combat --> Consumable --> Other
			Engine.MainWindow.HotKeyTreeView.Nodes[0].Nodes[3].Nodes[3].Nodes.Add("Other");
			keylist = RazorEnhanced.Settings.HotKey.ReadGroup("Other");
			foreach (HotKeyData keydata in keylist)
				Engine.MainWindow.HotKeyTreeView.Nodes[0].Nodes[3].Nodes[3].Nodes[1].Nodes.Add(GenerateNode(keydata));

			// Combat --> Hands
			Engine.MainWindow.HotKeyTreeView.Nodes[0].Nodes[3].Nodes.Add("Hands");
			keylist = RazorEnhanced.Settings.HotKey.ReadGroup("Hands");
			foreach (HotKeyData keydata in keylist)
				Engine.MainWindow.HotKeyTreeView.Nodes[0].Nodes[3].Nodes[4].Nodes.Add(GenerateNode(keydata));

			// Combat --> Hands -> Equip Wands
			Engine.MainWindow.HotKeyTreeView.Nodes[0].Nodes[3].Nodes.Add("Equip Wands");
			keylist = RazorEnhanced.Settings.HotKey.ReadGroup("Equip Wands");
			foreach (HotKeyData keydata in keylist)
				Engine.MainWindow.HotKeyTreeView.Nodes[0].Nodes[3].Nodes[5].Nodes.Add(GenerateNode(keydata));

			// Skills
			Engine.MainWindow.HotKeyTreeView.Nodes[0].Nodes.Add("Skills");
			keylist = RazorEnhanced.Settings.HotKey.ReadGroup("Skills");
			foreach (HotKeyData keydata in keylist)
				Engine.MainWindow.HotKeyTreeView.Nodes[0].Nodes[4].Nodes.Add(GenerateNode(keydata));

			// Spells
			Engine.MainWindow.HotKeyTreeView.Nodes[0].Nodes.Add("Spells");
			keylist = RazorEnhanced.Settings.HotKey.ReadGroup("SpellsAgent");
			foreach (HotKeyData keydata in keylist)
				Engine.MainWindow.HotKeyTreeView.Nodes[0].Nodes[5].Nodes.Add(GenerateNode(keydata));

			// Spells -- > Magery
			Engine.MainWindow.HotKeyTreeView.Nodes[0].Nodes[5].Nodes.Add("Magery");
			keylist = RazorEnhanced.Settings.HotKey.ReadGroup("SpellsMagery");
			foreach (HotKeyData keydata in keylist)
				Engine.MainWindow.HotKeyTreeView.Nodes[0].Nodes[5].Nodes[6].Nodes.Add(GenerateNode(keydata));

			// Spells -- > Necro
			Engine.MainWindow.HotKeyTreeView.Nodes[0].Nodes[5].Nodes.Add("Necro");
			keylist = RazorEnhanced.Settings.HotKey.ReadGroup("SpellsNecro");
			foreach (HotKeyData keydata in keylist)
				Engine.MainWindow.HotKeyTreeView.Nodes[0].Nodes[5].Nodes[7].Nodes.Add(GenerateNode(keydata));

			// Spells -- > Bushido
			Engine.MainWindow.HotKeyTreeView.Nodes[0].Nodes[5].Nodes.Add("Bushido");
			keylist = RazorEnhanced.Settings.HotKey.ReadGroup("SpellsBushido");
			foreach (HotKeyData keydata in keylist)
				Engine.MainWindow.HotKeyTreeView.Nodes[0].Nodes[5].Nodes[8].Nodes.Add(GenerateNode(keydata));

			// Spells -- > Ninjitsu
			Engine.MainWindow.HotKeyTreeView.Nodes[0].Nodes[5].Nodes.Add("Ninjitsu");
			keylist = RazorEnhanced.Settings.HotKey.ReadGroup("SpellsNinjitsu");
			foreach (HotKeyData keydata in keylist)
				Engine.MainWindow.HotKeyTreeView.Nodes[0].Nodes[5].Nodes[9].Nodes.Add(GenerateNode(keydata));

			// Spells -- > Spellweaving
			Engine.MainWindow.HotKeyTreeView.Nodes[0].Nodes[5].Nodes.Add("Spellweaving");
			keylist = RazorEnhanced.Settings.HotKey.ReadGroup("SpellsSpellweaving");
			foreach (HotKeyData keydata in keylist)
				Engine.MainWindow.HotKeyTreeView.Nodes[0].Nodes[5].Nodes[10].Nodes.Add(GenerateNode(keydata));

			// Spells -- > Mysticism
			Engine.MainWindow.HotKeyTreeView.Nodes[0].Nodes[5].Nodes.Add("Mysticism");
			keylist = RazorEnhanced.Settings.HotKey.ReadGroup("SpellsMysticism");
			foreach (HotKeyData keydata in keylist)
				Engine.MainWindow.HotKeyTreeView.Nodes[0].Nodes[5].Nodes[11].Nodes.Add(GenerateNode(keydata));

			// Spells -- > Chivalry
			Engine.MainWindow.HotKeyTreeView.Nodes[0].Nodes[5].Nodes.Add("Chivalry");
			keylist = RazorEnhanced.Settings.HotKey.ReadGroup("SpellsChivalry");
			foreach (HotKeyData keydata in keylist)
				Engine.MainWindow.HotKeyTreeView.Nodes[0].Nodes[5].Nodes[12].Nodes.Add(GenerateNode(keydata));

			// Spells -- > Mastery
			Engine.MainWindow.HotKeyTreeView.Nodes[0].Nodes[5].Nodes.Add("Mastery");
			keylist = RazorEnhanced.Settings.HotKey.ReadGroup("SpellsMastery");
			foreach (HotKeyData keydata in keylist)
				Engine.MainWindow.HotKeyTreeView.Nodes[0].Nodes[5].Nodes[13].Nodes.Add(GenerateNode(keydata));

            // Spells -- > Cleric
            Engine.MainWindow.HotKeyTreeView.Nodes[0].Nodes[5].Nodes.Add("Cleric");
            keylist = RazorEnhanced.Settings.HotKey.ReadGroup("SpellsCleric");
            foreach (HotKeyData keydata in keylist)
                Engine.MainWindow.HotKeyTreeView.Nodes[0].Nodes[5].Nodes[14].Nodes.Add(GenerateNode(keydata));

            // Spells -- > Druid
            Engine.MainWindow.HotKeyTreeView.Nodes[0].Nodes[5].Nodes.Add("Druid");
            keylist = RazorEnhanced.Settings.HotKey.ReadGroup("SpellsDruid");
            foreach (HotKeyData keydata in keylist)
                Engine.MainWindow.HotKeyTreeView.Nodes[0].Nodes[5].Nodes[15].Nodes.Add(GenerateNode(keydata));

            // Target
            Engine.MainWindow.HotKeyTreeView.Nodes[0].Nodes.Add("Target");
			keylist = RazorEnhanced.Settings.HotKey.ReadGroup("Target");
			foreach (HotKeyData keydata in keylist)
				Engine.MainWindow.HotKeyTreeView.Nodes[0].Nodes[6].Nodes.Add(GenerateNode(keydata));

			// Target -> List
			Engine.MainWindow.HotKeyTreeView.Nodes[0].Nodes[6].Nodes.Add("TList", "List");
			keylist = RazorEnhanced.Settings.HotKey.ReadTarget();
			foreach (HotKeyData keydata in keylist)
				Engine.MainWindow.HotKeyTreeView.Nodes[0].Nodes[6].Nodes[9].Nodes.Add(GenerateNode(keydata));

			// Script
			Engine.MainWindow.HotKeyTreeView.Nodes[0].Nodes.Add("Script");
			keylist = RazorEnhanced.Settings.HotKey.ReadGroup("Script");
			foreach (HotKeyData keydata in keylist)
				Engine.MainWindow.HotKeyTreeView.Nodes[0].Nodes[7].Nodes.Add(GenerateNode(keydata));

			// Script -> List
			Engine.MainWindow.HotKeyTreeView.Nodes[0].Nodes[7].Nodes.Add("SList", "List");
			keylist = RazorEnhanced.Settings.HotKey.ReadScript();
			Engine.MainWindow.GridScriptComboBox.Items.Clear();
			foreach (HotKeyData keydata in keylist)
			{
				Engine.MainWindow.GridScriptComboBox.Items.Add(keydata.Name); // Aggiorna lista script spellgrid
				Engine.MainWindow.HotKeyTreeView.Nodes[0].Nodes[7].Nodes[1].Nodes.Add(GenerateNode(keydata));
			}

			// Virtue
			Engine.MainWindow.HotKeyTreeView.Nodes[0].Nodes.Add("Virtue");
			keylist = RazorEnhanced.Settings.HotKey.ReadGroup("UseVirtue");
			foreach (HotKeyData keydata in keylist)
				Engine.MainWindow.HotKeyTreeView.Nodes[0].Nodes[8].Nodes.Add(GenerateNode(keydata));

			Engine.MainWindow.HotKeyTreeView.Nodes[0].Expand();
        }

		private static TreeNode GenerateNode(HotKeyData keydata)
		{
			TreeNode a = new TreeNode
			{
				Name = keydata.Name,
				Text = keydata.Name + " ( " + KeyString(keydata.Key) + " )"
			};
			if (keydata.Key != Keys.None)
				a.ForeColor = System.Drawing.Color.DarkGreen;

			return a;
		}
		private static void UpdateOldTreeView(TreeNodeCollection nodes, Keys k)
		{
			foreach (TreeNode node in nodes)
			{
				if (node.Text.Contains(node.Name + " ( " + KeyString(m_key) + " )"))
				{
					node.Text = node.Name + " ( " + KeyString(Keys.None) + " )";
					node.ForeColor = System.Drawing.Color.Black;
					break;
				}
				UpdateOldTreeView(node.Nodes, k);
			}
		}

		internal static void UpdateKey(TreeNode node, bool passkey)
		{
			string name = node.Name;
			if (!RazorEnhanced.Settings.HotKey.AssignedKey(m_key))
			{
				RazorEnhanced.Settings.HotKey.UpdateKey(name, m_key, passkey);
				node.Text = node.Name + " ( " + KeyString(m_key) + " )";
				node.ForeColor = System.Drawing.Color.DarkGreen;
			}
			else
			{
				DialogResult dialogResult = MessageBox.Show("Key: " + KeyString(m_key) + " already assigned! Want replace?", "HotKey", MessageBoxButtons.YesNo);
				if (dialogResult == DialogResult.Yes)
				{
					RazorEnhanced.Settings.HotKey.UnassignKey(m_key);
					RazorEnhanced.Settings.HotKey.UpdateKey(name, m_key, passkey);
					UpdateOldTreeView(Assistant.Engine.MainWindow.HotKeyTreeView.Nodes, m_key);
					node.Text = node.Name + " ( " + KeyString(m_key) + " )";
					node.ForeColor = System.Drawing.Color.DarkGreen;
				}
			}
		}

		internal static void UpdateTargetKey(TreeNode node, bool passkey)
		{
			string name = node.Name;
			if (!RazorEnhanced.Settings.HotKey.AssignedKey(m_key))
			{
				RazorEnhanced.Settings.HotKey.UpdateTargetKey(name, m_key, passkey);
				node.Text = node.Name + " ( " + KeyString(m_key) + " )";
				node.ForeColor = System.Drawing.Color.DarkGreen;
			}
			else
			{
				DialogResult dialogResult = MessageBox.Show("Key: " + KeyString(m_key) + " already assigned! Want replace?", "HotKey", MessageBoxButtons.YesNo);
				if (dialogResult == DialogResult.Yes)
				{
					RazorEnhanced.Settings.HotKey.UnassignKey(m_key);
					RazorEnhanced.Settings.HotKey.UpdateTargetKey(name, m_key, passkey);
					UpdateOldTreeView(Assistant.Engine.MainWindow.HotKeyTreeView.Nodes, m_key);
					node.Text = node.Name + " ( " + KeyString(m_key) + " )";
					node.ForeColor = System.Drawing.Color.DarkGreen;
				}
			}
		}

		internal static void UpdateDressKey(TreeNode node, bool passkey)
		{
			string name = node.Name;
			if (!RazorEnhanced.Settings.HotKey.AssignedKey(m_key))
			{
				RazorEnhanced.Settings.HotKey.UpdateDressKey(name, m_key, passkey);
				node.Text = node.Name + " ( " + KeyString(m_key) + " )";
				node.ForeColor = System.Drawing.Color.DarkGreen;
			}
			else
			{
				DialogResult dialogResult = MessageBox.Show("Key: " + KeyString(m_key) + " already assigned! Want replace?", "HotKey", MessageBoxButtons.YesNo);
				if (dialogResult == DialogResult.Yes)
				{
					RazorEnhanced.Settings.HotKey.UnassignKey(m_key);
					RazorEnhanced.Settings.HotKey.UpdateDressKey(name, m_key, passkey);
					UpdateOldTreeView(Assistant.Engine.MainWindow.HotKeyTreeView.Nodes, m_key);
					node.Text = node.Name + " ( " + KeyString(m_key) + " )";
					node.ForeColor = System.Drawing.Color.DarkGreen;
				}
			}
		}

		internal static void UpdateScriptKey(TreeNode node, bool passkey)
		{
			string name = node.Name;
			if (!RazorEnhanced.Settings.HotKey.AssignedKey(m_key))
			{
				RazorEnhanced.Settings.HotKey.UpdateScriptKey(name, m_key, passkey);
				node.Text = node.Name + " ( " + KeyString(m_key) + " )";
				node.ForeColor = System.Drawing.Color.DarkGreen;
			}
			else
			{
				DialogResult dialogResult = MessageBox.Show("Key: " + KeyString(m_key) + " already assigned! Want replace?", "HotKey", MessageBoxButtons.YesNo);
				if (dialogResult == DialogResult.Yes)
				{
					RazorEnhanced.Settings.HotKey.UnassignKey(m_key);
					RazorEnhanced.Settings.HotKey.UpdateScriptKey(name, m_key, passkey);
					UpdateOldTreeView(Assistant.Engine.MainWindow.HotKeyTreeView.Nodes, m_key);
					node.Text = node.Name + " ( " + KeyString(m_key) + " )";
					node.ForeColor = System.Drawing.Color.DarkGreen;
				}
			}
			Assistant.Engine.MainWindow.UpdateScriptGridKey();
		}

		internal static void UpdateMaster()
		{
			if (!RazorEnhanced.Settings.HotKey.AssignedKey(m_masterkey))
			{
				RazorEnhanced.Settings.General.WriteKey("HotKeyMasterKey", RazorEnhanced.HotKey.m_masterkey);
				Assistant.Engine.MainWindow.HotKeyKeyMasterLabel.Text = "ON/OFF Key: " + KeyString(RazorEnhanced.HotKey.m_masterkey);
			}
			else
			{
				DialogResult dialogResult = MessageBox.Show("Key: " + KeyString(m_masterkey) + " already assigned! Want replace?", "HotKey", MessageBoxButtons.YesNo);
				if (dialogResult == DialogResult.Yes)
				{
					RazorEnhanced.Settings.HotKey.UnassignKey(m_masterkey);
					RazorEnhanced.Settings.General.WriteKey("HotKeyMasterKey", RazorEnhanced.HotKey.m_masterkey);
					Assistant.Engine.MainWindow.HotKeyKeyMasterLabel.Text = "ON/OFF Key: " + KeyString(RazorEnhanced.HotKey.m_masterkey);
				}
			}
		}

		internal static void ClearKey(TreeNode node, string group)
		{
			string name = node.Name;
			if (group == "SList")
			{
				RazorEnhanced.Settings.HotKey.UpdateScriptKey(name, Keys.None, true);
				Assistant.Engine.MainWindow.UpdateScriptGridKey();
			}
			else if (group == "TList")
				RazorEnhanced.Settings.HotKey.UpdateTargetKey(name, Keys.None, true);

			else if (group == "DList")
				RazorEnhanced.Settings.HotKey.UpdateDressKey(name, Keys.None, true);
			else
				RazorEnhanced.Settings.HotKey.UpdateKey(name, Keys.None, true);
			node.Text = node.Name + " ( " + KeyString(Keys.None) + " )";
			node.ForeColor = System.Drawing.Color.Black;
		}

		internal static void ClearMasterKey()
		{
			RazorEnhanced.Settings.General.WriteKey("HotKeyMasterKey", Keys.None);
			Assistant.Engine.MainWindow.HotKeyKeyMasterLabel.Text = "ON/OFF Key: " + KeyString(RazorEnhanced.Settings.General.ReadKey("HotKeyMasterKey"));
		}

		private static bool UseItemByIdHue(Assistant.Item cont, ushort find, int hue)
		{
			foreach (Assistant.Item t in cont.Contains)
			{
				Assistant.Item item = (Assistant.Item)t;

				if (item.ItemID == find && item.Hue == hue)
				{
					PlayerData.DoubleClick(item);
					return true;
				}
				else if (item.ItemID == find && hue == -1) // All color
				{
					PlayerData.DoubleClick(item);
					return true;
				}
				else if (item.Contains != null && item.Contains.Count > 0)
				{
					if (UseItemByIdHue(item, find, hue))
						return true;
				}
			}

			return false;
		}
	}
}
