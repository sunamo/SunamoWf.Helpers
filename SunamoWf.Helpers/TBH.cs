namespace SunamoWf.Helpers;

/// <summary>
/// Helper methods for TextBox.
/// </summary>
public class TBH
{
    /// <summary>
    /// Makes Ctrl+A select all text in the TextBox. Prefer a TextBox control that does this itself.
    /// </summary>
    public static void RegisterHandlerSelectAll(TextBox textBox)
    {
        textBox.KeyDown += OnKeyDown;
    }

    /// <summary>
    /// Selects all text when Ctrl+A is pressed.
    /// </summary>
    private static void OnKeyDown(object? sender, KeyEventArgs eventArgs)
    {
        if (eventArgs.Control && eventArgs.KeyCode == Keys.A)
        {
            ((TextBox)sender!).SelectAll();
        }
    }
}
