using System.Windows.Forms;
using Microsoft.Win32;
using System;

namespace ALsSoundSwitcher
{
  public class RegistryUtils
  {
    private const string Separator = "_";

    private static string GetName(DeviceMode mode) 
      => Application.ProductName + Separator + mode;

    private static RegistryKey GetRegKey() 
      => Registry.CurrentUser.OpenSubKey(Globals.StartupRegistryKey, true);
    
    public static bool TryDeleteStartupRegistrySetting(DeviceMode mode)
    {
      try
      {
        var rk = GetRegKey();
        var name = GetName(mode);
        rk.DeleteValue(name, false);

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
        var rk = GetRegKey();
        var name = GetName(mode);
        rk.SetValue(name, Application.ExecutablePath);

        return true;
      }
      catch (Exception ex)
      {
        Console.WriteLine(ex.ToString());
        return false;
      }
    }

    public static bool DoesStartupRegistrySettingAlreadyExistForThisPath(DeviceMode mode)
    {
      try
      {
        var rk = GetRegKey();
        var name = GetName(mode);
        var regValue = (string)rk.GetValue(name);
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
