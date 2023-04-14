using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Xml.Linq;
using ALsSoundSwitcher.Properties;

namespace ALsSoundSwitcher
{
  public partial class ThemeCreator : Form
  {
    private const string TextBoxDefault = "Enter a name for your theme...";

    private static readonly ColourBundle[] AllColourBundles = {
      new ColourBundle(Color.FromArgb(43, 43, 43), "ColorBackground", Resources.mask_background),
      new ColourBundle(Color.FromArgb(61, 61, 67), "ColorMenuBorder", Resources.mask_border),
      new ColourBundle(Color.FromArgb(47, 79, 79), "ActiveSelectionColor", Resources.mask_active),
      new ColourBundle(Color.FromArgb(76, 76, 77), "ColorMenuItemSelected", Resources.mask_selected),
      new ColourBundle(Color.FromArgb(61, 61, 67), "ColorSeparator", Resources.mask_separator),
      new ColourBundle(Color.FromArgb(237, 237, 237), "ColorMenuArrow", Resources.mask_arrow),
      new ColourBundle(Color.FromArgb(237, 237, 237), "ColorMenuItemText", Resources.mask_text)
    };

    public ThemeCreator()
    {
      InitializeComponent();

      SetupButtons();

      textBox_ThemeName.Text = TextBoxDefault;

      GenerateFullPreview();
    }

    private void SetupButtons()
    {
      foreach (var bundle in AllColourBundles)
      {
        var button = Controls.Find($"btn_{bundle.JsonKey}", true)[0];
        button.BackColor = bundle.Colour;
      }
    }

    public void GenerateFullPreview()
    {
      foreach (var bundle in AllColourBundles)
      {
        UpdatePreview(bundle.Mask, bundle.Colour);
      }
    }

    public void UpdatePreview(Bitmap mask, Color colour)
    {
      var buffer = (Bitmap)pictureBox1.Image;

      for (var x = 0; x < mask.Width; x++)
      {
        for (var y = 0; y < mask.Height; y++)
        {
          var pixelColor = mask.GetPixel(x, y);
          if (ColourIsWithinTolerance(pixelColor, Color.Black, 0.10f))
          {
            buffer.SetPixel(x, y, colour);
          }
        }
      }

      pictureBox1.Image = buffer;
    }

    private static bool ColourIsWithinTolerance(Color color1, Color color2, float tolerance)
    {
      var maxDiff = 255 * tolerance;
      return Math.Abs(color1.R - color2.R) <= maxDiff &&
             Math.Abs(color1.G - color2.G) <= maxDiff &&
             Math.Abs(color1.B - color2.B) <= maxDiff;
    }

    private void HandleButtonClick(object sender, EventArgs e)
    {
      var button = (Button)sender;
      var bundle = AllColourBundles.First(x => button.Name.EndsWith(x.JsonKey));

      var colorDialog = new ColorDialog();
      colorDialog.Color = button.BackColor;
      if (colorDialog.ShowDialog() == DialogResult.OK)
      {
        button.BackColor = colorDialog.Color;
        bundle.Colour = colorDialog.Color;
        UpdatePreview(bundle.Mask, bundle.Colour);
        
        //AL.
        //TODO 
        //Should fix the masks... 
        //Also, must adjust the masks on the right border by 1px
        UpdatePreview(Resources.mask_text, btn_ColorMenuItemText.BackColor);
        //
      }
    }

    private void textBox_ThemeName_Enter(object sender, EventArgs e)
    {
      if (textBox_ThemeName.Text == TextBoxDefault)
      {
        textBox_ThemeName.Text = "";
        textBox_ThemeName.ForeColor = Color.Black;
      }
    }

    private void textBox_ThemeName_Leave(object sender, EventArgs e)
    {
      if (textBox_ThemeName.Text == "")
      {
        textBox_ThemeName.Text = TextBoxDefault;
        textBox_ThemeName.ForeColor = Color.Gray;
      }
    }
  }

  internal class ColourBundle
  {
    public ColourBundle(Color colour, string jsonKey, Bitmap mask)
    {
      Colour = colour;
      JsonKey = jsonKey;
      Mask = mask;
    }

    public Color Colour;
    public string JsonKey;
    public Bitmap Mask;
  }
}