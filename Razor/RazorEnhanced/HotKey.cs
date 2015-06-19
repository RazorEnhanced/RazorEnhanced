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



        [DllImport("user32.dll")]
        private static extern ushort GetAsyncKeyState(int key);

        internal static bool GameKeyDown(Keys k)
        {
            KeyDown(k);
            return true;
        }

        internal static void KeyDown(Keys k)
        {
            if (Engine.MainWindow.HotKeyTextBox.Focused)                // In caso di assegnazione
            {
                m_key = k;
                Engine.MainWindow.HotKeyTextBox.Text = k.ToString();
            }
            else    // Esecuzine reale
            {
                ExecFunction(RazorEnhanced.Settings.HotKey.FindString(k));
            }
        }

        private static void ExecFunction(string function)
        {
            if (function != "" && World.Player != null)
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
        }

        internal static void Init()
        {
            int subcount = 0;
            Engine.MainWindow.HotKeyTreeView.Nodes.Clear();
            Engine.MainWindow.HotKeyTreeView.Nodes.Add("HotKeys");
            Engine.MainWindow.HotKeyTreeView.Nodes[0].Nodes.Add("General");

            // General
            List<HotKeyData> keylist = RazorEnhanced.Settings.HotKey.ReadGroup("General");
            foreach(HotKeyData keydata in keylist)
            {
                if (keydata.Key != Keys.None)
                {
                    Engine.MainWindow.HotKeyTreeView.Nodes[0].Nodes[0].Nodes.Add(keydata.Name, keydata.Name + " ( " + keydata.Key.ToString() + " )");
                }
                else
                {
                    Engine.MainWindow.HotKeyTreeView.Nodes[0].Nodes[0].Nodes.Add(keydata.Name, keydata.Name + " ( None )");
                }
            }

            // Actions
            Engine.MainWindow.HotKeyTreeView.Nodes[0].Nodes.Add("Actions");
            keylist = RazorEnhanced.Settings.HotKey.ReadGroup("Actions");
            foreach (HotKeyData keydata in keylist)
            {
                if (keydata.Key != Keys.None)
                {
                    Engine.MainWindow.HotKeyTreeView.Nodes[0].Nodes[1].Nodes.Add(keydata.Name, keydata.Name + " ( " + keydata.Key.ToString() + " )");
                }
                else
                {
                    Engine.MainWindow.HotKeyTreeView.Nodes[0].Nodes[1].Nodes.Add(keydata.Name, keydata.Name + " ( None )");
                }
            }

            // Actions -> Use
            Engine.MainWindow.HotKeyTreeView.Nodes[0].Nodes[1].Nodes.Add("Use");
            keylist = RazorEnhanced.Settings.HotKey.ReadGroup("Use");
            foreach (HotKeyData keydata in keylist)
            {
                if (keydata.Key != Keys.None)
                {
                    Engine.MainWindow.HotKeyTreeView.Nodes[0].Nodes[1].Nodes[3].Nodes.Add(keydata.Name, keydata.Name + " ( " + keydata.Key.ToString() + " )");
                }
                else
                {
                    Engine.MainWindow.HotKeyTreeView.Nodes[0].Nodes[1].Nodes[3].Nodes.Add(keydata.Name, keydata.Name + " ( None )");
                }
            }

            // Actions -> Show Names
            Engine.MainWindow.HotKeyTreeView.Nodes[0].Nodes[1].Nodes.Add("Show Names");
            keylist = RazorEnhanced.Settings.HotKey.ReadGroup("Show Names");
            foreach (HotKeyData keydata in keylist)
            {
                if (keydata.Key != Keys.None)
                {
                    Engine.MainWindow.HotKeyTreeView.Nodes[0].Nodes[1].Nodes[4].Nodes.Add(keydata.Name, keydata.Name + " ( " + keydata.Key.ToString() + " )");
                }
                else
                {
                    Engine.MainWindow.HotKeyTreeView.Nodes[0].Nodes[1].Nodes[4].Nodes.Add(keydata.Name, keydata.Name + " ( None )");
                }
            }

            // Actions -> Per Commands
            Engine.MainWindow.HotKeyTreeView.Nodes[0].Nodes[1].Nodes.Add("Pet Commands");
            keylist = RazorEnhanced.Settings.HotKey.ReadGroup("Pet Commands");
            foreach (HotKeyData keydata in keylist)
            {
                if (keydata.Key != Keys.None)
                {
                    Engine.MainWindow.HotKeyTreeView.Nodes[0].Nodes[1].Nodes[5].Nodes.Add(keydata.Name, keydata.Name + " ( " + keydata.Key.ToString() + " )");
                }
                else
                {
                    Engine.MainWindow.HotKeyTreeView.Nodes[0].Nodes[1].Nodes[5].Nodes.Add(keydata.Name, keydata.Name + " ( None )");
                }
            }

            // Agents
            Engine.MainWindow.HotKeyTreeView.Nodes[0].Nodes.Add("Agents");
            keylist = RazorEnhanced.Settings.HotKey.ReadGroup("Agents");
            foreach (HotKeyData keydata in keylist)
            {
                if (keydata.Key != Keys.None)
                {
                    Engine.MainWindow.HotKeyTreeView.Nodes[0].Nodes[2].Nodes.Add(keydata.Name, keydata.Name + " ( " + keydata.Key.ToString() + " )");
                }
                else
                {
                    Engine.MainWindow.HotKeyTreeView.Nodes[0].Nodes[2].Nodes.Add(keydata.Name, keydata.Name + " ( None )");
                }
            }

            // Combats
            Engine.MainWindow.HotKeyTreeView.Nodes[0].Nodes.Add("Combat");

            // Combat  --> Abilities
            Engine.MainWindow.HotKeyTreeView.Nodes[0].Nodes[3].Nodes.Add("Abilities");
            keylist = RazorEnhanced.Settings.HotKey.ReadGroup("Abilities");
            foreach (HotKeyData keydata in keylist)
            {
                if (keydata.Key != Keys.None)
                {
                    Engine.MainWindow.HotKeyTreeView.Nodes[0].Nodes[3].Nodes[0].Nodes.Add(keydata.Name, keydata.Name + " ( " + keydata.Key.ToString() + " )");
                }
                else
                {
                    Engine.MainWindow.HotKeyTreeView.Nodes[0].Nodes[3].Nodes[0].Nodes.Add(keydata.Name, keydata.Name + " ( None )");
                }
            }

            // Combat  --> Attack
            Engine.MainWindow.HotKeyTreeView.Nodes[0].Nodes[3].Nodes.Add("Attack");
            keylist = RazorEnhanced.Settings.HotKey.ReadGroup("Attack");
            foreach (HotKeyData keydata in keylist)
            {
                if (keydata.Key != Keys.None)
                {
                    Engine.MainWindow.HotKeyTreeView.Nodes[0].Nodes[3].Nodes[1].Nodes.Add(keydata.Name, keydata.Name + " ( " + keydata.Key.ToString() + " )");
                }
                else
                {
                    Engine.MainWindow.HotKeyTreeView.Nodes[0].Nodes[3].Nodes[1].Nodes.Add(keydata.Name, keydata.Name + " ( None )");
                }
            }

            // Combat  --> Bandage
            Engine.MainWindow.HotKeyTreeView.Nodes[0].Nodes[3].Nodes.Add("Bandage");
            keylist = RazorEnhanced.Settings.HotKey.ReadGroup("Bandage");
            foreach (HotKeyData keydata in keylist)
            {
                if (keydata.Key != Keys.None)
                {
                    Engine.MainWindow.HotKeyTreeView.Nodes[0].Nodes[3].Nodes[2].Nodes.Add(keydata.Name, keydata.Name + " ( " + keydata.Key.ToString() + " )");
                }
                else
                {
                    Engine.MainWindow.HotKeyTreeView.Nodes[0].Nodes[3].Nodes[2].Nodes.Add(keydata.Name, keydata.Name + " ( None )");
                }
            }

            // Combat  --> Consumable
            Engine.MainWindow.HotKeyTreeView.Nodes[0].Nodes[3].Nodes.Add("Consumable");

            // Combat  --> Consumable --> Potions
            Engine.MainWindow.HotKeyTreeView.Nodes[0].Nodes[3].Nodes[3].Nodes.Add("Potions");
            keylist = RazorEnhanced.Settings.HotKey.ReadGroup("Potions");
            foreach (HotKeyData keydata in keylist)
            {
                if (keydata.Key != Keys.None)
                {
                    Engine.MainWindow.HotKeyTreeView.Nodes[0].Nodes[3].Nodes[3].Nodes[0].Nodes.Add(keydata.Name, keydata.Name + " ( " + keydata.Key.ToString() + " )");
                }
                else
                {
                    Engine.MainWindow.HotKeyTreeView.Nodes[0].Nodes[3].Nodes[3].Nodes[0].Nodes.Add(keydata.Name, keydata.Name + " ( None )");
                }
            }

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
