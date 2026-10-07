namespace SunamoWf.Helpers;

/// <summary>
/// A WinForms TextBox with folder-path autocomplete. Must be WinForms because the WPF TextBox has no built-in autocomplete.
/// </summary>
public class TextBoxPath : TextBox
{
    private string _basePath;
    private const string Delimiter = @"\";
    private int _previousOccurrences = 0;
    private List<string> _folders = null;

    /// <summary>
    /// Enables Suggest/CustomSource autocomplete and hooks TextChanged.
    /// </summary>
    public TextBoxPath()
    {
        AutoCompleteMode = AutoCompleteMode.Suggest;
        base.AutoCompleteSource = AutoCompleteSource.CustomSource;
        TextChanged += TextBoxPath_TextChanged;
    }

    /// <summary>
    /// Loads the initial folder suggestion list rooted at basePath.
    /// </summary>
    public void Init(string basePath)
    {
        _folders = GetFolders(basePath);

        basePath = PathShim.WithEndSlash(basePath);

        _basePath = basePath;
    }

    private List<string> GetFolders(string basePath)
    {
        _folders = PathShim.GetSubFolders(basePath);
        basePath = PathShim.WithEndSlash(basePath);
        for (int index = 0; index < _folders.Count; index++)
        {
            if (_folders[index].StartsWith(basePath, StringComparison.Ordinal))
            {
                _folders[index] = _folders[index].Substring(basePath.Length);
            }
        }

        return _folders;
    }

    private void TextBoxPath_TextChanged(object sender, EventArgs eventArgs)
    {
        TextBox textBox = (TextBox)sender;
        if (textBox != null)
        {
            List<string> suggestions = SuggestStrings(textBox.Text);

            AutoCompleteStringCollection collection = new AutoCompleteStringCollection();
            collection.AddRange(suggestions.ToArray());

            base.AutoCompleteCustomSource = collection;
        }
    }

    private List<string> SuggestStrings(string enteredText)
    {
        string trimmedText = enteredText.Trim().Trim('\\', '/');
        List<string> tokens = PathShim.GetTokens(trimmedText);

        string lastToken = tokens[tokens.Count - 1];

        if (!enteredText.EndsWith(Delimiter))
        {
            return _folders.Where(folder => folder.StartsWith(lastToken)).ToList();
        }

        // Entered text ends with the path delimiter - re-resolve the folder list for the new base path.
        int occurrences = enteredText.Split(new[] { Delimiter }, StringSplitOptions.None).Length - 1;
        if (occurrences != _previousOccurrences)
        {
            _previousOccurrences = occurrences;
            _folders = GetFolders(Path.Combine(_basePath, trimmedText));
        }

        return _folders;
    }
}
