using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Runtime.InteropServices;
using System.Text;
using Assistant;


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

        [DllImport("user32.dll")]
        private static extern ushort GetAsyncKeyState(int key);

        internal static bool GameKeyDown(Keys k)
        {
            KeyDown(k | Control.ModifierKeys);              // Aggiunta modificatori in quanto il passaggio key dal client non li supporta in modo diretto
            return true;
        }

        internal static void KeyDown(Keys k)
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
                            RazorEnhanced.Misc.SendMessage("HotKey: DISABLED");
                    }
                    else
                    {
                        Assistant.Engine.MainWindow.HotKeyStatusLabel.Text = "Status: Enable";
                        RazorEnhanced.Settings.General.WriteBool("HotKeyEnable", true);
                        if (World.Player != null)
                            RazorEnhanced.Misc.SendMessage("HotKey: ENABLED");
                    }
                    return;
                }
            }

            if (Engine.MainWindow.HotKeyTextBox.Focused)                // In caso di assegnazione hotKey normale
            {
                m_key = k;
                Engine.MainWindow.HotKeyTextBox.Text = k.ToString();
            }
            else if (Engine.MainWindow.HotKeyKeyMasterTextBox.Focused)                // In caso di assegnazione hotKey primaria
            {
                m_Masterkey = k;
                Engine.MainWindow.HotKeyKeyMasterTextBox.Text = k.ToString();
            }
            else    // Esecuzine reale
            {
                if (World.Player != null && RazorEnhanced.Settings.General.ReadBool("HotKeyEnable"))
                 ProcessGroup(RazorEnhanced.Settings.HotKey.FindGroup(k), k);
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
                        ProcessPetCommands(RazorEnhanced.Settings.HotKey.FindString(k));
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
                    case "LTarget":
                        TargetGUI.PerformTarget(RazorEnhanced.Settings.HotKey.FindTargetString(k));
                        break;
                    case "Script":
                        
                        break;
                    case "LScript":
                       // ProcessScriptList(RazorEnhanced.Settings.HotKey.FindScriptString(k));
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
                    World.Player.SendMessage("Da implementare");
                    break;
                case "Drop Item":
                    World.Player.SendMessage("Da implementare");
                    break;
                default:
                    break;
            }
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
        private static void ProcessPetCommands(string function)
        {
            switch (function)
            {
                case "Come":
                    RazorEnhanced.Player.ChatSay(RazorEnhanced.Settings.General.ReadInt("SpeechHue"), "All Come");
                    break;
                case "Follow":
                    RazorEnhanced.Player.ChatSay(RazorEnhanced.Settings.General.ReadInt("SpeechHue"), "All Follow");
                    break;
                case "Guard":
                    RazorEnhanced.Player.ChatSay(RazorEnhanced.Settings.General.ReadInt("SpeechHue"), "All Guard");
                    break;
                case "Kill":
                    RazorEnhanced.Player.ChatSay(RazorEnhanced.Settings.General.ReadInt("SpeechHue"), "All Kill");
                    break;
                case "Stay":
                    RazorEnhanced.Player.ChatSay(RazorEnhanced.Settings.General.ReadInt("SpeechHue"), "Stay");
                    break;
                case "Stop":
                    RazorEnhanced.Player.ChatSay(RazorEnhanced.Settings.General.ReadInt("SpeechHue"), "Stop");
                    break;
                default:
                    break;
            }
        }

        private static void ProcessAgents(string function)
        {
            switch (function)
            {
                case "Autoloot Start":
                    RazorEnhanced.AutoLoot.Start();
                    break;
                case "Autoloot Stop":
                    RazorEnhanced.AutoLoot.Stop();
                    break;
                case "Scavenger Start":
                    RazorEnhanced.Scavenger.Start();
                    break;
                case "Scavenger Stop":
                    RazorEnhanced.Scavenger.Stop();
                    break;
                case "Organizer Start":
                    RazorEnhanced.Organizer.FStop();
                    break;
                case "Organizer Stop":
                    RazorEnhanced.Organizer.FStart();
                    break;
                case "Sell Agent Enable":
                    RazorEnhanced.SellAgent.Enable();
                    break;
                case "Sell Agent Disable":
                    RazorEnhanced.SellAgent.Disable();
                    break;
                case "Buy Agent Enable":
                    RazorEnhanced.BuyAgent.Enable();
                    break;
                case "Buy Agent Disable":
                    RazorEnhanced.BuyAgent.Disable();
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
                case "Bandage Heal Enable":
                    RazorEnhanced.BandageHeal.Start();
                    break;
                case "Bandage Heal Dasable":
                    RazorEnhanced.BandageHeal.Stop();
                    break;
                default:
                    break;
            }
        }
        private static void ProcessAbilities(string function)
        {
            switch (function)
            {
                default:
                    break;
            }
        }
        private static void ProcessAttack(string function)
        {
            switch (function)
            {
                default:
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
                case "Agility":
                    if (pack != null)
                    {
                        if (!UseItemById(pack, 3848))
                            World.Player.SendMessage(MsgLevel.Warning, "No potions left");
                    }
                    break;
                case "Cure":
                    if (pack != null)
                    {
                        if (!UseItemById(pack, 3847))
                            World.Player.SendMessage(MsgLevel.Warning, "No potions left");
                    }
                    break;
                case "Explosion":
                    if (pack != null)
                    {
                        if (!UseItemById(pack, 3853))
                            World.Player.SendMessage(MsgLevel.Warning, "No potions left");
                    }
                    break;
                case "Heal":
                    if (pack != null)
                    {
                        if (!UseItemById(pack, 3852))
                            World.Player.SendMessage(MsgLevel.Warning, "No potions left");
                    }
                    break;
                case "Refresh":
                    if (pack != null)
                    {
                        if (!UseItemById(pack, 3851))
                            World.Player.SendMessage(MsgLevel.Warning, "No potions left");
                    }
                    break;
                case "Strenght":
                    if (pack != null)
                    {
                        if (!UseItemById(pack, 3849))
                            World.Player.SendMessage(MsgLevel.Warning, "No potions left");
                    }
                    break;
                case "Nightsight":
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
                    break;
            }
        }
        private static void ProcessSkills(string function)
        {
            if (function == "Last Used")
            {
                if (World.Player.LastSkill != -1)
                    ClientCommunication.SendToServer(new UseSkill(World.Player.LastSkill));
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
                case "Target Cancel":
                    RazorEnhanced.Target.Cancel();
                    break;
                default:
                    break;
            }
        }

        private static void ProcessScript(string function)
        {
            switch (function)
            {
                default:
                    break;
            }
        }
        private static void ProcessScriptList(string function)
        {
            switch (function)
            {
                default:
                    break;
            }
        }

        internal static void Init()
        {
            Engine.MainWindow.HotKeyTreeView.Nodes.Clear();
            Engine.MainWindow.HotKeyTreeView.Nodes.Add("HotKeys");
            Engine.MainWindow.HotKeyTreeView.Nodes[0].Nodes.Add("General");

            // General
            List<HotKeyData> keylist = RazorEnhanced.Settings.HotKey.ReadGroup("General");
            foreach(HotKeyData keydata in keylist)
            {
                    Engine.MainWindow.HotKeyTreeView.Nodes[0].Nodes[0].Nodes.Add(keydata.Name, keydata.Name + " ( " + keydata.Key.ToString() + " )");
            }

            // Actions
            Engine.MainWindow.HotKeyTreeView.Nodes[0].Nodes.Add("Actions");
            keylist = RazorEnhanced.Settings.HotKey.ReadGroup("Actions");
            foreach (HotKeyData keydata in keylist)
            {
                    Engine.MainWindow.HotKeyTreeView.Nodes[0].Nodes[1].Nodes.Add(keydata.Name, keydata.Name + " ( " + keydata.Key.ToString() + " )");
            }

            // Actions -> Use
            Engine.MainWindow.HotKeyTreeView.Nodes[0].Nodes[1].Nodes.Add("Use");
            keylist = RazorEnhanced.Settings.HotKey.ReadGroup("Use");
            foreach (HotKeyData keydata in keylist)
            {
                    Engine.MainWindow.HotKeyTreeView.Nodes[0].Nodes[1].Nodes[3].Nodes.Add(keydata.Name, keydata.Name + " ( " + keydata.Key.ToString() + " )");
            }

            // Actions -> Show Names
            Engine.MainWindow.HotKeyTreeView.Nodes[0].Nodes[1].Nodes.Add("Show Names");
            keylist = RazorEnhanced.Settings.HotKey.ReadGroup("Show Names");
            foreach (HotKeyData keydata in keylist)
            {
                    Engine.MainWindow.HotKeyTreeView.Nodes[0].Nodes[1].Nodes[4].Nodes.Add(keydata.Name, keydata.Name + " ( " + keydata.Key.ToString() + " )");
            }

            // Actions -> Per Commands
            Engine.MainWindow.HotKeyTreeView.Nodes[0].Nodes[1].Nodes.Add("Pet Commands");
            keylist = RazorEnhanced.Settings.HotKey.ReadGroup("Pet Commands");
            foreach (HotKeyData keydata in keylist)
            {
                    Engine.MainWindow.HotKeyTreeView.Nodes[0].Nodes[1].Nodes[5].Nodes.Add(keydata.Name, keydata.Name + " ( " + keydata.Key.ToString() + " )");
            }

            // Agents
            Engine.MainWindow.HotKeyTreeView.Nodes[0].Nodes.Add("Agents");
            keylist = RazorEnhanced.Settings.HotKey.ReadGroup("Agents");
            foreach (HotKeyData keydata in keylist)
            {
                    Engine.MainWindow.HotKeyTreeView.Nodes[0].Nodes[2].Nodes.Add(keydata.Name, keydata.Name + " ( " + keydata.Key.ToString() + " )");
            }

            // Combats
            Engine.MainWindow.HotKeyTreeView.Nodes[0].Nodes.Add("Combat");

            // Combat  --> Abilities
            Engine.MainWindow.HotKeyTreeView.Nodes[0].Nodes[3].Nodes.Add("Abilities");
            keylist = RazorEnhanced.Settings.HotKey.ReadGroup("Abilities");
            foreach (HotKeyData keydata in keylist)
            {
                    Engine.MainWindow.HotKeyTreeView.Nodes[0].Nodes[3].Nodes[0].Nodes.Add(keydata.Name, keydata.Name + " ( " + keydata.Key.ToString() + " )");
            }

            // Combat  --> Attack
            Engine.MainWindow.HotKeyTreeView.Nodes[0].Nodes[3].Nodes.Add("Attack");
            keylist = RazorEnhanced.Settings.HotKey.ReadGroup("Attack");
            foreach (HotKeyData keydata in keylist)
            {
                    Engine.MainWindow.HotKeyTreeView.Nodes[0].Nodes[3].Nodes[1].Nodes.Add(keydata.Name, keydata.Name + " ( " + keydata.Key.ToString() + " )");
            }

            // Combat  --> Bandage
            Engine.MainWindow.HotKeyTreeView.Nodes[0].Nodes[3].Nodes.Add("Bandage");
            keylist = RazorEnhanced.Settings.HotKey.ReadGroup("Bandage");
            foreach (HotKeyData keydata in keylist)
            {
                    Engine.MainWindow.HotKeyTreeView.Nodes[0].Nodes[3].Nodes[2].Nodes.Add(keydata.Name, keydata.Name + " ( " + keydata.Key.ToString() + " )");
            }

            // Combat  --> Consumable
            Engine.MainWindow.HotKeyTreeView.Nodes[0].Nodes[3].Nodes.Add("Consumable");

            // Combat  --> Consumable --> Potions
            Engine.MainWindow.HotKeyTreeView.Nodes[0].Nodes[3].Nodes[3].Nodes.Add("Potions");
            keylist = RazorEnhanced.Settings.HotKey.ReadGroup("Potions");
            foreach (HotKeyData keydata in keylist)
            {
                    Engine.MainWindow.HotKeyTreeView.Nodes[0].Nodes[3].Nodes[3].Nodes[0].Nodes.Add(keydata.Name, keydata.Name + " ( " + keydata.Key.ToString() + " )");
            }

            // Combat --> Consumable --> Other
            Engine.MainWindow.HotKeyTreeView.Nodes[0].Nodes[3].Nodes[3].Nodes.Add("Other");
            keylist = RazorEnhanced.Settings.HotKey.ReadGroup("Other");
            foreach (HotKeyData keydata in keylist)
            {
                Engine.MainWindow.HotKeyTreeView.Nodes[0].Nodes[3].Nodes[3].Nodes[1].Nodes.Add(keydata.Name, keydata.Name + " ( " + keydata.Key.ToString() + " )");
            }

            // Combat --> Hands
            Engine.MainWindow.HotKeyTreeView.Nodes[0].Nodes[3].Nodes.Add("Hands");
            keylist = RazorEnhanced.Settings.HotKey.ReadGroup("Hands");
            foreach (HotKeyData keydata in keylist)
            {
                Engine.MainWindow.HotKeyTreeView.Nodes[0].Nodes[3].Nodes[4].Nodes.Add(keydata.Name, keydata.Name + " ( " + keydata.Key.ToString() + " )");
            }

            // Combat --> Hands -> Equip Wands
            Engine.MainWindow.HotKeyTreeView.Nodes[0].Nodes[3].Nodes.Add("Equip Wands");
            keylist = RazorEnhanced.Settings.HotKey.ReadGroup("Equip Wands");
            foreach (HotKeyData keydata in keylist)
            {
                Engine.MainWindow.HotKeyTreeView.Nodes[0].Nodes[3].Nodes[5].Nodes.Add(keydata.Name, keydata.Name + " ( " + keydata.Key.ToString() + " )");
            }

            // Skills
            Engine.MainWindow.HotKeyTreeView.Nodes[0].Nodes.Add("Skills");
            keylist = RazorEnhanced.Settings.HotKey.ReadGroup("Skills");
            foreach (HotKeyData keydata in keylist)
            {
                Engine.MainWindow.HotKeyTreeView.Nodes[0].Nodes[4].Nodes.Add(keydata.Name, keydata.Name + " ( " + keydata.Key.ToString() + " )");
            }

            // Spells
            Engine.MainWindow.HotKeyTreeView.Nodes[0].Nodes.Add("Spells");
            keylist = RazorEnhanced.Settings.HotKey.ReadGroup("SpellsAgent");
            foreach (HotKeyData keydata in keylist)
            {
                Engine.MainWindow.HotKeyTreeView.Nodes[0].Nodes[5].Nodes.Add(keydata.Name, keydata.Name + " ( " + keydata.Key.ToString() + " )");
            }

            // Spells -- > Magery
            Engine.MainWindow.HotKeyTreeView.Nodes[0].Nodes[5].Nodes.Add("Magery");
            keylist = RazorEnhanced.Settings.HotKey.ReadGroup("SpellsMagery");
            foreach (HotKeyData keydata in keylist)
            {
                Engine.MainWindow.HotKeyTreeView.Nodes[0].Nodes[5].Nodes[3].Nodes.Add(keydata.Name, keydata.Name + " ( " + keydata.Key.ToString() + " )");
            }

            // Spells -- > Necro
            Engine.MainWindow.HotKeyTreeView.Nodes[0].Nodes[5].Nodes.Add("Necro");
            keylist = RazorEnhanced.Settings.HotKey.ReadGroup("SpellsNecro");
            foreach (HotKeyData keydata in keylist)
            {
                Engine.MainWindow.HotKeyTreeView.Nodes[0].Nodes[5].Nodes[4].Nodes.Add(keydata.Name, keydata.Name + " ( " + keydata.Key.ToString() + " )");
            }

            // Spells -- > Bushido
            Engine.MainWindow.HotKeyTreeView.Nodes[0].Nodes[5].Nodes.Add("Bushido");
            keylist = RazorEnhanced.Settings.HotKey.ReadGroup("SpellsBushido");
            foreach (HotKeyData keydata in keylist)
            {
                Engine.MainWindow.HotKeyTreeView.Nodes[0].Nodes[5].Nodes[5].Nodes.Add(keydata.Name, keydata.Name + " ( " + keydata.Key.ToString() + " )");
            }

            // Spells -- > Ninjitsu
            Engine.MainWindow.HotKeyTreeView.Nodes[0].Nodes[5].Nodes.Add("Ninjitsu");
            keylist = RazorEnhanced.Settings.HotKey.ReadGroup("SpellsNinjitsu");
            foreach (HotKeyData keydata in keylist)
            {
                Engine.MainWindow.HotKeyTreeView.Nodes[0].Nodes[5].Nodes[6].Nodes.Add(keydata.Name, keydata.Name + " ( " + keydata.Key.ToString() + " )");
            }

            // Spells -- > Spellweaving
            Engine.MainWindow.HotKeyTreeView.Nodes[0].Nodes[5].Nodes.Add("Spellweaving");
            keylist = RazorEnhanced.Settings.HotKey.ReadGroup("SpellsSpellweaving");
            foreach (HotKeyData keydata in keylist)
            {
                Engine.MainWindow.HotKeyTreeView.Nodes[0].Nodes[5].Nodes[7].Nodes.Add(keydata.Name, keydata.Name + " ( " + keydata.Key.ToString() + " )");
            }

            // Spells -- > Spellweaving
            Engine.MainWindow.HotKeyTreeView.Nodes[0].Nodes[5].Nodes.Add("Mysticism");
            keylist = RazorEnhanced.Settings.HotKey.ReadGroup("SpellsMysticism");
            foreach (HotKeyData keydata in keylist)
            {
                Engine.MainWindow.HotKeyTreeView.Nodes[0].Nodes[5].Nodes[8].Nodes.Add(keydata.Name, keydata.Name + " ( " + keydata.Key.ToString() + " )");
            }

            // Spells -- > Chivalry
            Engine.MainWindow.HotKeyTreeView.Nodes[0].Nodes[5].Nodes.Add("Chivalry");
            keylist = RazorEnhanced.Settings.HotKey.ReadGroup("SpellsChivalry");
            foreach (HotKeyData keydata in keylist)
            {
                Engine.MainWindow.HotKeyTreeView.Nodes[0].Nodes[5].Nodes[9].Nodes.Add(keydata.Name, keydata.Name + " ( " + keydata.Key.ToString() + " )");
            }

            // Target
            Engine.MainWindow.HotKeyTreeView.Nodes[0].Nodes.Add("Target");
            keylist = RazorEnhanced.Settings.HotKey.ReadGroup("Target");
            foreach (HotKeyData keydata in keylist)
            {
                Engine.MainWindow.HotKeyTreeView.Nodes[0].Nodes[6].Nodes.Add(keydata.Name, keydata.Name + " ( " + keydata.Key.ToString() + " )");
            }

            // Target -> List
            Engine.MainWindow.HotKeyTreeView.Nodes[0].Nodes[6].Nodes.Add("TList", "List");
            keylist = RazorEnhanced.Settings.HotKey.ReadTarget();
            foreach (HotKeyData keydata in keylist)
            {
                Engine.MainWindow.HotKeyTreeView.Nodes[0].Nodes[6].Nodes[3].Nodes.Add(keydata.Name, keydata.Name + " ( " + keydata.Key.ToString() + " )");
            }

            // Script
            Engine.MainWindow.HotKeyTreeView.Nodes[0].Nodes.Add("Script");
            keylist = RazorEnhanced.Settings.HotKey.ReadGroup("Script");
            foreach (HotKeyData keydata in keylist)
            {
                Engine.MainWindow.HotKeyTreeView.Nodes[0].Nodes[7].Nodes.Add(keydata.Name, keydata.Name + " ( " + keydata.Key.ToString() + " )");
            }
            Engine.MainWindow.HotKeyTreeView.Nodes[0].Nodes[7].Nodes.Add("List");
            Engine.MainWindow.HotKeyTreeView.Nodes[0].Nodes[7].Nodes[1].Nodes.Add("aaaa");

            // Virtue
            Engine.MainWindow.HotKeyTreeView.Nodes[0].Nodes.Add("Virtue");
            keylist = RazorEnhanced.Settings.HotKey.ReadGroup("UseVirtue");
            foreach (HotKeyData keydata in keylist)
            {
                Engine.MainWindow.HotKeyTreeView.Nodes[0].Nodes[8].Nodes.Add(keydata.Name, keydata.Name + " ( " + keydata.Key.ToString() + " )");
            }
            

            Engine.MainWindow.HotKeyTreeView.ExpandAll();
        }

        internal static void UpdateKey(string name)
        {
            if (!RazorEnhanced.Settings.HotKey.AssignedKey(m_key))
            {
                RazorEnhanced.Settings.HotKey.UpdateKey(name, m_key);
                Init();
            }
            else
            {
                DialogResult dialogResult = MessageBox.Show("Key: "+ m_key.ToString() + " already assigned! Want replace?", "HotKey", MessageBoxButtons.YesNo);
                if(dialogResult == DialogResult.Yes)
                {
                    RazorEnhanced.Settings.HotKey.UnassignKey(m_key);
                    RazorEnhanced.Settings.HotKey.UpdateKey(name, m_key);
                    Init();
                }
            }

        }

        internal static void UpdateTargetKey(string name)
        {
            if (!RazorEnhanced.Settings.HotKey.AssignedKey(m_key))
            {
                RazorEnhanced.Settings.HotKey.UpdateTargetKey(name, m_key);
                Init();
            }
            else
            {
                DialogResult dialogResult = MessageBox.Show("Key: "+ m_key.ToString() + " already assigned! Want replace?", "HotKey", MessageBoxButtons.YesNo);
                if(dialogResult == DialogResult.Yes)
                {
                    RazorEnhanced.Settings.HotKey.UnassignKey(m_key);
                    RazorEnhanced.Settings.HotKey.UpdateTargetKey(name, m_key);
                    Init();
                }
            }

        }

        internal static void UpdateMaster()
        {
            if (!RazorEnhanced.Settings.HotKey.AssignedKey(m_Masterkey))
            {
                RazorEnhanced.Settings.General.WriteKey("HotKeyMasterKey", RazorEnhanced.HotKey.m_Masterkey);
                Assistant.Engine.MainWindow.HotKeyKeyMasterLabel.Text = "ON/OFF Key: " + RazorEnhanced.HotKey.m_Masterkey.ToString();
            }
            else
            {
                DialogResult dialogResult = MessageBox.Show("Key: " + m_key.ToString() + " already assigned! Want replace?", "HotKey", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    RazorEnhanced.Settings.HotKey.UnassignKey(m_Masterkey);
                    RazorEnhanced.Settings.General.WriteKey("HotKeyMasterKey", RazorEnhanced.HotKey.m_Masterkey);
                    Assistant.Engine.MainWindow.HotKeyKeyMasterLabel.Text = "ON/OFF Key: " + RazorEnhanced.HotKey.m_Masterkey.ToString();
                    Init();
                }
            }
        }

        internal static void ClearKey(string name)
        {
            RazorEnhanced.Settings.HotKey.UpdateKey(name, Keys.None);
            Init();
        }

        internal static void ClearMasterKey()
        {
            RazorEnhanced.Settings.General.WriteKey("HotKeyMasterKey", Keys.None);
            Assistant.Engine.MainWindow.HotKeyKeyMasterLabel.Text = "ON/OFF Key: " + RazorEnhanced.Settings.General.ReadKey("HotKeyMasterKey").ToString();
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
