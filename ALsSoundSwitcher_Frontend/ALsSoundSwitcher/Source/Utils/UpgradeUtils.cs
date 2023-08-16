using System;
using System.Net.Http;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using ALsSoundSwitcher.Properties;

namespace ALsSoundSwitcher
{
  public static class UpgradeUtils
  {
    private static UpgradeLog _upgradeLog;

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

    public static void Upgrade()
    {
      const string latestReleaseUrl = Globals.LatestReleaseUrl;
      var localVersion = GetSemanticVersionFromCurrentExecutable();
      var latestVersion = GetSemanticVersionFromReleaseUrl(latestReleaseUrl);
      var upgradePack = new UpgradePack(localVersion, latestVersion, latestReleaseUrl);
      ProcessUpgradePack(upgradePack);
    }

    private static void ProcessUpgradePack(UpgradePack upgradePack)
    {
      if (Equals(upgradePack.OldVersion, upgradePack.NewVersion))
      {
        MessageBox.Show(
          Resources.UpgradeUtils_HandleUpgradeReason_already_on_latest_version + @" (" + upgradePack.NewVersion + @")",
          @"🎧 " + Resources.ALs_Sound_Switcher);
      }
      //AL.
      //TODO - uncomment else
      //else
      {
        try
        { 
          Upgrade(upgradePack);
        }
        catch (Exception ex)
        {
          Console.WriteLine(ex);
          Rollback(upgradePack);
        }
      }
    }

    private static SemanticVersion? GetSemanticVersionFromReleaseUrl(string url)
    {
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
      _upgradeLog.Log(message, showTimestamp);
    }

    private static void Upgrade(UpgradePack upgradePack)
    {
      //AL.
      //TODO

      _upgradeLog = new UpgradeLog();
      _upgradeLog.Show();

      Log("Upgrading from version v" + upgradePack.OldVersion + " to v" + upgradePack.NewVersion);

      Log("Creating backup of old version and user files");
      //CreateBackup("Backup_" + currentVersion)

      Log("Downloading latest release from " + upgradePack.Url);
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

      _upgradeLog.ShowControlBox();
    }

    private static void Rollback(UpgradePack upgradePack)
    {
      //AL.
      //TODO
      //Message("Failed to upgrade to latest version. Rolling back to previous version. We recommend you perform a clean install with the [latest version](https://github.com/creepyLANguy/SoundSwitcher/releases/latest."))
    }
  }
}