using System.Management.Automation;
using System.Windows.Forms;

namespace ALsSoundSwitcher
{
  public class PowershellUtils
  {
    //AL.
    public static bool AudioCmdletsNeedsInstallation()
    {
      using (var powerShell = PowerShell.Create())
      {
        powerShell.AddCommand("Get-Module");
        powerShell.AddParameter("Name", "AudioDeviceCmdlets");

        var results = powerShell.Invoke();
        if (powerShell.HadErrors)
        {
          MessageBox.Show(@"Error checking.");

          return true;
        }

        return results.Count == 0;
      }
    }

    //AL.
    public static bool InstallAudioCmdlets()
    {
      using (var powerShell = PowerShell.Create())
      {
        powerShell.AddCommand("Install-Module");
        powerShell.AddParameter("Name", "AudioDeviceCmdlets");
        powerShell.AddParameter("Scope", "CurrentUser");

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
