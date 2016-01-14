using Assistant;
using System.Collections.Concurrent;
using System.Threading;

namespace RazorEnhanced
{
	public class DragDropManager
	{
		internal static ConcurrentQueue<int> AutoLootSerialToGrab = new ConcurrentQueue<int>();
		internal static ConcurrentQueue<int> ScavengerSerialToGrab = new ConcurrentQueue<int>();

		internal static void Engine()
		{ 
			if (AutoLootSerialToGrab.Count > 0 && Assistant.Engine.MainWindow.AutolootCheckBox.Checked)
			{
				try
				{
					int itemserial = 0;
					AutoLootSerialToGrab.TryPeek(out itemserial);
					Assistant.Item item = Assistant.World.FindItem(itemserial);
					if (item == null)
					{
						AutoLootSerialToGrab.TryDequeue(out itemserial);
						return;
					}
					if (item.RootContainer == World.Player)
					{
						AutoLootSerialToGrab.TryDequeue(out itemserial);
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
					else
					{
						AutoLootSerialToGrab.TryDequeue(out itemserial);
						AutoLootSerialToGrab.Enqueue(itemserial);
					}
				}
				catch { }
			}

			if (ScavengerSerialToGrab.Count > 0 && Assistant.Engine.MainWindow.ScavengerCheckBox.Checked)
			{
				try
				{
					int itemserial = 0;
					ScavengerSerialToGrab.TryPeek(out itemserial);
					Assistant.Item item = Assistant.World.FindItem(itemserial);
					if (item == null)
					{
						ScavengerSerialToGrab.TryDequeue(out itemserial);
						return;
					}
					if (item.RootContainer == World.Player)
					{
						ScavengerSerialToGrab.TryDequeue(out itemserial);
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
					else
					{
						ScavengerSerialToGrab.TryDequeue(out itemserial);
						ScavengerSerialToGrab.Enqueue(itemserial);
					}
				}
				catch { }
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