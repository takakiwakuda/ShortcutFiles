using System;
using Xunit;

namespace ShortcutFiles.Tests;

public class ShortcutFileOpenTests
{
    [Fact]
    public void OpenNull()
    {
        string? path = null;

        Assert.Throws<ArgumentNullException>(() => ShortcutFile.Open(path));
    }

    [Fact]
    public void OpenEmpty()
    {
        Assert.Throws<ArgumentException>(() => ShortcutFile.Open(string.Empty));
    }

    [Fact]
    public void Open()
    {
        string path = TestHelpers.GetTestFileName();
        using var shortcut = ShortcutFile.Open(path);

        Assert.Equal(path, shortcut.Name, ignoreCase: true);
    }
}
