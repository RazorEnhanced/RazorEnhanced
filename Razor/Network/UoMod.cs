using System;
using System.Threading;
using System.Windows.Forms;
using System.Text;
using System.Runtime.InteropServices;

namespace Assistant
{
	internal class UoMod
	{
		[DllImport("User32.dll")]
		static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);

		[DllImport("kernel32.dll", CharSet = CharSet.Auto)]
		static extern int lstrlen(string lpString);

		[DllImport("kernel32", SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		static extern bool WriteProcessMemory(
			IntPtr hProcess,
			IntPtr baseAddress,
			string bufferToWrite,
			uint numBytesToWrite,
			out uint numBytesWritten
		);

		[DllImport("kernel32.dll")]
		public static extern IntPtr OpenProcess(int dwDesiredAccess, bool bInheritHandle, int dwProcessId);

		[DllImport("kernel32.dll", CharSet = CharSet.Auto)]
		public static extern IntPtr GetModuleHandle(string lpModuleName);

		[DllImport("kernel32", CharSet = CharSet.Ansi, ExactSpelling = true, SetLastError = true)]
		static extern IntPtr GetProcAddress(IntPtr hModule, string procName);

		[DllImport("kernel32.dll", SetLastError = true, ExactSpelling = true)]
		static extern IntPtr VirtualAllocEx(IntPtr hProcess, IntPtr lpAddress,
			uint dwSize, uint flAllocationType, uint flProtect);

		[DllImport("kernel32.dll")]
		static extern IntPtr CreateRemoteThread(IntPtr hProcess,
			IntPtr lpThreadAttributes, uint dwStackSize, IntPtr lpStartAddress, IntPtr lpParameter, uint dwCreationFlags,
			IntPtr lpThreadId);

		[DllImport("user32.dll", SetLastError = true)]
		static extern IntPtr FindWindow(string lpClassName, [MarshalAs(UnmanagedType.LPWStr)]string lpWindowName);


		// privileges
		const int PROCESS_CREATE_THREAD = 0x0002;
		const int PROCESS_QUERY_INFORMATION = 0x0400;
		const int PROCESS_VM_OPERATION = 0x0008;
		const int PROCESS_VM_WRITE = 0x0020;
		const int PROCESS_VM_READ = 0x0010;

		// used for memory allocation
		const uint MEM_COMMIT = 0x00001000;
		const uint MEM_RESERVE = 0x00002000;
		const uint PAGE_READWRITE = 4;

		internal static void InjectUoMod()
		{

			Thread.Sleep(5000);
			IntPtr hwnd = ClientCommunication.ClientProcess.Handle;
			String path = AppDomain.CurrentDomain.BaseDirectory + "UOMod.dll";

			MessageBox.Show(path);
			uint processID = 0;
			GetWindowThreadProcessId(hwnd, out processID);
			IntPtr hp =
				OpenProcess(
					PROCESS_CREATE_THREAD | PROCESS_VM_OPERATION | PROCESS_VM_WRITE | PROCESS_QUERY_INFORMATION | PROCESS_VM_READ, true,
					(int) processID);

			if (hp != IntPtr.Zero)
			{
				IntPtr hProcess = IntPtr.Zero;
				IntPtr hThread = IntPtr.Zero;
				IntPtr pszLibFileRemote = IntPtr.Zero;
				try
				{
					hProcess = OpenProcess(
						PROCESS_QUERY_INFORMATION | // Required by Alpha
						PROCESS_CREATE_THREAD | // For CreateRemoteThread
						PROCESS_VM_OPERATION | // For VirtualAllocEx/VirtualFreeEx
						PROCESS_VM_WRITE | // For WriteProcessMemory
						PROCESS_VM_READ,
						false, (int) processID);

					if (hProcess == IntPtr.Zero)
					{
						MessageBox.Show("Open process failed.");
						return;
					}

					int cch = 1 + lstrlen(path);
					int cb = cch * sizeof(char);

					pszLibFileRemote = VirtualAllocEx(hProcess, IntPtr.Zero, (uint) cb, MEM_COMMIT, PAGE_READWRITE);

					if (pszLibFileRemote == IntPtr.Zero)
					{
						MessageBox.Show("Memory can't be selected.");
						return;
					}

					// Copy the DLL's pathname to the remote process's address space
					byte[] array = Encoding.ASCII.GetBytes(path);
					uint writestatus;
					if (!WriteProcessMemory(hProcess, pszLibFileRemote, path, (uint) cb, out writestatus))
					{

						MessageBox.Show("Failed to write string in to the memory.");
						return;
					}
					

					IntPtr pfnThreadRtn = GetProcAddress(GetModuleHandle("Kernel32"), "LoadLibraryW");

					if (pfnThreadRtn == IntPtr.Zero)
					{

						MessageBox.Show("kernel error");
						return;
					}

					hThread = CreateRemoteThread(hProcess, IntPtr.Zero, 0, pfnThreadRtn, pszLibFileRemote, 0, IntPtr.Zero);

					if (hThread == IntPtr.Zero)
					{

						MessageBox.Show("Remote thread can't be created.");
						return;
					}

				}
				catch (Exception ex)
				{
					MessageBox.Show(ex.ToString());
				}
			}
			Thread.Sleep(500);

			// Qui sta il problema torna nullo
			IntPtr dllWindow = FindWindow("UOModWindow_" + hwnd.ToString("x8").ToUpper(), null);

			MessageBox.Show(dllWindow.ToString());
		}
	}

}