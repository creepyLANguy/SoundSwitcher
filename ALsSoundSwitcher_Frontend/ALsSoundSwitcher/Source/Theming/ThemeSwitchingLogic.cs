using System.Collections.Generic;
using System;
using System.Drawing;
using System.IO;
using static ALsSoundSwitcher.Globals;
using ALsSoundSwitcher.Properties;
using System.Windows.Forms;
using Newtonsoft.Json;

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

          if (MenuItems.MenuItemToggleTheme.HasDropDownItems)
          {
            foreach (ToolStripMenuItem item in MenuItems.MenuItemToggleTheme.DropDownItems)
            {
              if (item.Text == Settings.Current.Theme)
              {
                ActiveMenuItemTheme = item;
              }
            }
          }
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

      BaseMenu.Renderer = Theme;
      SetActiveMenuItemMarkers();
    }

    private static ColourPack GetColourPackFromThemeFile(string filename)
    {
      var jsonString = File.ReadAllText(filename);

      var settings = new JsonSerializerSettings
      {
        Converters = new List<JsonConverter> { new ColorConverter() }
      };

      return JsonConvert.DeserializeObject<ColourPack>(jsonString, settings);
    }

    public class ColorConverter : JsonConverter<Color>
    {
      public override Color ReadJson(JsonReader reader, Type objectType, Color existingValue, bool hasExistingValue, JsonSerializer serializer)
      {
        var str = (string)reader.Value;
        if (str == null)
        {
          throw new NullReferenceException();
        }

        var values = str.Split(',');

        var r = int.Parse(values[0].Trim());
        var g = int.Parse(values[1].Trim());
        var b = int.Parse(values[2].Trim());
        
        return Color.FromArgb(r, g, b);
      }

      public override void WriteJson(JsonWriter writer, Color value, JsonSerializer serializer)
      {
        writer.WriteValue($"{value.R}, {value.G}, {value.B}");
      }
    }
  }
}