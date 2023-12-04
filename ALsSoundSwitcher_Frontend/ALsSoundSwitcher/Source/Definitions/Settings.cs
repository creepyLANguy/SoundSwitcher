using Newtonsoft.Json;
using System.ComponentModel;

namespace ALsSoundSwitcher
{
  public struct Settings
  {
    [DefaultValue(1500)]
    [JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate)]
    public int BalloonTime { get; set; }

    [DefaultValue(15)]
    [JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate)]
    public int BestNameMatchPercentageMinimum { get; set; }

    [DefaultValue("Dark")]
    [JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate)]
    public string Theme { get; set; }

    [DefaultValue("")]
    [JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate)]
    public string DefaultIcon { get; set; }

    [DefaultValue(DeviceMode.Output)]
    [JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate)]
    public DeviceMode Mode { get; set; }

    [DefaultValue(false)]
    [JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate)] 
    public bool PreventAutoSwitch { get; set; }

    [DefaultValue(false)]
    [JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate)]
    public bool LaunchOnStartup { get; set; }

    [DefaultValue(MouseControlFunction.Switch_Next_Device)]
    [JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate)]
    public MouseControlFunction LeftClickFunction { get; set; }

    [DefaultValue(MouseControlFunction.Volume_Mixer)]
    [JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate)] 
    public MouseControlFunction MiddleClickFunction { get; set; }

    [DefaultValue("0d1h0m0s")]
    [JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate)]
    public string UpgradePollingTime { get; set; }
  }
}
