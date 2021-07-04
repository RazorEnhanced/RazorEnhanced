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
			PM_INSTALL = Client.WM_USER + 666,
			PM_INFO,
			PM_ENABLE,
			PM_DISABLE,
			PM_VIEW_RANGE_VALUE
		};

		private static IntPtr m_modhandle = IntPtr.Zero;
		private static bool m_soundpatch = false;
		private static bool m_fpspatch = false;
		private static bool m_paperdollpatch = false;
		private static bool m_viewrangepatch = false;

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

	//		if (Engine.ClientBuild > 49)
	//			return;

			String path = AppDomain.CurrentDomain.BaseDirectory + "\\UOMod.dll";

			IntPtr hp = DLLImport.Win.OpenProcess(PROCESS_CREATE_THREAD | PROCESS_VM_OPERATION | PROCESS_VM_WRITE | PROCESS_QUERY_INFORMATION | PROCESS_VM_READ, true, DLLImport.Razor.GetUOProcId());

			if (hp != IntPtr.Zero)
			{
				IntPtr hProcess = IntPtr.Zero;
				IntPtr hThread = IntPtr.Zero;
				IntPtr pszLibFileRemote = IntPtr.Zero;
				try
				{
					hProcess = DLLImport.Win.OpenProcess(
						PROCESS_QUERY_INFORMATION | // Required by Alpha
						PROCESS_CREATE_THREAD | // For CreateRemoteThread
						PROCESS_VM_OPERATION | // For VirtualAllocEx/VirtualFreeEx
						PROCESS_VM_WRITE | // For WriteProcessMemory
						PROCESS_VM_READ,
						false, DLLImport.Razor.GetUOProcId());

					if (hProcess == IntPtr.Zero)
						return;

					int cch = 1 + DLLImport.Win.lstrlen(path);
					int cb = cch * sizeof(char);

					pszLibFileRemote = DLLImport.Win.VirtualAllocEx(hProcess, IntPtr.Zero, (uint) cb, MEM_COMMIT, PAGE_READWRITE);
					if (pszLibFileRemote == IntPtr.Zero)
						return;

					UIntPtr bytesWritten;
					if (!DLLImport.Win.WriteProcessMemory(hProcess, pszLibFileRemote, Encoding.Default.GetBytes(path), (uint)((path.Length + 1) * Marshal.SizeOf(typeof(char))), out bytesWritten))
						return;

					IntPtr pfnThreadRtn = DLLImport.Win.GetProcAddress(DLLImport.Win.GetModuleHandle("Kernel32"), "LoadLibraryA");
					if (pfnThreadRtn == IntPtr.Zero)
						return;

					hThread = DLLImport.Win.CreateRemoteThread(hProcess, IntPtr.Zero, 0, pfnThreadRtn, pszLibFileRemote, 0, IntPtr.Zero);
					if (hThread == IntPtr.Zero)
						return;

					DLLImport.Win.WaitForSingleObject(hThread, int.MaxValue);
				}
				finally
				{
					if (pszLibFileRemote != IntPtr.Zero)
						DLLImport.Win.VirtualFreeEx(hProcess, pszLibFileRemote, 0, DLLImport.Win.FreeType.MEM_RELEASE);

					if (hThread != IntPtr.Zero)
						DLLImport.Win.CloseHandle(hThread);

					if (hProcess != IntPtr.Zero)
						DLLImport.Win.CloseHandle(hProcess);
				}
			}

			// Thread attesa che la windowhandle sia disponibile
			new Thread(() =>
			{
				Thread.CurrentThread.IsBackground = true;
				Thread.Sleep(1500);
				m_modhandle = DLLImport.Win.FindWindow("UOModWindow_" + Assistant.Client.Instance.GetWindowHandle().ToString("x8").ToUpper(), null);

				if (m_modhandle != IntPtr.Zero)
				{
					DLLImport.Win.SendMessage(m_modhandle, (int)PATCH_MESSAGES.PM_VIEW_RANGE_VALUE, 0, 0);
					DLLImport.Win.SendMessage(m_modhandle, (int)PATCH_MESSAGES.PM_INFO, 0, 0xFFFFFFFF);

				//	SendMessage(dllWindow, PM_VIEW_RANGE_VALUE, (DWORD)Handle, 0);
				//	SendMessage(dllWindow, PM_INFO, (DWORD)Handle, 0xFFFFFFFF);
					EnableOnStartMod();
				}

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
					DLLImport.Win.SendMessage(m_modhandle, m_enable, 0, (int)PATCH_TYPE.PT_FPS);
					m_fpspatch = enable;
					break;
                case (int)PATCH_TYPE.PT_PAPERDOLL_SLOTS:
					DLLImport.Win.SendMessage(m_modhandle, m_enable, 0, (int)PATCH_TYPE.PT_PAPERDOLL_SLOTS);
					m_paperdollpatch = enable;
					break;
				case (int)PATCH_TYPE.PT_GLOBAL_SOUND:
					DLLImport.Win.SendMessage(m_modhandle, m_enable, 0, (int)PATCH_TYPE.PT_GLOBAL_SOUND);
					m_soundpatch = enable;
					break;
				default:
					break;
			}
		}

		private static void EnableOnStartMod()
		{
			if (Engine.ClientMajor < 7)
				return;

			if (m_modhandle == IntPtr.Zero)
				return;

			if (RazorEnhanced.Settings.General.ReadBool("ForceSizeEnabled") && Engine.ClientBuild < 49 && Assistant.Engine.IP != Engine.Resolve("37.143.10.137"))
            {
				DLLImport.Win.SendMessage(m_modhandle, (int)PATCH_MESSAGES.PM_VIEW_RANGE_VALUE, 0, 30);
				DLLImport.Win.SendMessage(m_modhandle, (int)PATCH_MESSAGES.PM_ENABLE, 0, (int)PATCH_TYPE.PT_VIEW_RANGE);
			 	Assistant.Client.Instance.SendToClient(new SetUpdateRange(31));
				m_viewrangepatch = true;
			}

			if (RazorEnhanced.Settings.General.ReadBool("UoModFPS"))
			{
				DLLImport.Win.SendMessage(m_modhandle, (int)PATCH_MESSAGES.PM_ENABLE, 0, (int)PATCH_TYPE.PT_FPS);
				m_fpspatch = true;
            }

			if (RazorEnhanced.Settings.General.ReadBool("UoModPaperdoll"))
			{
				DLLImport.Win.SendMessage(m_modhandle, (int)PATCH_MESSAGES.PM_ENABLE, 0, (int)PATCH_TYPE.PT_PAPERDOLL_SLOTS);
				m_paperdollpatch = true;
			}

			if (RazorEnhanced.Settings.General.ReadBool("UoModSound"))
			{
				DLLImport.Win.SendMessage(m_modhandle, (int)PATCH_MESSAGES.PM_ENABLE, 0, (int)PATCH_TYPE.PT_GLOBAL_SOUND);
				m_soundpatch = true;
			}
		}

		internal static void ProfileChange()
		{
			if (Engine.ClientMajor < 7)
				return;

			//if (Engine.ClientBuild > 49)
			//	return;

			if (m_modhandle == IntPtr.Zero)
				return;

			// ViewRange
			if (Engine.ClientBuild < 49 && Assistant.Engine.IP != Engine.Resolve("37.143.10.137"))
			{
				if (RazorEnhanced.Settings.General.ReadBool("ForceSizeEnabled"))
				{
					if (!m_viewrangepatch)
					{
						DLLImport.Win.SendMessage(m_modhandle, (int)PATCH_MESSAGES.PM_VIEW_RANGE_VALUE, 0, 30);
						DLLImport.Win.SendMessage(m_modhandle, (int)PATCH_MESSAGES.PM_ENABLE, 0, (int)PATCH_TYPE.PT_VIEW_RANGE);
					 	Assistant.Client.Instance.SendToClient(new SetUpdateRange(31));
						m_viewrangepatch = true;
					}
				}
				else
				{
					if (m_viewrangepatch)
					{
						DLLImport.Win.SendMessage(m_modhandle, (int)PATCH_MESSAGES.PM_DISABLE, 0, (int)PATCH_TYPE.PT_VIEW_RANGE);
						m_viewrangepatch = false;
					}
				}
			}

			// FPS
			if (RazorEnhanced.Settings.General.ReadBool("UoModFPS"))
			{
				if (!m_fpspatch)
				{
					DLLImport.Win.SendMessage(m_modhandle, (int)PATCH_MESSAGES.PM_ENABLE, 0, (int)PATCH_TYPE.PT_FPS);
					m_fpspatch = true;
				}
			}
			else
			{
				if (m_fpspatch)
				{
					DLLImport.Win.SendMessage(m_modhandle, (int)PATCH_MESSAGES.PM_DISABLE, 0, (int)PATCH_TYPE.PT_FPS);
					m_fpspatch = false;
				}
			}

			// Paperdoll
			if (RazorEnhanced.Settings.General.ReadBool("UoModPaperdoll"))
			{
				if (!m_paperdollpatch)
				{
					DLLImport.Win.SendMessage(m_modhandle, (int)PATCH_MESSAGES.PM_ENABLE, 0, (int)PATCH_TYPE.PT_PAPERDOLL_SLOTS);
					m_paperdollpatch = true;
				}
			}
			else
			{
				if (m_paperdollpatch)
				{
					DLLImport.Win.SendMessage(m_modhandle, (int)PATCH_MESSAGES.PM_DISABLE, 0, (int)PATCH_TYPE.PT_PAPERDOLL_SLOTS);
					m_paperdollpatch = false;
				}
			}

			//Global Sound
			if (RazorEnhanced.Settings.General.ReadBool("UoModSound"))
			{
				if (!m_soundpatch)
				{
					DLLImport.Win.SendMessage(m_modhandle, (int)PATCH_MESSAGES.PM_ENABLE, 0, (int)PATCH_TYPE.PT_GLOBAL_SOUND);
					m_soundpatch = true;
				}
			}
			else
			{
				if (m_soundpatch)
				{
					DLLImport.Win.SendMessage(m_modhandle, (int)PATCH_MESSAGES.PM_DISABLE, 0, (int)PATCH_TYPE.PT_GLOBAL_SOUND);
					m_soundpatch = false;
				}
			}
		}
	}
}
