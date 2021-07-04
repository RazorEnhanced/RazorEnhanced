using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace JsonData
{

    public class TargetGUI  : RazorEnhanced.ListAbleItem
    {
        // Selector List
        internal static List<string> Selectors = new List<string>
        {
            "Random",
            "Nearest",
            "Farthest",
            "Weakest",
            "Strongest",
            "Next",
            "Previous"
        };


        string m_Name = "";

        [JsonProperty("Name")]
        public string Name
        {
            get { return m_Name; }
            set { m_Name = value; }
        }


        TargetGuiObject m_guiObject = new TargetGuiObject();

        [JsonProperty("TargetGUIObject")]
        public TargetGuiObject TargetGuiObject
        {
            get { return m_guiObject; }
            set { m_guiObject = value; }
        }

        Keys m_key = Keys.None;

        [JsonProperty("HotKey")]
        public Keys HotKey
        {
            get { return m_key; }
            set { m_key = value; }
        }

        private bool hotKeyPass = false;
        [JsonProperty("HotKeyPass")]
        public bool HotKeyPass { get => hotKeyPass; set => hotKeyPass = value; }

    }

    public partial class TargetGuiObject
    {
        private string selector = "";

        [JsonProperty("Selector")]
        public string Selector { get => selector; set => selector = value; }

        private Filter filter = new Filter();
        [JsonProperty("Filter")]
        public Filter Filter { get => filter; set => filter = value; }
    }

    public partial class Filter
    {

        public static Filter FromMobileFilter(RazorEnhanced.Mobiles.Filter mobileFilter)
        {
            Filter filter = new Filter();
            filter.Enabled = mobileFilter.Enabled;
            filter.Serials = mobileFilter.Serials;
            filter.Bodies = mobileFilter.Bodies;
            filter.Name = mobileFilter.Name;
            filter.Hues = mobileFilter.Hues;
            filter.RangeMin = mobileFilter.RangeMin;
            filter.RangeMax = mobileFilter.RangeMax;
            filter.CheckLineOfSite = mobileFilter.CheckLineOfSite;
            filter.Poisoned = mobileFilter.Poisoned;
            filter.Blessed = mobileFilter.Blessed;
            filter.Female = mobileFilter.Female;
            filter.IsHuman = mobileFilter.IsHuman;
            filter.IsGhost = mobileFilter.IsGhost;
            filter.Warmode = mobileFilter.Warmode;
            filter.Friend = mobileFilter.Friend;
            filter.Paralized = mobileFilter.Paralized;
            filter.CheckIgnoreObject = mobileFilter.CheckIgnoreObject;
            filter.Notorieties = mobileFilter.Notorieties;
            return filter;
        }

        private bool enabled = false;

        [JsonProperty("Enabled")]
        public bool Enabled { get => enabled; set => enabled = value; }

        private List<int> serials = new List<int>();

        [JsonProperty("Serials")]
        public List<int> Serials { get => serials; set => serials = value; }

        private List<int> bodies = new List<int>();

        [JsonProperty("Bodies")]
        public List<int> Bodies { get => bodies; set => bodies = value; }

        private string name = "";

        [JsonProperty("Name")]
        public string Name { get => name; set => name = value; }

        private List<int> hues = new List<int>();

        [JsonProperty("Hues")]
        public List<int> Hues { get => hues; set => hues = value; }

        private double rangeMin = -1;

        [JsonProperty("RangeMin")]
        public double RangeMin { get => rangeMin; set => rangeMin = value; }

        private double rangeMax = -1;

        [JsonProperty("RangeMax")]
        public double RangeMax { get => rangeMax; set => rangeMax = value; }

        [JsonProperty("CheckLineOfSite")]
        public bool CheckLineOfSite { get; set; }

        private int poisoned = 0;

        [JsonProperty("Poisoned")]
        public int Poisoned { get => poisoned; set => poisoned = value; }

        private int blessed = 0;

        [JsonProperty("Blessed")]
        public int Blessed { get => blessed; set => blessed = value; }

        private int isHuman = 0;

        [JsonProperty("IsHuman")]
        public int IsHuman { get => isHuman; set => isHuman = value; }

        private int isGhost = 0;

        [JsonProperty("IsGhost")]
        public int IsGhost { get => isGhost; set => isGhost = value; }

        private int female = 0;

        [JsonProperty("Female")]
        public int Female { get => female; set => female = value; }

        private int warmode = 0;

        [JsonProperty("Warmode")]
        public int Warmode { get => warmode; set => warmode = value; }

        private int friend = 0;

        [JsonProperty("Friend")]
        public int Friend { get => friend; set => friend = value; }

        private int paralized = 0;

        [JsonProperty("Paralized")]
        public int Paralized { get => paralized; set => paralized = value; }

        private bool checkIgnoreObject = false;

        [JsonProperty("CheckIgnoreObject")]
        public bool CheckIgnoreObject { get => checkIgnoreObject; set => checkIgnoreObject = value; }

        private List<byte> notorieties = new List<byte>();
        [JsonProperty("Notorieties")]
        public List<byte> Notorieties { get => notorieties; set => notorieties = value; }



        public RazorEnhanced.Mobiles.Filter ToMobileFilter()
        {
            RazorEnhanced.Mobiles.Filter filter = new RazorEnhanced.Mobiles.Filter();
            filter.Enabled = Enabled;
            filter.Serials = Serials;
            filter.Bodies = Bodies;
            filter.Name = Name;
            filter.Hues = Hues;
            filter.RangeMin = RangeMin;
            filter.RangeMax = RangeMax;
            filter.Poisoned = Poisoned;
            filter.IsHuman = IsHuman;
            filter.IsGhost = IsGhost;
            filter.Blessed = Blessed;
            filter.Female = Female;
            filter.Warmode = Warmode;
            filter.Friend = Friend;
            filter.Paralized = Paralized;
            filter.CheckIgnoreObject = CheckIgnoreObject;
            filter.Notorieties = Notorieties;

            return filter;
        }

        internal static void RefreshTargetShortCut(ListBox t)
        {
            t.Items.Clear();
            List<string> shortcutlist = RazorEnhanced.Settings.Target.ReadAllShortCut();
            foreach (string shortcut in shortcutlist)
            {
                t.Items.Add(shortcut);
            }
            if (t.Items.Count > 0)
                t.SelectedIndex = 0;
        }
    }
}
