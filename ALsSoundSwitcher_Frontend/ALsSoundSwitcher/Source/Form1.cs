using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using ALsSoundSwitcher.Properties;

namespace ALsSoundSwitcher
{
  public partial class Form1 : Form
  {
    public Form1()
    {
      InitializeComponent();
    }

    private void Form1_Load(object sender, EventArgs e)
    {
      ProcessUtils.SetWorkingDirectory();

      Globals.Instance = this;

      if (System.Diagnostics.Debugger.IsAttached)
      {
        TestUtils.RunDebugCode();
      }

      if (Config.Read() == false)
      {
        NotifyUserOfConfigReadFail();
      }

      if (Globals.UserSettings.Mode == DeviceMode.Input)
      {
        if (PowerShellUtils.VerifyAudioCmdletsAvailability() == false)
        {
          Globals.UserSettings.Mode = DeviceMode.Output;
        }
      }

      if (Globals.UserSettings.LaunchOnStartup)
      {
        if (RegistryUtils.DoesStartupRegistrySettingAlreadyExistForThisPath(Globals.UserSettings.Mode) == false)
        {
          Globals.UserSettings.LaunchOnStartup = false;
          Config.Save();
        }
      }

      SetupUI();

      Minimize();

      DeviceUtils.Monitor();
      
      UpgradeUtils.PollForUpdates_Async();
      
      UpgradeUtils.MonitorForOutdatedFilesAndAttemptRemoval_Async();

      FileWatcher.Run();

      //AL.
      MessageBox.Show(@"Attach", @"Attach"); //TODO - remove this line.
      var args = Environment.GetCommandLineArgs();
      if (args.Contains(ArgsType.RestoreMenu.ToString()))
      {
        RestoreMenuState(args);
      }
      else
      {
        ProcessUtils.Restart_ThreadSafe(ArgsType.RestoreMenu);
      }
      //
    }

    private void RestoreMenuState(string[] args)
    {
      var argsAsList = args.ToList();
      var menuStateStartIndex = args.ToList().IndexOf(ArgsType.RestoreMenu.ToString()) + 1;
      for (var i = menuStateStartIndex; i < argsAsList.Count; ++i)
      {
        //AL.
        //TODO - crashing here :< 
        var menuItem = Globals.BaseMenu.Items.Find(argsAsList[i], true).First();
        if (menuItem == null)
        {
          break;
        }

        menuItem.Visible = true;
      }
    }

    private void NotifyUserOfConfigReadFail()
    {
      notifyIcon1.ShowBalloonTip(
        Globals.UserSettings.BalloonTime,
        Resources.Form1_ReadConfig_Error_reading_config_file_ + Globals.ConfigFile,
        Resources.Form1_ReadConfig_Will_use_default_values,
        ToolTipIcon.Error
        );
    }

    private void Minimize()
    {
      WindowState = FormWindowState.Minimized;
      ShowInTaskbar = false;
      Visible = false;
    }

    public void ShowTrayIcon()
    {
      if (InvokeRequired)
      {
        Invoke(new MethodInvoker(ShowTrayIcon));
      }
      else
      {
        notifyIcon1.Visible = true;
      }
    }

    public void HideTrayIcon()
    {
      if (InvokeRequired)
      {
        Invoke(new MethodInvoker(HideTrayIcon));
      }
      else
      {
        notifyIcon1.Visible = false;
      }
    }

    public void SetTrayIcon(Icon icon)
    {
      if (InvokeRequired)
      {
        Invoke((MethodInvoker) delegate
        {
          SetTrayIcon(icon);
        });
      }
      else
      {
        notifyIcon1.Icon = icon;
      }
    }

    private void btn_restart_Click(object sender, EventArgs e)
    {
      Application.Restart();
    }
  }
}