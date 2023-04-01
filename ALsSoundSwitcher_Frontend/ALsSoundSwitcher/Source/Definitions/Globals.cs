using System.Collections.Generic;
using System.Windows.Forms;

namespace ALsSoundSwitcher
{
  public static class Globals
  {
    public static Form Instance;

    public const string GithubUrl = "https://github.com/creepyLANguy/SoundSwitcher";

    public const string SetDeviceExe = "SetPlaybackDevice.exe";

    public const string VolumeMixerExe = "sndvol.exe";
    public const string VolumeMixerArgs = "-r 88888888"; //TODO - figure out exactly why this works and do something better.
    
    public const string ConfigFile = "config.ini";
    public const string ConfigDelimiter = "=";

    public static bool WeAreSwitching = false;

    public struct ConfigKeys
    {
      public const string BalloonTime = "BalloonTime";
      public const string BestNameMatchPercentageMinimum = "BestNameMatchPercentageMinimum";
      public const string DarkMode = "DarkMode";
    }

    public static Dictionary<string, string> ActiveDevices = new Dictionary<string, string>();

    public static CustomRenderer Theme;

    public static ContextMenuStrip ContextMenuAudioDevices;

    public static ToolStripMenuItem ActiveMenuItem;

    public struct MenuItems
    {
      public static ToolStripMenuItem MenuItemExit;
      public static ToolStripMenuItem MenuItemHelp;
      public static ToolStripMenuItem MenuItemRefresh;
      public static ToolStripMenuItem MenuItemToggleTheme;
      public static ToolStripMenuItem MenuItemMixer;
      public static ToolStripMenuItem MenuItemMore;
    }
  }
}