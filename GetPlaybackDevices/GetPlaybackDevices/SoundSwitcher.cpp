#include "SoundSwitcher.h"
#include "Mmdeviceapi.h"
#include "PolicyConfig.h"
#include <InitGuid.h> 

#include "stdio.h"
#include "wchar.h"
#include "tchar.h"
#include "windows.h"
#include "Mmdeviceapi.h"
#include "PolicyConfig.h"
#include "Propidl.h"
#include "Functiondiscoverykeys_devpkey.h"

SoundSwitcher::SoundSwitcher()
{
	CoInitialize(nullptr);

	deviceReader = new DeviceReader();
	deviceReader->PopulateDeviceList(deviceList);
}

SoundSwitcher::~SoundSwitcher()
{
	while (deviceList.size() > 0)
	{
		Device* d = deviceList.back();
		deviceList.remove(d);
		delete d;
	}
}

void SoundSwitcher::PrintAudioDeviceList()
{
	std::list<Device*>::const_iterator iterator = deviceList.begin();
	for (; iterator != deviceList.end(); ++iterator)
	{
		OutputDebugString((LPCWSTR)(*iterator)->friendlyName);
		OutputDebugString((LPCWSTR)(*iterator)->deviceID);
		OutputDebugString(L"\r\n");
	}

}

HRESULT SoundSwitcher::SetAudioPlaybackDevice(const LPCWSTR devID) const
{
	IPolicyConfigVista *pPolicyConfig;

	ERole reserved = eConsole;

	HRESULT hr = CoCreateInstance(
		__uuidof(CPolicyConfigVistaClient),
		NULL,
		CLSCTX_ALL,
		__uuidof(IPolicyConfigVista),
		(LPVOID *)&pPolicyConfig);

	if (SUCCEEDED(hr))
	{
		hr = pPolicyConfig->SetDefaultEndpoint(devID, reserved);
		pPolicyConfig->Release();
	}


	return hr;
}
