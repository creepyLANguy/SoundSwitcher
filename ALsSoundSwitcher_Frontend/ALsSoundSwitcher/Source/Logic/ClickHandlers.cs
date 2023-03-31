using System;
using System.Diagnostics;
using System.Windows.Forms;
using static ALsSoundSwitcher.Globals;

namespace ALsSoundSwitcher
{
  public partial class Form1
  {
    private void notifyIcon1_MouseClick(object sender, MouseEventArgs e)
    {
      if (e.Button == MouseButtons.Left)
      {
        Toggle();
      }
      else if (e.Button == MouseButtons.Middle)
      {
        OpenVolumeMixer();
      }
    }

    private static void menuItemMixer_Click(object sender, EventArgs e)
    {
      OpenVolumeMixer();
    }

    private static void OpenVolumeMixer()
    {
      var processName = GetVolumeMixerProcessName();
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

    private static string GetVolumeMixerProcessName()
    {
      return VolumeMixerExe.Substring(
        0,
        VolumeMixerExe.LastIndexOf(".exe", StringComparison.Ordinal)
      );
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
      DeviceUtils.RefreshActiveDevices();
    }

    private static void menuItemHelp_Click(object sender, EventArgs e)
    {
      Process.Start(GithubUrl);
    }

    private static void menuItemSwitchTheme_Click(object sender, EventArgs e)
    {
      Settings.Current.DarkMode = (Settings.Current.DarkMode + 1) % 2;
      SetTheme();
      ContextMenuAudioDevices.Show();
      Config.Save();
    }
  }
}