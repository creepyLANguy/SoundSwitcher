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

    //AL.
    //TODO - implement
    public static void RefreshActiveDevices()
    {
      Globals.ActiveDevices.Clear();
      //Globals.ActiveDevices = ...

      TryExecuteCallback();
    }

    //AL.
    //TODO - implement
    public static void SetDeviceAsDefault(string deviceId)
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
      //TODO - this should be useful going forward.
    }

    public void OnPropertyValueChanged(string deviceId, PropertyKey propertyKey)
    {
      //Console.WriteLine($"Audio device {deviceId} property {propertyKey} value changed");
    }
  }
}