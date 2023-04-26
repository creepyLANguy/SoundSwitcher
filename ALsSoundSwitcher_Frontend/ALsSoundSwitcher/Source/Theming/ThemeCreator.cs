using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using ALsSoundSwitcher.Properties;

namespace ALsSoundSwitcher
{
  public partial class ThemeCreator : Form
  {
    private string TextBoxDefault = Resources.ThemeCreator_TextBoxDefault;

    private static ColourBundle[] _allColourBundles;

    private static ColorDialog ColourPicker = new ColorDialog();

    public ThemeCreator()
    {
      InitializeComponent();

      DoubleBuffered = true;

      SetupButtons();

      textBox_ThemeName.Text = TextBoxDefault;

      btn_save.Enabled = false;
      label_errorFileName.Visible = false;

      GenerateFullPreview();
    }

    private void SetupButtons()
    {
      var colours = Globals.Theme.GetPertinentColours();

      _allColourBundles = new[]
      {               
        new ColourBundle(colours["ColorBackground"],       "ColorBackground",       Resources.mask_background, LayerType.BACKGROUND),
        new ColourBundle(colours["ColorMenuBorder"],       "ColorMenuBorder",       Resources.mask_border,     LayerType.NORMAL),
        new ColourBundle(colours["ActiveSelectionColor"],  "ActiveSelectionColor",  Resources.mask_active,     LayerType.NORMAL),
        new ColourBundle(colours["ColorSeparator"],        "ColorSeparator",        Resources.mask_separator,  LayerType.NORMAL),
        new ColourBundle(colours["ColorMenuItemSelected"], "ColorMenuItemSelected", Resources.mask_selected,   LayerType.NORMAL),
        new ColourBundle(colours["ColorMenuArrow"],        "ColorMenuArrow",        Resources.mask_arrow,      LayerType.NORMAL),
        new ColourBundle(colours["ColorMenuItemText"],     "ColorMenuItemText",     Resources.mask_text,       LayerType.TOPMOST)
      }.OrderBy(cb => cb.Layer).ToArray();

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
          var sum = pixelColor.R + pixelColor.G + pixelColor.B;
          if (sum == 0)
          {
            buffer.SetPixel(x, y, colour);
          }
        }
      }

      pictureBox1.Image = buffer;
    }

    private void HandleButtonClick(object sender, EventArgs e)
    {
      var button = (Button)sender;

      ColourPicker.Color = button.BackColor;
      
      if (ColourPicker.ShowDialog() != DialogResult.OK)
      {
        return;
      }

      var bundle = _allColourBundles.First(x => button.Name.EndsWith(x.JsonKey));
      button.BackColor = ColourPicker.Color;
      bundle.Colour = ColourPicker.Color;

      Cursor.Current = Cursors.WaitCursor;

      UpdatePreview(bundle.Mask, bundle.Colour);

      if (bundle.Layer != LayerType.TOPMOST)
      {
        var topMostBundle = _allColourBundles.First(cb => cb.Layer == LayerType.TOPMOST);
        UpdatePreview(topMostBundle.Mask, topMostBundle.Colour);
      }

      Cursor.Current = Cursors.Default;
    }

    private void textBox_ThemeName_Enter(object sender, EventArgs e)
    {
      if (textBox_ThemeName.Text == TextBoxDefault)
      {
        label_errorFileName.Visible = false;
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
        label_errorFileName.Visible = false;
      }
    }

    private void textBox_ThemeName_TextChanged(object sender, EventArgs e)
    {
      var input = textBox_ThemeName.Text.Trim();
      var filename = input + Globals.ThemeFileExtension;

      if (input != TextBoxDefault && input != string.Empty && !File.Exists(filename))
      {
        label_errorFileName.Visible = false;
        btn_save.Enabled = true;
      }
      else
      {
        if (File.Exists(filename))
        {
          label_errorFileName.Visible = true;
        }
        
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
      var filename = input + Globals.ThemeFileExtension;
      WriteThemeToFile(_allColourBundles.ToList(), filename);

      Settings.Current.Theme = input;
      Config.Save();

      var argString = Globals.ShowMenusPostThemeRestartFlag;
      var x = Globals.LastBaseMenuInvokedPosition.X;
      var y = Globals.LastBaseMenuInvokedPosition.Y;
      var args = $"\"{argString}\" {x} {y}";
      ProcessUtils.RunExe(Application.ExecutablePath, args, true);
      
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

    private void pictureBox1_Paint(object sender, PaintEventArgs e)
    {
      return;

      if (pictureBox1.Image == null)
      {
        return;
      }

      var imageRectangle = pictureBox1.ClientRectangle;
      imageRectangle = GetAdjustedRect(pictureBox1.Image.Size, imageRectangle);

      e.Graphics.FillRectangle(new SolidBrush(BackColor), imageRectangle);      
      //e.Graphics.FillRectangle(Brushes.Red, imageRectangle); //debugging

      e.Graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
      //e.Graphics.InterpolationMode = InterpolationMode.NearestNeighbor; //no smoothing at all

      e.Graphics.DrawImage(pictureBox1.Image, imageRectangle);

      Rectangle GetAdjustedRect(Size size, Rectangle targetArea)
      {
        float widthRatio = (float)targetArea.Width / size.Width;
        float heightRatio = (float)targetArea.Height / size.Height;
        float ratio = Math.Min(widthRatio, heightRatio);
        float newWidth = size.Width * ratio;
        float newHeight = size.Height * ratio;
        float x = (targetArea.Width - newWidth) / 2;
        float y = (targetArea.Height - newHeight) / 2;
        return new Rectangle((int)Math.Round(x), (int)Math.Round(y), (int)Math.Round(newWidth), (int)Math.Round(newHeight));
      }
    }
  }

  internal class ColourBundle
  {
    public ColourBundle(Color colour, string jsonKey, Bitmap mask, LayerType layer)
    {
      Colour = colour;
      JsonKey = jsonKey;
      Mask = mask;
      Layer = layer;

    }

    public Color Colour;
    public string JsonKey;
    public Bitmap Mask;
    public LayerType Layer;
  }

  public enum LayerType
  {
    BACKGROUND,
    NORMAL,
    TOPMOST
  }
}