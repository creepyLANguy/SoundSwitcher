using System;
using System.Windows.Forms;
using CSCore.CoreAudioAPI;
using CSCore.Win32;

namespace ALsSoundSwitcher
{
  public static class DevicesWatcher
  {
    public static void Run()
    {
      var enumerator = new MMDeviceEnumerator();
      enumerator.RegisterEndpointNotificationCallback(new EndpointNotificationCallback());

      Console.WriteLine("DevicesWatcher running");
      Console.WriteLine("Monitoring for audio device changes...");
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
    }

    public void OnPropertyValueChanged(string deviceId, PropertyKey propertyKey)
    {
      //Console.WriteLine($"Audio device {deviceId} property {propertyKey} value changed");
    }

  }
}