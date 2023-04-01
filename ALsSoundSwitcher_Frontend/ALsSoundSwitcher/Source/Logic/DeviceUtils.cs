using System;
using System.Threading;
using System.Windows.Forms;
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

    public static void Restart_ThreadSafe()
    {
      if (Globals.Instance.InvokeRequired)
      {
        Globals.Instance.Invoke(new MethodInvoker(Restart_ThreadSafe));
      }
      else
      {
        Application.Restart();
      }
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
    public void OnDeviceStateChanged(string deviceId, DeviceState newState)
    {
      Console.WriteLine(Resources.EndpointNotificationCallback_OnDeviceStateChanged, deviceId, newState);

      Thread.Sleep(1000); //Have to avoid multiple calls on dying instance... orz
      DeviceUtils.Restart_ThreadSafe();
    }

    public void OnDeviceAdded(string deviceId)
    {
      Console.WriteLine(Resources.EndpointNotificationCallback_OnDeviceAdded, deviceId);

      DeviceUtils.Restart_ThreadSafe();
    }

    public void OnDeviceRemoved(string deviceId)
    {
      Console.WriteLine(Resources.EndpointNotificationCallback_OnDeviceRemoved, deviceId);

      DeviceUtils.Restart_ThreadSafe();
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