using Assistant;
using System.Collections.Concurrent;
using System.Threading;
using System;

namespace RazorEnhanced
{
	public class DragDropManager
	{
		internal static ConcurrentQueue<int> AutoLootSerialCorpseRefresh = new ConcurrentQueue<int>();
		internal static ConcurrentQueue<int> ScavengerSerialToGrab = new ConcurrentQueue<int>();
		internal static ConcurrentQueue<int> CorpseToCutSerial = new ConcurrentQueue<int>();

        //	internal static volatile bool HoldingItem = false;

        internal static void AutoRun()
        {
            if (World.Player == null || !Client.Running)
                return;

            if (World.Player.IsGhost)
            {
                Thread.Sleep(2000);
                return;
            }

            bool lootHidden = RazorEnhanced.Settings.General.ReadBool("AllowHiddenLooting");
            if (AutoLootSerialCorpseRefresh.Count > 0
                && Assistant.Engine.MainWindow.AutolootCheckBox.Checked
                && !Targeting.HasTarget
                && (Player.Visible || lootHidden))
            {
                try
                {
                    if (AutoLootSerialCorpseRefresh.TryPeek(out int itemserial))
                    {
                        Assistant.Item item = Assistant.World.FindItem(itemserial);

                        if (item == null)
                        {
                            AutoLootSerialCorpseRefresh.TryDequeue(out itemserial);
                            return;
                        }

                        if (Utility.Distance(World.Player.Position.X, World.Player.Position.Y, item.Position.X, item.Position.Y) <= AutoLoot.MaxRange && CheckZLevel(item.Position.Z, World.Player.Position.Z))
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
                }
                catch { }
            }
            uint lootbag = AutoLoot.GetLootBag();
            // at login, backpack is sometimes null
            if (lootbag != 0)
            {
                ProcessLootList(lootbag);
            }

			if (ScavengerSerialToGrab.Count > 0 && Assistant.Engine.MainWindow.ScavengerCheckBox.Checked)
			{
				try
				{
					if (ScavengerSerialToGrab.TryPeek(out int itemserial))
					{
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

						if (Utility.Distance(World.Player.Position.X, World.Player.Position.Y, item.Position.X, item.Position.Y) <= Scavenger.MaxRange && CheckZLevel(item.Position.Z, World.Player.Position.Z))
						{
							if ((World.Player.MaxWeight - World.Player.Weight) < 5)
							{
								RazorEnhanced.Scavenger.AddLog("- Max weight reached, Wait untill free some space");
								RazorEnhanced.Misc.SendMessage("SCAVENGER: Max weight reached, Wait untill free some space", true);
								Thread.Sleep(2000);
							}
							else
							{
								RazorEnhanced.Scavenger.AddLog("- Item Match found (" + item.Serial.ToString() + ") ... Grabbing");
								RazorEnhanced.Items.Move(item.Serial, Convert.ToInt32(Scavenger.GetScavengerBag()), 0);
								Thread.Sleep(Scavenger.ScavengerDelay);
								ScavengerSerialToGrab.TryDequeue(out itemserial);
							}
						}
						else
						{
							ScavengerSerialToGrab.TryDequeue(out itemserial);
							ScavengerSerialToGrab.Enqueue(itemserial);
						}
					}
				}
				catch { }
			}

			if (CorpseToCutSerial.Count > 0 && Filters.AutoCarver)
			{
				try
				{
					if (CorpseToCutSerial.TryPeek(out int itemserial))
					{
						Assistant.Item item = Assistant.World.FindItem(itemserial);

						if (item == null)
						{
							CorpseToCutSerial.TryDequeue(out itemserial);
							return;
						}

						if (Utility.Distance(World.Player.Position.X, World.Player.Position.Y, item.Position.X, item.Position.Y) <= 1 && CheckZLevel(item.Position.Z, World.Player.Position.Z))
						{
							Items.UseItem(Items.FindBySerial(Filters.AutoCarverBlade));
							Target.WaitForTarget(1000, true);
							Target.TargetExecute(item.Serial);
							Items.Message(item.Serial, 10, "*Cutting*");
							CorpseToCutSerial.TryDequeue(out itemserial);
							Thread.Sleep(RazorEnhanced.Settings.General.ReadInt("ObjectDelay"));
						}
						else
						{
							CorpseToCutSerial.TryDequeue(out itemserial);
							CorpseToCutSerial.Enqueue(itemserial);
						}

					}
				}
				catch { }
			}
			Thread.Sleep(250);
		}

        private static System.Object autolootLock = new System.Object();
        internal static void ProcessLootList(uint lootbag)
        {
            if (Monitor.TryEnter(autolootLock)) try
                {
                    bool lootHidden = RazorEnhanced.Settings.General.ReadBool("AllowHiddenLooting");

                    if ( (Player.Visible || lootHidden) && AutoLoot.SerialToGrabList.Count > 0)
                    {
                        try
                        {
                            if (AutoLoot.SerialToGrabList.TryPeek(out AutoLoot.SerialToGrab data))
                            {
                                Assistant.Item item = Assistant.World.FindItem(data.ItemSerial);

                                if (item == null)
                                {
                                    AutoLoot.SerialToGrabList.TryDequeue(out data);
                                    return;
                                }

                                if (item.RootContainer == World.Player)
                                {
                                    AutoLoot.SerialToGrabList.TryDequeue(out data);
                                    return;
                                }

                                Assistant.Item corpse = Assistant.World.FindItem(data.CorpseSerial);

                                if (corpse == null)
                                {
                                    AutoLoot.SerialToGrabList.TryDequeue(out data);
                                    return;
                                }

                                if (Utility.Distance(World.Player.Position.X, World.Player.Position.Y, corpse.Position.X, corpse.Position.Y) <= AutoLoot.MaxRange && CheckZLevel(corpse.Position.Z, World.Player.Position.Z))
                                {
                                    if ((World.Player.MaxWeight - World.Player.Weight) < 5)
                                    {
                                        RazorEnhanced.AutoLoot.AddLog("- Max weight reached, Wait untill free some space");
                                        RazorEnhanced.Misc.SendMessage("AUTOLOOT: Max weight reached, Wait untill free some space", true);
                                        Thread.Sleep(2000);
                                    }
                                    else
                                    {
                                        RazorEnhanced.AutoLoot.AddLog("- Item Match found (" + item.Name.ToString() + ") ... Looting");
                                        int lootBagOverride = data.DestContainerOverride;
                                        Assistant.Item destCont = Assistant.World.FindItem(data.DestContainerOverride);
                                        if (destCont != null)
                                        {
                                            RazorEnhanced.Items.Move(item.Serial, Convert.ToInt32(lootBagOverride), 0);
                                        }
                                        else
                                        {
                                            RazorEnhanced.Items.Move(item.Serial, Convert.ToInt32(lootbag), 0);
                                        }
                                        Thread.Sleep(AutoLoot.AutoLootDelay);
                                        AutoLoot.SerialToGrabList.TryDequeue(out data);
                                    }
                                }
                                else
                                {
                                    AutoLoot.SerialToGrabList.TryDequeue(out data);
                                    AutoLoot.SerialToGrabList.Enqueue(data);
                                }
                            }
                        }
                        catch { }
                    }
                }
                finally
                {
                    Monitor.Exit(autolootLock);
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
