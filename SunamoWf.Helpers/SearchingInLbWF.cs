namespace SunamoWf.Helpers;

/// <summary>
/// Adds live text filtering to a WinForms ListBox, driven by a ToolStripTextBox search field.
/// </summary>
public class SearchingInLbWF
{
    /// <summary>
    /// ListBox showing the filtered results.
    /// </summary>
    private ListBox _listBox;

    /// <summary>
    /// TextBox containing the entered search term.
    /// </summary>
    private ToolStripTextBox _searchTextBox;

    /// <summary>
    /// Original (unfiltered) items, restored when the search is cleared.
    /// </summary>
    private object[] _originalItems;

    /// <summary>
    /// Wires the search box, clear button and clear menu item to the given list box.
    /// </summary>
    public SearchingInLbWF(ListBox listBox, ToolStripTextBox searchTextBox, ToolStripButton clearButton, ToolStripMenuItem clearMenuItem)
    {
        _listBox = listBox;
        _searchTextBox = searchTextBox;
        _searchTextBox.TextChanged += SearchTextBox_TextChanged;
        _searchTextBox.KeyDown += SearchTextBox_KeyDown;
        clearButton.Click += ClearButton_Click;
        clearMenuItem.Click += ClearMenuItem_Click;

        List<object> items = new List<object>();
        foreach (object item in listBox.Items)
        {
            items.Add(item);
        }
        _originalItems = items.ToArray();
    }

    private void ClearMenuItem_Click(object sender, EventArgs e)
    {
        _searchTextBox.Text = "";
    }

    private void SearchTextBox_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.KeyCode == Keys.Back)
        {
            _searchTextBox.Text = "";
        }
    }

    /// <summary>
    /// Enables or disables filtering of the list box by the current search text.
    /// </summary>
    public void Searching(bool enabled)
    {
        if (enabled)
        {
            List<object> matchingItems = new List<object>();
            foreach (object item in _originalItems)
            {
                if (item.ToString().Contains(_searchTextBox.Text))
                {
                    matchingItems.Add(item);
                }
            }
            _listBox.Items.Clear();
            _listBox.Items.AddRange(matchingItems.ToArray());
        }
        else
        {
            _listBox.Items.Clear();
            _listBox.Items.AddRange(_originalItems);
        }
    }

    private void ClearButton_Click(object sender, EventArgs e)
    {
        _searchTextBox.Text = "";
    }

    private void SearchTextBox_TextChanged(object sender, EventArgs e)
    {
        if (_searchTextBox.Text == "")
        {
            Searching(false);
        }
        else
        {
            Searching(true);
        }
    }
}
