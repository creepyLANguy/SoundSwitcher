#include "Includes.h"

float GetDefaultAudioDeviceVolume()
{
  HRESULT hr;
  float volume = 0;

  hr = CoInitialize(nullptr);

  IMMDeviceEnumerator* pEnumerator = nullptr;
  IMMDevice* pDevice = nullptr;
  IAudioEndpointVolume* pEndpointVolume = nullptr;

  // Create a device enumerator
  hr = CoCreateInstance(__uuidof(MMDeviceEnumerator), nullptr,
    CLSCTX_INPROC_SERVER, __uuidof(IMMDeviceEnumerator),
    (LPVOID*)&pEnumerator);
  if (FAILED(hr))
  {
    CoUninitialize();
    return volume;
  }

  // Get the default audio device
  hr = pEnumerator->GetDefaultAudioEndpoint(eRender, eConsole, &pDevice);
  if (FAILED(hr))
  {
    pEnumerator->Release();
    CoUninitialize();
    return volume;
  }

  // Get the endpoint volume interface
  hr = pDevice->Activate(__uuidof(IAudioEndpointVolume),
    CLSCTX_INPROC_SERVER, nullptr, (LPVOID*)&pEndpointVolume);
  if (FAILED(hr))
  {
    pDevice->Release();
    pEnumerator->Release();
    CoUninitialize();
    return volume;
  }

  // Get the master volume level
  hr = pEndpointVolume->GetMasterVolumeLevelScalar(&volume);
  if (FAILED(hr))
  {
    // handle error
  }

  // Release resources
  pEndpointVolume->Release();
  pDevice->Release();
  pEnumerator->Release();
  CoUninitialize();

  return volume;
}

void SetDefaultAudioDeviceVolume(float volume)
{
  HRESULT hr;
  IMMDeviceEnumerator* pEnumerator = nullptr;
  IMMDevice* pDevice = nullptr;
  IAudioEndpointVolume* pEndpointVolume = nullptr;

  hr = CoInitialize(nullptr);

  // Create a device enumerator
  hr = CoCreateInstance(__uuidof(MMDeviceEnumerator), nullptr,
    CLSCTX_INPROC_SERVER, __uuidof(IMMDeviceEnumerator),
    (LPVOID*)&pEnumerator);
  if (FAILED(hr))
  {
    CoUninitialize();
    return;
  }

  // Get the default audio device
  hr = pEnumerator->GetDefaultAudioEndpoint(eRender, eConsole, &pDevice);
  if (FAILED(hr))
  {
    pEnumerator->Release();
    CoUninitialize();
    return;
  }

  // Get the endpoint volume interface
  hr = pDevice->Activate(__uuidof(IAudioEndpointVolume),
    CLSCTX_INPROC_SERVER, nullptr, (LPVOID*)&pEndpointVolume);
  if (FAILED(hr))
  {
    pDevice->Release();
    pEnumerator->Release();
    CoUninitialize();
    return;
  }

  hr = pEndpointVolume->SetMasterVolumeLevelScalar(volume, nullptr);
  if (FAILED(hr))
  {
    // handle error
  }

  // Release resources
  pEndpointVolume->Release();
  pDevice->Release();
  pEnumerator->Release();
  CoUninitialize();
}