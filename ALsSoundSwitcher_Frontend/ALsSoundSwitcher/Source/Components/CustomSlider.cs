using System.Windows.Forms;
using System;
using System.Threading;

namespace ALsSoundSwitcher
{
  public class SliderMenuItem : ToolStripControlHost
  {
    public readonly ColorSlider.ColorSlider trackBar;

    private static bool WeAreRefreshingVolumeSliderValue = false;
    private static bool WeAreCurrentlySettingTheVolume = false;

    public SliderMenuItem(int width, int height = 20) : base(new ColorSlider.ColorSlider())
    {
      trackBar = Control as ColorSlider.ColorSlider;
      trackBar.TickStyle = TickStyle.None;
      trackBar.Width = width;
      trackBar.Height = height;
      trackBar.Minimum = 0;
      trackBar.Maximum = 100;

      RefreshColours();
      RefreshValue();

      trackBar.ValueChanged += trackBar_ValueChanged;
    }

    public void RefreshColours()
    {
      var backgroundColour = Globals.Theme.ColorBackground;
      if (backgroundColour != trackBar.BackColor) 
      {
        trackBar.BackColor = backgroundColour;
      }

      var thumbColour = Globals.Theme.ActiveSelectionColor;
      if (thumbColour != trackBar.ThumbInnerColor)
      {
        trackBar.ThumbInnerColor = thumbColour;
        trackBar.ThumbOuterColor = thumbColour;
        trackBar.ThumbPenColor = thumbColour;
      }

      var barColour = ((MenuStripColorTable)Globals.Theme.ColorTable).ColorSeparator;
      if (barColour != trackBar.BarInnerColor)
      {
        trackBar.BarInnerColor = barColour;
        trackBar.BarPenColorBottom = barColour;
        trackBar.BarPenColorTop = barColour;

        trackBar.ElapsedInnerColor = barColour;
        trackBar.ElapsedPenColorBottom = barColour;
        trackBar.ElapsedPenColorTop = barColour;
      }
    }    
    
    public void RefreshValue()
    {
      var arg = Settings.Current.Mode == DeviceMode.Output ? Globals.GetVolumeArg : Globals.GetMicLevelArg;
      var volume = ProcessUtils.RunExe(Globals.SetDeviceExe, arg);
      if (trackBar.Value != volume)
      {
        WeAreRefreshingVolumeSliderValue = true;
        trackBar.Value = volume;
      }
    }

    protected override void OnMouseUp(MouseEventArgs e)
    {
      base.OnMouseUp(e);

      if (e.Button == MouseButtons.Left)
      {
        Globals.BaseMenu.Focus();
      }
    }

    private void trackBar_ValueChanged(object sender, EventArgs e)
    {
      if (WeAreRefreshingVolumeSliderValue)
      {
        WeAreRefreshingVolumeSliderValue = false;
        return;
      }

      if (WeAreCurrentlySettingTheVolume == false)
      {
        new Thread(SetVolume).Start();
      }   
    }

    private void SetVolume()
    {
      WeAreCurrentlySettingTheVolume = true;
      var arg = Settings.Current.Mode == DeviceMode.Output ? Globals.SetVolumeArg : Globals.SetMicLevelArg;
      ProcessUtils.RunExe(Globals.SetDeviceExe, arg + trackBar.Value);
      WeAreCurrentlySettingTheVolume = false;
    }
  }
}