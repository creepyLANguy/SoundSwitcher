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
    }

    public static SettingsStruct Current = new SettingsStruct
    {
      BalloonTime = 1500,
      BestNameMatchPercentageMinimum = 15,
      Theme = "Dark.theme",
      DefaultIcon = ""
    };

    public struct Keys
    {
      public const string BalloonTime = "BalloonTime";
      public const string BestNameMatchPercentageMinimum = "BestNameMatchPercentageMinimum";
      public const string Theme = "Theme";
      public const string DefaultIcon = "DefaultIcon";
    }
  }
}
