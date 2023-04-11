using System;
using System.Diagnostics;
using System.Management.Automation;
using System.Windows.Forms;
using ALsSoundSwitcher.Properties;

namespace ALsSoundSwitcher
{
  public class PowerShellUtils
  {
    public static void SetInputDeviceCmdlet(string deviceId)
    {
      using (var ps = PowerShell.Create())
      {
        ps.AddCommand("Set-AudioDevice");
        ps.AddParameter("-ID", deviceId);
        ps.Invoke();
      }
    }

    public static bool VerifyAudioCmdletsAvailability()
    {
      if (AudioCmdletsNeedsInstallation())
      {
        return InstallAudioCmdlets();
      }

      return true;
    }

    private static bool AudioCmdletsNeedsInstallation()
    {
      using (var powerShell = PowerShell.Create())
      {
        powerShell.AddCommand("Get-Command");
        powerShell.AddParameter("-Name", "Set-AudioDevice");
        var results = powerShell.Invoke();
        return results.Count == 0;
      }
    }

    private static bool InstallAudioCmdlets()
    {
      var result = MessageBox.Show(
        Resources.PowerShellUtils_InstallAudioCmdlets_Message,
        Resources.PowerShellUtils_InstallAudioCmdlets_Caption,
        MessageBoxButtons.OKCancel
      );

      if (result == DialogResult.Cancel)
      {
        return false;
      }

      var startInfo = new ProcessStartInfo
      {
        FileName = "powershell.exe",
        Verb = "runas",
        UseShellExecute = true,
        Arguments = "-Command \"Install-Module -Name AudioDeviceCmdlets -Force\""
      };
      var process = new Process();
      process.StartInfo = startInfo;

      try
      {
        process.Start();
        process.WaitForExit();
      }
      catch (Exception e)
      {
        Console.WriteLine(e);
        return false;
      }

      return true;
    }
  }
}
