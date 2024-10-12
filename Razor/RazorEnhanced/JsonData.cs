using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;

namespace JsonData
{

    public class TargetGUI : RazorEnhanced.ListAbleItem
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
            filter.ZLevelMin = mobileFilter.ZLevelMin;
            filter.ZLevelMax = mobileFilter.ZLevelMax;
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
            filter.IgnorePets = mobileFilter.IgnorePets;
            filter.Notorieties = mobileFilter.Notorieties;
            return filter;
        }

        [JsonProperty("Enabled")] public bool Enabled { get; set; } = true;

        [JsonProperty("Serials")] public List<int> Serials { get; set; } = new List<int>();

        [JsonProperty("Bodies")] public List<int> Bodies { get; set; } = new List<int>();

        [JsonProperty("Name")] public string Name { get; set; } = "";

        [JsonProperty("Hues")] public List<int> Hues { get; set; } = new List<int>();

        [JsonProperty("RangeMin")] public double RangeMin { get; set; } = -1;

        [JsonProperty("RangeMax")] public double RangeMax { get; set; } = -1;

        [JsonProperty("ZLevelMin")] public double ZLevelMin { get; set; } = -4096;

        [JsonProperty("ZLevelMax")] public double ZLevelMax { get; set; } = 4096;


        [JsonProperty("CheckLineOfSite")] public bool CheckLineOfSite { get; set; } = false;

        [JsonProperty("Poisoned")] public int Poisoned { get; set; } = 0;

        [JsonProperty("Blessed")] public int Blessed { get; set; } = 0;

        [JsonProperty("IsHuman")] public int IsHuman { get; set; } = 0;

        [JsonProperty("IsGhost")] public int IsGhost { get; set; } = 0;

        [JsonProperty("Female")] public int Female { get; set; } = 0;

        [JsonProperty("Warmode")] public int Warmode { get; set; } = 0;

        [JsonProperty("Friend")] public int Friend { get; set; } = 0;

        [JsonProperty("Paralized")] public int Paralized { get; set; } = 0;

        [JsonProperty("CheckIgnoreObject")] public bool CheckIgnoreObject { get; set; } = false;

        [JsonProperty("IgnorePets")][DefaultValue(false)] public bool IgnorePets { get; set; } = false;

        [JsonProperty("Notorieties")] public List<byte> Notorieties { get; set; } = new List<byte>();



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
            filter.ZLevelMin = ZLevelMin;
            filter.ZLevelMax = ZLevelMax;
            filter.Poisoned = Poisoned;
            filter.IsHuman = IsHuman;
            filter.IsGhost = IsGhost;
            filter.Blessed = Blessed;
            filter.Female = Female;
            filter.Warmode = Warmode;
            filter.Friend = Friend;
            filter.Paralized = Paralized;
            filter.CheckIgnoreObject = CheckIgnoreObject;
            filter.IgnorePets = IgnorePets;
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
