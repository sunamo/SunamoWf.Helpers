namespace SunamoWf.Helpers;

/// <summary>
/// Typed variant of TSDDBH exposing the selected value as T.
/// </summary>
/// <typeparam name="T">Type of the values (typically an enum).</typeparam>
public class TSDDBHT<T> : TSDDBH
{
    /// <summary>
    /// Wraps the given drop-down button without adding items.
    /// </summary>
    public TSDDBHT(ToolStripDropDownButton button)
        : base(button)
    {
    }

    /// <summary>
    /// Wraps the button, adds the values as items, selects defaultValue and appends it to the tooltip.
    /// </summary>
    public TSDDBHT(ToolStripDropDownButton button, Array values, T defaultValue)
        : base(button)
    {
        originalToolTipText = button.ToolTipText;
        AddValuesOfEnumAsItems(values);
        SelectedO = defaultValue;
        button.ToolTipText = originalToolTipText + " " + defaultValue;
    }

    /// <summary>
    /// The selected value cast to T.
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
/// ToolStripDropDownButtonHelper - fills a drop-down button with items and tracks the selected one.
/// </summary>
public class TSDDBH
{
    /// <summary>
    /// The wrapped drop-down button.
    /// </summary>
    protected ToolStripDropDownButton tsddb;

    /// <summary>
    /// The previously checked item.
    /// </summary>
    protected ToolStripMenuItem prev = new ToolStripMenuItem();

    /// <summary>
    /// Tooltip text of the button before the selection was appended.
    /// </summary>
    protected string originalToolTipText = "";

    /// <summary>
    /// Currently selected value (item Tag, or the item itself when tags are not used); null when nothing is selected.
    /// </summary>
    public object? SelectedO = null;

    private readonly bool _useTags = true;

    /// <summary>
    /// Wraps the button; useTags decides whether the selected value is the item Tag or the item itself.
    /// </summary>
    public TSDDBH(ToolStripDropDownButton button, bool useTags)
    {
        tsddb = button;
        _useTags = useTags;
    }

    /// <summary>
    /// Wraps the button, selected value is the item Tag.
    /// </summary>
    public TSDDBH(ToolStripDropDownButton button)
    {
        tsddb = button;
    }

    /// <summary>
    /// True when a value with non-empty text is selected.
    /// </summary>
    public bool Selected
    {
        get
        {
            return SelectedO != null && SelectedO.ToString()!.Trim() != "";
        }
    }

    /// <summary>
    /// Text of the selected value.
    /// </summary>
    public string SelectedS
    {
        get
        {
            return SelectedO!.ToString()!;
        }
    }

    /// <summary>
    /// Adds one checkable item per value; the first one is checked. Selection is tracked via Click.
    /// </summary>
    public void AddValuesOfEnumAsItems(Array values)
    {
        int index = 0;
        foreach (object value in values)
        {
            ToolStripMenuItem item = new ToolStripMenuItem
            {
                Text = value.ToString(),
                Tag = value
            };
            if (index == 0)
            {
                item.Checked = true;
                prev = item;
            }
            item.Click += OnItemClick;
            tsddb.DropDownItems.Add(item);
            index++;
        }
    }

    /// <summary>
    /// Moves the check mark to the clicked item, stores the selection and shows it in the tooltip.
    /// </summary>
    public void OnItemClick(object? sender, EventArgs eventArgs)
    {
        prev.Checked = false;
        ToolStripMenuItem item = (ToolStripMenuItem)sender!;
        item.Checked = true;
        prev = item;
        SelectedO = _useTags ? item.Tag : item;
        tsddb.ToolTipText = originalToolTipText + " " + SelectedO;
    }

    /// <summary>
    /// Adds one item per object with the object as Tag and the given Click handler.
    /// </summary>
    public void AddValuesOfArrayAsItems(EventHandler handler, params object[] values)
    {
        foreach (object value in values)
        {
            ToolStripMenuItem item = new ToolStripMenuItem
            {
                Text = value.ToString(),
                Tag = value
            };
            item.Click += handler;
            tsddb.DropDownItems.Add(item);
        }
    }

    /// <summary>
    /// Adds items with integer values: initialValue plus degrees steps of resizeOf below it (ascending, before it) and above it.
    /// </summary>
    public void AddValuesOfIntAsItems(EventHandler handler, int initialValue, int resizeOf, int degrees)
    {
        List<int> values = new List<int>();
        for (int index = degrees; index >= 1; index--)
        {
            values.Add(initialValue - index * resizeOf);
        }
        values.Add(initialValue);
        for (int step = 1; step <= degrees; step++)
        {
            values.Add(initialValue + step * resizeOf);
        }
        foreach (int value in values)
        {
            ToolStripMenuItem item = new ToolStripMenuItem
            {
                Text = value.ToString(),
                Tag = value
            };
            item.Click += handler;
            tsddb.DropDownItems.Add(item);
        }
    }
}
