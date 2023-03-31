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
        DeviceUtils.SetDefaultAudioDevice((string)menuItem.Tag);

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

    private static void Toggle()
    {
      if (ActiveDevices.Count <= 1)
      {
        return;
      }

      var items = ContextMenuAudioDevices.Items;

      var index = items.IndexOf(ActiveMenuItem);
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
  }
}