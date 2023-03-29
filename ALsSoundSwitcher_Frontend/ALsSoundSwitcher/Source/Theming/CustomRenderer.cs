using System.Drawing;
using System.Windows.Forms;

namespace ALsSoundSwitcher
{
  public abstract class CustomRenderer : ToolStripProfessionalRenderer
  {
    public abstract Color GetActiveSelectionColour();
    protected CustomRenderer(ProfessionalColorTable professionalColorTable) : base(professionalColorTable)
    {
    }
  }
}
