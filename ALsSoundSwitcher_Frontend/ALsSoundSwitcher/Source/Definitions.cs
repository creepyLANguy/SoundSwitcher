namespace ALsSoundSwitcher
{
  public static class Definitions
  {
    public const string GetDevicesExe = "GetPlaybackDevices.exe";
    public const string SetDeviceExe = "SetPlaybackDevice.exe";

    public const string VolumeMixerExe = "sndvol.exe";
    public const string VolumeMixerArgs = "-r 88888888"; //TODO - figure out exactly why this works and do something better.

    public const string DevicesFile = "devices.txt";

    public static int BalloonTime = 1500;
    public static string ActiveMarker = " 🎵";
    public static int BestIconNameMatchPercentageMinimum = 15;

    public const string ConfigFile = "config.ini";
  }
}