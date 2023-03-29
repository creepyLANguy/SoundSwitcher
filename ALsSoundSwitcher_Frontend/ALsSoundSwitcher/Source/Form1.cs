using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using ALsSoundSwitcher.Properties;
using ALsSoundSwitcher.Theming;
using CSCore.CoreAudioAPI;

// ReSharper disable InconsistentNaming
// ReSharper disable IdentifierTypo

namespace ALsSoundSwitcher
{
  public partial class Form1 : Form
  {
    private static string[] ar;
    private static int lastIndex = - 1;

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
          Resources.Form1_ReadConfig_Error_reading_config_file_ + Definitions.ConfigFile,
          Resources.Form1_ReadConfig_Will_use_default_values + Definitions.GetDevicesExe,
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

    private void SetupContextMenu()
    {
      var text = "";

      try
      {
        text = File.ReadAllText(Definitions.DevicesFile);
        //Console.WriteLine(text);
      }
      catch (Exception)
      {
        try
        {
          RunExe(Definitions.GetDevicesExe);
          Close();
        }
        catch (Exception)
        {
          MessageBox.Show(Resources.Form1_SetupContextMenu_ + Definitions.GetDevicesExe);
          Close();
        }

      }

      contextMenu = new ContextMenuStrip();

      var index = 0;

      ar = text.Trim().Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);
      for (var i = 0; i < ar.Length; i += 2)
      {
        var name = ar[i];

        var menuItem = new ToolStripMenuItem();
        menuItem.Text = GetFormattedDeviceName(name);
        menuItem.Click += menuItem_Click;
        menuItem.MergeIndex = index++;

        var iconFile = IconUtils.GetBestMatchIcon(name);
        if (iconFile.Length > 0)
        {
          try
          {
            menuItem.Image = Image.FromFile(iconFile);
          }
          catch (Exception e)
          {
            Console.WriteLine(e.Message);
          }
        }

        contextMenu.Items.Add(menuItem);
      }

      menuItemRefresh = new ToolStripMenuItem(Resources.Form1_SetupContextMenu_R_efresh);
      menuItemRefresh.Click += menuItemRefresh_Click;

      menuItemEdit = new ToolStripMenuItem(Resources.Form1_SetupContextMenu_E_dit);
      menuItemEdit.Click += menuItemEdit_Click;

      menuItemRestart = new ToolStripMenuItem(Resources.Form1_SetupContextMenu_Res_tart);
      menuItemRestart.Click += menuItemRestart_Click;


      menuItemToggleTheme = new ToolStripMenuItem(Resources.Form1_SetupContextMenu_Sw_itchTheme);
      menuItemToggleTheme.Click += menuItemSwitchTheme_Click;

      menuItemExit = new ToolStripMenuItem(Resources.Form1_SetupContextMenu_Ex_it);
      menuItemExit.Click += menuItemExit_Click;

      menuItemHelp = new ToolStripMenuItem(Resources.Form1_SetupContextMenu_H_elp);
      menuItemHelp.Click += menuItemHelp_Click;

      menuItemMixer = new ToolStripMenuItem(Resources.Form1_SetupContextMenu_V_olumeMixer);
      menuItemMixer.Click += menuItemMixer_Click;

      menuItemMore = new ToolStripMenuItem(Resources.Form1_SetupContextMenu_M_ore);

      menuItemMore.DropDownItems.Add(menuItemExit);
      menuItemMore.DropDownItems.Add("-");      
      menuItemMore.DropDownItems.Add(menuItemHelp);
      menuItemMore.DropDownItems.Add("-");
      //menuItemMore.DropDownItems.Add(menuItemEdit);
      //menuItemMore.DropDownItems.Add(menuItemRestart);
      //menuItemMore.DropDownItems.Add("-");
      menuItemMore.DropDownItems.Add(menuItemToggleTheme);
      menuItemMore.DropDownItems.Add("-");
      menuItemMore.DropDownItems.Add(menuItemMixer);
      menuItemMore.DropDownItems.Add("-");
      menuItemMore.DropDownItems.Add(menuItemRefresh);

      menuItemMore.ShowShortcutKeys = true;

      contextMenu.Items.Add("-");
      contextMenu.Items.Add(menuItemMore);

      SetItemMargins(contextMenu.Items.OfType<ToolStripMenuItem>().ToList());

      HideImageMarginOnSubItems(contextMenu.Items.OfType<ToolStripMenuItem>().ToList());

      SetTheme();

      notifyIcon1.ContextMenuStrip = contextMenu;
    }

    private static void SetItemMargins(List<ToolStripMenuItem> items)
    {
      items.ForEach(item =>
      {
        var dropdown = (ToolStripDropDownMenu)item.DropDown;

        item.Padding = new Padding(item.Margin.Right, 5, item.Margin.Right, 5);

        if (dropdown == null)
        {
          return;
        }

        SetItemMargins(item.DropDownItems.OfType<ToolStripMenuItem>().ToList());
      });
    }

    private static void HideImageMarginOnSubItems(List<ToolStripMenuItem> items)
    {
      items.ForEach(item =>
      {
        var dropdown = (ToolStripDropDownMenu)item.DropDown;

        if (dropdown == null)
        {
          return;
        }
        
        dropdown.ShowImageMargin = false;
        HideImageMarginOnSubItems(item.DropDownItems.OfType<ToolStripMenuItem>().ToList());
      });
    }

    private void SetTheme()
    {
      theme = Settings.Current.DarkMode == 1 ? (CustomRenderer)new DarkRenderer() : new LightRenderer();
      contextMenu.Renderer = theme;

      SetActiveMenuItemMarker(lastIndex);
    }

    private static string GetFormattedDeviceName(string name)
    {
      var buffer = "";

      var indexOfOpeningParenthesis = name.IndexOf("(", StringComparison.Ordinal);
      var deviceName = name.Substring(indexOfOpeningParenthesis + 1).TrimEnd(')');
      buffer += deviceName;
      //buffer += "  |  ";
      //buffer += name.Substring(0, indexOfOpeningParenthesis).Trim();
      return buffer;
    }

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

    private void menuItemHelp_Click(object Sender, EventArgs e)
    {
      Process.Start(Definitions.GithubUrl);
    }    
    
    private void menuItemMixer_Click(object Sender, EventArgs e)
    {
      OpenVolumeMixer();
    }

    private void OpenVolumeMixer()
    {
      var processName = GetVolumeMixerProcessName();
      var processes = Process.GetProcessesByName(processName);
      if (processes.Length > 0)
      {
        foreach (var process in processes)
        {
          SetForegroundWindow(process.MainWindowHandle);
        }
      }
      else
      {
        RunExe(Definitions.VolumeMixerExe, Definitions.VolumeMixerArgs);
      }
    }

    private string GetVolumeMixerProcessName()
    {
       return Definitions.VolumeMixerExe.Substring(
         0, 
         Definitions.VolumeMixerExe.LastIndexOf(".exe", StringComparison.Ordinal)
         );
    }

    private static void RunExe(string exeName, string args = "")
    {
      var process = new Process();
      process.StartInfo.FileName = exeName;
      process.StartInfo.RedirectStandardOutput = true;
      process.StartInfo.RedirectStandardError = true;
      process.StartInfo.UseShellExecute = false;
      process.StartInfo.CreateNoWindow = true;
      process.StartInfo.Arguments = args;
      process.Start();
    }

    private void menuItem_Click(object Sender, EventArgs e)
    {
      var index = ((ToolStripMenuItem)Sender).MergeIndex;
      PerformSwitch(index);
      lastIndex = index;
    }

    private void menuItemExit_Click(object Sender, EventArgs e)
    {
      Close();
    }

    private void menuItemRefresh_Click(object Sender, EventArgs e)
    {
      {
        try
        {
          RunExe(Definitions.GetDevicesExe);
          Close();
        }
        catch (Exception)
        {
          notifyIcon1.ShowBalloonTip(
            Settings.Current.BalloonTime, 
            Resources.Form1_menuItemRefresh_Click_Error_Refreshing_Device_List,
            Resources.Form1_menuItemRefresh_Click_Could_not_start_ + Definitions.GetDevicesExe,
            ToolTipIcon.Error
            );
        }
      }
    }

    private void menuItemRestart_Click(object Sender, EventArgs e)
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

    private void menuItemEdit_Click(object Sender, EventArgs e)
    {
      try
      {
        Process.Start(Definitions.DevicesFile);
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

    private void menuItemSwitchTheme_Click(object Sender, EventArgs e)
    {
      Settings.Current.DarkMode = (Settings.Current.DarkMode + 1) % 2;
      SetTheme();
      Config.Save();
    }

    private void PerformSwitch(int index)
    {
      if (index < 0)
      {
        MessageBox.Show(Resources.Form1_PerformSwitch_Invalid_index___1___selected_);
        return;
      }

      try
      {
        //Console.WriteLine(ar[index * 2]);

        var id = ar[(index * 2) + 1].TrimEnd();

        RunExe(Definitions.SetDeviceExe, id);
        
        var name = ar[index * 2];

        IconUtils.SetIcon(name, notifyIcon1);

        notifyIcon1.ShowBalloonTip(
          Settings.Current.BalloonTime, 
          "Switched Audio Device",
          name,
          ToolTipIcon.None
          );

        notifyIcon1.Text = name.Trim();

        SetActiveMenuItemMarker(index);
      }
      catch (Exception ex)
      {
        Console.WriteLine(ex.ToString());
        notifyIcon1.ShowBalloonTip(
          Settings.Current.BalloonTime, 
          Resources.Form1_PerformSwitch_Error_Switching_Audio_Device, 
          Resources.Form1_menuItemRefresh_Click_Could_not_start_ + Definitions.SetDeviceExe, 
          ToolTipIcon.Error
          );
      }
    }

    private void SetActiveMenuItemMarker(int index)
    {
      if (index < 0)
      {
        return;
      }

      foreach (ToolStripItem item in notifyIcon1.ContextMenuStrip.Items)
      {
        item.ResetBackColor();
      }

      notifyIcon1.ContextMenuStrip.Items[index].BackColor = theme.GetActiveSelectionColour();
    }

    private void Form1_LocationChanged(object sender, EventArgs e)
    {
      if (WindowState == FormWindowState.Minimized)
      {
        Minimize();
      }
    }

    private void Toggle()
    {
      IncrementLastIndex();
      PerformSwitch(lastIndex);
    }    
    
    private void IncrementLastIndex()
    {
      ++lastIndex;
      if (lastIndex == ar.Length / 2)
      {
        lastIndex = 0;
      }
    }

    //TODO - Fix bug where if you remove the active device from the devices text file, it will likely show the wrong menuitem (and icon) as active. 
    private void SetCurrentDeviceIconAndIndicatorOnStartup()
    {
      string currentDeviceName;
      using (var enumerator = new MMDeviceEnumerator())
      {
        currentDeviceName = enumerator.GetDefaultAudioEndpoint(DataFlow.Render, Role.Multimedia).FriendlyName;
      }

      var highestMatchPercentage = 0.0;
      var highestMatchIndex = 0;
      for(var i = 0; i < ar.Length; i+=2)
      {
        var currentMatchPercentage = IconUtils.GetMatchPercentage(currentDeviceName, ar[i]);
        if (currentMatchPercentage > highestMatchPercentage)
        {
          highestMatchPercentage = currentMatchPercentage;
          highestMatchIndex = i;
        }
      }

      if (highestMatchPercentage < Settings.Current.BestNameMatchPercentageMinimum)
      {
        return;
      }

      SetActiveMenuItemMarker(highestMatchIndex/2);
      IconUtils.SetIcon(currentDeviceName, notifyIcon1);
      notifyIcon1.Text = currentDeviceName.Trim();

      lastIndex = highestMatchIndex / 2;
    }
  }
}
