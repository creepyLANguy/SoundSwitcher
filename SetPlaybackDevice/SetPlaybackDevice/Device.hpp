#include "Includes.h"

HRESULT SetAudioPlaybackDevice(const LPCWSTR devID, ERole role)
{
		HRESULT hr;

		hr = CoInitialize(nullptr);

		IPolicyConfigVista* pPolicyConfig;

		hr = CoCreateInstance(__uuidof(CPolicyConfigVistaClient), nullptr, CLSCTX_ALL, __uuidof(IPolicyConfigVista), reinterpret_cast<LPVOID*>(&pPolicyConfig));

		if (SUCCEEDED(hr))
		{
				hr = pPolicyConfig->SetDefaultEndpoint(devID, role);
				pPolicyConfig->Release();
		}

		CoUninitialize();

		return hr;
}
