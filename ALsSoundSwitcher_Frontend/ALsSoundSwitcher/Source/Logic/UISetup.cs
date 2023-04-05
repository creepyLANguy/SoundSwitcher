using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using ALsSoundSwitcher.Properties;
using static ALsSoundSwitcher.Globals;
using static ALsSoundSwitcher.Globals.MenuItems;

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
      ContextMenuAudioDevices = new ContextMenuStrip();

      AddAudioDevicesAsMenuItems();

      SetupAdditionalMenuItems();

      AddAdditionalMenuItems();

      SetItemMargins(ContextMenuAudioDevices.Items.OfType<ToolStripMenuItem>().ToList());

      HideImageMarginOnSubItems(ContextMenuAudioDevices.Items.OfType<ToolStripMenuItem>().ToList());

      notifyIcon1.ContextMenuStrip = ContextMenuAudioDevices;

      SetTheme();
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
        menuItem.MergeIndex = ContextMenuAudioDevices.Items.Count;
        menuItem.Tag = device.Value;

        var iconFile = IconUtils.GetBestMatchIcon(device.Key);
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

        ContextMenuAudioDevices.Items.Add(menuItem);
      }
    }

    private static void SetupAdditionalMenuItems()
    {
      MenuItemRefresh = new ToolStripMenuItem(Resources.Form1_SetupContextMenu_Refresh);
      MenuItemRefresh.Click += menuItemRefresh_Click;

      MenuItemToggleTheme = new ToolStripMenuItem(Resources.Form1_SetupContextMenu_SwitchTheme);
      SetupThemeSubmenu();

      MenuItemExit = new ToolStripMenuItem(Resources.Form1_SetupContextMenu_Exit);
      MenuItemExit.Click += menuItemExit_Click;

      MenuItemHelp = new ToolStripMenuItem(Resources.Form1_SetupContextMenu_Help);
      MenuItemHelp.Click += menuItemHelp_Click;

      MenuItemMixer = new ToolStripMenuItem(Resources.Form1_SetupContextMenu_VolumeMixer);
      MenuItemMixer.Click += menuItemMixer_Click;
      
      MenuItemDeviceManager = new ToolStripMenuItem(Resources.Form1_SetupContextMenu_ManageDevices);
      MenuItemDeviceManager.Click += menuItemDeviceManager_Click;

      MenuItemMore = new ToolStripMenuItem(Resources.Form1_SetupContextMenu_More);
    }

    private static void SetupThemeSubmenu()
    {
      //AL.
      //GetAllThemes(); //read all .theme files 
      //foreach theme : 
      var dark = new ToolStripMenuItem("Dark");
      dark.Click += menuItemTheme_Click;
      MenuItemToggleTheme.DropDownItems.Add(dark);

      var light = new ToolStripMenuItem("Light");
      light.Click += menuItemTheme_Click;
      MenuItemToggleTheme.DropDownItems.Add(light);

      var pink = new ToolStripMenuItem("Pink");
      light.Click += menuItemTheme_Click;
      MenuItemToggleTheme.DropDownItems.Add(pink);
    }

    private static void AddAdditionalMenuItems()
    {
      MenuItemMore.DropDownItems.Add(MenuItemExit);
      MenuItemMore.DropDownItems.Add("-");
      MenuItemMore.DropDownItems.Add(MenuItemHelp);
      MenuItemMore.DropDownItems.Add("-");
      MenuItemMore.DropDownItems.Add(MenuItemToggleTheme);
      MenuItemMore.DropDownItems.Add("-");
      MenuItemMore.DropDownItems.Add(MenuItemRefresh);
      MenuItemMore.DropDownItems.Add("-");
      MenuItemMore.DropDownItems.Add(MenuItemMixer);
      MenuItemMore.DropDownItems.Add("-");
      MenuItemMore.DropDownItems.Add(MenuItemDeviceManager);

      ContextMenuAudioDevices.Items.Add("-");
      ContextMenuAudioDevices.Items.Add(MenuItemMore);
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
      var items = ContextMenuAudioDevices.Items.OfType<ToolStripMenuItem>().ToList();
      ActiveMenuItem = items.First(it => (string)it.Tag == currentDevice.DeviceID);
      SetActiveMenuItemMarker();

      IconUtils.SetTrayIcon(ActiveMenuItem.Text, notifyIcon1);
      notifyIcon1.Text = ActiveMenuItem.Text.Trim();
    }
  }
}