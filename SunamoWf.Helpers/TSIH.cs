namespace SunamoWf.Helpers;

/// <summary>
/// Helper for converting strings to ToolStrip items.
/// </summary>
public class TSIH
{
    /// <summary>
    /// Converts each string to a ToolStripMenuItem.
    /// </summary>
    public static ToolStripMenuItem[] ConvertFromArrayStringToTSMI(List<string> values)
    {
        return values.Select(TSMIH.CreateNew).ToArray();
    }

    /// <summary>
    /// Converts each string to a ToolStripItem (a ToolStripMenuItem).
    /// </summary>
    public static ToolStripItem[] ConvertFromArrayStringToTSI(List<string> values)
    {
        return values.Select(value => (ToolStripItem)TSMIH.CreateNew(value)).ToArray();
    }
}
