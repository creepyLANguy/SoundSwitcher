using System;
using System.Net.Http;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using ALsSoundSwitcher.Properties;

namespace ALsSoundSwitcher
{
  public static class UpgradeUtils
  {
    public static void Upgrade()
    {      
      var localVersion = GetSemanticVersionFromCurrentExecutable();
      var latestVersion = GetSemanticVersionFromReleaseUrl(Globals.LatestReleaseUrl);
      
      var upgradeReason = GetUpgradeReason(localVersion, latestVersion);
      HandleUpgradeReason(upgradeReason, localVersion, latestVersion);
    }

    private static UpgradeReason GetUpgradeReason(SemanticVersion? localVersion, SemanticVersion? latestVersion)
    {
      if (localVersion == null && latestVersion == null)
      {
        return UpgradeReason.CouldNotDetermineBothVersions;
      }
      if (localVersion == null)
      {
        return UpgradeReason.CouldNotDetermineLocalVersion;
      }
      if (latestVersion == null)
      {
        return UpgradeReason.CouldNotDetermineLatestVersion;
      }

      if (Equals(localVersion, latestVersion))
      {
        return UpgradeReason.AlreadyHaveLatestVersion;
      }
      
      return UpgradeReason.NewerVersionAvailable;
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
      catch (HttpRequestException e)
      {
        Console.WriteLine(e);
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

    private static void HandleUpgradeReason(UpgradeReason upgradeReason, SemanticVersion? currentVersion, SemanticVersion? latestVersion)
    {
      switch (upgradeReason)
      {
        case UpgradeReason.AlreadyHaveLatestVersion:
          MessageBox.Show(
            Resources.UpgradeUtils_HandleUpgradeReason_already_on_latest_version + @" (" + currentVersion + @")",
            "🎧 " + Resources.ALs_Sound_Switcher);
          break;
        case UpgradeReason.NewerVersionAvailable:
          PerformUpgrade();
          break;
        case UpgradeReason.CouldNotDetermineBothVersions:
          PerformUpgrade();
          break;
        case UpgradeReason.CouldNotDetermineLocalVersion:
          PerformUpgrade();
          break;
        case UpgradeReason.CouldNotDetermineLatestVersion:
          PerformUpgrade();
          break;
        default:
          throw new ArgumentOutOfRangeException(nameof(upgradeReason), upgradeReason, null);
      }
    }

    private static void PerformUpgrade()
    {
      //AL.
      //TODO
    }
  }
}