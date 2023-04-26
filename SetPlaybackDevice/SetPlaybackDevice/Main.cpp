#include "Device.hpp"
#include "Volume.hpp"

int main(const int argc, const char * argv[])
{
  if (argc < 2)
  {
    return -1;
  }

  wchar_t arg[256] = { 0 };
  MultiByteToWideChar(CP_ACP, 0, argv[1], -1, arg, 256);

  if (!wcscmp(arg, L"GetVolume")) 
  {
    return static_cast<int>(GetDefaultAudioDeviceVolume() * 100);
  }
  else if (!wcscmp(arg, L"SetVolume"))
  {
    constexpr int buffSize = 4;
    wchar_t buff[buffSize] = { 0 };
    MultiByteToWideChar(CP_ACP, 0, argv[2], -1, buff, buffSize);
    float vol = wcstof(buff,nullptr)/100.0f;
    SetDefaultAudioDeviceVolume(vol);
  }
  else if (!wcscmp(arg, L"GetMicLevel"))
  {
    return static_cast<int>(GetDefaultMicLevel() * 100);
  }
  else if (!wcscmp(arg, L"SetMicLevel"))
  {
    constexpr int buffSize = 4;
    wchar_t buff[buffSize] = { 0 };
    MultiByteToWideChar(CP_ACP, 0, argv[2], -1, buff, buffSize);
    float level = wcstof(buff, nullptr) / 100.0f;
    SetDefaultMicLevel(level);
  }
	 else if (wcslen(arg) > 0)
	 {
	   SetAudioPlaybackDevice(arg);
  }
}
