//AL.
/*
 * 15/4/2023
 * There's room for a lot more refinement, but I think this is perfectly usable as of commit d26f92 
 * Shipping as a beta feature in v2.0 
 */
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using ALsSoundSwitcher.Properties;

namespace ALsSoundSwitcher
{
  public partial class ThemeCreator : Form
  {
    private const string TextBoxDefault = "Enter a name for your theme...";

    private static ColourBundle[] _allColourBundles;

    public ThemeCreator()
    {
      InitializeComponent();

      SetupButtons();

      textBox_ThemeName.Text = TextBoxDefault;

      btn_save.Enabled = false;

      GenerateFullPreview();
    }

    private void SetupButtons()
    {
      var colours = Globals.Theme.GetPertinentColours();
      _allColourBundles = new[]
      {
        new ColourBundle(colours["ColorBackground"], "ColorBackground", Resources.mask_background),
        new ColourBundle(colours["ColorMenuBorder"], "ColorMenuBorder", Resources.mask_border),
        new ColourBundle(colours["ActiveSelectionColor"], "ActiveSelectionColor", Resources.mask_active),
        new ColourBundle(colours["ColorMenuItemSelected"], "ColorMenuItemSelected", Resources.mask_selected),
        new ColourBundle(colours["ColorSeparator"], "ColorSeparator", Resources.mask_separator),
        new ColourBundle(colours["ColorMenuArrow"], "ColorMenuArrow", Resources.mask_arrow),
        new ColourBundle(colours["ColorMenuItemText"], "ColorMenuItemText", Resources.mask_text)
      };

      foreach (var bundle in _allColourBundles)
      {
        var button = Controls.Find($"btn_{bundle.JsonKey}", true)[0];
        button.BackColor = bundle.Colour;
      }
    }

    public void GenerateFullPreview()
    {
      foreach (var bundle in _allColourBundles)
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
      var bundle = _allColourBundles.First(x => button.Name.EndsWith(x.JsonKey));

      var colorDialog = new ColorDialog();
      colorDialog.Color = button.BackColor;
      if (colorDialog.ShowDialog() == DialogResult.OK)
      {
        button.BackColor = colorDialog.Color;
        bundle.Colour = colorDialog.Color;
        UpdatePreview(bundle.Mask, bundle.Colour);

        var topLayer = _allColourBundles[_allColourBundles.Length - 1];
        if (bundle.Mask != topLayer.Mask)
        {
          UpdatePreview(topLayer.Mask, topLayer.Colour);
        }
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

    private void textBox_ThemeName_TextChanged(object sender, EventArgs e)
    {
      var input = textBox_ThemeName.Text.Trim();
      var filename = Globals.ThemeFilenamePattern.Replace("*", input);

      if (input != TextBoxDefault && input != string.Empty && !File.Exists(filename))
      {
        btn_save.Enabled = true;
      }
      else
      {
        btn_save.Enabled = false;
      }
    }

    private void btn_save_Click(object sender, EventArgs e)
    {
      PerformSaveOperations();
    }
    
    private void textBox_ThemeName_KeyUp(object sender, KeyEventArgs e)
    {
      if (e.KeyCode == Keys.Enter)
      {
        PerformSaveOperations();
      }
    }

    private void PerformSaveOperations()
    {
      var input = textBox_ThemeName.Text.Trim();
      var filename = Globals.ThemeFilenamePattern.Replace("*", input);
      WriteThemeToFile(_allColourBundles.ToList(), filename);

      Settings.Current.Theme = input;
      Config.Save();

      var args = Globals.LastBaseMenuInvokedPosition.X + " " + Globals.LastBaseMenuInvokedPosition.Y;
      ProcessUtils.RunExe(Application.ExecutablePath, args);
      Application.Exit();
    }

    private static void WriteThemeToFile(List<ColourBundle> bundles, string filePath)
    {
      using (var file = new StreamWriter(filePath))
      {
        file.WriteLine("{");
        foreach (var bundle in bundles)
        {
          var key = $"\"{bundle.JsonKey}\"";
          var colourString = $"\"{bundle.Colour.R}, {bundle.Colour.G}, {bundle.Colour.B}\"";
          var item = "  " + key + " : " + colourString + ",";
          file.WriteLine(item);
        }
        file.WriteLine("}");
      }
    }
  }
}