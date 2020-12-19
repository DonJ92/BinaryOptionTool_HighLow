// HLSender.cpp : Defines the exported functions for the DLL application.
// Coded by RedSpider
// 2020.10.06

#include "stdafx.h"
#include "WinHttpClient.h"
#include <stdint.h>
#include <stdlib.h>
#include <memory.h>
#include <iostream>
using namespace std;

#ifdef HLSender_EXPORTS
#define HLSender_API extern "C" __declspec(dllexport)
#else
#define HLSender_API extern "C" __declspec(dllimport)
#endif

string data = "";

HLSender_API int SendSignal(char source[], char symbol[], int cmd, int &result) {
	WinHttpClient client(SEND_SIGNAL_API_URL);

	string data = "source=";
	data += source;
	data += "&symbol=";
	data += symbol;
	data += "&cmd=";
	char lpszTemp[100] = { 0 };
	sprintf(lpszTemp, "%d", cmd);
	data += lpszTemp;

	client.SetAdditionalDataToSend((BYTE *)data.c_str(), data.size());

	wchar_t szSize[50] = L"";
	swprintf_s(szSize, L"%d", data.size());
	wstring headers = L"Content-Length: ";
	headers += szSize;
	headers += L"\r\nContent-Type: application/x-www-form-urlencoded\r\n";
	client.SetAdditionalRequestHeaders(headers);

	// Send http post request.
	bool ret_value = client.SendHttpRequest(L"POST");

	if (!ret_value) {
		result = -1;
		return 0;
	}

	wstring response = client.GetResponseContent();
	result = _wtoi(response.c_str());

	return 1;
}
