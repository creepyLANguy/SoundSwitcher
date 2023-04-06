using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
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
      //I know refreshing in-place with SetupUI would be awesome, but it does lack visual feedback. 
      //Could do a ContextMenuAudioDevices.Show() but for some reason its location is wrong, even after re-assigning it.
      Application.Restart();
      //SetupUI();
    }

    private static void menuItemHelp_Click(object sender, EventArgs e)
    {
      Process.Start(GithubUrl);
    }

    //Not sure why it behaves incorrectly without this.
    private static void menuItemTheme_Hover(object sender, EventArgs e)
    {
      ((ToolStripMenuItem) sender)?.ShowDropDown();
    }

    private static void menuItemTheme_Click(object sender, EventArgs e)
    {
      Settings.Current.Theme = ((ToolStripMenuItem) sender).Text;

      RefreshUITheme();
      
      ContextMenuAudioDevices.Show();
      
      Config.Save();
    }
  }
}