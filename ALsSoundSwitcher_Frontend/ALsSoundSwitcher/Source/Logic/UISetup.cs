using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;
using ALsSoundSwitcher.Properties;
using static ALsSoundSwitcher.Globals;
using static ALsSoundSwitcher.Globals.MoreMenuItems;

namespace ALsSoundSwitcher
{
  public class SliderMenuItem : ToolStripControlHost
  {
    private TrackBar trackBar;

    public SliderMenuItem(int value = 50)
        : base(new TrackBar())
    {
      trackBar = Control as TrackBar;
      trackBar.AutoSize = false;
      trackBar.TickStyle = TickStyle.None;
      trackBar.Height = 20;
      trackBar.Width = BaseMenu.Width;
      trackBar.Minimum = 0;
      trackBar.Maximum = 100;
      trackBar.Value = value;
      trackBar.BackColor = Theme.GetBackgroundColour();

      trackBar.Scroll += new EventHandler(trackBar_Scroll);

      trackBar.TickFrequency = 0;
    }

    private void trackBar_Scroll(object sender, EventArgs e)
    {
      ProcessUtils.RunExe(SetDeviceExe, "SetVolume " + trackBar.Value);;
    }
  }


  public partial class Form1
  {
    private static void SetupUI()
    {
      DeviceUtils.GetDeviceList();
      SetupContextMenu();
      SetCurrentDeviceIconAndIndicators();
    }

    private static void SetupContextMenu()
    {
      BaseMenu = new ContextMenuStrip();

      AddAudioDevicesAsMenuItems();

      SetupAdditionalMenuItems();

      AddAdditionalMenuItems();

      SetItemMargins(BaseMenu.Items.OfType<ToolStripMenuItem>().ToList());

      HideImageMarginOnSubItems(BaseMenu.Items.OfType<ToolStripMenuItem>().ToList());

      notifyIcon1.ContextMenuStrip = BaseMenu;

      RefreshUITheme();

      //AL.
      BaseMenu.Items.Add("-");
      var volume = ProcessUtils.RunExe(Globals.SetDeviceExe, "GetVolume");
      BaseMenu.Items.Add(new SliderMenuItem(volume));
      //
    }

    private static void AddAudioDevicesAsMenuItems()
    {
      var sortedDevices = ActiveDevices
        .Select(device => new KeyValuePair<string, string>(GetFormattedDeviceName(device.Key), device.Value))
        .OrderBy(pair => pair.Key);

      foreach (var device in sortedDevices)
      {
        var menuItem = new ToolStripMenuItem();
        menuItem.Text = device.Key;
        menuItem.Click += menuItem_Click;
        menuItem.MergeIndex = BaseMenu.Items.Count;
        menuItem.Tag = device.Value;

        var iconFile = IconUtils.GetBestMatchIconFileName(device.Key);
        if (iconFile.Length > 0)
        {
          try
          {
            menuItem.Image = IconUtils.GetPaddedImage(iconFile);
          }
          catch (Exception e)
          {
            Console.WriteLine(e.Message);
          }
        }

        BaseMenu.Items.Add(menuItem);
      }
    }

    private static void SetupAdditionalMenuItems()
    {
      MenuItemRefresh = new ToolStripMenuItem(Resources.Form1_SetupContextMenu_Refresh);
      MenuItemRefresh.Click += menuItemRefresh_Click;

      MenuItemToggleTheme = new ToolStripMenuItem(Resources.Form1_SetupContextMenu_SwitchTheme);
      MenuItemToggleTheme.MouseHover += menuItemExpandable_Hover;
      MenuItemCreateTheme = new ToolStripMenuItem(Resources.Form1_SetupContextMenu_CreateTheme);
      MenuItemCreateTheme.Click += menuItemCreateTheme_Click;
      SetupThemeSubmenu();

      MenuItemExit = new ToolStripMenuItem(Resources.Form1_SetupContextMenu_Exit);
      MenuItemExit.Click += menuItemExit_Click;

      MenuItemHelp = new ToolStripMenuItem(Resources.Form1_SetupContextMenu_Help);
      MenuItemHelp.Click += menuItemHelp_Click;

      MenuItemMixer = new ToolStripMenuItem(Resources.Form1_SetupContextMenu_VolumeMixer);
      MenuItemMixer.Click += menuItemMixer_Click;
      
      MenuItemDeviceManager = new ToolStripMenuItem(Resources.Form1_SetupContextMenu_ManageDevices);
      MenuItemDeviceManager.Click += menuItemDeviceManager_Click;
      
      MenuItemMode = new ToolStripMenuItem(Resources.Form1_SetupContextMenu_Mode);
      MenuItemMode.MouseHover += menuItemExpandable_Hover;
      SetupDeviceModesSubmenu();

      MenuItemMore = new ToolStripMenuItem(Resources.Form1_SetupContextMenu_More);
    }

    private static void SetupThemeSubmenu()
    {
      var allThemeFiles = GetAllThemesInFolder();
      allThemeFiles.Sort();

      foreach (var themeFile in allThemeFiles)
      {
        var themeName = Path.GetFileNameWithoutExtension(themeFile);
        var theme = new ToolStripMenuItem(themeName);
        theme.Click += menuItemTheme_Click;

        MenuItemToggleTheme.DropDownItems.Add(theme);
      }

      if(allThemeFiles.Count > 0)
      {
        MenuItemToggleTheme.DropDownItems.Add("-");
      }
      MenuItemToggleTheme.DropDownItems.Add(MenuItemCreateTheme);
    }

    private static void SetupDeviceModesSubmenu()
    {
      foreach (var deviceModeName in Enum.GetNames(typeof(DeviceMode)))
      {
        var mode = new ToolStripMenuItem(deviceModeName);
        mode.Click += menuItemMode_Click;

        MenuItemMode.DropDownItems.Add(mode);
      }
    }

    private static List<string> GetAllThemesInFolder()
    {
      return Directory.GetFiles(Directory.GetCurrentDirectory(), "*" + ThemeFileExtension)
      .Select(Path.GetFileName)
      .ToList();
    }

    private static void AddAdditionalMenuItems()
    {
      BaseMenu.Items.Add("-");
      BaseMenu.Items.Add(MenuItemMore);

      var menuItemFields = typeof(MoreMenuItems).GetFields(BindingFlags.Public | BindingFlags.Static);

      foreach (var field in menuItemFields)
      {
        var item = (ToolStripMenuItem)field.GetValue(null);

        if (item == null)
        {
          continue;
        }

        if (item.GetCurrentParent() == null)
        {
          MenuItemMore.DropDownItems.Add(item);
          MenuItemMore.DropDownItems.Add("-");
        } 
      }

      MenuItemMore.DropDownItems.RemoveAt(MenuItemMore.DropDownItems.Count - 1);
    }

    private static string GetFormattedDeviceName(string name)
    {
      var indexOfOpeningParenthesis = name.IndexOf("(", StringComparison.Ordinal);
      var deviceName = name.Substring(indexOfOpeningParenthesis + 1).TrimEnd(')');

      //Handle case where bracketed portion is identical but prefix is unique, e.g., systems with Realtek(R) audio.
      var occurences = ActiveDevices.Keys.Count(key => key.Contains(deviceName));
      if (occurences > 1)
      {
        deviceName = name.Substring(0, indexOfOpeningParenthesis);
      }

      return deviceName;
    }

    private static void SetItemMargins(List<ToolStripMenuItem> items)
    {
      items.ForEach(item =>
      {
        var dropdown = (ToolStripDropDownMenu) item.DropDown;

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
        var dropdown = (ToolStripDropDownMenu) item.DropDown;

        if (dropdown == null)
        {
          return;
        }

        dropdown.ShowImageMargin = false;
        HideImageMarginOnSubItems(item.DropDownItems.OfType<ToolStripMenuItem>().ToList());
      });
    }

    private static void ExpandMenusOnThemeCreation()
    {
      Thread.Sleep(Settings.Current.ThemeSwitchUIRefreshDelay);
      BaseMenu.Show(LastBaseMenuInvokedPosition, ToolStripDropDownDirection.Left);
      MenuItemToggleTheme.GetCurrentParent().Show();
      MenuItemToggleTheme.GetCurrentParent().Focus();
    }

    private static void SetCurrentDeviceIconAndIndicators()
    {
      var currentDevice = DeviceUtils.GetCurrentDefaultDevice();
      var items = BaseMenu.Items.OfType<ToolStripMenuItem>().ToList();
      ActiveMenuItemDevice = items.First(it => (string)it.Tag == currentDevice.DeviceID);
      
      SetActiveMenuItemMarkers();

      IconUtils.SetTrayIcon(ActiveMenuItemDevice.Text, notifyIcon1);
      notifyIcon1.Text = ActiveMenuItemDevice.Text.Trim();
    }

    private static void SetActiveMenuItemMarkers()
    {
      foreach (var item in BaseMenu.Items.OfType<ToolStripMenuItem>().ToList())
      {
        item.ResetBackColor();
      }
      if (ActiveMenuItemDevice != null)
      {
        ActiveMenuItemDevice.BackColor = Theme.GetActiveSelectionColour();
      }

      foreach (var item in MenuItemToggleTheme.DropDownItems.OfType<ToolStripMenuItem>().ToList())
      {
        item.ResetBackColor();

        if (item.Text == Settings.Current.Theme)
        {
          item.BackColor = Theme.GetActiveSelectionColour();
        }
      }

      if (MenuItemMode.HasDropDownItems)
      {
        foreach (ToolStripMenuItem item in MenuItemMode.DropDownItems)
        {
          item.ResetBackColor();

          if (item.Text == Settings.Current.Mode.ToString())
          {
            item.BackColor = Theme.GetActiveSelectionColour();
          }
        }
      }
    }
  }
}