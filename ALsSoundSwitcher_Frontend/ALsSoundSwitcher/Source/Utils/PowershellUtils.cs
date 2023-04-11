using System.Management.Automation;
using System.Windows.Forms;

namespace ALsSoundSwitcher
{
  public class PowerShellUtils
  {
    public static bool AudioCmdletsNeedsInstallation()
    {
      using (var powerShell = PowerShell.Create())
      {
        powerShell.AddCommand("Get-Command");
        powerShell.AddParameter("-Name", "Set-AudioDevice");
        var results = powerShell.Invoke();
        return results.Count == 0;
      }
    }

    //AL.
    public static bool InstallAudioCmdlets()
    {
      using (var powerShell = PowerShell.Create())
      {
        powerShell.AddCommand("Install-Module");
        powerShell.AddParameter("-Name", "AudioDeviceCmdlets");
        powerShell.AddParameter("-Scope", "CurrentUser");

        var results = powerShell.Invoke();
        if (powerShell.HadErrors)
        {
          MessageBox.Show(@"Error installing.");
          return false;
        }
      }

      return true;
    }

    public static void SetInputDeviceCmdlet(string deviceId)
    {
      using (var ps = PowerShell.Create())
      {
        ps.AddCommand("Set-AudioDevice");
        ps.AddParameter("-ID", deviceId);
        ps.Invoke();
      }
    }
  }
}
