using System.Drawing;
using System.Windows.Forms;

namespace ALsSoundSwitcher
{
  public static class Globals
  {
    public static Form Instance;

    public const string GithubUrl = "https://github.com/creepyLANguy/SoundSwitcher";

    public const string GetDevicesExe = "GetPlaybackDevices.exe";
    public const string SetDeviceExe = "SetPlaybackDevice.exe";

    public const string VolumeMixerExe = "sndvol.exe";
    public const string VolumeMixerArgs = "-r 88888888"; //TODO - figure out exactly why this works and do something better.

    public const string DevicesFile = "devices.txt";

    public const string ConfigFile = "config.ini";
    public const string ConfigDelimiter = "=";

    public struct ConfigKeys
    {
      public const string BalloonTime = "BalloonTime";
      public const string BestNameMatchPercentageMinimum = "BestNameMatchPercentageMinimum";
      public const string DarkMode = "DarkMode";
    }

    public static string[] Ar;
    public static int LastIndex = -1;

    public static CustomRenderer Theme;

    public static ContextMenuStrip ContextMenuAudioDevices;

    public struct MenuItems
    {

      public static ToolStripMenuItem MenuItemExit;
      public static ToolStripMenuItem MenuItemHelp;
      public static ToolStripMenuItem MenuItemRefresh;
      public static ToolStripMenuItem MenuItemEdit;
      public static ToolStripMenuItem MenuItemRestart;
      public static ToolStripMenuItem MenuItemToggleTheme;
      public static ToolStripMenuItem MenuItemMixer;
      public static ToolStripMenuItem MenuItemMore;
    }
  }
}