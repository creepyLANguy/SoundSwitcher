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
        WeAreSwitching = true;

        ProcessUtils.RunExe(SetDeviceExe, (string)menuItem.Tag);

        ActiveMenuItemOutput = menuItem;

        var deviceName = menuItem.Text;

        IconUtils.SetTrayIcon(deviceName, notifyIcon1);

        notifyIcon1.Text = deviceName;

        SetActiveMenuItemMarkers();

        notifyIcon1.ShowBalloonTip(
          Settings.Current.BalloonTime,
          "Switched Audio Device",
          deviceName,
          ToolTipIcon.None
        );
      }
      catch (Exception e)
      {
        WeAreSwitching = false;
        
        Console.WriteLine(e.ToString());
        
        notifyIcon1.ShowBalloonTip(
          Settings.Current.BalloonTime,
          Resources.Form1_PerformSwitch_Error_Switching_Audio_Device,
          Resources.Form1_PerformSwitch_could_not_set_default_device,
          ToolTipIcon.Error
        );
      }
    }

    private static void SetActiveMenuItemMarkers()
    {
      var menuItems = BaseMenu.Items.OfType<ToolStripMenuItem>().ToList();

      foreach (var item in menuItems)
      {
        item.ResetBackColor();
      }

      if (ActiveMenuItemOutput != null)
      {
        ActiveMenuItemOutput.BackColor = Theme.GetActiveSelectionColour();
      }

      if (MoreMenuItems.MenuItemToggleTheme.HasDropDownItems)
      {
        foreach (ToolStripMenuItem item in MoreMenuItems.MenuItemToggleTheme.DropDownItems)
        {
          item.ResetBackColor();
        }
      }

      if (ActiveMenuItemTheme != null)
      {
        ActiveMenuItemTheme.BackColor = Theme.GetActiveSelectionColour();
      }
    }

    private static void Toggle()
    {
      if (ActiveDevices.Count <= 1)
      {
        return;
      }

      var items = BaseMenu.Items;

      var index = items.IndexOf(ActiveMenuItemOutput);
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