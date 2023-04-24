using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using ALsSoundSwitcher.Properties;

namespace ALsSoundSwitcher
{
  public partial class Form1 : Form
  {
    public Form1(string[] args)
    {
      ProcessArgs(args.ToList());
    
      InitializeComponent();
    }

    private void Form1_Load(object sender, EventArgs e)
    {
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
      
      SetupUI();

      if (Globals.LastBaseMenuInvokedPosition != Point.Empty)
      {
        ExpandMenusOnThemeCreation();
      }

      Minimize();

      DeviceUtils.Monitor();
    }

    private void ProcessArgs(List<string> argsList)
    {
      var indexOfShowMenusFlag = argsList.IndexOf(Globals.ShowMenusPostThemeRestartFlag);
      
      if (indexOfShowMenusFlag == -1)
      {
        Globals.LastBaseMenuInvokedPosition = Point.Empty;
        return;
      }

      int x = int.Parse(argsList[++indexOfShowMenusFlag]);
      int y = int.Parse(argsList[++indexOfShowMenusFlag]);
      Globals.LastBaseMenuInvokedPosition = new Point(x, y);
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