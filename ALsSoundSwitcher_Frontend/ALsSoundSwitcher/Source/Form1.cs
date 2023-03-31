using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using ALsSoundSwitcher.Properties;

// ReSharper disable InconsistentNaming
// ReSharper disable IdentifierTypo

namespace ALsSoundSwitcher
{
  public partial class Form1 : Form
  {
    [DllImport("user32.dll", SetLastError = true)]
    private static extern bool SetForegroundWindow(IntPtr hwnd);

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

      DeviceUtils.Monitor(SetupContextMenu);
      DeviceUtils.RefreshActiveDevices();

      SetCurrentDeviceIconAndIndicatorOnStartup();
      Minimize();
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