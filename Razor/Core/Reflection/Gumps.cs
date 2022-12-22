#region License

// Copyright (C) 2021 Reetus
// 
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.

#endregion

using System.Collections.Generic;
using System.Linq;
using RazorEnhanced.Helpers;
using RazorEnhanced.Helpers2;
using RazorEnhanced.Reflection.Objects;
using System.Reflection;

namespace RazorEnhanced.Reflection
{
    public static class Gumps
    {
        private static readonly dynamic _gumps;

        static Gumps()
        {
            _gumps = Helpers.Reflection.GetTypePropertyValue<dynamic>("ClassicUO.Game.Managers.UIManager", "Gumps", null);
        }

        public static IEnumerable<dynamic> GetGumps()
        {
            return ((IEnumerable<dynamic>)_gumps)?.ToList();
        }

        internal class MessageBoxGump : ReflectionObject
        {
            public MessageBoxGump(object sealedObject) : base(sealedObject)
            {
            }

            public List<dynamic> Children => WrapProperty<List<dynamic>>();
        }

        public class MacroButtonGump : ReflectionObject
        {
            private const string MACRO_BUTTON_GUMP_TYPE = "ClassicUO.Game.UI.Gumps.MacroButtonGump";

            public MacroButtonGump(Macro macro, int x, int y) : base(null)
            {
                AssociatedObject =
                    Helpers.Reflection.CreateInstanceOfType(MACRO_BUTTON_GUMP_TYPE, null, macro.AssociatedObject, x, y);

                CreateMemberCache();
            }
        }





    }
}