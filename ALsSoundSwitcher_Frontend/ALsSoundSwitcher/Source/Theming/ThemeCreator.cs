using System;
using System.Drawing;
using System.Windows.Forms;

namespace ALsSoundSwitcher
{
  public partial class ThemeCreator : Form
  {
    private const string TextBoxDefault = "Enter a name for your theme...";

    private static readonly ColourBundle ActiveSelectionColor = new ColourBundle(Color.DarkSlateGray, "ActiveSelectionColor");
    private static readonly ColourBundle ColorMenuItemSelected = new ColourBundle(Color.FromArgb(76, 76, 77), "ColorMenuItemSelected");
    private static readonly ColourBundle ColorBackground = new ColourBundle(Color.FromArgb(43, 43, 43), "ColorBackground");
    private static readonly ColourBundle ColorMenuItemText = new ColourBundle(Color.FromArgb(237, 237, 237), "ColorMenuItemText");
    private static readonly ColourBundle ColorSeparator = new ColourBundle(Color.FromArgb(61, 61, 67), "ColorSeparator");
    private static readonly ColourBundle ColorMenuArrow = new ColourBundle(Color.FromArgb(237, 237, 237), "ColorMenuArrow");
    private static readonly ColourBundle ColorMenuBorder = new ColourBundle(Color.FromArgb(61, 61, 67), "ColorMenuBorder");

    private static readonly ColourBundle[] AllColourBundles =
    {
      ColorBackground,
      ColorMenuBorder,
      ActiveSelectionColor,
      ColorMenuItemSelected,
      ColorSeparator,
      ColorMenuArrow,
      ColorMenuItemText
    };

    private static readonly Bitmap[] Masks =
    {
      Properties.Resources.mask_background,
      Properties.Resources.mask_border,
       Properties.Resources.mask_active,
      Properties.Resources.mask_selected,
      Properties.Resources.mask_separator,
      Properties.Resources.mask_arrow,
      Properties.Resources.mask_text
    };

    public ThemeCreator()
    {
      InitializeComponent();

      btn_ActiveSelectionColor.BackColor  = ActiveSelectionColor.Colour;
      btn_ColorMenuItemSelected.BackColor = ColorMenuItemSelected.Colour;
      btn_ColorBackground.BackColor       = ColorBackground.Colour;
      btn_ColorMenuItemText.BackColor     = ColorMenuItemText.Colour;
      btn_ColorSeparator.BackColor        = ColorSeparator.Colour;
      btn_ColorMenuArrow.BackColor        = ColorMenuArrow.Colour;
      btn_ColorMenuBorder.BackColor       = ColorMenuBorder.Colour;

      label_updating.Visible = false;

      textBox_ThemeName.Text = TextBoxDefault;

      GeneratePreview();
    }

    public void GeneratePreview()
    {
      label_updating.Visible = true;

      var buffer = new Bitmap(Properties.Resources.mask_border.Width, Properties.Resources.mask_border.Height);

      for (var index = 0; index < Masks.Length; index++)
      {
        var mask = Masks[index];

        var bundle = AllColourBundles[index];

        for (var x = 0; x < mask.Width; x++)
        {
          for (var y = 0; y < mask.Height; y++)
          {
            var pixelColor = mask.GetPixel(x, y);
            if (ColourIsWithinTolerance(pixelColor, Color.Black, 0.1f))
            {
              buffer.SetPixel(x, y, bundle.Colour);
            }
          }
        }
      }

      pictureBox1.Image = buffer;

      label_updating.Visible = false;
    }

    public void UpdatePreview(Bitmap mask, Color colour)
    {
      label_updating.Visible = true;

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

      label_updating.Visible = false;
    }

    private bool ColourIsWithinTolerance(Color color1, Color color2, float tolerance)
    {
      var maxDiff = 255 * tolerance;

      return Math.Abs(color1.R - color2.R) <= maxDiff &&
             Math.Abs(color1.G - color2.G) <= maxDiff &&
             Math.Abs(color1.B - color2.B) <= maxDiff;
    }

    private void btn_ActiveSelectionColor_Click(object sender, EventArgs e)
    {
      var button = ((Button) sender);
      var colour = GetColour(button.BackColor);
      button.BackColor = colour;
      ActiveSelectionColor.Colour = colour;
      UpdatePreview(Properties.Resources.mask_active, colour);
      UpdatePreview(Properties.Resources.mask_text, ColorMenuItemText.Colour);
    }

    private void btn_ColorMenuItemSelected_Click(object sender, EventArgs e)
    {
      var button = ((Button)sender);
      var colour = GetColour(button.BackColor);
      button.BackColor = colour;
      ColorMenuItemSelected.Colour = colour;
      UpdatePreview(Properties.Resources.mask_selected, colour);
      UpdatePreview(Properties.Resources.mask_text, ColorMenuItemText.Colour);
    }

    private void btn_ColorBackground_Click(object sender, EventArgs e)
    {
      var button = ((Button)sender);
      var colour = GetColour(button.BackColor);
      button.BackColor = colour;
      ColorBackground.Colour = colour;
      UpdatePreview(Properties.Resources.mask_background, colour);
      UpdatePreview(Properties.Resources.mask_text, ColorMenuItemText.Colour);
    }

    private void btn_ColorMenuItemText_Click(object sender, EventArgs e)
    {
      var button = ((Button)sender);
      var colour = GetColour(button.BackColor);
      button.BackColor = colour;
      ColorMenuItemText.Colour = colour;
      UpdatePreview(Properties.Resources.mask_text, colour);
    }

    private void btn_ColorSeparator_Click(object sender, EventArgs e)
    {
      var button = ((Button)sender);
      var colour = GetColour(button.BackColor);
      button.BackColor = colour;
      ColorSeparator.Colour = colour;
      UpdatePreview(Properties.Resources.mask_separator, colour);
    }

    private void btn_ColorMenuArrow_Click(object sender, EventArgs e)
    {
      var button = ((Button)sender);
      var colour = GetColour(button.BackColor);
      button.BackColor = colour;
      ColorMenuArrow.Colour = colour;
      UpdatePreview(Properties.Resources.mask_arrow, colour);
    }

    private void btn_ColorMenuBorder_Click(object sender, EventArgs e)
    {
      var button = ((Button)sender);
      var colour = GetColour(button.BackColor);
      button.BackColor = colour;
      ColorMenuBorder.Colour = colour;
      UpdatePreview(Properties.Resources.mask_border, colour);
    }

    private static Color GetColour(Color defaultColour)
    {
      var colorPicker = new ColorDialog();
      var result = colorPicker.ShowDialog();
      return result == DialogResult.OK ? colorPicker.Color : defaultColour;
    }

    private void textBox_ThemeName_Enter(object sender, EventArgs e)
    {
      if (textBox_ThemeName.Text == TextBoxDefault)
      {
        textBox_ThemeName.Text = string.Empty;
      }
    }
  }

  internal class ColourBundle
  {
    public ColourBundle(Color colour, string jsonKey)
    {
      Colour = colour;
      JsonKey = jsonKey;
    }
    public Color Colour;
    public string JsonKey;
  }
}
