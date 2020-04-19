using System.Threading;

namespace Assistant.Core.ActionQueue
{
    public class ActionQueueItem
    {
        public ActionQueueItem(Packet packet, bool delaySend)
        {
            Packet = packet;
            DelaySend = delaySend;
            WaitHandle = new AutoResetEvent(false);
        }

        public bool DelaySend { get; set; }
        public Packet Packet { get; set; }
        public EventWaitHandle WaitHandle { get; set; }
    }
}
