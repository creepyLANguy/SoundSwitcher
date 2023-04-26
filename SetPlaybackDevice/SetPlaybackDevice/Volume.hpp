#include "Includes.h"

float GetDefaultAudioDeviceVolume()
{
  HRESULT hr;
  float volume = 0;

  hr = CoInitialize(nullptr);

  IMMDeviceEnumerator* pEnumerator = nullptr;
  IMMDevice* pDevice = nullptr;
  IAudioEndpointVolume* pEndpointVolume = nullptr;

  hr = CoCreateInstance(__uuidof(MMDeviceEnumerator), nullptr,
    CLSCTX_INPROC_SERVER, __uuidof(IMMDeviceEnumerator),
    (LPVOID*)&pEnumerator);
  if (FAILED(hr))
  {
    CoUninitialize();
    return volume;
  }

  hr = pEnumerator->GetDefaultAudioEndpoint(eRender, eConsole, &pDevice);
  if (FAILED(hr))
  {
    pEnumerator->Release();
    CoUninitialize();
    return volume;
  }

  hr = pDevice->Activate(__uuidof(IAudioEndpointVolume),
    CLSCTX_INPROC_SERVER, nullptr, (LPVOID*)&pEndpointVolume);
  if (FAILED(hr))
  {
    pDevice->Release();
    pEnumerator->Release();
    CoUninitialize();
    return volume;
  }

  hr = pEndpointVolume->GetMasterVolumeLevelScalar(&volume);
  if (FAILED(hr))
  {
    // handle error
  }

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

  hr = CoCreateInstance(__uuidof(MMDeviceEnumerator), nullptr,
    CLSCTX_INPROC_SERVER, __uuidof(IMMDeviceEnumerator),
    (LPVOID*)&pEnumerator);
  if (FAILED(hr))
  {
    CoUninitialize();
    return;
  }

  hr = pEnumerator->GetDefaultAudioEndpoint(eRender, eConsole, &pDevice);
  if (FAILED(hr))
  {
    pEnumerator->Release();
    CoUninitialize();
    return;
  }

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

  pEndpointVolume->Release();
  pDevice->Release();
  pEnumerator->Release();
  CoUninitialize();
}

float GetDefaultMicLevel()
{
  HRESULT hr = S_OK;
  IMMDeviceEnumerator* pEnumerator = NULL;
  IMMDevice* pDevice = NULL;
  IAudioEndpointVolume* pEndpointVolume = NULL;

  hr = CoInitialize(NULL);

  hr = CoCreateInstance(__uuidof(MMDeviceEnumerator), NULL, CLSCTX_ALL, __uuidof(IMMDeviceEnumerator), (LPVOID*)&pEnumerator);

  hr = pEnumerator->GetDefaultAudioEndpoint(eCapture, eConsole, &pDevice);

  hr = pDevice->Activate(__uuidof(IAudioEndpointVolume), CLSCTX_ALL, NULL, (LPVOID*)&pEndpointVolume);

  float level;
  hr = pEndpointVolume->GetMasterVolumeLevelScalar(&level);

  pEndpointVolume->Release();
  pDevice->Release();
  pEnumerator->Release();
  CoUninitialize();

  return level;
}

void SetDefaultMicLevel(float level)
{
  HRESULT hr = S_OK;
  IMMDeviceEnumerator* pEnumerator = NULL;
  IMMDevice* pDevice = NULL;
  IAudioEndpointVolume* pEndpointVolume = NULL;

  hr = CoInitialize(NULL);

  hr = CoCreateInstance(__uuidof(MMDeviceEnumerator), NULL, CLSCTX_ALL, __uuidof(IMMDeviceEnumerator), (LPVOID*)&pEnumerator);

  hr = pEnumerator->GetDefaultAudioEndpoint(eCapture, eConsole, &pDevice);

  hr = pDevice->Activate(__uuidof(IAudioEndpointVolume), CLSCTX_ALL, NULL, (LPVOID*)&pEndpointVolume);

  hr = pEndpointVolume->SetMasterVolumeLevelScalar(level, NULL);

  pEndpointVolume->Release();
  pDevice->Release();
  pEnumerator->Release();
  CoUninitialize();
}