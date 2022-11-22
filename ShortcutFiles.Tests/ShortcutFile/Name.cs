using System;
using Xunit;

namespace ShortcutFiles.Tests;

public class ShortcutFileNameTests
{
    [Fact]
    public void GetNameFromDisposedObject()
    {
        var shortcut = TestHelpers.OpenTestShortcutFile();
        shortcut.Dispose();

        Assert.Throws<ObjectDisposedException>(() => _ = shortcut.Name);
    }

    [Fact]
    public void GetName()
    {
        using var shortcut = TestHelpers.OpenTestShortcutFile();

        Assert.Equal(TestHelpers.GetTestFileName(), shortcut.Name, ignoreCase: true);
    }
}
