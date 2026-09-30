namespace SunamoWf.Helpers;

/// <summary>
/// Helper methods for traversing the WinForms control tree and range checks.
/// </summary>
public class OPH
{
    /// <summary>
    /// Returns the given control and all its descendant controls.
    /// </summary>
    public static List<Control> GetThisAndRecursiveAllSubControls(Control control)
    {
        List<Control> result = new List<Control>();
        CollectControls(control, result);
        return result;
    }

    /// <summary>
    /// Adds the control and all its descendants to the list.
    /// </summary>
    private static void CollectControls(Control control, List<Control> result)
    {
        result.Add(control);
        foreach (Control child in control.Controls)
        {
            CollectControls(child, result);
        }
    }

    /// <summary>
    /// Adds the control and all its descendants of exactly type T to the list.
    /// </summary>
    private static void CollectControlsOfType<T>(Control control, List<T> result) where T : class
    {
        if (control.GetType() == typeof(T))
        {
            result.Add((control as T)!);
        }
        foreach (Control child in control.Controls)
        {
            CollectControlsOfType(child, result);
        }
    }

    /// <summary>
    /// Returns all descendant controls of exactly type T (the control itself is not included).
    /// </summary>
    public static List<T> GetRecursiveAllSubControlsOfType<T>(Control control) where T : class
    {
        List<T> result = new List<T>();
        foreach (Control child in control.Controls)
        {
            CollectControlsOfType(child, result);
        }
        return result;
    }

    /// <summary>
    /// Returns true when index is a valid index into a collection of countItems items.
    /// </summary>
    public static bool IsInRange(int countItems, int index)
    {
        return index >= 0 && index < countItems;
    }
}
