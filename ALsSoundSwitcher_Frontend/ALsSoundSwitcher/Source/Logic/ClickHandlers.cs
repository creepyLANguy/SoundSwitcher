using ALsSoundSwitcher.Properties;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
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

    private static void InvokeRightClick()
    {
      UpdateSliderAndTooltip_Async();

      var mi = typeof(NotifyIcon).GetMethod("ShowContextMenu", BindingFlags.Instance | BindingFlags.NonPublic);
      mi?.Invoke(notifyIcon1, null);
    }

    private static async void UpdateSliderAndTooltip_Async()
    {
      await Task.Run(() => MenuItemSlider.RefreshValue());
      await Task.Run(() => SetToolTip(ActiveMenuItemDevice.Text));
    }

    private static void HandleCloseOnClick(object sender, ToolStripDropDownClosingEventArgs e)
    {
      if (e.CloseReason != ToolStripDropDownCloseReason.ItemClicked)
      {
        return;
      }

      var menuLocation = ((ContextMenuStrip)sender).PointToClient(MousePosition);
      var clickedItem = ((ContextMenuStrip)sender).GetItemAt(menuLocation);
      if (clickedItem != null && clickedItem.GetType() == typeof(ToolStripSeparator))
      {
        e.Cancel = true;
      }
    }

    private void notifyIcon1_MouseClick(object sender, MouseEventArgs e)
    {
      if (e.Button == MouseButtons.Right)
      {
        UpdateSliderAndTooltip_Async();
      }
      else if (e.Button == MouseButtons.Left)
      {
        HandleClickAsPerCurrentSettings(UserSettings.LeftClickFunction);
      }
      else if (e.Button == MouseButtons.Middle)
      {
        HandleClickAsPerCurrentSettings(UserSettings.MiddleClickFunction);
      }
    }

    private static void HandleClickAsPerCurrentSettings(MouseControlFunction mouseControlFunction)
    {
      switch (mouseControlFunction)
      {
        case MouseControlFunction.None:
          break;
        case MouseControlFunction.Exit:
          Instance.Close();
          break;
        case MouseControlFunction.Expand:
          InvokeRightClick();
          break;
        case MouseControlFunction.Browse:
          OpenFileExplorer();
          break;
        case MouseControlFunction.Refresh:
          ProcessUtils.Restart_ThreadSafe();
          break;
        case MouseControlFunction.Toggle_Mode:
          TrySwitchMode(GetNextAvailableMode());
          break;
        case MouseControlFunction.Volume_Mixer:
          OpenVolumeMixer();
          break;
        case MouseControlFunction.Manage_Devices:
          OpenDeviceManager();
          break;
        case MouseControlFunction.Switch_Next_Device:
          Toggle();
          break;
        default:
          throw new ArgumentOutOfRangeException();
      }
    }

    private static void menuItemMixer_Click(object sender, EventArgs e)
      => OpenVolumeMixer();

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
      => OpenDeviceManager();

    private static void OpenDeviceManager()
      => ProcessUtils.RunExe(DeviceManagerExe, DeviceManagerArgs, true);

    private static void MenuItemLaunchOnStartup_Click(object sender, EventArgs e)
    {
      var mode = UserSettings.Mode;
      var regResult = UserSettings.LaunchOnStartup ?
        RegistryUtils.TryDeleteStartupRegistrySetting(mode) : RegistryUtils.TrySaveStartupRegistrySetting(mode);

      if (regResult == false)
      {
        return;
      }

      UserSettings.LaunchOnStartup = !UserSettings.LaunchOnStartup;

      Config.Save();

      SetBackgroundForMenuItemLaunchOnStartup();

      RestoreMenus((ToolStripItem)sender);
    }

    private static void menuItem_Click(object sender, EventArgs e)
    => PerformSwitch((ToolStripMenuItem)sender);

    private static void menuItemExit_Click(object sender, EventArgs e)
      => Instance.Close();

    private static void menuItemUpdate_Click(object sender, EventArgs e)
    => UpgradeUtils.Run();

    private static void menuItemRefresh_Click(object sender, EventArgs e)
      => ProcessUtils.Restart_ThreadSafe();

    private static void menuItemHelp_Click(object sender, EventArgs e)
      => Process.Start(GithubUrl);

    private static void menuItemBrowse_Click(object sender, EventArgs e)
      => OpenFileExplorer();

    private static void OpenFileExplorer()
      => Process.Start(Directory.GetCurrentDirectory());

    //Not sure why non-BaseMenu hovers don't expand properly on first try without this call. 
    private static void menuItemExpandable_Hover(object sender, EventArgs e)
      => ((ToolStripMenuItem) sender)?.ShowDropDown();

    private static void menuItemTheme_Click(object sender, EventArgs e)
    {
      UserSettings.Theme = ((ToolStripMenuItem) sender).Text;

      RefreshUITheme();

      RestoreMenus((ToolStripItem)sender);

      Config.Save();
    }

    private static void menuItemMode_Click(object sender, EventArgs e)
      => TrySwitchMode((DeviceMode) ((ToolStripMenuItem) sender).Tag);

    private static void TrySwitchMode(DeviceMode selectedMode)
    {
      if (RegistryUtils.DoesStartupRegistrySettingAlreadyExistForThisPath(UserSettings.Mode))
      {
        if (RegistryUtils.TryDeleteStartupRegistrySetting(UserSettings.Mode) == false)
        {
          return;
        }
      }

      if (selectedMode == DeviceMode.Input)
      {
        if (PowerShellUtils.VerifyAudioCmdletsAvailability() == false)
        {
          return;
        }
      }

      UserSettings.Mode = selectedMode;

      Config.Save();

      if (UserSettings.LaunchOnStartup)
      {
        RegistryUtils.TrySaveStartupRegistrySetting(selectedMode);
      }

      Application.Restart();
    }

    private static DeviceMode GetNextAvailableMode()
    {
      var modes = DeviceModeDictionary.Keys.ToList();
      var nextIndex = modes.IndexOf(UserSettings.Mode) + 1;
      if (nextIndex >= modes.Count)
      {
        nextIndex = 0;
      }
      return modes[nextIndex];
    }

    private static void menuItemPreventAutoSwitch_Click(object sender, EventArgs e)
    {
      UserSettings.PreventAutoSwitch = !UserSettings.PreventAutoSwitch;

      Config.Save();

      SetBackgroundForMenuItemPreventAutoSwitch();

      RestoreMenus((ToolStripItem)sender);
    }

    private static void menuItemCreateTheme_Click(object sender, EventArgs e)
      => new ThemeCreator().Show();

    private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
    => Process.Start(GithubUrl);

    private static void menuItemMouseControlFunction_Click(object sender, EventArgs e)
    {
      var mouseControlFunction =
        MouseFunctionDictionary.First(kvp => kvp.Value == ((ToolStripDropDownItem)sender).Tag.ToString()).Key;

      var parent = ((ToolStripDropDownItem)sender).OwnerItem.Text;

      if (parent == Resources.Form1_SetupMouseControlsSubmenu_Left_Click)
      {
        UserSettings.LeftClickFunction = mouseControlFunction;
      }
      else if (parent == Resources.Form1_SetupMouseControlsSubmenu_Middle_Click)
      {
        UserSettings.MiddleClickFunction = mouseControlFunction;
      }
      
      Config.Save();

      SetBackgroundForMouseControlSubmenus();
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