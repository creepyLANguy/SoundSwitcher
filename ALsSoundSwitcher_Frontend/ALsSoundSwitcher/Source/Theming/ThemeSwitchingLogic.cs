using System.Collections.Generic;
using System;
using System.Drawing;
using System.IO;
using static ALsSoundSwitcher.Globals;
using ALsSoundSwitcher.Properties;
using System.Windows.Forms;
using Newtonsoft.Json;
using System.Linq;

namespace ALsSoundSwitcher
{
  public partial class Form1
  {
    public static void RefreshUITheme()
    {
      try
      {
        Theme = new CustomRenderer();

        if (UserSettings.Theme.Length > 0)
        {
          var filename = UserSettings.Theme + ThemeFileExtension;
          var filePath = Directory.GetFiles(Directory.GetCurrentDirectory(), filename, SearchOption.AllDirectories).First();

          var colourPack = GetColourPackFromThemeFile(filePath);
          Theme = new CustomRenderer(colourPack);
        }
      }
      catch (Exception ex)
      {
        Console.WriteLine(ex.ToString());

        notifyIcon1.ShowBalloonTip(
          UserSettings.BalloonTime,
          Resources.Form1_PerformSwitch_Error_Switching_Theme,
          ex.Message,
          ToolTipIcon.Error
        );
      }

      BaseMenu.Renderer = Theme;
      SetActiveMenuItemMarkers();

      MenuItemSlider.RefreshColours();
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