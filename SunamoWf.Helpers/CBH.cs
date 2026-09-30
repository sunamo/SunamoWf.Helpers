namespace SunamoWf.Helpers;

/// <summary>
/// Helper methods for WinForms ComboBox.
/// </summary>
public class CBH
{
    /// <summary>
    /// Returns the index of the first item whose text equals the given value, or -1 when not found.
    /// </summary>
    public static int IndexOf(ComboBox comboBox, string text)
    {
        int index = 0;
        foreach (object item in comboBox.Items)
        {
            if (item?.ToString() == text)
            {
                return index;
            }
            index++;
        }
        return -1;
    }
}
