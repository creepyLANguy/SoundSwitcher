using System;
using System.Windows.Input;
using GlobalHotKey;

namespace ALsSoundSwitcher
{
  //AL.
  //TODO - make hotkeys and actions generic and configurable.

  public static class KeyWatcher
  {
    private const ModifierKeys HkModifier = ModifierKeys.Alt;
    private const Key HkToggle= Key.OemPeriod;

    public static void Run(Action toggleAction)
    {
      Globals.GlobalHotKeyManager = new HotKeyManager();

      Globals.GlobalHotKeyManager.Register(HkToggle, HkModifier);

      Globals.GlobalHotKeyManager.KeyPressed += HotKeyPressed;

      void HotKeyPressed(object sender, KeyPressedEventArgs e)
      {
        if (e.HotKey.Key == HkToggle)
        {
          toggleAction.Invoke();
        }
      }
    }
  }
}