#include <windows.h>
#include <list>
#include "DeviceReader.h"

class SoundSwitcher
{
public:
	
	SoundSwitcher();
	~SoundSwitcher();

	void PrintAudioDeviceList();

	HRESULT SetAudioPlaybackDevice(const LPCWSTR devID) const;

	std::list<Device*> GetDeviceList() const { return deviceList; }

	DeviceReader* deviceReader;

	std::list<Device*> deviceList;
};
