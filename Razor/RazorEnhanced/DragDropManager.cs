using System;
using System.Media;
using System.Collections.Generic;
using Assistant;
using RazorEnhanced;
using System.Threading;


namespace RazorEnhanced
{
    public class DragDropManager
    {
        internal static Queue<int> AutoLootSerialToGrab = new Queue<int>();
        internal static Queue<int> ScavengerSerialToGrab = new Queue<int>();
        internal static void Engine()
        {
            if (AutoLootSerialToGrab.Count > 0)
            {
                Item itemtograb = new RazorEnhanced.Item(Assistant.World.FindItem((Assistant.Serial)((uint)AutoLootSerialToGrab.Peek())));
                if (itemtograb == null)
                {
                    AutoLootSerialToGrab.Dequeue();
                    return;
                }
                if (itemtograb.RootContainer == World.Player)
                {
                    AutoLootSerialToGrab.Dequeue();
                    return;
                }
                Assistant.Item corpse = (Assistant.Item)itemtograb.Container;
                if (Utility.InRange(new Assistant.Point2D(Assistant.World.Player.Position.X, Assistant.World.Player.Position.Y), new Assistant.Point2D(corpse.Position.X, corpse.Position.Y), 2))
                {
                    RazorEnhanced.AutoLoot.AddLog("- Item Match found (0x" + itemtograb.Serial.ToString("X8") + ") ... Looting");
                    Assistant.ClientCommunication.SendToServer(new LiftRequest(itemtograb.Serial, itemtograb.Amount));
                    Assistant.ClientCommunication.SendToServer(new DropRequest(itemtograb.Serial, Assistant.Point3D.MinusOne, AutoLoot.AutoLootBag));
                    Thread.Sleep(AutoLoot.AutoLootDelay);
                }
            }

            if (ScavengerSerialToGrab.Count > 0)
            {
                Item itemtograb = new RazorEnhanced.Item(Assistant.World.FindItem((Assistant.Serial)((uint)ScavengerSerialToGrab.Peek())));
                if (itemtograb == null)
                {
                    ScavengerSerialToGrab.Dequeue();
                    return;
                }
                if (itemtograb.RootContainer == World.Player)
                {
                    ScavengerSerialToGrab.Dequeue();
                    return;
                }
                if (Utility.InRange(new Assistant.Point2D(Assistant.World.Player.Position.X, Assistant.World.Player.Position.Y), new Assistant.Point2D(itemtograb.Position.X, itemtograb.Position.Y), 2))
                {
                    RazorEnhanced.Scavenger.AddLog("- Item Match found (0x" + itemtograb.Serial.ToString("X8") + ") ... Grabbing");
                    Assistant.ClientCommunication.SendToServer(new LiftRequest(itemtograb.Serial, itemtograb.Amount));
                    Assistant.ClientCommunication.SendToServer(new DropRequest(itemtograb.Serial, Assistant.Point3D.MinusOne, Scavenger.ScavengerBag));
                    Thread.Sleep(Scavenger.ScavengerDelay);
                }
            }

        }
    }

}
