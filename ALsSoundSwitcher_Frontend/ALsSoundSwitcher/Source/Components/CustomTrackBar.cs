using System.Windows.Forms;
using System;

namespace ALsSoundSwitcher
{
  public class CustomTrackBar : TrackBar
  {
    protected override void OnMouseDown(MouseEventArgs e)
    {
      base.OnMouseDown(e);

      if (e.Button == MouseButtons.Left)
      {
        int selectorWidth = 5;
        double ratio = (double)(e.X - selectorWidth / 2) / (Width - selectorWidth);
        Value = (int)Math.Round(ratio * (Maximum - Minimum)) + Minimum;
      }
    }
  }

  public class SliderMenuItem : ToolStripControlHost
  {
    private CustomTrackBar trackBar;

    public SliderMenuItem() : base(new CustomTrackBar())
    {
      trackBar = Control as CustomTrackBar;
      trackBar.AutoSize = false;
      trackBar.TickStyle = TickStyle.None;
      trackBar.Height = 20;
      trackBar.Width = Globals.BaseMenu.Width;
      trackBar.Minimum = 0;
      trackBar.Maximum = 100;

      Refresh();

      trackBar.ValueChanged += new EventHandler(trackBar_ValueChanged);
    }

    public void Refresh()
    {
      var backgroundColour = Globals.Theme.GetBackgroundColour();
      if (trackBar.BackColor != backgroundColour)
      {
        trackBar.BackColor = backgroundColour;
      }

      var volume = ProcessUtils.RunExe(Globals.SetDeviceExe, "GetVolume");
      if (trackBar.Value != volume)
      {
        trackBar.Value = volume;
      }
    }

    private void trackBar_ValueChanged(object sender, EventArgs e)
    {
      ProcessUtils.RunExe(Globals.SetDeviceExe, "SetVolume " + trackBar.Value);
    }
  }
}