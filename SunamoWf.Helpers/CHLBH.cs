namespace SunamoWf.Helpers;

/// <summary>
/// Helper for working with a CheckedListBox.
/// </summary>
public class CHLBH
{
    private readonly CheckedListBox _checkedListBox;

    /// <summary>
    /// Wraps the given CheckedListBox.
    /// </summary>
    public CHLBH(CheckedListBox checkedListBox)
    {
        _checkedListBox = checkedListBox;
    }

    /// <summary>
    /// Text of all checked items.
    /// </summary>
    public List<string> AllSelectedS
    {
        get
        {
            List<string> result = new List<string>();
            foreach (object item in _checkedListBox.CheckedItems)
            {
                result.Add(item.ToString()!);
            }
            return result;
        }
    }

    /// <summary>
    /// All checked items. The setter unchecks everything and then checks the given items (matched by equality).
    /// </summary>
    public object[] AllSelected
    {
        get
        {
            List<object> result = new List<object>();
            foreach (object item in _checkedListBox.CheckedItems)
            {
                result.Add(item);
            }
            return result.ToArray();
        }
        set
        {
            UnCheckAll();
            foreach (object item in value)
            {
                int index = _checkedListBox.Items.IndexOf(item);
                if (index >= 0)
                {
                    _checkedListBox.SetItemChecked(index, true);
                }
            }
        }
    }

    /// <summary>
    /// Unchecks all items.
    /// </summary>
    public void UnCheckAll()
    {
        for (int index = 0; index < _checkedListBox.Items.Count; index++)
        {
            _checkedListBox.SetItemChecked(index, false);
        }
    }

    /// <summary>
    /// Checks all items.
    /// </summary>
    public void CheckAll()
    {
        for (int index = 0; index < _checkedListBox.Items.Count; index++)
        {
            _checkedListBox.SetItemChecked(index, true);
        }
    }
}
