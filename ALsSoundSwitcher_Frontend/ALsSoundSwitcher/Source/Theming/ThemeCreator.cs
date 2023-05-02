using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
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

      checkBox_smoothing.Checked = true;

      ColourPicker.FullOpen = true;

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

      var tasks = new List<Task>();
      foreach (var bundle in _allColourBundles)
      {
        var task = Task.Run(() => Controls.Find($"btn_{bundle.JsonKey}", true)[0].BackColor = bundle.Colour);
        tasks.Add(task);
      }
      Task.WaitAll(tasks.ToArray());      
    }

    private void GenerateFullPreview()
    {
      var tasks = new List<Task>();
      foreach (var bundle in _allColourBundles.Where(cb => cb.Layer != LayerType.TOPMOST))
      {
        var task = Task.Run(() => ProcessMask(bundle.Mask, bundle.Colour));
        tasks.Add(task);
      }
      Task.WaitAll(tasks.ToArray());

      PaintTopMostBundle();

      pictureBox1.Refresh();
    }

    private void PaintTopMostBundle()
    {
      var topMostBundle = _allColourBundles.First(cb => cb.Layer == LayerType.TOPMOST);
      ProcessMask(topMostBundle.Mask, topMostBundle.Colour);
    }

    private void ProcessMask(Bitmap mask, Color colour, bool sharpen = false)
    {
      Bitmap buffer = new Bitmap(mask.Width, mask.Height);

      for (var x = 0; x < mask.Width; x++)
      {
        for (var y = 0; y < mask.Height; y++)
        {
          if (mask.GetPixel(x, y).R == 0) //Assumes masks are always 2-bit. ie, if not white, then draw.  
          {
            buffer.SetPixel(x, y, colour);
          }
        }
      }

      if (sharpen)
      {
        //AL.
        //TODO - do the thing. 
      }

      lock (pictureBox1)
      {
        using (var g = Graphics.FromImage(pictureBox1.Image))
        {
          g.DrawImage(buffer, 0, 0);
        }                
      }
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

      ProcessMask(bundle.Mask, bundle.Colour);

      if (bundle.Layer != LayerType.TOPMOST)
      {
        PaintTopMostBundle();
      }

      pictureBox1.Refresh();

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
      if (pictureBox1.Image == null)
      {
        return;
      }

      var imageRectangle = pictureBox1.ClientRectangle;
      imageRectangle = GetAdjustedRect(pictureBox1.Image.Size, imageRectangle);

      e.Graphics.FillRectangle(new SolidBrush(BackColor), imageRectangle);

      e.Graphics.InterpolationMode = checkBox_smoothing.Checked ? InterpolationMode.HighQualityBicubic : InterpolationMode.NearestNeighbor;

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

    private void checkBox_smoothing_CheckedChanged(object sender, EventArgs e)
    {
      pictureBox1.Refresh();
    }

    private void btn_reset_Click(object sender, EventArgs e)
    {
      Cursor.Current = Cursors.WaitCursor;

      SetupButtons();

      if (Globals.Instance.InvokeRequired)
      {
        Globals.Instance.Invoke(new MethodInvoker(GenerateFullPreview));
      }
      else
      {
        GenerateFullPreview();
      }

      Cursor.Current = Cursors.Default;
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