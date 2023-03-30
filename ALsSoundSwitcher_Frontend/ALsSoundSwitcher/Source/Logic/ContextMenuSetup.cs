using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using ALsSoundSwitcher.Properties;
using static ALsSoundSwitcher.Globals;
using static ALsSoundSwitcher.Globals.MenuItems;

namespace ALsSoundSwitcher
{
  public partial class Form1
  {
    private string  _allAudioDevices = "";

    private void SetupContextMenu()
    {
      ContextMenuAudioDevices = new ContextMenuStrip();

      TryReadingAllAudioDevices();

      AddAudioDevicesAsMenuItems();

      SetupAdditionalMenuItems();

      AddAdditionalMenuItems();

      SetItemMargins(ContextMenuAudioDevices.Items.OfType<ToolStripMenuItem>().ToList());

      HideImageMarginOnSubItems(ContextMenuAudioDevices.Items.OfType<ToolStripMenuItem>().ToList());

      SetTheme();

      notifyIcon1.ContextMenuStrip = ContextMenuAudioDevices;
    }

    private void TryReadingAllAudioDevices()
    {
      try
      {
        _allAudioDevices = File.ReadAllText(DevicesFile);
        //Console.WriteLine(text);

        if (_allAudioDevices.Trim().Length == 0)
        {
          throw new Exception();
        }
      }
      catch (Exception)
      {
        try
        {
          ProcessUtils.RunExe(GetDevicesExe);
          Close();
        }
        catch (Exception)
        {
          MessageBox.Show(Resources.Form1_SetupContextMenu_ + GetDevicesExe);
          Close();
        }
      }
    }

    private void AddAudioDevicesAsMenuItems()
    {
      var index = 0;

      Ar = _allAudioDevices.Trim().Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);

      for (var i = 0; i < Ar.Length; i += 2)
      {
        var name = Ar[i];

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

        ContextMenuAudioDevices.Items.Add(menuItem);
      }
    }

    private static void SetupAdditionalMenuItems()
    {
      MenuItemRefresh = new ToolStripMenuItem(Resources.Form1_SetupContextMenu_R_efresh);
      MenuItemRefresh.Click += menuItemRefresh_Click;

      MenuItemEdit = new ToolStripMenuItem(Resources.Form1_SetupContextMenu_E_dit);
      MenuItemEdit.Click += menuItemEdit_Click;

      MenuItemRestart = new ToolStripMenuItem(Resources.Form1_SetupContextMenu_Res_tart);
      MenuItemRestart.Click += menuItemRestart_Click;


      MenuItemToggleTheme = new ToolStripMenuItem(Resources.Form1_SetupContextMenu_Sw_itchTheme);
      MenuItemToggleTheme.Click += menuItemSwitchTheme_Click;

      MenuItemExit = new ToolStripMenuItem(Resources.Form1_SetupContextMenu_Ex_it);
      MenuItemExit.Click += menuItemExit_Click;

      MenuItemHelp = new ToolStripMenuItem(Resources.Form1_SetupContextMenu_H_elp);
      MenuItemHelp.Click += menuItemHelp_Click;

      MenuItemMixer = new ToolStripMenuItem(Resources.Form1_SetupContextMenu_V_olumeMixer);
      MenuItemMixer.Click += menuItemMixer_Click;

      MenuItemMore = new ToolStripMenuItem(Resources.Form1_SetupContextMenu_M_ore);
    }

    private void AddAdditionalMenuItems()
    {
      MenuItemMore.DropDownItems.Add(MenuItemExit);
      MenuItemMore.DropDownItems.Add("-");
      MenuItemMore.DropDownItems.Add(MenuItemHelp);
      MenuItemMore.DropDownItems.Add("-");
      //menuItemMore.DropDownItems.Add(menuItemEdit);
      //menuItemMore.DropDownItems.Add(menuItemRestart);
      //menuItemMore.DropDownItems.Add("-");
      MenuItemMore.DropDownItems.Add(MenuItemToggleTheme);
      MenuItemMore.DropDownItems.Add("-");
      MenuItemMore.DropDownItems.Add(MenuItemMixer);
      MenuItemMore.DropDownItems.Add("-");
      MenuItemMore.DropDownItems.Add(MenuItemRefresh);

      ContextMenuAudioDevices.Items.Add("-");
      ContextMenuAudioDevices.Items.Add(MenuItemMore);
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
  }
}