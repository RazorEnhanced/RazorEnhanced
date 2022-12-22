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

using System;
using System.Collections.Generic;
using System.Linq;
using RazorEnhanced.Helpers;
using RazorEnhanced.Helpers2;
using RazorEnhanced.Reflection.Objects;


namespace RazorEnhanced.Reflection
{
    public static class Macros
    {
        public static void CreateMacroButton( string macroEntry )
        {
           
                GameScene gameScene = new GameScene();

                IEnumerable<Macro> allMacros = gameScene.Macros.GetAllMacros();

                Macro macroObj = allMacros.FirstOrDefault( e => e.Name == macroEntry );

                if ( macroObj == null )
                {
                    macroObj = new Macro( macroEntry );

                    gameScene.Macros.PushToBack( macroObj );
                }

                macroObj.Items = new MacroObjectString( macroEntry );

                Gumps.MacroButtonGump macroButton = new Gumps.MacroButtonGump( macroObj, 200, 200 );

                UIManager.Add( macroButton );
           
        }      
    }
}