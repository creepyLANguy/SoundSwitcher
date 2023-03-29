namespace ALsSoundSwitcher
{
  public partial class Form1
  {
    private void SetTheme()
    {
      theme = Settings.Current.DarkMode == 1 ? (CustomRenderer) new DarkRenderer() : new LightRenderer();
      contextMenu.Renderer = theme;

      SetActiveMenuItemMarker(lastIndex);
    }
  }
}