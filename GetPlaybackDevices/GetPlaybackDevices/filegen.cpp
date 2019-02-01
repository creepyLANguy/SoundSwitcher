#include "windows.h"
#include "SoundSwitcher.h"
#include <fstream>

BOOL StartFrontEnd(LPCWSTR name)
{
	// additional information
	STARTUPINFO si;
	PROCESS_INFORMATION pi;

	// set the size of the structures
	ZeroMemory(&si, sizeof(si));
	si.cb = sizeof(si);
	ZeroMemory(&pi, sizeof(pi));

	// start the program up
	BOOL b = CreateProcess(
		name,   // the path
		NULL,			// Command line
		NULL,           // Process handle not inheritable
		NULL,           // Thread handle not inheritable
		FALSE,          // Set handle inheritance to FALSE
		0,              // No creation flags
		NULL,           // Use parent's environment block
		NULL,           // Use parent's starting directory 
		&si,            // Pointer to STARTUPINFO structure
		&pi             // Pointer to PROCESS_INFORMATION structure (removed extra parentheses)
	);

	// Close process and thread handles. 
	CloseHandle(pi.hProcess);
	CloseHandle(pi.hThread);

	return b;
}

void CreateDeviceListFile(std::list<Device*> list)
{
	std::ofstream myfile;
	myfile.open("devices.txt");

	std::list<Device*>::const_iterator iterator = list.begin();
	for (; iterator != list.end(); ++iterator)
	{
		char str[256] = {0};

		WideCharToMultiByte(CP_ACP, 0, (*iterator)->friendlyName, -1, str, 256, nullptr, nullptr);
		myfile << str;
		myfile << "\n";
		
		memset(str, 0, 256);

		WideCharToMultiByte(CP_ACP, 0, (*iterator)->deviceID, -1, str, 256, nullptr, nullptr);
		myfile << str;
		myfile << "\n";
	}

	myfile.close();
}

void main()
{
	SoundSwitcher* s = new SoundSwitcher();

	const std::list<Device*> l = s->GetDeviceList();

	CreateDeviceListFile(l);

	if (StartFrontEnd(L"ALsSoundSwitcher.exe") == false)
	{
		MessageBox(nullptr, L"Failed to load ALsSoundSwitcher.exe", L"Error", 0);
	}
	

	delete s;
}

