namespace SunamoWf.Helpers;

/// <summary>
/// Helper for ToolStripTextBox raising an event when Enter is pressed.
/// </summary>
public class TSTBH
{
    /// <summary>
    /// The wrapped ToolStripTextBox.
    /// </summary>
    public ToolStripTextBox tstb;

    /// <summary>
    /// Raised with the text when Enter is pressed and the text is not empty.
    /// </summary>
    public event KeyEventHandler? PressEnter;

    private readonly bool _clearAfterClick;

    /// <summary>
    /// Wraps the ToolStripTextBox; when clearAfterClick is true the text is cleared on click, otherwise after Enter.
    /// </summary>
    public TSTBH(ToolStripTextBox tstb, bool clearAfterClick)
    {
        this.tstb = tstb;
        _clearAfterClick = clearAfterClick;
        tstb.Click += OnClick;
        tstb.KeyDown += OnKeyDown;
    }

    /// <summary>
    /// Raises PressEnter on Enter with non-empty text, then clears the text when configured not to clear on click.
    /// </summary>
    private void OnKeyDown(object? sender, KeyEventArgs e)
    {
        if (tstb.Text.Trim() != "" && e.KeyCode == Keys.Enter)
        {
            PressEnter?.Invoke(tstb.Text, e);
            if (!_clearAfterClick)
            {
                tstb.Text = "";
            }
        }
    }

    /// <summary>
    /// Clears the text on click when configured so.
    /// </summary>
    private void OnClick(object? sender, EventArgs e)
    {
        if (_clearAfterClick)
        {
            tstb.Text = "";
        }
    }

    /// <summary>
    /// Creates a helper for the given ToolStripTextBox.
    /// </summary>
    public static TSTBH NastavTSTB(ToolStripTextBox tstb, bool cleanAfterClick)
    {
        return new TSTBH(tstb, cleanAfterClick);
    }
}
