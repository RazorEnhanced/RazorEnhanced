using Assistant;
using System.Collections.Concurrent;
using System.Threading;

namespace RazorEnhanced
{
	public class DragDropManager
	{
		public class AutoLootSerialToGrab
		{
			private int m_corpseserial;
			public int CorpseSerial { get { return m_corpseserial; } }

			private int m_itemserial;
			public int ItemSerial { get { return m_itemserial; } }

			public AutoLootSerialToGrab(int itemserial, int corpseserial)
			{
				m_corpseserial = corpseserial;
				m_itemserial = itemserial;
			}
		}

		internal static ConcurrentQueue<int> AutoLootSerialCorpseRefresh = new ConcurrentQueue<int>();
		internal static ConcurrentQueue<AutoLootSerialToGrab> AutoLootSerialToGrabList = new ConcurrentQueue<AutoLootSerialToGrab>();
		internal static ConcurrentQueue<int> ScavengerSerialToGrab = new ConcurrentQueue<int>();
		internal static ConcurrentQueue<int> CorpseToCutSerial = new ConcurrentQueue<int>();

		internal static int LastAutolootItem = 0;

		internal static void AutoRun()
		{
			if (World.Player.IsGhost)
			{
				Thread.Sleep(2000);
				return;
			}

			if (AutoLootSerialCorpseRefresh.Count > 0 && Assistant.Engine.MainWindow.AutolootCheckBox.Checked && !Targeting.HasTarget && Player.Visible)
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

			if (AutoLootSerialToGrabList.Count > 0 && Assistant.Engine.MainWindow.AutolootCheckBox.Checked && Player.Visible)
			{
				try
				{
					AutoLootSerialToGrab data;
					AutoLootSerialToGrabList.TryDequeue(out data);
					Assistant.Item item = Assistant.World.FindItem(data.ItemSerial);

					if (item == null)
						return;

					if (item.RootContainer == World.Player)
						return;

					Assistant.Item corpse = Assistant.World.FindItem(data.CorpseSerial);

					if (corpse == null)
						return;

					if ((Utility.InRange(new Assistant.Point2D(Assistant.World.Player.Position.X, Assistant.World.Player.Position.Y), new Assistant.Point2D(corpse.Position.X, corpse.Position.Y), 2) && CheckZLevel(corpse.Position.Z, World.Player.Position.Z)))
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
				}
				catch { }
			}

			if (ScavengerSerialToGrab.Count > 0 && Assistant.Engine.MainWindow.ScavengerCheckBox.Checked)
			{
				try
				{
					int itemserial = 0;
					ScavengerSerialToGrab.TryDequeue(out itemserial);
					Assistant.Item item = Assistant.World.FindItem(itemserial);

					if (item == null)
						return;

					if (item.RootContainer == World.Player)
						return;

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
						Target.WaitForTarget(1000, true);
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

			if (diff < -8 || diff > 8)
				return false;
			else
				return true;
		}
	}
}