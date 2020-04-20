using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Assistant.Core.ActionQueue
{
    internal class Queue
    {

        private static readonly ThreadPriorityQueue<ActionQueueItem> _actionPacketQueue = new ThreadPriorityQueue<ActionQueueItem>(ProcessActionPacketQueue);
        private static readonly object _actionQueueLock = new object();

        private static void ProcessActionPacketQueue(ActionQueueItem queueItem)
        {
            lock (_actionQueueLock)
            {
                if (queueItem.DelaySend)
                {
                    while (Engine.LastActionPacket +
                            TimeSpan.FromMilliseconds(RazorEnhanced.Settings.General.ReadInt("ObjectDelay")) >
                            DateTime.Now)
                    {
                        Thread.Sleep(1);
                    }
                }

                Engine.LastActionPacket = DateTime.Now;
                Client.Instance.SendToServerWait(queueItem.Packet);
                queueItem.WaitHandle.Set();
            }
        }

        public static Task EnqueueActionPackets(IEnumerable<Packet> packets, QueuePriority priority = QueuePriority.Low, bool delaySend = true)
        {
            lock (_actionQueueLock)
            {
                List<EventWaitHandle> handles = new List<EventWaitHandle>();

                if (packets.Count() > 1)
                    delaySend = false;

                foreach (Packet packet in packets)
                {

                    ActionQueueItem queueItem = new ActionQueueItem(packet, delaySend);
                    handles.Add(queueItem.WaitHandle);
                    _actionPacketQueue.Enqueue(queueItem, priority);
                }

                return handles.ToTask();
            }
        }

        public static Task EnqueueDrag(int serial, int amount, QueuePriority priority = QueuePriority.Low,
            bool delaySend = true)
        {
            return EnqueueActionPackets(new Packet[] { new LiftRequest(serial, amount) }, priority, delaySend);

        }

        public static Task EnqueueDropContainer(int serial, int containerSerial, QueuePriority priority = QueuePriority.Low,
            bool delaySend = true)
        {
            return EnqueueActionPackets(new Packet[] { new DropRequest(World.FindItem(serial), World.FindItem(containerSerial)) }, priority, delaySend);

        }

        public static Task EnqueueDropRelative(int serial, int containerSerial, Point3D point3d, QueuePriority priority = QueuePriority.Low,
            bool delaySend = true)
        {
            return EnqueueActionPackets(new Packet[] { new DropRequest(serial, point3d, containerSerial) }, priority, delaySend);

        }

        public static Task EnqueueDragDrop(int serial, int amount, int containerSerial, Point3D point3d, QueuePriority priority = QueuePriority.Low, bool delaySend = true)
        {
            return EnqueueActionPackets(
                new Packet[] { new LiftRequest(serial, amount), new DropRequest(serial, point3d, containerSerial) },
                priority, delaySend);
            

        }

        public static Task EnqueueEquip(int serial, int amount, Mobile mobile, Layer layer, QueuePriority priority = QueuePriority.Low, bool delaySend = true)
        {
            return EnqueueActionPackets(
                new Packet[] { new LiftRequest(serial, amount), new EquipRequest(serial, mobile, layer) },
                priority, delaySend);

        }

        public static Task EnqueueDoubleClick(Serial s, QueuePriority priority = QueuePriority.Low, bool delaySend = true)
        {
            return EnqueueActionPackets(new Packet[] { new DoubleClick(s) }, priority, delaySend);
        }

        public static void Stop()
        {
            _actionPacketQueue.Clear();
        }

    }
}
