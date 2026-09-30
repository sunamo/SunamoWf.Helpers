namespace SunamoWf.Helpers;

/// <summary>
/// Simple Dialog Helper - shows message boxes titled with the application name.
/// </summary>
public class SDH
{
    /// <summary>
    /// Shows an information message box.
    /// </summary>
    public static DialogResult Information(string message)
    {
        return MessageBox.Show(message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
    }

    /// <summary>
    /// Shows a warning message box.
    /// </summary>
    public static DialogResult Warning(string message)
    {
        return MessageBox.Show(message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
    }

    /// <summary>
    /// Shows an error message box.
    /// </summary>
    public static DialogResult Error(string message)
    {
        return MessageBox.Show(message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
    }
}
