using System;
using System.Windows.Forms;
using ALsSoundSwitcher.Properties;

namespace ALsSoundSwitcher
{
  public partial class Form1 : Form
  {
    public Form1(string[] args)
    {
      InitializeComponent();
    }

    private void Form1_Load(object sender, EventArgs e)
    {
      ProcessUtils.SetWorkingDirectory();

      Globals.Instance = this;

      if (Config.Read() == false)
      {
        NotifyUserOfConfigReadFail();
      }

      if (Settings.Current.Mode == DeviceMode.Input)
      {
        if (PowerShellUtils.VerifyAudioCmdletsAvailability() == false)
        {
          Settings.Current.Mode = DeviceMode.Output;
        }
      }

      if (Settings.Current.LaunchOnStartup)
      {
        if (RegistryUtils.DoesStartupRegistrySettingAlreadyExistsForThisPath(Settings.Current.Mode) == false)
        {
          Settings.Current.LaunchOnStartup = false;
          Config.Save();
        }
      }

      SetupUI();

      Minimize();

      DeviceUtils.Monitor();
    }

    private void NotifyUserOfConfigReadFail()
    {
      notifyIcon1.ShowBalloonTip(
        Settings.Current.BalloonTime,
        Resources.Form1_ReadConfig_Error_reading_config_file_ + Globals.ConfigFile,
        Resources.Form1_ReadConfig_Will_use_default_values,
        ToolTipIcon.Error
        );
    }

    private void Minimize()
    {
      WindowState = FormWindowState.Minimized;
      //notifyIcon1.ShowBalloonTip(Settings.Current.BalloonTime);
      ShowInTaskbar = false;
      Visible = false;
    }
  }
}