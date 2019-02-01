#include <stdio.h>
#include <wchar.h>
#include <tchar.h>
#include "windows.h"
#include "Mmdeviceapi.h"
#include "PolicyConfig.h"
#include "Propidl.h"
#include "Functiondiscoverykeys_devpkey.h"
#include <list>
#include "device.h"

class DeviceReader
{
public:
	HRESULT PopulateDeviceList(std::list<Device*> &deviceList);
};