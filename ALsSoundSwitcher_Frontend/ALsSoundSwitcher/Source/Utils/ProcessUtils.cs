using System.Diagnostics;
using System.Windows.Forms;
using static ALsSoundSwitcher.Globals;

namespace ALsSoundSwitcher
{
  public class ProcessUtils
  {
    public static int RunExe(string exeName, string args = "")
    {
      var process = new Process();
      process.StartInfo.FileName = exeName;
      process.StartInfo.RedirectStandardOutput = true;
      process.StartInfo.RedirectStandardError = true;
      process.StartInfo.UseShellExecute = false;
      process.StartInfo.CreateNoWindow = true;
      process.StartInfo.Arguments = args;
      process.Start();
      process.WaitForExit();
      return process.ExitCode;
    }

    public static void Restart_ThreadSafe()
    {
      DeviceEnumerator.UnregisterEndpointNotificationCallback(NotificationCallback);

      if (Instance.InvokeRequired)
      {
        Instance.Invoke(new MethodInvoker(Restart_ThreadSafe));
      }
      else
      {
        Application.Restart();
      }
    }
  }
}