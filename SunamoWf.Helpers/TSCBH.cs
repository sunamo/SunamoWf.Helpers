namespace SunamoWf.Helpers;

/// <summary>
/// Typed variant of TSCBH exposing the selected item as T.
/// </summary>
/// <typeparam name="T">Type of the items in the ToolStripComboBox.</typeparam>
public class TSCBHT<T> : TSCBH
{
    /// <summary>
    /// Wraps the given ToolStripComboBox.
    /// </summary>
    public TSCBHT(ToolStripComboBox comboBox)
        : base(comboBox)
    {
    }

    /// <summary>
    /// The selected item cast to T.
    /// </summary>
    public T SelectedT
    {
        get
        {
            return (T)SelectedO!;
        }
    }
}

/// <summary>
/// Helper for ToolStripComboBox: list-only drop down, SelectedIndexChanged event and selection accessors.
/// </summary>
public class TSCBH
{
    /// <summary>
    /// The wrapped ToolStripComboBox.
    /// </summary>
    protected ToolStripComboBox tscb;

    /// <summary>
    /// Raised when the selected index changes.
    /// </summary>
    public event Action? SelectedIndexChanged;

    /// <summary>
    /// Wraps the ToolStripComboBox and makes it a list-only drop down.
    /// </summary>
    public TSCBH(ToolStripComboBox comboBox)
    {
        tscb = comboBox;
        tscb.SelectedIndexChanged += OnSelectedIndexChanged;
        tscb.DropDownStyle = ComboBoxStyle.DropDownList;
    }

    /// <summary>
    /// Forwards the change to the SelectedIndexChanged event.
    /// </summary>
    private void OnSelectedIndexChanged(object? sender, EventArgs eventArgs)
    {
        SelectedIndexChanged?.Invoke();
    }

    /// <summary>
    /// The selected item.
    /// </summary>
    public object? SelectedO
    {
        get
        {
            return tscb.SelectedItem;
        }
    }

    /// <summary>
    /// True when an item with non-empty text is selected.
    /// </summary>
    public bool Selected
    {
        get
        {
            return !string.IsNullOrEmpty(SelectedS);
        }
    }

    /// <summary>
    /// Text of the selected item (null when nothing is selected). The setter replaces the selected item.
    /// </summary>
    public string? SelectedS
    {
        get
        {
            return tscb.SelectedItem?.ToString();
        }
        set
        {
            tscb.Items[tscb.SelectedIndex] = value;
        }
    }

    /// <summary>
    /// Fills the combo box with the names (without path) of files in the folder matching the mask (top directory only).
    /// </summary>
    public static void LoadFiles(string folder, string mask, ToolStripComboBox comboBox)
    {
        comboBox.Items.Clear();
        string[] files = Directory.GetFiles(folder, mask, SearchOption.TopDirectoryOnly);
        comboBox.Items.AddRange(files.Select(Path.GetFileName).ToArray()!);
    }
}
