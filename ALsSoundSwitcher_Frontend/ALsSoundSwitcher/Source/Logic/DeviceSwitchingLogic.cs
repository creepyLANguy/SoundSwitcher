using System;
using System.Linq;
using System.Management.Automation;
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

        if (Settings.Current.Mode == DeviceMode.Output)
        {
          ProcessUtils.RunExe(SetDeviceExe, (string)menuItem.Tag);
        }
        else
        {
          SetInputDevice((string)menuItem.Tag);
        }

        ActiveMenuItemDevice = menuItem;

        var deviceName = menuItem.Text;

        IconUtils.SetTrayIcon(deviceName, notifyIcon1);

        notifyIcon1.Text = deviceName;

        SetActiveMenuItemMarkers();

        NotifyUserOfSwitchResult(deviceName);
      }
      catch (Exception e)
      {
        WeAreSwitching = false;
        
        Console.WriteLine(e.ToString());

        NotifyUserOfSwitchResult();
      }
    }

    private static void SetInputDevice(string id)
    {
      using (var ps = PowerShell.Create())
      {
        ps.AddCommand("Set-AudioDevice");
        ps.AddParameter("-ID", id);
        ps.Invoke();
      }
    }

    private static void SetActiveMenuItemMarkers()
    {
      var menuItems = BaseMenu.Items.OfType<ToolStripMenuItem>().ToList();
      foreach (var item in menuItems)
      {
        item.ResetBackColor();
      }
      if (ActiveMenuItemDevice != null)
      {
        ActiveMenuItemDevice.BackColor = Theme.GetActiveSelectionColour();
      }


      if (MoreMenuItems.MenuItemToggleTheme.HasDropDownItems)
      {
        foreach (ToolStripMenuItem item in MoreMenuItems.MenuItemToggleTheme.DropDownItems)
        {
          item.ResetBackColor();

          if (item.Text == Settings.Current.Theme)
          {
            item.BackColor = Theme.GetActiveSelectionColour();
          }
        }
      }


      if (MoreMenuItems.MenuItemMode.HasDropDownItems)
      {
        foreach (ToolStripMenuItem item in MoreMenuItems.MenuItemMode.DropDownItems)
        {
          item.ResetBackColor();

          if (item.Text == Settings.Current.Mode.ToString())
          {
            item.BackColor = Theme.GetActiveSelectionColour();
          }
        }
      }
    }

    private static void Toggle()
    {
      if (ActiveDevices.Count <= 1)
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