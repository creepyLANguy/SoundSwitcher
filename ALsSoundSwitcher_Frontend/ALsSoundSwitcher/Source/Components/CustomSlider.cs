using System.Windows.Forms;
using System;

namespace ALsSoundSwitcher
{
  public class SliderMenuItem : ToolStripControlHost
  {
    public readonly ColorSlider.ColorSlider trackBar;

    public SliderMenuItem() : base(new ColorSlider.ColorSlider())
    {
      trackBar = Control as ColorSlider.ColorSlider;
      trackBar.TickStyle = TickStyle.None;
      trackBar.Height = 20;
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
        Globals.WeAreRefreshingVolumeSliderValue = true;
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
      if (Globals.WeAreRefreshingVolumeSliderValue)
      {
        Globals.WeAreRefreshingVolumeSliderValue = false;
        return;
      }

      var arg = Settings.Current.Mode == DeviceMode.Output ? Globals.SetVolumeArg : Globals.SetMicLevelArg;
      ProcessUtils.RunExe(Globals.SetDeviceExe, arg + trackBar.Value);      
    }
  }
}