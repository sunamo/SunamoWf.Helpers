namespace SunamoWf.Helpers;

/// <summary>
/// Helper for easier use of ToolStripProgressBar updated from worker threads.
/// </summary>
public class TSPBH
{
    private readonly ToolStripProgressBar _progressBar;
    private readonly Form _form;
    private readonly double _onePercent;
    private double _last;

    /// <summary>
    /// Resets the progress bar and prepares progress for the given total count of steps.
    /// </summary>
    public TSPBH(ToolStripProgressBar progressBar, int totalCount, Form form)
    {
        _form = form;
        _progressBar = progressBar;
        _progressBar.Value = 0;
        _onePercent = totalCount > 0 ? 100d / totalCount : 100d;
    }

    /// <summary>
    /// Advances the progress by one step (capped at 100 percent).
    /// </summary>
    public void Hotovo()
    {
        _last = Math.Min(100d, _last + _onePercent);
        int value = (int)_last;
        _form.Invoke(() => _progressBar.Value = value);
    }

    /// <summary>
    /// Sets the progress to 100 percent.
    /// </summary>
    public void HotovoUplne()
    {
        _form.Invoke(() => _progressBar.Value = 100);
    }
}
