using Accord.Math;
using RazorEnhanced;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Assistant
{
    public class BaseEvent{ 
        public DateTime Timestamp = DateTime.Now;
    }
    public class PacketEvent : BaseEvent
    {
        public string packetPath;
        public byte packetID;
        public byte[] packetData;
        
    }

    public class JournalEvent : BaseEvent
    {
        public Journal.JournalEntry line;
    }
    public class HotkeyEvent : BaseEvent
    {
        public string data;
        public string key;
        public bool modCtrl;
        public bool modAlt;
        public bool modShift;
        public bool modMeta;
        public bool modFn;
    }

    public class CastEvent : BaseEvent
    {
        public int soruce;
        public int spellID;
        public string spellName;
        public string spellSkill;
    }

    public class DamageEvent : BaseEvent
    {
        public int soruce;
        public int amount;
    }

    public class SoundEvent : BaseEvent
    {
        //TODO check fileds
        public int soruce;
        public int soundID;
    }

    public class TimeEvent : BaseEvent
    {
        //TODO check fileds
        public int soruce;
        public int soundID;
    }




    /*
   Events.OnPacket(packetid, callback)
   Events.OnJournal(pattern, callback)
   Events.OnHotKey(hotkeyid, callback)
   
   Events.OnCast(spellid, callback)
   Events.OnDamage(serial, callback)
   Events.OnSound(soundid, callback)
   
   Events.onTimeout(millisec, callback, repeatOnce)
   */

    public class EventManager
    {
        static readonly public EventManager Instance = new EventManager();


        public delegate void OnPacketCallback(string path, byte[] data);
        public delegate void OnJournalCallback(Journal.JournalEntry entry);
        public delegate void OnHotkeyCallback(string hotkey);



        private HashSet<Thread> m_Threads = new HashSet<Thread>();
        private readonly ConcurrentDictionary<Thread, ConcurrentDictionary<int, HashSet<OnPacketCallback>>> m_PacketCallbacks = new ConcurrentDictionary<Thread, ConcurrentDictionary<int, HashSet<OnPacketCallback>>>();
        private readonly ConcurrentDictionary<Thread, ConcurrentDictionary<string, HashSet<OnJournalCallback>>> m_JournalCallbacks = new ConcurrentDictionary<Thread, ConcurrentDictionary<string, HashSet<OnJournalCallback>>>();
        private readonly ConcurrentDictionary<Thread, ConcurrentDictionary<string, HashSet<OnHotkeyCallback>>> m_HotkeyCallbacks = new ConcurrentDictionary<Thread, ConcurrentDictionary<string, HashSet<OnHotkeyCallback>>>();
        

        // Auto-cleanup based on thead state/activity, can and should be improved.
        public void Cleanup()
        {
            //TODO: Verify this filter for real (Dalamar)

            var running = m_Threads.Where(thread => thread.IsAlive).ToHashSet();

            if (running.Count() == m_Threads.Count()) { return; }

            // Should lock (?)
            var dead = m_Threads.Where(thread => !running.Contains(thread));

            dead.ToList().ForEach(thread => Unsubscribe(thread));
        }

        private object m_Lock = new();
        public void Unsubscribe(Thread thread = null)
        {
            if (thread == null) { thread = Thread.CurrentThread; }
            lock (m_Lock) {
                m_PacketCallbacks.TryRemove(thread, out var _);
                m_JournalCallbacks.TryRemove(thread, out var _);
                m_HotkeyCallbacks.TryRemove(thread, out var _);
                m_Threads.Remove(thread);
            }
        }

        // notify event menager for new events

        public void didRecievePacket(PacketPath path, byte[] packetData)
        {
            var notify = new Task(() =>
            {
                int packetID = packetData[0];

                Cleanup();
                foreach (var pairs in m_PacketCallbacks)
                {
                    var thread = pairs.Key;
                    var packet = pairs.Value;
                    if (packetID != -1 && !packet.ContainsKey(packetID)) { continue; }
                    var callbacks = packet[packetID];
                    callbacks.ToList().ForEach(callback =>
                    {
                        try { 
                            callback(Packet.PathToString[path], packetData); 
                        } catch (Exception ex) {
                            Misc.SendMessage($"OnPacket: Error in callback {ex.Message}", 138);
                        }
                    });
                }
            });
            notify.Start();
        }

        public void didRecieveJournal(Journal.JournalEntry entry)
        {
            var notify = new Task(() =>
            {
                Cleanup();
                foreach (var pairs in m_JournalCallbacks)
                {
                    var thread = pairs.Key;
                    var matches = pairs.Value;

                    var validMatches = matches.ToArray().Where((match) => {
                        var textMatch = match.Key;
                        if (textMatch == "") { return true; } // Empty => match all
                        if (entry.Text.ToLowerInvariant().Equals(textMatch.ToLowerInvariant())) { return true; }  // Exact match
                        if (entry.Text.ToLowerInvariant().Contains(textMatch.ToLowerInvariant())) { return true; } // Sub amtch 

                        if (textMatch.StartsWith("/") && textMatch.EndsWith("/") && textMatch.Length > 2)
                        { // try regexpr. no, it doesn't support modifiers
                            try { 
                                return Regex.Match(entry.Text, textMatch).Success; 
                            } catch (Exception ex) {
                                Misc.SendMessage($"OnJournal: Error pattern RegEx: {textMatch}\nError: {ex.Message}", 138);
                            }
                }

                        return false;
                    });

                    validMatches.ToList().ForEach(match => {
                        var callbacks = match.Value;
                        callbacks.All(callback =>
                        {
                            try { 
                                callback(entry); 
                            } catch (Exception ex) {
                                Misc.SendMessage($"OnJournal: Error in callback {ex.Message}", 138);
                                return false; 
                            }
                            return true;
                        });
                    });
                }
            });
            notify.Start();
        }

        public void didRecieveHotkey(Keys hotkey)
        {
            var notify = new Task(() =>
            {
                var textHotkey = hotkey.ToString() ;
                Cleanup();
                foreach (var pairs in m_HotkeyCallbacks)
                {
                    var thread = pairs.Key;
                    var matches = pairs.Value;

                    var validMatches = matches.ToArray().Where( (match) => {
                        if (match.Key == "") { return true; }  // Empty => match all
                        return match.Key == textHotkey.ToLower();
                    });
                    validMatches.ToList().ForEach(match => {
                        var callbacks = match.Value;
                        callbacks.All(callback =>
                        {
                            try { 
                                callback(textHotkey); 
                            }catch (Exception ex){
                                Misc.SendMessage($"OnHotkey: Error in callback {ex.Message}", 138);
                                return false;
                            }
                            return true;
                        });
                    });
                }
            });
            notify.Start();
        }

        // Subscribe to the event manager to be notified for new events

        public void OnPacket(int packetID, OnPacketCallback callback)
        {
            if (packetID <= 0) { packetID = -1; }
            Thread thread = Thread.CurrentThread;
            if (!m_PacketCallbacks.ContainsKey(thread))
            {
                m_PacketCallbacks[thread] = new ConcurrentDictionary<int, HashSet<OnPacketCallback>>();
            }
            if (!m_PacketCallbacks[thread].ContainsKey(packetID))
            {
                m_PacketCallbacks[thread][packetID] = new HashSet<OnPacketCallback>();
            }
            m_PacketCallbacks[thread][packetID].Add(callback);
            m_Threads.Add(thread);
        }

        public void OnJournal(string match, OnJournalCallback callback)
        {
            match = match.Trim();
            Thread thread = Thread.CurrentThread;
            if (!m_JournalCallbacks.ContainsKey(thread))
            {
                m_JournalCallbacks[thread] = new ConcurrentDictionary<string, HashSet<OnJournalCallback>>();
            }
            if (!m_JournalCallbacks[thread].ContainsKey(match))
            {
                m_JournalCallbacks[thread][match] = new HashSet<OnJournalCallback>();
            }
            m_JournalCallbacks[thread][match].Add(callback);
            m_Threads.Add(thread);
        }

        public void OnHotkey(string hotkey, OnHotkeyCallback callback)
        {
            hotkey = hotkey.ToLower().Trim();
            Thread thread = Thread.CurrentThread;
            if (!m_HotkeyCallbacks.ContainsKey(thread))
            {
                m_HotkeyCallbacks[thread] = new ConcurrentDictionary<string, HashSet<OnHotkeyCallback>>();
            }
            if (!m_HotkeyCallbacks[thread].ContainsKey(hotkey))
            {
                m_HotkeyCallbacks[thread][hotkey] = new HashSet<OnHotkeyCallback>();
            }
            m_HotkeyCallbacks[thread][hotkey].Add(callback);
            m_Threads.Add(thread);
        }
    }
}
