using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace ALsSoundSwitcher
{
  public class CustomRenderer : ToolStripProfessionalRenderer
  {
    private new static readonly MenuStripColorTable ColorTable = new MenuStripColorTable();

    public Color ActiveSelectionColor = Color.DarkSlateGray;
    public Color ColorMenuArrow = Color.FromArgb(237, 237, 237);
    public Color ColorCheckSquare = Color.FromArgb(0, 122, 204);
    public Color ColorCheckMark = Color.FromArgb(237, 237, 237);
    public Color ColorMenuItemText = Color.FromArgb(237, 237, 237);
    public Color ColorBackground = Color.FromArgb(43, 43, 43);

    public CustomRenderer() : base(new MenuStripColorTable())
    {
    }

    public CustomRenderer(ColourPack colourPack) : base(ColorTable)
    {
      SetColours(colourPack);
    }

    public Dictionary<string, Color> GetPertinentColours()
    {
      return new Dictionary<string, Color>
      {
        {nameof(ColorBackground),ColorBackground},
        {nameof(ColorTable.ColorMenuBorder),ColorTable.ColorMenuBorder},
        {nameof(ActiveSelectionColor),ActiveSelectionColor},
        {nameof(ColorTable.ColorMenuItemSelected),ColorTable.ColorMenuItemSelected},
        {nameof(ColorTable.ColorSeparator),ColorTable.ColorSeparator},
        {nameof(ColorMenuArrow),ColorMenuArrow},
        {nameof(ColorMenuItemText),ColorMenuItemText}
      };
    }

    

    private void SetColours(ColourPack colourPack)
    {
      ActiveSelectionColor = colourPack.ActiveSelectionColor;
      ColorMenuArrow = colourPack.ColorMenuArrow;
      ColorCheckSquare = colourPack.ColorCheckSquare;
      ColorCheckMark = colourPack.ColorCheckMark;
      ColorMenuItemText = colourPack.ColorMenuItemText;
      ColorBackground = colourPack.ColorBackground;

      ColorTable.SetColours(colourPack);
    }

    protected override void OnRenderArrow(ToolStripArrowRenderEventArgs e)
    {
      e.ArrowColor = ColorMenuArrow;
      base.OnRenderArrow(e);
    }

    protected override void OnRenderItemCheck(ToolStripItemImageRenderEventArgs e)
    {
      var g = e.Graphics;
      g.SmoothingMode = SmoothingMode.AntiAlias;

      var rectImage = new Rectangle(e.ImageRectangle.Location, e.ImageRectangle.Size);
      rectImage.Inflate(-1, -1);

      using (var p = new Pen(ColorCheckSquare, 1))
      {
        g.DrawRectangle(p, rectImage);
      }

      var rectCheck = rectImage;
      rectCheck.Width = rectImage.Width - 6;
      rectCheck.Height = rectImage.Height - 8;
      rectCheck.X += 3;
      rectCheck.Y += 4;

      using (var p = new Pen(ColorCheckMark, 2))
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
      e.TextColor = ColorMenuItemText;
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