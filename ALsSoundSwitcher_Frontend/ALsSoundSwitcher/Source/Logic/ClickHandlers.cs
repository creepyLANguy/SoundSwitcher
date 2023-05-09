using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Forms;
using static ALsSoundSwitcher.Globals;

namespace ALsSoundSwitcher
{
  public partial class Form1
  {
    [DllImport("user32.dll", SetLastError = true)]
    private static extern bool SetForegroundWindow(IntPtr hwnd);

    private static void HandleCloseOnClick(object sender, ToolStripDropDownClosingEventArgs e)
    {
      if (e.CloseReason != ToolStripDropDownCloseReason.ItemClicked)
      {
        return;
      }

      Point menuLocation = ((ContextMenuStrip)sender).PointToClient(MousePosition);
      ToolStripItem clickedItem = ((ContextMenuStrip)sender).GetItemAt(menuLocation);
      if (clickedItem != null && clickedItem.GetType() == typeof(ToolStripSeparator))
      {
        e.Cancel = true;
      }
    }

    private async void notifyIcon1_MouseClick(object sender, MouseEventArgs e)
    {
      if (e.Button == MouseButtons.Left)
      {
        Toggle();        
      }
      else if (e.Button == MouseButtons.Right)
      {
        await Task.Run(() => MenuItemSlider.RefreshValue());
      }
    }

    private static void menuItemMixer_Click(object sender, EventArgs e)
    {
      OpenVolumeMixer();
    }

    private static void OpenVolumeMixer()
    {
      var processName = Path.GetFileNameWithoutExtension(VolumeMixerExe);
      var processes = Process.GetProcessesByName(processName);
      if (processes.Length > 0)
      {
        foreach (var process in processes)
        {
          SetForegroundWindow(process.MainWindowHandle);
        }
      }
      else
      {
        ProcessUtils.RunExe(VolumeMixerExe, VolumeMixerArgs, true);
      }
    }

    private static void menuItemDeviceManager_Click(object sender, EventArgs e)
    {
      ProcessUtils.RunExe(DeviceManagerExe, DeviceManagerArgs, true);
    }

    private static void MenuItemLaunchOnStartup_Click(object sender, EventArgs e)
    {
      var mode = Settings.Current.Mode;
      var regResult = Settings.Current.LaunchOnStartup ?
        RegistryUtils.TryDeleteStartupRegistrySetting(mode) : RegistryUtils.TrySaveStartupRegistrySetting(mode);

      if (regResult == false)
      {
        return;
      }

      Settings.Current.LaunchOnStartup = !Settings.Current.LaunchOnStartup;

      Config.Save();

      SetBackgroundForMenuItemLaunchOnStartup();

      RestoreMenus((ToolStripItem)sender);
    }

    private static void menuItem_Click(object sender, EventArgs e)
    {
      PerformSwitch((ToolStripMenuItem)sender);
    }

    private static void menuItemExit_Click(object sender, EventArgs e)
    {
      Instance.Close();
    }

    private static void menuItemRefresh_Click(object sender, EventArgs e)
    {
      ProcessUtils.Restart_ThreadSafe();
    }

    private static void menuItemHelp_Click(object sender, EventArgs e)
    {
      Process.Start(GithubUrl);
    }

    //Not sure why non-BaseMenu hovers don't expand properly on first try without this call. 
    private static void menuItemExpandable_Hover(object sender, EventArgs e)
    {
      ((ToolStripMenuItem) sender)?.ShowDropDown();
    }

    private static void menuItemTheme_Click(object sender, EventArgs e)
    {
      Settings.Current.Theme = ((ToolStripMenuItem) sender).Text;

      RefreshUITheme();

      RestoreMenus((ToolStripItem)sender);

      Config.Save();
    }

    private static void menuItemMode_Click(object sender, EventArgs e)
    {
      if (RegistryUtils.DoesStartupRegistrySettingAlreadyExistsForThisPath(Settings.Current.Mode))
      {
        if (RegistryUtils.TryDeleteStartupRegistrySetting(Settings.Current.Mode) == false)
        {
          return;
        }
      }

      var selection = ((ToolStripMenuItem) sender).Text;
      var selectedMode = (DeviceMode)Enum.Parse(typeof(DeviceMode), selection);

      if (selectedMode == DeviceMode.Input)
      {
        if (PowerShellUtils.VerifyAudioCmdletsAvailability() == false)
        {
          return;
        }
      }
      
      Settings.Current.Mode = selectedMode;

      Config.Save();

      if (Settings.Current.LaunchOnStartup)
      {
        RegistryUtils.TrySaveStartupRegistrySetting(selectedMode);
      }

      Application.Restart();
    }

    private static void menuItemPreventAutoSwitch_Click(object sender, EventArgs e)
    {      
      Settings.Current.PreventAutoSwitch = !Settings.Current.PreventAutoSwitch;

      Config.Save();

      SetBackgroundForMenuItemPreventAutoSwitch();

      RestoreMenus((ToolStripItem)sender);
    }

    private static void menuItemCreateTheme_Click(object sender, EventArgs e)
    {
      new ThemeCreator().Show();
    }

    private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
    {
      Process.Start(GithubUrl);
    }

    public static void RestoreMenus(ToolStripItem sender)
    {
      BaseMenu.Show();
      MenuItemMore.Select();
      MenuItemMore.DropDown.Show();
      sender.GetCurrentParent().Show();
      sender.Select();
    }
  }
}