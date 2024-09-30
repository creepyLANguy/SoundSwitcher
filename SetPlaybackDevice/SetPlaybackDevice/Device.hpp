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

HRESULT SetCommunicationPlaybackDevice(const LPCWSTR devID)
{
    HRESULT hr;

    // Initialize COM library
    hr = CoInitialize(nullptr);

    IPolicyConfigVista* pPolicyConfig;

    // Set the role to eCommunications to specify this as the default communication device
    const ERole role = eCommunications;

    hr = CoCreateInstance(__uuidof(CPolicyConfigVistaClient), nullptr, CLSCTX_ALL, __uuidof(IPolicyConfigVista), reinterpret_cast<LPVOID*>(&pPolicyConfig));

    if (SUCCEEDED(hr))
    {
        // Set the default communication device using the provided device ID
        hr = pPolicyConfig->SetDefaultEndpoint(devID, role);
        pPolicyConfig->Release();
    }

    CoUninitialize();

    return hr;
}
