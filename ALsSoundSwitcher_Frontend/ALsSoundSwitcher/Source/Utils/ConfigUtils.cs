﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
        ProcessJsonSettings();

        //Should only do this *after* json processing else you'll write a blank blob to the file.
        TryUpdateFileStructure();

        return true;
      }
      catch (Exception ex)
      {
        Console.WriteLine(ex.ToString());

        Save();

        return false;
      }
    }

    private static void ProcessJsonSettings()
    {
      var jsonString = File.ReadAllText(ConfigFile);

      UserSettings = JsonConvert.DeserializeObject<Settings>(jsonString);
    }

    public static void TryUpdateFileStructure()
    {
      var jsonString = File.ReadAllText(ConfigFile);
      var keysInFile = JObject.Parse(jsonString).Properties().Select(p => p.Name).Count();
      var keysInStruct = typeof(Settings).GetProperties().Length;

      if (keysInFile != keysInStruct)
      {
        Save();
      }
    }

    public static void Save()
    {
      var jsonDict = new Dictionary<string, object>();

      foreach (var setting in typeof(Settings).GetProperties())
      {
        var key = setting.Name;
        var value = setting.GetValue(UserSettings).ToString();
        jsonDict[key] = value;
      }

      var jsonString = JsonConvert.SerializeObject(jsonDict, Formatting.Indented);

      using var sw = File.CreateText(ConfigFile);
      sw.WriteLine(jsonString);
    }
  }
}
