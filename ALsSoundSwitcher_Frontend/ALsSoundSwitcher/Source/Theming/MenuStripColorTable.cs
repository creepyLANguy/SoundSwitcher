using System.Drawing;
using System.Windows.Forms;

namespace ALsSoundSwitcher
{
  internal class MenuStripColorTable : ProfessionalColorTable
  {
    private Color _colorMenuBorder = Color.FromArgb(61, 61, 67);
    private Color _colorMenuItemSelected = Color.FromArgb(76, 76, 77);
    private Color _colorBackground = Color.FromArgb(43, 43, 43);
    private Color _colorSeparator = Color.FromArgb(61, 61, 67);
    private Color _colorStatusStripGradient = Color.FromArgb(234, 237, 241);
    private Color _colorButtonSelected = Color.FromArgb(88, 146, 226);
    private Color _colorButtonPressed = Color.FromArgb(110, 160, 230);

    public void SetColours(ColourPack colourPack)
    {
      _colorMenuBorder = colourPack.ColorMenuBorder;
      _colorMenuItemSelected = colourPack.ColorMenuItemSelected;
      _colorBackground = colourPack.ColorBackground;
      _colorSeparator = colourPack.ColorSeparator;
      _colorStatusStripGradient = colourPack.ColorStatusStripGradient;
      _colorButtonSelected = colourPack.ColorButtonSelected;
      _colorButtonPressed = colourPack.ColorButtonPressed;
    }

    public override Color ToolStripDropDownBackground => _colorBackground;
    public override Color MenuStripGradientBegin => _colorBackground;
    public override Color MenuStripGradientEnd => _colorBackground;
    public override Color CheckBackground => _colorBackground;
    public override Color CheckPressedBackground => _colorBackground;
    public override Color CheckSelectedBackground => _colorBackground;
    public override Color MenuItemSelected => _colorMenuItemSelected;
    public override Color ImageMarginGradientBegin => _colorBackground;
    public override Color ImageMarginGradientMiddle => _colorBackground;
    public override Color ImageMarginGradientEnd => _colorBackground;
    public override Color MenuItemBorder => _colorMenuItemSelected;
    public override Color MenuBorder => _colorMenuBorder;
    public override Color SeparatorDark => _colorSeparator;
    public override Color SeparatorLight => _colorSeparator;
    public override Color StatusStripGradientBegin => _colorStatusStripGradient;
    public override Color StatusStripGradientEnd => _colorStatusStripGradient;
    public override Color ButtonSelectedGradientBegin => _colorButtonSelected;
    public override Color ButtonSelectedGradientMiddle => _colorButtonSelected;
    public override Color ButtonSelectedGradientEnd => _colorButtonSelected;
    public override Color ButtonSelectedBorder => _colorButtonSelected;
    public override Color ButtonPressedGradientBegin => _colorButtonPressed;
    public override Color ButtonPressedGradientMiddle => _colorButtonPressed;
    public override Color ButtonPressedGradientEnd => _colorButtonPressed;
    public override Color ButtonPressedBorder => _colorButtonPressed;
  }
}
