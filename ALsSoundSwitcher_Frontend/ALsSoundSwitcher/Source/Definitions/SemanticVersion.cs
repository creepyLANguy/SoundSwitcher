using System;

namespace ALsSoundSwitcher
{
  public struct SemanticVersion
  {
    public int Major;
    public int Minor;
    public int Patch;
    public bool IsValid;

    public SemanticVersion(string versionString)
    {
      var delimiters = new[] { '.', '-' };
      var parts = versionString.Split(delimiters);

      try
      {
        Major = ParsePart(parts, 0);
        Minor = ParsePart(parts, 1);
        Patch = ParsePart(parts, 2);

        IsValid = true;
      }
      catch (Exception ex)
      {
        Console.WriteLine(ex);

        Major = Minor = Patch = 0;

        IsValid = false;
      }
    }

    public SemanticVersion(bool isValid = false)
    {
      Major = Minor = Patch = 0;
      IsValid = isValid;
    }

    private static int ParsePart(string[] parts, int index)
    {
      if (index < parts.Length && int.TryParse(parts[index], out var value))
      {
        return value;
      }
      return 0;
    }

    public override int GetHashCode()
    {
      unchecked
      {
        var hashCode = Major;
        hashCode = (hashCode * 397) ^ Minor;
        hashCode = (hashCode * 397) ^ Patch;
        hashCode = (hashCode * 397) ^ IsValid.GetHashCode();
        return hashCode;
      }
    }

    public bool Equals(SemanticVersion other)
      => GetHashCode() == other.GetHashCode();

    public override bool Equals(object obj)
      => obj is SemanticVersion other && Equals(other);

    public static bool operator == (SemanticVersion a, SemanticVersion b)
      => a.Equals(b);

    public static bool operator != (SemanticVersion a, SemanticVersion b)
      => !(a == b);

    public static bool operator < (SemanticVersion a, SemanticVersion b)
    {
      if (a == b)
      {
        return false;
      }

      if (a.Major < b.Major)
      {
        return true;
      }
      if (a.Minor < b.Minor)
      {
        return true;
      }
      if (a.Patch < b.Patch)
      {
        return true;
      }

      return false;
    }

    public static bool operator > (SemanticVersion a, SemanticVersion b) 
      => b < a;
    
    public static bool operator <= (SemanticVersion a, SemanticVersion b) 
      => a < b || a == b;

    public static bool operator >= (SemanticVersion a, SemanticVersion b) 
      => a > b || a == b;

    public override string ToString() => Major + "." + Minor + "." + Patch;
  }
}