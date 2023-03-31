using System;
using System.Windows.Forms;
using ALsSoundSwitcher.Properties;
using static ALsSoundSwitcher.Globals;

namespace ALsSoundSwitcher
{
  public partial class Form1
  {
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

        var id = Ar[(index * 2) + 1].TrimEnd();

        ProcessUtils.RunExe(SetDeviceExe, id);

        var name = Ar[index * 2];

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
          Resources.Form1_menuItemRefresh_Click_Could_not_start_ + SetDeviceExe,
          ToolTipIcon.Error
        );
      }
    }

    private static void SetActiveMenuItemMarker(int index)
    {
      if (index < 0)
      {
        return;
      }

      foreach (ToolStripItem item in notifyIcon1.ContextMenuStrip.Items)
      {
        item.ResetBackColor();
      }

      notifyIcon1.ContextMenuStrip.Items[index].BackColor = Theme.GetActiveSelectionColour();
    }

    private void Toggle()
    {
      IncrementLastIndex();
      PerformSwitch(LastIndex);
    }

    private void IncrementLastIndex()
    {
      ++LastIndex;
      if (LastIndex == Ar.Length / 2)
      {
        LastIndex = 0;
      }
    }

    //TODO - Fix bug where if you remove the active device from the devices text file, it will likely show the wrong menuitem (and icon) as active. 
    private void SetCurrentDeviceIconAndIndicatorOnStartup()
    {
      var currentDeviceName = DeviceUtils.GetCurrentDefaultDeviceName();
      
      var highestMatchPercentage = 0.0;
      var highestMatchIndex = 0;
      for (var i = 0; i < Ar.Length; i += 2)
      {
        var currentMatchPercentage = IconUtils.GetMatchPercentage(currentDeviceName, Ar[i]);
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

      SetActiveMenuItemMarker(highestMatchIndex / 2);
      IconUtils.SetIcon(currentDeviceName, notifyIcon1);
      notifyIcon1.Text = currentDeviceName.Trim();

      LastIndex = highestMatchIndex / 2;
    }
  }
}