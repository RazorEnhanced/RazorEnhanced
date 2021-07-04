using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace Assistant
{
	internal class DragDropManager
	{
		internal enum ProcStatus
		{
			Nothing,
			Success,
			KeepWaiting,
			ReQueue,
		}

		internal static bool Debug = false;

		private static void Log(string str, params object[] args)
		{
			if (!Debug)
				return;

			try
			{
				using (StreamWriter w = new StreamWriter("DragDrop.log", true))
				{
					w.Write(DateTime.Now.ToString("HH:mm:ss.fff"));
					w.Write(":: ");
					w.WriteLine(str, args);
					w.Flush();
				}
			}
			catch
			{
			}
		}

		private class LiftReq
		{
			private static int NextID = 1;

			internal LiftReq(Serial s, int a, bool cli, bool last)
			{
				Serial = s;
				Amount = a;
				FromClient = cli;
				DoLast = last;
				Id = NextID++;
			}

			internal Serial Serial;
			internal int Amount;
			internal int Id;
			internal bool FromClient;
			internal bool DoLast;

			public override string ToString()
			{
				return String.Format("{2}({0},{1},{3},{4})", Serial, Amount, Id, FromClient, DoLast);
			}
		}

		private class DropReq
		{
			internal DropReq(Serial s, Point3D pt)
			{
				Serial = s; Point = pt;
			}

			internal DropReq(Serial s, Layer layer)
			{
				Serial = s; Layer = layer;
			}

			internal Serial Serial;
			internal Point3D Point;
			internal Layer Layer;
		}

		public static void Initialize()
		{

		}


		private static int m_LastID;

		private static Serial m_Pending, m_Holding;
		private static Item m_HoldingItem;
		private static bool m_ClientLiftReq = false;
		private static DateTime m_Lifted = DateTime.MinValue;

		private static Dictionary<Serial, Queue<DropReq>> m_DropReqs = new Dictionary<Serial, Queue<DropReq>>();

		private static LiftReq[] m_LiftReqs = new LiftReq[500];
		private static byte m_Front, m_Back;

		internal static Item Holding { get { return m_HoldingItem; } }
		internal static Serial Pending { get { return m_Pending; } }

		internal static int LastIDLifted { get { return m_LastID; } }

		internal static void Clear()
		{
			Log("Clearing....");

			m_DropReqs.Clear();
			for (int i = 0; i < 256; i++)
				m_LiftReqs[i] = null;
			m_Front = m_Back = 0;
			m_Holding = m_Pending = Serial.Zero;
			m_HoldingItem = null;
			m_Lifted = DateTime.MinValue;
		}

		internal static void DragDrop(Item i, Serial to)
		{
			Drag(i, i.Amount);
			Drop(i, to, Point3D.MinusOne);
		}

		internal static void DragDrop(Item i, int amount, Serial to)
		{
			Drag(i, amount);
			Drop(i, to, Point3D.MinusOne);
		}

		internal static void DragDrop(Item i, Item to)
		{
			Drag(i, i.Amount);
			Drop(i, to.Serial, Point3D.MinusOne);
		}

		internal static void DragDrop(Item i, Point3D dest)
		{
			Drag(i, i.Amount);
			Drop(i, Serial.MinusOne, dest);
		}

		internal static void DragDrop(Item i, Point3D dest, int amount)
		{
			Drag(i, amount);
			Drop(i, Serial.MinusOne, dest);
		}

		internal static void DragDrop(Item i, int amount, Item to)
		{
			Drag(i, amount);
			Drop(i, to.Serial, Point3D.MinusOne);
		}
		internal static void DragDrop(Item i, int amount, Item to, Point3D dest)
		{
			Drag(i, amount);
			Drop(i, to.Serial, dest);
		}

		internal static void DragDrop(Item i, Mobile to, Layer layer, int amount)
		{
			Drag(i, amount, false);
			Drop(i, to, layer);
		}

		internal static void DragDrop(Item i, Mobile to, Layer layer, bool doLast)
		{
			Drag(i, i.Amount, false, doLast);
			Drop(i, to, layer);
		}

		internal static void DragDrop(Item i, Mobile to, Layer layer)
		{
			Drag(i, i.Amount, false);
			Drop(i, to, layer);
		}

		internal static int Drag(Item i, int amount, bool fromClient)
		{
			return Drag(i, amount, fromClient, false);
		}

		internal static int Drag(Item i, int amount)
		{
			return Drag(i, amount, false, false);
		}

		internal static bool Full { get { return ((byte)(m_Back + 1)) == m_Front; } }

		internal static int Drag(Item i, int amount, bool fromClient, bool doLast)
		{
			LiftReq lr = new LiftReq(i.Serial, amount, fromClient, doLast);
			LiftReq prev = null;

			if (Full)
			{
                Log("Queue FULL {0}", lr);
                World.Player.SendMessage(MsgLevel.Error, LocString.DragDropQueueFull);
				if (fromClient)
					Assistant.Client.Instance.SendToClient(new LiftRej());
				return 0;
			}

			Log("Queuing Drag request {0}", lr);

			if (m_Back >= m_LiftReqs.Length)
				m_Back = 0;

			if (m_Back <= 0)
				prev = m_LiftReqs[m_LiftReqs.Length - 1];
			else if (m_Back <= m_LiftReqs.Length)
				prev = m_LiftReqs[m_Back - 1];

			// if the current last req must stay last, then insert this one in its place
			if (prev != null && prev.DoLast)
			{
				Log("Back-Queuing {0}", prev);
				if (m_Back <= 0)
					m_LiftReqs[m_LiftReqs.Length - 1] = lr;
				else if (m_Back <= m_LiftReqs.Length)
					m_LiftReqs[m_Back - 1] = lr;

				// and then re-insert it at the end
				lr = prev;
			}

			m_LiftReqs[m_Back++] = lr;

			ActionQueue.SignalLift(!fromClient);
			return lr.Id;
		}

		internal static bool Drop(Item i, Mobile to, Layer layer)
		{
			if (m_Pending == i.Serial)
			{
				Log("Equipping {0} to {1} (@{2})", i, to.Serial, layer);
			 	Assistant.Client.Instance.SendToServer(new EquipRequest(i.Serial, to, layer));
				m_Pending = Serial.Zero;
				m_Lifted = DateTime.MinValue;
				return true;
			}
			else
			{
				bool add = false;

				for (byte j = m_Front; j != m_Back && !add; j++)
				{
					if (m_LiftReqs[j] != null && m_LiftReqs[j].Serial == i.Serial)
					{
						add = true;
						break;
					}
				}

				if (add)
				{
					Log("Queuing Equip {0} to {1} (@{2})", i, to.Serial, layer);

					if (!m_DropReqs.ContainsKey(i.Serial))
						m_DropReqs.Add(i.Serial, new Queue<DropReq>());

					Queue<DropReq> q = m_DropReqs[i.Serial];
					q.Enqueue(new DropReq(to == null ? Serial.Zero : to.Serial, layer));
					return true;
				}
				else
				{
					Log("Drop/Equip for {0} (to {1} (@{2})) not found, skipped", i, to == null ? Serial.Zero : to.Serial, layer);
					return false;
				}
			}
		}

		internal static bool Drop(Item i, Serial dest, Point3D pt)
		{
			if (m_Pending == i.Serial)
			{
				Log("Dropping {0} to {1} (@{2})", i, dest, pt);

			 	Assistant.Client.Instance.SendToServer(new DropRequest(i.Serial, pt, dest));
				m_Pending = Serial.Zero;
				m_Lifted = DateTime.MinValue;
				return true;
			}
			else
			{
				bool add = false;

				for (byte j = m_Front; j != m_Back && !add; j++)
				{
					if (m_LiftReqs[j] != null && m_LiftReqs[j].Serial == i.Serial)
					{
						add = true;
						break;
					}
				}

				if (add)
				{
					Log("Queuing Drop {0} (to {1} (@{2}))", i, dest, pt);

					if (!m_DropReqs.ContainsKey(i.Serial))
						m_DropReqs.Add(i.Serial, new Queue<DropReq>());

					Queue<DropReq> q = m_DropReqs[i.Serial];
					q.Enqueue(new DropReq(dest, pt));
					return true;
				}
				else
				{
					Log("Drop for {0} (to {1} (@{2})) not found, skipped", i, dest, pt);
					return false;
				}
			}
		}

		internal static bool Drop(Item i, Item to, Point3D pt)
		{
			return Drop(i, to == null ? Serial.MinusOne : to.Serial, pt);
		}

		internal static bool Drop(Item i, Item to)
		{
			return Drop(i, to.Serial, Point3D.MinusOne);
		}

		internal static bool LiftReject()
		{
			Log("Server rejected lift for item {0}", m_Holding);
			if (m_Holding == Serial.Zero)
				return true;

			m_Holding = m_Pending = Serial.Zero;
			m_HoldingItem = null;
			m_Lifted = DateTime.MinValue;

			return m_ClientLiftReq;
		}

		internal static bool HasDragFor(Serial s)
		{
			for (byte j = m_Front; j != m_Back; j++)
			{
				if (m_LiftReqs[j] != null && m_LiftReqs[j].Serial == s)
					return true;
			}

			return false;
		}

		internal static bool EndHolding(Serial s)
		{
			if (m_Holding == s)
			{
				m_Holding = Serial.Zero;
				m_HoldingItem = null;
			}

			return true;
		}

		private static DropReq DequeueDropFor(Serial s)
		{
			if (!m_DropReqs.ContainsKey(s))
				return null;

			DropReq dr = null;
			Queue<DropReq> q = m_DropReqs[s];
			if (q.Count > 0)
				dr = q.Dequeue();
			if (q.Count <= 0)
				m_DropReqs.Remove(s);
			return dr;
		}

		internal static ProcStatus ProcessNext(int numPending)
		{
			if (m_Pending != Serial.Zero)
			{
				if (m_Lifted + TimeSpan.FromMinutes(2) < DateTime.Now)
				{
					Item i = World.FindItem(m_Pending);

				    Log("Lift timeout, forced drop to pack for {0}", m_Pending);

					if (World.Player != null)
					{
						World.Player.SendMessage(MsgLevel.Force, LocString.ForceEndHolding);

						if (World.Player.Backpack != null)
						 	Assistant.Client.Instance.SendToServer(new DropRequest(m_Pending, Point3D.MinusOne, World.Player.Backpack.Serial));
						else
						 	Assistant.Client.Instance.SendToServer(new DropRequest(m_Pending, World.Player.Position, Serial.Zero));
					}

					m_Holding = m_Pending = Serial.Zero;
					m_HoldingItem = null;
					m_Lifted = DateTime.MinValue;
				}
				else
				{
					return ProcStatus.KeepWaiting;
				}
			}

			if (m_Front >= m_Back)
			{
				m_Front = m_Back = 0;
				return ProcStatus.Nothing;
			}

			LiftReq lr = m_LiftReqs[m_Front];

			if (numPending > 0 && lr != null && lr.DoLast)
				return ProcStatus.ReQueue;

			m_LiftReqs[m_Front] = null;
			m_Front++;

            if (lr != null)
			{
				Log("Lifting {0}", lr);

				Item item = World.FindItem(lr.Serial);
				if (item != null && item.Container == null)
				{ // if the item is on the ground and out of range then dont grab it
					if (Utility.Distance(item.GetWorldPosition(), World.Player.Position) > 3)
					{
						Log("Item is too far away... uncaching.");
						return ProcStatus.Nothing;
					}
				}

			 	Assistant.Client.Instance.SendToServer(new LiftRequest(lr.Serial, lr.Amount));

				m_LastID = lr.Id;
				m_Holding = lr.Serial;
				m_HoldingItem = World.FindItem(lr.Serial);
				m_ClientLiftReq = lr.FromClient;

				DropReq dr = DequeueDropFor(lr.Serial);
				if (dr != null)
				{
					m_Pending = Serial.Zero;
					m_Lifted = DateTime.MinValue;

					Log("Dropping {0} to {1}", lr, dr.Serial);

					if (dr.Serial.IsMobile && dr.Layer > Layer.Invalid && dr.Layer <= Layer.LastUserValid)
					 	Assistant.Client.Instance.SendToServer(new EquipRequest(lr.Serial, dr.Serial, dr.Layer));
					else
					 	Assistant.Client.Instance.SendToServer(new DropRequest(lr.Serial, dr.Point, dr.Serial));
				}
				else
				{
					m_Pending = lr.Serial;
					m_Lifted = DateTime.Now;
				}

				return ProcStatus.Success;
			}
			else
			{
				Log("No lift to be done?!");
				return ProcStatus.Nothing;
			}
		}
	}

	internal class ActionQueue
	{
		private static Serial m_Last = Serial.Zero;
		private static Queue<Serial> m_Queue = new Queue<Serial>();
		private static ProcTimer m_Timer = new ProcTimer();
		private static int m_Total = 0;

		internal static void DoubleClick(bool silent, Serial s)
		{
			if (s == Serial.Zero)
				return;

			if (m_Last != s)
			{
				m_Queue.Enqueue(s);
				m_Last = s;
				m_Total++;
				if (m_Queue.Count == 1 && !m_Timer.Running)
					m_Timer.StartMe();
				else if (!silent && m_Total > 1)
					World.Player.SendMessage(LocString.ActQueued, m_Queue.Count, TimeLeft);
			}
			else if (!silent)
			{
				World.Player.SendMessage(LocString.QueueIgnore);
			}
		}

		internal static void SignalLift(bool silent)
		{
			m_Queue.Enqueue(Serial.Zero);
			m_Total++;
			if ( /*m_Queue.Count == 1 &&*/ !m_Timer.Running)
				m_Timer.StartMe();
			else if (!silent && m_Total > 1)
				World.Player.SendMessage(LocString.LiftQueued, m_Queue.Count, TimeLeft);
		}

		internal static void Stop()
		{
            if (m_Timer != null && m_Timer.Running)
            {
                m_Timer.Stop();
            }
			m_Queue.Clear();
			DragDropManager.Clear();
		}

		internal static string TimeLeft
		{
			get
			{
				if (m_Timer.Running)
				{
					double time = RazorEnhanced.Settings.General.ReadInt("ObjectDelay") / 1000.0;
					double init = 0;
					if (m_Timer.LastTick != DateTime.MinValue)
						init = time - (DateTime.Now - m_Timer.LastTick).TotalSeconds;
					time = init + time * m_Queue.Count;
					if (time < 0)
						time = 0;
					return String.Format("{0:F1} seconds", time);
				}
				else
				{
					return "0.0 seconds";
				}
			}
		}

		private class ProcTimer
		{

            private DateTime m_StartTime;
			private DateTime m_LastTick;

            private ManualResetEvent m_stop = new ManualResetEvent(false);
            private RegisteredWaitHandle m_registeredWait = null;
            public bool Running
            {
                get { return null != m_registeredWait; }
            }

            internal DateTime LastTick { get { return m_LastTick; } }

            internal ProcTimer()
            {
                Stop();

            }

            internal void Start()
            {
                int interval = RazorEnhanced.Settings.General.ReadInt("ObjectDelay");
                m_registeredWait = ThreadPool.RegisterWaitForSingleObject(m_stop, new WaitOrTimerCallback(OnTick),
                                                                          null, interval, false);


            }

            internal void Stop()
            {
                if (m_registeredWait != null)
                {
                    m_registeredWait.Unregister(null);
                    m_registeredWait = null;
                }
            }

            internal void StartMe()
			{
				m_LastTick = DateTime.Now;
				m_StartTime = DateTime.Now;

				OnTick(null, true);

				Start();
			}

			protected void OnTick(object state, bool timeout)
			{
				List<Serial> requeue = null;

				m_LastTick = DateTime.Now;

				if (m_Queue != null && m_Queue.Count > 0)
				{

					while (m_Queue.Count > 0)
					{
						Serial s = m_Queue.Peek();
						if (s == Serial.Zero) // dragdrop action
						{
							DragDropManager.ProcStatus status = DragDropManager.ProcessNext(m_Queue.Count - 1);
							if (status != DragDropManager.ProcStatus.KeepWaiting && m_Queue.Count > 0)
							{
								m_Queue.Dequeue(); // if not waiting then dequeue it

								if (status == DragDropManager.ProcStatus.ReQueue)
									m_Queue.Enqueue(s);
							}

							if (status == DragDropManager.ProcStatus.KeepWaiting || status == DragDropManager.ProcStatus.Success)
								break; // don't process more if we're waiting or we just processed something
						}
						else
						{
							m_Queue.Dequeue();
							Assistant.Client.Instance.SendToServer(new DoubleClick(s));
							break;
						}
					}

					if (requeue != null)
					{
						for (int i = 0; i < requeue.Count; i++)
							m_Queue.Enqueue(requeue[i]);
					}
				}
				else
				{
					Stop();

				//	if (m_Total > 1 && World.Player != null)
				//		World.Player.SendMessage(LocString.QueueFinished, m_Total, ((DateTime.Now - m_StartTime) - this.Interval).TotalSeconds);

					m_Last = Serial.Zero;
					m_Total = 0;
				}
			}
		}
	}
}
