namespace ALsSoundSwitcher
{
  public static class Definitions
  {
    public static string getDevicesExe = "GetPlaybackDevices.exe";
    public static string setDeviceExe = "SetPlaybackDevice.exe";

    public static string volumeMixerExe = "sndvol.exe";
    public static string volumeMixerArgs = "-r 88888888"; //TODO - figure out exactly why this works and do something better.

    public static string devicesFile = "devices.txt";

    public static int balloonTime = 1500;

    public static string activeMarker = " *";

    public static int bestIconNameMatchPercentageMinimum = 85;
  }
}