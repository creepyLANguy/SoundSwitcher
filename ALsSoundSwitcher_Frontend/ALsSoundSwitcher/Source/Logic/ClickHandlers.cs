using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static ALsSoundSwitcher.Globals;

namespace ALsSoundSwitcher
{
  public partial class Form1
  {
    [DllImport("user32.dll", SetLastError = true)]
    private static extern bool SetForegroundWindow(IntPtr hwnd);

    private static void PreventCloseOnSeparatorClick(object sender, ToolStripDropDownClosingEventArgs e)
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
        LastBaseMenuInvokedPosition = Cursor.Position;

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
      //Application.Restart();
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

      Thread.Sleep(Settings.Current.ThemeSwitchUIRefreshDelay);
      
      BaseMenu.Show();
      MoreMenuItems.MenuItemToggleTheme.GetCurrentParent().Show();
      MoreMenuItems.MenuItemToggleTheme.DropDown.Show();      
      ((ToolStripItem)sender).Select();

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

    private static void menuItemPreventAutoSwitch_Click(object sender, EventArgs e)
    {      
      Settings.Current.PreventAutoSwitch = !Settings.Current.PreventAutoSwitch;

      Config.Save();

      SetBackgroundForMenuItemPreventAutoSwitch();
    }

    private static void menuItemCreateTheme_Click(object sender, EventArgs e)
    {
      new ThemeCreator().Show();
    }
  }
}