namespace SunamoWf.Helpers.Internal;

/// <summary>
/// Minimal path/file helpers used by the WinForms helpers, replacing the former dependency on the FS/TF/SH/CA utility packages.
/// </summary>
internal static class PathShim
{
    /// <summary>
    /// Returns filePath with its extension replaced by newExtension (which includes the leading dot), keeping the directory.
    /// </summary>
    internal static string ChangeExtension(string filePath, string newExtension)
    {
        string directory = Path.GetDirectoryName(filePath) ?? string.Empty;
        return Path.Combine(directory, Path.GetFileNameWithoutExtension(filePath) + newExtension);
    }

    /// <summary>
    /// Returns the file name of fileName placed into the folder changeFolderTo.
    /// </summary>
    internal static string ChangeDirectory(string fileName, string changeFolderTo)
    {
        return Path.Combine(changeFolderTo, Path.GetFileName(fileName));
    }

    /// <summary>
    /// Creates the parent directory of the given file path unless it already exists.
    /// </summary>
    internal static void CreateUpfoldersUnlessThere(string path)
    {
        string? directory = Path.GetDirectoryName(path);
        if (!string.IsNullOrEmpty(directory))
        {
            Directory.CreateDirectory(directory);
        }
    }

    /// <summary>
    /// Ensures the value ends with exactly one backslash and starts with an upper-case first character (drive letter).
    /// </summary>
    internal static string WithEndSlash(string value)
    {
        if (value != string.Empty)
        {
            value = value.TrimEnd('\\') + '\\';
            value = char.ToUpperInvariant(value[0]) + value.Substring(1);
        }
        return value;
    }

    /// <summary>
    /// Returns the direct sub-folders of folderPath (each with trailing backslash), skipping junction points and unreadable folders.
    /// </summary>
    internal static List<string> GetSubFolders(string folderPath)
    {
        List<string> result = new List<string>();
        try
        {
            foreach (string folder in Directory.GetDirectories(folderPath))
            {
                if (new DirectoryInfo(folder).Attributes.HasFlag(FileAttributes.ReparsePoint))
                {
                    continue;
                }
                result.Add(WithEndSlash(folder));
            }
        }
        catch (Exception)
        {
            // Unreadable/missing folder: no suggestions, same as the previous silent behavior.
        }
        return result;
    }

    /// <summary>
    /// Splits a path into its non-empty segments using both slash kinds as separators.
    /// </summary>
    internal static List<string> GetTokens(string path)
    {
        return path.Split(new[] { '\\', '/' }, StringSplitOptions.RemoveEmptyEntries).ToList();
    }
}
