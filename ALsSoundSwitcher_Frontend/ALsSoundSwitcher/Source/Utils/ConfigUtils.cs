using System;
using System.Collections.Generic;
using System.IO;
using static ALsSoundSwitcher.Globals;

namespace ALsSoundSwitcher
{
  public class Config
  {
    private static Dictionary<string, string> dictionary = new Dictionary<string, string>();

    public static bool Read()
    {
      try
      {
        var items = File.ReadAllText(ConfigFile).Split(new[] {"\r\n", "\r", "\n"}, StringSplitOptions.None);
        foreach (var item in items)
        {
          if (item.Length == 0)
          {
            continue;
          }

          var indexOfDelim = item.IndexOf(ConfigDelimiter, StringComparison.Ordinal);
          var key = item.Substring(0, indexOfDelim).Trim().ToLower();
          var val = item.Substring(indexOfDelim + 1).Trim();
          dictionary.Add(key, val);
        }

        string buff;

        if (TryGetValue(Settings.Keys.BalloonTime, out buff))
        {
          Settings.Current.BalloonTime = Convert.ToInt32(buff);
        }

        if (TryGetValue(Settings.Keys.BestNameMatchPercentageMinimum, out buff))
        {
          Settings.Current.BestNameMatchPercentageMinimum = Convert.ToInt32(buff);
        }

        if (TryGetValue(Settings.Keys.DarkMode, out buff))
        {
          Settings.Current.DarkMode = Convert.ToInt32(buff);
        }

        if (TryGetValue(Settings.Keys.DefaultIcon, out buff))
        {
          if (buff.Length > 0)
          {
            Settings.Current.DefaultIcon = buff;
          }
        }
      }
      catch (Exception)
      {
        return false;
      }

      return true;

      bool TryGetValue(string key, out string buff)
      {
        return dictionary.TryGetValue(key.ToLower(), out buff);
      }
    }

    public static void Save()
    {
      WriteAllSettingsToConfigFile();
    }

    private static void WriteAllSettingsToConfigFile()
    {
      using (var sw = File.CreateText(ConfigFile))
      {
        foreach (var field in typeof(Settings.SettingsStruct).GetFields())
        {
          var buff = field.Name + ConfigDelimiter + field.GetValue(Settings.Current);
          sw.WriteLine(buff);
        }
      }
    }
  }
}
