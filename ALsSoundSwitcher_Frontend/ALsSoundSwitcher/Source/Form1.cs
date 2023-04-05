using System;
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
  }
}