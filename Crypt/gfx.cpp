#include "stdafx.h"
#include "Crypt.h"
#include "Resource.h"
#include <uxtheme.h>
#include <vssym32.h>
#include <dwmapi.h>

HICON hRazorIcon = NULL;
HFONT hRazorFont = NULL;

DLLFUNCTION HBITMAP CaptureScreen(BOOL full, const char *msg)
{
	HDC hScreen = NULL;
	int destWidth, destHeight;
	RECT rect;

	//SetForegroundWindow( hWatchWnd );
	UpdateWindow(hWatchWnd);

	if (full)
	{
		hScreen = GetDC(NULL);
		rect.left = 0;
		rect.top = 0;
		destWidth = GetDeviceCaps(hScreen, HORZRES);
		destHeight = GetDeviceCaps(hScreen, VERTRES);
	}
	else
	{
		hScreen = GetWindowDC(hWatchWnd);
		GetWindowRect(hWatchWnd, &rect);
		destWidth = rect.right - rect.left;
		destHeight = rect.bottom - rect.top;
	}

	HDC hCap = CreateCompatibleDC(hScreen);
	HBITMAP hBMP = CreateCompatibleBitmap(hScreen, destWidth, destHeight);
	SelectObject(hCap, hBMP);

	BitBlt(hCap, 0, 0, destWidth, destHeight, hScreen, 0, 0, SRCCOPY);
	//StretchBlt( hCap, 0, 0, destWidth, destHeight, hScreen, rect.left, rect.top, srcWidth, srcHeight, SRCCOPY );

	if (!hRazorIcon)
		hRazorIcon = LoadIcon(hInstance, MAKEINTRESOURCE(IDI_RAZOR));

	if (hRazorIcon)
	{
		rect.left = destWidth - 32;
		rect.right = destWidth;
		rect.top = 0;
		rect.bottom = 32;

		//FillRect( hCap, &rect, (HBRUSH)GetStockObject( BLACK_BRUSH ) );
		DrawIcon(hCap, rect.left, rect.top, hRazorIcon);
	}

	if (msg != NULL && msg[0] != 0)
	{
		SIZE text;
		int len = (int)strlen(msg);
		SelectObject(hCap, hRazorFont);
		SetTextColor(hCap, RGB(255, 255, 255));
		SetBkMode(hCap, TRANSPARENT);

		GetTextExtentPoint32(hCap, msg, len, &text);

		rect.top = 0;
		rect.bottom = text.cy + 4;
		rect.right = destWidth - 32;
		rect.left = destWidth - 32 - text.cx - 4;
		FillRect(hCap, &rect, (HBRUSH)GetStockObject(BLACK_BRUSH));

		TextOut(hCap, rect.left + 2, 2, msg, len);
	}

	ReleaseDC(full ? NULL : hWatchWnd, hScreen);
	DeleteDC(hCap);

	return hBMP;
}