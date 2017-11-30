using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace RazorEnhanced
{
	internal class UoNet
	{
		internal static UO UOHandler;

		internal sealed class UODLL
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
			public static extern void PushBoolean(IntPtr handle, Boolean value);
			[DllImport("uo.dll")]
			public static extern String GetString(IntPtr handle, int index);
			[DllImport("uo.dll")]
			public static extern int GetInteger(IntPtr handle, int index);
			[DllImport("uo.dll")]
			public static extern bool GetBoolean(IntPtr handle, int index);
		}

		internal class UO
		{
			internal static IntPtr UOHandle;
			internal static object Lock_Item;

			private static bool GetBoolean(string command)
			{
				lock (Lock_Item)
				{
					UODLL.SetTop(UOHandle, 0);
					UODLL.PushStrVal(UOHandle, "Get");
					UODLL.PushStrVal(UOHandle, command);
					int result = UODLL.Execute(UOHandle);
					if (result == 0)
					{
						return UODLL.GetBoolean(UOHandle, 1);
					}
					else
					{
						return false;
					}
				}
			}

			private void SetBoolean(string command, bool value)
			{
				lock (Lock_Item)
				{
					UODLL.SetTop(UOHandle, 0);
					UODLL.PushStrVal(UOHandle, "Set");
					UODLL.PushStrVal(UOHandle, command);
					UODLL.PushBoolean(UOHandle, value);
					UODLL.Execute(UOHandle);
				}
			}

			private static int GetInt(string command)
			{
				lock (Lock_Item)
				{
					UODLL.SetTop(UOHandle, 0);
					UODLL.PushStrVal(UOHandle, "Get");
					UODLL.PushStrVal(UOHandle, command);
					int result = UODLL.Execute(UOHandle);
					if (result == 0)
					{
						return UODLL.GetInteger(UOHandle, 1);
					}
					else
					{
						return 0;
					}
				}
			}

			private static void SetInt(string command, int value)
			{
				lock (Lock_Item)
				{
					UODLL.SetTop(UOHandle, 0);
					UODLL.PushStrVal(UOHandle, "Set");
					UODLL.PushStrVal(UOHandle, command);
					UODLL.PushInteger(UOHandle, value);
					UODLL.Execute(UOHandle);
				}
			}

			private static string GetString(string command)
			{
				lock (Lock_Item)
				{
					UODLL.SetTop(UOHandle, 0);
					UODLL.PushStrVal(UOHandle, "Get");
					UODLL.PushStrVal(UOHandle, command);
					int result = UODLL.Execute(UOHandle);
					if (result == 0)
					{
						return UODLL.GetString(UOHandle, 1);
					}
					else
					{
						return null;
					}
				}
			}

			private void SetString(string command, string value)
			{
				lock (Lock_Item)
				{
					UODLL.SetTop(UOHandle, 0);
					UODLL.PushStrVal(UOHandle, "Set");
					UODLL.PushStrVal(UOHandle, command);
					UODLL.PushStrVal(UOHandle, value);
					UODLL.Execute(UOHandle);
				}
			}


			internal static void Close()
			{
				try
				{
					lock (Lock_Item)
					{
						UODLL.Clean(UOHandle);
						UODLL.Close(UOHandle);
					}

				}
				catch { }
			}

			internal static bool Open()
			{

				try
				{
					UOHandle = UODLL.Open();
					int ver = UODLL.Version();
					if (ver != 3)
					{
						return false;
					}
					UODLL.SetTop(UOHandle, 0);
					UODLL.PushStrVal(UOHandle, "Set");
					UODLL.PushStrVal(UOHandle, "CliNr");
					UODLL.PushInteger(UOHandle, 1);
					if (UODLL.Execute(UOHandle) != 0)
					{
						return false;
					}

					Lock_Item = new object();
					return true;
				}
				catch
				{
					return false;
				}
			}

			internal void PathFind(int X, int Y, int Z)
			{
				lock (Lock_Item)
				{
					_executeCommand(false, "Pathfind", new object[] { X, Y, Z });
				}
			}

			private static List<object> _executeCommand(bool ReturnResults, string CommandName, object[] args)
			{
				lock (Lock_Item)
				{
					// Maybe return bool and results as an Out?
					UODLL.SetTop(UOHandle, 0);
					UODLL.PushStrVal(UOHandle, "Call");
					UODLL.PushStrVal(UOHandle, CommandName);
					foreach (object o in args)
					{
						if (o is Int32)
						{
							// (o.GetType() == typeof(int))
							UODLL.PushInteger(UOHandle, Convert.ToInt32(o));
						}
						else if (o is string)
						{
							UODLL.PushStrVal(UOHandle, (string)o);
						}
						else if (o is bool)
						{
							UODLL.PushBoolean(UOHandle, Convert.ToBoolean(o));
						}
					}
					if (UODLL.Execute(UOHandle) != 0)
					{
						return null;
					}
					if (!ReturnResults)
					{
						return null;
					}
					int objectcnt = UODLL.GetTop(UOHandle);
					List<object> Results = new List<object>();
					for (int i = 1; i <= objectcnt; i++)
					{
						int _gettype = UODLL.GetType(UOHandle, i);
						switch (_gettype)
						{
							case 1:
								Results.Add(UODLL.GetBoolean(UOHandle, i).ToString());
								break;
							case 3:
								Results.Add(UODLL.GetInteger(UOHandle, i).ToString());
								break;
							case 4:
								Results.Add(UODLL.GetString(UOHandle, i));
								break;
							default:
								return null;
						}
					}
					return Results;
				}
			}
		}
	}
}