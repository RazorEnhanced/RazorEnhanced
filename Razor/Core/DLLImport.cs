using System;
using System.Text;
using System.Runtime.InteropServices;

namespace Assistant
{
	internal class DLLImport
	{
		internal class Win
		{
			[return: MarshalAs(UnmanagedType.Bool)]
			[DllImport("user32.dll", SetLastError = true)]
			internal static extern bool PostMessage(IntPtr hWnd, int Msg, System.Windows.Forms.Keys wParam, int lParam);

			[DllImport("user32.dll")]
			[return: MarshalAs(UnmanagedType.Bool)]
			internal static extern bool IsIconic(IntPtr hWnd);

			[DllImport("user32.dll")]
			internal static extern IntPtr SetParent(IntPtr child, IntPtr newParent);

			[DllImport("Gdi32.dll")]
			internal static extern IntPtr DeleteObject(IntPtr hGdiObj);

			[DllImport("Kernel32", EntryPoint = "_lread")]
			internal static extern unsafe int lread(IntPtr hFile, void* lpBuffer, int wBytes);

			[DllImport("user32.dll")]
			[return: MarshalAs(UnmanagedType.Bool)]
			internal static extern bool GetWindowRect(IntPtr hWnd, out RECT lpRect);

			[StructLayout(LayoutKind.Sequential)]
			internal struct RECT
			{
				internal int Left;        // x position of upper-left corner
				internal int Top;         // y position of upper-left corner
				internal int Right;       // x position of lower-right corner
				internal int Bottom;      // y position of lower-right corner
			}

			[DllImport("User32.dll")]
			internal static extern int SendMessage(IntPtr hWnd, int uMsg, int wParam, uint lParam);

			[DllImport("kernel32.dll", SetLastError = true)]
			internal static extern UInt32 WaitForSingleObject(IntPtr hHandle, UInt32 dwMilliseconds);

			[DllImport("User32.dll", SetLastError = true)]
			internal static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);

			[DllImport("kernel32.dll", SetLastError = true)]
			internal static extern bool CloseHandle(IntPtr hHandle);

			[DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
			internal static extern int lstrlen(string lpString);

			[DllImport("kernel32.dll", SetLastError = true)]
			internal static extern bool WriteProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, byte[] lpBuffer, uint nSize, out UIntPtr lpNumberOfBytesWritten);

			[Flags]
			internal enum FreeType
			{
				MEM_DECOMMIT = 0x4000,
				MEM_RELEASE = 0x8000,
			}

			[DllImport("kernel32.dll", SetLastError = true, ExactSpelling = true)]
			internal static extern bool VirtualFreeEx(IntPtr hProcess, IntPtr lpAddress, int dwSize, FreeType dwFreeType);


			[DllImport("kernel32.dll", SetLastError = true)]
			internal static extern IntPtr OpenProcess(
			  int dwDesiredAccess,
			  bool bInheritHandle,
			  int dwProcessId
			);

			[DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
			internal static extern IntPtr GetModuleHandle(string lpModuleName);

			[DllImport("kernel32", CharSet = CharSet.Ansi, ExactSpelling = true, SetLastError = true)]
			internal static extern IntPtr GetProcAddress(IntPtr hModule, string procName);

			[DllImport("kernel32.dll", SetLastError = true, ExactSpelling = true)]
			internal static extern IntPtr VirtualAllocEx(IntPtr hProcess, IntPtr lpAddress,
				uint dwSize, uint flAllocationType, uint flProtect);

			[DllImport("kernel32.dll", SetLastError = true)]
			internal static extern IntPtr CreateRemoteThread(IntPtr hProcess,
				IntPtr lpThreadAttributes, uint dwStackSize, IntPtr lpStartAddress, IntPtr lpParameter, uint dwCreationFlags,
				IntPtr lpThreadId);

			[DllImport("user32.dll", SetLastError = true)]
			internal static extern IntPtr FindWindow(string lpClassName, [MarshalAs(UnmanagedType.LPWStr)]string lpWindowName);

			[DllImport("user32.dll")]
			internal static extern bool ShowWindow(IntPtr handle, int flags);

			[DllImport("msvcrt.dll", CallingConvention = CallingConvention.Cdecl)]
			internal static unsafe extern void memcpy(void* to, void* from, int len);

			[DllImport("user32.dll")]
			internal static extern uint PostMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

			[DllImport("user32.dll")]
			internal static extern bool SetForegroundWindow(IntPtr hWnd);

			[DllImport("kernel32.dll")]
			internal static extern ushort GlobalAddAtom(string str);

			[DllImport("kernel32.dll")]
			internal static extern ushort GlobalDeleteAtom(ushort atom);

			[DllImport("kernel32.dll")]
			internal static extern uint GlobalGetAtomName(ushort atom, StringBuilder buff, int bufLen);

			[DllImport("kernel32.dll")]
			internal static extern IntPtr LoadLibrary(string path);

			[DllImport("kernel32.dll")]
			internal static extern bool FreeLibrary(IntPtr hModule);

			[DllImport("Advapi32.dll")]
			internal unsafe static extern int GetUserNameA(StringBuilder buff, int* len);

			[DllImport("User32.dll")]
			internal static extern IntPtr GetSystemMenu(IntPtr wnd, bool reset);

			[DllImport("User32.dll")]
			internal static extern IntPtr EnableMenuItem(IntPtr menu, uint item, uint options);

		}
		internal class Razor
		{
			[DllImport("Loader.dll")]
			internal static unsafe extern uint Load(string exe, string dll, string func, void* dllData, int dataLen, out uint pid);

			[DllImport("Crypt.dll")]
			internal static unsafe extern int InstallLibrary(IntPtr thisWnd, int procid, int features);

			[DllImport("Crypt.dll")]
			internal static unsafe extern void Shutdown(bool closeClient);

			[DllImport("Crypt.dll")]
			internal static unsafe extern IntPtr FindUOWindow();

			[DllImport("Crypt.dll")]
			internal static unsafe extern IntPtr GetSharedAddress();

			[DllImport("Crypt.dll")]
			internal static unsafe extern int GetPacketLength(byte* data, int bufLen);//GetPacketLength( [MarshalAs(UnmanagedType.LPArray, SizeParamIndex=1)] byte[] data, int bufLen );

			[DllImport("Crypt.dll")]
			internal static unsafe extern bool IsDynLength(byte packetId);

			[DllImport("Crypt.dll")]
			internal static unsafe extern int GetUOProcId();

			//[DllImport("Crypt.dll")]
			//internal static unsafe extern int InitializeLibrary(string version);

			[DllImport("Crypt.dll")]
			internal static unsafe extern IntPtr GetCommMutex();

			[DllImport("Crypt.dll")]
			internal static unsafe extern uint TotalIn();

			[DllImport("Crypt.dll")]
			internal static unsafe extern uint TotalOut();

			[DllImport("Crypt.dll")]
			internal static unsafe extern IntPtr CaptureScreen(bool isFullScreen, string msgStr);

			[DllImport("Crypt.dll")]
			internal static unsafe extern void WaitForWindow(int pid);

			[DllImport("Crypt.dll")]
			internal static unsafe extern void SetDataPath(string path);

			[DllImport("Crypt.dll")]
			internal static unsafe extern void SetDeathMsg(string msg);

			[DllImport("Crypt.dll")]
			internal static unsafe extern void CalibratePosition(int x, int y, int z);

			[DllImport("Crypt.dll")]
			internal static unsafe extern bool IsCalibrated();

			[DllImport("Crypt.dll")]
			internal static unsafe extern bool GetPosition(int* x, int* y, int* z);

			[DllImport("Crypt.dll")]
			internal static unsafe extern void DoFeatures(int features, bool oldversion);

			//[DllImport("Crypt.dll")]
			//internal static unsafe extern bool AllowBit(uint bit);

			[DllImport("Crypt.dll")]
			internal static unsafe extern void SetAllowDisconn(bool allowed);

			[DllImport("Crypt.dll")]
			internal static unsafe extern void SetServer(uint ip, ushort port);

			[DllImport("Crypt.dll")]
			internal static unsafe extern bool HandleNegotiate(ulong word);

			[DllImport("Crypt.dll")]
			internal static unsafe extern IntPtr GetUOVersion();
		}
		internal class ZLib
		{
			[DllImport("zlib")]
			internal static extern string zlibVersion();

			[DllImport("zlib")]
			internal static extern ZLibError compress(byte[] dest, ref int destLength, byte[] source, int sourceLength);

			[DllImport("zlib")]
			internal static extern ZLibError compress2(byte[] dest, ref int destLength, byte[] source, int sourceLength, ZLibCompressionLevel level);

			[DllImport("zlib")]
			internal static extern ZLibError uncompress(byte[] dest, ref int destLen, byte[] source, int sourceLen);
		}

		internal class Uo
		{
			[DllImport("uo.dll")]
			internal static extern IntPtr Open();

			[DllImport("uo.dll")]
			internal static extern int Version();

			[DllImport("uo.dll")]
			internal static extern void Close(IntPtr handle);

			[DllImport("uo.dll")]
			internal static extern void Clean(IntPtr handle);

			[DllImport("uo.dll")]
			internal static extern int Query(IntPtr handle);

			[DllImport("uo.dll")]
			internal static extern int Execute(IntPtr handle);

			[DllImport("uo.dll")]
			internal static extern int GetTop(IntPtr handle);

			[DllImport("uo.dll")]
			internal static extern int GetType(IntPtr handle, int index);

			[DllImport("uo.dll")]
			internal static extern void SetTop(IntPtr handle, int index);

			[DllImport("uo.dll")]
			internal static extern void PushStrVal(IntPtr handle, string value);

			[DllImport("uo.dll")]
			internal static extern void PushInteger(IntPtr handle, int value);

			[DllImport("uo.dll")]
			internal static extern void PushBoolean(IntPtr handle, bool value);

			[DllImport("uo.dll")]
			internal static extern void PushPointer(IntPtr handle, IntPtr value);

			[DllImport("uo.dll")]
			internal static extern void PushPtrOrNil(IntPtr handle, IntPtr value);

			[DllImport("uo.dll")]
			internal static extern void PushValue(IntPtr handle, int index);

			[DllImport("uo.dll")]
			internal static extern IntPtr GetString(IntPtr handle, int index);

			[DllImport("uo.dll")]
			internal static extern int GetInteger(IntPtr handle, int index);

			[DllImport("uo.dll")]
			internal static extern bool GetBoolean(IntPtr handle, int index);

			[DllImport("uo.dll")]
			internal static extern IntPtr GetPointer(IntPtr handle, int index);

			[DllImport("uo.dll")]
			internal static extern double GetDouble(IntPtr handle, int index);

        }

	}
}
