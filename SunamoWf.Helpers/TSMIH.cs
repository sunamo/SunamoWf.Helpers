namespace SunamoWf.Helpers;

/// <summary>
/// Helper for ToolStripMenuItem.
/// </summary>
public class TSMIH
{
    private readonly ToolStripMenuItem _menuItem;

    /// <summary>
    /// Wraps the given menu item.
    /// </summary>
    public TSMIH(ToolStripMenuItem menuItem)
    {
        _menuItem = menuItem;
    }

    /// <summary>
    /// Adds one drop-down item per value of the array, with the value as Tag and the handler as Click.
    /// </summary>
    public void AddValuesOfEnumAsItems(Array values, EventHandler handler)
    {
        foreach (object value in values)
        {
            ToolStripMenuItem item = new ToolStripMenuItem
            {
                Text = value.ToString(),
                Tag = value
            };
            item.Click += handler;
            _menuItem.DropDownItems.Add(item);
        }
    }

    /// <summary>
    /// Creates a ToolStripMenuItem with the given text.
    /// </summary>
    public static ToolStripMenuItem CreateNew(string text)
    {
        return new ToolStripMenuItem { Text = text };
    }
}
