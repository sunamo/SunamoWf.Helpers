namespace SunamoWf.Helpers;

/// <summary>
/// Typed variant of LBH_WF exposing the selected item as T.
/// </summary>
/// <typeparam name="T">Type of the items in the ListBox.</typeparam>
public class LBHT<T> : LBH_WF
{
    /// <summary>
    /// Wraps the given ListBox.
    /// </summary>
    public LBHT(ListBox listBox)
        : base(listBox)
    {
    }

    /// <summary>
    /// The selected item cast to T.
    /// </summary>
    public T SelectedT
    {
        get
        {
            return (T)SelectedO;
        }
    }

    /// <summary>
    /// Returns the items of the collection that are of type T.
    /// </summary>
    public static List<T> GetItemsListT(ListBox.ObjectCollection collection)
    {
        List<T> result = new List<T>();
        foreach (object item in collection)
        {
            if (item is T typed)
            {
                result.Add(typed);
            }
        }
        return result;
    }
}

/// <summary>
/// Easier handling of a WinForms ListBox: multi-select, Enter runs the item, C copies it, Delete raises ItemRemoved.
/// </summary>
public class LBH_WF
{
    /// <summary>
    /// ListBox being handled.
    /// </summary>
    protected ListBox lb;

    /// <summary>
    /// Raised when the selected item should be removed (Delete pressed and removeOne is true).
    /// </summary>
    public event Action<object>? ItemRemoved;

    /// <summary>
    /// When true, Enter starts the selected item as a process.
    /// </summary>
    public bool runOne = false;

    /// <summary>
    /// When true, C copies the selected item to the clipboard.
    /// </summary>
    public bool saveToClipboard = false;

    /// <summary>
    /// When true, Delete raises ItemRemoved for the selected item.
    /// </summary>
    public bool removeOne = false;

    /// <summary>
    /// Wraps the ListBox, enables extended multi-selection and hooks the KeyDown handler.
    /// </summary>
    public LBH_WF(ListBox listBox)
    {
        lb = listBox;
        lb.SelectionMode = SelectionMode.MultiExtended;
        lb.KeyDown += OnKeyDown;
    }

    /// <summary>
    /// Adds the items to the ListBox.
    /// </summary>
    public void AddRange(params object[] list)
    {
        lb.Items.AddRange(list);
    }

    /// <summary>
    /// Copies all ListBox items, one per line, to the clipboard.
    /// </summary>
    public void CopyToClipboard()
    {
        StringBuilder stringBuilder = new StringBuilder();
        foreach (object item in lb.Items)
        {
            stringBuilder.AppendLine(item.ToString());
        }
        Clipboard.SetText(stringBuilder.ToString());
    }

    /// <summary>
    /// Enter starts the selected item, C copies it to the clipboard, Delete raises ItemRemoved (each only when enabled).
    /// </summary>
    private void OnKeyDown(object? sender, KeyEventArgs eventArgs)
    {
        if (!Selected)
        {
            return;
        }
        if (eventArgs.KeyCode == Keys.Enter)
        {
            if (runOne)
            {
                System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo(SelectedS!) { UseShellExecute = true });
            }
        }
        else if (eventArgs.KeyCode == Keys.C)
        {
            if (saveToClipboard)
            {
                Clipboard.SetText(SelectedS!);
            }
        }
        else if (eventArgs.KeyCode == Keys.Delete)
        {
            if (removeOne)
            {
                ItemRemoved?.Invoke(SelectedO);
            }
        }
    }

    /// <summary>
    /// The selected item.
    /// </summary>
    public object SelectedO
    {
        get
        {
            return lb.SelectedItem;
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
            return lb.SelectedItem?.ToString();
        }
        set
        {
            lb.Items[lb.SelectedIndex] = value;
        }
    }

    /// <summary>
    /// Adds the item without checking whether it already exists.
    /// </summary>
    public void Add(string item)
    {
        lb.Items.Add(item);
    }

    /// <summary>
    /// Removes the item from the ListBox.
    /// </summary>
    public void Remove(string item)
    {
        lb.Items.Remove(item);
    }

    /// <summary>
    /// Returns the text of all items.
    /// </summary>
    public List<string> GetItemsListString()
    {
        return GetItemsListString(lb.Items);
    }

    /// <summary>
    /// Returns the text of all selected items.
    /// </summary>
    public static List<string> GetSelectedListString(ListBox.SelectedObjectCollection selectedObjectCollection)
    {
        List<string> result = new List<string>();
        foreach (object item in selectedObjectCollection)
        {
            result.Add(item.ToString()!);
        }
        return result;
    }

    /// <summary>
    /// Returns all items of the collection cast to T1.
    /// </summary>
    public static List<T1> GetItemsListT<T1>(ListBox.ObjectCollection objectCollection)
    {
        List<T1> result = new List<T1>();
        foreach (T1 item in objectCollection)
        {
            result.Add(item);
        }
        return result;
    }

    /// <summary>
    /// Returns the text of all items of the collection.
    /// </summary>
    public static List<string> GetItemsListString(ListBox.ObjectCollection objectCollection)
    {
        List<string> result = new List<string>();
        foreach (object item in objectCollection)
        {
            result.Add(item.ToString()!);
        }
        return result;
    }
}
