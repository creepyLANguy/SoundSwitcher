namespace ALsSoundSwitcher
{
  public class Settings
  {
    public struct SettingsStruct
    {
      public int BalloonTime;
      public int BestNameMatchPercentageMinimum;
      public int ThemeSwitchUIRefreshDelay;
      public string Theme;
      public string DefaultIcon;
      public DeviceMode Mode;
      public bool PreventAutoSwitch;
    }

    public static SettingsStruct Current = new SettingsStruct
    {
      BalloonTime = 1500,
      BestNameMatchPercentageMinimum = 15,
      ThemeSwitchUIRefreshDelay = 75,
      Theme = "Dark",
      DefaultIcon = "",
      Mode = DeviceMode.Output,
      PreventAutoSwitch = true
    };
  }
}
