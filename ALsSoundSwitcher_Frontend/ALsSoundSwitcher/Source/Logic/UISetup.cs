using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using ALsSoundSwitcher.Properties;
using static ALsSoundSwitcher.Globals;
using static ALsSoundSwitcher.Globals.MoreMenuItems;

namespace ALsSoundSwitcher
{
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
            menuItem.Image = IconUtils.GetSquarePaddedImage(iconFile);
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
      return Directory.GetFiles(Directory.GetCurrentDirectory(), ThemeFilenamePattern)
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

      if (MenuItemToggleTheme.HasDropDownItems == false)
      {
        var index = MenuItemMore.DropDownItems.IndexOf(MenuItemToggleTheme);
        MenuItemMore.DropDownItems.RemoveAt(index);
        MenuItemMore.DropDownItems.RemoveAt(index);
      }

      MenuItemMore.DropDownItems.RemoveAt(MenuItemMore.DropDownItems.Count - 1);
    }

    private static string GetFormattedDeviceName(string name)
    {
      var indexOfOpeningParenthesis = name.IndexOf("(", StringComparison.Ordinal);
      var deviceName = name.Substring(indexOfOpeningParenthesis + 1).TrimEnd(')');
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

      if (MenuItemToggleTheme.HasDropDownItems)
      {
        foreach (ToolStripMenuItem item in MenuItemToggleTheme.DropDownItems)
        {
          item.ResetBackColor();

          if (item.Text == Settings.Current.Theme)
          {
            item.BackColor = Theme.GetActiveSelectionColour();
          }
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