using Assistant;
using Assistant.UI;
using System;
using System.Collections.Generic;
using System.Media;
using System.Threading;
using System.Collections.Concurrent;
using System.Linq;
using System.Drawing;
using static RazorEnhanced.HotKey;
using System.Windows.Forms;
using System.IO;
using System.Reflection;

namespace RazorEnhanced
{
    /// <summary>
    /// The CUO_Functions class contains invocation of CUO code using reflection
    /// DANGER !!
    /// </summary>
    public class CUO
    {
        /// <summary>
        /// Invokes the LoadMarkers function inside the CUO code
        /// Map must be open for this to work
        /// </summary>
        public static void LoadMarkers()
        {
            if (!Client.IsOSI)
            {
                // WorldMapGump worldMap = UIManager.GetGump<WorldMapGump>();
                var getAllGumps = ClassicUOClient.CUOAssembly?.GetType("ClassicUO.Game.Managers.UIManager")?.GetProperty("Gumps", BindingFlags.Public | BindingFlags.Static);
                if (getAllGumps != null)
                {
                    var listOfGumps = getAllGumps.GetValue(null);
                    if (listOfGumps != null)
                    {
                        IEnumerable<Object> temp = listOfGumps as IEnumerable<Object>;
                        foreach (var gump in temp)
                        {
                            if (gump != null)
                            {
                                var GumpType = ClassicUOClient.CUOAssembly?.GetType("ClassicUO.Game.UI.Gumps.Gump")?.GetProperty("GumpType", BindingFlags.Public | BindingFlags.Instance);
                                if (GumpType != null)
                                {
                                    int GumpTypeEnum = (int)GumpType.GetValue(gump);
                                    if (GumpTypeEnum == 18)
                                    {
                                        var WorldMapGump = ClassicUOClient.CUOAssembly?.GetType("ClassicUO.Game.UI.Gumps.WorldMapGump");
                                        if (WorldMapGump != null)
                                        {
                                            var LoadMarkers = WorldMapGump?.GetMethod("LoadMarkers", BindingFlags.Instance | BindingFlags.NonPublic);
                                            if (LoadMarkers != null)
                                            {
                                                LoadMarkers.Invoke(gump, null);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        //PropertyInfo ProfileClass = ClassicUOClient.CUOAssembly?.GetType("ClassicUO.Configuration.Profile")?.GetProperty("AutoOpenDoors", BindingFlags.Public | BindingFlags.Instance);
                        //if (ProfileClass != null)
                        //{
                        //    ProfileClass.SetValue(profile, true, null);
                        //}
                    }
                }
            }
        }


        /// <summary>
        /// Invokes the GoToMarker function inside the CUO code
        /// Map must be open for this to work
        /// </summary>
        public static void GoToMarker(int x, int y)
        {
            if (!Client.IsOSI)
            {
                // WorldMapGump worldMap = UIManager.GetGump<WorldMapGump>();
                var getAllGumps = ClassicUOClient.CUOAssembly?.GetType("ClassicUO.Game.Managers.UIManager")?.GetProperty("Gumps", BindingFlags.Public | BindingFlags.Static);
                if (getAllGumps != null)
                {
                    var listOfGumps = getAllGumps.GetValue(null);
                    if (listOfGumps != null)
                    {
                        IEnumerable<Object> temp = listOfGumps as IEnumerable<Object>;
                        foreach (var gump in temp)
                        {
                            if (gump != null)
                            {
                                var GumpType = ClassicUOClient.CUOAssembly?.GetType("ClassicUO.Game.UI.Gumps.Gump")?.GetProperty("GumpType", BindingFlags.Public | BindingFlags.Instance);
                                if (GumpType != null)
                                {
                                    int GumpTypeEnum = (int)GumpType.GetValue(gump);
                                    if (GumpTypeEnum == 18)
                                    {
                                        var WorldMapGump = ClassicUOClient.CUOAssembly?.GetType("ClassicUO.Game.UI.Gumps.WorldMapGump");
                                        if (WorldMapGump != null)
                                        {
                                            var flags = BindingFlags.Default; //BindingFlags.Instance | BindingFlags.Public;
                                            foreach (var method in WorldMapGump.GetMethods())
                                            {
                                               if (method.Name == "GoToMarker")
                                                {
                                                    if (method.IsPublic)
                                                        flags |= BindingFlags.Public;
                                                    else
                                                        flags |= BindingFlags.NonPublic;
                                                    if (method.IsStatic)
                                                        flags |= BindingFlags.Static;
                                                    else
                                                        flags |= BindingFlags.Instance;
                                                }

                                            }
                                            var GoToMarker = WorldMapGump?.GetMethod("GoToMarker", BindingFlags.Instance | BindingFlags.Public);
                                            if (GoToMarker != null)
                                            {
                                                var parameters = new object[3] { x, y, true };
                                                GoToMarker.Invoke(gump, parameters);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
        /// <summary>
        /// Invokes the FreeView function inside the CUO code
        /// First value is retrieved, and then only set if its not correct
        /// </summary>
        public static void FreeView(bool free )
        {
            if (!Client.IsOSI)
            {
                // WorldMapGump worldMap = UIManager.GetGump<WorldMapGump>();
                var getAllGumps = ClassicUOClient.CUOAssembly?.GetType("ClassicUO.Game.Managers.UIManager")?.GetProperty("Gumps", BindingFlags.Public | BindingFlags.Static);
                if (getAllGumps != null)
                {
                    var listOfGumps = getAllGumps.GetValue(null);
                    if (listOfGumps != null)
                    {
                        IEnumerable<Object> temp = listOfGumps as IEnumerable<Object>;
                        foreach (var gump in temp)
                        {
                            if (gump != null)
                            {
                                var GumpType = ClassicUOClient.CUOAssembly?.GetType("ClassicUO.Game.UI.Gumps.Gump")?.GetProperty("GumpType", BindingFlags.Public | BindingFlags.Instance);
                                if (GumpType != null)
                                {
                                    int GumpTypeEnum = (int)GumpType.GetValue(gump);
                                    if (GumpTypeEnum == 18)
                                    {
                                        var WorldMapGump = ClassicUOClient.CUOAssembly?.GetType("ClassicUO.Game.UI.Gumps.WorldMapGump");
                                        if (WorldMapGump != null)
                                        {
                                            System.Reflection.PropertyInfo property = null;
                                            foreach (var propSearch in WorldMapGump.GetProperties())
                                            {
                                                if (propSearch.Name == "FreeView")
                                                {
                                                    property = propSearch;
                                                    break;
                                                }
                                            }
                                            if (property != null)
                                            {
                                                bool curr = (bool)property.GetValue(gump);
                                                if (curr != free)
                                                    property.SetValue(gump, free, null);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        //PropertyInfo ProfileClass = ClassicUOClient.CUOAssembly?.GetType("ClassicUO.Configuration.Profile")?.GetProperty("AutoOpenDoors", BindingFlags.Public | BindingFlags.Instance);
                        //if (ProfileClass != null)
                        //{
                        //    ProfileClass.SetValue(profile, true, null);
                        //}
                    }
                }
            }
        }




        /// <summary>
        /// Set a bool Config property in CUO by name
        /// </summary>
        public static void ProfilePropertySet(string propertyName, bool enable)
        {
            if (!Client.IsOSI)
            {
                var currentProfileProperty = ClassicUOClient.CUOAssembly?.GetType("ClassicUO.Configuration.ProfileManager")?.GetProperty("CurrentProfile", BindingFlags.Public | BindingFlags.Static);
                if (currentProfileProperty != null)
                {
                    var profile = currentProfileProperty.GetValue(null);
                    if (profile != null)
                    {
                        var profileClass = ClassicUOClient.CUOAssembly?.GetType("ClassicUO.Configuration.Profile");
                        System.Reflection.PropertyInfo property = null;
                        foreach (var propSearch in profileClass.GetProperties())
                        {
                            if (propSearch.Name == propertyName)
                            {
                                property = propSearch;
                                break;
                            }
                        }
                        if (property != null)
                        {
                            property.SetValue(profile, enable);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Set a int Config property in CUO by name
        /// </summary>
        public static void ProfilePropertySet(string propertyName, int value)
        {
            if (!Client.IsOSI)
            {
                var currentProfileProperty = ClassicUOClient.CUOAssembly?.GetType("ClassicUO.Configuration.ProfileManager")?.GetProperty("CurrentProfile", BindingFlags.Public | BindingFlags.Static);
                if (currentProfileProperty != null)
                {
                    var profile = currentProfileProperty.GetValue(null);
                    if (profile != null)
                    {
                        var profileClass = ClassicUOClient.CUOAssembly?.GetType("ClassicUO.Configuration.Profile");
                        System.Reflection.PropertyInfo property = null;
                        foreach (var propSearch in profileClass.GetProperties())
                        {
                            if (propSearch.Name == propertyName)
                            {
                                property = propSearch;
                                break;
                            }
                        }
                        if (property != null)
                        {
                            property.SetValue(profile, value);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Set a string Config property in CUO by name
        /// </summary>
        public static void ProfilePropertySet(string propertyName, string value)
        {
            if (!Client.IsOSI)
            {
                var currentProfileProperty = ClassicUOClient.CUOAssembly?.GetType("ClassicUO.Configuration.ProfileManager")?.GetProperty("CurrentProfile", BindingFlags.Public | BindingFlags.Static);
                if (currentProfileProperty != null)
                {
                    var profile = currentProfileProperty.GetValue(null);
                    if (profile != null)
                    {
                        var profileClass = ClassicUOClient.CUOAssembly?.GetType("ClassicUO.Configuration.Profile");
                        System.Reflection.PropertyInfo property = null;
                        foreach (var propSearch in profileClass.GetProperties())
                        {
                            if (propSearch.Name == propertyName)
                            {
                                property = propSearch;
                                break;
                            }
                        }
                        if (property != null)
                        {
                            property.SetValue(profile, value);
                        }
                    }
                }
            }
        }

    }

}

