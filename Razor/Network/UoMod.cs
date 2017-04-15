using System;
using System.Threading;
using System.Runtime.InteropServices;
using System.Text;

namespace Assistant
{
	internal class UoMod
	{
		internal enum PATCH_TYPE
		{
			PT_FPS = 1,
			PT_STAMINA,
			PT_ALWAYS_LIGHT,
			PT_PAPERDOLL_SLOTS,
			PT_SPLASH_SCREEN,
			PT_RESOLUTION,
			PT_OPTIONS_NOTIFICATION,
			PT_MULTI_UO,
			PT_NO_CRYPT,
			PT_GLOBAL_SOUND,
			PT_VIEW_RANGE,
			PT_COUNT
		};

		internal enum PATCH_STATE
		{
			PS_DISABLE = 0,
			PS_ENABLE,
			PS_NOT_FOUND
		};

		internal enum PATCH_MESSAGES
		{
			PM_INSTALL = ClientCommunication.WM_USER + 666,
			PM_INFO,
			PM_ENABLE,
			PM_DISABLE,
			PM_VIEW_RANGE_VALUE
		};

		private static IntPtr m_modhandle = IntPtr.Zero;
		private static bool m_soundpatch = false;
		private static bool m_fpspatch = false;
		private static bool m_paperdoolpatch = false;
		private static bool m_viewrangepatch = false;

		[DllImport("User32.dll")]
		public static extern int SendMessage(IntPtr hWnd, int uMsg, int wParam, int lParam);

		[DllImport("kernel32.dll", SetLastError = true)]
		static extern UInt32 WaitForSingleObject(IntPtr hHandle, UInt32 dwMilliseconds);

		[DllImport("User32.dll", SetLastError = true)]
		static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);

		[DllImport("kernel32.dll", SetLastError = true)]
		static extern bool CloseHandle(IntPtr hHandle);

		[DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		static extern int lstrlen(string lpString);

		[DllImport("kernel32.dll", SetLastError = true)]
		static extern bool WriteProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, byte[] lpBuffer, uint nSize, out UIntPtr lpNumberOfBytesWritten);

		[Flags]
		public enum FreeType
		{
			MEM_DECOMMIT = 0x4000,
			MEM_RELEASE = 0x8000,
		}

		[DllImport("kernel32.dll", SetLastError = true, ExactSpelling = true)]
		static extern bool VirtualFreeEx(IntPtr hProcess, IntPtr lpAddress, int dwSize, FreeType dwFreeType);


		[DllImport("kernel32.dll", SetLastError = true)]
		public static extern IntPtr OpenProcess(
		  int dwDesiredAccess,
		  bool bInheritHandle,
		  int dwProcessId
		);

		[DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		public static extern IntPtr GetModuleHandle(string lpModuleName);

		[DllImport("kernel32", CharSet = CharSet.Ansi, ExactSpelling = true, SetLastError = true)]
		static extern IntPtr GetProcAddress(IntPtr hModule, string procName);

		[DllImport("kernel32.dll", SetLastError = true, ExactSpelling = true)]
		static extern IntPtr VirtualAllocEx(IntPtr hProcess, IntPtr lpAddress,
			uint dwSize, uint flAllocationType, uint flProtect);

		[DllImport("kernel32.dll", SetLastError = true)]
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
			if (Engine.ClientMajor < 7)
				return;

			String path = AppDomain.CurrentDomain.BaseDirectory + "\\UOMod.dll";

			IntPtr hp = OpenProcess(PROCESS_CREATE_THREAD | PROCESS_VM_OPERATION | PROCESS_VM_WRITE | PROCESS_QUERY_INFORMATION | PROCESS_VM_READ, true, ClientCommunication.GetUOProcId());

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
						false, ClientCommunication.GetUOProcId());

					if (hProcess == IntPtr.Zero)
						return;

					int cch = 1 + lstrlen(path);
					int cb = cch * sizeof(char);

					pszLibFileRemote = VirtualAllocEx(hProcess, IntPtr.Zero, (uint) cb, MEM_COMMIT, PAGE_READWRITE);
					if (pszLibFileRemote == IntPtr.Zero)
						return;

					UIntPtr bytesWritten;
					if (!WriteProcessMemory(hProcess, pszLibFileRemote, Encoding.Default.GetBytes(path), (uint)((path.Length + 1) * Marshal.SizeOf(typeof(char))), out bytesWritten))
						return;

					IntPtr pfnThreadRtn = GetProcAddress(GetModuleHandle("Kernel32"), "LoadLibraryA");
					if (pfnThreadRtn == IntPtr.Zero)
						return;

					hThread = CreateRemoteThread(hProcess, IntPtr.Zero, 0, pfnThreadRtn, pszLibFileRemote, 0, IntPtr.Zero);
					if (hThread == IntPtr.Zero)
						return;

					WaitForSingleObject(hThread, int.MaxValue);
				}
				finally
				{
					if (pszLibFileRemote != IntPtr.Zero)
						VirtualFreeEx(hProcess, pszLibFileRemote, 0, FreeType.MEM_RELEASE);

					if (hThread != IntPtr.Zero)
						CloseHandle(hThread);

					if (hProcess != IntPtr.Zero)
						CloseHandle(hProcess);
				}
			}

			// Thread attesa che la windowhandle sia disponibile
			new Thread(() =>
			{
				Thread.CurrentThread.IsBackground = true;
				Thread.Sleep(1500);
				m_modhandle = FindWindow("UOModWindow_" + ClientCommunication.FindUOWindow().ToString("x8").ToUpper(), null);

				if (m_modhandle != IntPtr.Zero)
					EnableOnStartMod();

            }).Start();
		}

		internal static void EnableDisable(bool enable, int patch)
		{
			if (Engine.ClientMajor < 7)
				return;

			if (m_modhandle == IntPtr.Zero)
				return;

			int m_enable = 0;
			if (enable)
				m_enable = (int)PATCH_MESSAGES.PM_ENABLE;
			else
				m_enable = (int)PATCH_MESSAGES.PM_DISABLE;

			switch (patch)
			{
				case (int)PATCH_TYPE.PT_FPS:
					SendMessage(m_modhandle, m_enable, 0, (int)PATCH_TYPE.PT_FPS);
					m_fpspatch = true;
					break;
                case (int)PATCH_TYPE.PT_PAPERDOLL_SLOTS:
					SendMessage(m_modhandle, m_enable, 0, (int)PATCH_TYPE.PT_PAPERDOLL_SLOTS);
					m_paperdoolpatch = true;
					break;
				case (int)PATCH_TYPE.PT_GLOBAL_SOUND:
					SendMessage(m_modhandle, m_enable, 0, (int)PATCH_TYPE.PT_GLOBAL_SOUND);
					m_soundpatch = true;
					break;
				default:
					break;
			}
		}

		internal static void EnableOnStartMod()
		{
			if (Engine.ClientMajor < 7)
				return;

			if (m_modhandle == IntPtr.Zero)
				return;

			if (RazorEnhanced.Settings.General.ReadBool("ForceSizeEnabled") && Engine.ClientBuild < 49)
			{
				SendMessage(m_modhandle, (int)PATCH_MESSAGES.PM_VIEW_RANGE_VALUE, 0, 30);
				SendMessage(m_modhandle, (int)PATCH_MESSAGES.PM_ENABLE, 0, (int)PATCH_TYPE.PT_VIEW_RANGE);
				ClientCommunication.SendToClient(new SetUpdateRange(31));
				m_viewrangepatch = true;
			}

			if (RazorEnhanced.Settings.General.ReadBool("UoModFPS"))
			{
				SendMessage(m_modhandle, (int)PATCH_MESSAGES.PM_ENABLE, 0, (int)PATCH_TYPE.PT_FPS);
				m_fpspatch = true;
            }

			if (RazorEnhanced.Settings.General.ReadBool("UoModPaperdool"))
			{
				SendMessage(m_modhandle, (int)PATCH_MESSAGES.PM_ENABLE, 0, (int)PATCH_TYPE.PT_PAPERDOLL_SLOTS);
				m_paperdoolpatch = true;
			}

			if (RazorEnhanced.Settings.General.ReadBool("UoModSound"))
			{
				SendMessage(m_modhandle, (int)PATCH_MESSAGES.PM_ENABLE, 0, (int)PATCH_TYPE.PT_GLOBAL_SOUND);
				m_soundpatch = true;
			}

		}

		internal static void DisableAllPatch()
		{
			if (Engine.ClientMajor < 7)
				return;

			if (m_modhandle == IntPtr.Zero)
				return;

			if (m_viewrangepatch)
			{
				SendMessage(m_modhandle, (int)PATCH_MESSAGES.PM_DISABLE, 0, (int)PATCH_TYPE.PT_VIEW_RANGE);
				m_viewrangepatch = false;
            }

			if (m_fpspatch)
			{ 
				SendMessage(m_modhandle, (int)PATCH_MESSAGES.PM_DISABLE, 0, (int)PATCH_TYPE.PT_FPS);
				m_fpspatch = false;
			}

			if (m_paperdoolpatch)
			{
                SendMessage(m_modhandle, (int)PATCH_MESSAGES.PM_DISABLE, 0, (int)PATCH_TYPE.PT_PAPERDOLL_SLOTS);
				m_paperdoolpatch = false;
            }

			if (m_soundpatch)
			{
				SendMessage(m_modhandle, (int)PATCH_MESSAGES.PM_DISABLE, 0, (int)PATCH_TYPE.PT_GLOBAL_SOUND);
				m_soundpatch = false;
			}
		}

	}

}