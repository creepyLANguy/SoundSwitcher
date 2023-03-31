using System;
using System.Linq;
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
        DeviceUtils.SetDeviceAsDefault((string)menuItem.Tag);

        ActiveMenuItem = menuItem;

        var deviceName = menuItem.Text;

        IconUtils.SetTrayIcon(deviceName, notifyIcon1);

        notifyIcon1.Text = deviceName;

        SetActiveMenuItemMarker();

        notifyIcon1.ShowBalloonTip(
          Settings.Current.BalloonTime,
          "Switched Audio Device",
          deviceName,
          ToolTipIcon.None
        );
      }
      catch (Exception ex)
      {
        Console.WriteLine(ex.ToString());
        notifyIcon1.ShowBalloonTip(
          Settings.Current.BalloonTime,
          Resources.Form1_PerformSwitch_Error_Switching_Audio_Device,
          Resources.Form1_PerformSwitch_could_not_set_default_device,
          ToolTipIcon.Error
        );
      }
    }

    private static void SetActiveMenuItemMarker()
    {
      foreach (ToolStripItem item in notifyIcon1.ContextMenuStrip.Items)
      {
        item.ResetBackColor();
      }

      if (ActiveMenuItem != null)
      {
        ActiveMenuItem.BackColor = Theme.GetActiveSelectionColour();
      }
    }

    private void Toggle()
    {
      var deviceCount = 0;
      foreach (ToolStripMenuItem item in ContextMenuAudioDevices.Items)
      {
        if (item.DropDown == null)
        {
          ++deviceCount;
        }
      }

      var index = ContextMenuAudioDevices.Items.IndexOf(ActiveMenuItem) + 1;

      if (index <= deviceCount)
      {
        index = 0;
      }

      PerformSwitch((ToolStripMenuItem)ContextMenuAudioDevices.Items[index]);
    }
    
    private static void SetCurrentDeviceIconAndIndicatorOnStartup()
    {
      var currentDeviceName = DeviceUtils.GetCurrentDefaultDeviceName();
      var items = ContextMenuAudioDevices.Items.OfType<ToolStripMenuItem>().ToList();
      ActiveMenuItem = items.FirstOrDefault(it => it.Text == currentDeviceName);
      SetActiveMenuItemMarker();

      IconUtils.SetTrayIcon(currentDeviceName, notifyIcon1);
      notifyIcon1.Text = currentDeviceName.Trim();
    }
  }
}