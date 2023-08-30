namespace ALsSoundSwitcher
{
  public struct SemanticVersion
  {
    public int Major;
    public int Minor;
    public int Patch;

    public SemanticVersion(string versionString)
    {
      var delimiters = new[] { '.', '-' };
      var parts = versionString.Split(delimiters);

      Major = ParsePart(parts, 0);
      Minor = ParsePart(parts, 1);
      Patch = ParsePart(parts, 2);
    }

    private static int ParsePart(string[] parts, int index)
    {
      if (index < parts.Length && int.TryParse(parts[index], out var value))
      {
        return value;
      }
      return 0;
    }

    public override string ToString() => Major + "." + Minor + "." + Patch;
  }
}