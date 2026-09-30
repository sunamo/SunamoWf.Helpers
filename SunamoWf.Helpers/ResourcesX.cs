using System.Collections;
using System.Resources;

namespace SunamoWf.Helpers;

/// <summary>
/// Helpers for editing .resx resource files.
/// </summary>
public class ResourcesX
{
    /// <summary>
    /// Keeps all existing entries of the .resx file at the path (in the original order) and appends the keys from data that are not present yet.
    /// Based on https://stackoverflow.com/a/40509690
    /// </summary>
    public static void UpdateResourceFile(Hashtable data, string path)
    {
        Hashtable existingKeys = new Hashtable();
        List<ResXDataNode> existingNodes = new List<ResXDataNode>();

        using (ResXResourceReader reader = new ResXResourceReader(path))
        {
            reader.UseResXDataNodes = true;
            foreach (DictionaryEntry entry in reader)
            {
                existingKeys.Add(entry.Key.ToString()!, "");
                existingNodes.Add((ResXDataNode)entry.Value!);
            }
        }

        using ResXResourceWriter writer = new ResXResourceWriter(path);
        foreach (ResXDataNode node in existingNodes)
        {
            writer.AddResource(node);
        }
        foreach (string key in data.Keys)
        {
            if (!existingKeys.ContainsKey(key))
            {
                writer.AddResource(key, data[key]?.ToString() ?? "");
            }
        }
        writer.Generate();
    }
}
