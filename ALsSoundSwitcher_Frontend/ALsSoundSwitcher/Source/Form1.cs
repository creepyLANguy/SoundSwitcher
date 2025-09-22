using ALsSoundSwitcher.Properties;
using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

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
      RunKeyWatcher();
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

    //AL.

    // Import RegisterHotKey and UnregisterHotKey from user32.dll
    [DllImport("user32.dll")]
    private static extern bool RegisterHotKey(IntPtr hWnd, int id, uint fsModifiers, uint vk);

    [DllImport("user32.dll")]
    private static extern bool UnregisterHotKey(IntPtr hWnd, int id);

    private const int hotkey_id = 1;
    //AL.
    //private const Keys hotkey_modifier = Keys.Alt;
    private const uint hotkey_modifier = 0x0001; // MOD_ALT
    private const uint hotkey_switchToNextDeviceKey = (uint)Keys.OemPeriod;

    private static IntPtr _windowHandle;
    private static ApplicationContext _context;

    public static void RunKeyWatcher()
    {
      //UnregisterHotKey(Globals.Instance.Handle, hotkey_id);
      if (RegisterHotKey(Globals.Instance.Handle, hotkey_id, hotkey_modifier, hotkey_switchToNextDeviceKey))
      {
        return;
      }

      MessageBox.Show("Failed to register hotkey.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
    }

    protected override void WndProc(ref Message m)
    {
      if (m.WParam.ToInt32() == hotkey_id && m.Msg == hotkey_switchToNextDeviceKey)
      {
        Toggle();
      }
      base.WndProc(ref m);
    }

    protected override void OnHandleDestroyed(EventArgs e)
    {
      UnregisterHotKey(Globals.Instance.Handle, hotkey_id);
      base.OnHandleDestroyed(e);
    }

    //
  }
}