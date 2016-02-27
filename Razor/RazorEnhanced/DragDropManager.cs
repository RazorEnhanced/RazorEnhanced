using Assistant;
using System.Collections.Concurrent;
using System.Threading;

namespace RazorEnhanced
{
	public class DragDropManager
	{
		internal static ConcurrentQueue<int> AutoLootSerialCorpseRefresh = new ConcurrentQueue<int>();
		internal static ConcurrentQueue<int> AutoLootSerialToGrab = new ConcurrentQueue<int>();
		internal static ConcurrentQueue<int> ScavengerSerialToGrab = new ConcurrentQueue<int>();
		internal static ConcurrentQueue<int> CorpseToCutSerial = new ConcurrentQueue<int>();

		internal static int LastAutolootItem = 0;

		internal static void AutoRun()
		{
			if (AutoLootSerialCorpseRefresh.Count > 0 && Assistant.Engine.MainWindow.AutolootCheckBox.Checked)
			{
				try
				{
					int itemserial = 0;
					AutoLootSerialCorpseRefresh.TryPeek(out itemserial);
					Assistant.Item item = Assistant.World.FindItem(itemserial);

					if (item == null)
					{
						AutoLootSerialCorpseRefresh.TryDequeue(out itemserial);
						return;
					}

					if (Utility.InRange(new Assistant.Point2D(Assistant.World.Player.Position.X, Assistant.World.Player.Position.Y), new Assistant.Point2D(item.Position.X, item.Position.Y), 2) && CheckZLevel(item.Position.Z, World.Player.Position.Z))
					{
						RazorEnhanced.Items.WaitForContents(Items.FindBySerial(itemserial), 1000);
						AutoLoot.AddLog("- Refresh Corpse: 0x" + itemserial.ToString("X8"));
						Thread.Sleep(AutoLoot.AutoLootDelay);
						if (item.Updated)
							AutoLootSerialCorpseRefresh.TryDequeue(out itemserial);
					}
					else
					{
						AutoLootSerialCorpseRefresh.TryDequeue(out itemserial);
						AutoLootSerialCorpseRefresh.Enqueue(itemserial);
					}
				}
				catch { }
			}

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

					Assistant.Item corpse = null;
					corpse = item.Container as Assistant.Item;

					if (corpse == null)
						return;

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
							Assistant.ClientCommunication.SendToServerWait(new LiftRequest(item.Serial, item.Amount));
							Assistant.ClientCommunication.SendToServerWait(new DropRequest(item.Serial, Assistant.Point3D.MinusOne, AutoLoot.AutoLootBag));
							LastAutolootItem = item.Serial;
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
							Assistant.ClientCommunication.SendToServerWait(new LiftRequest(item.Serial, item.Amount));
							Assistant.ClientCommunication.SendToServerWait(new DropRequest(item.Serial, Assistant.Point3D.MinusOne, Scavenger.ScavengerBag));
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

			if (CorpseToCutSerial.Count > 0 && Filters.AutoCarver)
			{
				try
				{
					int itemserial = 0;
					CorpseToCutSerial.TryPeek(out itemserial);
					Assistant.Item item = Assistant.World.FindItem(itemserial);

					if (item == null)
					{
						CorpseToCutSerial.TryDequeue(out itemserial);
						return;
					}

					if (Utility.InRange(new Assistant.Point2D(Assistant.World.Player.Position.X, Assistant.World.Player.Position.Y), new Assistant.Point2D(item.Position.X, item.Position.Y), 1) && CheckZLevel(item.Position.Z, World.Player.Position.Z))
					{
						Items.UseItem(Items.FindBySerial(Filters.AutoCarverBlade));
						Target.WaitForTarget(1000);
						Target.TargetExecute(item.Serial);
						Items.Message(item.Serial, 10, "*Cutting*");

						CorpseToCutSerial.TryDequeue(out itemserial);
						Thread.Sleep(800);
					}
					else
					{
						CorpseToCutSerial.TryDequeue(out itemserial);
						CorpseToCutSerial.Enqueue(itemserial);
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