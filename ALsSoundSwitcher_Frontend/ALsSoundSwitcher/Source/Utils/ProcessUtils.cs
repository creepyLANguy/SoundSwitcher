using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using static ALsSoundSwitcher.Globals;

namespace ALsSoundSwitcher
{
  public class ProcessUtils
  {
    public static int RunExe(string exeName, string args = "", bool async = false)
    {
      var process = new Process();
      process.StartInfo.FileName = exeName;
      process.StartInfo.RedirectStandardOutput = true;
      process.StartInfo.RedirectStandardError = true;
      process.StartInfo.UseShellExecute = false;
      process.StartInfo.CreateNoWindow = true;
      process.StartInfo.Arguments = args;
      process.Start();

      if (async)
      {
        return -1;        
      }

      process.WaitForExit();
      return process.ExitCode;
    }

    public static void Restart_ThreadSafe()
    {
      DeviceEnumerator.UnregisterEndpointNotificationCallback((Form1)Instance);

      if (Instance.InvokeRequired)
      {
        Instance.Invoke(new MethodInvoker(Restart_ThreadSafe));
      }
      else
      {
        Application.Restart();
      }
    }

    public static void SetWorkingDirectory()
    {
      var exePath = Process.GetCurrentProcess().MainModule.FileName;
      var exeDir = Path.GetDirectoryName(exePath);
      Directory.SetCurrentDirectory(exeDir);
    }
  }
}