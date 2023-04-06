using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System;
using System.Drawing;
using System.IO;
using static ALsSoundSwitcher.Globals;
using ALsSoundSwitcher.Properties;
using System.Windows.Forms;

namespace ALsSoundSwitcher
{
  public partial class Form1
  {
    private static void RefreshUITheme()
    {
      try
      {
        if (Settings.Current.Theme.Length > 0)
        {
          var themeFilename = ThemeFilenamePattern.Replace("*", Settings.Current.Theme);
          var colourPack = GetColourPackFromThemeFile(themeFilename);
          Theme = new CustomRenderer(colourPack);
        }
        else
        {
          Theme = new CustomRenderer();
        }

        ContextMenuAudioDevices.Renderer = Theme;

        SetActiveMenuItemMarker();
      }
      catch (Exception e)
      {
        Console.WriteLine(e.ToString());

        notifyIcon1.ShowBalloonTip(
          Settings.Current.BalloonTime,
          Resources.Form1_PerformSwitch_Error_Switching_Theme,
          e.Message,
          ToolTipIcon.Error
        );
      }
    }

    private static ColourPack GetColourPackFromThemeFile(string filename)
    {
      var colourPack = new ColourPack();

      var jsonString = File.ReadAllText(filename);
      var jsonObject = JObject.Parse(jsonString);
      var jsonDict = jsonObject.ToObject<Dictionary<string, string>>();

      if (jsonDict == null)
      {
        throw new Exception();
      }

      foreach (var kvp in jsonDict)
      {
        var colour = GetColour(kvp.Value);

        var field = typeof(ColourPack).GetField( kvp.Key);

        field.SetValueDirect(__makeref(colourPack), colour);
      }

      return colourPack;
    }

    private static Color GetColour(string rgbString)
    {
      var colorComponents = rgbString.Split(',');

      var red = int.Parse(colorComponents[0]);
      var green = int.Parse(colorComponents[1]);
      var blue = int.Parse(colorComponents[2]);

      var colour = Color.FromArgb(red, green, blue);

      return colour;
    }

    //AL.
    //TODO - use to make next theme
    void al()
    {
      var ActiveSelectionColor  = Color.FromArgb(255, 192, 203);
      var ColorMenuArrow        = Color.FromArgb(18, 18, 18);
      var ColorCheckSquare      = Color.FromArgb(0, 122, 204);
      var ColorCheckMark        = Color.FromArgb(237, 237, 237);
      var ColorMenuItemText     = Color.FromArgb(255, 192, 203);
      var ColorMenuBorder       = Color.FromArgb(255, 192, 203);
      var ColorMenuItemSelected = Color.FromArgb(255, 192, 203);
      var ColorBackground       = Color.FromArgb(255, 192, 203);
      var ColorSeparator        = Color.FromArgb(255, 255, 203);
      var ColorStatusStripGradient = Color.FromArgb(234, 237, 241);
      var ColorButtonSelected   = Color.FromArgb(88, 146, 226);
      var ColorButtonPressed    = Color.FromArgb(110, 160, 230);
    }
  }
}