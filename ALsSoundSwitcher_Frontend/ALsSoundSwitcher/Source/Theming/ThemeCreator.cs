using System;
using System.Drawing;
using System.Windows.Forms;

namespace ALsSoundSwitcher
{
  public partial class ThemeCreator : Form
  {
    private static ColourBundle ActiveSelectionColor = new ColourBundle(Color.DarkSlateGray, Color.Blue, "ActiveSelectionColor");
    private static ColourBundle ColorMenuItemSelected = new ColourBundle(Color.FromArgb(76, 76, 77), Color.Aqua, "ColorMenuItemSelected");
    private static ColourBundle ColorBackground = new ColourBundle(Color.FromArgb(43, 43, 43), Color.FromArgb(0, 255, 0), "ColorBackground");
    private static ColourBundle ColorMenuItemText = new ColourBundle(Color.FromArgb(237, 237, 237), Color.Black, "ColorMenuItemText");
    private static ColourBundle ColorSeparator = new ColourBundle(Color.FromArgb(61, 61, 67), Color.White, "ColorSeparator");
    private static ColourBundle ColorMenuArrow = new ColourBundle(Color.FromArgb(237, 237, 237), Color.Yellow, "ColorMenuArrow");
    private static ColourBundle ColorMenuBorder = new ColourBundle(Color.FromArgb(61, 61, 67), Color.Red, "ColorMenuBorder");

    private static ColourBundle[] allColourBundles =
    {
      ActiveSelectionColor,
      ColorMenuItemSelected,
      ColorBackground,
      ColorMenuItemText,
      ColorSeparator,
      ColorMenuArrow,
      ColorMenuBorder
    }; 

    public ThemeCreator()
    {
      InitializeComponent();

      btn_ActiveSelectionColor.BackColor = ActiveSelectionColor.Colour;
      btn_ColorMenuItemSelected.BackColor = ColorMenuItemSelected.Colour;
      btn_ColorBackground.BackColor = ColorBackground.Colour;
      btn_ColorMenuItemText.BackColor = ColorMenuItemText.Colour;
      btn_ColorSeparator.BackColor = ColorSeparator.Colour;
      btn_ColorMenuArrow.BackColor = ColorMenuArrow.Colour;
      btn_ColorMenuBorder.BackColor = ColorMenuBorder.Colour;

      label_updating.Visible = false;

      UpdatePreview();
    }

    public void UpdatePreview()
    {
      label_updating.Visible = true;
      
      var bmp = (Bitmap)pictureBox1.Image;

      foreach (var colourBundle in allColourBundles)
      {
        for (var x = 0; x < bmp.Width; x++)
        {
          for (var y = 0; y < bmp.Height; y++)
          {
            var pixelColor = bmp.GetPixel(x, y);
            if (ColourIsWithinTolerance(pixelColor, colourBundle.PreviewMaskColour, 0.1f))
            {
              bmp.SetPixel(x, y, colourBundle.Colour);
            }
          }
        }
      }

      pictureBox1.Image = bmp;

      label_updating.Visible = false;
    }

    private bool ColourIsWithinTolerance(Color color1, Color color2, float tolerance)
    {
      var maxDiff = 255 * tolerance;

      return Math.Abs(color1.R - color2.R) <= maxDiff &&
             Math.Abs(color1.G - color2.G) <= maxDiff &&
             Math.Abs(color1.B - color2.B) <= maxDiff;
    }
  }

  class ColourBundle
  {
    public ColourBundle(Color colour, Color previewMaskColour, string jsonKey)
    {
      Colour = colour;
      PreviewMaskColour = previewMaskColour;
      JsonKey = jsonKey;
    }
    public Color Colour;
    public Color PreviewMaskColour;
    public string JsonKey;
  }
}
