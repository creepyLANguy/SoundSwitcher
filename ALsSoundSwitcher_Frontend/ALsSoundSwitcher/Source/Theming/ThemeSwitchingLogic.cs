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
        Theme = new CustomRenderer();

        if (Settings.Current.Theme.Length > 0)
        {
          var themeFilename = ThemeFilenamePattern.Replace("*", Settings.Current.Theme);
          var colourPack = GetColourPackFromThemeFile(themeFilename);
          Theme = new CustomRenderer(colourPack);
        }
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

      ContextMenuAudioDevices.Renderer = Theme;
      SetActiveMenuItemMarker();
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
  }
}