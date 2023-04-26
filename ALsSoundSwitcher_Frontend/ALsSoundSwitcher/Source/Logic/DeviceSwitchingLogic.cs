using System;
using System.Windows.Forms;
using ALsSoundSwitcher.Properties;
using static ALsSoundSwitcher.Globals;

namespace ALsSoundSwitcher
{
  public partial class Form1
  {
    private static void PerformSwitch(ToolStripMenuItem menuItem)
    {
      try
      {
        WeAreSwitching = true;

        var deviceId = (string) menuItem.Tag;

        if (Settings.Current.Mode == DeviceMode.Output)
        {
          ProcessUtils.RunExe(SetDeviceExe, deviceId);
        }
        else
        {
          PowerShellUtils.SetInputDeviceCmdlet(deviceId);
        }

        ActiveMenuItemDevice = menuItem;

        var deviceName = menuItem.Text;

        IconUtils.SetTrayIcon(deviceName, notifyIcon1);

        notifyIcon1.Text = deviceName;

        SetActiveMenuItemMarkers();

        MenuItemSlider.RefreshValue();

        NotifyUserOfSwitchResult(deviceName);
      }
      catch (Exception e)
      {
        WeAreSwitching = false;
        
        Console.WriteLine(e.ToString());

        NotifyUserOfSwitchResult();
      }
    }

    private static void Toggle()
    {
      if (ActiveDevices.Count == 0)
      {
        return;
      }

      var items = BaseMenu.Items;

      var index = items.IndexOf(ActiveMenuItemDevice);
      while (true)
      {
        ++index;
        if (index == items.Count)
        {
          index = 0;
        }

        if (items[index].Tag != null)
        {
          PerformSwitch((ToolStripMenuItem)items[index]);
          return;
        }
      }
    }

    private static void NotifyUserOfSwitchResult(string deviceName = null)
    {
      if (deviceName != null)
      {
        var title = Settings.Current.Mode == DeviceMode.Output
          ? Resources.Form1_PerformSwitch_Switched_Audio_Output_Device
          : Resources.Form1_PerformSwitch_Switched_Audio_Input_Device;

        notifyIcon1.ShowBalloonTip(
          Settings.Current.BalloonTime,
          title,
          deviceName,
          ToolTipIcon.None
        );
      }
      else
      {
        notifyIcon1.ShowBalloonTip(
          Settings.Current.BalloonTime,
          Resources.Form1_PerformSwitch_Error_Switching_Audio_Device,
          Resources.Form1_PerformSwitch_could_not_set_default_device,
          ToolTipIcon.Error
        );
      }
    }
  }
}