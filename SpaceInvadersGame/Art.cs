using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace SpaceInvadersGame
{
  public static class Art
  {
    public readonly static Bitmap[,] Enemies = Split(Load("enemies"), 16, 8);
    public readonly static Bitmap[,] Player = Split(Load("player"), 15, 8);
    public readonly static Bitmap[,] Letters = Split(Load("letters"), 6, 6);
    public readonly static Bitmap[,] Shields = Split(Load("shields"), 6, 6);

    public static Bitmap Load(string resourceName)
    {
      var resource = Resources.ResourceManager.GetObject(resourceName);
      if (resource is Bitmap) return resource as Bitmap;

      throw new InvalidCastException($"Resource {resourceName} cast is failed");
    }

    public static Bitmap[,] Split(Bitmap image, int xs, int ys)
    {
      int xSlices = image.Width / xs;
      int ySlices = image.Height / ys;
      var images = new Bitmap[xSlices, ySlices];
      for (int x = 0; x < xSlices; x++)
        for (int y = 0; y < ySlices; y++)
        {
          var bitmap = new Bitmap(xs, ys);
          using (var g = Graphics.FromImage(bitmap))
          {
            g.DrawImage(image, -x * xs, -y * ys);
            images[x, y] = bitmap;
          }
        }

      return images;
    }

    public static Bitmap Scale(Bitmap image, int scale)
    {
      var width = image.Width * scale;
      var height = image.Height * scale;
      var newimage = new Bitmap(width, height);
      using (var g = Graphics.FromImage(newimage))
      {
        g.InterpolationMode = InterpolationMode.NearestNeighbor;
        g.CompositingQuality = CompositingQuality.AssumeLinear;
        g.SmoothingMode = SmoothingMode.None;
        g.PixelOffsetMode = PixelOffsetMode.Half;
        g.DrawImage(image, 0, 0, width, height);
      }
      return newimage;
    }
  }
}
