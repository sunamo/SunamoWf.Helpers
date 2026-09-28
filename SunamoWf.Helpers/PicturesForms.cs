namespace SunamoWf.Helpers;

/// <summary>
/// Small WinForms bitmap helper (diagnostic info).
/// </summary>
public class PicturesForms
{
    /// <summary>
    /// Returns a human readable multi-line summary (width/height) of the given bitmap.
    /// </summary>
    public static string InfoAbout(Bitmap bitmap)
    {
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.AppendLine("Width: " + bitmap.Width);
        stringBuilder.AppendLine("Height: " + bitmap.Height);
        return stringBuilder.ToString();
    }
}
