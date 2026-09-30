namespace SunamoWf.Helpers;

/// <summary>
/// WinForms (System.Drawing) image helpers shared across the SunamoWf family.
/// </summary>
public partial class PicturesSunamo
{
    /// <summary>
    /// Returns the file extension (without dot) matching the image's RawFormat.
    /// </summary>
    public static string ExtensionFromImage(Image image)
    {
        Guid formatGuid = image.RawFormat.Guid;
        if (formatGuid == ImageFormat.Jpeg.Guid)
        {
            return ImageExtensions.jpg;
        }
        else if (formatGuid == ImageFormat.Gif.Guid)
        {
            return ImageExtensions.gif;
        }
        else if (formatGuid == ImageFormat.Bmp.Guid)
        {
            return ImageExtensions.bmp;
        }
        else if (formatGuid == ImageFormat.Icon.Guid)
        {
            return ImageExtensions.ico;
        }
        else if (formatGuid == ImageFormat.Tiff.Guid)
        {
            return ImageExtensions.tiff;
        }
        else if (formatGuid == ImageFormat.Wmf.Guid)
        {
            return ImageExtensions.wmf;
        }
        else if (formatGuid == ImageFormat.Emf.Guid)
        {
            return ImageExtensions.emf;
        }
        else if (formatGuid == ImageFormat.Exif.Guid)
        {
            return ImageExtensions.exif;
        }
        else if (formatGuid == ImageFormat.MemoryBmp.Guid)
        {
            return ImageExtensions.bmp;
        }
        else
        {
            throw new NotImplementedException($"Unsupported image format: {formatGuid}");
        }
        return null;
    }
}
