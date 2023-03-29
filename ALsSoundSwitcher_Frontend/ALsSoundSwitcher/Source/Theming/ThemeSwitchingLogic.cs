using static ALsSoundSwitcher.Globals;

namespace ALsSoundSwitcher
{
  public partial class Form1
  {
    private static void SetTheme()
    {
      Theme = Settings.Current.DarkMode == 1 ? (CustomRenderer) new DarkRenderer() : new LightRenderer();
      Globals.ContextMenu.Renderer = Theme;

      SetActiveMenuItemMarker(LastIndex);
    }
  }
}