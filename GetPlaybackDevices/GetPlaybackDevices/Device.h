#pragma once

struct Device
{
	Device(LPWSTR name, LPWSTR id)
	{
		int nameSize = (wcslen(name) + 1) * sizeof(LPWSTR);
		friendlyName = new WCHAR[nameSize];
		memset(friendlyName, 0, nameSize);
		wcscpy_s(friendlyName, nameSize, name);

		int idSize = (wcslen(id) + 1) * sizeof(LPWSTR);
		deviceID = new WCHAR[idSize];
		memset(deviceID, 0, idSize);
		wcscpy_s(deviceID, idSize, id);
	}

	~Device()
	{
		delete[] friendlyName;
		delete[] deviceID;
	}

	WCHAR* friendlyName;
	WCHAR* deviceID;
};