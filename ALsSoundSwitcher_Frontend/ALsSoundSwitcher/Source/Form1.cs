using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using ALsSoundSwitcher.Properties;

namespace ALsSoundSwitcher
{
  [SuppressMessage("ReSharper", "InconsistentNaming")]
  [SuppressMessage("ReSharper", "CommentTypo")]
  public partial class Form1 : Form
  {
    private string[] ar;
    private int lastIndex;

    private ContextMenu contextMenu1;
    private MenuItem menuItemExit;
    private MenuItem menuItemRefresh;
    private MenuItem menuItemEdit;
    private MenuItem menuItemRestart;

    public Form1()
    {
      InitializeComponent();
    }

    private void Form1_Load(object sender, EventArgs e)
    {
      ReadConfig();
      SetupContextMenu();
      Minimize();
    }

    private void Minimize()
    {
      WindowState = FormWindowState.Minimized;
      notifyIcon1.ShowBalloonTip(Definitions.BalloonTime);
      ShowInTaskbar = false;
      Visible = false;
    }

    private void SetupContextMenu()
    {
      var text = "";

      try
      {
        text = File.ReadAllText(Definitions.DevicesFile);
        Console.WriteLine(text);
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

      contextMenu1 = new ContextMenu();

      text = text.Trim();
      ar = text.Split('\n');
      for (var i = 0; i < ar.Length; i += 2)
      {
        listBox1.Items.Add(ar[i]);

        var menuItem = new MenuItem();
        menuItem.Text = ar[i];
        menuItem.Click += menuItem_Click;
        contextMenu1.MenuItems.Add(menuItem);
      }

      contextMenu1.MenuItems.Add("-");

      menuItemRefresh = new MenuItem();
      menuItemRefresh.Index = 0;
      menuItemRefresh.Text = Resources.Form1_SetupContextMenu_R_efresh;
      menuItemRefresh.Click += menuItemRefresh_Click;
      contextMenu1.MenuItems.AddRange(new[] { menuItemRefresh });

      menuItemEdit = new MenuItem();
      menuItemEdit.Index = 0;
      menuItemEdit.Text = Resources.Form1_SetupContextMenu_E_dit;
      menuItemEdit.Click += menuItemEdit_Click;
      contextMenu1.MenuItems.AddRange(new[] { menuItemEdit });

      menuItemRestart = new MenuItem();
      menuItemRestart.Index = 0;
      menuItemRestart.Text = Resources.Form1_SetupContextMenu_Res_tart;
      menuItemRestart.Click += menuItemRestart_Click;
      contextMenu1.MenuItems.AddRange(new[] { menuItemRestart });

      contextMenu1.MenuItems.Add("-");

      menuItemExit = new MenuItem();
      menuItemExit.Index = 0;
      menuItemExit.Text = Resources.Form1_SetupContextMenu_Ex_it;
      menuItemExit.Click += menuItemExit_Click;
      contextMenu1.MenuItems.AddRange(new[] { menuItemExit });

      notifyIcon1.ContextMenu = contextMenu1;
    }

    private void ReadConfig()
    {
      try
      {
        var items = File.ReadAllText(Definitions.ConfigFile).Split();
        Definitions.BalloonTime = Convert.ToInt32(items[0]);
        Definitions.ActiveMarker = " " + items[1];
        Definitions.BestIconNameMatchPercentageMinimum = Convert.ToInt32(items[2]);
      }
      catch (Exception)
      {
        notifyIcon1.ShowBalloonTip(
          Definitions.BalloonTime,
          Resources.Form1_ReadConfig_Error_reading_config_file_ + Definitions.ConfigFile,
          Resources.Form1_ReadConfig_Will_use_default_values + Definitions.GetDevicesExe,
          ToolTipIcon.Error
        );
      }
    }

    private void listBox1_Click(object sender, EventArgs e)
    {
      var index = ((ListBox)sender).SelectedIndex;
      PerformSwitch(index);
      lastIndex = index;
    }

    /*
    private void InvokeRightClick()
    {
      MethodInfo mi = typeof(NotifyIcon).GetMethod("ShowContextMenu",
        BindingFlags.Instance | BindingFlags.NonPublic);
      mi.Invoke(notifyIcon1, null);
    }
    */

    private void notifyIcon1_MouseClick(object sender, MouseEventArgs e)
    {
      if (e.Button == MouseButtons.Left)
      {
        //InvokeRightClick();
        Toggle();
      }
      else if (e.Button == MouseButtons.Middle)
      {
        var processName = 
          Definitions.VolumeMixerExe.Substring(
            0, 
            Definitions.VolumeMixerExe.LastIndexOf(".exe", StringComparison.Ordinal)
            );
        
        var processes = Process.GetProcessesByName(processName);
        if (processes.Length > 0)
        {
          foreach (var process in processes)
          {
            process.Kill();
          }
        }
        else
        {
          RunExe(Definitions.VolumeMixerExe, Definitions.VolumeMixerArgs);
        }
      }
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
      var index = ((MenuItem)Sender).Index;
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
            Definitions.BalloonTime, 
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
        Application.Restart();
        Environment.Exit(0);
      }
      catch (Exception)
      {
        notifyIcon1.ShowBalloonTip(
          Definitions.BalloonTime, 
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
          Definitions.BalloonTime, 
          Resources.Form1_menuItemEdit_Click_Error_Opening_Device_List_File, 
          "Try navigating to file from .exe location.", 
          ToolTipIcon.Error
          );
      }
    }

    private void PerformSwitch(int index)
    {
      if (index < 0)
      {
        MessageBox.Show(Resources.Form1_PerformSwitch_Invalid_index___1___selected_);
        return;
      }

      Console.WriteLine(ar[index * 2]);

      var id = ar[(index * 2) + 1].TrimEnd();

      try
      {
        RunExe(Definitions.SetDeviceExe, id);

        SetIcon(ar[index * 2]);

        notifyIcon1.ShowBalloonTip(
          Definitions.BalloonTime, 
          "Switched Audio Device",
          ar[index*2],
          ToolTipIcon.None
          );
        notifyIcon1.Text = ar[index * 2];

        foreach (MenuItem item in notifyIcon1.ContextMenu.MenuItems)
        {
          var i = item.Text.IndexOf(Definitions.ActiveMarker, StringComparison.Ordinal);
          if (i >= 0)
          {
            item.Text = item.Text.Substring(0, i);
          }
        }
        notifyIcon1.ContextMenu.MenuItems[index].Text += Definitions.ActiveMarker;
      }
      catch (Exception ex)
      {
        Console.WriteLine(ex.ToString());
        notifyIcon1.ShowBalloonTip(
          Definitions.BalloonTime, 
          Resources.Form1_PerformSwitch_Error_Switching_Audio_Device, 
          Resources.Form1_menuItemRefresh_Click_Could_not_start_ + Definitions.SetDeviceExe, 
          ToolTipIcon.Error
          );
      }
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

    private void SetIcon(string iconName)
    {
      notifyIcon1.Icon = Resources.Icon;

      var alIcons = GetAllIconsInFolder();
      if (alIcons.Count == 0)
      {
        return;
      }

      var matches = GetMatchPercentages(iconName.Trim(), alIcons);
      var bestMatch = matches.OrderByDescending(it => it.Item2).First();
      if (bestMatch.Item2 < Definitions.BestIconNameMatchPercentageMinimum)
      {
        return;
      }

      var icon = GetIconByRawName(bestMatch.Item1);
      if (icon != null)
      {
        notifyIcon1.Icon = icon;
      }
    }

    private static List<string> GetAllIconsInFolder()
    { 
      var allIconFilePaths = 
        Directory.GetFiles(Directory.GetCurrentDirectory(), "*", SearchOption.AllDirectories)
          .Where(it => it.Contains(".ico")).ToList();
      
      var allIcons = new List<string>(allIconFilePaths.Count);
      allIconFilePaths.ForEach(it => allIcons.Add(Path.GetFileName(it)));
      return allIcons;
    }

    private static List<Tuple<string, double>> GetMatchPercentages(string reference, List<string> candidates)
    {
      var list = new List<Tuple<string, double>>();

      foreach (var candidate in candidates)
      {
        var matchPercentage = GetMatchPercentage(reference, candidate);
        list.Add(new Tuple<string, double>(candidate, matchPercentage));
      }

      return list;
    }

    private static double GetMatchPercentage(string reference, string candidate)
    {
      var largerStringLength = Math.Max(reference.Length, candidate.Length);

      var editDistance = LevenshteinDistance.Calculate(reference, candidate);

      return (double)(largerStringLength - editDistance) / largerStringLength * 100;
    }

    private static Icon GetIconByRawName(string iconName)
    {
      try
      {
        return new Icon(iconName);
      }
      catch (Exception)
      {
        return null;
      }
    }
  }
}
