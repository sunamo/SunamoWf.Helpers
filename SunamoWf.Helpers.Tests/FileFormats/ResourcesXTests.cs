using System.Collections;
using System.IO;
using System.Resources;
using SunamoWf.Helpers;

namespace SunamoWf.Helpers.Tests;

public class ResourcesXTests
{
    [Fact]
    public void UpdateResourceFileTest()
    {
        Hashtable ht = new Hashtable();
        ht.Add("path", @"E:\vs\Projects\PlatformIndependentNuGetPackages\sunamo\MultilingualResources\sunamo.en-US.xlf");

        // The original test pointed at a resx file that no longer exists; a temporary one with the same shape is used instead.
        string resx = Path.Combine(Path.GetTempPath(), "ResourcesDuo_" + Guid.NewGuid().ToString("N") + ".resx");
        using (ResXResourceWriter writer = new ResXResourceWriter(resx))
        {
            writer.AddResource("existing", "value");
            writer.Generate();
        }

        try
        {
            ResourcesX.UpdateResourceFile(ht, resx);
            Assert.Contains("path", File.ReadAllText(resx));
        }
        finally
        {
            File.Delete(resx);
        }
    }
}
