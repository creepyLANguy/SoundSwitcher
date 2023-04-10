using System;
using System.Management.Automation;
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
      FISH();

      Globals.Instance = this;

      if (Config.Read() == false)
      {
        notifyIcon1.ShowBalloonTip(
          Settings.Current.BalloonTime,
          Resources.Form1_ReadConfig_Error_reading_config_file_ + Globals.ConfigFile,
          Resources.Form1_ReadConfig_Will_use_default_values,
          ToolTipIcon.Error
        );
      }

      SetupUI();

      Minimize();

      DeviceUtils.Monitor();
    }

    private void Minimize()
    {
      WindowState = FormWindowState.Minimized;
      notifyIcon1.ShowBalloonTip(Settings.Current.BalloonTime);
      ShowInTaskbar = false;
      Visible = false;
    }

    private static void FISH()
    {
      var isInstalled = false;

      using (PowerShell powerShell = PowerShell.Create())
      {
        powerShell.AddCommand("Get-Module");
        powerShell.AddParameter("Name", "AudioDeviceCmdlets");

        var results = powerShell.Invoke();
        if (powerShell.HadErrors)
        {
          // Handle errors
          MessageBox.Show("Error checking.");
        }

        isInstalled = results.Count > 0;
      }

      if (isInstalled == false)
      {
        using (PowerShell powerShell = PowerShell.Create())
        {
          powerShell.AddCommand("Install-Module");
          powerShell.AddParameter("Name", "AudioDeviceCmdlets");
          powerShell.AddParameter("Scope", "CurrentUser");

          var results = powerShell.Invoke();
          if (powerShell.HadErrors)
          {
            // Handle errors
            MessageBox.Show("Error installing.");
          }
        }
      }
    }
  }
}