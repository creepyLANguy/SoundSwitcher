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
    private static string[] ar;
    private static int lastIndex = -1;

    private ContextMenuStrip contextMenu;
    private ToolStripMenuItem menuItemExit;
    private ToolStripMenuItem menuItemHelp;
    private ToolStripMenuItem menuItemRefresh;
    private ToolStripMenuItem menuItemEdit;
    private ToolStripMenuItem menuItemRestart;
    private ToolStripMenuItem menuItemToggleTheme;
    private ToolStripMenuItem menuItemMixer;
    private ToolStripMenuItem menuItemMore;

    private CustomRenderer theme;

    [DllImport("user32.dll", SetLastError = true)]
    private static extern bool SetForegroundWindow(IntPtr hwnd);

    public Form1()
    {
      InitializeComponent();
    }

    private void Form1_Load(object sender, EventArgs e)
    {
      if (Config.Read() == false)
      {
        notifyIcon1.ShowBalloonTip(
          Settings.Current.BalloonTime,
          Resources.Form1_ReadConfig_Error_reading_config_file_ + Globals.ConfigFile,
          Resources.Form1_ReadConfig_Will_use_default_values + Globals.GetDevicesExe,
          ToolTipIcon.Error
        );
      }

      SetupContextMenu();
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