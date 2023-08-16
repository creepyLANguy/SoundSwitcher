using System;
using System.Net.Http;
using System.Reflection;
using System.Text.RegularExpressions;

namespace ALsSoundSwitcher
{
  public static class UpgradeUtils
  {
    public static UpgradeReason ShouldUpgrade()
    {
      var currentVersion = GetSemanticVersionFromCurrentExecutable();
      var latestVersion = GetSemanticVersionFromReleaseUrl(Globals.LatestReleaseUrl);

      if (latestVersion.Beta)
      {
        return UpgradeReason.BetaAvailable;
      }
      if (currentVersion.Valid == false && latestVersion.Valid == false)
      {
        return UpgradeReason.CouldNotDetermineBothVersions;
      }
      if (currentVersion.Valid == false)
      {
        return UpgradeReason.CouldNotDetermineLocalVersion;
      }
      if (latestVersion.Valid == false)
      {
        return UpgradeReason.CouldNotDetermineLatestVersion;
      }

      if (Equals(currentVersion, latestVersion))
      {
        return UpgradeReason.AlreadyHaveLatestVersion;
      }
      
      return UpgradeReason.NewerVersionAvailable;
    }

    private static SemanticVersion GetSemanticVersionFromReleaseUrl(string url)
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

      return new SemanticVersion();
    }

    private static SemanticVersion GetSemanticVersionFromCurrentExecutable()
    {
      var entryAssembly = Assembly.GetEntryAssembly();
      var version = entryAssembly?.GetName().Version;

      return version == null ? new SemanticVersion() : new SemanticVersion(version.ToString());
    }
  }
}