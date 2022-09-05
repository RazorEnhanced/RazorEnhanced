using System.Collections.Generic;
using Ultima.Data;

internal class ProfessionInfo
{
    internal static readonly int[,] _VoidSkills = new int[4, 2]
    {
            { 0, InitialSkillValue }, { 0, InitialSkillValue },
            { 0, Client.Version < ClientVersion.CV_70160 ? 0 : InitialSkillValue }, { 0, InitialSkillValue }
    };
    internal static readonly int[] _VoidStats = new int[3] { 60, RemainStatValue, RemainStatValue };
    public static int InitialSkillValue => Client.Version >= ClientVersion.CV_70160 ? 30 : 50;
    public static int RemainStatValue => Client.Version >= ClientVersion.CV_70160 ? 15 : 10;
    public string Name { get; set; }
    public string TrueName { get; set; }
    public int Localization { get; set; }
    public int Description { get; set; }
    public int DescriptionIndex { get; set; }
    public Ultima.IO.Resources.ProfessionLoader.PROF_TYPE Type { get; set; }

    public ushort Graphic { get; set; }

    public bool TopLevel { get; set; }
    public int[,] SkillDefVal { get; set; } = _VoidSkills;
    public int[] StatsVal { get; set; } = _VoidStats;
    public List<string> Childrens { get; set; }
}