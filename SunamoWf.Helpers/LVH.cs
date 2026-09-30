namespace SunamoWf.Helpers;

/// <summary>
/// Helper methods for ListView that marshal calls onto the UI thread.
/// </summary>
public class LVH
{
    /// <summary>
    /// Returns the sub item at the given column of the given row, adding an empty sub item when the column does not exist yet. Runs on the UI thread of the ListView.
    /// </summary>
    public static ListViewItem.ListViewSubItem GetListViewSubItemOnIndex(ListView listView, int rowIndex, int columnIndex)
    {
        return listView.Invoke(() =>
        {
            ListViewItem item = listView.Items[rowIndex];
            if (columnIndex < item.SubItems.Count)
            {
                return item.SubItems[columnIndex];
            }
            return item.SubItems.Add(string.Empty);
        });
    }
}
