using System;
using ALsSoundSwitcher.Properties;
using CSCore.CoreAudioAPI;
using CSCore.Win32;

namespace ALsSoundSwitcher
{
  public static class DeviceUtils
  {
    public static void Monitor()
    {
      var enumerator = new MMDeviceEnumerator();
      var notificationCallback = new EndpointNotificationCallback();
      enumerator.RegisterEndpointNotificationCallbackNative(notificationCallback);

      Console.WriteLine(Resources.DeviceUtils_Monitor);
    }

    public static void GetDeviceList()
    {
      Globals.ActiveDevices.Clear();

      using (var enumerator = new MMDeviceEnumerator())
      {
        foreach (var device in enumerator.EnumAudioEndpoints(DataFlow.Render, DeviceState.Active))
        {
          Globals.ActiveDevices.Add(device.FriendlyName, device.DeviceID);
        }
      }
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
    public static string LastMonitoredDeviceUpdate;

    public void OnDeviceStateChanged(string deviceId, DeviceState newState)
    {
      Console.WriteLine(Resources.EndpointNotificationCallback_OnDeviceStateChanged, deviceId, newState);

      if (LastMonitoredDeviceUpdate == deviceId)
      {
        return;
      }
      
      LastMonitoredDeviceUpdate = deviceId;

      ProcessUtils.Restart_ThreadSafe();
    }

    public void OnDeviceAdded(string deviceId)
    {
      Console.WriteLine(Resources.EndpointNotificationCallback_OnDeviceAdded, deviceId);

      ProcessUtils.Restart_ThreadSafe();
    }

    public void OnDeviceRemoved(string deviceId)
    {
      Console.WriteLine(Resources.EndpointNotificationCallback_OnDeviceRemoved, deviceId);

      ProcessUtils.Restart_ThreadSafe();
    }

    public void OnDefaultDeviceChanged(DataFlow flow, Role role, string defaultDeviceId)
    {
      Console.WriteLine(Resources.EndpointNotificationCallback_OnDefaultDeviceChanged, defaultDeviceId);

      if (LastMonitoredDeviceUpdate == defaultDeviceId)
      {
        return;
      }

      LastMonitoredDeviceUpdate = defaultDeviceId;

      if (Globals.WeAreSwitching)
      {
        Globals.WeAreSwitching = false;
      }
      else
      {
        ProcessUtils.Restart_ThreadSafe();
      }
    }

    public void OnPropertyValueChanged(string deviceId, PropertyKey propertyKey)
    {
      //Console.WriteLine($"Audio device {deviceId} property {propertyKey} value changed");
    }
  }
}