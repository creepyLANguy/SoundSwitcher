﻿using System;
using System.Drawing;
using System.Windows.Forms;
using ALsSoundSwitcher.Properties;

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
      Resources.mask_background,
      Resources.mask_border,
      Resources.mask_active,
      Resources.mask_selected,
      Resources.mask_separator,
      Resources.mask_arrow,
      Resources.mask_text
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

      textBox_ThemeName.Text = TextBoxDefault;

      GeneratePreview();
    }

    public void GeneratePreview()
    {
      var buffer = new Bitmap(Resources.mask_border.Width, Resources.mask_border.Height);

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
      UpdatePreview(Resources.mask_active, colour);
      UpdatePreview(Resources.mask_text, ColorMenuItemText.Colour);
    }

    private void btn_ColorMenuItemSelected_Click(object sender, EventArgs e)
    {
      var button = ((Button)sender);
      var colour = GetColour(button.BackColor);
      button.BackColor = colour;
      ColorMenuItemSelected.Colour = colour;
      UpdatePreview(Resources.mask_selected, colour);
      UpdatePreview(Resources.mask_text, ColorMenuItemText.Colour);
    }

    private void btn_ColorBackground_Click(object sender, EventArgs e)
    {
      var button = ((Button)sender);
      var colour = GetColour(button.BackColor);
      button.BackColor = colour;
      ColorBackground.Colour = colour;
      UpdatePreview(Resources.mask_background, colour);
      UpdatePreview(Resources.mask_text, ColorMenuItemText.Colour);
    }

    private void btn_ColorMenuItemText_Click(object sender, EventArgs e)
    {
      var button = ((Button)sender);
      var colour = GetColour(button.BackColor);
      button.BackColor = colour;
      ColorMenuItemText.Colour = colour;
      UpdatePreview(Resources.mask_text, colour);
    }

    private void btn_ColorSeparator_Click(object sender, EventArgs e)
    {
      var button = ((Button)sender);
      var colour = GetColour(button.BackColor);
      button.BackColor = colour;
      ColorSeparator.Colour = colour;
      UpdatePreview(Resources.mask_separator, colour);
    }

    private void btn_ColorMenuArrow_Click(object sender, EventArgs e)
    {
      var button = ((Button)sender);
      var colour = GetColour(button.BackColor);
      button.BackColor = colour;
      ColorMenuArrow.Colour = colour;
      UpdatePreview(Resources.mask_arrow, colour);
    }

    private void btn_ColorMenuBorder_Click(object sender, EventArgs e)
    {
      var button = ((Button)sender);
      var colour = GetColour(button.BackColor);
      button.BackColor = colour;
      ColorMenuBorder.Colour = colour;
      UpdatePreview(Resources.mask_border, colour);
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
        textBox_ThemeName.ForeColor = Color.Black;
        textBox_ThemeName.Text = string.Empty;
      }
    }

    private void textBox_ThemeName_Leave(object sender, EventArgs e)
    {
      if (textBox_ThemeName.Text == string.Empty)
      {
        textBox_ThemeName.ForeColor = Color.Gray;
        textBox_ThemeName.Text = TextBoxDefault;
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