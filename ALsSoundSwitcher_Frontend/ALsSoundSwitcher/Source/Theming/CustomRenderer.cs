using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace ALsSoundSwitcher
{
  public class CustomRenderer : ToolStripProfessionalRenderer
  {
    private new static readonly MenuStripColorTable ColorTable = new MenuStripColorTable();

    private Color _activeSelectionColor = Color.DarkSlateGray;
    private Color _colorMenuArrow = Color.FromArgb(237, 237, 237);
    private Color _colorCheckSquare = Color.FromArgb(0, 122, 204);
    private Color _colorCheckMark = Color.FromArgb(237, 237, 237);
    private Color _colorMenuItemText = Color.FromArgb(237, 237, 237);

    public CustomRenderer() : base(new MenuStripColorTable())
    {
    }

    public CustomRenderer(ColourPack colourPack) : base(ColorTable)
    {
      SetColours(colourPack);
    }
    
    public Color GetActiveSelectionColour()
    {
      return _activeSelectionColor;
    }

    private void SetColours(ColourPack colourPack)
    {
      _activeSelectionColor = colourPack.ActiveSelectionColor;
      _colorMenuArrow = colourPack.ColorMenuArrow;
      _colorCheckSquare = colourPack.ColorCheckSquare;
      _colorCheckMark = colourPack.ColorCheckMark;
      _colorMenuItemText = colourPack.ColorMenuItemText;

      ColorTable.SetColours(colourPack);
    }

    protected override void OnRenderArrow(ToolStripArrowRenderEventArgs e)
    {
      e.ArrowColor = _colorMenuArrow;
      base.OnRenderArrow(e);
    }

    protected override void OnRenderItemCheck(ToolStripItemImageRenderEventArgs e)
    {
      var g = e.Graphics;
      g.SmoothingMode = SmoothingMode.AntiAlias;

      var rectImage = new Rectangle(e.ImageRectangle.Location, e.ImageRectangle.Size);
      rectImage.Inflate(-1, -1);

      using (var p = new Pen(_colorCheckSquare, 1))
      {
        g.DrawRectangle(p, rectImage);
      }

      var rectCheck = rectImage;
      rectCheck.Width = rectImage.Width - 6;
      rectCheck.Height = rectImage.Height - 8;
      rectCheck.X += 3;
      rectCheck.Y += 4;

      using (var p = new Pen(_colorCheckMark, 2))
      {
        g.DrawLines(p, new[] {
            new Point(rectCheck.Left, rectCheck.Bottom - rectCheck.Height / 2), 
            new Point(rectCheck.Left + rectCheck.Width / 3, rectCheck.Bottom), 
            new Point(rectCheck.Right, rectCheck.Top)
          });
      }
    }

    protected override void OnRenderItemText(ToolStripItemTextRenderEventArgs e)
    {
      var textRect = e.TextRectangle;
      textRect.Height = e.Item.Height - 4; //4 is the default difference between the item height and the text rectangle height

      e.TextRectangle = textRect;
      e.TextFormat = TextFormatFlags.VerticalCenter;
      e.TextColor = _colorMenuItemText;
      base.OnRenderItemText(e);
    }

    protected override void OnRenderMenuItemBackground(ToolStripItemRenderEventArgs e)
    {
      if (e.Item.Enabled == false)
      {
        return;
      }
        
      base.OnRenderMenuItemBackground(e);
    }
  }
}