using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using ALsSoundSwitcher.Properties;
using Ionic.Zip;

namespace ALsSoundSwitcher
{
  public static class UpgradeUtils
  {
    private static HashSet<SemanticVersion?> _skippedVersions = new HashSet<SemanticVersion?>() ;

    private static UpgradeLog _logWindow;

    private static UpgradePack _pack;

    private struct UpgradePack
    {
      public readonly SemanticVersion? OldVersion;
      public readonly SemanticVersion? NewVersion;
      public readonly string DownloadUrl;
      public readonly string InstallationPath;
      public readonly string Timestamp;

      public UpgradePack(
        SemanticVersion? oldVersion, 
        SemanticVersion? newVersion,
        string downloadUrl, 
        string installationPath,
        string timestamp
        )
      {
        OldVersion = oldVersion;
        NewVersion = newVersion;
        DownloadUrl = downloadUrl;
        InstallationPath = installationPath;
        Timestamp = timestamp;
      }
    }

    public static void Run()
    {
      var baseForm = (Form1) Globals.Instance;
      baseForm.HideTrayIcon();

      SetupUpgradePack();

      if (Equals(_pack.OldVersion, _pack.NewVersion))
      {
        MessageBox.Show(
          Resources.UpgradeUtils_HandleUpgradeReason_already_on_latest_version + @" (" + _pack.NewVersion + @")",
          Resources.ALs_Sound_Switcher
        );
        return;
      }

      SetupUpgradeLog();

      var res = ProcessUpgradePack();
      if (res == false)
      {
        baseForm.ShowTrayIcon();
      }
    }

    private static void SetupUpgradeLog()
    {
      _logWindow = new UpgradeLog();
      _logWindow.Show();

      if (System.Diagnostics.Debugger.IsAttached)
      {
        _logWindow.TopMost = false;
      }
    }

    private static void SetupUpgradePack()
    {
      var localVersion = GetSemanticVersionFromCurrentExecutable();

      var latestVersion = GetSemanticVersionFromUrl(Globals.LatestReleaseUrl);

      var downloadUrl = GetDownloadUrl();

      var installationPath = Directory.GetCurrentDirectory();

      var timestamp = DateTime.Now.ToString("dd-MM-yyyy_HH-mm-ss");

      _pack = new UpgradePack(
        localVersion,
        latestVersion,
        downloadUrl,
        installationPath,
        timestamp
        );
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

    private static SemanticVersion? GetSemanticVersionFromUrl(string url)
    {
      var html = GetHtmlFromUrl(url);
      return GetSemanticVersionFromHtml(html);
    }

    private static string GetHtmlFromUrl(string url)
    {
      var html = string.Empty;

      using (var webClient = new WebClient())
      {
        webClient.Headers.Add("user-agent", Resources.ALs_Sound_Switcher);
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

        try
        {
          html = webClient.DownloadString(url);
        }
        catch (WebException ex)
        {
          Console.WriteLine(ex);
        }
      }

      return html;
    }

    private static SemanticVersion? GetSemanticVersionFromHtml(string html)
    {
      const string versionPattern = @"\/releases\/tag\/v([0-9]+\.[0-9]+\.[0-9]+)";
      var match = Regex.Match(html, versionPattern);

      if (match.Success == false)
      {
        return null;
      }

      var latestVersion = match.Groups[1].Value;
      return new SemanticVersion(latestVersion);
    }

    private static string GetDownloadUrl()
    {
      return Globals.DownloadUrl;
    }

    private static bool ProcessUpgradePack()
    {
      try
      {
        if (Upgrade())
        {
          MakeUpgradeLogDismissible();
          return true;
        }
      }
      catch (Exception ex)
      {
        Console.WriteLine(ex);
      }

      IndicateFailure();
      return false;
    }

    private static void MakeUpgradeLogDismissible()
    {
      _logWindow.ShowControlBox();
    }

    private static void IndicateFailure()
    {
      Log("FAILED to complete upgrade process " + Environment.NewLine +
          "We recommend you perform a clean install with the latest version here: " + Environment.NewLine +
          Globals.LatestReleaseUrl);
    }

    private static void Log(string message, bool showTimestamp = true)
    {
      _logWindow.Log(message, showTimestamp);
    }

    private static bool Backup()
    {
      Log("Backing up current installation");

      var backupName = "backup_v" + _pack.OldVersion + "_" + _pack.Timestamp;
      var backupFolder = Path.Combine(_pack.InstallationPath, backupName);
      var archiveName = backupFolder + ".zip";
      try
      {
        Log("Copying to temp folder: " + Environment.NewLine + backupFolder);
        CopyDirectoryContents(_pack.InstallationPath, backupFolder, true, new[]{".zip"});
        Log("Copy complete");

        Log("Archiving backup to file: " + Environment.NewLine + archiveName);
        CreateArchive(backupFolder, archiveName, _pack.InstallationPath);
        Log("Archiving complete");

        Log("Cleaning up temp folder");
        DeleteFolder(backupFolder);
        Log("Temp folder successfully removed");

        return true;
      }
      catch (Exception ex)
      {
        Console.WriteLine(ex);
        Log("Backup Failed");

        return false;
      }
    }

    private static void CopyDirectoryContents(string sourceDir, string destinationDir, bool overwrite, string[] exclusions)
    {
      var sourceInfo = new DirectoryInfo(sourceDir);
      var destInfo = new DirectoryInfo(destinationDir);

      if (sourceInfo.Exists == false)
      {
        throw new DirectoryNotFoundException("Source directory not found: " + sourceDir);
      }

      if (destInfo.Exists == false)
      {
        destInfo.Create();
      }

      var files = sourceInfo.GetFiles();
      foreach (var file in files)
      {
        if (exclusions.Any(exclusion => file.Name.Contains(exclusion)))
        {
          continue;
        }

        var destinationFilePath = Path.Combine(destinationDir, file.Name);
        file.CopyTo(destinationFilePath, overwrite);
      }

      var dirs = sourceInfo.GetDirectories();
      foreach (var dir in dirs)
      {
        if (destinationDir.Contains(dir.Name))
        {
          continue;
        }

        var destinationSubDir = Path.Combine(destinationDir, dir.Name);
        CopyDirectoryContents(dir.FullName, destinationSubDir, overwrite, exclusions);
      }
    }

    private static void CreateArchive(string inputFolder, string outputFileName, string destinationPath)
    {
      var finalOutputPath = Path.Combine(destinationPath, outputFileName);
      
      using (var zip = new ZipFile())
      {
        if (Directory.Exists(inputFolder) == false)
        {
          throw new DirectoryNotFoundException("Source directory not found: " + inputFolder);
        }

        zip.AddDirectory(inputFolder);
        zip.Save(finalOutputPath);
      }
    }

    private static void DeleteFolder(string folderPath)
    {
      if (Directory.Exists(folderPath) == false)
      {
        throw new DirectoryNotFoundException("Folder not found: " + folderPath);
      }

      Directory.Delete(folderPath, true);
    }

    private static bool FetchLatestRelease()
    {
      Log("Downloading latest release from: " + Environment.NewLine + _pack.DownloadUrl);

      using (var webClient = new WebClient())
      {
        webClient.Headers.Add("user-agent", Resources.ALs_Sound_Switcher);
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

        try
        {
          var fileName = Path.GetFileName(_pack.DownloadUrl);
          var filePath = Path.Combine(_pack.InstallationPath, fileName);
          webClient.DownloadFile(_pack.DownloadUrl ?? throw new InvalidOperationException(), filePath);

          Log(
            "Downloaded:" + Environment.NewLine + 
            fileName + Environment.NewLine + 
            "to" + Environment.NewLine + 
            _pack.InstallationPath);

          return true;
        }
        catch (WebException ex)
        {
          Console.WriteLine(ex);
          Log("Download failed");

          return false;
        }
      }
    }

    private static bool UnzipLatestRelease()
    {
      Log("Extracting latest release");
      
      try
      {
        using (var zip = ZipFile.Read(Path.GetFileName(_pack.DownloadUrl)))
        {
          zip.ExtractAll(_pack.InstallationPath, ExtractExistingFileAction.OverwriteSilently);
        }

        Log("Extraction complete");
        return true;
      }
      catch (Exception ex)
      {
        Console.WriteLine(ex);
        Log("Extraction Failed");

        return false;
      }
    }

    private static bool MarkCurrentExeForDeletion()
    {
      Log("Marking current executable as old");

      PowerShellUtils.Rename(Application.ExecutablePath, _pack.Timestamp + "_outdated");

      Log("Marking complete");

      return true;
    }

    private static bool CopyNewFiles()
    {
      Log("Copying new files");

      try
      {
        CopyDirectoryContents(
          Path.GetFileNameWithoutExtension(_pack.DownloadUrl),
          _pack.InstallationPath,
          true,
          new[] {"settings.json"}
        );

        Log("Copying complete");
        return true;
      }
      catch (Exception ex)
      {
        Console.WriteLine(ex);
        Log("Copying failed");

        return false;
      }
    }

    private static bool Cleanup()
    {
      Log("Cleaning up installation directory");

      try
      {
        var file = Path.GetFileName(_pack.DownloadUrl);
        File.Delete(file);

        var folder = 
          Path.GetFileNameWithoutExtension(_pack.DownloadUrl) ?? throw new InvalidOperationException();
        Directory.Delete(folder, true);

        Log("Cleanup complete");
        return true;
      }
      catch (Exception ex)
      {
        Console.WriteLine(ex);
        Log("Cleanup failed.");

        return false;
      }
    }

    private static bool LaunchNewVersion()
    {
      var exe = Application.ProductName + ".exe";

      Log("Launching new version of " + exe);

      ProcessUtils.RunExe(exe, "", true);

      return true;
    }

    private static bool Upgrade()
    {
      Log("Upgrading from version v" + _pack.OldVersion + " to v" + _pack.NewVersion);

      Log("Installation directory: " + Environment.NewLine + _pack.InstallationPath, false);

      var steps = new List<Func<bool>>
      {
        Backup,
        FetchLatestRelease,
        UnzipLatestRelease,
        MarkCurrentExeForDeletion,
        CopyNewFiles,
        Cleanup,
        LaunchNewVersion,
      };

      foreach (var step in steps)
      {
        if (step())
        {
          continue;
        }

        return false;
      }

      Log("Upgrade SUCCESSFUL!");

      Log(
      "If you encounter issues, please rollback to the zipped backup in your install folder." + Environment.NewLine +
      "Alternatively, perform a clean install with the latest version available here:" + Environment.NewLine +
      Globals.LatestReleaseUrl,
      false);

      return true;
    }

    private static void CleanupOutdatedFiles()
    {
      const int sleepMs = 5000;
      const int maxFailures = 50;

      var failures = 0;

      while (failures < maxFailures)
      {
        var allFiles =
          Directory.GetFiles(Directory.GetCurrentDirectory(), "*" + "_outdated", SearchOption.AllDirectories)
            .Select(Path.GetFileName)
            .ToList();

        if (allFiles.Count == 0)
        {
          break;
        }

        try
        {
          foreach (var file in allFiles)
          {
            File.Delete(file);
          }
        }
        catch (Exception e)
        {
          ++failures;
          Console.WriteLine(e);
          Thread.Sleep(sleepMs);
        }
      }
    }

    public static async void MonitorForOutdatedFilesAndAttemptRemoval_Async()
    {
      await Task.Run(CleanupOutdatedFiles);
    }

    public static async void PollForUpdates_Async()
    {
      while (true)
      {
        await Task.Delay(TimeSpan.FromHours(1));

        var currentVersion = GetSemanticVersionFromCurrentExecutable();
        var latestVersion = GetSemanticVersionFromUrl(Globals.LatestReleaseUrl);

        if (Equals(currentVersion, latestVersion))
        {
          continue;
        }

        if (_skippedVersions.Contains(latestVersion))
        {
          continue;
        }

        var selection = 
          MessageBox.Show(
          @"An update is available." +
          Environment.NewLine + Environment.NewLine + @"Current version: " + currentVersion +
          Environment.NewLine + @"nLatest version: " + latestVersion +
          Environment.NewLine + Environment.NewLine + @"Would you like to update?",
          Resources.ALs_Sound_Switcher,
          MessageBoxButtons.YesNo
        );

        if (selection == DialogResult.Yes)
        { 
          Run();
          return;
        }

        _skippedVersions.Add(latestVersion);
      }
    }
  }
}