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
      var args = Environment.GetCommandLineArgs();
      if (args.Contains(ArgsType.RestoreMenu.ToString()))
      {
        RestoreMenuState(args);
      }
      else
      {
        var choice = 
          MessageBox.Show(
            @"RESTART?" + Environment.NewLine + @"Press Yes to Restart or No to Exit.", 
            "", 
            MessageBoxButtons.YesNo);
        if (choice == DialogResult.Yes)
        {
          ProcessUtils.Restart_ThreadSafe(ArgsType.RestoreMenu);
        }
        else
        {
          Application.Exit();
        }
      }
      //
    }

    private void RestoreMenuState(string[] args)
    {
      var menuStateIndex = args.ToList().IndexOf(ArgsType.RestoreMenu.ToString()) + 1;
      var menuState = args[menuStateIndex];
      var choice = MessageBox.Show(menuState, "", MessageBoxButtons.OKCancel);
      if (choice == DialogResult.Cancel)
      {
        Application.Exit();
      }

      Globals.BaseMenu.Show();
      var activeItem = Globals.BaseMenu.Items.Find(menuState, false);
      if (activeItem.Length > 0)
      {
        activeItem[0].Select();
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