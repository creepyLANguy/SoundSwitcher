namespace ALsSoundSwitcher
{
  public static class Globals
  {
    public const string GetDevicesExe = "GetPlaybackDevices.exe";
    public const string SetDeviceExe = "SetPlaybackDevice.exe";

    public const string VolumeMixerExe = "sndvol.exe";
    public const string VolumeMixerArgs = "-r 88888888"; //TODO - figure out exactly why this works and do something better.

    public const string DevicesFile = "devices.txt";

    public const string ConfigFile = "config.ini";
    public const string ConfigDelimiter = "=";

    public const string GithubUrl = "https://github.com/creepyLANguy/SoundSwitcher";

    public struct ConfigKeys
    {
      public const string BalloonTime = "BalloonTime";
      public const string BestNameMatchPercentageMinimum = "BestNameMatchPercentageMinimum";
      public const string DarkMode = "DarkMode";
    }
  }
}