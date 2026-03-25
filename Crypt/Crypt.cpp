// Crypt.cpp -- Main DLL implementation for Crypt.dll
//
// Crypt.dll is a Winsock-hooking DLL injected into the Ultima Online client.
//
// Architecture overview:
//   The DLL intercepts all network I/O by patching the client's PE Import
//   Address Table (IAT) to redirect Winsock calls (send / recv / connect /
//   closesocket / select) through our own hook functions.
//
//   Two Windows message hooks (WH_CALLWNDPROCRET and WH_GETMESSAGE) are
//   installed on the UO window thread to intercept window messages.  These are
//   the transport mechanism for IPC commands from Razor: Razor PostMessages a
//   WM_UONETEVENT to the UO window, the hook fires in the UO thread context,
//   and MessageProc() dispatches the command.
//
// Injection sequence:
//   1. Razor injects Crypt.dll into the UO process via CreateRemoteThread or
//      a similar technique.
//   2. DllMain (DLL_PROCESS_ATTACH) records the module handle and timer baseline.
//   3. OnAttach() is called (via CreateRemoteThread or a hook) to:
//      a. Create the named shared-memory block and mutex.
//      b. Run MemFinder to locate the packet table, encryption keys, version
//         string, and various patchable code addresses.
//      c. Apply startup patches (multi-instance bypass, splash skip, etc.).
//   4. Razor calls InstallLibrary(hRazorWnd, uoPid) to:
//      a. Find the UO client window handle.
//      b. Create / re-open the shared memory.
//      c. Install WH_CALLWNDPROCRET and WH_GETMESSAGE hooks.
//      d. Post WM_PROCREADY to the UO window to trigger the in-process part.
//   5. WM_PROCREADY handler (in MessageProc, called from the UO thread) calls
//      PatchMemory() to IAT-hook all five Winsock functions.
//
// Network data flow:
//   Incoming (server -> client):
//     Server sends encrypted bytes -> HookSelect intercepts select(), sees data
//     ready -> calls RecvData() which calls the original recv() -> decrypts
//     (OSIEncryption::DecryptFromServer or LoginEncryption::Decrypt) ->
//     Huffman-decompresses -> writes plaintext into SharedMemory.InRecv ->
//     SendMessages Razor via WM_UONETEVENT RECV.
//     When Razor wants to inject packets back to the client, it fills OutRecv,
//     and HookRecv returns that data (after re-compressing and re-encrypting).
//
//   Outgoing (client -> server):
//     Client calls send() -> HookSend intercepts -> on first call, reads the
//     seed, initialises encryption -> decrypts client data (LoginEncryption or
//     OSIEncryption::DecryptFromClient) -> writes plaintext into InSend ->
//     posts SEND to Razor.  Razor's injected packets are queued in OutSend and
//     flushed by FlushSendData(), which re-encrypts with EncryptForServer()
//     before calling the real send().

#include "StdAfx.h"
#include "Crypt.h"
#include "uo_huffman.h"
#include "PacketInfo.h"
#include "OSIEncryption.h"
#include "LoginEncryption.h"
#include "MemFinder.h"
#include "Obfuscation.h"
#include <iostream>


//*************************************************************************************
//**************************************Varaibles**************************************
//*************************************************************************************
// Windows message hook handles installed on the UO window thread.
HHOOK hWndProcRetHook = NULL;
HHOOK hGetMsgHook = NULL;   // WH_GETMESSAGE hook (intercepts messages as they are retrieved).
HWND hUOWindow = NULL;      // Handle to the UO client main window.
HWND hRazorWnd = NULL;      // Handle to Razor's window (IPC target for PostMessage/SendMessage).
HWND hMapWnd = NULL;        // Handle to the optional map overlay window (for focus tracking).
DWORD UOProcId = 0;         // Process ID of the UO client process.

HANDLE hFileMap = NULL;     // Handle to the named file-mapping object (shared memory).
HMODULE hInstance = NULL;   // This DLL's module handle.
SOCKET CurrentConnection = 0; // The active Winsock socket handle being monitored.
int ConnectedIP = 0;        // The IPv4 address of the server we last connected to.

sockaddr_in CurrentConnectionAddr; // Full sockaddr of the current connection (saved in HookConnect).

HANDLE CommMutex = NULL;    // Named mutex serialising all SharedMemory accesses.

char* tempBuff = NULL;      // Temporary buffer for encrypted outgoing data in FlushSendData().

SharedMemory* pShared = NULL; // Mapped view of the named shared-memory block.

LARGE_INTEGER PerfFreq, Counter; // High-resolution timer used by SmartCPU to cap frame rate.

DWORD DeathMsgAddr = 0xFFFFFFFF; // Address of "You are dead." string inside the client (lazily found).
HWND hUOAWnd = NULL;        // Handle to the helper "UOASSIST-TP-MSG-WND" message window.

// Video resize state: track the desired viewport dimensions and the client's
// internal size variable so we can force a custom resolution.
SIZE DesiredSize = { 0, 0 };        // The resolution Razor wants (0,0 = use default).
SIZE OriginalSize = { 640, 480 };   // The resolution the client had before we changed it.
DWORD ResizeFuncaddr = 0;    // Address of the client's screen resize function.
DWORD ABetterEntrypoint = 0; // Address within the resize function to call directly.

BYTE SavedInstructions[5];   // Original bytes at the jump we patched in BypassResize().

// Original (pre-hook) function pointers for the five Winsock functions we intercept.
// These are the real function addresses as read from the IAT before patching.
unsigned long OldRecv, OldSend, OldConnect, OldCloseSocket, OldSelect, OldCreateFileA;
// The IAT slot addresses (where we wrote the new function pointers).
unsigned long RecvAddress, SendAddress, ConnectAddress, CloseSocketAddress, SelectAddress, CreateFileAAddress;


// Connection / protocol state flags.
bool Seeded = false;         // True after the 4-byte connection seed has been seen in HookSend.
bool FirstRecv = true;       // True before the first recv() call on this connection.
bool FirstSend = true;       // True before the first send() call on this connection.
bool LoginServer = false;    // True when the current connection is to the login server.
bool Active = true;          // Tracks UO window activation state (WM_ACTIVATE).
bool SmartCPU = false;       // If true, cap game loop to ~30 fps via select() timeout.
bool ServerNegotiated = false; // True after the AuthBits handshake succeeded.
bool InGame = false;         // True after the "play character" (0x5D) packet is sent.
bool CopyFailed = true;      // True if OnAttach() could not locate the packet table.
bool Forwarding = false;     // True when a login-to-game-server forward is in progress.
bool Forwarded = false;      // True once the forward handshake has completed.
bool ClientEncrypted = false;// True if client-side encryption is enabled (login + game phase).
bool ServerEncrypted = false;// True if server-side encryption is enabled.
bool DwmAttrState = true;    // Tracks current DWM NCRENDERING_POLICY state.
bool connected = false;      // True while CurrentConnection is an active socket.

// Which UO client variant is running.
enum class CLIENT_TYPE { TWOD = 1, THREED = 2 };
CLIENT_TYPE ClientType = CLIENT_TYPE::TWOD;

BYTE CryptChecksum[16] = { 0,1,2,3,4,5,6,7,8,9,10,11,12,13,14,15, }; // Unused checksum placeholder.

//**************************************OSI Only Stuff*********************************
// The connection seed read from the first 4 bytes the client sends.  Defaults to
// 127.0.0.1 as a safe fallback before the real seed is observed.
DWORD CryptSeed = 0x7f000001;
// Separate encryption instances for each direction (client-side vs server-side)
// to allow independent cipher state when the DLL sits between both endpoints.
OSIEncryption* ClientCrypt = NULL;  // Game-phase cipher for client-facing traffic.
OSIEncryption* ServerCrypt = NULL;  // Game-phase cipher for server-facing traffic.
LoginEncryption* ClientLogin = NULL;// Login-phase cipher for client-facing traffic.
LoginEncryption* ServerLogin = NULL;// Login-phase cipher for server-facing traffic.
//*************************************************************************************

//*************************************************************************************
//**************************************Functions**************************************
//*************************************************************************************
LRESULT CALLBACK WndProcRetHookFunc(int, WPARAM, LPARAM);
LRESULT CALLBACK GetMsgHookFunc(int, WPARAM, LPARAM);

bool HookFunction(const char*, const char*, int, unsigned long, unsigned long*, unsigned long*);
void FlushSendData();

bool CreateSharedMemory();
void CloseSharedMemory();

//Hooks:
int PASCAL HookRecv(SOCKET, char*, int, int);
int PASCAL HookSend(SOCKET, char*, int, int);
int PASCAL HookConnect(SOCKET, const sockaddr*, int);
int PASCAL HookCloseSocket(SOCKET);
int PASCAL HookSelect(int, fd_set*, fd_set*, fd_set*, const struct timeval*);
//HANDLE WINAPI CreateFileAHook( LPCTSTR, DWORD, DWORD, LPSECURITY_ATTRIBUTES, DWORD, DWORD, HANDLE );

typedef int (PASCAL* NetIOFunc)(SOCKET, char*, int, int);
typedef int (PASCAL* ConnFunc)(SOCKET, const sockaddr*, int);
typedef int (PASCAL* CLSFunc)(SOCKET);
typedef int (PASCAL* SelectFunc)(int, fd_set*, fd_set*, fd_set*, const struct timeval*);
typedef HANDLE(WINAPI* CreateFileAFunc)(LPCTSTR, DWORD, DWORD, LPSECURITY_ATTRIBUTES, DWORD, DWORD, HANDLE);
typedef char* (__cdecl* GetUOVersionFunc)();

GetUOVersionFunc NativeGetUOVersion = NULL;

// ---------------------------------------------------------------------------
// DllMain() -- DLL entry point called by the OS loader.
//
// DLL_PROCESS_ATTACH:
//   * Disables per-thread notifications (we don't need them).
//   * Snapshots the high-resolution performance counter so SmartCPU has a
//     baseline for the 30 fps limiter logic in HookSelect().
//
// DLL_PROCESS_DETACH:
//   * If we are being unloaded from the UO process (not just unmapped from
//     another process that loaded us for exports), notify Razor via CLOSE
//     and tear down shared memory.
//   * If hRazorWnd belongs to this process we are being unloaded in the Razor
//     process, not the UO process -- take no network action.
// ---------------------------------------------------------------------------
BOOL APIENTRY DllMain(HANDLE hModule, DWORD dwReason, LPVOID)
{
	DWORD postID, thisID;
	//Log("DllMain");
	hInstance = (HMODULE)hModule;
	switch (dwReason)
	{
	case DLL_PROCESS_ATTACH:
		//Log("Process Attach");
		// Suppress DLL_THREAD_ATTACH / DLL_THREAD_DETACH notifications for efficiency.
		DisableThreadLibraryCalls(hInstance);
		// Record timer frequency and baseline counter for SmartCPU.
		QueryPerformanceFrequency(&PerfFreq);
		QueryPerformanceCounter(&Counter);
		break;

	case DLL_PROCESS_DETACH:
		//Log("Process Detach");
		postID = 0;
		thisID = GetCurrentProcessId();
		// Determine which process owns Razor's window.
		if (IsWindow(hRazorWnd))
			GetWindowThreadProcessId(hRazorWnd, &postID);

		// Only perform cleanup if unloading from the UO process.
		if (thisID == postID || thisID == UOProcId)
		{
			// Notify Razor that the DLL is going away.
			if (IsWindow(hRazorWnd))
				PostMessage(hRazorWnd, WM_UONETEVENT, CLOSE, 0);

			// Post WM_QUIT to the UO window to terminate the client.
			if (IsWindow(hUOWindow))
			{
				PostMessage(hUOWindow, WM_QUIT, 0, 0);
				SetForegroundWindow(hUOWindow);
				SetFocus(hUOWindow);
			}

			// Unmap shared memory, release hooks, delete encryption objects.
			CloseSharedMemory();
		}
		break;

	case DLL_THREAD_ATTACH:
	case DLL_THREAD_DETACH:
		break;
	}

	return TRUE;
}


// ---------------------------------------------------------------------------
// GetSharedAddress() -- exported; returns the pShared pointer to Razor.
// Razor MapViewOfFile()s the same section independently, but this export
// lets C# callers get the pointer without a second mapping if needed.
// ---------------------------------------------------------------------------
DLLFUNCTION void* GetSharedAddress()
{
	//Log("GetSharedAddress Get shared address [0x%x]", pShared);
	return pShared;
}

// ---------------------------------------------------------------------------
// FindUOWindow() -- exported; locate the UO client main window.
//
// Tries both the 2D client class name ("Ultima Online") and the Third Dawn
// class name ("Ultima Online Third Dawn").  Returns the cached hUOWindow if
// it is still a valid window, otherwise does a fresh FindWindow search.
// ---------------------------------------------------------------------------
DLLFUNCTION HWND FindUOWindow(void)
{
	//Log("FindUOWindow");
	if (hUOWindow == NULL || !IsWindow(hUOWindow))
	{
		// Try the 2D client window class first.
		HWND hWnd = FindWindow("Ultima Online", NULL);
		if (hWnd == NULL)
			hWnd = FindWindow("Ultima Online Third Dawn", NULL); // 3D client fallback.
		return hWnd;
	}
	else
	{
		return hUOWindow; // Already have a valid cached window handle.
	}
}

DLLFUNCTION void SetDataPath(const char* path)
{
	//Log("SetDataPath");
	WaitForSingleObject(CommMutex, INFINITE);
	strncpy(pShared->DataPath, path, 256);
	ReleaseMutex(CommMutex);
}

DLLFUNCTION void SetDeathMsg(const char* msg)
{
	//Log("SetDeathMsg");
	WaitForSingleObject(CommMutex, INFINITE);
	strncpy(pShared->DeathMsg, msg, 16);
	ReleaseMutex(CommMutex);
	PostMessage(hUOWindow, WM_UONETEVENT, DEATH_MSG, 0);
}

void PatchDeathMsg()
{
	//Log("PatchDeathMessage");
	if (DeathMsgAddr == 0xFFFFFFFF)
		DeathMsgAddr = MemFinder::Find("You are dead.", 14);

	if (DeathMsgAddr)
	{
		WaitForSingleObject(CommMutex, INFINITE);
		strncpy((char*)DeathMsgAddr, pShared->DeathMsg, 16);
		ReleaseMutex(CommMutex);
	}
}

// ---------------------------------------------------------------------------
// InstallLibrary() -- exported; set up the DLL-to-Razor communication channel.
//
// Called from Razor (NOT from inside the UO process) after the UO client is
// already running.  It:
//   1. Locates the UO window for the given PID.
//   2. Creates / opens the named shared memory block.
//   3. Installs WH_CALLWNDPROCRET and WH_GETMESSAGE hooks on the UO thread.
//      These hooks cause GetMsgHookFunc / WndProcRetHookFunc to run in the
//      UO thread context when the UO window processes messages.
//   4. Creates a small hidden helper window ("UOASSIST-TP-MSG-WND") in Razor's
//      process for receiving UOAssist-compatible messages.
//   5. Posts WM_PROCREADY to the UO window to trigger PatchMemory() (IAT
//      hooking) from inside the UO process.
//
// Parameters:
//   PostWindow -- Razor's HWND; all UONET_MESSAGE events will be sent here.
//   pid        -- UO client process ID; 0 = find any UO window.
//   flags      -- bitfield: 0x04 = allow negotiate, 0x08 = client encrypted,
//                 0x10 = server encrypted.
//
// Returns an IError code (SUCCESS on success).
// ---------------------------------------------------------------------------
DLLFUNCTION int InstallLibrary(HWND PostWindow, DWORD pid, int flags)
{
	DWORD UOTId = 0;

	//Log("Install library...");

	HWND hWnd = NULL;
	if (pid != 0)
	{
		hWnd = FindWindow("Ultima Online", NULL);
		while (hWnd != NULL)
		{
			UOTId = GetWindowThreadProcessId(hWnd, &UOProcId);
			if (UOProcId == pid)
				break;
			hWnd = FindWindowEx(NULL, hWnd, "Ultima Online", NULL);
		}

		if (UOProcId != pid || hWnd == NULL)
		{
			hWnd = FindWindow("Ultima Online Third Dawn", NULL);
			while (hWnd != NULL)
			{
				UOTId = GetWindowThreadProcessId(hWnd, &UOProcId);
				if (UOProcId == pid)
					break;
				hWnd = FindWindowEx(NULL, hWnd, "Ultima Online Third Dawn", NULL);
			}
		}

		if (UOProcId != pid)
			return NO_TID;
	}
	else
	{
		hWnd = FindUOWindow();
		if (hWnd != NULL)
			UOTId = GetWindowThreadProcessId(hWnd, &UOProcId);
	}

	hUOWindow = hWnd;
	hRazorWnd = PostWindow;
	Log("InstallLibrary: hUOWindow=0x%x hRazorWnd=0x%x pid=%d", hUOWindow, hRazorWnd, pid);

	if (hUOWindow == NULL)
		return NO_UOWND;

	if (!UOTId || !UOProcId)
		return NO_TID;

	if (!CreateSharedMemory())
		return NO_SHAREMEM;
	//memset( pShared, 0, sizeof(SharedMemory) );

	pShared->IsHaxed = false;

	hWndProcRetHook = SetWindowsHookEx(WH_CALLWNDPROCRET, WndProcRetHookFunc, hInstance, UOTId);
	if (!hWndProcRetHook)
		return NO_HOOK;

	hGetMsgHook = SetWindowsHookEx(WH_GETMESSAGE, GetMsgHookFunc, hInstance, UOTId);
	if (!hGetMsgHook)
		return NO_HOOK;

	WNDCLASS wc;
	wc.style = 0;
	wc.lpfnWndProc = (WNDPROC)UOAWndProc;
	wc.cbClsExtra = 0;
	wc.cbWndExtra = 0;
	wc.hInstance = hInstance;
	wc.hIcon = LoadIcon(NULL, IDI_WINLOGO);
	wc.hCursor = LoadCursor(NULL, IDC_ARROW);
	wc.hbrBackground = NULL;
	wc.lpszMenuName = NULL;
	wc.lpszClassName = "UOASSIST-TP-MSG-WND";
	RegisterClass(&wc);
	DWORD error = GetLastError();

	hUOAWnd = CreateWindow("UOASSIST-TP-MSG-WND", "UOASSIST-TP-MSG-WND", WS_OVERLAPPEDWINDOW, 0, 0, 50, 50, NULL, NULL, hInstance, 0);
	if (hUOAWnd)
		ShowWindow(hUOAWnd, FALSE);

	ServerEncrypted = (flags & 0x10) != 0;
	ClientEncrypted = (flags & 0x08) != 0;

	Log("InstallLibrary: posting WM_PROCREADY to UO 0x%x flags=0x%x hRazorWnd=0x%x", hUOWindow, flags, hRazorWnd);
	PostMessage(hUOWindow, WM_PROCREADY, (WPARAM)flags, (LPARAM)hRazorWnd);
	return SUCCESS;
}

DLLFUNCTION void WaitForWindow(DWORD pid)
{
	Log("WaitForWindow: pid=%d", pid);
	DWORD UOTId = 0;
	DWORD exitCode;
	HANDLE hProc = OpenProcess(PROCESS_QUERY_INFORMATION, FALSE, pid);

	UOProcId = 0;

	do
	{
		Sleep(10);
		HWND hWnd = FindWindow("Ultima Online", NULL);
		while (hWnd != NULL)
		{
			UOTId = GetWindowThreadProcessId(hWnd, &UOProcId);
			if (UOProcId == pid)
				break;
			hWnd = FindWindowEx(NULL, hWnd, "Ultima Online", NULL);
		}

		if (UOProcId != pid || hWnd == NULL)
		{
			hWnd = FindWindow("Ultima Online Third Dawn", NULL);
			while (hWnd != NULL)
			{
				UOTId = GetWindowThreadProcessId(hWnd, &UOProcId);
				if (UOProcId == pid)
					break;
				hWnd = FindWindowEx(NULL, hWnd, "Ultima Online Third Dawn", NULL);
			}
		}

		GetExitCodeProcess(hProc, &exitCode);
	} while (UOProcId != pid && exitCode == STILL_ACTIVE);

	CloseHandle(hProc);
}

DLLFUNCTION void Shutdown(bool close)
{
	Log("Shutdown: close=%d", close);

	if (hUOAWnd && IsWindow(hUOAWnd))
	{
		UnregisterClass("UOASSIST-TP-MSG-WND", hInstance);
		SendMessage(hUOAWnd, WM_CLOSE, 0, 0);
		hUOAWnd = NULL;
	}

	if (hUOWindow && IsWindow(hUOWindow))
		PostMessage(hUOWindow, WM_QUIT, 0, 0);
}

DLLFUNCTION int GetUOProcId()
{
	//Log("GetUOProc");
	return UOProcId;
}

DLLFUNCTION HANDLE GetCommMutex()
{
	//Log("GetCommMutex");
	return CommMutex;
}

// totalsend and totalrecv are NOT mutex sync'd
DLLFUNCTION unsigned int TotalOut()
{
	if (pShared)
		return pShared->TotalSend;
	else
		return 0;
}

DLLFUNCTION unsigned int TotalIn()
{
	if (pShared)
		return pShared->TotalRecv;
	else
		return 0;
}

DLLFUNCTION bool IsCalibrated()
{
	return pShared && pShared->Position[0] == 0xFFFFFFFF && pShared->Position[1] == 0xDEADBEEF && pShared->Position[2] != 0 && pShared->Position[2] != 0xFFFFFFFF;
}

DLLFUNCTION void CalibratePosition(int x, int y, int z)
{
	Log("CalibratePosition: x=%d y=%d z=%d", x, y, z);
	pShared->Position[2] = x;
	pShared->Position[1] = y;
	pShared->Position[0] = z;

	PostMessage(hUOWindow, WM_UONETEVENT, CALIBRATE_POS, 0);
}

DLLFUNCTION bool GetPosition(int* x, int* y, int* z)
{
	if (IsCalibrated())
	{
		int buffer[3];
		DWORD Read = 0;
		HANDLE hProc = OpenProcess(PROCESS_VM_READ, FALSE, UOProcId);
		if (!hProc)
			return false;

		if (ReadProcessMemory(hProc, (void*)pShared->Position[2], buffer, sizeof(int) * 3, &Read))
		{
			if (Read == sizeof(int) * 3)
			{
				if (x)
					*x = buffer[2];
				if (y)
					*y = buffer[1];
				if (z)
					*z = buffer[0];
			}
			else
			{
				Read = 0;
			}
		}
		else
		{
			Read = 0;
		}

		CloseHandle(hProc);

		if (Read == sizeof(int) * 3 && (x == NULL || (*x >= 0 && *x < 8192)) && (y == NULL || (*y >= 0 && *y < 8192)))
		{
			return true;
		}
		else
		{
			memset(pShared->Position, 0, sizeof(int) * 3);
			return false;
		}
	}
	else
	{
		return false;
	}
}

DLLFUNCTION void BringToFront(HWND hWnd)
{
	Log("BringToFront: hWnd=0x%x", hWnd);
	SetWindowPos(hWnd, HWND_TOP, 0, 0, 0, 0, SWP_NOMOVE | SWP_NOSIZE);
	ShowWindow(hWnd, SW_SHOW);
	SetForegroundWindow(hWnd);
	SetFocus(hWnd);
}

#define CHEATPROC_STR "jTjAjHjC"
#define CHEATPROC_LEN 8

DLLFUNCTION void SetAllowDisconn(bool newVal)
{
	Log("SetAllowDisconn: newVal=%d", newVal);
	if (pShared && CommMutex)
	{
		WaitForSingleObject(CommMutex, INFINITE);
		pShared->AllowDisconn = newVal;
		ReleaseMutex(CommMutex);
	}
}

DLLFUNCTION BOOL HandleNegotiate(__int64 features)
{
	Log("HandleNegotiate: features=0x%llx", features);
	if (pShared && pShared->AuthBits && pShared->AllowNegotiate)
	{
		memcpy(pShared->AuthBits, &features, 8);

		ServerNegotiated = true;

		return TRUE;
	}
	else
	{
		return FALSE;
	}
}


SIZE* SizePtr = NULL;

// ---------------------------------------------------------------------------
// OnAttach() -- runs INSIDE the UO process to scan memory and apply patches.
//
// This function is called very early (before the UO window may exist) via a
// remote thread or the WM_PROCREADY hook mechanism.  It performs all the
// one-time memory scanning and patching that must happen from within the
// client process:
//
//   1. Creates shared memory and mutex.
//   2. Copies the static packet length table into SharedMemory.PacketTable.
//   3. Runs MemFinder over the executable image to locate:
//        - The client's internal packet length table (ClientPacketInfo array).
//        - Login encryption keys (Key1 and Key2 pointers).
//        - The screen-size variable and resize function.
//        - Version strings and other patchable constants.
//   4. Replaces the located client packet table in SharedMemory so Razor
//      sees the client's authoritative lengths rather than the static fallback.
//   5. Calls LoginEncryption::SetKeys() with the located key pointers.
//   6. Applies several in-memory patches:
//        - "UoClientApp" string -> "UoApp<pid>" (multi-instance support).
//        - "Another copy of " / "Multiple Instances Running" conditional jumps
//          -> unconditional jumps (bypass single-instance check).
//        - "report\0" -> "r<pid>" (per-instance report tag).
//        - Splash screen delay: 5000 ms -> 1 ms.
//        - Intro BIK filename -> garbage to skip video.
//        - "Electronic Arts Inc." banner -> RazorEnhanced credit.
//   7. Resolves NativeGetUOVersion (the client's own version-string function).
// ---------------------------------------------------------------------------
DLLFUNCTION void __stdcall OnAttach(void* params, int paramsLen)
{
	Log("OnAttach: paramsLen=%d", paramsLen);
	int count = 0;
	DWORD addr = 0, oldProt;
	MemFinder mf;

	UOProcId = GetCurrentProcessId();

	if (!CreateSharedMemory())
		return;

	pShared->AllowDisconn = true;

	CopyFailed = false;

	// Register all patterns that MemFinder should locate in a single pass.
	mf.AddEntry("UoClientApp", 12, 0x00500000);
	mf.AddEntry("report\0", 8, 0x00500000);
	mf.AddEntry("Another copy of ", 16, 0x00500000);
	mf.AddEntry("\x00\x68\x88\x13\x00\x00\x56\xE8", 8);
	mf.AddEntry("\x68\x88\x13\x00\x00", 5, 16, 0x00400000); // (end of a push offset), push 5000, push esi
	mf.AddEntry("Electronic Arts Inc.", 20);
	mf.AddEntry("intro.bik", 10);
	mf.AddEntry("osilogo.bik", 12);
	mf.AddEntry("\x80\x02\x00\x00\xE0\x01\x00\x00", 8, 0x00500000); // current screen size
	mf.AddEntry("\x8B\x44\x24\x04\xBA\x80\x02\x00\x00\x3B\xC2\xB9\xE0\x01\x00\x00", 16, 0x00400000); // resize screen function
	mf.AddEntry("\x57\x56\x6A\x00\x6A\x00\xE8", 7); // redraw screen/edge function
	mf.AddEntry(PACKET_TBL_STR, PACKET_TS_LEN, 10, 0x00500000);
	mf.AddEntry(CRYPT_KEY_STR, CRYPT_KEY_LEN);
	mf.AddEntry(CRYPT_KEY_STR_3D, CRYPT_KEY_3D_LEN);
	mf.AddEntry(CRYPT_KEY_STR_NEW, CRYPT_KEY_NEW_LEN);
	mf.AddEntry(CRYPT_KEY_STR_MORE_NEW, CRYPT_KEY_MORE_NEW_LEN);
	mf.AddEntry(CHEATPROC_STR, CHEATPROC_LEN);
	mf.AddEntry("CHEAT %s", 8, 0x00500000);
	mf.AddEntry("UO Version %s", 12);
	mf.AddEntry("Multiple Instances Running", 26, 0x00500000);

	//mf.AddEntry(ANIM_PATTERN_6, ANIM_PATTERN_LENGTH_6);

	//mf.AddEntry(SPEEDHACK_PATTERN_1, SPEEDHACK_PATTERN_LENGTH_1);

	memcpy(pShared->PacketTable, StaticPacketTable, 256 * sizeof(short));

	const BYTE defaultCheatKey[] = { 0x98, 0x5B, 0x51, 0x7E, 0x11, 0x0C, 0x3D, 0x77, 0x2D, 0x28, 0x41, 0x22, 0x74, 0xAD, 0x5B, 0x39 };
	memcpy(pShared->CheatKey, defaultCheatKey, 16);
	bool newclient = true;
	mf.Execute();

	SizePtr = (SIZE*)mf.GetAddress("\x80\x02\x00\x00\xE0\x01\x00\x00", 8);
	ResizeFuncaddr = mf.GetAddress("\x8B\x44\x24\x04\xBA\x80\x02\x00\x00\x3B\xC2\xB9\xE0\x01\x00\x00", 16);
	DWORD jump = ResizeFuncaddr + 16;
	BYTE offset = *(char*)(jump + 2);

	BYTE instruction = *(BYTE*)(jump);
	if (instruction == 0x0F)
		ABetterEntrypoint = jump + offset + 6 /* len of jump*/ + 12 /* skip default mov*/;
	if (instruction == 0x74)
	{
		offset = *(char*)(jump + 1);
		ABetterEntrypoint = jump + offset + 2 /* len of jump*/ + 12 /* skip default mov*/;
	}
	if (instruction == 0x75)
	{
		offset = *(char*)(jump + 1);
		ABetterEntrypoint = jump + offset - 3 + 42;
	}

	if (ABetterEntrypoint == 0)
	{
		Log("Unable to determine jump operations for video");
		ABetterEntrypoint = ResizeFuncaddr;
	}
	else
	{
		BYTE newOffset = (BYTE)(ABetterEntrypoint - jump - 5) /* I am totally confused*/;
		BYTE patch[5] = { 0xE9, newOffset, 0x0, 0x0, 0x0 };
		Log("BYPASS Patch Jump at 0x%x with 0x%x 0x%x", jump, patch[0], patch[1], patch[2]);
		memcpy(&SavedInstructions, (void*)jump, 5);
	}

	Log("SizePtr = 0x%x", SizePtr);
	Log("ResizeFuncAddr = 0x%x", ResizeFuncaddr);
	Log("ABetterEntrypoint = 0x%x", ABetterEntrypoint);

	int i = 0;
	while ((addr = mf.GetAddress(PACKET_TBL_STR, PACKET_TS_LEN, i++)) != 0)
	{
		memset(pShared->PacketTable, 0xFF, 512);

		addr += PACKET_TBL_OFFSET;
		if (IsBadReadPtr((void*)addr, sizeof(ClientPacketInfo) * 128))
			continue;
		ClientPacketInfo* tbl = (ClientPacketInfo*)addr;

		if (tbl[0].Id == 1 || tbl[0].Id == 2 || tbl[0].Id >= 256)
			continue;

		// this one isnt in order because OSI are idiots (0xF8)
		pShared->PacketTable[tbl[0].Id] = tbl[0].Length;

		int idx = 1;
		bool got1 = false, got2 = false;
		for (int prev = 0; prev < 255 && idx < 256; idx++)
		{
			if (IsBadReadPtr((void*)(tbl + idx), sizeof(ClientPacketInfo)) ||
				tbl[idx].Id <= prev || tbl[idx].Id >= 256)
			{
				break;
			}

			got1 |= tbl[idx].Id == 1 && tbl[idx].Length == StaticPacketTable[1];
			got2 |= tbl[idx].Id == 2 && tbl[idx].Length == StaticPacketTable[2];

			prev = tbl[idx].Id;
			if (pShared->PacketTable[prev] == 0xFFFF)
				pShared->PacketTable[prev] = tbl[idx].Length;
		}

		if (idx < 128 || !got1 || !got2)
			continue;
		else
			break;
	}

	if (!addr)
		CopyFailed = true;

	/*
	i = 0;
	while ((addr = mf.GetAddress(ANIM_PATTERN_6, ANIM_PATTERN_LENGTH_6, i++)) != 0)
	{
		//char blah[256];
		//sprintf_s(blah, 256, "%02X %02X %02X %02X", *(unsigned char*)(addr), *(unsigned char*)(addr + 1), *(unsigned char*)(addr - 14), *(unsigned char*)(addr - 5));
		//MessageBox(NULL, blah, "ANIM_PATTERN_6", MB_OK);
		if (memcmp((void*)(addr - 14), ANIM_PATTERN_4, ANIM_PATTERN_LENGTH_4) != 0)
			continue;

		//MessageBox(NULL, "Matched", "ANIM_PATTERN_6", MB_OK);
		DWORD origAddr = addr - 30;
		VirtualProtect((void*)origAddr, 128, PAGE_EXECUTE_READWRITE, &oldProt);
		memcpy((void*)(origAddr + 11), "\x33\xC0\x90\x90\x90", 5);
		memcpy((void*)(origAddr + 35), "\x08", 1);
		memcpy((void*)(origAddr + 38), "\x04", 1);
		VirtualProtect((void*)origAddr, 128, oldProt, &oldProt);
	}

	i = 0;
	while ((addr = mf.GetAddress(SPEEDHACK_PATTERN_1, SPEEDHACK_PATTERN_LENGTH_1, i++)) != 0)
	{
		//char blah[256];
		//sprintf_s(blah, 256, "%02X %02X %02X %02X", *(unsigned char*)(addr+8), *(unsigned char*)(addr + 9), *(unsigned char*)(addr +11), *(unsigned char*)(addr +12));
		//MessageBox(NULL, blah, "SPEEDHACK_PATTERN_1", MB_OK);
		if (memcmp((void*)(addr + 8), SPEEDHACK_PATTERN_2, SPEEDHACK_PATTERN_LENGTH_2) != 0)
			continue;

		//MessageBox(NULL, "Matched", "SPEEDHACK_PATTERN_1", MB_OK);
		DWORD origAddr = addr;
		VirtualProtect((void*)origAddr, 128, PAGE_EXECUTE_READWRITE, &oldProt);
		memcpy((void*)(origAddr + 10), "\x26", 1);
		VirtualProtect((void*)origAddr, 128, oldProt, &oldProt);
	}
	*/

	addr = mf.GetAddress(CRYPT_KEY_STR, CRYPT_KEY_LEN);
	if (!addr)
	{
		addr = mf.GetAddress(CRYPT_KEY_STR_NEW, CRYPT_KEY_NEW_LEN);

		if (!addr)
		{
			addr = mf.GetAddress(CRYPT_KEY_STR_MORE_NEW, CRYPT_KEY_MORE_NEW_LEN);
			if (!addr)
			{
				addr = mf.GetAddress(CRYPT_KEY_STR_3D, CRYPT_KEY_3D_LEN);
				if (addr)
				{
					LoginEncryption::SetKeys((const DWORD*)(addr + CRYPT_KEY_3D_LEN), (const DWORD*)(addr + CRYPT_KEY_3D_LEN + 19));
					ClientType = CLIENT_TYPE::THREED;
				}
				else
				{
					CopyFailed = true;
				}
			}
			else
			{
				addr += CRYPT_KEY_MORE_NEW_LEN;

				const DWORD* pKey1 = *((DWORD**)addr);
				const DWORD* pKey2 = pKey1 + 1;
				if (IsBadReadPtr(pKey2, 4) || IsBadReadPtr(pKey1, 4))
					CopyFailed = true;
				else
					LoginEncryption::SetKeys(pKey1, pKey2);
			}
		}
		else
		{
			addr += CRYPT_KEY_NEW_LEN;

			const DWORD* pKey1 = *((DWORD**)addr);
			const DWORD* pKey2 = pKey1 - 1;
			if (IsBadReadPtr(pKey2, 4) || IsBadReadPtr(pKey1, 4))
				CopyFailed = true;
			else
				LoginEncryption::SetKeys(pKey1, pKey2);
		}
	}
	else
	{
		LoginEncryption::SetKeys((const DWORD*)(addr + CRYPT_KEY_LEN), (const DWORD*)(addr + CRYPT_KEY_LEN + 6));
	}

	/*addr = mf.GetAddress( CHEATPROC_STR, CHEATPROC_LEN );
	if ( addr )
	{
		addr = MemFinder::Find( "\x8A\x91", 2, addr, addr + 0x80 );
		if ( addr )
		{
			addr += 2;

			if ( !IsBadReadPtr( (void*)(*((DWORD*)addr)), 16 ) )
				memcpy( pShared->CheatKey, (void*)(*((DWORD*)addr)), 16 );
		}
	}
	else
	{
		addr = mf.GetAddress( "CHEAT %s", 8 );
		if ( addr )
		{
			addr -= 16;
			if ( !IsBadReadPtr( (void*)(*((DWORD*)addr)), 16 ) )
				memcpy( pShared->CheatKey, (void*)(*((DWORD*)addr)), 16 );
			ClientType = THREED;
		}
	}*/
	BYTE cheatKey[16] = { 0x98, 0x5B, 0x51, 0x7E, 0x11, 0x0C, 0x3D, 0x77, 0x2D, 0x28, 0x41, 0x22, 0x74, 0xAD, 0x5B, 0x39 };
	memcpy(pShared->CheatKey, cheatKey, 16);

	// Multi UO
	addr = mf.GetAddress("UoClientApp", 12);
	if (addr)
	{
		VirtualProtect((void*)addr, 12, PAGE_READWRITE, &oldProt);
		_snprintf((char*)addr, 12, "UoApp%d", UOProcId);
		VirtualProtect((void*)addr, 12, oldProt, &oldProt);
	}

	addr = mf.GetAddress("Another copy of ", 16);
	if (addr)
	{
		char buff[5];

		buff[0] = 0x68; // push
		*((DWORD*)(&buff[1])) = addr;

		addr = 0x00400000;
		do {
			addr = MemFinder::Find(buff, 5, addr, 0x00600000);
			if (addr)
			{
				if ((*((unsigned char*)(addr - 5))) == 0x74) // jz?
					MemoryPatch(addr - 5, 0xEB, 1); // change to jmp
				addr += 5; // skip ahead to find the next instance
			}
		} while (addr > 0 && addr < 0x00600000);
	}

	addr = mf.GetAddress("Multiple Instances Running", 26);
	if (addr)
	{
		char buff[5];

		buff[0] = 0x68; // push
		*((DWORD*)(&buff[1])) = addr;

		addr = 0x00400000;
		do {
			addr = MemFinder::Find(buff, 5, addr, 0x00600000);
			if (addr)
			{
				char in = (*((unsigned char*)(addr - 4)));
				if (in == 0x74 || in == 0x75) { // jz or jnz
					MemoryPatch(addr - 4, 0xEB, 1); // change to jmp
				}
				addr += 5; // skip ahead to find the next instance
			}
		} while (addr > 0 && addr < 0x00600000);
	}

	addr = mf.GetAddress("report\0", 8);
	if (addr)
	{
		VirtualProtect((void*)addr, 12, PAGE_READWRITE, &oldProt);
		_snprintf((char*)addr, 8, "r%d", UOProcId);
		VirtualProtect((void*)addr, 12, oldProt, &oldProt);
	}

	// Splash screen crap:
	/*addr = mf.GetAddress( "\x00\x68\x88\x13\x00\x00\x56\xE8", 8 );
	if ( addr )
		MemoryPatch( addr+2, 0x00000005 ); // change 5000ms to 5ms*/
	for (int i = 0; i < 16; i++)
	{
		addr = mf.GetAddress("\x68\x88\x13\x00\x00", 5, i);
		if (!addr)
			break;
		for (int e = 5; e < 24; e++)
		{
			if (*((BYTE*)(addr + e)) == 0x8B && *((BYTE*)(addr + e + 1)) == 0x3D)
			{
				MemoryPatch(addr + 1, 0x00000001); // change 5000ms to 1ms
				i = 99;
				break;
			}
		}
	}
	addr = mf.GetAddress("intro.bik", 10);
	if (addr)
		MemoryPatch(addr, "intro.SUX", 10);
	addr = mf.GetAddress("ostlogo.bik", 12);
	if (addr)
		MemoryPatch(addr, "osilogo.SUX", 12);

	addr = mf.GetAddress("Electronic Arts Inc.", 20);
	if (addr)
	{
		addr -= 7;
		VirtualProtect((void*)addr, 52, PAGE_EXECUTE_READWRITE, &oldProt);
		strncpy((char*)addr, "[Powered by RazorEnhanced - Just Better]\0", 52);
		VirtualProtect((void*)addr, 52, oldProt, &oldProt);
	}

	NativeGetUOVersion = NULL;
	if (ClientType == CLIENT_TYPE::TWOD)
	{
		addr = mf.GetAddress("UO Version %s", 12);
		if (addr)
		{
			char temp[8];
			temp[0] = 0x68;
			*((DWORD*)&temp[1]) = addr;

			addr = MemFinder::Find(temp, 5);

			if (addr)
			{
				count = 0;
				while (*((BYTE*)addr) != 0xE8 && count < 128)
				{
					addr--;
					count++;
				}

				if (*((BYTE*)addr) == 0xE8)
					NativeGetUOVersion = (GetUOVersionFunc)((addr + 5) + *((DWORD*)(addr + 1)));
			}
		}
	}

	//HookFunction( "kernel32.dll", "CreateFileA", 0, (unsigned long)CreateFileAHook, &OldCreateFileA, &CreateFileAAddress );
}

DLLFUNCTION void SetServer(unsigned int addr, unsigned short port)
{
	Log("SetServer: addr=0x%x port=%d", addr, port);
	if (pShared)
	{
		pShared->ServerIP = addr;
		pShared->ServerPort = port;
	}
}

DLLFUNCTION const char* GetUOVersion()
{
	Log("GetUOVersion");
	if (pShared)
	{
		std::cout << "pshared";
		std::cout << pShared->UOVersion << std::endl;
		return pShared->UOVersion;
	}
	else
	{
		std::cout << "notpshared";
		return "";
	}
}

void BypassResize() {
	Log("BypassResize");
	DWORD jump = ResizeFuncaddr + 16;
	BYTE instruction = *(BYTE*)(jump);
	switch (instruction)
	{
	case 0x0F:
	case 0x74:
	case 0x75:
	{
		BYTE newOffset = (BYTE)(ABetterEntrypoint - jump - 5) /* I am totally confused*/;
		BYTE patch[5] = { 0xE9, newOffset, 0x0, 0x0, 0x0 };
		Log("BYPASS Patch Jump at 0x%x with 0x%x 0x%x", jump, patch[0], patch[1], patch[2]);
		memcpy(&SavedInstructions, (void*)jump, 5);
		MemoryPatch(jump, (const void*)&patch, 5);
	}
	break;
	default:
		break;
	}
}

void RestoreResize() {
	Log("RestoreResize");
	DWORD jump = ResizeFuncaddr + 16;
	BYTE instruction = *(BYTE*)(jump);
	// If its not bypassed leave it alone
	if (instruction == 0xE9)
	{
		Log("RESTORE Patch Jump at 0x%x with 0x%x 0x%x", jump, SavedInstructions[0], SavedInstructions[1], SavedInstructions[2]);
		MemoryPatch(jump, (const void*)&SavedInstructions, 5);
	}
}

// ---------------------------------------------------------------------------
// CreateSharedMemory() -- create (or open) the named shared memory block.
//
// Uses the UO process ID in the object names so multiple UO instances can
// coexist without colliding:
//   Mutex  : "UONetSharedCOMM_<pid>"  -- serialises all pShared accesses.
//   Mapping: "UONetSharedFM_<pid>"    -- sizeof(SharedMemory) bytes (~2.1 MB).
//
// Called from both the Razor side (InstallLibrary) and the UO side (OnAttach /
// WM_PROCREADY).  CreateFileMapping / CreateMutex will open the existing objects
// if they already exist (same name), so both processes end up sharing the same
// memory and the same mutex.
// ---------------------------------------------------------------------------
bool CreateSharedMemory()
{
	char name[256];

	CommMutex = NULL;
	hFileMap = NULL;
	pShared = NULL;

	Log("CreateSharedMemory: UOProcId=0x%x", UOProcId);

	// Create or open the named mutex for serialised access.
	sprintf(name, "UONetSharedCOMM_%x", UOProcId);
	CommMutex = CreateMutex(NULL, FALSE, name);
	if (!CommMutex)
		return false;

	// Create or open the named file mapping backed by the page file.
	sprintf(name, "UONetSharedFM_%x", UOProcId);
	hFileMap = CreateFileMapping(INVALID_HANDLE_VALUE, NULL, PAGE_READWRITE, 0, sizeof(SharedMemory), name);
	if (!hFileMap)
		return false;

	// Map the whole region into this process's address space.
	pShared = (SharedMemory*)MapViewOfFile(hFileMap, FILE_MAP_ALL_ACCESS, 0, 0, 0);
	if (!pShared)
		return false;

	//memset( pShared, 0, sizeof(SharedMemory) );

	return true;
}

// ---------------------------------------------------------------------------
// CloseSharedMemory() -- tear down all resources allocated by CreateSharedMemory.
//
// Removes the Windows message hooks, releases the mutex and file-mapping
// handles, and deletes the encryption objects.  Called from DllMain
// DLL_PROCESS_DETACH and from Shutdown().
// ---------------------------------------------------------------------------
void CloseSharedMemory()
{
	Log("CloseSharedMemory");

	// Remove the message hooks so they no longer fire in the UO thread.
	if (hWndProcRetHook)
		UnhookWindowsHookEx(hWndProcRetHook);
	if (hGetMsgHook)
		UnhookWindowsHookEx(hGetMsgHook);

	if (CommMutex)
		CloseHandle(CommMutex);
	CommMutex = NULL;
	if (pShared)
		UnmapViewOfFile(pShared);
	pShared = NULL;
	if (hFileMap)
		CloseHandle(hFileMap);
	hFileMap = NULL;

	//these are shared vars
	hWndProcRetHook = NULL;
	hGetMsgHook = NULL;

	//FreeArt();

	delete ClientCrypt;
	delete ClientLogin;
	delete ServerCrypt;
	delete ServerLogin;

	ClientCrypt = NULL;
	ClientLogin = NULL;
	ServerCrypt = NULL;
	ServerLogin = NULL;
}

// ---------------------------------------------------------------------------
// CreateEncryption() -- (re)allocate cipher objects for a new connection.
//
// Called from HookConnect() every time a new socket connection is established
// to ensure fresh cipher state.  Old objects are deleted before new ones are
// created so there is no state bleed across sessions.
//
// ClientEncrypted / ServerEncrypted control which sets of objects are created;
// both flags are set from the 'flags' argument of InstallLibrary() / WM_PROCREADY.
// ---------------------------------------------------------------------------
void CreateEncryption()
{
	Log("CreateEncryption: ClientEncrypted=%d ServerEncrypted=%d", ClientEncrypted, ServerEncrypted);
	// Destroy any existing cipher objects to reset state.
	delete ClientCrypt;
	delete ClientLogin;
	delete ServerCrypt;
	delete ServerLogin;

	// Allocate ciphers for the client-facing (DLL <-> client) direction.
	if (ClientEncrypted)
	{
		ClientCrypt = new OSIEncryption();
		ClientLogin = new LoginEncryption();
	}

	// Allocate ciphers for the server-facing (DLL <-> server) direction.
	if (ServerEncrypted)
	{
		ServerCrypt = new OSIEncryption();
		ServerLogin = new LoginEncryption();
	}
}

// ---------------------------------------------------------------------------
// Maintenance() -- defragment a shared-memory ring buffer.
//
// The Buffer uses Start as a read pointer that advances as data is consumed.
// When all data has been consumed (Length <= 0) we reset Start to 0 to keep
// the write position near the beginning.
//
// When Start drifts past the halfway point but there is still live data, we
// memmove() the live bytes back to offset 0.  This prevents the "write position"
// (Start + Length) from running off the end of the 512 KB Buff array.
// ---------------------------------------------------------------------------
inline void Maintenance(Buffer& buff)
{
	//Log("Maintenance");
	if (buff.Length <= 0)
	{
		// Buffer is empty; reset to beginning for next write.
		buff.Start = 0;
		buff.Length = 0;
	}
	else if (buff.Start > SHARED_BUFF_SIZE / 2)
	{
		// Live data is in the back half of the buffer; move it to the front
		// to reclaim space for future writes.
		//shift all the data to the begining of the buffer
		memmove(buff.Buff, &buff.Buff[buff.Start], buff.Length);
		buff.Start = 0;
	}
}

// ---------------------------------------------------------------------------
// RecvData() -- pull incoming data from the real socket and process it.
//
// Called from HookSelect() when select() reports that CurrentConnection has
// data ready to read.  We call the original recv() on the real socket, then:
//
//   Login server path (LoginServer == true):
//     Data arrives uncompressed; copy directly into InRecv for Razor.
//
//   Game server path:
//     1. Decrypt with ServerCrypt->DecryptFromServer() (XOR stream).
//     2. Huffman-decompress into InRecv.
//     3. If negotiation is pending and AllowNegotiate, scan for the 0xA9
//        (character list) packet to extract AuthBits and verify the MD5 hash.
//
//   On SOCKET_ERROR (not WSAEWOULDBLOCK): set ForceDisconn to signal
//   HookRecv() to simulate a connection reset to the client.
//
// The decrypted, decompressed data in InRecv is then available for Razor to
// read at any time.  A SendMessage(hRazorWnd, RECV) notifies Razor.
// ---------------------------------------------------------------------------
int RecvData()
{
	int len = SHARED_BUFF_SIZE;
	std::vector<char> buff(len);

	// Call the real (pre-hook) recv() to get data from the server.
	int ackLen = (*(NetIOFunc)OldRecv)(CurrentConnection, reinterpret_cast<char*>(buff.data()), buff.size(), 0);

	if (ackLen == SOCKET_ERROR)
	{
		if (WSAGetLastError() != WSAEWOULDBLOCK)
		{
			WaitForSingleObject(CommMutex, INFINITE);
			pShared->ForceDisconn = true;
			ReleaseMutex(CommMutex);
		}
		else
		{
			WSASetLastError(WSAEWOULDBLOCK);
		}

		ackLen = -1;
	}
	else if (ackLen > 0)
	{
		if (FirstRecv)
		{
			//Log("First Receive");
			Compression::Reset();
			FirstRecv = false;
		}

		WaitForSingleObject(CommMutex, INFINITE);

		pShared->TotalRecv += ackLen;

		if (LoginServer)
		{
			//Log("LoginServer");
			memcpy(&pShared->InRecv.Buff[pShared->InRecv.Start + pShared->InRecv.Length], reinterpret_cast<char*>(buff.data()), ackLen);
			pShared->InRecv.Length += ackLen;
		}
		else
		{
			//Log("Not LoginServer");
			if (ServerEncrypted)
				ServerCrypt->DecryptFromServer((BYTE*)reinterpret_cast<char*>(buff.data()), (BYTE*)reinterpret_cast<char*>(buff.data()), ackLen);

			int blen = Compression::Decompress((char*)&pShared->InRecv.Buff[pShared->InRecv.Start + pShared->InRecv.Length], reinterpret_cast<char*>(buff.data()), ackLen);
			pShared->InRecv.Length += blen;

			if (!ServerNegotiated && !InGame && pShared && pShared->AllowNegotiate)
			{
				int pos = pShared->InRecv.Start;
				unsigned char* p_buff = &pShared->InRecv.Buff[pos];

				while (pos < pShared->InRecv.Length)
				{
					int left = pShared->InRecv.Length - pos;
					int p_len = GetPacketLength(p_buff, left);

					if (*p_buff == 0xA9 && p_len >= 1 + 2 + 1 + 30 + 30 && p_len <= left)
					{
						// character list
						unsigned char hash[16], test[16];

						memcpy(pShared->AuthBits, p_buff + 1 + 2 + 1 + 30 + 1, 8);

						if (p_buff[3] > 1)
							memcpy(hash, p_buff + 1 + 2 + 1 + 30 + 30 + 30 + 1, 16);
						else
							memcpy(hash, p_buff + 1 + 2 + 1 + 30 + 1 + 8, 16);

						for (int i = 0; i < p_buff[3]; i++)
							memset(p_buff + 1 + 2 + 1 + 30 + 60 * i, 0, 30);

						if (memcmp(hash, "\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0", 16) != 0)
						{
							OSIEncryption::MD5(p_buff, p_len, test);

							ServerNegotiated = memcmp(hash, test, 16) == 0;
						}

						if (!ServerNegotiated)
							memset(pShared->AuthBits, 0, 8);

						Forwarding = Forwarded = false;

						break;
					}

					if (p_len <= 0)
					{
						break;
					}
					else
					{
						pos += p_len;
						p_buff += p_len;
					}
				}
			}
		}

		ReleaseMutex(CommMutex);

		Log("RecvData: SendMessage RECV to hRazorWnd=0x%x ackLen=%d", hRazorWnd, ackLen);
		SendMessage(hRazorWnd, WM_UONETEVENT, RECV, 0);
	}

	return ackLen;
}

// ---------------------------------------------------------------------------
// HookRecv() -- Winsock recv() intercept.
//
// The UO client calls recv() to retrieve data from the server.  We intercept
// this call so we can:
//   1. Force a disconnect: if ForceDisconn is set and the OutRecv queue is
//      empty, return WSAECONNRESET to make the client think the server closed
//      the connection.
//   2. Inject data from Razor: if OutRecv has data, compress it (Huffman) and
//      optionally re-encrypt it (EncryptForClient), then return it to the
//      client as if it came from the server.
//   3. For login-server connections, inject OutRecv data verbatim (no
//      compression or encryption on the login path).
//
// If the socket is not CurrentConnection, the original recv() is called
// transparently (pass-through for unmonitored sockets).
//
// Note: actual inbound data from the real server is processed by RecvData()
// which is driven by HookSelect(), NOT by HookRecv().  HookRecv() only
// handles the "client reads data" half of the loop.
// ---------------------------------------------------------------------------
int PASCAL HookRecv(SOCKET sock, char* buff, int len, int flags)
{
	int ackLen;
	Log("HookRecv: sock=%d CurrentConnection=%d len=%d", sock, CurrentConnection, len);
	if (sock == CurrentConnection && CurrentConnection)
	{
		WaitForSingleObject(CommMutex, INFINITE);
		// Force disconnect: signal WSAECONNRESET if Razor requested a disconnect
		// and the client has consumed all injected data from OutRecv.
		if (pShared->ForceDisconn && pShared->AllowDisconn && pShared->OutRecv.Length <= 0)
		{
			ReleaseMutex(CommMutex);
			WSASetLastError(WSAECONNRESET);
			return -1;
		}

		//Log("LoginServer setting %d", LoginServer);
		if (LoginServer)
		{
			if (pShared->OutRecv.Length > 0)
			{
				ackLen = pShared->OutRecv.Length;
				memcpy(buff, &pShared->OutRecv.Buff[pShared->OutRecv.Start], ackLen);

				if (((BYTE)buff[0]) == 0x8C)
					LoginServer = false;

				if (Forwarding)
					Seeded = Forwarded = true;

				pShared->OutRecv.Start += ackLen;
				pShared->OutRecv.Length -= ackLen;
			}
			else
			{
				ackLen = 0;
			}
		}
		else
		{
			ackLen = 0;
			while (pShared->OutRecv.Length > 0)
			{
				int blen = GetPacketLength(&pShared->OutRecv.Buff[pShared->OutRecv.Start], pShared->OutRecv.Length);

				if (blen <= 0 || blen > pShared->OutRecv.Length || ackLen + blen > len)
					break;

				ackLen += Compression::Compress(&buff[ackLen], (char*)&pShared->OutRecv.Buff[pShared->OutRecv.Start], blen);

				pShared->OutRecv.Start += blen;
				pShared->OutRecv.Length -= blen;
			}

			if (ClientEncrypted && ackLen > 0)
				ClientCrypt->EncryptForClient((BYTE*)buff, (BYTE*)buff, ackLen);
		}

		Maintenance(pShared->InRecv);
		Maintenance(pShared->OutRecv);

		ReleaseMutex(CommMutex);

		if (ackLen == 0)
		{
			WSASetLastError(WSAEWOULDBLOCK);
			return -1;
		}
		else
		{
			return ackLen;
		}
	}
	else
	{
		return (*(NetIOFunc)OldRecv)(sock, buff, len, flags);
	}
}

// ---------------------------------------------------------------------------
// HookSend() -- Winsock send() intercept.
//
// The UO client calls send() to transmit packets to the server.  We intercept
// to:
//   1. Capture the 4-byte connection seed (the very first data the client sends)
//      and use it to initialise the cipher objects (Initialize() on both
//      OSIEncryption and LoginEncryption instances).
//   2. After seeding, decrypt the outgoing packet stream:
//        - LoginServer + ClientEncrypted: use ClientLogin->Decrypt() (XOR cipher).
//        - Game server + ClientEncrypted: use ClientCrypt->DecryptFromClient() (Twofish).
//        - Forwarded (post-0x8C redirect): reinitialise with GenerateBadSeed() then decrypt.
//   3. Store the decrypted plaintext in InSend so Razor can inspect it.
//   4. Post SEND to Razor so it can act on the outgoing packet.
//   5. Call FlushSendData() to send any queued Razor->server packets (OutSend).
//   6. Lie to the client: always return 'len' as the sent count so the client
//      doesn't retry.  The actual transmission of OutSend happens in FlushSendData.
//
// The 0xEF prefix detection skips 16 bytes (a UO protocol handshake prefix that
// some servers use instead of the normal 4-byte seed).
// ---------------------------------------------------------------------------
int SkipSendData = 0;
int PASCAL HookSend(SOCKET sock, char* buff, int len, int flags)
{
	int ackLen;

	Log("HookSend: sock=%d CurrentConnection=%d len=%d", sock, CurrentConnection, len);
	if (sock == CurrentConnection && CurrentConnection)
	{
		if (!Seeded)
		{
			Log("HookSend: not seeded yet");
			// 0xEF prefix: this is a pre-seed protocol header; skip the next 16 bytes.
			if (len > 0 && ((BYTE)*buff) == ((BYTE)0xEF))
				SkipSendData = 16;

			if (len >= 4)
			{
				Seeded = true;

				CryptSeed = *((DWORD*)buff);

				if (ServerEncrypted)
				{
					ServerCrypt->Initialize(CryptSeed);
					ServerLogin->Initialize((BYTE*)&CryptSeed);
				}

				if (ClientEncrypted)
				{
					ClientCrypt->Initialize(CryptSeed);
					ClientLogin->Initialize((BYTE*)&CryptSeed);
				}

				Compression::Reset();
			}

			ackLen = (*(NetIOFunc)OldSend)(sock, buff, len, flags);
			pShared->TotalSend += len;
		}
		else if (SkipSendData < len)
		{
			SkipSendData = 0;

			if (FirstSend)
			{
				//Log("FirstSend");
				FirstSend = false;

				if (ClientEncrypted)
				{
					LoginServer = ClientLogin->TestForLogin((BYTE)buff[0]);
					// OSI was messing up the TestForLogin sometimes so
					// if it isn't the login server IP, FORCE LoginServer to false
					if (CurrentConnectionAddr.sin_addr.S_un.S_addr != pShared->ServerIP)
					{
						LoginServer = false;
					}
				}
				else
					LoginServer = LoginEncryption::IsLoginByte((BYTE)buff[0]);

				if (LoginServer)
				{
					Forwarding = Forwarded = false;
				}
			}

			WaitForSingleObject(CommMutex, INFINITE);

			//memcpy( &pShared->InSend.Buff[pShared->InSend.Start+pShared->InSend.Length], buff, len );

			if (ClientEncrypted)
			{
				if (Forwarded)
				{
					//Log("Forwarded");
					CryptSeed = LoginEncryption::GenerateBadSeed(CryptSeed);

					ClientCrypt->Initialize(CryptSeed);

					ClientCrypt->DecryptFromClient((BYTE*)buff, (BYTE*)(&pShared->InSend.Buff[pShared->InSend.Start + pShared->InSend.Length]), len);
					ClientLogin->Decrypt((BYTE*)(&pShared->InSend.Buff[pShared->InSend.Start + pShared->InSend.Length]), (BYTE*)(&pShared->InSend.Buff[pShared->InSend.Start + pShared->InSend.Length]), len);

					LoginServer = Forwarding = Forwarded = false;
				}
				else
				{
					if (LoginServer)
					{
						//Log("Not forwarded and login server");
						ClientLogin->Decrypt((BYTE*)(buff), (BYTE*)(&pShared->InSend.Buff[pShared->InSend.Start + pShared->InSend.Length]), len);

						if (((BYTE)pShared->InSend.Buff[pShared->InSend.Start + pShared->InSend.Length]) == 0xA0)
							Forwarding = true;
					}
					else
					{
						//Log("Not forwarded and not login server");
						ClientCrypt->DecryptFromClient((BYTE*)(buff), (BYTE*)(&pShared->InSend.Buff[pShared->InSend.Start + pShared->InSend.Length]), len);
					}
				}
			}

			pShared->InSend.Length += len;
			ReleaseMutex(CommMutex);

			Log("HookSend: SendMessage SEND to hRazorWnd=0x%x len=%d", hRazorWnd, len);
			SendMessage(hRazorWnd, WM_UONETEVENT, SEND, 0);

			WaitForSingleObject(CommMutex, INFINITE);
			FlushSendData();
			Maintenance(pShared->InSend);
			ReleaseMutex(CommMutex);

			ackLen = len;//lie and say we sent it all -- or should we tell the truth? (probably not since then it could try to send it again..)
		}
		else
		{
			ackLen = (*(NetIOFunc)OldSend)(sock, buff, len, flags);
			pShared->TotalSend += len;
			SkipSendData -= len;
		}
	}
	else
	{
		ackLen = (*(NetIOFunc)OldSend)(sock, buff, len, flags);
	}
	return ackLen;
}

#define RAZOR_ID_KEY "\x9\x11\x83+\x4\x17\x83\x5\x24\x85\x7\x17\x87\x6\x19\x88"
#define RAZOR_ID_KEY_LEN 16

// ---------------------------------------------------------------------------
// FlushSendData() -- transmit Razor's queued outgoing packets to the server.
//
// Called from HookSend() and from the SEND message handler in MessageProc().
// Drains OutSend by:
//   1. Scanning for 0x5D (play character) and 0x00 with sentinel 0xEDEDEDED
//      (char creation) packets -- embeds AuthBits + RAZOR_ID_KEY for server
//      feature negotiation.
//   2. If ServerEncrypted:
//        - Login phase: encrypts with ServerLogin->Encrypt() (XOR cipher).
//        - Game phase: encrypts with ServerCrypt->EncryptForServer() (Twofish).
//   3. Calls the real send() with the encrypted data.
//   4. Updates OutSend.Start / OutSend.Length to reflect what was sent.
// ---------------------------------------------------------------------------
void FlushSendData()
{
	Log("FlushSendData: OutSend.Length=%d CurrentConnection=%d", pShared ? pShared->OutSend.Length : -1, CurrentConnection);
	WaitForSingleObject(CommMutex, INFINITE);
	if (pShared->OutSend.Length > 0 && CurrentConnection)
	{
		int ackLen = 0;
		int outLen = pShared->OutSend.Length;
		//Log("FlushSend Data InGame: %d LoginServer: %d", InGame, LoginServer);
		if (!InGame && !LoginServer)
		{
			int pos = pShared->OutSend.Start;
			unsigned char* buff = &pShared->OutSend.Buff[pos];

			while (pos < outLen)
			{
				int left = pShared->OutSend.Length - pos;
				int len = GetPacketLength(buff, left);

				if (*buff == 0x5D && len >= 1 + 4 + 30 + 30 && len <= left)
				{
					// play character
					if (pShared->AllowNegotiate && ServerNegotiated)
					{
						// the first 2 bytes are 0
						// the next 4 bytes are "flags" which say the user's client type (lbr,t2a,aos,etc)
						// the rest are ignored, so we can use them for auth
						memcpy(buff + 1 + 4 + 30 + 2 + 4, pShared->AuthBits, 8);
						memcpy(buff + 1 + 4 + 30 + 2 + 4 + 8, RAZOR_ID_KEY, RAZOR_ID_KEY_LEN);
					}

					InGame = true;

					break;
				}
				else if (*buff == 0x00 && (*((DWORD*)&buff[1])) == 0xEDEDEDED && len >= 1 + 4 + 4 + 1 + 30 + 30 && len <= left)
				{
					// char creation
					if (pShared->AllowNegotiate && ServerNegotiated)
					{
						memcpy(buff + 1 + 4 + 4 + 1 + 30 + 15, pShared->AuthBits, 8);
						memcpy(buff + 1 + 4 + 4 + 1 + 30 + 15 + 8, RAZOR_ID_KEY, min(7, RAZOR_ID_KEY_LEN));
					}

					InGame = true;

					break;
				}

				if (len <= 0)
				{
					break;
				}
				else
				{
					pos += len;
					buff += len;
				}
			} // END while
		} // END if ( !InGame && !LoginServer

		if (ServerEncrypted)
		{
			if (tempBuff == NULL)
				tempBuff = new char[SHARED_BUFF_SIZE];

			if (LoginServer)
			{
				ServerLogin->Encrypt((BYTE*)&pShared->OutSend.Buff[pShared->OutSend.Start], (BYTE*)tempBuff, outLen);
			}
			else
			{
				ServerCrypt->EncryptForServer((BYTE*)&pShared->OutSend.Buff[pShared->OutSend.Start], (BYTE*)tempBuff, outLen);
			}

			ackLen = (*(NetIOFunc)OldSend)(CurrentConnection, tempBuff, outLen, 0);
		}
		else
		{
			ackLen = (*(NetIOFunc)OldSend)(CurrentConnection, (char*)&pShared->OutSend.Buff[pShared->OutSend.Start], outLen, 0);
		}

		if (ackLen == SOCKET_ERROR)
		{
			if (WSAGetLastError() != WSAEWOULDBLOCK)
				pShared->ForceDisconn = true;
		}
		else //if ( ackLen >= 0 )
		{
			pShared->TotalSend += ackLen;

			pShared->OutSend.Start += ackLen;
			pShared->OutSend.Length -= ackLen;
		}
	}

	Maintenance(pShared->OutSend);
	ReleaseMutex(CommMutex);
}

// ---------------------------------------------------------------------------
// HookConnect() -- Winsock connect() intercept.
//
// Intercepts every outbound TCP connection attempt by the UO client.  When
// the UO client tries to connect to a game server:
//   1. If Razor has set a server IP/port in pShared (and we haven't been
//      forwarded yet), substitute Razor's address for the one the client
//      originally targeted.  This lets Razor redirect the client to a
//      different server without the client knowing.
//   2. Record the connection address in CurrentConnectionAddr (used later in
//      HookSend to distinguish login vs game server connections).
//   3. On success: allocate fresh cipher objects, reset all state flags,
//      record the socket as CurrentConnection, and notify Razor via CONNECT.
// ---------------------------------------------------------------------------
int PASCAL HookConnect(SOCKET sock, const sockaddr* addr, int addrlen)
{
	int retVal;
	Log("HookConnect: sock=%d hRazorWnd=0x%x", sock, hRazorWnd);
	if (addr && addrlen >= sizeof(sockaddr_in))
	{
		const sockaddr_in* old_addr = (const sockaddr_in*)addr;
		sockaddr_in useAddr;

		memcpy(&useAddr, old_addr, sizeof(sockaddr_in));

		Log("HookConnect: original target ip=0x%x port=%d pShared->ServerIP=0x%x pShared->ServerPort=%d Forwarded=%d",
			old_addr->sin_addr.S_un.S_addr, ntohs(old_addr->sin_port),
			pShared ? pShared->ServerIP : 0, pShared ? pShared->ServerPort : 0, Forwarded);

		if (!Forwarded && pShared->ServerIP != 0)
		{
			useAddr.sin_addr.S_un.S_addr = pShared->ServerIP;
			useAddr.sin_port = htons(pShared->ServerPort);
			Log("HookConnect: redirecting to ip=0x%x port=%d", useAddr.sin_addr.S_un.S_addr, ntohs(useAddr.sin_port));
		}

		CurrentConnectionAddr = useAddr;
		retVal = (*(ConnFunc)OldConnect)(sock, (sockaddr*)&useAddr, sizeof(sockaddr_in));

		ConnectedIP = useAddr.sin_addr.S_un.S_addr;

		if (retVal != SOCKET_ERROR)
		{
			Log("HookConnect: connect succeeded sock=%d ip=0x%x", sock, useAddr.sin_addr.S_un.S_addr);
			CreateEncryption();

			Seeded = false;
			LoginServer = false;
			FirstRecv = true;
			FirstSend = true;
			Forwarding = Forwarded = false;

			WaitForSingleObject(CommMutex, INFINITE);
			CurrentConnection = sock;
			pShared->OutRecv.Length = pShared->InRecv.Length = pShared->OutSend.Length = pShared->InSend.Length = 0;
			pShared->ForceDisconn = false;
			ReleaseMutex(CommMutex);

			Log("HookConnect: PostMessage CONNECT to hRazorWnd=0x%x ip=0x%x", hRazorWnd, useAddr.sin_addr.S_un.S_addr);
			PostMessage(hRazorWnd, WM_UONETEVENT, CONNECT, useAddr.sin_addr.S_un.S_addr);
			connected = true;
		}
		else
		{
			Log("HookConnect: connect FAILED sock=%d WSAError=%d ip=0x%x port=%d",
				sock, WSAGetLastError(), useAddr.sin_addr.S_un.S_addr, ntohs(useAddr.sin_port));
		}
	}
	else
	{
		retVal = (*(ConnFunc)OldConnect)(sock, addr, addrlen);
	}

	return retVal;
}

// ---------------------------------------------------------------------------
// HookCloseSocket() -- Winsock closesocket() intercept.
//
// When the UO client closes the active socket:
//   1. Calls the real closesocket().
//   2. Clears CurrentConnection and all shared buffer lengths.
//   3. Resets the player position / stats counters.
//   4. If we were in-game, restore the screen size to 640x480 and undo the
//      resize bypass patch.
//   5. Notifies Razor via DISCONNECT.
// ---------------------------------------------------------------------------
int PASCAL HookCloseSocket(SOCKET sock)
{
	Log("HookCloseSocket: sock=%d CurrentConnection=%d", sock, CurrentConnection);
	int retVal = (*(CLSFunc)OldCloseSocket)(sock);

	if (sock == CurrentConnection && sock != 0)
	{
		Log("Closing socket %i", sock);
		CurrentConnection = 0;

		WaitForSingleObject(CommMutex, INFINITE);
		pShared->OutRecv.Length = pShared->InRecv.Length = pShared->OutSend.Length = pShared->InSend.Length = 0;
		memset(pShared->Position, 0, 4 * 3);
		pShared->TotalSend = pShared->TotalRecv = 0;
		pShared->ForceDisconn = false;
		ReleaseMutex(CommMutex);

		ServerNegotiated = false;
		if (InGame)
		{
			RestoreResize();
			SizePtr->cx = 640;
			SizePtr->cy = 480;
			InGame = false;
			if (ResizeFuncaddr)
			{
				((void(*)(int))ResizeFuncaddr)(640);
			}
		}

		memset(pShared->AuthBits, 0, 8);

		PostMessage(hRazorWnd, WM_UONETEVENT, DISCONNECT, 0);
		connected = false;
	}

	return retVal;
}

// ---------------------------------------------------------------------------
// HookSelect() -- Winsock select() intercept.
//
// select() is the UO client's main network wait function.  It blocks until one
// or more sockets are ready for I/O.  We intercept it to:
//
//   1. SmartCPU: if enabled, cap the select() timeout so the game loop runs at
//      ~30 fps (33,333 microseconds).  Saves CPU when the game is idle.
//
//   2. Server receive: if CurrentConnection was in readfd when select() returns
//      with data ready, remove it from readfd (so the client doesn't call recv()
//      itself), then call RecvData() to do our decryption and buffering.
//      Afterwards, if InRecv or OutRecv have data, add CurrentConnection back
//      to readfd so the client does call HookRecv() to get that data.
//
//   3. Force disconnect: if ForceDisconn is set, add CurrentConnection to
//      exceptfd so the client sees an error and initiates a clean disconnect.
//
// This architecture means the client never calls recv() on the real socket
// directly -- all server data flows through RecvData() first.
// ---------------------------------------------------------------------------
int PASCAL HookSelect(int ndfs, fd_set* readfd, fd_set* writefd, fd_set* exceptfd, const struct timeval* timeout)
{
	bool checkRecv = false;
	bool checkErr = false;
	bool modified = false;
	int realRet = 0;
	int myRet = 0;

	//Log("HookSelect CurrentConnection : % d", CurrentConnection);
	if (CurrentConnection)
	{
		if (readfd != NULL)
			checkRecv = FD_ISSET(CurrentConnection, readfd);

		if (exceptfd != NULL)
			checkErr = FD_ISSET(CurrentConnection, exceptfd);
	}

	timeval myTimeout;

	if (SmartCPU)
	{
		int length = 0;

		if (Active)
		{
			LARGE_INTEGER end;
			QueryPerformanceCounter(&end);

			length = int(1000000.0 * ((end.QuadPart - Counter.QuadPart) / double(PerfFreq.QuadPart)));
		}

		if (length < 33333)
		{
			myTimeout.tv_sec = 0;
			myTimeout.tv_usec = 33333 - length;
			timeout = &myTimeout;
		}
	}

	realRet = (*(SelectFunc)OldSelect)(ndfs, readfd, writefd, exceptfd, timeout);

	if (SmartCPU)
		QueryPerformanceCounter(&Counter);

	if (checkRecv)
	{
		if (FD_ISSET(CurrentConnection, readfd))
		{
			FD_CLR(CurrentConnection, readfd);
			RecvData();
			realRet--;
		}

		WaitForSingleObject(CommMutex, INFINITE);
		if (pShared->OutRecv.Length > 0 || (pShared->ForceDisconn && pShared->AllowDisconn))
		{
			FD_SET(CurrentConnection, readfd);
			myRet++;
		}
		ReleaseMutex(CommMutex);
	}

	if (checkErr && !FD_ISSET(CurrentConnection, exceptfd))
	{
		WaitForSingleObject(CommMutex, INFINITE);
		if (pShared->ForceDisconn && pShared->AllowDisconn && pShared->OutRecv.Length <= 0)
		{
			FD_SET(CurrentConnection, exceptfd);
			myRet++;
		}
		ReleaseMutex(CommMutex);
	}

	if (realRet < 0)
	{
		return myRet;
	}
	else
	{
		return realRet + myRet;
	}
}

// ---------------------------------------------------------------------------
// HookFunction() -- patch one entry in the PE Import Address Table (IAT).
//
// Winsock hooking is implemented by overwriting the function pointer that the
// UO client's executable stores for each imported Winsock function.  When the
// client calls (e.g.) wsock32!recv, it goes through a thunk that reads the
// pointer from the IAT -- we replace that pointer with our hook function.
//
// Algorithm:
//   1. Walk the PE IMAGE_IMPORT_DESCRIPTOR list to find 'Dll'.
//   2. Walk that DLL's thunk chains (OriginalFirstThunk for names, FirstThunk
//      for the actual function pointers) to find the entry matching 'FuncName'
//      or 'Ordinal'.
//   3. Save the original pointer in *OldAddr (so the hook can call-through).
//   4. Save the IAT slot address in *PatchAddr.
//   5. Call MemoryPatch() to write 'NewAddr' into the IAT slot (temporarily
//      changing page permissions with VirtualProtect).
//
// Parameters:
//   Dll       : DLL name to search (e.g., "wsock32.dll"), case-insensitive.
//   FuncName  : Exported function name (NULL if matching by ordinal only).
//   Ordinal   : Import ordinal (0 if matching by name only).
//   NewAddr   : Address of our hook function to install.
//   OldAddr   : Receives the original (pre-hook) function pointer.
//   PatchAddr : Receives the address of the IAT slot we patched.
//
// Returns true on success.
// ---------------------------------------------------------------------------
bool HookFunction(const char* Dll, const char* FuncName, int Ordinal, unsigned long NewAddr,
	unsigned long* OldAddr, unsigned long* PatchAddr)
{
	Log("HookFunction: Dll=%s FuncName=%s Ordinal=%d", Dll, FuncName ? FuncName : "(null)", Ordinal);
	DWORD baseAddr = (DWORD)GetModuleHandle(NULL);
	if (!baseAddr)
		return false;

	IMAGE_DOS_HEADER* idh = (IMAGE_DOS_HEADER*)baseAddr;

	IMAGE_FILE_HEADER* ifh = (IMAGE_FILE_HEADER*)(baseAddr + idh->e_lfanew + sizeof(DWORD));

	IMAGE_OPTIONAL_HEADER* ioh = (IMAGE_OPTIONAL_HEADER*)((DWORD)(ifh)+sizeof(IMAGE_FILE_HEADER));

	IMAGE_IMPORT_DESCRIPTOR* iid = (IMAGE_IMPORT_DESCRIPTOR*)(baseAddr + ioh->DataDirectory[IMAGE_DIRECTORY_ENTRY_IMPORT].VirtualAddress);

	while (iid->Name)
	{
		if (_stricmp(Dll, (char*)(baseAddr + iid->Name)) == 0)
		{
			IMAGE_THUNK_DATA* pThunk = (IMAGE_THUNK_DATA*)((DWORD)iid->OriginalFirstThunk + baseAddr);
			IMAGE_THUNK_DATA* pThunk2 = (IMAGE_THUNK_DATA*)((DWORD)iid->FirstThunk + baseAddr);

			while (pThunk->u1.AddressOfData)
			{
				char* name = NULL;
				int ord;

				if (pThunk->u1.Ordinal & 0x80000000)
				{
					// Imported by ordinal only:
					ord = pThunk->u1.Ordinal & 0xFFFF;
				}
				else
				{
					// Imported by name (with ordinal hint)
					IMAGE_IMPORT_BY_NAME* pName = (IMAGE_IMPORT_BY_NAME*)((DWORD)pThunk->u1.AddressOfData + baseAddr);
					ord = pName->Hint;
					name = (char*)pName->Name;
				}

				if (ord == Ordinal || (name && FuncName && !strcmp(name, FuncName)))
				{
					*OldAddr = (unsigned long)pThunk2->u1.Function;
					*PatchAddr = (unsigned long)(&pThunk2->u1.Function);
					MemoryPatch(*PatchAddr, NewAddr);

					return true;
				}

				pThunk++;
				pThunk2++;
			}
		}
		iid++;
	}

	return false;
}

bool FindFunction(const char* Dll, const char* FuncName, int Ordinal, unsigned long* ImpAddr, unsigned long* CallAddr)
{
	DWORD baseAddr = (DWORD)GetModuleHandle(NULL);
	if (!baseAddr)
		return false;

	IMAGE_DOS_HEADER* idh = (IMAGE_DOS_HEADER*)baseAddr;

	IMAGE_FILE_HEADER* ifh = (IMAGE_FILE_HEADER*)(baseAddr + idh->e_lfanew + sizeof(DWORD));

	IMAGE_OPTIONAL_HEADER* ioh = (IMAGE_OPTIONAL_HEADER*)((DWORD)(ifh)+sizeof(IMAGE_FILE_HEADER));

	IMAGE_IMPORT_DESCRIPTOR* iid = (IMAGE_IMPORT_DESCRIPTOR*)(baseAddr + ioh->DataDirectory[IMAGE_DIRECTORY_ENTRY_IMPORT].VirtualAddress);

	while (iid->Name)
	{
		if (_stricmp(Dll, (char*)(baseAddr + iid->Name)) == 0)
		{
			IMAGE_THUNK_DATA* pThunk = (IMAGE_THUNK_DATA*)((DWORD)iid->OriginalFirstThunk + baseAddr);
			IMAGE_THUNK_DATA* pThunk2 = (IMAGE_THUNK_DATA*)((DWORD)iid->FirstThunk + baseAddr);

			while (pThunk->u1.AddressOfData)
			{
				char* name = NULL;
				int ord;

				if (pThunk->u1.Ordinal & 0x80000000)
				{
					// Imported by ordinal only:
					ord = pThunk->u1.Ordinal & 0xFFFF;
				}
				else
				{
					// Imported by name (with ordinal hint)
					IMAGE_IMPORT_BY_NAME* pName = (IMAGE_IMPORT_BY_NAME*)((DWORD)pThunk->u1.AddressOfData + baseAddr);
					ord = pName->Hint;
					name = (char*)pName->Name;
				}

				if (ord == Ordinal || (name && FuncName && !strcmp(name, FuncName)))
				{
					*ImpAddr = (unsigned long)pThunk2->u1.Function;
					*CallAddr = (unsigned long)(&pThunk2->u1.Function);

					return true;
				}

				pThunk++;
				pThunk2++;
			}
		}
		iid++;
	}

	return false;
}

#define NOTO_HUE_STR "\0\0\0\0\x59\0\0\0\x3F\0\0\0\xb2\x03\0\0"
#define NOTO_HUE_LEN 16

DWORD NotoLoc = 0;
void SetCustomNotoHue(int hue)
{
	Log("SetCustomNotoHue: hue=%d", hue);
	if (!NotoLoc)
	{
		NotoLoc = MemFinder::Find(NOTO_HUE_STR, NOTO_HUE_LEN);
		if (!NotoLoc)
			NotoLoc = 0xFFFFFFFF;
	}

	if (NotoLoc != 0xFFFFFFFF)
		*((int*)(NotoLoc + 8 * 4)) = hue;
}

// ---------------------------------------------------------------------------
// PatchMemory() -- install all five Winsock hooks via IAT patching.
//
// Called from the WM_PROCREADY handler (MessageProc) which runs in the UO
// thread context -- required because GetModuleHandle(NULL) must return the UO
// executable base, not Razor's.
//
// The five hooks form the complete interception chain:
//   closesocket -- detect disconnect, clean up state, notify Razor.
//   connect     -- intercept server address, create cipher objects.
//   recv        -- serve injected data from OutRecv back to the client.
//   select      -- the primary recv pump; call RecvData() here.
//   send        -- capture seed, decrypt client->server, store in InSend.
//
// Returns true only if ALL five hooks succeed (one failure = no hooks).
// ---------------------------------------------------------------------------
bool PatchMemory(void)
{
	Log("PatchMemory: installing IAT hooks");

	return
		HookFunction("wsock32.dll", "closesocket", 3, (unsigned long)HookCloseSocket, &OldCloseSocket, &CloseSocketAddress) &&
		HookFunction("wsock32.dll", "connect", 4, (unsigned long)HookConnect, &OldConnect, &ConnectAddress) &&
		HookFunction("wsock32.dll", "recv", 16, (unsigned long)HookRecv, &OldRecv, &RecvAddress) &&
		HookFunction("wsock32.dll", "select", 18, (unsigned long)HookSelect, &OldSelect, &SelectAddress) &&
		HookFunction("wsock32.dll", "send", 19, (unsigned long)HookSend, &OldSend, &SendAddress)
		;
	//HookFunction( "wsock32.dll", "socket", 23, (unsigned long)HookSocket, &OldSocket, &SocketAddress)
	//HookFunction( "wsock32.dll", "WSAAsyncSelect", 101, (unsigned long)HookAsyncSelect, &OldAsyncSelect, &AsyncSelectAddress );
}

// ---------------------------------------------------------------------------
// MemoryPatch() -- overloads for writing to read-only client memory regions.
//
// The UO .text section is PAGE_EXECUTE_READ; IAT slots are PAGE_READONLY.
// To patch either, we temporarily change the page protection to PAGE_READWRITE,
// write the new value, then restore the original protection.
//
// Three overloads:
//   (Address, DWORD)             -- write a 4-byte DWORD value.
//   (Address, int, numBytes)     -- write the low 'numBytes' of an integer.
//   (Address, void*, length)     -- write arbitrary 'length' bytes.
// ---------------------------------------------------------------------------
void MemoryPatch(unsigned long Address, unsigned long value)
{
	MemoryPatch(Address, &value, 4); // sizeof(int)
}

void MemoryPatch(unsigned long Address, int value, int numBytes)
{
	MemoryPatch(Address, &value, numBytes);
}

void MemoryPatch(unsigned long Address, const void* value, int length)
{
	DWORD OldProtect;
	// Make the target page writable.
	if (!VirtualProtect((void*)Address, length, PAGE_READWRITE, &OldProtect))
		return;

	memcpy((void*)Address, value, length);

	// Restore the original page protection.
	VirtualProtect((void*)Address, length, OldProtect, &OldProtect);
}

bool CheckParent(HWND hCheck, HWND hComp)
{
	hCheck = GetParent(hCheck);
	while (hCheck != hComp && hCheck != NULL)
		hCheck = GetParent(hCheck);

	return (hCheck == hComp);
}

vector<DWORD> addrList;
void FindList(DWORD val, unsigned short size)
{
	if (size == 0xFFFF)
	{
		addrList.clear();
		return;
	}

	if (size > 4 || size < 1) size = 4;

	MemFinder mf;

	mf.AddEntry(&val, size, 0x7FFF, 0x00400000);

	mf.Execute();

	DWORD addr;
	if (addrList.size() == 0)
	{
		for (int i = 0; i < 0x7FFF; i++)
		{
			addr = mf.GetAddress(&val, size, i);
			if (addr)
				addrList.push_back(addr);
			else
				break;
		}
	}
	else
	{
		vector<DWORD> newList;
		for (unsigned int i = 0; i < addrList.size(); i++)
		{
			if (memcmp((void*)addrList[i], &val, size) == 0)
				newList.push_back(addrList[i]);
		}

		addrList = newList;
	}

	PostMessage(hRazorWnd, WM_UONETEVENT, MAKELONG(FINDDATA, 0), addrList.size());
	for (unsigned int i = 0; i < addrList.size() && i < 10; i++)
		PostMessage(hRazorWnd, WM_UONETEVENT, MAKELONG(FINDDATA, i + 1), addrList[i]);
}

void CALLBACK MessageProc(HWND hWnd, UINT nMsg, WPARAM wParam, LPARAM lParam, MSG* pMsg)
{
	HWND hFore;

	Log("MessageProc hwnd=0x%x, nMsg=0x%x, wParam=0x%x, lPARAM=0x%x", hWnd, nMsg, wParam, lParam);
	switch (nMsg)
	{
		// Custom messages
	case WM_PROCREADY:
		hRazorWnd = (HWND)lParam;
		UOProcId = GetCurrentProcessId();
		hUOWindow = hWnd;
		Log("WM_PROCREADY: hRazorWnd=0x%x UOProcId=%d hUOWindow=0x%x", hRazorWnd, UOProcId, hUOWindow);

		ClientEncrypted = (wParam & 0x08) != 0;
		ServerEncrypted = (wParam & 0x10) != 0;

		InitThemes();

		if (!pShared) // If this failed the first time or was not run at all, try it once more before panicing
			OnAttach(NULL, 0);

		if (!pShared)
		{
			Log("WM_PROCREADY: posting NOT_READY (NO_SHAREMEM)");
			PostMessage(hRazorWnd, WM_UONETEVENT, NOT_READY, NO_SHAREMEM);
		}
		else if (CopyFailed)
		{
			Log("WM_PROCREADY: posting NOT_READY (NO_COPY)");
			PostMessage(hRazorWnd, WM_UONETEVENT, NOT_READY, NO_COPY);
		}
		else if (!PatchMemory())
		{
			Log("WM_PROCREADY: posting NOT_READY (NO_PATCH)");
			PostMessage(hRazorWnd, WM_UONETEVENT, NOT_READY, NO_PATCH);
		}
		else
		{
			Log("WM_PROCREADY: posting READY");
			PostMessage(hRazorWnd, WM_UONETEVENT, READY, SUCCESS);
		}

		if (pShared)
		{
			pShared->AllowNegotiate = (wParam & 0x04) != 0;

			pShared->UOVersion[0] = 0;

			if (NativeGetUOVersion != NULL)
				strncpy(pShared->UOVersion, NativeGetUOVersion(), 16);
		}

		break;

		// ZIPPY REV 80
		/*	case WM_SETFWDWND:
				PostMessage( hPostWnd, WM_UONETEVENT, SET_FWD_HWND, lParam );
				break;
		*/
	case WM_UONETEVENT:
		switch (LOWORD(wParam))
		{
		case SEND:
			FlushSendData();
			break;
			//case STAT_BAR:
			//	PatchStatusBar((BOOL)lParam);
			//	break;
		case NOTO_HUE:
			SetCustomNotoHue((int)lParam);
			break;
		case DEATH_MSG:
			PatchDeathMsg();
			break;
		case CALIBRATE_POS:
			WaitForSingleObject(CommMutex, INFINITE);
			if (pShared->Position[0] >= -255 && pShared->Position[0] <= 255 && pShared->Position[1] >= 0 && pShared->Position[1] <= 8192 && pShared->Position[2] >= 0 && pShared->Position[2] <= 8192)
			{
				pShared->Position[2] = (int)MemFinder::Find(pShared->Position, sizeof(int) * 3, 0x00500000, 0x00C00000);
				if (pShared->Position[2])
				{
					pShared->Position[0] = 0xFFFFFFFF;
					pShared->Position[1] = 0xDEADBEEF;
				}
				else
				{
					memset(pShared->Position, 0, sizeof(int) * 3);
				}
			}
			ReleaseMutex(CommMutex);
			break;

		case OPEN_RPV:
			SendMessage(hRazorWnd, WM_UONETEVENT, OPEN_RPV, lParam);
			break;

		case SETWNDSIZE:
		{

			int x = LOWORD(lParam);
			int y = HIWORD(lParam);
			Log("SetWndSize called with x: %d y: %d", x, y);
			SIZE desiredSize = { (x / 4) * 4, (y / 4) * 4 };

			if (desiredSize.cx != 0 && desiredSize.cy != 0)
			{
				if (SizePtr != 0)
				{
					if (DesiredSize.cx == 0 && DesiredSize.cy == 0)  // Set from initial value
					{
						OriginalSize.cx = SizePtr->cx;
						OriginalSize.cy = SizePtr->cy;
					}

					if (desiredSize.cx != SizePtr->cx || desiredSize.cy != SizePtr->cy)
					{
						DesiredSize = desiredSize;
						if (InGame == true)
						{
							SizePtr->cx = DesiredSize.cx;
							SizePtr->cy = DesiredSize.cy;
						}
						else
						{
							SizePtr->cx = OriginalSize.cx;
							SizePtr->cy = OriginalSize.cy;
						}
						BypassResize();
						if (ABetterEntrypoint)
						{
							((void(*)(int))ABetterEntrypoint)(SizePtr->cx);
						}
					}

				}
			}
			else
			{
				RestoreResize();
				if (ResizeFuncaddr)
				{
					//((void(*)(int))ResizeFuncaddr)(OriginalSize.cx);
				}
				DesiredSize.cx = 0;
				DesiredSize.cy = 0;
			}
		}
		break;

		case FINDDATA:
			FindList((DWORD)lParam, HIWORD(wParam));
			break;

		case SMART_CPU:
			SmartCPU = lParam;
			break;

		case NEGOTIATE:
			if (pShared)
				pShared->AllowNegotiate = (lParam != 0);
			break;

		case SET_MAP_HWND:
			hMapWnd = (HWND)lParam;
			break;

			// ZIPPY REV 80
			/*		case SET_FWD_HWND:
						PostMessage( hPostWnd, WM_UONETEVENT, SET_FWD_HWND, lParam );
						break;
			*/
		}
		break;
	case WM_MOVE:
	{
		UINT width = LOWORD(lParam);
		UINT height = HIWORD(lParam);
		Log("WM_MOVE called with width: %d, height: %d", width, height);
		if (SizePtr != 0)
		{
			if ((DesiredSize.cy != 0 && DesiredSize.cx != 0) && (SizePtr->cx != DesiredSize.cx && SizePtr->cy != DesiredSize.cy))
			{
				if (InGame == true)
				{
					SizePtr->cx = DesiredSize.cx;
					SizePtr->cy = DesiredSize.cy;
				}
				else
				{
					SizePtr->cx = 640;
					SizePtr->cy = 480;
				}
				if (ABetterEntrypoint)
				{
					((void(*)(int))ABetterEntrypoint)(SizePtr->cx);
				}

			}
		}


	}
	break;
	//case WM_SIZE:
	//break;
	/*if (wParam == 2 && pMsg && pMsg->hwnd == hWnd)
	pMsg->lParam = lParam = MAKELONG( 800, 600 );
	break;
	*/
	case WM_GETMINMAXINFO:
		if (false /*SetMaxSize*/)
		{
			int x = ((MINMAXINFO*)lParam)->ptMaxSize.x;
			int y = ((MINMAXINFO*)lParam)->ptMaxSize.y;
			int x_track = ((MINMAXINFO*)lParam)->ptMaxTrackSize.x;
			int y_track = ((MINMAXINFO*)lParam)->ptMaxTrackSize.y;
			Log("WM_GETMINMAXINFO called with x: %d y: %d x_track: %d y_track: %d", x, y, x_track, y_track);
			/*
		((MINMAXINFO *)lParam)->ptMaxSize.x = 800;
		((MINMAXINFO *)lParam)->ptMaxSize.y = 600;
		((MINMAXINFO *)lParam)->ptMaxTrackSize.x = 800;
		((MINMAXINFO *)lParam)->ptMaxTrackSize.y = 600;
		*/
		}
		break;

		// Macro stuff
	case WM_SYSKEYDOWN:
	case WM_KEYDOWN:
	{
		/** Get the shift state and send it along with the keypress **/
		int lcontrol = (int)(GetAsyncKeyState(VK_CONTROL));
		int lalt = (int)(GetAsyncKeyState(VK_MENU));
		int lshift = (int)(GetAsyncKeyState(VK_SHIFT));
		unsigned int mods = 0;
		if (lcontrol != 0)
		{
			mods |= 131072;
		}
		if (lshift != 0)
		{
			mods |= 65536;
		}
		if (lalt != 0)
		{
			mods |= 262144;
		}
		mods |= ((int)wParam);

		if (pMsg && !SendMessage(hRazorWnd, WM_UONETEVENT, KEYDOWN, mods))
		{
			// dont give the key to the client
			pMsg->message = WM_NULL;
			pMsg->lParam = 0;
			pMsg->wParam = 0;
		}
	}
	break;

	case WM_SYSKEYUP:
	case WM_KEYUP:
		if (pMsg && wParam == VK_SNAPSHOT) // VK_SNAPSHOT (Print Screen) Doesn't seem to send a KeyDown message
			SendMessage(hRazorWnd, WM_UONETEVENT, KEYDOWN, wParam);
		break;

	case WM_MOUSEWHEEL:
		PostMessage(hRazorWnd, WM_UONETEVENT, MOUSE, MAKELONG(0, (((short)HIWORD(wParam)) < 0 ? -1 : 1)));
		break;
	case WM_MBUTTONDOWN:
		PostMessage(hRazorWnd, WM_UONETEVENT, MOUSE, MAKELONG(1, 0));
		break;
	case WM_XBUTTONDOWN:
		PostMessage(hRazorWnd, WM_UONETEVENT, MOUSE, MAKELONG(HIWORD(wParam) + 1, 0));
		break;

		//Activation tracking :
	case WM_ACTIVATE:
		Active = wParam;
		PostMessage(hRazorWnd, WM_UONETEVENT, ACTIVATE, wParam);
		break;
	case WM_KILLFOCUS:
		hFore = GetForegroundWindow();
		if (((HWND)wParam) != hRazorWnd && hFore != hRazorWnd && ((HWND)wParam) != hMapWnd && hFore != hMapWnd
			&& !CheckParent(hFore, hRazorWnd))
		{
			PostMessage(hRazorWnd, WM_UONETEVENT, FOCUS, FALSE);
		}
		break;
	case WM_SETFOCUS:
		PostMessage(hRazorWnd, WM_UONETEVENT, FOCUS, TRUE);
		break;

		//Custom title bar:
	case WM_NCACTIVATE:
		Active = wParam;
		//fallthrough
	case WM_NCPAINT:
	case WM_GETICON:
	case WM_SETTEXT:
	case WM_CUSTOMTITLE:
		CheckTitlebarAttr(hWnd);
		RedrawTitleBar(hWnd, Active);
		break;
	}
	return;
}

// ---------------------------------------------------------------------------
// GetMsgHookFunc() -- WH_GETMESSAGE hook callback.
//
// Fires inside the UO thread when a message is retrieved (removed) from the
// UO thread message queue.  This is the primary IPC path from Razor:
//   - Razor PostMessages WM_PROCREADY / WM_UONETEVENT to hUOWindow.
//   - The hook fires, MessageProc() processes the command, and we can safely
//     call any Win32 API that requires the UO thread context.
//
// We only process messages directed to hUOWindow (or WM_PROCREADY before
// hUOWindow is known).  All other messages are passed through unchanged.
// 'pMsg' is a MSG* that can be modified (e.g., setting message = WM_NULL
// suppresses delivery to the UO window's wndproc).
// ---------------------------------------------------------------------------
LRESULT CALLBACK GetMsgHookFunc(int Code, WPARAM Flag, LPARAM pMsg)
{
	if (Code >= 0 && Flag != PM_NOREMOVE)
	{
		MSG* DbgMsg = (MSG*)pMsg;
		Log("GetMsgHookFunc: Code=%d msg=0x%x hwnd=0x%x", Code, DbgMsg->message, DbgMsg->hwnd);
	}
	if (Code >= 0 && Flag != PM_NOREMOVE) //dont process messages until they are removed from the queue
	{
		MSG* Msg = (MSG*)pMsg;
		/*
		Msg->message ^= 0x11;
		Msg->message ^= Disabled * 101;
		Msg->message *= !(Disabled * 020);
		Msg->message ^= 0x11;
		*/
		if (Msg->hwnd == hUOWindow || (hUOWindow == NULL && Msg->message == WM_PROCREADY))
			MessageProc(Msg->hwnd, Msg->message, Msg->wParam, Msg->lParam, Msg);
	}

	return CallNextHookEx(NULL, Code, Flag, pMsg);
}

// ---------------------------------------------------------------------------
// WndProcRetHookFunc() -- WH_CALLWNDPROCRET hook callback.
//
// Fires inside the UO thread after the UO window procedure has processed a
// message.  Used for messages that must be handled after the UO wndproc runs
// (e.g., WM_NCPAINT / WM_NCACTIVATE for the custom title bar) where we draw
// on top of whatever the UO wndproc painted.
//
// Unlike GetMsgHookFunc, the MSG pointer here is not modifiable (CWPRETSTRUCT).
// MessageProc() receives NULL for pMsg to indicate this.
// ---------------------------------------------------------------------------
LRESULT CALLBACK WndProcRetHookFunc(int Code, WPARAM Flag, LPARAM pMsg)
{
	if (Code >= 0)
	{
		CWPRETSTRUCT* Msg = (CWPRETSTRUCT*)(pMsg);
		Log("WndProcRetHookFunc: Code=%d msg=0x%x hwnd=0x%x", Code, Msg->message, Msg->hwnd);
		/*
		Msg->message ^= 0x11;
		Msg->message ^= Disabled * 101;
		Msg->message *= !(Disabled * 020);
		Msg->message ^= 0x11;
		*/
		if (Msg->hwnd == hUOWindow || (hUOWindow == NULL && Msg->message == WM_PROCREADY))
			MessageProc(Msg->hwnd, Msg->message, Msg->wParam, Msg->lParam, NULL);
	}

	return CallNextHookEx(NULL, Code, Flag, pMsg);
}

// ---------------------------------------------------------------------------
// UOAWndProc() -- window procedure for the "UOASSIST-TP-MSG-WND" helper window.
//
// Razor creates this window class in InstallLibrary() to receive forwarded
// UOAssist-compatible messages (WM_USER+200 .. WM_USER+314).  Any message in
// that range is forwarded to hRazorWnd; all others get default processing.
// ---------------------------------------------------------------------------
LRESULT CALLBACK UOAWndProc(HWND hWnd, UINT nMsg, WPARAM wParam, LPARAM lParam)
{
	if (nMsg >= WM_USER + 200 && nMsg < WM_USER + 315)
		return SendMessage(hRazorWnd, nMsg, wParam, lParam); // Forward to Razor.
	else
		return DefWindowProc(hWnd, nMsg, wParam, lParam);
}

// ---------------------------------------------------------------------------
// Log() -- optional debug logging to C:\Crypt.log.
//
// Only active when _DEBUG and LOGGING are both defined at compile time.
// Each call appends a timestamped line.  Safe to call from any thread.
// ---------------------------------------------------------------------------
void Log(const char* format, ...)
{
#ifdef _DEBUG
#ifdef LOGGING
	FILE* log = fopen("C:\\RazorCrypt.log", "a");
	if (log)
	{
		char timeStr[256];
		struct tm* newtime;
		time_t aclock;

		time(&aclock);
		newtime = localtime(&aclock);
		strncpy(timeStr, asctime(newtime), 256);
		int len = (int)strlen(timeStr);
		if (timeStr[len - 1] == '\n')
			timeStr[len - 1] = 0;

		char OutTxt[512];
		va_list argList;
		va_start(argList, format);
		_vsnprintf(OutTxt, 512, format, argList);
		va_end(argList);

		fprintf(log, "%s: %s\n", timeStr, OutTxt);
		fclose(log);
	}
#endif
#endif
}
