using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using static ALsSoundSwitcher.Globals;
using static ALsSoundSwitcher.Settings;

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

      Current = JsonConvert.DeserializeObject<SettingsStruct>(jsonString);

      return true;
    }
    
    public static void Save()
    {
      var jsonDict = new Dictionary<string, object>();

      foreach (var setting in typeof(SettingsStruct).GetFields())
      {
        var key = setting.Name;
        var value = setting.GetValue(Current).ToString();
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
