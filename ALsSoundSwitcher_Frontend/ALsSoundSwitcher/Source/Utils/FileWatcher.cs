using System;
using System.IO;
using System.Windows.Forms;
using ALsSoundSwitcher.Properties;

namespace ALsSoundSwitcher
{
  public static class FileWatcher
  {
    public static void Run()
    {
      var fileWatcher = new FileSystemWatcher(Application.StartupPath);
      fileWatcher.NotifyFilter = NotifyFilters.LastWrite;
      fileWatcher.Changed += OnChanged;
      fileWatcher.Filter = Globals.ConfigFile;
      fileWatcher.EnableRaisingEvents = true;
    }

    private static void OnChanged(object sender, FileSystemEventArgs e)
    {
      if (e.ChangeType != WatcherChangeTypes.Changed)
      {
        return;
      }

      try
      {
        HandleFileUpdateEvent(e);
      }
      catch (Exception ex)
      {
        Console.WriteLine(ex);
      }
    }

    private static void HandleFileUpdateEvent(FileSystemEventArgs e)
    {
      var hash = File.ReadAllText(e.FullPath).GetHashCode();
      if (hash == Globals.SettingsHash)
      {
        return;
      }

      Globals.SettingsHash = hash;

      var selection =
        MessageBox.Show(
          Resources.FileWatcher_OnChanged,
          Resources.ALs_Sound_Switcher,
          MessageBoxButtons.YesNo,
          MessageBoxIcon.Question
        );

      if (selection == DialogResult.Yes)
      {
        Application.Restart();
      }
    }
  }
}