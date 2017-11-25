using System;   
using System.Runtime.InteropServices;   
using System.Security;  
         

namespace Assistant
{
	[SuppressUnmanagedCodeSecurityAttribute]
	internal static class SafeNativeMethods
	{
		[System.Runtime.InteropServices.DllImport("Gdi32.dll")]
		internal static extern IntPtr DeleteObject(IntPtr hGdiObj);

		[DllImport("user32.dll")]
		[return: MarshalAs(UnmanagedType.Bool)]
		internal static extern bool GetWindowRect(IntPtr hWnd, out RECT lpRect);

		[StructLayout(LayoutKind.Sequential)]
		public struct RECT
		{
			public int Left;        // x position of upper-left corner
			public int Top;         // y position of upper-left corner
			public int Right;       // x position of lower-right corner
			public int Bottom;      // y position of lower-right corner
		}
		[DllImport("zlib")]
		internal static extern string zlibVersion();

		[DllImport("zlib")]
		internal static extern ZLibError compress(byte[] dest, ref int destLength, byte[] source, int sourceLength);

		[DllImport("zlib")]
		internal static extern ZLibError compress2(byte[] dest, ref int destLength, byte[] source, int sourceLength, ZLibCompressionLevel level);

		[DllImport("zlib")]
		internal static extern ZLibError uncompress(byte[] dest, ref int destLen, byte[] source, int sourceLen);


		[DllImport("User32.dll")]
		internal static extern int SendMessage(IntPtr hWnd, int uMsg, int wParam, int lParam);

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

		[DllImport("Kernel32", EntryPoint = "_lread")]
		internal static extern unsafe int lread(IntPtr hFile, void* lpBuffer, int wBytes);
	}

	[SuppressUnmanagedCodeSecurityAttribute]
	internal static class UnsafeNativeMethods
	{
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

		[DllImport("Crypt.dll")]
		internal static unsafe extern int InitializeLibrary(string version);

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
		internal static unsafe extern void BringToFront(IntPtr hWnd);

		[DllImport("Crypt.dll")]
		internal static unsafe extern void DoFeatures(int features);

		[DllImport("Crypt.dll")]
		internal static unsafe extern bool AllowBit(uint bit);

		[DllImport("Crypt.dll")]
		internal static unsafe extern void SetAllowDisconn(bool allowed);

		[DllImport("Crypt.dll")]
		internal static unsafe extern void SetServer(uint ip, ushort port);

		[DllImport("Crypt.dll")]
		internal static unsafe extern bool HandleNegotiate(ulong word);

		[DllImport("Crypt.dll")]
		internal static unsafe extern IntPtr GetUOVersion();

	}
}