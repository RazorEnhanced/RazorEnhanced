#include "stdafx.h"
#include "UOArt.h"
#include "Crypt.h"
#include "Titlebar.h"

#pragma comment(lib, "msimg32.lib")
BOOL dataLoaded = false;

void DrawTitlebar(HDC hdc, HWND hWnd, RECT rect, LPCSTR title)
{
	HFONT hFont;
	char str[512];

	WaitForSingleObject(CommMutex, INFINITE);
	sprintf(str, "%s", pShared->DataPath);
	ReleaseMutex(CommMutex);

	if (!dataLoaded && strlen(str) > 0)
		dataLoaded = UOArtUop::Init(str);

	NONCLIENTMETRICS ncm;
	ncm.cbSize = sizeof(NONCLIENTMETRICS);
	if (SystemParametersInfo(SPI_GETNONCLIENTMETRICS, 0, &ncm, 0))
	{
		hFont = CreateFontIndirect(&ncm.lfCaptionFont);
		SelectObject(hdc, hFont);
	}
	else
	{
		hFont = (HFONT)GetStockObject(ANSI_VAR_FONT);
		SelectObject(hdc, hFont);
	}

	SetBkMode(hdc, TRANSPARENT);

	char *ptr = (char*)title;

	while (*ptr)
	{
		if (*ptr == '~')
		{
			ptr++;
			if (*ptr == '#')
			{
				ptr++;
				if (*ptr == '~')
				{
					ptr++;
					SetBkMode(hdc, TRANSPARENT);
					SetTextColor(hdc, GetSysColor(COLOR_CAPTIONTEXT));
				}

				if (isxdigit((BYTE)*ptr))
				{
					char hex[7];
					strncpy(hex, ptr, 6);
					hex[6] = 0;
					long color = strtol(hex, NULL, 16);
					SetTextColor(hdc, RGB((short)(color >> 16 & 0xff), (short)(color >> 8 & 0xff), (short)(color & 0xff)));
					ptr += 6;
				}
			}

			if (*ptr == '^')
			{
				ptr++;
				long color = StrToHex(6, ptr);
				SetBkMode(hdc, OPAQUE);
				SetBkColor(hdc, RGB((short)(color >> 16 & 0xff), (short)(color >> 8 & 0xff), (short)(color & 0xff)));
				ptr += 6;
			}

			if (*ptr == 'I' || *ptr == 'i')
			{
				ptr++;
				rect.left += DrawItem(hdc, &rect, StrToHex(4, ptr));
				rect.left += 5;
				ptr += 4;
			}

			if (*ptr == 'S')
			{
				/*
				SXPAABBCC

				X = size
				P = 1 if poisoned
				AA = Hitpoints - 1
				BB = Mana - 1
				CC = Stamina - 1
				*/
				ptr++;
				int width = 25;
				switch (*ptr)
				{
				case 'L':
					width = 50;
					break;
				case 'X':
					width = 100;
					break;
				}
				ptr++;
				int status = *ptr - '0';
				ptr++;
				rect.left += DrawStatBar(hdc, rect, width, status, StrToHex(2, ptr), StrToHex(2, ptr + 2), StrToHex(2, ptr + 4));
				rect.left += 4;
				ptr += 6;
			}
		}
		else
		{
			int i = 0;
			char *ptr2 = ptr;
			do
			{
				*ptr2++;
				i++;
			} while (*ptr2 != 0 && *ptr2 != '~');

			DrawTextA(hdc, ptr, i, &rect, DT_LEFT | DT_VCENTER | DT_SINGLELINE);
			SIZE size;
			GetTextExtentPoint32A(hdc, ptr, i, &size);
			rect.left += size.cx;
			ptr += i;
		}
	}
}

int StrToHex(int size, char *ptr)
{
	// "the lazy way"
	PCHAR tmp = (PCHAR)malloc(size + 1);
	strncpy(tmp, ptr, size);
	tmp[size] = 0;
	return (int)strtol(tmp, NULL, 16);
}

int DrawStatBar(HDC hDest, RECT rect, int width, int status, int hits, int mana, int stam)
{
	HGDIOBJ hOrig;
	POINT pt[2];
	hOrig = SelectObject(hDest, GetStockObject(BLACK_PEN));

	rect.top += 1;
	rect.bottom = rect.top + 6 + 6 + 6 + 1;
	rect.right = rect.left + width + 2;
	FrameRect(hDest, &rect, (HBRUSH)GetStockObject(BLACK_BRUSH));

	pt[0].x = rect.left;
	pt[1].x = rect.right;

	pt[0].y = rect.top + 6;
	pt[1].y = rect.top + 6;
	Polyline(hDest, pt, 2);

	pt[0].y = rect.top + 12;
	pt[1].y = rect.top + 12;
	Polyline(hDest, pt, 2);

	RECT statRect;
	statRect.left = rect.left + 1;
	statRect.top = rect.top + 1;
	statRect.bottom = statRect.top + 5;
	double strWidth = ((hits + 1) / 100.0)*width;
	statRect.right = (statRect.left + (int)strWidth);
	FillRect(hDest, &statRect, CreateSolidBrush(RGB(0x00, 0xFF, 0x00)));
	statRect.left = rect.left + 1;
	statRect.top = rect.top + 7;
	statRect.bottom = statRect.top + 5;
	double manaWidth = ((mana + 1) / 100.0)*width;
	statRect.right = (statRect.left + (int)manaWidth);
	FillRect(hDest, &statRect, CreateSolidBrush(RGB(0x00, 0xFF, 0x00)));
	statRect.left = rect.left + 1;
	statRect.top = rect.top + 13;
	statRect.bottom = statRect.top + 5;
	double stamWidth = ((stam + 1) / 100.0)*width;
	statRect.right = (statRect.left + (int)stamWidth);
	FillRect(hDest, &statRect, CreateSolidBrush(RGB(0x00, 0xFF, 0x00)));
	SelectObject(hDest, hOrig);
	return width;
}

int DrawItem(HDC hDest, RECT *r, int itemid)
{
	BITMAP bitmap;
	HDC hdcMem = CreateCompatibleDC(hDest);
	HBITMAP hBitmap = UOArtUop::LoadStatic(itemid)->bitmap;
	SelectObject(hdcMem, hBitmap);
	GetObject(hBitmap, sizeof(bitmap), &bitmap);
	TransparentBlt(hDest, r->left, (r->bottom / 2) - (bitmap.bmHeight / 2), bitmap.bmWidth, bitmap.bmHeight, hdcMem, 0, 0, bitmap.bmWidth, bitmap.bmHeight, 0);
	DeleteDC(hdcMem);
	return bitmap.bmWidth;
}


RECT GetTitlebarRect(HWND hwnd)
{
	HDC hdc;
	RECT rect;

	hdc = GetWindowDC(hwnd);
	GetWindowRect(hwnd, &rect);

	rect.top = GetSystemMetrics(SM_CYFRAME) + 1;
	rect.bottom = rect.top + GetSystemMetrics(SM_CYCAPTION);

	rect.right = (rect.right - rect.left) - (4 * GetSystemMetrics(SM_CXSIZE) + GetSystemMetrics(SM_CXFRAME));
	rect.left = GetSystemMetrics(SM_CXSIZEFRAME) + GetSystemMetrics(SM_CXSMICON) + 5;

	ReleaseDC(hwnd, hdc);
	return rect;
}