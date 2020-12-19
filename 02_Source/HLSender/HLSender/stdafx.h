// stdafx.h : include file for standard system include files,
// or project specific include files that are used frequently, but
// are changed infrequently
//

#pragma once

#include "targetver.h"

#define WIN32_LEAN_AND_MEAN             // Exclude rarely-used stuff from Windows headers
// Windows Header Files:
#include <windows.h>

#include <stdio.h>
#include <tchar.h>

#include <wincrypt.h>
#include <winhttp.h>
#include <vector>
using namespace std;

//#define USE_TEST

#ifdef USE_TEST
#define SEND_SIGNAL_API_URL			_T("http://localhost:8000/api/sendSignal")
#else
#define SEND_SIGNAL_API_URL			_T("http://153.127.70.45/api/sendSignal")
#endif

// TODO: reference additional headers your program requires here
