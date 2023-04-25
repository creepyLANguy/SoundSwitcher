using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using static ALsSoundSwitcher.Globals;

namespace ALsSoundSwitcher
{
  public partial class Form1
  {
    [DllImport("user32.dll", SetLastError = true)]
    private static extern bool SetForegroundWindow(IntPtr hwnd);

    private void notifyIcon1_MouseClick(object sender, MouseEventArgs e)
    {
      if (e.Button == MouseButtons.Left)
      {
        Toggle();
      }
      else if (e.Button == MouseButtons.Right)
      {
        LastBaseMenuInvokedPosition = Cursor.Position;
        MenuItem_Slider.RefreshValue();
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
        ProcessUtils.RunExe(VolumeMixerExe, VolumeMixerArgs);
      }
    }

    private static void menuItemDeviceManager_Click(object sender, EventArgs e)
    {
      ProcessUtils.RunExe(DeviceManagerExe, DeviceManagerArgs);
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
      Application.Restart();
    }

    private static void menuItemHelp_Click(object sender, EventArgs e)
    {
      Process.Start(GithubUrl);
    }

    //Not sure why it behaves incorrectly without this.
    private static void menuItemExpandable_Hover(object sender, EventArgs e)
    {
      ((ToolStripMenuItem) sender)?.ShowDropDown();
    }

    private static void menuItemTheme_Click(object sender, EventArgs e)
    {
      Settings.Current.Theme = ((ToolStripMenuItem) sender).Text;

      RefreshUITheme();

      Thread.Sleep(Settings.Current.ThemeSwitchUIRefreshDelay);
      BaseMenu.Show();
      MoreMenuItems.MenuItemToggleTheme.GetCurrentParent().Show();
      MoreMenuItems.MenuItemToggleTheme.DropDown.Show();
      ((ToolStripItem) sender).Select();

      Config.Save();
    }

    private static void menuItemMode_Click(object sender, EventArgs e)
    {
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

      Application.Restart();
    }

    private static void menuItemCreateTheme_Click(object sender, EventArgs e)
    {
      new ThemeCreator().Show();
    }
  }
}