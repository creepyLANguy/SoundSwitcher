using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using ALsSoundSwitcher.Properties;

namespace ALsSoundSwitcher
{
  public class IconUtils
  {
    public static bool SetTrayIcon(string iconName)
    {
      var bestMatch = GetBestMatchIconFileName(iconName);

      var icon = GetIconByRawName(bestMatch) ?? GetDefaultIcon();

      if (icon == null)
      {
        return false;
      }

      ((Form1)Globals.Instance).SetTrayIcon(icon);

      return true;
    }

    public static Icon GetDefaultIcon()
    {
      var icon = GetIconByRawName(Globals.UserSettings.DefaultIcon);
      if (icon == null)
      {
        icon = Globals.UserSettings.Mode == DeviceMode.Output ? Resources.Headset : Resources.Mic;
      }
      return icon;
    }

    public static string GetBestMatchIconFileName(string iconName)
    {
      if (iconName.Length == 0)
      {
        return string.Empty;
      }

      var allIcons = GetAllIconsInFolder();
      if (allIcons.Count == 0)
      {
        return string.Empty;
      }

      var matches = GetMatchPercentages(iconName.Trim(), allIcons);
      var bestMatch = matches.OrderByDescending(it => it.Item2).First();
      if (bestMatch.Item2 < Globals.UserSettings.BestNameMatchPercentageMinimum)
      {
        return string.Empty;
      }

      return bestMatch.Item1;
    }

    private static List<string> GetAllIconsInFolder()
    {
      return Directory.GetFiles(Directory.GetCurrentDirectory(), "*")
        .Select(Path.GetFileName)
        .ToList();
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

    private static double GetMatchPercentage(string reference, string candidate)
    {
      var largerStringLength = Math.Max(reference.Length, candidate.Length);

      var editDistance = LevenshteinDistance.Calculate(reference, candidate);

      return (double)(largerStringLength - editDistance) / largerStringLength * 100;
    }

    public static Icon GetIconByRawName(string iconName)
    {
      if (iconName.Length == 0)
      {
        return null;
      }

      try
      {
        //.ico files look much better when using the specific constructor so keep this branching logic.  
        return iconName.EndsWith(".ico") ? LoadIconIntoMemory(iconName) : CreateIconFromImageFile(iconName);
      }
      catch (Exception ex)
      {
        Console.WriteLine(ex.Message);
        return null;
      }
    }

    public static Icon CreateIconFromImageFile(string imageFilename)
    {
      var paddedImage = GetPaddedImage(imageFilename);
      var bitmap = new Bitmap(paddedImage);
      return BitmapToIcon(bitmap);
    }

    public static Image GetPaddedImage(string imageFilename)
    {
      var originalImage = LoadImageIntoMemory(imageFilename);
      if (originalImage.Width == originalImage.Height)
      {
        return originalImage;
      }

      var largestDimension = Math.Max(originalImage.Height, originalImage.Width);
      var squareImage = new Bitmap(largestDimension, largestDimension);

      using var graphics = Graphics.FromImage(squareImage);
      graphics.CompositingQuality = CompositingQuality.HighQuality;
      graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
      graphics.SmoothingMode = SmoothingMode.HighQuality;

      var x = (largestDimension / 2) - (originalImage.Width / 2);
      var y = (largestDimension / 2) - (originalImage.Height / 2);
      graphics.DrawImage(originalImage, x, y, originalImage.Width, originalImage.Height);

      return squareImage;
    }

    //Based on: https://gist.github.com/darkfall/1656050
    public static Icon BitmapToIcon(Bitmap input, int size = 16)
    {
      using var output = new MemoryStream();
      var newBitmap = new Bitmap(input, new Size(size, size));

      using (var memoryStream = new MemoryStream())
      {
        newBitmap.Save(memoryStream, ImageFormat.Png);

        var iconWriter = new BinaryWriter(output);

        iconWriter.Write((byte)0);
        iconWriter.Write((byte)0);
        iconWriter.Write((short)1);
        iconWriter.Write((short)1);
        iconWriter.Write((byte)size);
        iconWriter.Write((byte)size);
        iconWriter.Write((byte)0);
        iconWriter.Write((byte)0);
        iconWriter.Write((short)0);
        iconWriter.Write((short)32);
        iconWriter.Write((int)memoryStream.Length);
        iconWriter.Write(6 + 16);
        iconWriter.Write(memoryStream.ToArray());
        iconWriter.Flush();
      }

      output.Position = 0;
      return new Icon(output);
    }

    private static Icon LoadIconIntoMemory(string filename)
    {
      Icon icon;

      using var stream = new FileStream(filename, FileMode.Open, FileAccess.Read);
      var tempIcon = new Icon(stream);
      icon = new Icon(tempIcon, tempIcon.Size);
      tempIcon.Dispose();

      return icon;
    }

    private static Image LoadImageIntoMemory(string filename)
    {
      Image image;

      using var stream = new FileStream(filename, FileMode.Open, FileAccess.Read);
      var tempImage = Image.FromStream(stream);
      image = new Bitmap(tempImage);
      tempImage.Dispose();

      return image;
    }
  }
}
