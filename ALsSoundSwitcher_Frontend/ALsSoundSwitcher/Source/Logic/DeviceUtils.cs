using System;
using System.Collections.Generic;
using System.Linq;
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
    public static MMDevice GetCurrentDefaultDevice()
    {
      using (var enumerator = new MMDeviceEnumerator())
      {
        return enumerator.GetDefaultAudioEndpoint(DataFlow.Render, Role.Multimedia);
      }
    }

    public static void GetDeviceList()
    {
      Globals.ActiveDevices.Clear();

      using (var enumerator = new MMDeviceEnumerator())
      {
        var deviceCollection = enumerator.EnumAudioEndpoints(DataFlow.Render, DeviceState.Active);

        var deviceInfoList = deviceCollection.Select(device => Tuple.Create(device.FriendlyName, device.DeviceID)).ToList();

        UpdateDuplicates(deviceInfoList);

        foreach (var device in deviceInfoList)
        {
          Globals.ActiveDevices.Add(device.Item1, device.Item2);
        }
      }
    }

    private static void UpdateDuplicates(List<Tuple<string, string>> deviceInfoList)
    {
      var duplicates = new HashSet<string>();

      for (var i = 0; i < deviceInfoList.Count; i++)
      {
        for (var j = i + 1; j < deviceInfoList.Count; j++)
        {
          if (deviceInfoList[i].Item1 == deviceInfoList[j].Item1)
          {
            duplicates.Add(deviceInfoList[i].Item1);
          }
        }
      }

      foreach (var duplicate in duplicates)
      {
        var count = 1;
        for (var k = 0; k < deviceInfoList.Count; k++)
        {
          if (duplicate != deviceInfoList[k].Item1)
          {
            continue;
          }

          var s = deviceInfoList[k].Item1;
          var label = s.Substring(0, s.IndexOf("(", StringComparison.Ordinal));
          var newName = "(" + label + " " + count + ")";
          deviceInfoList[k] = Tuple.Create(newName, deviceInfoList[k].Item2);
          count++;
        }
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