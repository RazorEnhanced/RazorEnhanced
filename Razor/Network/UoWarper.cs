using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading;

namespace RazorEnhanced
{
	internal class UoWarper
	{
		internal static UO UODLLHandleClass = null;

		public sealed class DLL
		{
			internal static class NativeMethods
			{
				[DllImport("uo.dll")]
				public static extern IntPtr Open();

				[DllImport("uo.dll")]
				public static extern int Version();

				[DllImport("uo.dll")]
				public static extern void Close(IntPtr handle);

				[DllImport("uo.dll")]
				public static extern void Clean(IntPtr handle);

				[DllImport("uo.dll")]
				public static extern int Query(IntPtr handle);

				[DllImport("uo.dll")]
				public static extern int Execute(IntPtr handle);

				[DllImport("uo.dll")]
				public static extern int GetTop(IntPtr handle);

				[DllImport("uo.dll")]
				public static extern int GetType(IntPtr handle, int index);

				[DllImport("uo.dll")]
				public static extern void SetTop(IntPtr handle, int index);

				[DllImport("uo.dll")]
				public static extern void PushStrVal(IntPtr handle, string value);

				[DllImport("uo.dll")]
				public static extern void PushInteger(IntPtr handle, int value);

				[DllImport("uo.dll")]
				public static extern void PushBoolean(IntPtr handle, bool value);

				[DllImport("uo.dll")]
				public static extern void PushPointer(IntPtr handle, IntPtr value);

				[DllImport("uo.dll")]
				public static extern void PushPtrOrNil(IntPtr handle, IntPtr value);

				[DllImport("uo.dll")]
				public static extern void PushValue(IntPtr handle, int index);

				[DllImport("uo.dll")]
				public static extern IntPtr GetString(IntPtr handle, int index);

				[DllImport("uo.dll")]
				public static extern int GetInteger(IntPtr handle, int index);

				[DllImport("uo.dll")]
				public static extern bool GetBoolean(IntPtr handle, int index);

				[DllImport("uo.dll")]
				public static extern IntPtr GetPointer(IntPtr handle, int index);

				[DllImport("uo.dll")]
				public static extern double GetDouble(IntPtr handle, int index);
			}
		}


		private static IntPtr _UOHandle = DLL.NativeMethods.Open();

		internal class UO
		{
			internal bool Open(int clinr = 1)
			{
				_UOHandle = DLL.NativeMethods.Open();
				if (DLL.NativeMethods.Version() != 3)
				{
					return false;
				}
				DLL.NativeMethods.SetTop(_UOHandle, 0);
				DLL.NativeMethods.PushStrVal(_UOHandle, "Set");
				DLL.NativeMethods.PushStrVal(_UOHandle, "CliNr");
				DLL.NativeMethods.PushInteger(_UOHandle, clinr);
				return DLL.NativeMethods.Execute(_UOHandle) == 0;
			}

			internal void Pathfind(int X, int Y, int Z)
			{
				this._executeCommand(false, "Pathfind", new object[]
				{
				X,
				Y,
				Z
				});
			}

			internal List<object> _executeCommand(bool ReturnResults, string CommandName, object[] args)
			{
				List<object> Results = new List<object>();
				DLL.NativeMethods.SetTop(_UOHandle, 0);
				DLL.NativeMethods.PushStrVal(_UOHandle, "Call");
				DLL.NativeMethods.PushStrVal(_UOHandle, CommandName);
				for (int j = 0; j < args.Length; j++)
				{
					object o = args[j];
					if (o is int)
					{
						DLL.NativeMethods.PushInteger(_UOHandle, Convert.ToInt32(o));
					}
					else if (o is string)
					{
						DLL.NativeMethods.PushStrVal(_UOHandle, Convert.ToString(o));
					}
					else if (o is bool)
					{
						DLL.NativeMethods.PushBoolean(_UOHandle, Convert.ToBoolean(o));
					}
					else if (o is IntPtr)
					{
						DLL.NativeMethods.PushPointer(_UOHandle, (IntPtr)o);
					}
				}
				if (DLL.NativeMethods.Execute(_UOHandle) != 0)
				{
					return null;
				}
				if (!ReturnResults)
				{
					return null;
				}
				int objectcnt = DLL.NativeMethods.GetTop(_UOHandle);
				for (int i = 1; i <= objectcnt; i++)
				{
					switch (DLL.NativeMethods.GetType(_UOHandle, i))
					{
						case 1:
							Results.Add(DLL.NativeMethods.GetBoolean(_UOHandle, i).ToString());
							break;
						case 2:
							Results.Add(DLL.NativeMethods.GetPointer(_UOHandle, i));
							break;
						case 3:
							Results.Add(DLL.NativeMethods.GetInteger(_UOHandle, i).ToString());
							break;
						case 4:
							Results.Add(Marshal.PtrToStringAnsi(DLL.NativeMethods.GetString(_UOHandle, i)));
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