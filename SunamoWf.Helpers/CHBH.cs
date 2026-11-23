namespace SunamoWf;

/// <summary>
/// Helper for working with multiple CheckBox controls at once (not CheckedListBox).
/// </summary>
public class CHBH
{
    /// <summary>
    /// Sets the Checked state of every CheckBox nested in the given control.
    /// </summary>
    public static void SetChecked(Control control, bool isChecked)
    {
        foreach (CheckBox checkBox in OPH.GetRecursiveAllSubControlsOfType<CheckBox>(control))
        {
            checkBox.Checked = isChecked;
        }
    }

    /// <summary>
    /// Returns Checked when all values are true, Unchecked when all are false, otherwise Indeterminate.
    /// </summary>
    public static CheckState GetCheckState(List<bool> values)
    {
        // No values means nothing is checked.
        if (values.Count == 0)
        {
            return CheckState.Unchecked;
        }

        if (values.All(value => value))
        {
            return CheckState.Checked;
        }
        if (values.All(value => !value))
        {
            return CheckState.Unchecked;
        }
        return CheckState.Indeterminate;
    }
}