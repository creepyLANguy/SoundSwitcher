using System;
using ALsSoundSwitcher.Properties;
using CSCore.CoreAudioAPI;
using CSCore.Win32;

namespace ALsSoundSwitcher
{
  public static class DeviceUtils
  {
    private static Action _callback;

    public static void Monitor(Action callback)
    {
      _callback = callback;
      
      new MMDeviceEnumerator().RegisterEndpointNotificationCallback(new EndpointNotificationCallback());

      Console.WriteLine(Resources.DeviceUtils_Monitor);
    }

    public static bool TryExecuteCallback()
    {
      if (_callback == null)
      {
        return false;
      }

      //AL.
      //TODO - do the fancy thing here 
      /*
        if (Globals.Instance.InvokeRequired)
        {
          Globals.Instance.Invoke(new MethodInvoker(CloseApplication_ThreadSafe));
        }
        else
        {
          Globals.Instance.Close();
        }
       */

      _callback();
      return true;
    }

    public static string GetCurrentDefaultDeviceName()
    {
      using (var enumerator = new MMDeviceEnumerator())
      {
        return enumerator.GetDefaultAudioEndpoint(DataFlow.Render, Role.Multimedia).FriendlyName;
      }
    }

    public static void RefreshActiveDevices()
    {
      Globals.ActiveDevices.Clear();

      using (var deviceEnumerator = new MMDeviceEnumerator())
      {
        foreach (var device in deviceEnumerator.EnumAudioEndpoints(DataFlow.Render, DeviceState.Active))
        {
          Globals.ActiveDevices.Add(device.FriendlyName, device.DeviceID);
        }
      }

      TryExecuteCallback();
    }

    //AL.
    //TODO - implement
    public static void SetDefaultAudioDevice(string deviceId)
    {
      
    }
  }

  internal class EndpointNotificationCallback : IMMNotificationClient
  {
    public void OnDeviceStateChanged(string deviceId, DeviceState newState)
    {
      Console.WriteLine(Resources.EndpointNotificationCallback_OnDeviceStateChanged, deviceId, newState);

      DeviceUtils.RefreshActiveDevices();
    }

    public void OnDeviceAdded(string deviceId)
    {
      Console.WriteLine(Resources.EndpointNotificationCallback_OnDeviceAdded, deviceId);

      DeviceUtils.RefreshActiveDevices();
    }

    public void OnDeviceRemoved(string deviceId)
    {
      Console.WriteLine(Resources.EndpointNotificationCallback_OnDeviceRemoved, deviceId);

      DeviceUtils.RefreshActiveDevices();
    }

    public void OnDefaultDeviceChanged(DataFlow flow, Role role, string defaultDeviceId)
    {
      Console.WriteLine(Resources.EndpointNotificationCallback_OnDefaultDeviceChanged, defaultDeviceId);
      //AL.
      //TODO - Use this to keep state correct when external apps mess with audio devices.
      /*
      eg, When performing a switch, set a global flag to 1
      then, when UI has been refreshed, set it back to 0
      In this function, if the flag is 0, it means we did not set the device (was external) and so trigger a UI refresh
      Important, make sure we do NOT update UI from this function if the flag is 1, as that would be an extra UI update
       */ 

    }

    public void OnPropertyValueChanged(string deviceId, PropertyKey propertyKey)
    {
      //Console.WriteLine($"Audio device {deviceId} property {propertyKey} value changed");
    }
  }
}