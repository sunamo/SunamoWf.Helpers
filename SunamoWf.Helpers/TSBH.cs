namespace SunamoWf.Helpers;

/// <summary>
/// Helper for ToolStripButton.
/// </summary>
public class TSBH
{
    /// <summary>
    /// Creates an image-only ToolStripButton with the given image and text (text is used as tooltip).
    /// </summary>
    public static ToolStripButton CreateNew(Image image, string text)
    {
        return new ToolStripButton
        {
            Text = text,
            Image = image,
            DisplayStyle = ToolStripItemDisplayStyle.Image
        };
    }
}
