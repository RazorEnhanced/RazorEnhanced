using System;
using System.Media;
using System.Collections.Generic;
using Assistant;
using System.Windows.Forms;
using RazorEnhanced;
using System.Threading;


namespace RazorEnhanced
{
    public class DragDropManager
    {
        internal static Queue<int> AutoLootSerialToGrab = new Queue<int>();
        internal static Queue<int> AutoLootOpenAction = new Queue<int>();
        internal static Queue<int> ScavengerSerialToGrab = new Queue<int>();
        internal static void Engine()
        {
            if (AutoLootOpenAction.Count > 100)
                AutoLootOpenAction.Clear();

            if (AutoLootSerialToGrab.Count > 100)
                AutoLootSerialToGrab.Clear();

            if (ScavengerSerialToGrab.Count > 100)
                ScavengerSerialToGrab.Clear();

            if (AutoLootOpenAction.Count > 0 && Assistant.Engine.MainWindow.AutolootCheckBox.Checked)
            {
                Assistant.Item item = Assistant.World.FindItem(AutoLootOpenAction.Peek());
                if (item == null)
                {
                    AutoLootOpenAction.Dequeue();
                    return;
                }
                if (Utility.InRange(new Assistant.Point2D(Assistant.World.Player.Position.X, Assistant.World.Player.Position.Y), new Assistant.Point2D(item.Position.X, item.Position.Y), 2))
                {
                    RazorEnhanced.AutoLoot.AddLog("- Force Open: 0x" + item.Serial.ToString());
                    Assistant.ClientCommunication.SendToServer(new DoubleClick(item.Serial));            
                    Thread.Sleep(AutoLoot.AutoLootDelay);
                    AutoLootOpenAction.Dequeue();
                }

            }

            if (AutoLootSerialToGrab.Count > 0 && Assistant.Engine.MainWindow.AutolootCheckBox.Checked)
            {
                Assistant.Item item = Assistant.World.FindItem(AutoLootSerialToGrab.Peek());
                if (item == null)
                {
                    AutoLootSerialToGrab.Dequeue();
                    return;
                }
                if (item.RootContainer == World.Player)
                {
                    AutoLootSerialToGrab.Dequeue();
                    return;
                }
                Assistant.Item corpse = (Assistant.Item)item.Container;
                if (Utility.InRange(new Assistant.Point2D(Assistant.World.Player.Position.X, Assistant.World.Player.Position.Y), new Assistant.Point2D(corpse.Position.X, corpse.Position.Y), 2) && CheckZLevel(corpse.Position.Z, World.Player.Position.Z))
                {
                    if ((World.Player.MaxWeight - World.Player.Weight) < 5)
                    {
                        RazorEnhanced.AutoLoot.AddLog("- Max weight reached, Wait untill free some space");
                        RazorEnhanced.Misc.SendMessage("AUTOLOOT: Max weight reached, Wait untill free some space");
                        Thread.Sleep(2000);
                    }
                    else
                    {
                        RazorEnhanced.AutoLoot.AddLog("- Item Match found (" + item.Serial.ToString() + ") ... Looting");
                        Assistant.ClientCommunication.SendToServer(new LiftRequest(item.Serial, item.Amount));
                        Assistant.ClientCommunication.SendToServer(new DropRequest(item.Serial, Assistant.Point3D.MinusOne, AutoLoot.AutoLootBag));
                        Thread.Sleep(AutoLoot.AutoLootDelay);
                    }
                }
                
            }

            if (ScavengerSerialToGrab.Count > 0 && Assistant.Engine.MainWindow.ScavengerCheckBox.Checked)
            {
                Assistant.Item item = Assistant.World.FindItem(ScavengerSerialToGrab.Peek());
                if (item == null)
                {
                    ScavengerSerialToGrab.Dequeue();
                    return;
                }
                if (item.RootContainer == World.Player)
                {
                    ScavengerSerialToGrab.Dequeue();
                    return;
                }
                if (Utility.InRange(new Assistant.Point2D(Assistant.World.Player.Position.X, Assistant.World.Player.Position.Y), new Assistant.Point2D(item.Position.X, item.Position.Y), 2) && CheckZLevel(item.Position.Z, World.Player.Position.Z))
                {
                    if ((World.Player.MaxWeight - World.Player.Weight) < 5)
                    {
                        RazorEnhanced.Scavenger.AddLog("- Max weight reached, Wait untill free some space");
                        RazorEnhanced.Misc.SendMessage("SCAVENGER: Max weight reached, Wait untill free some space");
                        Thread.Sleep(2000);
                    }
                    else
                    {
                        RazorEnhanced.Scavenger.AddLog("- Item Match found (" + item.Serial.ToString() + ") ... Grabbing");
                        Assistant.ClientCommunication.SendToServer(new LiftRequest(item.Serial, item.Amount));
                        Assistant.ClientCommunication.SendToServer(new DropRequest(item.Serial, Assistant.Point3D.MinusOne, Scavenger.ScavengerBag));
                        Thread.Sleep(Scavenger.ScavengerDelay);
                    }
                }

           }

        }
        private static bool CheckZLevel(int x, int y)
        {
            int diff = x - y;

            if (diff < -4 || diff > 4)
                return false;
            else
                return true;
        }
    }


}
