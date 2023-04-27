using System;
using System.Collections.Generic;
using System.Linq;
using CSCore.CoreAudioAPI;
using ALsSoundSwitcher.Properties;
using static ALsSoundSwitcher.Globals;

namespace ALsSoundSwitcher
{
  public static class DeviceUtils
  {
    public static void Monitor()
    {
      DeviceEnumerator.RegisterEndpointNotificationCallbackNative((Form1)Instance);

      Console.WriteLine(Resources.DeviceUtils_Monitor);
    }
    public static MMDevice GetCurrentDefaultDevice()
    {
      var dataFlow = Settings.Current.Mode == DeviceMode.Output ? DataFlow.Render : DataFlow.Capture;

      return DeviceEnumerator.GetDefaultAudioEndpoint(dataFlow, Role.Multimedia);
    }

    public static void GetDeviceList()
    {
      ActiveDevices.Clear();

      var dataFlow = Settings.Current.Mode == DeviceMode.Output ? DataFlow.Render : DataFlow.Capture;

      var deviceCollection = DeviceEnumerator.EnumAudioEndpoints(dataFlow, DeviceState.Active);

      var deviceInfoList = deviceCollection.Select(device => Tuple.Create(device.FriendlyName, device.DeviceID)).ToList();

      UpdateDuplicates(deviceInfoList);

      foreach (var device in deviceInfoList)
      {
        ActiveDevices.Add(device.Item1, device.Item2);
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
}