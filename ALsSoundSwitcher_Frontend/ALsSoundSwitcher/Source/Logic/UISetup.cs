using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using ALsSoundSwitcher.Properties;
using CSCore.CoreAudioAPI;
using static ALsSoundSwitcher.Globals;
using static ALsSoundSwitcher.Globals.MoreMenuItems;
using static ALsSoundSwitcher.Globals.ControlPanelMenuItems;
using static ALsSoundSwitcher.Globals.MouseControlMenuItems;

namespace ALsSoundSwitcher
{
  public partial class Form1
  {
    private static void SetupUI()
    {
      DeviceUtils.GetDeviceList();
      SetupContextMenu();
      CacheCurrentDevice();
      SetCurrentDeviceTrayIcon();
      SetToolTip(ActiveMenuItemDevice.Text);
      SetActiveMenuItemMarkers();
    }

    private static void SetupContextMenu()
    {
      BaseMenu = new ContextMenuStrip();
      BaseMenu.Closing += HandleCloseOnClick;

      AddAudioDevicesAsMenuItems();

      SetupAdditionalMenuItems();

      AddAdditionalMenuItems();

      AddVolumeSlider();

      var items = BaseMenu.Items.OfType<ToolStripMenuItem>().ToList();

      SetItemMargins(items);

      HideImageMarginOnSubItems(items);

      HideImageMarginOnBaseMenuIfNoIcons(items);

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
            menuItem.Image = IconUtils.GetPaddedImage(iconFile);
          }
          catch (Exception ex)
          {
            Console.WriteLine(ex.Message);
          }
        }

        BaseMenu.Items.Add(menuItem);
      }
    }

    private static void SetupAdditionalMenuItems()
    {
      MenuItemExit = new ToolStripMenuItem(Resources.Form1_SetupContextMenu_Exit);
      MenuItemExit.Click += menuItemExit_Click;

      MenuItemHelp = new ToolStripMenuItem(Resources.Form1_SetupContextMenu_Help);
      MenuItemHelp.Click += menuItemHelp_Click;

      MenuItemUpdate = new ToolStripMenuItem(Resources.Form1_SetupContextMenu_Update);
      MenuItemUpdate.Click += menuItemUpdate_Click;
      
      MenuItemRefresh = new ToolStripMenuItem(Resources.Form1_SetupContextMenu_Refresh);
      MenuItemRefresh.Click += menuItemRefresh_Click;

      MenuItemMode = new ToolStripMenuItem(Resources.Form1_SetupContextMenu_Mode);
      MenuItemMode.MouseHover += menuItemExpandable_Hover;
      SetupDeviceModesSubmenu();

      MenuItemToggleTheme = new ToolStripMenuItem(Resources.Form1_SetupContextMenu_SwitchTheme);
      MenuItemToggleTheme.MouseHover += menuItemExpandable_Hover;
      MenuItemCreateTheme = new ToolStripMenuItem(Resources.Form1_SetupContextMenu_CreateTheme);
      MenuItemCreateTheme.Click += menuItemCreateTheme_Click;      
      SetupThemeSubmenu();

      MenuItemMouseControls = new ToolStripMenuItem(Resources.Form1_SetupContextMenu_MouseControls);
      MenuItemMouseControls.MouseHover += menuItemExpandable_Hover;
      SetupMouseControlsSubmenu();

      MenuItemControlPanel = new ToolStripMenuItem(Resources.Form1_SetupContextMenu_ControlPanel);
      MenuItemControlPanel.MouseHover += menuItemExpandable_Hover;
      SetupControlPanelSubmenu();

      MenuItemMore = new ToolStripMenuItem(Resources.Form1_SetupContextMenu_More);
    }

    public static void SetupThemeSubmenu()
    {
      MenuItemToggleTheme.DropDownItems.Clear();

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
      foreach (var deviceMode in DeviceModeDictionary)
      {
        var mode = new ToolStripMenuItem(deviceMode.Value);
        mode.Tag = deviceMode.Key;
        mode.Click += menuItemMode_Click;

        MenuItemMode.DropDownItems.Add(mode);
      }
    }

    private static void SetupMouseControlsSubmenu()
    {
      MenuItemLeftClick = new ToolStripMenuItem(Resources.Form1_SetupMouseControlsSubmenu_Left_Click);
      MenuItemLeftClick.Click += menuItemExpandable_Hover;

      MenuItemMiddleClick = new ToolStripMenuItem(Resources.Form1_SetupMouseControlsSubmenu_Middle_Click);
      MenuItemMiddleClick.Click += menuItemExpandable_Hover;

      MenuItemMouseControls.DropDownItems.Add(MenuItemLeftClick);
      MenuItemMouseControls.DropDownItems.Add(MenuItemMiddleClick);

      var mouseItems = typeof(MouseControlMenuItems).GetFields(BindingFlags.Public | BindingFlags.Static);

      foreach (var mouseFunction in MouseFunctionDictionary)
      {
        foreach (var item in mouseItems)
        {
          var label = mouseFunction.Value;
          var toolStripItem = new ToolStripMenuItem(label);
          toolStripItem.Tag = label;
          toolStripItem.Click += menuItemMouseControlFunction_Click;

          ((ToolStripMenuItem)item.GetValue(null)).DropDownItems.Add(toolStripItem);
        }
      }
    }

    private static void SetupControlPanelSubmenu()
    {
      MenuItemMixer = new ToolStripMenuItem(Resources.Form1_SetupContextMenu_VolumeMixer);
      MenuItemMixer.Click += menuItemMixer_Click;

      MenuItemDeviceManager = new ToolStripMenuItem(Resources.Form1_SetupContextMenu_ManageDevices);
      MenuItemDeviceManager.Click += menuItemDeviceManager_Click;

      MenuItemLaunchOnStartup = new ToolStripMenuItem(Resources.Form1_SetupContextMenu_LaunchOnStartup);
      MenuItemLaunchOnStartup.Click += MenuItemLaunchOnStartup_Click;

      MenuItemPreventAutoSwitch = new ToolStripMenuItem(Resources.Form1_SetupContextMenu_PreventAutoSwitch);
      MenuItemPreventAutoSwitch.Click += menuItemPreventAutoSwitch_Click;

      var menuItemFields = typeof(ControlPanelMenuItems).GetFields(BindingFlags.Public | BindingFlags.Static);
      
      foreach (var field in menuItemFields)
      {
        var item = (ToolStripMenuItem)field.GetValue(null);

        if (item == null)
        {
          continue;
        }

        if (item.GetCurrentParent() == null)
        {
          MenuItemControlPanel.DropDownItems.Add(item);
          MenuItemControlPanel.DropDownItems.Add("-");
        }
      }

      MenuItemControlPanel.DropDownItems.RemoveAt(MenuItemControlPanel.DropDownItems.Count - 1);
    }

    private static List<string> GetAllThemesInFolder() => 
      Directory.GetFiles(Directory.GetCurrentDirectory(), "*" + ThemeFileExtension, SearchOption.AllDirectories)
      .Select(Path.GetFileName)
      .ToList();

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
    
    private static void AddVolumeSlider()
    {
      BaseMenu.Items.Add("-");
      BaseMenu.Items.Add(MenuItemSlider);
    }

    private static string GetFormattedDeviceName(string name)
    {
      var indexOfOpeningParenthesis = name.IndexOf("(", StringComparison.Ordinal);
      var indexOfClosingParenthesis = name.LastIndexOf(")", StringComparison.Ordinal);
      var lengthOfFormattedString = indexOfClosingParenthesis - (indexOfOpeningParenthesis + 1);
      var deviceName = name.Substring(indexOfOpeningParenthesis + 1, lengthOfFormattedString);

      //Handles the case where bracketed portion is identical but prefix is unique, e.g., systems with Realtek(R) audio.
      var occurrences = ActiveDevices.Keys.Count(key => key.Contains(deviceName));
      if (occurrences > 1)
      {
        deviceName = name.Substring(0, indexOfOpeningParenthesis);
      }

      return deviceName;
    }

    public static void SetItemMargins(List<ToolStripMenuItem> items)
    {
      foreach (var item in items)
      {
        item.Padding = new Padding(item.Margin.Right, 5, item.Margin.Right, 5);

        if (item.DropDown is ToolStripDropDownMenu dropdown == false)
        {
          continue;
        }

        SetItemMargins(dropdown.Items.OfType<ToolStripMenuItem>().ToList());
      }
    }

    private static void HideImageMarginOnSubItems(List<ToolStripMenuItem> items)
    {
      foreach (var item in items)
      {
        if (item.DropDown is ToolStripDropDownMenu dropdown == false)
        {
          continue;
        }

        dropdown.ShowImageMargin = false;

        HideImageMarginOnSubItems(item.DropDownItems.OfType<ToolStripMenuItem>().ToList());
      }
    }

    private static void HideImageMarginOnBaseMenuIfNoIcons(List<ToolStripMenuItem> items)
    {
      BaseMenu.ShowImageMargin = items.Any(item => item.Image != null);
    }

    public static void CacheCurrentDevice()
    {
      var currentDevice = DeviceUtils.GetCurrentDefaultDevice();
      var items = BaseMenu.Items.OfType<ToolStripMenuItem>().ToList();
      
      var debugInfo =
        Environment.NewLine +
        (MethodBase.GetCurrentMethod() == null ? "" : MethodBase.GetCurrentMethod()?.Name + "()") +
        Environment.NewLine + Environment.NewLine + currentDevice.FriendlyName +
        Environment.NewLine + "State:\t\t" + currentDevice.DeviceState +
        Environment.NewLine + "IsDisposed:\t" + currentDevice.IsDisposed +
        Environment.NewLine + Environment.NewLine + "Items:" +
        Environment.NewLine + Environment.NewLine + string.Join(Environment.NewLine, items) +
        Environment.NewLine + Environment.NewLine
        ;

      Console.WriteLine(debugInfo);

      try
      {
        ActiveMenuItemDevice = items.First(it => (string)it.Tag == currentDevice.DeviceID);
      }
      catch (Exception e)
      {
        Console.WriteLine(e);

        MessageBox.Show(debugInfo, Application.ProductName);

        throw;
      }
    }

    public static void SetCurrentDeviceTrayIcon()
    {
      IconUtils.SetTrayIcon(ActiveMenuItemDevice.Text);
      notifyIcon1.Visible = true;
    }

    public static void SetToolTip(string text, bool appendVolume = true)
    {
      var shortened = text.Trim();

      var volume = appendVolume ? " - " + DeviceUtils.GetVolume() + "%" : "";

      if (shortened.Length >= ToolTipMaxLength)
      {
        shortened = shortened.Substring(0, ToolTipMaxLength - Ellipsis.Length - volume.Length).Trim() + Ellipsis;     
      }
      notifyIcon1.Text = shortened + volume;
    }

    private static void SetActiveMenuItemMarkers()
    {
      SetBackgroundForBaseMenuItems();

      SetBackgroundForMenuItemToggleTheme();

      SetBackgroundForMenuItemModeSelected();
      
      SetBackgroundForMenuItemPreventAutoSwitch();

      SetBackgroundForMenuItemLaunchOnStartup();

      SetBackgroundForMouseControlSubmenus();
    }
    
    private static void SetBackgroundForBaseMenuItems()
    {
      foreach (var item in BaseMenu.Items.OfType<ToolStripMenuItem>())
      {
        item.ResetBackColor();
      }
      if (ActiveMenuItemDevice != null)
      {
        ActiveMenuItemDevice.BackColor = Theme.ActiveSelectionColor;
      }
    }

    private static void SetBackgroundForMenuItemToggleTheme()
    {
      foreach (var item in MenuItemToggleTheme.DropDownItems.OfType<ToolStripMenuItem>())
      {
        item.ResetBackColor();

        if (item.Text == UserSettings.Theme)
        {
          item.BackColor = Theme.ActiveSelectionColor;
        }
      }
    }

    private static void SetBackgroundForMenuItemModeSelected()
    {
      var currentMode = Enum.GetName(typeof(DeviceMode), UserSettings.Mode);     
      var selectedItem = MenuItemMode.DropDownItems.OfType<ToolStripMenuItem>().First(it => it.Text == currentMode);
      selectedItem.BackColor = Theme.ActiveSelectionColor;
    }

    private static void SetBackgroundForMenuItemPreventAutoSwitch()
    {
      if (UserSettings.PreventAutoSwitch)
      {
        MenuItemPreventAutoSwitch.BackColor = Theme.ActiveSelectionColor;
      }
      else
      {
        MenuItemPreventAutoSwitch.ResetBackColor();
      }
    }
    
    private static void SetBackgroundForMenuItemLaunchOnStartup()
    {
      if (UserSettings.LaunchOnStartup)
      {
        MenuItemLaunchOnStartup.BackColor = Theme.ActiveSelectionColor;
      }
      else
      {
        MenuItemLaunchOnStartup.ResetBackColor();
      }
    }

    private static void SetBackgroundForMouseControlSubmenus()
    {
      SetBackgroundForMouseControlSubmenu(MenuItemLeftClick, MouseFunctionDictionary[UserSettings.LeftClickFunction]);
      SetBackgroundForMouseControlSubmenu(MenuItemMiddleClick, MouseFunctionDictionary[UserSettings.MiddleClickFunction]);

      static void SetBackgroundForMouseControlSubmenu(ToolStripMenuItem menuItem, string label)
      {
        foreach (var item in menuItem.DropDownItems.OfType<ToolStripMenuItem>())
        {
          item.ResetBackColor();

          if (item.Tag.ToString() == label)
          {
            item.BackColor = Theme.ActiveSelectionColor;
          }
        }
      }
    }
  }
}