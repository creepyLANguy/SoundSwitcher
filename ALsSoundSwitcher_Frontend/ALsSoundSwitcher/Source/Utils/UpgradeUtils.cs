using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using ALsSoundSwitcher.Properties;
using Ionic.Zip;

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
      public readonly string DownloadUrl;
      public readonly string InstallationPath;

      public UpgradePack(
        SemanticVersion? oldVersion, 
        SemanticVersion? newVersion,
        string downloadUrl, 
        string installationPath
        )
      {
        OldVersion = oldVersion;
        NewVersion = newVersion;
        DownloadUrl = downloadUrl;
        InstallationPath = installationPath;
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
      var html = GetHtmlFromUrl(Globals.LatestReleaseUrl);

      var downloadUrl = GetDownloadUrlFromHtml(html);

      var latestVersion = GetSemanticVersionFromHtml(html);

      var localVersion = GetSemanticVersionFromCurrentExecutable();

      var installationPath = Directory.GetCurrentDirectory();

      Pack = new UpgradePack(localVersion, latestVersion, downloadUrl, installationPath);
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
        if (Upgrade())
        {
          return;
        }
      }
      catch (Exception ex)
      {
        Console.WriteLine(ex);
      }

      IndicateFailure();
    }

    private static void MakeUpgradeLogDismissible()
    {
      LogWindow.ShowControlBox();
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

    private static string GetDownloadUrlFromHtml(string html)
    {
      //AL.
      //TODO - implement
      return "https://github.com/creepyLANguy/SoundSwitcher/releases/download/v2.1.2/ALsSoundSwitcher.zip";
    }

    private static SemanticVersion? GetSemanticVersionFromHtml(string html)
    {
      //AL.
      //TODO - remove
      return new SemanticVersion();
      //
      
      const string versionPattern = @"\/releases\/tag\/v([0-9]+\.[0-9]+\.[0-9]+)";
      var match = Regex.Match(html, versionPattern);

      if (match.Success == false)
      {
        return null;
      }

      var latestVersion = match.Groups[1].Value;
      return new SemanticVersion(latestVersion);
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

    private static void IndicateFailure()
    {
      Log("Failed to upgrade to latest version \n" +
          "We recommend you perform a clean install with the latest version here: \n" +
          Globals.LatestReleaseUrl);
    }

    private static bool Backup()
    {
      Log("Backing up current installation");

      var backupName = "v" + Pack.OldVersion + "_" + DateTime.Now.ToString("dd-MM-yyyy_HH-mm-ss");
      var backupFolder = Path.Combine(Pack.InstallationPath, backupName);
      var archiveName = backupFolder + ".zip";
      try
      {
        Log("Copying to temp folder: \n" + backupFolder);
        CopyDirectoryContents(Pack.InstallationPath, backupFolder, true, new[]{".zip"});
        Log("Copy complete");

        Log("Archiving backup to file: \n" + archiveName);
        CreateArchive(backupFolder, archiveName, Pack.InstallationPath);
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
      Log("Downloading latest release from: \n" + Pack.DownloadUrl);

      using (var webClient = new WebClient())
      {
        webClient.Headers.Add("user-agent", Resources.ALs_Sound_Switcher);
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

        try
        {
          var fileName = Path.GetFileName(Pack.DownloadUrl);
          var filePath = Path.Combine(Pack.InstallationPath, fileName);
          webClient.DownloadFile(Pack.DownloadUrl ?? throw new InvalidOperationException("Pack.DownloadUrl is null"), filePath);
          Log("Downloaded:\n" + fileName + "\nto\n" + Pack.InstallationPath);

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
        using (var zip = ZipFile.Read(Path.GetFileName(Pack.DownloadUrl)))
        {
          zip.ExtractAll(Pack.InstallationPath, ExtractExistingFileAction.OverwriteSilently);
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

    private static bool CopyNewFiles()
    {
      Log("Copying new files");

      try
      {
        CopyDirectoryContents(
          Path.GetFileNameWithoutExtension(Pack.DownloadUrl),
          Pack.InstallationPath,
          true,
          new[] {Path.GetFileName(Application.ExecutablePath), "settings.json"}
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

    //AL.
    //TODO
    private static bool Upgrade()
    {
      Log("Upgrading from version v" + Pack.OldVersion + " to v" + Pack.NewVersion);

      var steps = new List<Func<bool>>
      {
        Backup,
        FetchLatestRelease,
        UnzipLatestRelease,
        CopyNewFiles
      };

      //Log("Marking current executable for deletion");
      //RenameCurrentExecutable()

      //Log("Cleaning up");
      //CleanUp()

      //Log("Relaunching application");
      //Run exe or something. This step is still eh

      foreach (var step in steps)
      {
        if (step())
        {
          continue;
        }

        return false;
      }

      Log("Upgrade successful!");

      Log(
      "If you encounter issues, please rollback to the zipped backup in your install folder.\nAlternatively, perform a clean install with the latest version available here:\n" +
      Globals.LatestReleaseUrl,
      false);

      return true;
    }
  }
}