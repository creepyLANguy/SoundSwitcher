namespace ALsSoundSwitcher
{
  public class Settings
  {
    public struct SettingsStruct
    {
      public int BalloonTime;
      public int BestNameMatchPercentageMinimum;
      public string Theme;
      public string DefaultIcon;
      public DeviceMode Mode;
      public bool PreventAutoSwitch;
      public bool LaunchOnStartup;
    }

    public static SettingsStruct Current = new SettingsStruct
    {
      BalloonTime = 1500,
      BestNameMatchPercentageMinimum = 15,
      Theme = "Dark",
      DefaultIcon = "",
      Mode = DeviceMode.Output,
      PreventAutoSwitch = false,
      LaunchOnStartup = false,
    };
  }
}
