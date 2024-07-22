#pragma once

#define Color16to32(c16) (((c16) & 0x7C00) >> 7) | (((c16) & 0x3E0) << 6) | (((c16) & 0x1F) << 19)

#include <stdint.h>

#pragma pack(push, 1)
struct UOItem
{
	int RealWidth;
	int RealHeight;

	int Left;
	int Top;
	int Bottom;
	int Right;

	int GetWidth() const { return Right - Left; }
	int GetHeight() const { return Bottom - Top; }

	unsigned short **Data; // [ReadlWidth][RealHeight] 32bit color data

	int ItemID;
	UOItem *pNext;
};

struct ArtIdx
{
	long FilePos;
	long Length;
	long Unused;
};

struct ArtHeader
{
	long Unknown;
	short Width;
	short Height;
	//followed by short LookupTable
};

// uop structures
/*
[2] Block Header
DWORD -> Number of files in this block
QWORD -> Address of the next block
*/
struct BlockHeader
{
	int32_t num_files;
	int64_t next_address;
};

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

struct FormatHeader
{
	CHAR magic[4];
	DWORD version;
	DWORD signature;
	int64_t first_address;
	DWORD block_count;
	DWORD num_files;
} ;

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
struct FileHeader
{
	int64_t data_header_address;
	int32_t length;
	int32_t compressed_size;
	int32_t uncompressed_size;
	uint64_t hash;
	int32_t adler_value;
	int16_t zlib_quality;
};

struct Entry3D
{
	long FilePos;
	long Length;
};
#pragma pack(pop)

