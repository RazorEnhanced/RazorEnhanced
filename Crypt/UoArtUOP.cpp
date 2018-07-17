#include "stdafx.h"
#include "UOArtUop.h"

#pragma comment(lib, "Shlwapi.lib")

BOOL UOArtUop::_loaded = false;
CHAR UOArtUop::_dataPath[MAX_PATH];
std::map<INT32, struct UOArtUop::Entry3D> UOArtUop::_index;
BOOL UOArtUop::_isUOPFormat = false;

BOOL UOArtUop::Init(LPCSTR datapath)
{
	CHAR UOPPath[MAX_PATH];
	CHAR MULPath[MAX_PATH];

	strcpy_s(_dataPath, MAX_PATH, datapath);
	_loaded = false;

	PathCombineA(UOPPath, _dataPath, "artLegacyMUL.uop");
	PathCombineA(MULPath, _dataPath, "artidx.mul");

	if (GetFileAttributesA(UOPPath) != -1)
	{
		LoadUOP(UOPPath);
		_loaded = true;
		_isUOPFormat = true;
	}
	else if (GetFileAttributesA(MULPath) != -1)
	{
		LoadMUL(MULPath);
		_loaded = true;
		_isUOPFormat = false;
	}

	return _loaded;
}

BOOL UOArtUop::LoadMUL(LPCSTR fileName)
{
	HANDLE fileHandle;

	fileHandle = CreateFileA(fileName, GENERIC_READ, FILE_SHARE_READ | FILE_SHARE_WRITE, NULL, OPEN_EXISTING, FILE_ATTRIBUTE_NORMAL, NULL);
	if (fileHandle == INVALID_HANDLE_VALUE)
	{
		return false;
	}

	DWORD bytesRead;
	ENTRY3D entry3d;
	int index = 0;
	do {
		ReadFile(fileHandle, &entry3d, sizeof(ENTRY3D), &bytesRead, NULL);
		_index[index++] = entry3d;
		//LogPrintf("UOArtUop::LoadMUL() _index size = %d\r\n", _index.size());
	} while (bytesRead > 0);

	CloseHandle(fileHandle);
	return true;
}

BOOL UOArtUop::LoadUOP(LPCSTR fileName)
{
	HANDLE fileHandle;

	fileHandle = CreateFileA(fileName, GENERIC_READ, FILE_SHARE_READ | FILE_SHARE_WRITE, NULL, OPEN_EXISTING, FILE_ATTRIBUTE_NORMAL, NULL);

	if (fileHandle == INVALID_HANDLE_VALUE)
		return false;

	FORMATHEADER formatHeader;
	DWORD bytesRead;
	ReadFile(fileHandle, &formatHeader, sizeof(FORMATHEADER), &bytesRead, NULL);

	std::map<INT64, INT32> hashes;

	PCHAR tmp = new CHAR[256];
	for (int i = 0; i < 0x13FDC; i++)
	{
		sprintf_s(tmp, 256, "build/artlegacymul/%08d.tga", i);
		UINT64 hash = HashFileName(tmp);
		if (!hashes.count(hash)) {
			hashes[hash] = i;
		}
	}

	INT64 nextAddress = formatHeader.firstAddress;

	do
	{
		BLOCKHEADER blockHeader;
		LARGE_INTEGER li;
		li.QuadPart = nextAddress;

		SetFilePointer(fileHandle, li.LowPart, &li.HighPart, FILE_BEGIN);
		ReadFile(fileHandle, &blockHeader, sizeof(BLOCKHEADER), &bytesRead, NULL);

		for (UINT i = 0; i < blockHeader.numFiles; i++)
		{
			FILEHEADER fileHeader;
			ReadFile(fileHandle, &fileHeader, sizeof(FILEHEADER), &bytesRead, NULL);
			if (fileHeader.dataHeaderAddress == 0)
				continue;
			if (hashes.count(fileHeader.hash))
			{
				int ii = hashes[fileHeader.hash];
				_index[ii].lookup = (int)(fileHeader.dataHeaderAddress + fileHeader.length);
				_index[ii].length = fileHeader.isCompressed ? fileHeader.compressedSize : fileHeader.uncompressedSize;
			}
		}
		nextAddress = blockHeader.nextAddress;
	} while (nextAddress > 0);

	hashes.clear();
	CloseHandle(fileHandle);
	return true;
}

UOArtUop::IMAGEDATA *UOArtUop::LoadStatic(int itemId)
{
	CHAR fileName[MAX_PATH];
	HANDLE fileHandle;

	if (!_loaded)
		return NULL;

	itemId += 0x4000;

	PathCombineA(fileName, _dataPath, "art.mul");

	if (_isUOPFormat)
		PathCombineA(fileName, _dataPath, "artLegacyMUL.uop");

	fileHandle = CreateFileA(fileName, GENERIC_READ, FILE_SHARE_READ | FILE_SHARE_WRITE, NULL, OPEN_EXISTING, FILE_ATTRIBUTE_NORMAL, NULL);

	if (fileHandle == INVALID_HANDLE_VALUE)
		return NULL;

	int lookup = _index[itemId].lookup;
	int length = _index[itemId].length;

	PBYTE rawData = new BYTE[length];

	SetFilePointer(fileHandle, lookup, NULL, FILE_BEGIN);

	DWORD bytesRead = 0;
	ReadFile(fileHandle, rawData, length, &bytesRead, NULL);

	USHORT* bindata = (USHORT*)rawData;
	int count = 2;

	int width = bindata[count++];
	int height = bindata[count++];


	int *lookups = new int[height];
	int start = (height + 4);

	for (int i = 0; i < height; i++)
		lookups[i] = (int)(start + (bindata[count++]));

	PUSHORT image = new USHORT[width*height];
	memset(image, 0x0, (width*height) * 2);

	int smallestX, largestX = 0;
	smallestX = width;

	int smallestY, largestY = 0;
	smallestY = height;

	int delta = width;
	USHORT* line = (USHORT*)image;

	for (int y = 0; y < height; y++, line += delta)
	{
		count = lookups[y];
		USHORT runOffset = 0, runLength = 0;

		USHORT* cur = line;
		USHORT* end;
		int x = 0;

		while (true)
		{

			runOffset = bindata[count++];
			runLength = bindata[count++];

			if (runOffset + runLength == 0)
			{
				break;
			}

			if (runLength == 0 || runOffset > delta)
				break;

			if (y > largestY)
				largestY = y + 1;

			if (y < smallestY)
				smallestY = y;

			cur += runOffset;

			if (runOffset + runLength > delta)
				break;

			x += runOffset;

			if (x < smallestX)
				smallestX = x;

			end = cur + runLength;

			while (cur < end)
			{
				*(cur++) = (USHORT)(bindata[count++] ^ 0x8000);
				x++;
			}

			if (x > largestX)
				largestX = x;
		}
	}

	int realWidth = largestX - smallestX;
	int realHeight = largestY - smallestY;

	BITMAPINFO bmi = { 0 };
	bmi.bmiHeader.biPlanes = 1;
	bmi.bmiHeader.biBitCount = 16;
	bmi.bmiHeader.biWidth = width;
	bmi.bmiHeader.biHeight = -height;
	bmi.bmiHeader.biSize = sizeof(BITMAPINFO);

	UINT *pixels;
	HBITMAP bitmap = CreateDIBSection(GetWindowDC(GetDesktopWindow()), &bmi, DIB_RGB_COLORS, (void**)&pixels, NULL, 0);
	memcpy(pixels, image, (width*height) * 2);

	HDC hSrc = CreateCompatibleDC(0);
	SelectObject(hSrc, bitmap);
	HDC hDest = CreateCompatibleDC(hSrc);
	HBITMAP realBitmap = CreateCompatibleBitmap(hSrc, realWidth, realHeight);
	SelectObject(hDest, realBitmap);

	BitBlt(hDest, 0, 0, realWidth, realHeight, hSrc, smallestX, smallestY, SRCCOPY);

	DeleteDC(hSrc);
	DeleteDC(hDest);

	CloseHandle(fileHandle);

	IMAGEDATA *imageData = new IMAGEDATA;
	imageData->originalWidth = width;
	imageData->originalHeight = height;
	imageData->originalBitmap = bitmap;
	imageData->width = realWidth;
	imageData->height = realHeight;
	imageData->bitmap = realBitmap;

	return imageData;
}

INT64 UOArtUop::HashFileName(PCHAR s)
{
	UINT eax, ecx, edx, ebx, esi, edi;
	DWORD length = strlen(s);

	eax = ecx = edx = ebx = esi = edi = 0;
	ebx = edi = esi = (UINT)length + 0xDEADBEEF;

	UINT i = 0;

	for (i = 0; i + 12 < length; i += 12)
	{
		edi = (UINT)((s[i + 7] << 24) | (s[i + 6] << 16) | (s[i + 5] << 8) | s[i + 4]) + edi;
		esi = (UINT)((s[i + 11] << 24) | (s[i + 10] << 16) | (s[i + 9] << 8) | s[i + 8]) + esi;
		edx = (UINT)((s[i + 3] << 24) | (s[i + 2] << 16) | (s[i + 1] << 8) | s[i]) - esi;

		edx = (edx + ebx) ^ (esi >> 28) ^ (esi << 4);
		esi += edi;
		edi = (edi - edx) ^ (edx >> 26) ^ (edx << 6);
		edx += esi;
		esi = (esi - edi) ^ (edi >> 24) ^ (edi << 8);
		edi += edx;
		ebx = (edx - esi) ^ (esi >> 16) ^ (esi << 16);
		esi += edi;
		edi = (edi - ebx) ^ (ebx >> 13) ^ (ebx << 19);
		ebx += esi;
		esi = (esi - edi) ^ (edi >> 28) ^ (edi << 4);
		edi += ebx;
	}

	if (length - i > 0)
	{
		switch (length - i)
		{
		case 12:
			esi += (UINT)s[i + 11] << 24;
		case 11:
			esi += (UINT)s[i + 10] << 16;
		case 10:
			esi += (UINT)s[i + 9] << 8;
		case 9:
			esi += (UINT)s[i + 8];
		case 8:
			edi += (UINT)s[i + 7] << 24;
		case 7:
			edi += (UINT)s[i + 6] << 16;
		case 6:
			edi += (UINT)s[i + 5] << 8;
		case 5:
			edi += (UINT)s[i + 4];
		case 4:
			ebx += (UINT)s[i + 3] << 24;
		case 3:
			ebx += (UINT)s[i + 2] << 16;
		case 2:
			ebx += (UINT)s[i + 1] << 8;
		case 1:
			ebx += (UINT)s[i];
			break;
		}

		esi = (esi ^ edi) - ((edi >> 18) ^ (edi << 14));
		ecx = (esi ^ ebx) - ((esi >> 21) ^ (esi << 11));
		edi = (edi ^ ecx) - ((ecx >> 7) ^ (ecx << 25));
		esi = (esi ^ edi) - ((edi >> 16) ^ (edi << 16));
		edx = (esi ^ ecx) - ((esi >> 28) ^ (esi << 4));
		edi = (edi ^ edx) - ((edx >> 18) ^ (edx << 14));
		eax = (esi ^ edi) - ((edi >> 8) ^ (edi << 24));

		return ((INT64)edi << 32) | eax;
	}

	return ((INT64)esi << 32) | eax;
}