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
    public static bool Read()
    {
      try
      {
        return ProcessJsonSettings();
      }
      catch (Exception e)
      {
        Console.WriteLine(e.ToString());

        Save();

        return false;
      }
    }

    private static bool ProcessJsonSettings()
    {
      var jsonString = File.ReadAllText(ConfigFile);
      var jsonObject = JObject.Parse(jsonString);
      var jsonDict = jsonObject.ToObject<Dictionary<string, string>>();

      if (jsonDict == null)
      {
        throw new Exception();
      }

      string buff;

      if (jsonDict.TryGetValue(Settings.Keys.BalloonTime, out buff))
      {
        Settings.Current.BalloonTime = Convert.ToInt32(buff);
      }

      if (jsonDict.TryGetValue(Settings.Keys.BestNameMatchPercentageMinimum, out buff))
      {
        Settings.Current.BestNameMatchPercentageMinimum = Convert.ToInt32(buff);
      }

      if (jsonDict.TryGetValue(Settings.Keys.Theme, out buff))
      {
        if (buff.Length > 0)
        {
          Settings.Current.Theme = buff;
        }
      }

      if (jsonDict.TryGetValue(Settings.Keys.DefaultIcon, out buff))
      {
        if (buff.Length > 0)
        {
          Settings.Current.DefaultIcon = buff;
        }
      }

      return true;
    }
    
    public static void Save()
    {
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
