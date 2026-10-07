using System.Drawing.Text;

namespace SunamoWf.Helpers;

/// <summary>
/// Helper methods for recoloring System.Drawing bitmaps.
/// </summary>
public class BitmapHelper
{
    /// <summary>
    /// Returns a copy of the bitmap where every sufficiently opaque pixel (alpha above 150) is painted red.
    /// </summary>
    /// <param name="scrBitmap">Source bitmap.</param>
    public static Bitmap ChangeColor(Bitmap scrBitmap)
    {
        Color newColor = Color.Red;
        var newBitmap = new Bitmap(scrBitmap.Width, scrBitmap.Height);

        for (int pixelX = 0; pixelX < scrBitmap.Width; pixelX++)
        {
            for (int pixelY = 0; pixelY < scrBitmap.Height; pixelY++)
            {
                var actualColor = scrBitmap.GetPixel(pixelX, pixelY);

                // Edge pixels have low alpha; keeping them untouched preserves smoothness.
                if (actualColor.A > 150)
                {
                    newBitmap.SetPixel(pixelX, pixelY, newColor);
                }
                else
                {
                    newBitmap.SetPixel(pixelX, pixelY, actualColor);
                }
            }
        }

        return newBitmap;
    }

    /// <summary>
    /// Replaces one color with another in the given image (drawn in place) and returns the same image.
    /// </summary>
    /// <param name="image">Image to modify.</param>
    /// <param name="fromColor">Color to replace.</param>
    /// <param name="toColor">Replacement color.</param>
    public static Image ChangeColor2(Image image, Color fromColor, Color toColor)
    {
        var attributes = new ImageAttributes();
        attributes.SetRemapTable(new ColorMap[]
        {
            new ColorMap
            {
                OldColor = fromColor,
                NewColor = toColor,
            }
        }, ColorAdjustType.Bitmap);

        using (Graphics graphics = Graphics.FromImage(image))
        {
            graphics.SmoothingMode = SmoothingMode.AntiAlias;
            graphics.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;
            graphics.InterpolationMode = InterpolationMode.High;

            graphics.DrawImage(
                image,
                new Rectangle(Point.Empty, image.Size),
                0, 0, image.Width, image.Height,
                GraphicsUnit.Pixel,
                attributes);
        }

        return image;
    }
}
