namespace SunamoWf.Helpers;

/// <summary>
/// WinForms (System.Drawing) image helpers - rotation, base64 conversion, resizing, EXIF date, ico conversion.
/// </summary>
/// <remarks>
/// PlaceToCenter/PlaceToCenterExactly from the original desktop.wf/PicturesShared.cs were not migrated:
/// they depended on InitApp.TemplateLogger (not present anywhere in pinp/wnp) and were unfinished/experimental
/// (empty "#region MyRegion" blocks, garbled comments) in the source.
/// </remarks>
public class PicturesDesktopShared
{
    private static readonly Regex s_colonRegex = new Regex(":");

    /// <summary>
    /// Converts an image file to an .ico file (written into newDir) using the given conversion method.
    /// </summary>
    public static void ConvertImageToIco(string newDir, string path, Func<Image, Icon> method)
    {
        string newPath = PathShim.ChangeExtension(path, ".ico");
        newPath = PathShim.ChangeDirectory(newPath, newDir);
        using (FileStream fileStream = new FileStream(newPath, FileMode.OpenOrCreate))
        {
            Bitmap image = new Bitmap(path);
            Icon icon = method.Invoke(image);
            icon.Save(fileStream);
        }
    }

    /// <summary>
    /// Rotates a bitmap by a random angle between -90 and 45 degrees.
    /// </summary>
    public static Bitmap RotateBitmap(Image bitmap)
    {
        int angle = Random.Shared.Next(0, 45);
        angle -= Random.Shared.Next(0, 90);
        return RotateBitmap(bitmap, (float)angle);
    }

    /// <summary>
    /// Rotates a bitmap by the given angle (in degrees), resizing the canvas so the whole rotated image fits.
    /// </summary>
    public static Bitmap RotateBitmap(Image bitmap, float angle)
    {
        int newWidth, newHeight, offsetX, offsetY;
        double doubleWidth = (double)bitmap.Width;
        double doubleHeight = (double)bitmap.Height;

        double degrees = Math.Abs(angle);
        if (degrees <= 90)
        {
            double radians = 0.0174532925 * degrees;
            double sin = Math.Sin(radians);
            double cos = Math.Cos(radians);
            newWidth = (int)(doubleHeight * sin + doubleWidth * cos);
            newHeight = (int)(doubleWidth * sin + doubleHeight * cos);
            offsetX = (newWidth - bitmap.Width) / 2;
            offsetY = (newHeight - bitmap.Height) / 2;
        }
        else
        {
            degrees -= 90;
            double radians = 0.0174532925 * degrees;
            double sin = Math.Sin(radians);
            double cos = Math.Cos(radians);
            newWidth = (int)(doubleWidth * sin + doubleHeight * cos);
            newHeight = (int)(doubleHeight * sin + doubleWidth * cos);
            offsetX = (newWidth - bitmap.Width) / 2;
            offsetY = (newHeight - bitmap.Height) / 2;
        }

        float rotateAtX = bitmap.Width / 2f;
        float rotateAtY = bitmap.Height / 2f;

        Bitmap rotatedBitmap = new Bitmap(newWidth, newHeight);
        rotatedBitmap.SetResolution(bitmap.HorizontalResolution, bitmap.VerticalResolution);
        using (Graphics graphics = Graphics.FromImage(rotatedBitmap))
        {
            graphics.Clear(Color.Transparent);
            graphics.TranslateTransform(rotateAtX + offsetX, rotateAtY + offsetY);
            graphics.RotateTransform(angle);
            graphics.TranslateTransform(-rotateAtX - offsetX, -rotateAtY - offsetY);
            graphics.DrawImage(bitmap, new PointF(0 + offsetX, 0 + offsetY));
        }
        return rotatedBitmap;
    }

    /// <summary>
    /// Loads an image file and returns it as a base64 string, in the format matching its extension.
    /// </summary>
    public static string ImageToBase64(string imageFile)
    {
        return ImageToBase64(Bitmap.FromFile(imageFile), GetImageFormatFromExtension2(Path.GetExtension(imageFile)));
    }

    /// <summary>
    /// Re-encodes an image file at a new resolution (DPI), keeping the same pixel size, saving it as jpg.
    /// </summary>
    public static void ChangeResolution(string path, float dpiX, float dpiY)
    {
        Bitmap originalBitmap = new Bitmap(path);
        int width = originalBitmap.Width;
        int height = originalBitmap.Height;

        using (Bitmap resultBitmap = new Bitmap(width, height))
        {
            Point[] points =
            {
                new Point(0, 0),
                new Point(width, 0),
                new Point(0, height),
            };
            using (Graphics graphics = Graphics.FromImage(resultBitmap))
            {
                graphics.DrawImage(originalBitmap, points);
            }
            resultBitmap.SetResolution(dpiX, dpiY);
            SaveImage(PathShim.ChangeExtension(path, ".jpg"), resultBitmap, GetImageFormatFromExtension2(Path.GetExtension(path)));
        }
    }

    /// <summary>
    /// Maps a file extension (jpg/jpeg/png/gif) to its System.Drawing.Imaging.ImageFormat. Returns null for anything else.
    /// </summary>
    public static ImageFormat GetImageFormatFromExtension2(string extension)
    {
        extension = extension.TrimStart('.');
        if (extension == "jpeg" || extension == "jpg")
        {
            return ImageFormat.Jpeg;
        }
        else if (extension == "png")
        {
            return ImageFormat.Png;
        }
        else if (extension == "gif")
        {
            return ImageFormat.Gif;
        }
        return null;
    }

    /// <summary>
    /// Loads an image file and returns it as a base64 string, also returning its pixel width/height.
    /// </summary>
    public static string ImageToBase64(string path, ImageFormat format, out int width, out int height)
    {
        if (File.Exists(path))
        {
            Image image = Image.FromFile(path);
            width = image.Width;
            height = image.Height;
            return ImageToBase64(image, format);
        }
        width = 0;
        height = 0;
        return "";
    }

    /// <summary>
    /// Encodes an in-memory image as a base64 string in the given format.
    /// </summary>
    public static string ImageToBase64(Image image, ImageFormat format)
    {
        using (MemoryStream memoryStream = new MemoryStream())
        {
            image.Save(memoryStream, format);
            byte[] imageBytes = memoryStream.ToArray();
            return Convert.ToBase64String(imageBytes);
        }
    }

    /// <summary>
    /// Decodes a base64 string back into an Image.
    /// </summary>
    public static Image Base64ToImage(string base64String)
    {
        byte[] imageBytes = Convert.FromBase64String(base64String);
        MemoryStream memoryStream = new MemoryStream(imageBytes, 0, imageBytes.Length);
        memoryStream.Write(imageBytes, 0, imageBytes.Length);
        return Image.FromStream(memoryStream, true);
    }

    /// <summary>
    /// Reads the EXIF "date taken" property from an image file without loading the whole image, or returns getIfNotFound.
    /// </summary>
    public static DateTime GetDateTakenFromImage(string path, DateTime getIfNotFound)
    {
        using (FileStream fileStream = new FileStream(path, FileMode.Open, FileAccess.Read))
        using (Image image = Image.FromStream(fileStream, false, false))
        {
            const int dateTakenPropertyId = 36867;
            foreach (PropertyItem item in image.PropertyItems)
            {
                if (item.Id == dateTakenPropertyId)
                {
                    PropertyItem propertyItem = image.GetPropertyItem(dateTakenPropertyId);
                    string dateTaken = s_colonRegex.Replace(Encoding.UTF8.GetString(propertyItem.Value), "-", 2);
                    return DateTime.Parse(dateTaken);
                }
            }
            return getIfNotFound;
        }
    }

    /// <summary>
    /// Saves an image to disk in the given format at maximum quality settings.
    /// </summary>
    public static void SaveImage(string path, Image image, ImageFormat imageFormat)
    {
        using (MemoryStream memoryStream = new MemoryStream())
        using (FileStream fileStream = new FileStream(path, FileMode.Create, FileAccess.ReadWrite))
        {
            image.Save(memoryStream, imageFormat);
            byte[] bytes = memoryStream.ToArray();
            fileStream.Write(bytes, 0, bytes.Length);
        }
    }

    /// <summary>
    /// Resizes an image to the given width/height, using a high quality resize for jpg and GetThumbnailImage for png/gif.
    /// Caller must Dispose the returned image once no longer needed.
    /// </summary>
    public static Image ImageResize(Image image, int width, int height, ImageFormats imageFormats)
    {
        Bitmap sourceBitmap = new Bitmap(image);

        if (width > sourceBitmap.Width)
        {
            width = sourceBitmap.Width;
        }
        if (height > sourceBitmap.Height)
        {
            height = sourceBitmap.Height;
        }

        if (width == 0 & height == 0)
        {
            width = sourceBitmap.Width;
            height = sourceBitmap.Height;
        }
        else if (height == 0 & width != 0)
        {
            height = sourceBitmap.Height * width / sourceBitmap.Width;
        }
        else if (width == 0 & height != 0)
        {
            width = sourceBitmap.Width * height / sourceBitmap.Height;
        }

        Image resizedImage = null;
        switch (imageFormats)
        {
            case ImageFormats.Jpg:
                Size size = new Size(width, height);
                resizedImage = ResizeImageHighQuality(sourceBitmap, size);
                break;
            case ImageFormats.Png:
            case ImageFormats.Gif:
                resizedImage = sourceBitmap.GetThumbnailImage(width, height, null, IntPtr.Zero);
                break;
            default:
                break;
        }
        return resizedImage;
    }

    /// <summary>
    /// Resizes an image using GetThumbnailImage (suited to png/gif).
    /// Caller must Dispose the returned image once no longer needed.
    /// </summary>
    public static Image ImageResize(Image image, int width, int height)
    {
        return ImageResize(image, width, height, ImageFormats.Gif);
    }

    private static Bitmap ResizeImageHighQuality(Bitmap sourceBitmap, Size size)
    {
        int sourceWidth = sourceBitmap.Width;
        int sourceHeight = sourceBitmap.Height;

        float percentWidth = (float)size.Width / (float)sourceWidth;
        float percentHeight = (float)size.Height / (float)sourceHeight;

        Bitmap destinationBitmap = new Bitmap(size.Width, size.Height);

        using (Graphics graphics = Graphics.FromImage(destinationBitmap))
        {
            graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            graphics.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;
            graphics.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
            graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
            graphics.DrawImage(sourceBitmap, new Rectangle(0, 0, size.Width, size.Height), new Rectangle(0, 0, sourceBitmap.Width, sourceBitmap.Height), GraphicsUnit.Pixel);
        }

        foreach (PropertyItem propertyItem in sourceBitmap.PropertyItems)
        {
            destinationBitmap.SetPropertyItem(propertyItem);
        }

        sourceBitmap.Dispose();
        return destinationBitmap;
    }

    /// <summary>
    /// Calculates the top-left offset to center a source of size (w,h) within a target of size (finalWidth,finalHeight).
    /// Returns Point.Empty when sourceMustFullFillRequiredSize is true and the source is smaller than the target.
    /// </summary>
    public static Point CalculateForCrop(double width, double height, double finalWidth, double finalHeight, bool sourceMustFullFillRequiredSize)
    {
        if (width < finalWidth && sourceMustFullFillRequiredSize)
        {
            return Point.Empty;
        }

        if (height < finalHeight && sourceMustFullFillRequiredSize)
        {
            return Point.Empty;
        }

        double horizontalDifference = width - finalWidth;
        double left = 0;
        if (horizontalDifference != 0)
        {
            left = horizontalDifference / 2d;
        }

        double verticalDifference = height - finalHeight;
        double top = 0;
        if (verticalDifference != 0)
        {
            top = verticalDifference / 2d;
        }

        return new Point(Convert.ToInt32(left), Convert.ToInt32(top));
    }
}
