#include "stdafx.h"
#include "UOArt.h"
#include "Crypt.h"

#include <iostream>
#include <fstream>
#include <strstream>
#include <exception>

#include <stdint.h>
#include <stdio.h>
#include <stdlib.h>
#include <errno.h>
#include <sys/stat.h>

#include <map>
#include <memory>


namespace {

    UOItem *ArtCache = NULL;
    unsigned short **Hues = NULL;
    int NumHues = 0;
    static bool _loaded;
    static std::map<int32_t, struct Entry3D> _index;
}

int64_t HashFileName(char* s);

inline bool fileExists(const std::string fullPath)
{
    struct stat buf;
    return (stat(fullPath.c_str(), &buf) == 0);
}

inline long fileSize(const std::string& filename)
{
    struct stat stat_buf;
    int rc = stat(filename.c_str(), &stat_buf);
    return rc == 0 ? stat_buf.st_size : -1;
}

inline int Round(float n)
{
    int i = (int)n;
    return i + (n - i >= 0.5 ? 1 : 0);
}

unsigned short *GetHue(int index)
{
    if (Hues == NULL)
    {
        if (!pShared)
            return NULL;

        WaitForSingleObject(CommMutex, INFINITE);
        std::string path(pShared->DataPath);
        ReleaseMutex(CommMutex);
        path += "/hues.mul";

        int length = fileSize(path);

        int  blockCount, index;
        std::ifstream huesMul;
        huesMul.open(path.c_str(), std::ios::in | std::ios::binary);
        if (!huesMul.is_open())
        {
            Log("Failed to load hues file");
            Hues = new unsigned short *[1];
            Hues[0] = new unsigned short[34];
            memset(Hues[0], 0, 34 * 2);
            NumHues = 1;
            return NULL;
        }
        blockCount = length / 708;
        if (blockCount > 375)
            blockCount = 375;
        NumHues = blockCount * 8;

        Hues = new unsigned short *[NumHues];

        index = 0;
        for (int b = 0; b < blockCount; b++)
        {
            huesMul.seekg(4, std::ios_base::cur);

            for (int i = 0; i < 8; i++, index++)
            {
                Hues[index] = new unsigned short[34];
                for (int c = 0; c < 34; c++)
                {
                    unsigned short color;
                    huesMul.read((char*)&color, 2);
                    Hues[index][c] = color | 0x8000;
                }

                huesMul.seekg(20, std::ios_base::cur);// ignore name
            }
        }
        huesMul.close();
    }

    if (index > 0 && index <= NumHues)
        return Hues[index - 1];
    else
        return NULL;
}

unsigned short ApplyHueToPixel(unsigned short *hue, unsigned short pix)
{
    if (hue)
        return hue[(pix >> 10) & 31];
    else
        return pix;
}

void load_uop_pointers(const std::string& from_file)
{
    // even if fail don't try again
    _loaded = true;

    // Build hash to itemId map
    std::map<int64_t, INT32> hashes;

    char* tmp = new CHAR[256];
    for (int itemId = 0; itemId < 0x13FDC; itemId++)
    {
        sprintf_s(tmp, 256, "build/artlegacymul/%08d.tga", itemId);
        UINT64 hash = HashFileName(tmp);
        if (!hashes.count(hash)) {
            hashes[hash] = itemId;
        }
    }
    Log("built hash file");

    std::ifstream art_uop;
    art_uop.open(from_file.c_str(), std::ios::in | std::ios::binary);
    if (!art_uop.is_open())
    {

        std::strstream temp;
        temp << "Open Failed in load pointers for "
            << from_file.c_str()
            << std::ends;
        Log(temp.str());
        return;
    }

    long  bytes_read = 0;
    FormatHeader header;
    if (!art_uop.read((char*)&header, sizeof(header)))
    {
        Log("Unable to Read art uop header");
        return;
    }

    int64_t next_address = header.first_address;

    for (unsigned int block_count = 0; block_count < header.block_count; block_count++)
    {
        if (next_address == 0)
        {
            break;
        }

        if (!art_uop.seekg(next_address))
        {
            std::strstream temp;
            temp << "setpos Failed in load pointers for "
                << from_file.c_str()
                << " errno: " << errno
                << " errstr: " << strerror(errno)
                << std::ends;
            Log(temp.str());
            return;
        }

        BlockHeader block_header;
        if (!art_uop.read((char*)&block_header, sizeof(block_header)))
        {
            Log("block search failed");
            return;
        }

        for (int32_t i = 0; i < block_header.num_files; i++)
        {
            FileHeader fileHeader;
            if (!art_uop.read((char*)&fileHeader, sizeof(FileHeader)))
            {
                std::strstream temp;
                temp << "read file header failed for "
                    << from_file.c_str()
                    << " file_num: " << i
                    << " max_file: " << block_header.num_files
                    << " next_pos: 0X" << std::hex << next_address
                    << " block_count: " << block_count
                    << " max block count: " << header.block_count
                    << std::ends;
                Log(temp.str());
                return;
            }
            if (fileHeader.data_header_address != 0)
            {
                if (hashes.count(fileHeader.hash))
                {
                    int itemId = hashes[fileHeader.hash];
                    _index[itemId].FilePos = (int)(fileHeader.data_header_address + fileHeader.length);
                    _index[itemId].Length = fileHeader.zlib_quality ? fileHeader.compressed_size : fileHeader.uncompressed_size;
                }
            }
        }
        next_address = block_header.next_address;
    }

    art_uop.close();
    {
        std::strstream temp;
        temp << "Success loading indexes from "
            << from_file.c_str()
            << std::ends;
        Log(temp.str());
    }
    return;
}


// Returns a partially constructed UIItem with the image and sizes populated
UOItem *ReadImage(std::ifstream& file, int bh)
{
    ArtHeader header;
    if (!file.read((char*)&header, sizeof(header)))
    {
        Log("Failed to read image header");
        return NULL;
    }

    if (header.Height <= 0 || header.Width <= 0 || header.Height >= 1024 || header.Width >= 1024 || header.Unknown > 0xFFFF || header.Unknown == 0)
    {
        return NULL;
    }

    std::shared_ptr<unsigned short> Run(new unsigned short[header.Width]); // it should never be wider than the whole image!
    std::shared_ptr<unsigned short> Lookup(new unsigned short[header.Height]);
    if (!file.read((char*)Lookup.get(), header.Height * sizeof(short)))
    {
        Log("Unable to read art lookup table");
        return NULL;
    }
    std::streamoff dataStart = file.tellg();

    UOItem *pNew = new UOItem;
    pNew->pNext = NULL;

    unsigned short **Image = new unsigned short*[header.Width];
    for (int i = 0; i < header.Width; i++)
    {
        Image[i] = new unsigned short[header.Height];
        memset(Image[i], 0, header.Height * 2);
    }

    pNew->Left = pNew->Top = 0x7FFFFFFF;
    pNew->Right = pNew->Bottom = 0;
    for (int y = 0; y < header.Height; y++)
    {
        int x = 0;

        if (!file.seekg(dataStart + Lookup.get()[y] * 2))
        {
            std::strstream temp;
            temp << "setpos Failed in read image for lookup "
                << y
                << std::ends;
            Log(temp.str());
            return NULL;
        }

        do {
            short RunOffset = 0, RunLength = 0;

            if (!file.read((char*)&RunOffset, 2))
            {
                Log("failed to read image run offset");
                break;
            }
            if (!file.read((char*)&RunLength, 2))
            {
                Log("failed to read image run length");
                break;
            }

            if (RunLength <= 0 || RunOffset < 0 || RunOffset + RunLength >= 2048 || RunLength > header.Width)
                break;

            if (y > pNew->Bottom)
                pNew->Bottom = y;
            if (y < pNew->Top)
                pNew->Top = y;

            x += RunOffset;
            if (x < pNew->Left)
                pNew->Left = x;

            file.read((char*)Run.get(), RunLength * 2);
            for (int o = 0; o < RunLength; o++, x++)
                Image[x][y] = Run.get()[o];

            if (x > pNew->Right)
                pNew->Right = x;
        } while (true);
    }

    float scale = float(bh) / float(pNew->GetHeight());
    if (scale > 1 || scale <= 0)
        scale = 1;

    pNew->RealHeight = (int)(header.Height * scale + 1);
    pNew->RealWidth = (int)(header.Width * scale + 1);
    pNew->Data = new unsigned short *[pNew->RealWidth];
    for (int x = 0; x < pNew->RealWidth; x++)
    {
        pNew->Data[x] = new unsigned short[pNew->RealHeight];
        memset(pNew->Data[x], 0, 2 * pNew->RealHeight);
    }

    for (int x = 0; x < header.Width; x++)
    {
        for (int y = 0; y < header.Height; y++)
            pNew->Data[(int)(x * scale)][(int)(y * scale)] |= Image[x][y];
    }

    pNew->Top = (int)(pNew->Top * scale);
    pNew->Left = (int)(pNew->Left * scale);
    pNew->Bottom = (int)(pNew->Bottom * scale);
    pNew->Right = (int)(pNew->Right * scale);

    for (int x = 0; x < header.Width; x++)
        delete[] Image[x];
    delete[] Image;

    {
        std::strstream temp;
        temp << "Finished "
            << " real W: " << pNew->RealWidth
            << " real H: " << pNew->RealHeight
            << std::ends;
        Log(temp.str());
    }

    {
        std::strstream temp;
        temp << "Left: " << pNew->Left
            << " Right: " << pNew->Right
            << " Top" << pNew->Top
            << " Bottom: " << pNew->Bottom
            << std::ends;
        Log(temp.str());
    }

    return pNew;
}


UOItem* ReadUOPItem(int itemId, const std::string& path, int bh)
{
    if (itemId == 0 || itemId >= 0xFFFF || !pShared)
        return NULL;

    const std::string fileName = path + "/artLegacyMUL.uop";

    if (!_loaded)
        load_uop_pointers(fileName);

    std::ifstream file;
    file.open(fileName.c_str(), std::ios::in | std::ios::binary);
    if (!file.is_open())
    {
        std::strstream temp;
        temp << "unable to open " << fileName.c_str()
            << " for Image 0x" << std::hex << itemId
            << std::ends;
        Log(temp.str());
        return NULL;
    }

    if (_index.count(itemId) == 0)
    {
        std::strstream temp;
        temp << "Image 0x" << std::hex << itemId
            << " not found in index"
            << std::ends;
        Log(temp.str());
        return NULL;
    }

    int file_pos = _index[itemId].FilePos;
    int length = _index[itemId].Length;

    {
        std::strstream temp;
        temp << "Image 0x" << std::hex << itemId << " data at 0x" << std::hex << file_pos
            << std::ends;
        Log(temp.str());
    }
    if (!file.seekg(file_pos))
    {
        Log("File setpos failed");
        return NULL;
    }

    UOItem* pNew = ReadImage(file, bh);

    pNew->ItemID = itemId;
    pNew->pNext = 0;

    return pNew;
}


UOItem* ReadUOItem(int itemId, const std::string& path, int bh)
{
    if (itemId == 0 || itemId >= 0xFFFF || !pShared)
        return NULL;

    ArtIdx idx;
    ArtHeader header;
    memset(&header, 0, sizeof(ArtHeader));

    const std::string idxFileName = path + "/artidx.mul";

    std::ifstream idxMul;
    idxMul.open(idxFileName.c_str(), std::ios::in | std::ios::binary);
    if (!idxMul.is_open())
    {
        std::strstream temp;
        temp << "Unable to open "
            << idxFileName.c_str()
            << std::ends;
        Log(temp.str());
        return NULL;
    }

    {
        std::strstream temp;
        temp << "Image 0x" << std::hex << itemId << " data at 0x" << std::hex << itemId * sizeof(ArtIdx)
            << std::ends;
        Log(temp.str());
    }
    if (!idxMul.seekg(itemId * sizeof(ArtIdx)))
    {
        std::strstream temp;
        temp << "Unable to seek to index for item 0X"
            << std::hex << itemId
            << std::ends;
        Log(temp.str());
        return NULL;
    }
    if (!idxMul.read((char*)&idx, sizeof(ArtIdx)))
    {
        std::strstream temp;
        temp << "Unable to read index for item 0X"
            << std::hex << itemId
            << std::ends;
        Log(temp.str());
        return NULL;
    }

    idxMul.close();

    if (idx.FilePos == -1 || idx.Length == -1)
    {
        return NULL;
    }

    const std::string artFileName = path + "/art.mul";
    std::ifstream artMul;
    artMul.open(artFileName.c_str(), std::ios::in | std::ios::binary);
    if (!artMul.is_open())
    {
        std::strstream temp;
        temp << "Unable to open "
            << artFileName.c_str()
            << std::ends;
        Log(temp.str());
        return NULL;
    }

    if (!artMul.seekg(idx.FilePos))
    {
        std::strstream temp;
        temp << "Unable to position for data in "
            << artFileName.c_str()
            << " for item " << std::hex << itemId
            << std::ends;
        Log(temp.str());
        return NULL;
    }
    UOItem* pNew = ReadImage(artMul, bh);
    pNew->ItemID = itemId;

    return pNew;
}


UOItem *FindItem(int item)
{
    UOItem *node = ArtCache;
    while (node != NULL)
    {
        if (node->ItemID == item)
            return node;
        else
            node = node->pNext;
    }

    return NULL;
}

inline COLORREF Brightness(int shift, COLORREF c)
{
    return RGB(min(255, GetRValue(c) + shift), min(255, GetGValue(c) + shift), min(255, GetBValue(c) + shift));
}



enum SystemType
{
    unknown,
    uopSystem,
    mulSystem
}  systemType = unknown;

int DrawUOItem(HDC hDC, RECT rect, int item, int hueIdx)
{
    item |= 0x4000;

    rect.top++;
    rect.bottom--;
    int maxHeight = rect.bottom - rect.top;

    static std::string path;
    if (systemType == unknown)
    {
        WaitForSingleObject(CommMutex, INFINITE);
        path = pShared->DataPath;
        ReleaseMutex(CommMutex);
        if (fileExists(path + "/art.mul"))
        {
            systemType = mulSystem;
        }
        else if (fileExists(path + "/artLegacyMUL.uop"))
        {
            systemType = uopSystem;
        }
        else
        {
            Log("UNKNOWN SYSTEM");
            throw std::exception("Unable to determine system type. no art.mul or artLegacy.uop");
        }
    }

    UOItem *i = FindItem(item);
    if (i == NULL)
    {
        switch (systemType)
        {
        case unknown:
            i = NULL;
            break;
        case uopSystem:
            i = ReadUOPItem(item, path, maxHeight);
            break;
        case mulSystem:
            i = ReadUOItem(item, path, maxHeight);
            break;
        }
        if (i == NULL)
        {
            return 0;
        }
        else
        {
            i->pNext = ArtCache;
            ArtCache = i;
        }
    }

    if (i->GetHeight() < maxHeight)
        rect.top += (maxHeight - i->GetHeight()) / 2;

    unsigned short *hue = GetHue(hueIdx);
    for (int x = i->Left; x <= i->Right; x++)
    {
        for (int y = i->Top; y <= i->Bottom; y++)
        {
            if (i->Data[x][y] != 0)
                SetPixel(hDC, rect.left + x - i->Left, rect.top + y - i->Top, Brightness(0x30, Color16to32(ApplyHueToPixel(hue, i->Data[x][y]))));
        }
    }

    return i->GetWidth() + 3;
}

void FreeItem(UOItem *node)
{
    if (node != NULL)
    {
        FreeItem(node->pNext);
        for (int i = 0; i < node->RealWidth; i++)
            delete[] node->Data[i];
        delete[] node->Data;
        delete node;
    }
}

void FreeArt()
{
    FreeItem(ArtCache);
    if (Hues && NumHues > 0)
    {
        for (int i = 0; i < NumHues; i++)
            delete[] Hues[i];
        delete[] Hues;
    }
}


int64_t HashFileName(char* s)
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

        return ((int64_t)edi << 32) | eax;
    }

    return ((int64_t)esi << 32) | eax;
}
