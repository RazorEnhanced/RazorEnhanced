using System;
using System.Threading;

namespace RazorEnhanced
{
    internal static class AgentStatusGump
    {
        private const uint GUMP_ID = 887766;
        private const int POLL_INTERVAL_MS = 200;

        private const int BG_GUMP_ID = 5054;
        private const int TILE_GUMP_ID = 2624;
        private const int BTN_ACTIVE = 9027;
        private const int BTN_INACTIVE = 9026;
        private const int HUE_ACTIVE = 68;
        private const int HUE_INACTIVE = 1153;

        private static readonly (string Label, int BtnId, int SlotWidth)[] AgentMeta = {
            ("Auto Loot",   1, 90),
            ("Scavenger",   2, 90),
            ("Bandage Heal", 3, 110),
            ("Vendor Buy",  4, 95),
            ("Vendor Sell", 5, 95),
        };

        private static System.Threading.Timer m_Timer;
        private static System.Threading.Timer m_RefreshTimer;
        private static readonly object m_Lock = new object();
        private static long m_LastStateHash;
        private static int m_GumpX;
        private static int m_GumpY;

        public static void Initialize()
        {
            m_GumpX = Settings.General.ReadInt("GumpStatusX");
            m_GumpY = Settings.General.ReadInt("GumpStatusY");

            if (IsAnyGumpEnabled())
                EnsureTimerRunning();
        }

        public static void RequestStart()
        {
            EnsureTimerRunning();
        }

        public static void RequestStop()
        {
            if (!IsAnyGumpEnabled())
            {
                if (m_Timer != null)
                {
                    m_Timer.Change(System.Threading.Timeout.Infinite, System.Threading.Timeout.Infinite);
                    m_Timer = null;
                }
                if (m_RefreshTimer != null)
                {
                    m_RefreshTimer.Change(System.Threading.Timeout.Infinite, System.Threading.Timeout.Infinite);
                    m_RefreshTimer = null;
                }
                lock (m_Lock)
                    m_LastStateHash = 0;
                CloseGumpIfOpen();
            }
        }

        private static void EnsureTimerRunning()
        {
            if (m_Timer == null)
            {
                lock (m_Lock)
                    m_LastStateHash = 0;
                m_Timer = new System.Threading.Timer(Poll, null, POLL_INTERVAL_MS, POLL_INTERVAL_MS);
            }
        }

        private static void Poll(object state)
        {
            if (!Assistant.Client.Instance.ClientRunning || Assistant.World.Player == null)
            {
                CloseGumpIfOpen();
                return;
            }

            long hash = ComputeStateHash();

            bool needsRefresh = false;
            lock (m_Lock)
            {
                if (hash != m_LastStateHash)
                {
                    m_LastStateHash = hash;
                    needsRefresh = true;
                }
            }

            if (!needsRefresh)
                return;

            if (!IsAnyGumpEnabled())
            {
                CloseGumpIfOpen();
                return;
            }

            SendGump();
        }

        private static long ComputeStateHash()
        {
            long hash = 0;
            for (int i = 0; i < AgentMeta.Length; i++)
            {
                bool showGump = GetShowGump(i);
                bool enabled = GetAgentEnabled(i);
                hash = ((hash << 5) ^ hash) ^ (showGump ? 1L << (i * 2) : 0) ^ (enabled ? 1L << (i * 2 + 1) : 0);
            }
            if (Settings.General.ReadBool("AgentGumpVertical"))
                hash ^= 1L << 31;
            return hash;
        }

        private static bool IsAnyGumpEnabled()
        {
            return Settings.General.ReadBool("GumpStatusEnabled");
        }

        private static bool GetShowGump(int index)
        {
            return IsAnyGumpEnabled();
        }

        private static bool GetAgentEnabled(int index)
        {
            if (Assistant.Engine.MainWindow == null)
                return false;

            switch (index)
            {
                case 0: return AutoLoot.AutoMode;
                case 1: return Scavenger.AutoMode;
                case 2: return BandageHeal.AutoMode;
                case 3: return BuyAgent.Status();
                case 4: return SellAgent.Status();
                default: return false;
            }
        }

        private static void ToggleAgent(int index)
        {
            switch (index)
            {
                case 0:
                    AutoLoot.AutoMode = !AutoLoot.AutoMode;
                    break;
                case 1:
                    Scavenger.AutoMode = !Scavenger.AutoMode;
                    break;
                case 2:
                    BandageHeal.AutoMode = !BandageHeal.AutoMode;
                    break;
                case 3:
                    if (BuyAgent.Status())
                        BuyAgent.Disable();
                    else BuyAgent.Enable();
                    break;
                case 4:
                    if (SellAgent.Status())
                        SellAgent.Disable();
                    else SellAgent.Enable();
                    break;
            }
        }

        private static void CloseGumpIfOpen()
        {
            Gumps.CloseGump(GUMP_ID);
        }

        public static void SendGump()
        {
            m_GumpX = Settings.General.ReadInt("GumpStatusX");
            m_GumpY = Settings.General.ReadInt("GumpStatusY");

            CloseGumpIfOpen();

            var gd = Gumps.CreateGump(true, false, true, false);
            gd.gumpId = GUMP_ID;
            gd.serial = (uint)Player.Serial;
            gd.x = (uint)m_GumpX;
            gd.y = (uint)m_GumpY;
            gd.action = OnGumpResponse;

            bool vertical = Settings.General.ReadBool("AgentGumpVertical");

            Gumps.AddPage(ref gd, 0);

            if (vertical)
            {
                int activeCount = 0;
                for (int i = 0; i < AgentMeta.Length; i++)
                    if (GetShowGump(i)) activeCount++;

                int bgHeight = 20 + activeCount * 21;
                int tileHeight = bgHeight - 12;

                Gumps.AddBackground(ref gd, 0, 0, 130, bgHeight, BG_GUMP_ID);
                Gumps.AddImageTiled(ref gd, 5, 6, 120, tileHeight, TILE_GUMP_ID);
                Gumps.AddAlphaRegion(ref gd, 5, 6, 120, tileHeight);

                int y = 10;
                for (int i = 0; i < AgentMeta.Length; i++)
                {
                    if (!GetShowGump(i))
                        continue;

                    var (label, btnId, _) = AgentMeta[i];
                    bool isActive = GetAgentEnabled(i);
                    int graphic = isActive ? BTN_ACTIVE : BTN_INACTIVE;
                    int hue = isActive ? HUE_ACTIVE : HUE_INACTIVE;

                    Gumps.AddButton(ref gd, 10, y, graphic, graphic, btnId, 1, 0);
                    Gumps.AddLabel(ref gd, 32, y, hue, label);

                    y += 21;
                }
            }
            else
            {
                int totalWidth = 10;
                for (int i = 0; i < AgentMeta.Length; i++)
                {
                    if (GetShowGump(i))
                        totalWidth += AgentMeta[i].SlotWidth;
                }
                totalWidth += 10;

                Gumps.AddBackground(ref gd, 0, 0, totalWidth, 39, BG_GUMP_ID);
                Gumps.AddImageTiled(ref gd, 5, 6, totalWidth - 10, 28, TILE_GUMP_ID);
                Gumps.AddAlphaRegion(ref gd, 5, 6, totalWidth - 10, 28);

                int x = 10;
                for (int i = 0; i < AgentMeta.Length; i++)
                {
                    if (!GetShowGump(i))
                        continue;

                    var (label, btnId, slotWidth) = AgentMeta[i];
                    bool isActive = GetAgentEnabled(i);
                    int graphic = isActive ? BTN_ACTIVE : BTN_INACTIVE;
                    int hue = isActive ? HUE_ACTIVE : HUE_INACTIVE;

                    Gumps.AddButton(ref gd, x, 10, graphic, graphic, btnId, 1, 0);
                    Gumps.AddLabel(ref gd, x + 22, 10, hue, label);

                    x += slotWidth;
                }
            }

            Gumps.SendGump(gd, (uint)m_GumpX, (uint)m_GumpY);
        }

        public static void SetPosition(int x, int y)
        {
            m_GumpX = x;
            m_GumpY = y;
            Settings.General.WriteInt("GumpStatusX", x);
            Settings.General.WriteInt("GumpStatusY", y);
            Refresh();
        }

        public static void Refresh()
        {
            if (!IsAnyGumpEnabled())
                return;
            CloseGumpIfOpen();
            lock (m_Lock)
                m_LastStateHash = ComputeStateHash();
            if (m_RefreshTimer != null)
            {
                m_RefreshTimer.Change(System.Threading.Timeout.Infinite, System.Threading.Timeout.Infinite);
                m_RefreshTimer = null;
            }
            m_RefreshTimer = new System.Threading.Timer(_ =>
            {
                if (IsAnyGumpEnabled())
                    SendGump();
            }, null, 150, System.Threading.Timeout.Infinite);
        }

        public static void PickPosition(Action<int, int> onChanged = null)
        {
            LocationPicker.Pick((x, y) =>
            {
                SetPosition(x, y);
                onChanged?.Invoke(x, y);
            });
        }

        private static void OnGumpResponse(Gumps.GumpData gd)
        {
            int btnId = gd.buttonid;
            if (btnId <= 0)
                return;

            for (int i = 0; i < AgentMeta.Length; i++)
            {
                if (AgentMeta[i].BtnId == btnId)
                {
                    ToggleAgent(i);
                    break;
                }
            }

            lock (m_Lock)
                m_LastStateHash = 0;

            Poll(null);
        }
    }
}
