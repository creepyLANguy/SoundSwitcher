using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using ALsSoundSwitcher.Properties;

namespace ALsSoundSwitcher
{
  public partial class Form1
  {
    private string  _allAudioDevices = "";

    private void SetupContextMenu()
    {
      contextMenu = new ContextMenuStrip();

      TryReadingAllAudioDevices();

      AddAudioDevicesAsMenuItems();

      SetupAdditionalMenuItems();

      AddAdditionalMenuItems();

      SetItemMargins(contextMenu.Items.OfType<ToolStripMenuItem>().ToList());

      HideImageMarginOnSubItems(contextMenu.Items.OfType<ToolStripMenuItem>().ToList());

      SetTheme();

      notifyIcon1.ContextMenuStrip = contextMenu;
    }

    private void TryReadingAllAudioDevices()
    {
      try
      {
        _allAudioDevices = File.ReadAllText(Globals.DevicesFile);
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
          ProcessUtils.RunExe(Globals.GetDevicesExe);
          Close();
        }
        catch (Exception)
        {
          MessageBox.Show(Resources.Form1_SetupContextMenu_ + Globals.GetDevicesExe);
          Close();
        }
      }
    }

    private void AddAudioDevicesAsMenuItems()
    {
      var index = 0;

      ar = _allAudioDevices.Trim().Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);

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
    }

    private void SetupAdditionalMenuItems()
    {
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
    }

    private void AddAdditionalMenuItems()
    {
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

      contextMenu.Items.Add("-");
      contextMenu.Items.Add(menuItemMore);
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