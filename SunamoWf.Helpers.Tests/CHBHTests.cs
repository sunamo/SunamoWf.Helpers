using System.Collections.Generic;
using System.Windows.Forms;

namespace SunamoWf.Helpers.Tests;

public class CHBHTests
{
    [Fact]
    public void GetCheckState_EmptyListIsUnchecked()
    {
        Assert.Equal(CheckState.Unchecked, SunamoWf.CHBH.GetCheckState(new List<bool>()));
    }

    [Fact]
    public void GetCheckState_MixedIsIndeterminate()
    {
        Assert.Equal(CheckState.Checked, SunamoWf.CHBH.GetCheckState(new List<bool> { true, true }));
        Assert.Equal(CheckState.Indeterminate, SunamoWf.CHBH.GetCheckState(new List<bool> { true, false }));
    }
}
