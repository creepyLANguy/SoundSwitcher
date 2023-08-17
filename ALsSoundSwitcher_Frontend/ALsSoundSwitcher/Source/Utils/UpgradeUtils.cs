using System;
using System.IO;
using System.Net.Http;
using System.Reflection;
using System.Text.RegularExpressions;
using System.IO.Compression;
using ALsSoundSwitcher.Properties;

namespace ALsSoundSwitcher
{
  public static class UpgradeUtils
  {
    private static UpgradeLog LogWindow;

    private static UpgradePack Pack;

    private struct UpgradePack
    {
      public readonly SemanticVersion? OldVersion;
      public readonly SemanticVersion? NewVersion;
      public string Url;

      public UpgradePack(SemanticVersion? oldVersion, SemanticVersion? newVersion, string url)
      {
        OldVersion = oldVersion;
        NewVersion = newVersion;
        Url = url;
      }
    }

    public static void Run()
    {
      SetupUpgradeLog();

      SetupUpgradePack();

      ProcessUpgradePack();

      MakeUpgradeLogDismissible();
    }

    private static void SetupUpgradeLog()
    {
      LogWindow = new UpgradeLog();
      LogWindow.Show();

      if (System.Diagnostics.Debugger.IsAttached)
      {
        LogWindow.TopMost = false;
      }
    }

    private static void SetupUpgradePack()
    {
      const string latestReleaseUrl = Globals.LatestReleaseUrl;
      var localVersion = GetSemanticVersionFromCurrentExecutable();
      var latestVersion = GetSemanticVersionFromReleaseUrl(latestReleaseUrl);
      Pack = new UpgradePack(localVersion, latestVersion, latestReleaseUrl);
    }

    private static void ProcessUpgradePack()
    {
      if (Equals(Pack.OldVersion, Pack.NewVersion))
      {
        Log(Resources.UpgradeUtils_HandleUpgradeReason_already_on_latest_version + @" (" + Pack.NewVersion + @")");
        return;
      }

      try
      { 
        Upgrade();
      }
      catch (Exception ex)
      {
        Console.WriteLine(ex);
        Rollback();
      }
    }

    private static void MakeUpgradeLogDismissible()
    {
      LogWindow.ShowControlBox();
    }

    private static SemanticVersion? GetSemanticVersionFromReleaseUrl(string url)
    {
      //AL.
      return new SemanticVersion();
      //
      try
      {
        using (var client = new HttpClient())
        {
          System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;

          var response = client.GetAsync(url).Result;
          response.EnsureSuccessStatusCode();

          var htmlContent = response.Content.ReadAsStringAsync().Result;

          const string versionPattern = @"\/releases\/tag\/v([0-9]+\.[0-9]+\.[0-9]+)";
          var match = Regex.Match(htmlContent, versionPattern);

          if (match.Success == false)
          {
            throw new Exception("No version information found.");
          }

          var latestVersion = match.Groups[1].Value;
          return new SemanticVersion(latestVersion);
        }
      }
      catch (HttpRequestException ex)
      {
        Console.WriteLine(ex);
      }

      return null;
    }

    private static SemanticVersion? GetSemanticVersionFromCurrentExecutable()
    {
      var entryAssembly = Assembly.GetEntryAssembly();
      var version = entryAssembly?.GetName().Version;

      if (version == null)
      {
        return null;
      }
      
      return new SemanticVersion(version.ToString());
    }

    private static void Log(string message, bool showTimestamp = true)
    {
      LogWindow.Log(message, showTimestamp);
    }

    private static void Backup()
    {
      var fileName = "v" + Pack.OldVersion + "_" + DateTime.Now.ToString("dd-MM-yyyy_HH:mm:ss") + ".zip";

      var directory = Directory.GetCurrentDirectory();

      //AL.
      var zipFilePath = Path.Combine(directory, fileName).Replace("\\","/");

      try
      {
        using (var zipToCreate = new FileStream(zipFilePath, FileMode.Create))
        {
          using (var archive = new ZipArchive(zipToCreate, ZipArchiveMode.Create))
          {
            foreach (var filePath in Directory.GetFiles(directory, "*", SearchOption.AllDirectories))
            {
              if (filePath.Contains(".zip"))
              {
                continue;
              }

              var relativePath = filePath.Substring(directory.Length + 1);
              var entry = archive.CreateEntry(relativePath);

              using (var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
              using (var entryStream = entry.Open())
              {
                fileStream.CopyTo(entryStream);
              }
            }
          }
        }

        Console.WriteLine(@"Folder zipped successfully.");
      }
      catch (Exception ex)
      {
        Console.WriteLine($@"An error occurred: {ex.Message}");
      }
    }

    //AL.
    //TODO
    private static void Upgrade()
    {
      Log("Upgrading from version v" + Pack.OldVersion + " to v" + Pack.NewVersion);

      Backup();

      Log("Downloading latest release from " + Pack.Url);
      //FetchLatestRelease()

      Log("Extracting latest release");
      //UnzipContents()

      Log("Reading file manifest");
      //ReadManifest()

      Log("Attempting full merge");
      //AttemptBlanketCopyOfFilesButDoNotOverwrite([])

      Log("Replacing required files");
      //ReplaceFiles([])

      Log("Merging file contents and migrating user settings");
      //MergeFileContents([])

      Log("Cleaning up");
      //Delete([])

      Log("Relaunching application");
      //Run exe

      Log("Upgrade successful");

      Log(
        "If you encounter issues, please rollback to the zipped backup in your install folder, or perform a clean install with the latest version available here: https://github.com/creepyLANguy/SoundSwitcher/releases/latest",
        false
        );
    }

    private static void Rollback()
    {
      //AL.
      //TODO
      //Message("Failed to upgrade to latest version. Rolling back to previous version. We recommend you perform a clean install with the [latest version](https://github.com/creepyLANguy/SoundSwitcher/releases/latest."))
    }
  }
}