using Newtonsoft.Json;
using System.ComponentModel;

namespace ALsSoundSwitcher
{
  public struct Settings
  {
    private const int DefaultBalloonTime = 1500;
    private const int DefaultBestNameMatchPercentageMinimum = 15;
    private const string DefaultTheme = "Dark";
    private const string DefaultDefaultIcon = "";
    private const DeviceMode DefaultMode = DeviceMode.Output;
    private const bool DefaultPreventAutoSwitch = false;
    private const bool DefaultLaunchOnStartup = false;
    private const MouseControlFunction DefaultLeftClickFunction = MouseControlFunction.Switch_Next_Device;
    private const MouseControlFunction DefaultMiddleClickFunction = MouseControlFunction.Volume_Mixer;
    private const string DefaultUpgradePollingTime = "0d1h0m0s";

    [DefaultValue(DefaultBalloonTime)]
    [JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate)]
    public int BalloonTime { get; set; }

    [DefaultValue(DefaultBestNameMatchPercentageMinimum)]
    [JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate)]
    public int BestNameMatchPercentageMinimum { get; set; }

    [DefaultValue(DefaultTheme)]
    [JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate)]
    public string Theme { get; set; }

    [DefaultValue(DefaultDefaultIcon)]
    [JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate)]
    public string DefaultIcon { get; set; }

    [DefaultValue(DefaultMode)]
    [JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate)]
    public DeviceMode Mode { get; set; }

    [DefaultValue(DefaultPreventAutoSwitch)]
    [JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate)] 
    public bool PreventAutoSwitch { get; set; }

    [DefaultValue(DefaultLaunchOnStartup)]
    [JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate)]
    public bool LaunchOnStartup { get; set; }

    [DefaultValue(DefaultLeftClickFunction)]
    [JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate)]
    public MouseControlFunction LeftClickFunction { get; set; }

    [DefaultValue(DefaultMiddleClickFunction)]
    [JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate)] 
    public MouseControlFunction MiddleClickFunction { get; set; }

    [DefaultValue(DefaultUpgradePollingTime)]
    [JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate)]
    public string UpgradePollingTime { get; set; }

    public Settings()
    {
      BalloonTime = DefaultBalloonTime;
      BestNameMatchPercentageMinimum = DefaultBestNameMatchPercentageMinimum;
      Theme = DefaultTheme;
      DefaultIcon = DefaultDefaultIcon;
      Mode = DefaultMode;
      PreventAutoSwitch = DefaultPreventAutoSwitch;
      LaunchOnStartup = DefaultLaunchOnStartup;
      LeftClickFunction = DefaultLeftClickFunction;
      MiddleClickFunction = DefaultMiddleClickFunction;
      UpgradePollingTime = DefaultUpgradePollingTime;
    }
  }
}
