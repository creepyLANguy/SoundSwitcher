using System;
using System.Diagnostics;
using System.Windows.Forms;
using ALsSoundSwitcher.Properties;
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
          ALsSoundSwitcher.Form1.SetForegroundWindow(process.MainWindowHandle);
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

    private void menuItem_Click(object sender, EventArgs e)
    {
      var index = ((ToolStripMenuItem) sender).MergeIndex;
      PerformSwitch(index);
      LastIndex = index;
    }

    private static void menuItemExit_Click(object sender, EventArgs e)
    {
      Instance.Close();
    }

    private static void menuItemRefresh_Click(object sender, EventArgs e)
    {
      {
        try
        {
          ProcessUtils.RunExe(GetDevicesExe);
            Instance.Close();
        }
        catch (Exception)
        {
          notifyIcon1.ShowBalloonTip(
            Settings.Current.BalloonTime,
            Resources.Form1_menuItemRefresh_Click_Error_Refreshing_Device_List,
            Resources.Form1_menuItemRefresh_Click_Could_not_start_ + GetDevicesExe,
            ToolTipIcon.Error
          );
        }
      }
    }

    private static void menuItemRestart_Click(object sender, EventArgs e)
    {
      try
      {
        notifyIcon1.Icon.Dispose();
        notifyIcon1.Dispose();
        Application.Restart();
        Environment.Exit(0);
      }
      catch (Exception)
      {
        notifyIcon1.ShowBalloonTip(
          Settings.Current.BalloonTime,
          Resources.Form1_menuItemRestart_Click_Error_Restarting_Application,
          Resources.Form1_menuItemRestart_Click_Please_try_manually_closing_and_starting_the_application_,
          ToolTipIcon.Error
        );
      }
    }

    private static void menuItemEdit_Click(object sender, EventArgs e)
    {
      try
      {
        Process.Start(DevicesFile);
      }
      catch (Exception)
      {
        notifyIcon1.ShowBalloonTip(
          Settings.Current.BalloonTime,
          Resources.Form1_menuItemEdit_Click_Error_Opening_Device_List_File,
          "Try navigating to file from .exe location.",
          ToolTipIcon.Error
        );
      }
    }

    private static void menuItemSwitchTheme_Click(object sender, EventArgs e)
    {
      Settings.Current.DarkMode = (Settings.Current.DarkMode + 1) % 2;
      SetTheme();
      Config.Save();
    }

    private static void menuItemHelp_Click(object sender, EventArgs e)
    {
      Process.Start(GithubUrl);
    }
  }
}