using System.Windows.Forms;
using System.Windows.Input;
using GlobalHotKey;

namespace ALsSoundSwitcher
{
  public static class KeyWatcher
  {
    //AL.
    //TODO - make configurable etc
    private static ModifierKeys hkModifier = ModifierKeys.Alt;
    private static Key hkToggle= Key.OemPeriod;
    //

    public static void Run()
    {
      Globals.GlobalHotKeyManager = new HotKeyManager();

      Globals.GlobalHotKeyManager.Register(hkToggle, hkModifier);

      Globals.GlobalHotKeyManager.KeyPressed += HotKeyPressed;
    }

    private static void HotKeyPressed(object sender, KeyPressedEventArgs e)
    {
      var key = e.HotKey.Key;

      if (key == hkToggle)
      {
        //AL.
        //TODO - implement clean call to Toggle()
        MessageBox.Show(@"Pressed");
      }
    }
  }
}