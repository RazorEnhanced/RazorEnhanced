#pragma once
#pragma pack(1)
#include "stdafx.h"
#include "Shlwapi.h"

class UOArtUop
{
private:
	/*
	[1] Format Header
	BYTE -> 'M'
	BYTE -> 'Y'
	BYTE -> 'P'
	BYTE -> 0
	DWORD -> Version
	DWORD -> Signature?
	QWORD -> Address of the first [2] Block
	DWORD -> Max number of files per block
	DWORD -> Number of files in this package
	BYTE[]-> 0
	*/
	typedef struct FormatHeader
	{
		CHAR magic[4];
		DWORD version;
		DWORD signature;
		INT64 firstAddress;
		DWORD maxFiles;
		DWORD numFiles;
	} FORMATHEADER;

	/*
	[2] Block Header
	DWORD -> Number of files in this block
	QWORD -> Address of the next block
	*/
	typedef struct BlockHeader
	{
		DWORD numFiles;
		INT64 nextAddress;
	} BLOCKHEADER;

	/*
	[3] File Header
	QWORD -> Address of [4] Data Header
	DWORD -> Length of file header
	DWORD -> Size of compressed file
	DWORD -> Size of decompressed file
	QWORD -> File hash
	DWORD -> Adler32 of [4a] Data Header in little endian, unknown in Version 5
	WORD -> Compression type (0 - no compression, 1 - zlib)
	*/
	typedef struct FileHeader
	{
		INT64 dataHeaderAddress;
		DWORD length;
		DWORD compressedSize;
		DWORD uncompressedSize;
		UINT64 hash;
		DWORD unknown;
		INT16 isCompressed;
	} FILEHEADER;

	typedef struct Entry3D
	{
		int lookup;
		int length;
		int extra;
	} ENTRY3D;

	typedef struct ImageData
	{
		int originalWidth;
		int originalHeight;
		HBITMAP originalBitmap;
		int width;
		int height;
		HBITMAP bitmap;
	} IMAGEDATA;

	static CHAR _dataPath[MAX_PATH];
	static BOOL _loaded;
	static std::map<INT32, struct Entry3D> _index;
	static BOOL _isUOPFormat;
public:
	static BOOL Init(LPCSTR dataPath);
	static BOOL LoadUOP(LPCSTR fileName);
	static BOOL LoadMUL(LPCSTR dataPath);
	static INT64 HashFileName(PCHAR s);
	static IMAGEDATA *LoadStatic(int itemId);
};