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
            return AllExtensions.jpg;
        }
        else if (formatGuid == ImageFormat.Gif.Guid)
        {
            return AllExtensions.gif;
        }
        else if (formatGuid == ImageFormat.Bmp.Guid)
        {
            return AllExtensions.bmp;
        }
        else if (formatGuid == ImageFormat.Icon.Guid)
        {
            return AllExtensions.ico;
        }
        else if (formatGuid == ImageFormat.Tiff.Guid)
        {
            return AllExtensions.tiff;
        }
        else if (formatGuid == ImageFormat.Wmf.Guid)
        {
            return AllExtensions.wmf;
        }
        else if (formatGuid == ImageFormat.Emf.Guid)
        {
            return AllExtensions.emf;
        }
        else if (formatGuid == ImageFormat.Exif.Guid)
        {
            return AllExtensions.exif;
        }
        else if (formatGuid == ImageFormat.MemoryBmp.Guid)
        {
            return AllExtensions.bmp;
        }
        else
        {
            ThrowEx.NotImplementedCase(formatGuid);
        }
        return null;
    }
}
