#pragma once
#pragma pack(1)

// Crypt.h -- Main header for Crypt.dll
//
// Crypt.dll is injected into the Ultima Online client process. Once inside, it:
//   1. Hooks Winsock functions (send/recv/connect/closesocket/select) by patching
//      the client's PE import address table (IAT) so that every network call goes
//      through our replacement functions first.
//   2. Decrypts all traffic using LoginEncryption (XOR stream cipher, login phase)
//      or OSIEncryption (Twofish + MD5-XOR stream, game phase).
//   3. Places decrypted data into a named shared-memory region (SharedMemory below)
//      that Razor / UO_CoPilot reads via MapViewOfFile.
//   4. Communicates events (CONNECT, RECV, SEND, DISCONNECT, ...) back to Razor's
//      window via PostMessage / SendMessage using the UONET_MESSAGE codes below.
//
// Two injection paths exist:
//   * OnAttach()        -- called very early, before the UO window exists, to
//                          scan memory for encryption keys / packet table.
//   * InstallLibrary()  -- called by Razor after the UO window is visible; installs
//                          the Windows message hooks (WH_CALLWNDPROCRET / WH_GETMESSAGE)
//                          that drive the message-processing loop.

// Exported DLL version strings used for compatibility checks with Razor.
#define DLL_VERSION "0.6.62"
#define DLL_VERSION_OLD "1.0.14"  // Not change IT!

// Convenience macros so every exported symbol has __declspec(dllexport).
#define DLLFUNCTION __declspec(dllexport)
#define DLLVAR DLLFUNCTION

#ifdef _DEBUG
  //#define LOGGING   // Uncomment to enable file logging in debug builds.
#endif

// ---------------------------------------------------------------------------
// IError -- return codes from InstallLibrary() and related setup functions.
// ---------------------------------------------------------------------------
enum IError
{
	SUCCESS,       // Everything went fine.
	NO_UOWND,      // Could not find the UO client window (FindWindow failed).
	NO_TID,        // Could not get the UO thread/process ID.
	NO_HOOK,       // SetWindowsHookEx failed (e.g., insufficient permissions).
	NO_SHAREMEM,   // CreateFileMapping / MapViewOfFile failed.
	LIB_DISABLED,  // Library has been administratively disabled.
	NO_PATCH,      // IAT patching of Winsock functions failed.
	NO_COPY,       // Memory scan for the packet table or crypto keys failed.
	INVALID_PARAMS,// Caller passed bad arguments.

	UNKNOWN,       // Unclassified failure.
};

// ---------------------------------------------------------------------------
// UONET_MESSAGE -- IPC message identifiers sent via PostMessage / WM_UONETEVENT
// to Razor's window so it knows what just happened on the network.
// ---------------------------------------------------------------------------
enum UONET_MESSAGE
{
	SEND = 1,         // Outgoing (client -> server) packet data is ready in InSend.
	RECV = 2,         // Incoming (server -> client) packet data is ready in InRecv.
	READY = 3,        // DLL is fully initialised and hooks are in place.
	NOT_READY = 4,    // Initialisation failed; lParam carries the IError code.
	CONNECT = 5,      // Socket connected; lParam is the server IP address.
	DISCONNECT = 6,   // Socket closed or reset.
	KEYDOWN = 7,      // Key press forwarded from the UO window.
	MOUSE = 8,        // Mouse wheel / middle / X-button event.

	ACTIVATE = 9,     // WM_ACTIVATE forwarded (UO window active/inactive).
	FOCUS = 10,       // WM_SETFOCUS / WM_KILLFOCUS forwarded.

	CLOSE = 11,       // DLL is being unloaded (DllMain DLL_PROCESS_DETACH).
	STAT_BAR = 12,    // Request to draw the custom stat bar.
	NOTO_HUE = 13,    // Set a custom notoriety hue value in client memory.
	DLL_ERROR = 14,   // An internal error occurred; lParam carries IError.

	DEATH_MSG = 15,   // Patch the "You are dead." string in the client.

	CALIBRATE_POS = 16, // Scan client memory to find the player position structure.
	GET_POS = 17,       // Read the player position from the located structure.

	OPEN_RPV = 18,    // Open the Razor packet viewer.

	SETWNDSIZE = 19,  // Resize the UO client viewport.

	FINDDATA = 20,    // Memory scan for a user-supplied data pattern.

	SMART_CPU = 21,   // Enable/disable SmartCPU (30 fps cap via select() timeout).
	NEGOTIATE = 22,   // Toggle server-feature negotiation (AuthBits handshake).
	SET_MAP_HWND = 23,// Register the map overlay window so focus events are correct.

	// ZIPPY REV 80	SET_FWD_HWND = 24,  // (unused, preserved for ABI compat)
};

// ---------------------------------------------------------------------------
// Buffer -- one ring-buffer slot inside SharedMemory.
//
// Start  : byte offset within Buff[] where valid data begins.
// Length : number of valid bytes starting at Buff[Start].
// Buff[] : raw storage; 512 KB per direction per side = ~2 MB total.
//
// When Start drifts past the halfway point, Maintenance() shifts the live
// bytes back to offset 0 to prevent running off the end of the array.
// ---------------------------------------------------------------------------
//#define SHARED_BUFF_SIZE 0x80000 // Client's buffers are 500k
#define SHARED_BUFF_SIZE 524288 // 262144 // 250k
struct Buffer
{
	int Length;              // Number of valid bytes currently in Buff[Start..].
	int Start;               // Index into Buff[] of the first valid byte.
	BYTE Buff[SHARED_BUFF_SIZE]; // Raw packet data (plaintext after decryption).
};

// ---------------------------------------------------------------------------
// SharedMemory -- the 2.1 MB named file mapping that Razor reads.
//
// Created by CreateSharedMemory() as "UONetSharedFM_<pid>" with PAGE_READWRITE.
// A named mutex "UONetSharedCOMM_<pid>" serialises all accesses.
//
// Field layout (do NOT reorder -- Razor expects exact offsets):
//   InRecv  : Decrypted data received FROM the server (written by HookRecv).
//   OutRecv : Plaintext data that Razor wants TO INJECT as if the server sent it.
//   InSend  : Decrypted data sent BY the client (written by HookSend).
//   OutSend : Plaintext data that Razor wants to SEND on behalf of the client.
// ---------------------------------------------------------------------------
struct SharedMemory
{
	// Do *not* mess with this struct.  Really.  I mean it.
	Buffer InRecv;               // Server -> client (decrypted), ready for Razor to read.
	Buffer OutRecv;              // Razor -> client (will be compressed/encrypted by HookRecv).
	Buffer InSend;               // Client -> server (decrypted), ready for Razor to read.
	Buffer OutSend;              // Razor -> server (will be encrypted by FlushSendData).

	char TitleBar[1024];         // Custom title-bar text with embedded markup (color/icon tags).
	bool ForceDisconn;           // If true, HookRecv returns WSAECONNRESET to force a disconnect.
	bool AllowDisconn;           // Gates whether ForceDisconn actually fires (can be paused).
	unsigned int TotalSend;      // Running byte count of outgoing data (approximate, not mutex'd).
	unsigned int TotalRecv;      // Running byte count of incoming data.
	unsigned short PacketTable[256]; // Per-packet-ID byte lengths; 0x8000+ = dynamic (read word).
	char DataPath[256];          // Filesystem path to the UO data directory (art.mul, hues.mul, ...).
	char DeathMsg[16];           // Replacement text for the "You are dead." string in client memory.
	int Position[3];             // Player position (calibrated via CALIBRATE_POS): [z, y, x_or_addr].
	unsigned char CheatKey[16];  // Anti-cheat token embedded in character-selection packets.
	bool AllowNegotiate;         // If true, DLL participates in the AuthBits negotiation handshake.
	unsigned char AuthBits[8];   // 8-byte authentication token extracted from the character list (0xA9).
	bool IsHaxed;                // Internal flag; set false on InstallLibrary, true if tamper detected.
	unsigned int ServerIP;       // Target server IPv4 address (used to redirect HookConnect).
	unsigned short ServerPort;   // Target server port.
	char UOVersion[16];          // UO client version string from the native GetUOVersion function.
};

// ---------------------------------------------------------------------------
// PatchInfo -- RAII snapshot of a memory range before it is patched.
// Stores the original bytes so the patch can be reversed if needed.
// ---------------------------------------------------------------------------
class PatchInfo
{
public:
	// Snapshot 'len' bytes starting at 'addr'.
	PatchInfo(DWORD addr, int len)
	{
		Address = addr;
		Length = len;
		Data = new char[Length];
		memcpy(Data, (const void*)Address, Length);
	}

	~PatchInfo()
	{
		delete[] Data;
	}

	DWORD Address; // Address of the patched region.
	int Length;    // Number of bytes saved / patched.
	char* Data;    // Original bytes before patching.
};

// ---------------------------------------------------------------------------
// Windows user-message IDs used for Razor <-> DLL communication.
// WM_USER is the first application-specific message number.
// ---------------------------------------------------------------------------
#define WM_PROCREADY WM_USER       // Sent from InstallLibrary() to the UO window to trigger DLL init.
#define WM_UONETEVENT WM_USER+1    // General IPC event; wParam = UONET_MESSAGE code, lParam = data.
#define WM_CUSTOMTITLE WM_USER+2   // Razor asks the DLL to repaint the custom title bar.
#define WM_UOA_MSG WM_USER+3       // Forwarded UOAssist-compatible messages.
// ZIPPY REV 80#define WM_SETFWDWND WM_USER+4
// ZIPPY REV 80#define WM_FWDPACKET WM_USER+5

// WM_XBUTTONDOWN may not be defined in older SDKs.
#ifndef WM_XBUTTONDOWN
#define WM_XBUTTONDOWN                  0x020B
#endif

// ---------------------------------------------------------------------------
// Globals declared in Crypt.cpp, shared across translation units.
// ---------------------------------------------------------------------------
extern HWND hUOWindow;      // Handle to the UO client window.
extern HINSTANCE hInstance; // Handle to this DLL module.
extern SharedMemory* pShared;// Pointer to the mapped shared memory region.
extern HANDLE CommMutex;    // Named mutex guarding pShared accesses.

// ---------------------------------------------------------------------------
// Exported API used by Razor / UO_CoPilot.
// ---------------------------------------------------------------------------
DLLFUNCTION int InstallLibrary(HWND PostWindow, DWORD pId); // Setup hooks, shared mem, window hooks.
DLLFUNCTION void Shutdown(bool closeClient);                 // Tear down hooks, optionally kill client.
DLLFUNCTION HWND FindUOWindow();                             // Locate the UO client window by class name.
DLLFUNCTION void* GetSharedAddress();                        // Return the pShared pointer to Razor.
DLLFUNCTION int GetPacketLength(unsigned char* data, int len);// Decode packet length from the table.
DLLFUNCTION bool IsDynLength(unsigned char packet);          // True if the packet has a dynamic length.
DLLFUNCTION int GetUOProcId();                               // Return the UO process ID.
DLLFUNCTION DWORD InitializeLibrary(const char*);           // Alternate init path (legacy).
DLLFUNCTION HANDLE GetCommMutex();                           // Return CommMutex to Razor.

// Internal callback prototypes (installed via SetWindowsHookEx).
LRESULT CALLBACK UOAWndProc(HWND, UINT, WPARAM, LPARAM);    // Window proc for the helper message window.
void Log(const char* format, ...);                           // Debug logging (only active if LOGGING is defined).
void MemoryPatch(unsigned long, unsigned long);               // Overwrite a DWORD in client memory (VirtualProtect).
void MemoryPatch(unsigned long, int, int);                   // Overwrite N bytes with an integer value.
void MemoryPatch(unsigned long, const void*, int);          // Overwrite N bytes with arbitrary data.
void RedrawTitleBar(HWND, bool);                             // Repaint the custom title bar for the UO window.
void CheckTitlebarAttr(HWND);                                // Toggle DWM non-client rendering when title bar is active.
void FreeArt();                                              // Release all cached UO art items from memory.
void InitThemes();                                           // Dynamically load uxtheme.dll / dwmapi.dll for theming.
bool PatchStatusBar(BOOL preAOS);                            // Install the custom HP/mana/stam status bar hook.

// ---------------------------------------------------------------------------
// Pattern strings used by MemFinder to locate data in client memory.
// ---------------------------------------------------------------------------

// PACKET_TBL_STR -- two adjacent DWORDs (7 and 3) that appear immediately
// before the ClientPacketInfo array.  The offset PACKET_TBL_OFFSET is
// subtracted from the match address to reach the table base.
//#define PACKET_TBL_STR "Got Logout OK packet!\0\0\0"
//#define PACKET_TS_LEN 24
#define PACKET_TBL_STR "\x07\0\0\0\x03\0\0\0"
#define PACKET_TS_LEN 8
#define PACKET_TBL_OFFSET (0-(8+12+12))  // Bytes before the match where the table actually starts.

// CRYPT_KEY_STR -- disassembly signature for the 2D client login-key update loop.
// The two DWORDs at addr+CRYPT_KEY_LEN are Key1 and Key1+6 bytes later is Key2.
//search disassembly for
//static key1 C1 E2 1F D1 E8 D1 E9 0B C6 0B CA 35 static key2 81 F1 dynamic key 4D
#define CRYPT_KEY_STR "\xC1\xE2\x1F\xD1\xE8\xD1\xE9\x0B\xC6\x0B\xCA\x35"
#define CRYPT_KEY_LEN 12

// CRYPT_KEY_STR_3D -- login-key pattern for the 3D (Third Dawn) client.
//static key1 D1 E8 0B C6 C1 E2 1F 35 static key2 D1 E9 89 83 F0 00 42 00 8B 45 08 0B CA 81 F1 dynamic key 48
#define CRYPT_KEY_STR_3D "\xD1\xE8\x0B\xC6\xC1\xE2\x1F\x35"
#define CRYPT_KEY_3D_LEN 8

/* Login key derivation formulae (for reference, not used at runtime):
key1 = ( Major << 23 ) | ( Minor << 14 ) | ( Revision << 4 );
key1 ^= ( Revision * Revision ) << 9;
key1 ^= ( Minor * Minor );
key1 ^= ( Minor * 11 ) << 24;
key1 ^= ( Revision * 7 ) << 19;
key1 ^= 0x2C13A5FD;
key2 = ( Major << 22 ) | ( Revision << 13 ) | ( Minor << 3 );
key2 ^= ( Revision * Revision * 3 ) << 10;
key2 ^= ( Minor * Minor );
key2 ^= ( Minor * 13 ) << 23;
key2 ^= ( Revision * 7 ) << 18;
key2 ^= 0xA31D527F;
*/
/*
.text:0041AA2F C1 E6 1F                          shl     esi, 31
.text:0041AA32 D1 E8                             shr     eax, 1
.text:0041AA34 0B C6                             or      eax, esi
.text:0041AA36 47                                inc     edi
.text:0041AA37 33 05 BC 29 6B 00                 xor     eax, LoginKey_2
.text:0041AA3D C1 E2 1F                          shl     edx, 31
.text:0041AA40 89 83 F8 00 0A 00                 mov     [ebx+0A00F8h], eax
.text:0041AA46 D1 E8                             shr     eax, 1
.text:0041AA48 0B C6                             or      eax, esi
.text:0041AA4A 8B 35 BC 29 6B 00                 mov     esi, LoginKey_2
.text:0041AA50 33 C6                             xor     eax, esi
.text:0041AA52 D1 E9                             shr     ecx, 1
.text:0041AA54 89 83 F8 00 0A 00                 mov     [ebx+0A00F8h], eax
.text:0041AA5A 0B CA                             or      ecx, edx
.text:0041AA5C 8B 15 B8 29 6B 00                 mov     edx, LoginKey_1
*/
// -- -- -- -- -- --
// 1F D1 E8 0B C6 47 33 05 memoryloc_2 C1 E2 1F 89 83 F8 00 0A 00 D1 E8 0B C6 8B 35 memoryloc_2 33 C6 D1 E9 89 83 F8 00 0A 00 0B Ca 8b 15 memoryloc_1 33 CA
// Pattern for newer 2D client builds where the key XOR pattern shifted slightly.
#define CRYPT_KEY_STR_NEW "\x1F\xD1\xE8\x0B\xC6\x47\x33\x05"
#define CRYPT_KEY_NEW_LEN 8

/*
.text:0041C599 8B F0                             mov     esi, eax
.text:0041C59B 8B FA                             mov     edi, edx
.text:0041C59D D1 E8                             shr     eax, 1
.text:0041C59F C1 E7 1F                          shl     edi, 31
.text:0041C5A2 0B C7                             or      eax, edi
.text:0041C5A4 33 05 FC AB 6C 00                 xor     eax, dword_6CABFC
.text:0041C5AA D1 EA                             shr     edx, 1
.text:0041C5AC D1 E8                             shr     eax, 1
.text:0041C5AE C1 E6 1F                          shl     esi, 31
.text:0041C5B1 0B C7                             or      eax, edi
.text:0041C5B3 33 05 FC AB 6C 00                 xor     eax, dword_6CABFC
.text:0041C5B9 0B D6                             or      edx, esi
.text:0041C5BB 33 15 00 AC 6C 00                 xor     edx, dword_6CAC00
.text:0041C5C1 83 EB 01                          sub     ebx, 1
.text:0041C5C4 83 C5 01                          add     ebp, 1
.text:0041C5C7 85 DB                             test    ebx, ebx
*/
// E8 C1 E7 1F 0B C7 33 05 memoryloc_2
// Pattern for even newer 2D client versions (slightly different opcode sequencing).
#define CRYPT_KEY_STR_MORE_NEW "\xE8\xC1\xE7\x1F\x0B\xC7\x33\x05"
#define CRYPT_KEY_MORE_NEW_LEN 8

// ---------------------------------------------------------------------------
// Smooth-animation and speed-hack byte patterns (currently commented out in
// OnAttach but left here for reference / future re-enabling).
// ---------------------------------------------------------------------------
#define ANIM_PATTERN_1 "\x55\x68"
// \xn\xn\xn\xn
#define ANIM_PATTERN_2 "\x68"
// \xn\xn\xn\xn
#define ANIM_PATTERN_3 "\xE8"
// \xn\xn\xn\xn
#define ANIM_PATTERN_4 "\x8B\xE8\x83\xC4\x08\x85\xED\x75"
// \xn
#define ANIM_PATTERN_5 "\xB8"
// \xn\xn\xn\xn
#define ANIM_PATTERN_6 "\x8B\xFF\xC6\x40\xFF\x04\xC6\x00\x02\x83\xC0\x02\x3D"
// \xn\xn\xn\xn

#define ANIM_PATTERN_LENGTH_1 2
#define ANIM_PATTERN_LENGTH_2 1
#define ANIM_PATTERN_LENGTH_3 1
#define ANIM_PATTERN_LENGTH_4 8
#define ANIM_PATTERN_LENGTH_5 1
#define ANIM_PATTERN_LENGTH_6 13

// #define ANIM_PATTERN_LENGTH 47

/*var animPattern = new byte?[]
{
0x55,                               // push ebp
0x68, null, null, null, null,       // push offset
0x68, null, null, null, null,       // push offset
0xe8, null, null, null, null,       // call offset
0x8b, 0xe8,                         // mov ebp, eax
0x83, 0xc4, 0x08,                   // add esp, 8
0x85, 0xed,                         // test, ebp, ebp
0x75, null,                         // jnz short
0xb8, null, null, null, null,       // mov eax, offset
0x8b, 0xff,                         // mov edi, edi
0xc6, 0x40, 0xff, 0x04,             // mov [eax-1], 4
0xc6, 0x00, 0x02,                   // mov [eax], 2
0x83, 0xc0, 0x02,                   // add eax, 2
0x3d, null, null, null, null        // cmp eax, dword
};*/

// Speed-hack pattern: locates the frame-rate limiter compare instruction.
#define SPEEDHACK_PATTERN_1 "\x8B\xFE\x2B\x3D"
// \xn\xn\xn\xn
#define SPEEDHACK_PATTERN_2 "\x83\xFF"
// \xn
#define SPEEDHACK_PATTERN_3 "\x0F\x82"
//\xn\xn\xn\xn"

#define SPEEDHACK_PATTERN_LENGTH_1 4
#define SPEEDHACK_PATTERN_LENGTH_2 2
#define SPEEDHACK_PATTERN_LENGTH_3 2

// #define SPEEDHACK_PATTERN_LENGTH 17

/*var speedhackPattern = new byte?[]
{
0x8b, 0xfe,                          // mov edi, esi
0x2b, 0x3d, null, null, null, null,  // sub edi, far
0x83, 0xff, null,                    // cmp edi, byte
0x0f, 0x82, null, null, null, null,  // jb short
};*/

/*  var animAddress = MemoryTools.FindPattern(animPattern, Process.GetCurrentProcess().MainModule);
	var speedhackAddress = MemoryTools.FindPattern(speedhackPattern, Process.GetCurrentProcess().MainModule);
	if (speedhackAddress == IntPtr.Zero || animAddress == IntPtr.Zero)
	{
		Trace.WriteLine("Smooth movement offsets not found", "Patch manager");
		return;
	}

	using (new TemporaryVirtualProtect(animAddress, (uint)animPattern.Length, 0x0040))
	{
		MemoryManager.Write((void*)(animAddress.ToInt32() + 11), new byte[] {
			0x33, 0xc0, 0x90, 0x90, 0x90    // xor eax, eax
		});
		MemoryManager.Write((void*)(animAddress.ToInt32() + 35), new byte[] {
			0x08                            // mov [eax-1], 8
		});
		MemoryManager.Write((void*)(animAddress.ToInt32() + 38), new byte[] {
			0x04                            // mov [eax], 2
		});
	}
	using (new TemporaryVirtualProtect(speedhackAddress, (uint)speedhackPattern.Length, 0x0040))
	{
		MemoryManager.Write((void*)(speedhackAddress.ToInt32() + 10), new byte[] {
			0x26,                           // cmp edi, 0x26
		});
	}

 Trace.WriteLine("Smooth movement applied", "Patch manager");*/
