using System.Diagnostics;
using System.Windows.Forms;

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

    public static void ForceRefreshApplication()
    {
      RunExe(Globals.GetDevicesExe);
      CloseApplication_ThreadSafe();
    }

    public static void CloseApplication_ThreadSafe()
    {
      if (Globals.Instance.InvokeRequired)
      {
        Globals.Instance.Invoke(new MethodInvoker(CloseApplication_ThreadSafe));
      }
      else
      {
        Globals.Instance.Close();
      }
    }
  }
}