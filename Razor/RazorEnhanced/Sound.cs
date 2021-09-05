using Assistant;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace RazorEnhanced
{
    /// <summary>
    /// The Sound class provides an api to manipulate Sounds. 
    /// For now it just turns logging for sounds on / off or waits for a list of sounds
    /// All the WeakRef stuff seems like overkill and a pia. 
    /// The problem was if you started the wait and then killed the python script, the entry in the waiters list just stayed forever
    /// The only way around this is to have a weakref stored in the list, then if the local var ManualResetEvent went out of scope, 
    /// the WeakRef will go to null.  At end of loop we clean up all null entries so the list stays clean.
    /// </summary>

    public class Sound
    {
        static internal PacketViewerCallback m_Callback = new PacketViewerCallback(Sound.OnFilter);
        static internal bool m_logActive = false;

        static private bool m_registered = false;
        static private bool Register
        {
            get { return m_registered; }
            set
            {
                if ((value == true) && (m_registered == false))
                {
                    m_registered = true;
                    PacketHandler.RegisterServerToClientViewer(0x54, m_Callback);
                }
                if ((value == false) && (m_registered == true))
                {
                    PacketHandler.RemoveServerToClientViewer(0x54, m_Callback);
                    m_registered = false;
                }
            }
        }

        static internal bool Logging
        {
            get
            {
                return m_logActive;
            }
            set
            {
                m_logActive = value;
            }
        }

        /// <summary>
        /// Enables/Disables logging of incoming sound requests
        /// </summary>
        /// <param name="activateLogging"> True= activate sound logging/ False Deactivate sound logging</param>
        public static void Log(bool activateLogging)
        {
            Register = true;
            Logging = activateLogging;
        }


        public static void OnFilter(PacketReader p, PacketHandlerEventArgs args)
        {
            p.ReadByte(); // flags

            ushort sound = p.ReadUInt16();
            if (RazorEnhanced.Sound.Logging)
            {
                RazorEnhanced.Misc.SendMessage(string.Format("Play Sound 0x{0:x} - {0}", sound), false);
            }
            waiterMutex.WaitOne();
            bool needsCleanup = false;
            foreach (var entry in waiters)
            {
                if (entry.Item1 == null)
                {
                    needsCleanup = true;
                    continue;
                }
                if (entry.Item1 == null)
                {
                    needsCleanup = true;
                    continue;
                }
                ManualResetEvent anEvent = null;
                entry.Item1.TryGetTarget(out anEvent);
                if (anEvent != null)
                {
                    if (entry.Item2.Contains(sound))
                    {
                        anEvent.Set();
                    }
                }
                else {
                    needsCleanup = true;
                }
            }
            if (needsCleanup)
                waiters.RemoveWhere(IsDead);
            waiterMutex.ReleaseMutex();

        }
        private static bool IsDead(Tuple<WeakReference<ManualResetEvent>, List<int>> entry)
        {
            if (entry.Item1 == null)
                return true;
            entry.Item1.TryGetTarget(out var el);
            if (el == null)
                return true;

            return false;

        }

        internal static Mutex waiterMutex = new Mutex();
        internal static HashSet<Tuple<WeakReference<ManualResetEvent>, List<int>>> waiters = new HashSet<Tuple<WeakReference<ManualResetEvent>, List<int>>>();

        /// <summary>
        /// Waits for a sound to arrive, or for timeout 
        /// </summary>
        /// <param name="sounds"> list of sound ids to wait for</param>
        /// <param name="timeout"> maximum time to wait for sound to arrive. not specified will be 10 minutes</param>
        public static bool WaitForSound(List<int> sounds, int timeout = -1)
        {
            Register = true;
            ManualResetEvent manualResetEvent = new ManualResetEvent(false);

            Tuple<WeakReference<ManualResetEvent>, List<int>> entry = new Tuple<WeakReference<ManualResetEvent>, List<int>>(new WeakReference<ManualResetEvent>(manualResetEvent), sounds);
            waiterMutex.WaitOne();
            waiters.Add(entry);
            waiterMutex.ReleaseMutex();

            bool isSignalled = true;
            if (timeout < 1)
                isSignalled = manualResetEvent.WaitOne(TimeSpan.FromMinutes(10));  // never wait more than 10 minutes !
            else
                isSignalled = manualResetEvent.WaitOne(TimeSpan.FromMilliseconds(timeout));

            waiterMutex.WaitOne();
            waiters.Remove(entry);
            waiterMutex.ReleaseMutex();

            return isSignalled;
        }



    }
}
