#include "Includes.h"

HRESULT SetAudioPlaybackDevice(const LPCWSTR devID)
{
		HRESULT hr;

		hr = CoInitialize(nullptr);

		IPolicyConfigVista* pPolicyConfig;

		const ERole reserved = eConsole;

		hr = CoCreateInstance(__uuidof(CPolicyConfigVistaClient), nullptr, CLSCTX_ALL, __uuidof(IPolicyConfigVista), reinterpret_cast<LPVOID*>(&pPolicyConfig));

		if (SUCCEEDED(hr))
		{
				hr = pPolicyConfig->SetDefaultEndpoint(devID, reserved);
				pPolicyConfig->Release();
		}

		CoUninitialize();

		return hr;
}
