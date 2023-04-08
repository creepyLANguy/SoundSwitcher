using System.Diagnostics;
using System.Windows.Forms;
using static ALsSoundSwitcher.Globals;

namespace ALsSoundSwitcher
{
  public class ProcessUtils
  {
    public static void RunExe(string exeName, string args = "")
    {
      var process = new Process();
      process.StartInfo.FileName = exeName;
      process.StartInfo.RedirectStandardOutput = true;
      process.StartInfo.RedirectStandardError = true;
      process.StartInfo.UseShellExecute = false;
      process.StartInfo.CreateNoWindow = true;
      process.StartInfo.Arguments = args;
      process.Start();
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