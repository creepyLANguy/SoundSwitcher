using System.Drawing;

namespace ALsSoundSwitcher
{
  public class ColourBundle
  {
    public ColourBundle(Color colour, string jsonKey, Bitmap mask)
    {
      Colour = colour;
      JsonKey = jsonKey;
      Mask = mask;
    }

    public Color Colour;
    public string JsonKey;
    public Bitmap Mask;
  }
}