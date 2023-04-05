using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using static ALsSoundSwitcher.Globals;

namespace ALsSoundSwitcher
{
  public class Config
  {
    public enum ConfigType
    {
      Ini,
      Json
    }

    public static bool Read(ConfigType configType = ConfigType.Json)
    {
      switch (configType)
      {
        case ConfigType.Ini:
          return Read_INI();
        case ConfigType.Json:
          //AL.
          //return Read_JSON();
          return Read_INI();
        default:
          return false;
      }
    }
    
    public static void Save(ConfigType configType = ConfigType.Json)
    {
      switch (configType)
      {
        case ConfigType.Ini:
          Save_INI();
          break;
        case ConfigType.Json:
          Save_JSON();
          break;
      }
    }

    private static bool Read_INI()
    { 
      var dictionary = new Dictionary<string, string>();

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

    private static void Save_INI()
    {
      using (var sw = File.CreateText(ConfigFile))
      {
        foreach (var setting in typeof(Settings.SettingsStruct).GetFields())
        {
          var buff = setting.Name + ConfigDelimiter + setting.GetValue(Settings.Current);
          sw.WriteLine(buff);
        }
      }
    }

    //AL.
    //TODO - implement
    private static bool Read_JSON()
    {
      var jsonString = File.ReadAllText("settings.json");
      var jsonObject = JObject.Parse(jsonString);
      var jsonDict = jsonObject.ToObject<Dictionary<string, string>>();

      string balloonTime = jsonDict["BalloonTime"];
      string defaultIcon = jsonDict["DefaultIcon"];

      return false;
    }

    //AL.
    //TODO - implement
    private static void Save_JSON()
    {
      //var inputList = new List<string>
      //{
      //  "BalloonTime=1500",
      //  "BestNameMatchPercentageMinimum=25",
      //  "DarkMode=1",
      //  "DefaultIcon="
      //};

      var jsonDict = new Dictionary<string, object>();

      foreach (var setting in typeof(Settings.SettingsStruct).GetFields())
      {
        var key = setting.Name;
        var value = setting.GetValue(Settings.Current).ToString();
        jsonDict[key] = value;
      }

      var jsonString = JsonConvert.SerializeObject(jsonDict, Formatting.Indented);

      using (var sw = File.CreateText(ConfigFile))
      {
        sw.WriteLine(jsonString);
      }
    }
  }
}
