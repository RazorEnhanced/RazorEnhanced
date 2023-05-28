using Assistant;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using static RazorEnhanced.Player;
using static RazorEnhanced.Trade;

namespace RazorEnhanced
{
    public class TradeService {
        public static readonly TradeService Instance = new TradeService();

        public static TradeData Copy(TradeData trade) {
            var tradeInfo = new TradeData(); //export a copy of the original object.
            tradeInfo.TradeID = trade.TradeID;
            tradeInfo.LastUpdate = trade.LastUpdate;
            tradeInfo.SerialTrader = trade.SerialTrader;
            tradeInfo.NameTrader = trade.NameTrader;
            tradeInfo.ContainerMe = trade.ContainerMe;
            tradeInfo.ContainerTrader = trade.ContainerTrader;
            tradeInfo.GoldMax = trade.GoldMax;
            tradeInfo.PlatinumMax = trade.PlatinumMax;
            tradeInfo.GoldMe = trade.GoldMe;
            tradeInfo.PlatinumMe = trade.PlatinumMe;
            tradeInfo.GoldTrader = trade.GoldTrader;
            tradeInfo.PlatinumTrader = trade.PlatinumTrader;
            tradeInfo.AcceptMe = trade.AcceptMe;
            tradeInfo.AcceptTrader = trade.AcceptTrader;
            return tradeInfo;
        }

        public static float Timestamp() { return ((DateTimeOffset)DateTime.UtcNow).ToUnixTimeMilliseconds() / 1000; }
        
        private readonly Dictionary<int, TradeData> m_SecureTrades = new Dictionary<int, TradeData>();

        public Dictionary<int, TradeData> TradeData
        {
            get { return m_SecureTrades; }
        }

        public List<TradeData> TradeList
        {
            get {
                return m_SecureTrades.Values.ToList().OrderBy(trade => trade.LastUpdate).ToList();
            }
        }



    }

    public class Trade
    {
        /// <summary>
        /// SecureTrades holds the information about a single tradeing window.
        /// </summary>
        public class TradeData
        {
            /// <summary>
            /// ID of the Trade.
            /// </summary>
            public int TradeID;

            /// <summary>
            /// Last update of the Trade as UnixTime ( format: "Seconds,Milliseconds" from 1-1-1970 )
            /// </summary>
            public float LastUpdate;

            /// <summary>
            /// Serial of the Trader (other) .
            /// </summary>
            public int SerialTrader;
            /// <summary>
            /// Name of the Trader (other).
            /// </summary>
            public string NameTrader;

            /// <summary>
            /// Serial of the container holding the items offerd by the Player (me).
            /// </summary>
            public int ContainerMe;

            /// <summary>
            /// Serial of the container holding the items offerd by the Trader (other).
            /// </summary>
            public int ContainerTrader;

            /// <summary>
            /// Maximum amount of Gold available for the Player (me).
            /// </summary>
            public int GoldMax;

            /// <summary>
            /// Maximum amount of Platinum available for the Player (me).
            /// </summary>
            public int PlatinumMax;

            /// <summary>
            /// Amount of Gold offerd by the Player (me).
            /// </summary>
            public int GoldMe;

            /// <summary>
            /// Amount of Platinum offerd by the Player (me).
            /// </summary>
            public int PlatinumMe;

            /// <summary>
            /// Amount of Gold offerd by the Trader (other).
            /// </summary>
            public int GoldTrader;

            /// <summary>
            /// Amount of Platinum offerd by the Trader (other).
            /// </summary>
            public int PlatinumTrader;

            /// <summary>
            /// Trade has been accepted by the Player (me).
            /// </summary>
            public bool AcceptMe;

            /// <summary>
            /// Trade has been accepted by the Trader (other).
            /// </summary>
            public bool AcceptTrader;
        }

        /// <summary>
        /// Returns the list of currently active Secure Trading gumps, sorted by LastUpdate.
        /// </summary>
        /// <returns>A list of Player.SecureTrade objects. Each containing the details of each trade window.</returns>
        public static List<TradeData> TradeList()
        {
            var trades = TradeService.Instance.TradeList.Select(trade => TradeService.Copy(trade));
            return trades.ToList();
        }

        /// <summary>
        /// Set the accept state of the trade
        /// </summary>
        /// <param name="TradeID">ID of the Trade (Default = -1: Pick a random active trade)</param>
        /// <param name="accept">Set the state ofthe checkbox</param>
        /// <returns>True: Trade found, False: Trade not found</returns>
        public static bool Accept(int TradeID, bool accept = true)
        {
            if (!TradeService.Instance.TradeData.ContainsKey(TradeID)) { return false; }
            var packet = new TradeAccept((uint)TradeID, accept);
            Assistant.Client.Instance.SendToServer(packet);
            
            return true;
        }
        public static bool Accept(bool accept = true)
        {
            if (TradeService.Instance.TradeData.Count == 0) { return false; }
            var trade = TradeService.Instance.TradeList.First();
            return Accept(trade.TradeID, accept);
        }

        /// <summary>
        /// Set the accept state of the trade
        /// </summary>
        /// <param name="TradeID">ID of the Trade (Default = -1: Pick a random active trade)</param>
        /// <returns>True: Trade found, False: Trade not found</returns>
        public static bool Cancel(int TradeID)
        {
            if (!TradeService.Instance.TradeData.ContainsKey(TradeID)) { return false; }
            //World.Player.SecureTrades.Remove(TradeID);

            var packet = new TradeCancel((uint)TradeID);
            Assistant.Client.Instance.SendToServer(packet);

            return true;
        }
        public static bool Cancel()
        {
            if (TradeService.Instance.TradeData.Count == 0) { return false; }
            var trade = TradeService.Instance.TradeList.First();
            return Cancel(trade.TradeID);
        }

        /// <summary>
        /// Update the amount of gold and platinum in the trade. ( client view dosen't update )
        /// </summary>
        /// <param name="TradeID">ID of the Trade (Default = -1: Pick latest active trade)</param>
        /// <param name="gold">Amount of Gold to offer</param>
        /// <param name="platinum">Amount of Platinum to offer</param>
        /// <param name="quiet">Suppress output (Default: false - Show warning)</param>
        /// <returns>True: Trade found, False: Trade not found</returns>
        public static bool Offer(int TradeID, int gold, int platinum, bool quiet = false)
        {
            if (!TradeService.Instance.TradeData.ContainsKey(TradeID)) { return false; }
            var trade = TradeService.Instance.TradeData[TradeID];

            var packet = new TradeOffer((uint)TradeID, (uint)gold, (uint)platinum);
            Assistant.Client.Instance.SendToServer(packet); //DONT SEND TO CLIENT, EVER
            if (!quiet) { 
                Misc.SendMessage("WARNING: Trade.Offer() CANNOT update the offer Gold and Platinum on client, but works.", 148);
                Misc.SendMessage($"CURRENTLY OFFERING: \nGold:     {gold} \nPlatinum: {platinum}", 148);
            }
            trade.LastUpdate = TradeService.Timestamp();
            trade.GoldMe = gold;
            trade.PlatinumMe = platinum;
            return true;
        }

        public static bool Offer(int gold, int platinum, bool quiet = false) {
            if (TradeService.Instance.TradeData.Count == 0) { return false; }
            var trade = TradeService.Instance.TradeList.First();
            return Offer(trade.TradeID, gold,platinum, quiet);
        }
    }
}
