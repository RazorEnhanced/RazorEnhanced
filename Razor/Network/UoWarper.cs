using Assistant;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;

namespace RazorEnhanced
{
    internal class UoWarper
    {
        internal static UO UODLLHandleClass = null;

        private static IntPtr _UOHandle = DLLImport.Uo.Open();

        internal class UO
        {
            internal bool Open(int clinr = 1)
            {
                if (!Client.IsOSI)
                    return false;
                _UOHandle = DLLImport.Uo.Open();
                if (DLLImport.Uo.Version() != 3)
                {
                    return false;
                }
                DLLImport.Uo.SetTop(_UOHandle, 0);
                DLLImport.Uo.PushStrVal(_UOHandle, "Set");
                DLLImport.Uo.PushStrVal(_UOHandle, "CliNr");
                DLLImport.Uo.PushInteger(_UOHandle, clinr);
                return DLLImport.Uo.Execute(_UOHandle) == 0;
            }

            internal void Pathfind(int X, int Y, int Z)
            {
                if (!Client.IsOSI)
                    return;
                this._executeCommand(false, "Pathfind", new object[]
                {
                X,
                Y,
                Z
                });
            }

            internal void OpenPaperdoll()
            {
                if (!Client.IsOSI)
                    return;
                this._executeCommand(false, "Macro", new object[] { 8, 1 });
            }

            internal void EUOWeaponPrimary()
            {
                if (!Client.IsOSI)
                    return;
                this._executeCommand(false, "Macro", new object[] { 35, 0 });
            }

            internal void EUOWeaponSecondary()
            {
                if (!Client.IsOSI)
                    return;
                this._executeCommand(false, "Macro", new object[] { 36, 0 });
            }

            internal void CloseBackpack()
            {
                if (!Client.IsOSI)
                    return;
                this._executeCommand(false, "Macro", new object[] { 9, 7 });
            }

            internal void NextContPos(int X, int Y)
            {
                if (!Client.IsOSI)
                    return;
                DLLImport.Uo.SetTop(_UOHandle, 0);
                DLLImport.Uo.PushStrVal(_UOHandle, "Set");
                DLLImport.Uo.PushStrVal(_UOHandle, "NextCPosX");
                DLLImport.Uo.PushInteger(_UOHandle, X);
                if (DLLImport.Uo.Execute(_UOHandle) != 0)
                {
                    return;
                }

                DLLImport.Uo.SetTop(_UOHandle, 0);
                DLLImport.Uo.PushStrVal(_UOHandle, "Set");
                DLLImport.Uo.PushStrVal(_UOHandle, "NextCPosY");
                DLLImport.Uo.PushInteger(_UOHandle, Y);
                if (DLLImport.Uo.Execute(_UOHandle) != 0)
                {
                    return;
                }

            }

            internal Point GetContPos()
            {
                if (!Client.IsOSI)
                    new Point(-1, -1);
                int x = -1;
                DLLImport.Uo.SetTop(_UOHandle, 0);
                DLLImport.Uo.PushStrVal(_UOHandle, "Get");
                DLLImport.Uo.PushStrVal(_UOHandle, "ContPosX");
                if (DLLImport.Uo.Execute(_UOHandle) == 0)
                {
                    int numRetValues = DLLImport.Uo.GetTop(_UOHandle);
                    if (numRetValues == 1)
                    {
                        x = DLLImport.Uo.GetInteger(_UOHandle, 1);
                    }
                }

                int y = -1;
                DLLImport.Uo.SetTop(_UOHandle, 0);
                DLLImport.Uo.PushStrVal(_UOHandle, "Get");
                DLLImport.Uo.PushStrVal(_UOHandle, "ContPosY");
                if (DLLImport.Uo.Execute(_UOHandle) == 0)
                {
                    int numRetValues = DLLImport.Uo.GetTop(_UOHandle);
                    if (numRetValues == 1)
                    {
                        y = DLLImport.Uo.GetInteger(_UOHandle, 1);
                    }
                }

                return new Point(x, y);

            }

            internal void ToggleAlwaysRun()
            {
                if (!Client.IsOSI)
                    return;
                this._executeCommand(false, "Macro", new object[] { 32, 0 });
            }



            internal List<object> _executeCommand(bool ReturnResults, string CommandName, object[] args)
            {
                List<object> Results = new();
                DLLImport.Uo.SetTop(_UOHandle, 0);
                DLLImport.Uo.PushStrVal(_UOHandle, "Call");
                DLLImport.Uo.PushStrVal(_UOHandle, CommandName);
                for (int j = 0; j < args.Length; j++)
                {
                    object o = args[j];
                    if (o is int)
                    {
                        DLLImport.Uo.PushInteger(_UOHandle, Convert.ToInt32(o));
                    }
                    else if (o is string)
                    {
                        DLLImport.Uo.PushStrVal(_UOHandle, Convert.ToString(o));
                    }
                    else if (o is bool)
                    {
                        DLLImport.Uo.PushBoolean(_UOHandle, Convert.ToBoolean(o));
                    }
                    else if (o is IntPtr)
                    {
                        DLLImport.Uo.PushPointer(_UOHandle, (IntPtr)o);
                    }
                }
                if (DLLImport.Uo.Execute(_UOHandle) != 0)
                {
                    return null;
                }
                if (!ReturnResults)
                {
                    return null;
                }
                int objectcnt = DLLImport.Uo.GetTop(_UOHandle);
                for (int i = 1; i <= objectcnt; i++)
                {
                    switch (DLLImport.Uo.GetType(_UOHandle, i))
                    {
                        case 1:
                            Results.Add(DLLImport.Uo.GetBoolean(_UOHandle, i).ToString());
                            break;
                        case 2:
                            Results.Add(DLLImport.Uo.GetPointer(_UOHandle, i));
                            break;
                        case 3:
                            Results.Add(DLLImport.Uo.GetInteger(_UOHandle, i).ToString());
                            break;
                        case 4:
                            Results.Add(Marshal.PtrToStringAnsi(DLLImport.Uo.GetString(_UOHandle, i)));
                            break;
                        default:
                            throw new NotImplementedException();
                    }
                }
                return Results;
            }
        }
    }
}
