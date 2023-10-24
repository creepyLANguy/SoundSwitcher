﻿using System.Diagnostics;

namespace ALsSoundSwitcher
{
  public class TestUtils
  {
    public static void RunDebugCode()
    {
      //new ThemeCreator().Show();
    }

    public static void RemoveAudioDeviceCmdletsAndExit()
    {
      var process = new Process();
      process.StartInfo = new ProcessStartInfo
      {
        FileName = "powershell.exe",
        Verb = "runas",
        UseShellExecute = true,
        Arguments = "-Command \"Uninstall-Module -Name AudioDeviceCmdlets\""
      };
      process.Start();
      process.WaitForExit();
      Globals.Instance.Close();
    }

  }
}
