#include <stdio.h>
#include <wchar.h>
#include <tchar.h>
#include "windows.h"
#include "Mmdeviceapi.h"
#include "PolicyConfig.h"

HRESULT SetAudioPlaybackDevice(const LPCWSTR devID)
{
	CoInitialize(nullptr);

	IPolicyConfigVista *pPolicyConfig;

  const ERole reserved = eConsole;

	HRESULT hr = CoCreateInstance(__uuidof(CPolicyConfigVistaClient), nullptr, CLSCTX_ALL, __uuidof(IPolicyConfigVista), reinterpret_cast<LPVOID*>(&pPolicyConfig));

	if (SUCCEEDED(hr))
	{
		hr = pPolicyConfig->SetDefaultEndpoint(devID, reserved);
		pPolicyConfig->Release();
	}

	return hr;
}

void main(const int argc, const char * argv[])
{
	//MessageBox(nullptr, L"attach", L"attach", 0);

	wchar_t id[256] = {0};
	MultiByteToWideChar(CP_ACP, 0, argv[1], -1, id, 256);

	if ((argc==2 ) && (wcslen(id)>0))
	{
		SetAudioPlaybackDevice(id);
	}
}
