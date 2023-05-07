using System;
using ALsSoundSwitcher.Properties;
using CSCore.CoreAudioAPI;
using CSCore.Win32;

namespace ALsSoundSwitcher
{
  public partial class Form1 : IMMNotificationClient
  {
    public static string LastMonitoredDeviceUpdate;

    public void OnDeviceStateChanged(string deviceId, DeviceState newState)
    {
      if (IgnoreThisUpdate(deviceId))
      {
        return;
      }

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
      if (IgnoreThisUpdate(deviceId))
      {
        return;
      }

      Console.WriteLine(Resources.EndpointNotificationCallback_OnDeviceAdded, deviceId);

      ProcessUtils.Restart_ThreadSafe();
    }

    public void OnDeviceRemoved(string deviceId)
    {
      if (IgnoreThisUpdate(deviceId))
      {
        return;
      }

      Console.WriteLine(Resources.EndpointNotificationCallback_OnDeviceRemoved, deviceId);
    }

    public void OnDefaultDeviceChanged(DataFlow flow, Role role, string deviceId)
    {
      if (IgnoreThisUpdate(deviceId))
      {
        return;
      }

      Console.WriteLine(Resources.EndpointNotificationCallback_OnDefaultDeviceChanged, deviceId);

      var cachedActiveDeviceId = (string)Globals.ActiveMenuItemDevice.Tag;

      if (Settings.Current.PreventAutoSwitch && Globals.WeAreSwitching == false)
      {
        if (cachedActiveDeviceId == deviceId)
        {
          return;
        }

        Globals.WeAreSwitching = true;
     
        //AL.
        //TODO - there's a bug here.
        //This code is reached when we are in PreventAutioSwitch mode and changed the device ourselves.
        //The result is that we retrigger a switch many times.
        //This might have something to do with us not chaching the active device correctly or in time.
        if (Settings.Current.Mode == DeviceMode.Output)
        {
          ProcessUtils.RunExe(Globals.SetDeviceExe, cachedActiveDeviceId);
        }
        else
        {
          PowerShellUtils.SetInputDeviceCmdlet(cachedActiveDeviceId);
        }
        return;
      }

      if (LastMonitoredDeviceUpdate == deviceId)
      {
        return;
      }

      LastMonitoredDeviceUpdate = deviceId;

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
      
      var cachedActiveDeviceId = (string)Globals.ActiveMenuItemDevice.Tag;
      if ((deviceId == cachedActiveDeviceId) && (propertyKey.ToString() == Globals.VolumeChangedPropertyKey))
      {
        Globals.MenuItemSlider.RefreshValue();
      }
    }

    private static bool IgnoreThisUpdate(string deviceId)
    {
      var device = Globals.DeviceEnumerator.GetDevice(deviceId);
      var dataFlow = Settings.Current.Mode == DeviceMode.Output ? DataFlow.Render : DataFlow.Capture;
      return device.DataFlow != dataFlow;
    }
  }
}