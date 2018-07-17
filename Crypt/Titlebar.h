#pragma once
#include "stdafx.h"

RECT GetTitlebarRect(HWND hwnd);
int DrawItem(HDC hDest, RECT *r, int itemid);
void DrawTitlebar(HDC hdc, HWND hWnd, RECT r, LPCSTR title);
int DrawStatBar(HDC hDest, RECT rect, int width, int status, int hits, int mana, int stam);
int StrToHex(int size, char *ptr);