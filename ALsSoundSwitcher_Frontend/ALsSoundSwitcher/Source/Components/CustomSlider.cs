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

    public SliderMenuItem() : base(new ColorSlider.ColorSlider())
    {
      trackBar = Control as ColorSlider.ColorSlider;
      trackBar.TickStyle = TickStyle.None;
      trackBar.Height = 20;
      trackBar.Minimum = 0;
      trackBar.Maximum = 100;

      AutoSize = false;

      RefreshColours();
      RefreshValue();

      //TODO - bug: dragging slider can cause jumps back to previous volume value. Doing mouseup helps, but still not a great user experience.
      trackBar.ValueChanged += trackBar_ValueChanged;
    }

    protected override void OnParentChanged(ToolStrip oldParent, ToolStrip newParent)
    {
      base.OnParentChanged(oldParent, newParent);

      if (newParent != null)
      {
        trackBar.Width = Parent.Width - (Parent.Padding.Horizontal * 2);
      }
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
      var volume = DeviceUtils.GetVolume();
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
      var arg = Globals.UserSettings.Mode == DeviceMode.Output ? Globals.SetVolumeArg : Globals.SetMicLevelArg;
      ProcessUtils.RunExe(Globals.SetDeviceExe, arg + trackBar.Value);
      WeAreCurrentlySettingTheVolume = false;
    }
  }
}