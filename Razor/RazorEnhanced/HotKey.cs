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
            KeyDown(k);
            return true;
        }

        internal static void KeyDown(Keys k)
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
                        ProcessSpellsMagery(RazorEnhanced.Settings.HotKey.FindString(k));
                        break;
                    case "SpellsNecro":
                        ProcessSpellsNecro(RazorEnhanced.Settings.HotKey.FindString(k));
                        break;
                    case "SpellsBushido":
                        ProcessSpellsBushido(RazorEnhanced.Settings.HotKey.FindString(k));
                        break;
                    case "SpellsNinjitsu":
                        ProcessSpellsNinjitsu(RazorEnhanced.Settings.HotKey.FindString(k));
                        break;
                    case "SpellsSpellweaving":
                        ProcessSpellsSpellweaving(RazorEnhanced.Settings.HotKey.FindString(k));
                        break;
                    case "SpellsMysticism":
                        ProcessSpellsMysticism(RazorEnhanced.Settings.HotKey.FindString(k));
                        break;
                    case "Target":
                        ProcessTarget(RazorEnhanced.Settings.HotKey.FindString(k));
                        break;
                    case "TargetList":
                        ProcessTargetList(RazorEnhanced.Settings.HotKey.FindString(k));
                        break;
                    case "Script":
                        ProcessScript(RazorEnhanced.Settings.HotKey.FindString(k));
                        break;
                    case "ScriptList":
                        ProcessScriptList(RazorEnhanced.Settings.HotKey.FindString(k));
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
                default:
                    break;
            }
        }

        private static void ProcessActions(string function)
        {
            switch (function)
            {
                default:
                    break;
            }
        }
        private static void ProcessUse(string function)
        {
            switch (function)
            {
                default:
                    break;
            }
        }

        private static void ProcessShowName(string function)
        {
            switch (function)
            {
                default:
                    break;
            }
        }
        private static void ProcessPetCommands(string function)
        {
            switch (function)
            {
                default:
                    break;
            }
        }

        private static void ProcessAgents(string function)
        {
            switch (function)
            {
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
            switch (function)
            {
                default:
                    break;
            }
        }
        private static void ProcessPotions(string function)
        {
            switch (function)
            {
                default:
                    break;
            }
        }
        private static void ProcessOther(string function)
        {
            switch (function)
            {
                default:
                    break;
            }
        }
        private static void ProcessHands(string function)
        {
            switch (function)
            {
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
            switch (function)
            {
                default:
                    break;
            }
        }
        private static void ProcessSpellsAgent(string function)
        {
            switch (function)
            {
                default:
                    break;
            }
        }
        private static void ProcessSpellsMagery(string function)
        {
            switch (function)
            {
                default:
                    break;
            }
        }
        private static void ProcessSpellsNecro(string function)
        {
            switch (function)
            {
                default:
                    break;
            }
        }
        private static void ProcessSpellsBushido(string function)
        {
            switch (function)
            {
                default:
                    break;
            }
        }
        private static void ProcessSpellsNinjitsu(string function)
        {
            switch (function)
            {
                default:
                    break;
            }
        }
        private static void ProcessSpellsSpellweaving(string function)
        {
            switch (function)
            {
                default:
                    break;
            }
        }
        private static void ProcessSpellsMysticism(string function)
        {
            switch (function)
            {
                default:
                    break;
            }
        }
        private static void ProcessTarget(string function)
        {
            switch (function)
            {
                default:
                    break;
            }
        }
        private static void ProcessTargetList(string function)
        {
            switch (function)
            {
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

            // Combat --> Hands
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

            // Target
            Engine.MainWindow.HotKeyTreeView.Nodes[0].Nodes.Add("Target");
            Engine.MainWindow.HotKeyTreeView.Nodes[0].Nodes[6].Nodes.Add("List");

            // Script
            Engine.MainWindow.HotKeyTreeView.Nodes[0].Nodes.Add("Script");
            Engine.MainWindow.HotKeyTreeView.Nodes[0].Nodes[7].Nodes.Add("List");


            Engine.MainWindow.HotKeyTreeView.ExpandAll();
        }

        internal static void UpdateKey(string name)
        {
            RazorEnhanced.Settings.HotKey.UpdateKey(name, m_key);
            Init();
        }

        internal static void ClearKey(string name)
        {
            RazorEnhanced.Settings.HotKey.UpdateKey(name, Keys.None);
            Init();
        }
	}
}
