using System;
using System.Collections.Generic;
using System.IO;

namespace ALsSoundSwitcher
{
  public class ConfigUtils
  {
    private static Dictionary<string, string> dictionary = new Dictionary<string, string>();
    public static bool ReadAllConfig()
    {
      try
      {
        var items = File.ReadAllText(Definitions.ConfigFile).Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);
        foreach (var item in items)
        {
          if (item.Length == 0)
          {
            continue;
          }

          var indexOfDelim = item.IndexOf(Definitions.ConfigDelimiter, StringComparison.Ordinal);
          var key = item.Substring(0, indexOfDelim).Trim().ToLower();
          var val = item.Substring(indexOfDelim + 1).Trim();
          dictionary.Add(key, val);
        }

        string buff;
        if (TryGetValue(Definitions.Config_BalloonTime, out buff))
        {
          Definitions.BalloonTime = Convert.ToInt32(buff);
        }

        if (TryGetValue(Definitions.Config_BestNameMatchPercentageMinimum, out buff))
        {
          Definitions.BestNameMatchPercentageMinimum = Convert.ToInt32(buff);
        }        
        
        if (TryGetValue(Definitions.Config_DarkMode, out buff))
        {
          Definitions.DarkMode = Convert.ToBoolean(buff);
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

    //AL.
    //TODO
    //Write to file
  }
}
