using Assistant;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace RazorEnhanced
{
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

		internal static Keys m_key;

		internal static Keys m_Masterkey;

		internal static void OnMouse(int button, int wheel)
		{
			if (World.Player == null)
				return;

			switch (button)
			{
				case 0:
					{
						if (wheel == -1)
							KeyDown((Keys)501);
						else
							KeyDown((Keys)502);
						break;
					}
				case 1:
					KeyDown((Keys)500);
					break;

				case 2:
					KeyDown((Keys)503);
					break;

				case 3:
					KeyDown((Keys)504);
					break;
			}
		}

		internal static bool GameKeyDown(Keys k)
		{
			return KeyDown(k | Control.ModifierKeys);              // Aggiunta modificatori in quanto il passaggio key dal client non li supporta in modo diretto;
		}

		internal static bool KeyDown(Keys k)
		{
			if (!Engine.MainWindow.HotKeyTextBox.Focused && !Engine.MainWindow.HotKeyKeyMasterTextBox.Focused)
			{
				if (k == RazorEnhanced.Settings.General.ReadKey("HotKeyMasterKey"))         // Pressione master key abilita o disabilita
				{
					if (RazorEnhanced.Settings.General.ReadBool("HotKeyEnable"))
					{
						RazorEnhanced.Settings.General.WriteBool("HotKeyEnable", false);
						Assistant.Engine.MainWindow.HotKeyStatusLabel.Text = "Status: Disable";
						if (World.Player != null)
							World.Player.SendMessage("HotKey: DISABLED", 37);
					}
					else
					{
						Assistant.Engine.MainWindow.HotKeyStatusLabel.Text = "Status: Enable";
						RazorEnhanced.Settings.General.WriteBool("HotKeyEnable", true);
						if (World.Player != null)
							World.Player.SendMessage("HotKey: ENABLED", 168);
					}
				}
			}

			if (Engine.MainWindow.HotKeyTextBox.Focused)                // In caso di assegnazione hotKey normale
			{
				m_key = k;
				Engine.MainWindow.HotKeyTextBox.Text = KeyString(k);
				return false;
			}
			else if (Engine.MainWindow.HotKeyKeyMasterTextBox.Focused)                // In caso di assegnazione hotKey primaria
			{
				m_Masterkey = k;
				Engine.MainWindow.HotKeyKeyMasterTextBox.Text = KeyString(k);
				return false;
			}
			else    // Esecuzine reale
			{
				if (World.Player != null && RazorEnhanced.Settings.General.ReadBool("HotKeyEnable"))
				{
					bool passkey = true;
					string group = "";
					RazorEnhanced.Settings.HotKey.FindGroup(k, out group, out passkey);
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

				default:
					return k.ToString();
			}
		}

		private static void ProcessGroup(string group, Keys k)
		{
			if (group != "")
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

					case "Agents":
						ProcessAgents(RazorEnhanced.Settings.HotKey.FindString(k));
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
						RazorEnhanced.Spells.CastMagery(RazorEnhanced.Settings.HotKey.FindString(k));
						break;

					case "SpellsNecro":
						RazorEnhanced.Spells.CastNecro(RazorEnhanced.Settings.HotKey.FindString(k));
						break;

					case "SpellsBushido":
						RazorEnhanced.Spells.CastBushido(RazorEnhanced.Settings.HotKey.FindString(k));
						break;

					case "SpellsNinjitsu":
						RazorEnhanced.Spells.CastNinjitsu(RazorEnhanced.Settings.HotKey.FindString(k));
						break;

					case "SpellsSpellweaving":
						RazorEnhanced.Spells.CastSpellweaving(RazorEnhanced.Settings.HotKey.FindString(k));
						break;

					case "SpellsMysticism":
						RazorEnhanced.Spells.CastMysticism(RazorEnhanced.Settings.HotKey.FindString(k));
						break;

					case "SpellsChivalry":
						RazorEnhanced.Spells.CastChivalry(RazorEnhanced.Settings.HotKey.FindString(k));
						break;

					case "Target":
						ProcessTarget(RazorEnhanced.Settings.HotKey.FindString(k));
						break;

					case "TList":
						RazorEnhanced.Target.SetLastTargetFromList(RazorEnhanced.Settings.HotKey.FindTargetString(k));
						break;

					case "Script":
						if (RazorEnhanced.Settings.HotKey.FindString(k) == "Stop All")
						{
							World.Player.SendMessage("Stopping all scripts...",33);
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
							if (!script.Wait && script.IsRunning)
							{
								script.Stop();
								script.Reset();
							}
							else
							{
								if (script.IsStopped)
								{
									script.Reset();
								}

								if (script.IsUnstarted)
								{
									script.Start();
								}
							}
						}
						break;

					case "UseVirtue":
						RazorEnhanced.Player.InvokeVirtue(RazorEnhanced.Settings.HotKey.FindString(k));
						break;

					default:
						break;
				}
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

				case "Ping Server":
					Assistant.Ping.StartPing(4);
					break;

				case "Accept Party":
					if (PacketHandlers.PartyLeader != Assistant.Serial.Zero)
					{
						ClientCommunication.SendToServer(new AcceptParty(PacketHandlers.PartyLeader));
						PacketHandlers.PartyLeader = Assistant.Serial.Zero;
					}
					break;

				case "Decline Party":
					if (PacketHandlers.PartyLeader != Assistant.Serial.Zero)
					{
						ClientCommunication.SendToServer(new DeclineParty(PacketHandlers.PartyLeader));
						PacketHandlers.PartyLeader = Assistant.Serial.Zero;
					}
					break;

				default:
					break;
			}
		}

		private static void ProcessActions(string function)
		{
			switch (function)
			{
				case "Unmount":
					if (World.Player.GetItemOnLayer(Layer.Mount) != null)
						ActionQueue.DoubleClick(true, World.Player.Serial);
					else
						World.Player.SendMessage("You are not mounted.");
					break;

				case "Grab Item":
					World.Player.SendMessage("Target item to Grab.");
					Targeting.OneTimeTarget(new Targeting.TargetResponseCallback(grabitemTarget_Callback));
					break;

				case "Drop Item":
					World.Player.SendMessage("Target item to Drop at feet.");
					Targeting.OneTimeTarget(new Targeting.TargetResponseCallback(dropitemTarget_Callback));
					break;

				default:
					break;
			}
		}

		private static void grabitemTarget_Callback(bool loc, Assistant.Serial serial, Assistant.Point3D pt, ushort itemid)
		{
			Assistant.Item itemtograb = Assistant.World.FindItem(serial);

			if (itemtograb != null && itemtograb.Serial.IsItem && itemtograb.Movable)
				RazorEnhanced.Items.Move(itemtograb.Serial, World.Player.Backpack.Serial, 0);
			else
				World.Player.SendMessage("Invalid or inaccessible item.");
		}

		private static void dropitemTarget_Callback(bool loc, Assistant.Serial serial, Assistant.Point3D pt, ushort itemid)
		{
			Item itemtodrop = RazorEnhanced.Items.FindBySerial(serial);

			if (itemtodrop != null && itemtodrop.Movable && itemtodrop.RootContainer == World.Player)
				RazorEnhanced.Items.DropItemGroundSelf(itemtodrop, 0);
			else
				World.Player.SendMessage("Invalid or inaccessible item.");
		}

		private static void ProcessUse(string function)
		{
			Assistant.Item item;
			switch (function)
			{
				case "Last Item":
					if (World.Player.LastObject != Assistant.Serial.Zero)
						RazorEnhanced.Items.UseItem(World.Player.LastObject);
					break;

				case "Left Hand":
					item = World.Player.GetItemOnLayer(Layer.LeftHand);
					if (item != null)
						RazorEnhanced.Items.UseItem(item.Serial);
					break;

				case "Right Hand":
					item = World.Player.GetItemOnLayer(Layer.RightHand);
					if (item != null)
						RazorEnhanced.Items.UseItem(item.Serial);
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
							ClientCommunication.SendToServer(new SingleClick(m));

						if (RazorEnhanced.Settings.General.ReadBool("LastTargTextFlags"))
							Targeting.CheckTextFlags(m);
					}
					foreach (Assistant.Item i in World.Items.Values)
					{
						if (i.IsCorpse)
							ClientCommunication.SendToServer(new SingleClick(i));
					}
					break;

				case "Corpses":
					foreach (Assistant.Item i in World.Items.Values)
					{
						if (i.IsCorpse)
							ClientCommunication.SendToServer(new SingleClick(i));
					}
					break;

				case "Mobiles":
					foreach (Assistant.Mobile m in World.MobilesInRange())
					{
						if (m != World.Player)
							ClientCommunication.SendToServer(new SingleClick(m));

						if (RazorEnhanced.Settings.General.ReadBool("LastTargTextFlags"))
							Targeting.CheckTextFlags(m);
					}
					break;

				case "Items":
					foreach (Assistant.Item i in World.Items.Values)
					{
						ClientCommunication.SendToServer(new SingleClick(i));
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
				{
					Assistant.ClientCommunication.SendToServer(new DoubleClick(Filters.AutoRemountSerial));
				}
			}
			else if (function == "Dismount")
			{
				Assistant.ClientCommunication.SendToServer(new DoubleClick(World.Player.Serial));
			}
			else
			{
				RazorEnhanced.Player.ChatSay(RazorEnhanced.Settings.General.ReadInt("SpeechHue"), function);
			}
		}

		private static void ProcessAgents(string function)
		{
			switch (function)
			{
				case "Autoloot ON/OFF":
					if (RazorEnhanced.AutoLoot.Status())
						RazorEnhanced.AutoLoot.Stop();
					else
						RazorEnhanced.AutoLoot.Start();
					break;

				case "Scavenger ON/OFF":
					if (RazorEnhanced.Scavenger.Status())
						RazorEnhanced.Scavenger.Stop();
					else
						RazorEnhanced.Scavenger.Start();
					break;

				case "Organizer Start":
					RazorEnhanced.Organizer.FStart();
					break;

				case "Organizer Stop":
					RazorEnhanced.Organizer.FStop();
					break;

				case "Sell Agent ON/OFF":
					if (RazorEnhanced.SellAgent.Status())
						RazorEnhanced.SellAgent.Disable();
					else
						RazorEnhanced.SellAgent.Enable();
					break;

				case "Buy Agent ON/OFF":
					if (RazorEnhanced.BuyAgent.Status())
						RazorEnhanced.BuyAgent.Disable();
					else
						RazorEnhanced.BuyAgent.Enable();
					break;

				case "Dress Start":
					RazorEnhanced.Dress.DressFStart();
					break;

				case "Dress Stop":
					RazorEnhanced.Dress.DressFStop();
					break;

				case "Undress":
					RazorEnhanced.Dress.UnDressFStart();
					break;

				case "Restock Start":
					RazorEnhanced.Restock.FStart();
					break;

				case "Restock Stop":
					RazorEnhanced.Restock.FStop();
					break;

				case "Bandage Heal ON/OFF":
					if (RazorEnhanced.BandageHeal.Status())
						RazorEnhanced.BandageHeal.Stop();
					else
						RazorEnhanced.BandageHeal.Start();
					break;

				case "Bone Cutter ON/OFF":
					if (Assistant.Engine.MainWindow.BoneCutterCheckBox.Checked)
						Assistant.Engine.MainWindow.BoneCutterCheckBox.Checked = false;
					else
						Assistant.Engine.MainWindow.BoneCutterCheckBox.Checked = true;
					break;

				case "Auto Carver ON/OFF":
					if (Assistant.Engine.MainWindow.AutoCarverCheckBox.Checked)
						Assistant.Engine.MainWindow.AutoCarverCheckBox.Checked = false;
					else
						Assistant.Engine.MainWindow.AutoCarverCheckBox.Checked = true;
					break;

				case "Auto Remount ON/OFF":
					if (Assistant.Engine.MainWindow.RemountCheckbox.Checked)
						Assistant.Engine.MainWindow.RemountCheckbox.Checked = false;
					else
						Assistant.Engine.MainWindow.RemountCheckbox.Checked = true;
					break;

				case "Graphics Filter ON/OFF":
					if (Assistant.Engine.MainWindow.MobFilterCheckBox.Checked)
						Assistant.Engine.MainWindow.MobFilterCheckBox.Checked = false;
					else
						Assistant.Engine.MainWindow.MobFilterCheckBox.Checked = true;
					break;

				default:
					break;
			}
		}

		private static void ProcessAbilities(string function)
		{
			switch (function)
			{
				case "Primary":
					Assistant.SpecialMoves.SetPrimaryAbility();
					break;
				case "Secondary":
					Assistant.SpecialMoves.SetSecondaryAbility();
					break;
				case "Stun":
					Assistant.SpecialMoves.OnStun();
					break;
				case "Disarm":
					Assistant.SpecialMoves.OnDisarm();
					break;
				default:
					break;
			}
		}

		private static void ProcessAttack(string function)
		{
			switch (function)
			{
				default:
					uint target = Assistant.Targeting.GetLastTarger;
					RazorEnhanced.Player.Attack((int)target);
					break;
			}
		}

		private static void ProcessBandage(string function)
		{
			Assistant.Item pack = World.Player.Backpack;
			switch (function)
			{
				case "Self":
					if (pack != null)
					{
						if (!UseItemById(pack, 3617))
						{
							World.Player.SendMessage(MsgLevel.Warning, LocString.NoBandages);
						}
						else
						{
							Targeting.ClearQueue();
							Targeting.TargetSelf(true);
						}
					}
					break;

				case "Last":
					if (pack != null)
					{
						if (!UseItemById(pack, 3617))
						{
							World.Player.SendMessage(MsgLevel.Warning, LocString.NoBandages);
						}
						else
						{
							Targeting.ClearQueue();
							Targeting.LastTarget(true);
						}
					}
					break;

				case "Use Only":
					if (pack != null)
					{
						if (!UseItemById(pack, 3617))
							World.Player.SendMessage(MsgLevel.Warning, LocString.NoBandages);
					}
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
						if (!UseItemById(pack, 3848))
							World.Player.SendMessage(MsgLevel.Warning, "No potions left");
					}
					break;

				case "Potion Cure":
					if (pack != null)
					{
						if (!UseItemById(pack, 3847))
							World.Player.SendMessage(MsgLevel.Warning, "No potions left");
					}
					break;

				case "Potion Explosion":
					if (pack != null)
					{
						if (!UseItemById(pack, 3853))
							World.Player.SendMessage(MsgLevel.Warning, "No potions left");
					}
					break;

				case "Potion Heal":
					if (pack != null)
					{
						if (!UseItemById(pack, 3852))
							World.Player.SendMessage(MsgLevel.Warning, "No potions left");
					}
					break;

				case "Potion Refresh":
					if (pack != null)
					{
						if (!UseItemById(pack, 3851))
							World.Player.SendMessage(MsgLevel.Warning, "No potions left");
					}
					break;

				case "Potion Strenght":
					if (pack != null)
					{
						if (!UseItemById(pack, 3849))
							World.Player.SendMessage(MsgLevel.Warning, "No potions left");
					}
					break;

				case "Potion Nightsight":
					if (pack != null)
					{
						if (!UseItemById(pack, 3846))
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
						World.Player.SendMessage("Da implementare");
						if (!UseItemByIdHue(pack, 13848, 0))
							World.Player.SendMessage(MsgLevel.Warning, "No item left");
					}
					break;

				case "Wrath Grapes":
					if (pack != null)
					{
						World.Player.SendMessage("Da implementare");
						if (!UseItemByIdHue(pack, 13848, 0))
							World.Player.SendMessage(MsgLevel.Warning, "No item left");
					}
					break;

				case "Rose Of Trinsic":
					if (pack != null)
					{
						World.Player.SendMessage("Da implementare");
						if (!UseItemByIdHue(pack, 13848, 0))
							World.Player.SendMessage(MsgLevel.Warning, "No item left");
					}
					break;

				case "Smoke Bomb":
					if (pack != null)
					{
						World.Player.SendMessage("Da implementare");
						if (!UseItemByIdHue(pack, 13848, 0))
							World.Player.SendMessage(MsgLevel.Warning, "No item left");
					}
					break;

				case "Spell Stone":
					if (pack != null)
					{
						World.Player.SendMessage("Da implementare");
						if (!UseItemByIdHue(pack, 13848, 0))
							World.Player.SendMessage(MsgLevel.Warning, "No item left");
					}
					break;

				case "Healing Stone":
					if (pack != null)
					{
						World.Player.SendMessage("Da implementare");
						if (!UseItemByIdHue(pack, 13848, 0))
							World.Player.SendMessage(MsgLevel.Warning, "No item left");
					}
					break;

				default:
					break;
			}
		}

		private static void ProcessHands(string function)
		{
			switch (function)
			{
				case "Clear Left":
					RazorEnhanced.Player.UnEquipItemByLayer("LeftHand");
					break;

				case "Clear Right":
					RazorEnhanced.Player.UnEquipItemByLayer("RightHand");
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
					ClientCommunication.SendToServer(new UseSkill(World.Player.LastSkill));
				}
			}
			else
			{
				RazorEnhanced.Player.UseSkill(function);
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

				default:
					break;
			}
		}

		private static void ProcessTarget(string function)
		{
			switch (function)
			{
				case "Target Self":
					RazorEnhanced.Target.Self();
					break;

				case "Target Last":
					Assistant.Targeting.LastTarget();
					break;

				case "Target Self Queued":
					Assistant.ClientCommunication.SendToClient(new UnicodeMessage(World.Player.Serial, World.Player.Body, MessageType.Regular, 993, 3, Language.CliLocName, World.Player.Name, "Target \"self\" queued."));
					Assistant.Targeting.TargetSelf(true);
					break;

				case "Target Last Queued":
					Assistant.ClientCommunication.SendToClient(new UnicodeMessage(World.Player.Serial, World.Player.Body, MessageType.Regular, 993, 3, Language.CliLocName, World.Player.Name, "Target \"last\" queued."));
					Assistant.Targeting.LastTarget(true);
					break;

				case "Target Cancel":
					RazorEnhanced.Target.Cancel();
					break;

				case "Clear Target Queue":
					Assistant.ClientCommunication.SendToClient(new UnicodeMessage(World.Player.Serial, World.Player.Body, MessageType.Regular, 993, 3, Language.CliLocName, World.Player.Name, "Target \"queue\" cleared."));
					Assistant.Targeting.ClearQueue();
					break;

				case "Clear Last Target":
					Assistant.ClientCommunication.SendToClient(new UnicodeMessage(World.Player.Serial, World.Player.Body, MessageType.Regular, 993, 3, Language.CliLocName, World.Player.Name, "Target \"last\" cleared."));
					Assistant.Targeting.ClearLast();
                    break;

				case "Clear Last and Queue":
					Assistant.ClientCommunication.SendToClient(new UnicodeMessage(World.Player.Serial, World.Player.Body, MessageType.Regular, 993, 3, Language.CliLocName, World.Player.Name, "Target \"last and queue\" cleared."));
					Assistant.Targeting.ClearLast();
					Assistant.Targeting.ClearQueue();
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
			{
				Engine.MainWindow.HotKeyTreeView.Nodes[0].Nodes[0].Nodes.Add(keydata.Name, keydata.Name + " ( " + KeyString(keydata.Key) + " )");
			}

			// Actions
			Engine.MainWindow.HotKeyTreeView.Nodes[0].Nodes.Add("Actions");
			keylist = RazorEnhanced.Settings.HotKey.ReadGroup("Actions");
			foreach (HotKeyData keydata in keylist)
			{
				TreeNode a = new TreeNode();
				a.Name = keydata.Name;
				a.Text = keydata.Name + " ( " + KeyString(keydata.Key) + " )";
				if (keydata.Key != Keys.None)
					a.ForeColor = System.Drawing.Color.DarkGreen;
				Engine.MainWindow.HotKeyTreeView.Nodes[0].Nodes[1].Nodes.Add(a);
			}

			// Actions -> Use
			Engine.MainWindow.HotKeyTreeView.Nodes[0].Nodes[1].Nodes.Add("Use");
			keylist = RazorEnhanced.Settings.HotKey.ReadGroup("Use");
			foreach (HotKeyData keydata in keylist)
			{
				TreeNode a = new TreeNode();
				a.Name = keydata.Name;
				a.Text = keydata.Name + " ( " + KeyString(keydata.Key) + " )";
				if (keydata.Key != Keys.None)
					a.ForeColor = System.Drawing.Color.DarkGreen;
				Engine.MainWindow.HotKeyTreeView.Nodes[0].Nodes[1].Nodes[3].Nodes.Add(a);
			}

			// Actions -> Show Names
			Engine.MainWindow.HotKeyTreeView.Nodes[0].Nodes[1].Nodes.Add("Show Names");
			keylist = RazorEnhanced.Settings.HotKey.ReadGroup("Show Names");
			foreach (HotKeyData keydata in keylist)
			{
				TreeNode a = new TreeNode();
				a.Name = keydata.Name;
				a.Text = keydata.Name + " ( " + KeyString(keydata.Key) + " )";
				if (keydata.Key != Keys.None)
					a.ForeColor = System.Drawing.Color.DarkGreen;
				Engine.MainWindow.HotKeyTreeView.Nodes[0].Nodes[1].Nodes[4].Nodes.Add(a);
			}

			// Actions -> Per Commands
			Engine.MainWindow.HotKeyTreeView.Nodes[0].Nodes[1].Nodes.Add("Pet Commands");
			keylist = RazorEnhanced.Settings.HotKey.ReadGroup("Pet Commands");
			foreach (HotKeyData keydata in keylist)
			{
				TreeNode a = new TreeNode();
				a.Name = keydata.Name;
				a.Text = keydata.Name + " ( " + KeyString(keydata.Key) + " )";
				if (keydata.Key != Keys.None)
					a.ForeColor = System.Drawing.Color.DarkGreen;
				Engine.MainWindow.HotKeyTreeView.Nodes[0].Nodes[1].Nodes[5].Nodes.Add(a);
			}

			// Agents
			Engine.MainWindow.HotKeyTreeView.Nodes[0].Nodes.Add("Agents");
			keylist = RazorEnhanced.Settings.HotKey.ReadGroup("Agents");
			foreach (HotKeyData keydata in keylist)
			{
				TreeNode a = new TreeNode();
				a.Name = keydata.Name;
				a.Text = keydata.Name + " ( " + KeyString(keydata.Key) + " )";
				if (keydata.Key != Keys.None)
					a.ForeColor = System.Drawing.Color.DarkGreen;
				Engine.MainWindow.HotKeyTreeView.Nodes[0].Nodes[2].Nodes.Add(a);
			}

			// Combats
			Engine.MainWindow.HotKeyTreeView.Nodes[0].Nodes.Add("Combat");

			// Combat  --> Abilities
			Engine.MainWindow.HotKeyTreeView.Nodes[0].Nodes[3].Nodes.Add("Abilities");
			keylist = RazorEnhanced.Settings.HotKey.ReadGroup("Abilities");
			foreach (HotKeyData keydata in keylist)
			{
				TreeNode a = new TreeNode();
				a.Name = keydata.Name;
				a.Text = keydata.Name + " ( " + KeyString(keydata.Key) + " )";
				if (keydata.Key != Keys.None)
					a.ForeColor = System.Drawing.Color.DarkGreen;
				Engine.MainWindow.HotKeyTreeView.Nodes[0].Nodes[3].Nodes[0].Nodes.Add(a);
			}

			// Combat  --> Attack
			Engine.MainWindow.HotKeyTreeView.Nodes[0].Nodes[3].Nodes.Add("Attack");
			keylist = RazorEnhanced.Settings.HotKey.ReadGroup("Attack");
			foreach (HotKeyData keydata in keylist)
			{
				TreeNode a = new TreeNode();
				a.Name = keydata.Name;
				a.Text = keydata.Name + " ( " + KeyString(keydata.Key) + " )";
				if (keydata.Key != Keys.None)
					a.ForeColor = System.Drawing.Color.DarkGreen;
				Engine.MainWindow.HotKeyTreeView.Nodes[0].Nodes[3].Nodes[1].Nodes.Add(a);
			}

			// Combat  --> Bandage
			Engine.MainWindow.HotKeyTreeView.Nodes[0].Nodes[3].Nodes.Add("Bandage");
			keylist = RazorEnhanced.Settings.HotKey.ReadGroup("Bandage");
			foreach (HotKeyData keydata in keylist)
			{
				TreeNode a = new TreeNode();
				a.Name = keydata.Name;
				a.Text = keydata.Name + " ( " + KeyString(keydata.Key) + " )";
				if (keydata.Key != Keys.None)
					a.ForeColor = System.Drawing.Color.DarkGreen;
				Engine.MainWindow.HotKeyTreeView.Nodes[0].Nodes[3].Nodes[2].Nodes.Add(a);
			}

			// Combat  --> Consumable
			Engine.MainWindow.HotKeyTreeView.Nodes[0].Nodes[3].Nodes.Add("Consumable");

			// Combat  --> Consumable --> Potions
			Engine.MainWindow.HotKeyTreeView.Nodes[0].Nodes[3].Nodes[3].Nodes.Add("Potions");
			keylist = RazorEnhanced.Settings.HotKey.ReadGroup("Potions");
			foreach (HotKeyData keydata in keylist)
			{
				TreeNode a = new TreeNode();
				a.Name = keydata.Name;
				a.Text = keydata.Name + " ( " + KeyString(keydata.Key) + " )";
				if (keydata.Key != Keys.None)
					a.ForeColor = System.Drawing.Color.DarkGreen;
				Engine.MainWindow.HotKeyTreeView.Nodes[0].Nodes[3].Nodes[3].Nodes[0].Nodes.Add(a);
			}

			// Combat --> Consumable --> Other
			Engine.MainWindow.HotKeyTreeView.Nodes[0].Nodes[3].Nodes[3].Nodes.Add("Other");
			keylist = RazorEnhanced.Settings.HotKey.ReadGroup("Other");
			foreach (HotKeyData keydata in keylist)
			{
				TreeNode a = new TreeNode();
				a.Name = keydata.Name;
				a.Text = keydata.Name + " ( " + KeyString(keydata.Key) + " )";
				if (keydata.Key != Keys.None)
					a.ForeColor = System.Drawing.Color.DarkGreen;
				Engine.MainWindow.HotKeyTreeView.Nodes[0].Nodes[3].Nodes[3].Nodes[1].Nodes.Add(a);
			}

			// Combat --> Hands
			Engine.MainWindow.HotKeyTreeView.Nodes[0].Nodes[3].Nodes.Add("Hands");
			keylist = RazorEnhanced.Settings.HotKey.ReadGroup("Hands");
			foreach (HotKeyData keydata in keylist)
			{
				TreeNode a = new TreeNode();
				a.Name = keydata.Name;
				a.Text = keydata.Name + " ( " + KeyString(keydata.Key) + " )";
				if (keydata.Key != Keys.None)
					a.ForeColor = System.Drawing.Color.DarkGreen;
				Engine.MainWindow.HotKeyTreeView.Nodes[0].Nodes[3].Nodes[4].Nodes.Add(a);
			}

			// Combat --> Hands -> Equip Wands
			Engine.MainWindow.HotKeyTreeView.Nodes[0].Nodes[3].Nodes.Add("Equip Wands");
			keylist = RazorEnhanced.Settings.HotKey.ReadGroup("Equip Wands");
			foreach (HotKeyData keydata in keylist)
			{
				TreeNode a = new TreeNode();
				a.Name = keydata.Name;
				a.Text = keydata.Name + " ( " + KeyString(keydata.Key) + " )";
				if (keydata.Key != Keys.None)
					a.ForeColor = System.Drawing.Color.DarkGreen;
				Engine.MainWindow.HotKeyTreeView.Nodes[0].Nodes[3].Nodes[5].Nodes.Add(a);
			}

			// Skills
			Engine.MainWindow.HotKeyTreeView.Nodes[0].Nodes.Add("Skills");
			keylist = RazorEnhanced.Settings.HotKey.ReadGroup("Skills");
			foreach (HotKeyData keydata in keylist)
			{
				TreeNode a = new TreeNode();
				a.Name = keydata.Name;
				a.Text = keydata.Name + " ( " + KeyString(keydata.Key) + " )";
				if (keydata.Key != Keys.None)
					a.ForeColor = System.Drawing.Color.DarkGreen;
				Engine.MainWindow.HotKeyTreeView.Nodes[0].Nodes[4].Nodes.Add(a);
			}

			// Spells
			Engine.MainWindow.HotKeyTreeView.Nodes[0].Nodes.Add("Spells");
			keylist = RazorEnhanced.Settings.HotKey.ReadGroup("SpellsAgent");
			foreach (HotKeyData keydata in keylist)
			{
				TreeNode a = new TreeNode();
				a.Name = keydata.Name;
				a.Text = keydata.Name + " ( " + KeyString(keydata.Key) + " )";
				if (keydata.Key != Keys.None)
					a.ForeColor = System.Drawing.Color.DarkGreen;
				Engine.MainWindow.HotKeyTreeView.Nodes[0].Nodes[5].Nodes.Add(a);
			}

			// Spells -- > Magery
			Engine.MainWindow.HotKeyTreeView.Nodes[0].Nodes[5].Nodes.Add("Magery");
			keylist = RazorEnhanced.Settings.HotKey.ReadGroup("SpellsMagery");
			foreach (HotKeyData keydata in keylist)
			{
				TreeNode a = new TreeNode();
				a.Name = keydata.Name;
				a.Text = keydata.Name + " ( " + KeyString(keydata.Key) + " )";
				if (keydata.Key != Keys.None)
					a.ForeColor = System.Drawing.Color.DarkGreen;
				Engine.MainWindow.HotKeyTreeView.Nodes[0].Nodes[5].Nodes[3].Nodes.Add(a);
			}

			// Spells -- > Necro
			Engine.MainWindow.HotKeyTreeView.Nodes[0].Nodes[5].Nodes.Add("Necro");
			keylist = RazorEnhanced.Settings.HotKey.ReadGroup("SpellsNecro");
			foreach (HotKeyData keydata in keylist)
			{
				TreeNode a = new TreeNode();
				a.Name = keydata.Name;
				a.Text = keydata.Name + " ( " + KeyString(keydata.Key) + " )";
				if (keydata.Key != Keys.None)
					a.ForeColor = System.Drawing.Color.DarkGreen;
				Engine.MainWindow.HotKeyTreeView.Nodes[0].Nodes[5].Nodes[4].Nodes.Add(a);
			}

			// Spells -- > Bushido
			Engine.MainWindow.HotKeyTreeView.Nodes[0].Nodes[5].Nodes.Add("Bushido");
			keylist = RazorEnhanced.Settings.HotKey.ReadGroup("SpellsBushido");
			foreach (HotKeyData keydata in keylist)
			{
				TreeNode a = new TreeNode();
				a.Name = keydata.Name;
				a.Text = keydata.Name + " ( " + KeyString(keydata.Key) + " )";
				if (keydata.Key != Keys.None)
					a.ForeColor = System.Drawing.Color.DarkGreen;
				Engine.MainWindow.HotKeyTreeView.Nodes[0].Nodes[5].Nodes[5].Nodes.Add(a);
			}

			// Spells -- > Ninjitsu
			Engine.MainWindow.HotKeyTreeView.Nodes[0].Nodes[5].Nodes.Add("Ninjitsu");
			keylist = RazorEnhanced.Settings.HotKey.ReadGroup("SpellsNinjitsu");
			foreach (HotKeyData keydata in keylist)
			{
				TreeNode a = new TreeNode();
				a.Name = keydata.Name;
				a.Text = keydata.Name + " ( " + KeyString(keydata.Key) + " )";
				if (keydata.Key != Keys.None)
					a.ForeColor = System.Drawing.Color.DarkGreen;
				Engine.MainWindow.HotKeyTreeView.Nodes[0].Nodes[5].Nodes[6].Nodes.Add(a);
			}

			// Spells -- > Spellweaving
			Engine.MainWindow.HotKeyTreeView.Nodes[0].Nodes[5].Nodes.Add("Spellweaving");
			keylist = RazorEnhanced.Settings.HotKey.ReadGroup("SpellsSpellweaving");
			foreach (HotKeyData keydata in keylist)
			{
				TreeNode a = new TreeNode();
				a.Name = keydata.Name;
				a.Text = keydata.Name + " ( " + KeyString(keydata.Key) + " )";
				if (keydata.Key != Keys.None)
					a.ForeColor = System.Drawing.Color.DarkGreen;
				Engine.MainWindow.HotKeyTreeView.Nodes[0].Nodes[5].Nodes[7].Nodes.Add(a);
			}

			// Spells -- > Spellweaving
			Engine.MainWindow.HotKeyTreeView.Nodes[0].Nodes[5].Nodes.Add("Mysticism");
			keylist = RazorEnhanced.Settings.HotKey.ReadGroup("SpellsMysticism");
			foreach (HotKeyData keydata in keylist)
			{
				TreeNode a = new TreeNode();
				a.Name = keydata.Name;
				a.Text = keydata.Name + " ( " + KeyString(keydata.Key) + " )";
				if (keydata.Key != Keys.None)
					a.ForeColor = System.Drawing.Color.DarkGreen;
				Engine.MainWindow.HotKeyTreeView.Nodes[0].Nodes[5].Nodes[8].Nodes.Add(a);
			}

			// Spells -- > Chivalry
			Engine.MainWindow.HotKeyTreeView.Nodes[0].Nodes[5].Nodes.Add("Chivalry");
			keylist = RazorEnhanced.Settings.HotKey.ReadGroup("SpellsChivalry");
			foreach (HotKeyData keydata in keylist)
			{
				TreeNode a = new TreeNode();
				a.Name = keydata.Name;
				a.Text = keydata.Name + " ( " + KeyString(keydata.Key) + " )";
				if (keydata.Key != Keys.None)
					a.ForeColor = System.Drawing.Color.DarkGreen;
				Engine.MainWindow.HotKeyTreeView.Nodes[0].Nodes[5].Nodes[9].Nodes.Add(a);
			}

			// Target
			Engine.MainWindow.HotKeyTreeView.Nodes[0].Nodes.Add("Target");
			keylist = RazorEnhanced.Settings.HotKey.ReadGroup("Target");
			foreach (HotKeyData keydata in keylist)
			{
				TreeNode a = new TreeNode();
				a.Name = keydata.Name;
				a.Text = keydata.Name + " ( " + KeyString(keydata.Key) + " )";
				if (keydata.Key != Keys.None)
					a.ForeColor = System.Drawing.Color.DarkGreen;
				Engine.MainWindow.HotKeyTreeView.Nodes[0].Nodes[6].Nodes.Add(a);
			}

			// Target -> List
			Engine.MainWindow.HotKeyTreeView.Nodes[0].Nodes[6].Nodes.Add("TList", "List");
			keylist = RazorEnhanced.Settings.HotKey.ReadTarget();
			foreach (HotKeyData keydata in keylist)
			{
				TreeNode a = new TreeNode();
				a.Name = keydata.Name;
				a.Text = keydata.Name + " ( " + KeyString(keydata.Key) + " )";
				if (keydata.Key != Keys.None)
					a.ForeColor = System.Drawing.Color.DarkGreen;
				Engine.MainWindow.HotKeyTreeView.Nodes[0].Nodes[6].Nodes[8].Nodes.Add(a);
			}

			// Script
			Engine.MainWindow.HotKeyTreeView.Nodes[0].Nodes.Add("Script");
			keylist = RazorEnhanced.Settings.HotKey.ReadGroup("Script");
			foreach (HotKeyData keydata in keylist)
			{
				TreeNode a = new TreeNode();
				a.Name = keydata.Name;
				a.Text = keydata.Name + " ( " + KeyString(keydata.Key) + " )";
				if (keydata.Key != Keys.None)
					a.ForeColor = System.Drawing.Color.DarkGreen;
				Engine.MainWindow.HotKeyTreeView.Nodes[0].Nodes[7].Nodes.Add(a);
			}

			// Script -> List
			Engine.MainWindow.HotKeyTreeView.Nodes[0].Nodes[7].Nodes.Add("SList", "List");
			keylist = RazorEnhanced.Settings.HotKey.ReadScript();
			foreach (HotKeyData keydata in keylist)
			{
				TreeNode a = new TreeNode();
				a.Name = keydata.Name;
				a.Text = keydata.Name + " ( " + KeyString(keydata.Key) + " )";
				if (keydata.Key != Keys.None)
					a.ForeColor = System.Drawing.Color.DarkGreen;
				Engine.MainWindow.HotKeyTreeView.Nodes[0].Nodes[7].Nodes[1].Nodes.Add(a);
			}

			// Virtue
			Engine.MainWindow.HotKeyTreeView.Nodes[0].Nodes.Add("Virtue");
			keylist = RazorEnhanced.Settings.HotKey.ReadGroup("UseVirtue");
			foreach (HotKeyData keydata in keylist)
			{
				TreeNode a = new TreeNode();
				a.Name = keydata.Name;
				a.Text = keydata.Name + " ( " + KeyString(keydata.Key) + " )";
				if (keydata.Key != Keys.None)
					a.ForeColor = System.Drawing.Color.DarkGreen;
				Engine.MainWindow.HotKeyTreeView.Nodes[0].Nodes[8].Nodes.Add(a);
			}

			Engine.MainWindow.HotKeyTreeView.Nodes[0].Expand();
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
			if (!RazorEnhanced.Settings.HotKey.AssignedKey(m_Masterkey))
			{
				RazorEnhanced.Settings.General.WriteKey("HotKeyMasterKey", RazorEnhanced.HotKey.m_Masterkey);
				Assistant.Engine.MainWindow.HotKeyKeyMasterLabel.Text = "ON/OFF Key: " + KeyString(RazorEnhanced.HotKey.m_Masterkey);
			}
			else
			{
				DialogResult dialogResult = MessageBox.Show("Key: " + KeyString(m_Masterkey) + " already assigned! Want replace?", "HotKey", MessageBoxButtons.YesNo);
				if (dialogResult == DialogResult.Yes)
				{
					RazorEnhanced.Settings.HotKey.UnassignKey(m_Masterkey);
					RazorEnhanced.Settings.General.WriteKey("HotKeyMasterKey", RazorEnhanced.HotKey.m_Masterkey);
					Assistant.Engine.MainWindow.HotKeyKeyMasterLabel.Text = "ON/OFF Key: " + KeyString(RazorEnhanced.HotKey.m_Masterkey);
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

		private static bool UseItemById(Assistant.Item cont, ushort find)
		{
			for (int i = 0; i < cont.Contains.Count; i++)
			{
				Assistant.Item item = (Assistant.Item)cont.Contains[i];

				if (item.ItemID == find)
				{
					RazorEnhanced.Items.UseItem(item.Serial);
					return true;
				}
				else if (item.Contains != null && item.Contains.Count > 0)
				{
					if (UseItemById(item, find))
						return true;
				}
			}
			return false;
		}

		private static bool UseItemByIdHue(Assistant.Item cont, ushort find, ushort hue)
		{
			for (int i = 0; i < cont.Contains.Count; i++)
			{
				Assistant.Item item = (Assistant.Item)cont.Contains[i];

				if (item.ItemID == find && item.Hue == hue)
				{
					RazorEnhanced.Items.UseItem(item.Serial);
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