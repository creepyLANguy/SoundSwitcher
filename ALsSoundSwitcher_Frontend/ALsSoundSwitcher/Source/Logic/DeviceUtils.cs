using System;
using CSCore.CoreAudioAPI;
using CSCore.Win32;

namespace ALsSoundSwitcher
{
  public static class DeviceUtils
  {
    public static void Monitor()
    {
      var enumerator = new MMDeviceEnumerator();
      enumerator.RegisterEndpointNotificationCallback(new EndpointNotificationCallback());

      Console.WriteLine("Monitoring for audio device changes...");
    }

    public static string GetCurrentDefaultDeviceName()
    {
      using (var enumerator = new MMDeviceEnumerator())
      {
        return enumerator.GetDefaultAudioEndpoint(DataFlow.Render, Role.Multimedia).FriendlyName;
      }
    }
  }

  internal class EndpointNotificationCallback : IMMNotificationClient
  {
    public void OnDeviceStateChanged(string deviceId, DeviceState newState)
    {
      Console.WriteLine($"Audio device {deviceId} state changed to {newState}");

      ProcessUtils.ForceRefreshApplication();
    }

    public void OnDeviceAdded(string deviceId)
    {
      Console.WriteLine($"Audio device {deviceId} added");

      ProcessUtils.ForceRefreshApplication();
    }

    public void OnDeviceRemoved(string deviceId)
    {
      Console.WriteLine($"Audio device {deviceId} removed");

      ProcessUtils.ForceRefreshApplication();
    }

    public void OnDefaultDeviceChanged(DataFlow flow, Role role, string defaultDeviceId)
    {
      Console.WriteLine($"Audio device {defaultDeviceId} set as default");
      //AL.
      //TODO - this should be useful going forward.
    }

    public void OnPropertyValueChanged(string deviceId, PropertyKey propertyKey)
    {
      //Console.WriteLine($"Audio device {deviceId} property {propertyKey} value changed");
    }
  }
}