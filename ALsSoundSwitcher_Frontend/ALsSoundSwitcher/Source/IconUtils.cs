using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using ALsSoundSwitcher.Properties;

namespace ALsSoundSwitcher
{
  public class IconUtils
  {
    public static bool SetIcon(string iconName, NotifyIcon notifyIcon)
    {
      notifyIcon.Icon = Resources.Icon;

      var bestMatch = GetBestMatchIcon(iconName);

      var icon = GetIconByRawName(bestMatch);
      if (icon == null)
      {
        return false;
      }

      notifyIcon.Icon = icon;
      return true;
    }

    public static string GetBestMatchIcon(string iconName)
    {

      var allIcons = GetAllIconsInFolder();
      if (allIcons.Count == 0)
      {
        return string.Empty;
      }

      var matches = GetMatchPercentages(iconName.Trim(), allIcons);
      var bestMatch = matches.OrderByDescending(it => it.Item2).First();
      if (bestMatch.Item2 < Settings.Current.BestNameMatchPercentageMinimum)
      {
        return string.Empty;
      }

      return bestMatch.Item1;
    }

    private static List<string> GetAllIconsInFolder()
    {
      var allIconFilePaths =
        Directory.GetFiles(Directory.GetCurrentDirectory(), "*", SearchOption.AllDirectories).ToList();
          //Where(it => it.Contains(supportedIconFiletypes)).ToList();

      var allIcons = new List<string>(allIconFilePaths.Count);
      allIconFilePaths.ForEach(it => allIcons.Add(Path.GetFileName(it)));
      return allIcons;
    }

    private static List<Tuple<string, double>> GetMatchPercentages(string reference, List<string> candidates)
    {
      var list = new List<Tuple<string, double>>();

      foreach (var candidate in candidates)
      {
        var matchPercentage = GetMatchPercentage(reference, candidate);
        list.Add(new Tuple<string, double>(candidate, matchPercentage));
      }

      return list;
    }

    public static double GetMatchPercentage(string reference, string candidate)
    {
      var largerStringLength = Math.Max(reference.Length, candidate.Length);

      var editDistance = LevenshteinDistance.Calculate(reference, candidate);

      return (double)(largerStringLength - editDistance) / largerStringLength * 100;
    }

    private static Icon GetIconByRawName(string iconName)
    {
      try
      {
        using (var stream = new MemoryStream(File.ReadAllBytes(iconName)))
        {
          using (var bitmap = new Bitmap(stream))
          {
            return Icon.FromHandle(bitmap.GetHicon());
          }
        }
      }
      catch (Exception)
      {
        return null;
      }
    }
  }
}
