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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace ALsSoundSwitcher
{
  public static partial class UpgradeUtils
  {
    private const int CleanupMaxFailures = 50;
    private const int CleanupSleepMs = 5000;

    private const string VersionPattern = @"\/releases\/tag\/v([0-9]+\.[0-9]+\.[0-9]+)";
    private const string DateTimeFormatPattern = "dd-MM-yyyy_HH-mm-ss";
    private const string OutdatedMarker = "_outdated";

    private static readonly string Newline = Environment.NewLine;
    private const string ExecutableExtension = ".exe";
    private const string ZipExtension = ".zip";


    private static HashSet<SemanticVersion> _skippedVersions = [];

    private static UpgradeLog _logWindow;

    private static UpgradePack _pack;

    private struct UpgradePack(
      SemanticVersion oldVersion,
      SemanticVersion newVersion,
      string downloadUrl,
      string installationPath,
      string timestamp)
    {
      public readonly SemanticVersion OldVersion = oldVersion;
      public readonly SemanticVersion NewVersion = newVersion;
      public readonly string DownloadUrl = downloadUrl;
      public readonly string InstallationPath = installationPath;
      public readonly string Timestamp = timestamp;
    }

    public static void Run()
    {
      var baseForm = (Form1) Globals.Instance;
      baseForm.HideTrayIcon();

      SetupUpgradePack();

      if (_pack.OldVersion >= _pack.NewVersion)
      {
        var message = AlreadyHaveLatestVersion + @" ( " + _pack.NewVersion + @" )" +
          Environment.NewLine + Environment.NewLine +
          ForceUpdate;

        var selection =
        MessageBox.Show(
          message,
          Resources.ALs_Sound_Switcher,
          MessageBoxButtons.YesNo
        );

        if (selection == DialogResult.No)
        {
          baseForm.ShowTrayIcon();
          return;
        }
      }

      SetupUpgradeLog();


      var buttonMessage = PostUpgradeCompleteDismissButtonMessage;

      try
      {
        if (Upgrade() == false)
        {
          LogFailure();
          baseForm.ShowTrayIcon();

          buttonMessage = PostUpgradeFailedDismissButtonMessage;
          MakeUpgradeLogDismissible(buttonMessage, true);
          return;
        }
      }
      catch (Exception ex)
      {
        Console.WriteLine(ex);
      }

      MakeUpgradeLogDismissible(buttonMessage);
    }

    private static void SetupUpgradePack()
    {
      var localVersion = GetSemanticVersionFromCurrentExecutable();

      var latestVersion = GetSemanticVersionFromUrl(Globals.LatestReleaseUrl);

      var downloadUrl = GetDownloadUrl();

      var installationPath = Directory.GetCurrentDirectory();

      var timestamp = DateTime.Now.ToString(DateTimeFormatPattern);

      _pack = new UpgradePack(
        localVersion,
        latestVersion,
        downloadUrl,
        installationPath,
        timestamp
        );
    }

    private static SemanticVersion GetSemanticVersionFromCurrentExecutable()
    {
      var entryAssembly = Assembly.GetEntryAssembly();
      var version = entryAssembly?.GetName().Version;

      return version == null ? new SemanticVersion() : new SemanticVersion(version.ToString());
    }

    private static SemanticVersion GetSemanticVersionFromUrl(string url)
    {
      var html = GetHtmlFromUrl(url);
      return GetSemanticVersionFromHtml(html);
    }

    private static string GetHtmlFromUrl(string url)
    {
      var html = string.Empty;

      using var webClient = new WebClient();
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

      return html;
    }

    private static SemanticVersion GetSemanticVersionFromHtml(string html)
    {
      var match = Regex.Match(html, VersionPattern);

      if (match.Success == false)
      {
        return new SemanticVersion();
      }

      var latestVersion = match.Groups[1].Value;
      return new SemanticVersion(latestVersion);
    }

    private static string GetDownloadUrl()
    {
      return Globals.DownloadUrl;
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

    private static bool Upgrade()
    {
      Log("Upgrading from version v" + _pack.OldVersion + " to v" + _pack.NewVersion);

      Log("Installation directory: " + Newline + _pack.InstallationPath, false);

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

      for (var index = 0; index < steps.Count; index++)
      {
        var step = steps[index];
        if (step())
        {
          _logWindow.UpdateProgress((float) (index + 1) / steps.Count * 100);
          continue;
        }

        return false;
      }

      Log(UpgradeComplete);

      Log(
        PostUpgradeAdvice + Newline +
        PostUpgradeAdviceAlternative + Newline +
        Globals.LatestReleaseUrl,
        false);

      return true;
    }

    private static void MakeUpgradeLogDismissible(string buttonMessage, bool hasFailed = false)
    {
      _logWindow.MakeDismissible(buttonMessage, hasFailed);
    }

    private static void LogFailure()
    {
      Log(UpgradeFailed + Newline +
          UpgradeFailedAdvice + Newline +
          Globals.LatestReleaseUrl);
    }

    private static void Log(string message, bool showTimestamp = true)
    {
      _logWindow.Log(message, showTimestamp);
    }

    private static bool Backup()
    {
      Log(BackingUp);

      var backupName = "backup_v" + _pack.OldVersion + "_" + _pack.Timestamp;
      var backupFolder = Path.Combine(_pack.InstallationPath, backupName);
      var archiveName = backupFolder + ZipExtension;
      try
      {
        Log(BackupCopying + Newline + backupFolder);
        CopyDirectoryContents(
          _pack.InstallationPath, 
          backupFolder, 
          true, 
          new[]{ZipExtension, OutdatedMarker}
          );
        Log(CopyingComplete);

        Log(Archiving + Newline + archiveName);
        CreateArchive(backupFolder, archiveName, _pack.InstallationPath);
        Log(ArchivingComplete);

        Log(CleaningTemp);
        DeleteFolder(backupFolder);
        Log(CleaningTempComplete);

        return true;
      }
      catch (Exception ex)
      {
        Console.WriteLine(ex);
        Log(BackupFailed);

        return false;
      }
    }

    private static void CopyDirectoryContents(string sourceDir, string destinationDir, bool overwrite, string[] exclusions)
    {
      var sourceInfo = new DirectoryInfo(sourceDir);
      var destInfo = new DirectoryInfo(destinationDir);

      if (sourceInfo.Exists == false)
      {
        throw new DirectoryNotFoundException(DirectoryNotFound + sourceDir);
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

      using var zip = new ZipFile();
      if (Directory.Exists(inputFolder) == false)
      {
        throw new DirectoryNotFoundException(DirectoryNotFound + inputFolder);
      }

      zip.AddDirectory(inputFolder);
      zip.Save(finalOutputPath);
    }

    private static void DeleteFolder(string folderPath)
    {
      if (Directory.Exists(folderPath) == false)
      {
        throw new DirectoryNotFoundException(FolderNotFound + folderPath);
      }

      Directory.Delete(folderPath, true);
    }

    private static bool FetchLatestRelease()
    {
      Log(Downloading + Newline + _pack.DownloadUrl);

      using var webClient = new WebClient();
      webClient.Headers.Add("user-agent", Resources.ALs_Sound_Switcher);
      ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

      try
      {
        var fileName = Path.GetFileName(_pack.DownloadUrl);
        var filePath = Path.Combine(_pack.InstallationPath, fileName);
        webClient.DownloadFile(_pack.DownloadUrl ?? throw new InvalidOperationException(), filePath);

        Log(
          "Downloaded:" + Newline + 
          fileName + Newline + 
          "to" + Newline + 
          _pack.InstallationPath);

        return true;
      }
      catch (WebException ex)
      {
        Console.WriteLine(ex);
        Log(DownloadFailed);

        return false;
      }
    }

    private static bool UnzipLatestRelease()
    {
      Log(Extracting);
      
      try
      {
        using (var zip = ZipFile.Read(Path.GetFileName(_pack.DownloadUrl)))
        {
          zip.ExtractAll(_pack.InstallationPath, ExtractExistingFileAction.OverwriteSilently);
        }

        Log(ExtractionComplete);
        return true;
      }
      catch (Exception ex)
      {
        Console.WriteLine(ex);
        Log(ExtractionFailed);

        return false;
      }
    }

    private static bool MarkCurrentExeForDeletion()
    {
      Log(MarkingExe);

      PowerShellUtils.Rename(Application.ExecutablePath, _pack.Timestamp + OutdatedMarker);

      Log(MarkingComplete);

      return true;
    }

    private static bool CopyNewFiles()
    {
      Log(CopyingFiles);

      try
      {
        CopyDirectoryContents(
          Path.GetFileNameWithoutExtension(_pack.DownloadUrl),
          _pack.InstallationPath,
          true,
          new[] {Globals.ConfigFile}
        );

        Log(CopyingComplete);
        return true;
      }
      catch (Exception ex)
      {
        Console.WriteLine(ex);
        Log(CopyingFailed);

        return false;
      }
    }

    private static bool Cleanup()
    {
      Log(CleaningUp);

      try
      {
        var file = Path.GetFileName(_pack.DownloadUrl);
        File.Delete(file);

        var folder = 
          Path.GetFileNameWithoutExtension(_pack.DownloadUrl) ?? throw new InvalidOperationException();
        Directory.Delete(folder, true);

        Log(CleanupComplete);
        return true;
      }
      catch (Exception ex)
      {
        Console.WriteLine(ex);
        Log(CleanupFailed);

        return false;
      }
    }

    private static bool LaunchNewVersion()
    {
      var exe = Application.ProductName + ExecutableExtension;

      Log(LaunchingNewVersionOf + exe);

      ProcessUtils.RunExe(exe, "", true);

      return true;
    }

    public static async void MonitorForOutdatedFilesAndAttemptRemoval_Async()
    {
      await Task.Run(CleanupOutdatedFiles);
    }

    private static void CleanupOutdatedFiles()
    {
      var failures = 0;

      while (failures < CleanupMaxFailures)
      {
        var allFiles =
          Directory.GetFiles(Directory.GetCurrentDirectory(), "*" + OutdatedMarker + "*", SearchOption.AllDirectories)
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
          Thread.Sleep(CleanupSleepMs);
        }
      }
    }

    public static async void PollForUpdates_Async()
    {
      var upgradePollingTimeSpan = GetUpgradePollingTimeSpan();

      while (true)
      {
        var currentVersion = GetSemanticVersionFromCurrentExecutable();
        var latestVersion = GetSemanticVersionFromUrl(Globals.LatestReleaseUrl);

        if (ShouldPromptUser(currentVersion, latestVersion) == false)
        {
          await Task.Delay(upgradePollingTimeSpan);
          continue;
        }

        var selection = 
          MessageBox.Show(
          @"An update is available." +
          Newline + (currentVersion.IsValid ? "" : Newline + @"Current version: " + currentVersion) +
          Newline + @"Latest version: " + latestVersion +
          Newline + Newline + @"Would you like to update?",
          Resources.ALs_Sound_Switcher,
          MessageBoxButtons.YesNo
        );

        if (selection == DialogResult.No)
        {
          _skippedVersions.Add(latestVersion);
        }
        else if (selection == DialogResult.Yes)
        {
          Run();
          return;
        }
      }
    }

    private static TimeSpan GetUpgradePollingTimeSpan()
    {
      char[] delimiters = { 'd', 'h', 'm', 's' };

      var parts = Globals.UserSettings.UpgradePollingTime
        .Split(delimiters, StringSplitOptions.RemoveEmptyEntries)
        .Select(int.Parse).ToArray();

      return new TimeSpan(parts[0], parts[1], parts[2], parts[3]);
    }

    public static bool ShouldPromptUser(SemanticVersion currentVersion, SemanticVersion latestVersion)
    {
      return latestVersion.IsValid &&
             currentVersion < latestVersion &&
             _skippedVersions.Contains(latestVersion) == false;
    }
  }
}