using System.Windows.Forms;
using Microsoft.Win32;
using System;

namespace ALsSoundSwitcher
{
  public class RegistryUtils
  {
    public static bool TryDeleteStartupRegistrySetting(DeviceMode mode)
    {
      try
      {
        var rk = Registry.CurrentUser.OpenSubKey(Globals.StartupRegistryKey, true);

        var productName = Application.ProductName + "_" + mode;

        rk.DeleteValue(productName, false);

        return true;
      }
      catch (Exception ex)
      {
        Console.WriteLine(ex.ToString());
        return false;
      }
    }

    public static bool TrySaveStartupRegistrySetting(DeviceMode mode)
    {
      try
      {
        var rk = Registry.CurrentUser.OpenSubKey(Globals.StartupRegistryKey, true);

        var productName = Application.ProductName + "_" + mode;

        rk.SetValue(productName, Application.ExecutablePath);

        return true;
      }
      catch (Exception ex)
      {
        Console.WriteLine(ex.ToString());
        return false;
      }
    }

    public static bool DoesStartupRegistrySettingAlreadyExistsForThisPath(DeviceMode mode)
    {
      try
      {
        var rk = Registry.CurrentUser.OpenSubKey(Globals.StartupRegistryKey, true);

        var productName = Application.ProductName + "_" + mode;

        var regValue = (string)rk.GetValue(productName);
        return regValue == Application.ExecutablePath;
      }
      catch (Exception ex)
      {
        Console.WriteLine(ex.ToString());
        return false;
      }
    }
  }
}
