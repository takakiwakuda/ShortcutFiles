using System;
using Xunit;

namespace ShortcutFiles.Tests;

public class ShortcutFileIconLocationTests
{
    [Fact]
    public void GetIconLocationFromDisposedObject()
    {
        var shortcut = TestHelpers.OpenTestShortcutFile();
        shortcut.Dispose();

        Assert.Throws<ObjectDisposedException>(() => _ = shortcut.IconLocation);
    }

    [Fact]
    public void GetIconLocation()
    {
        using var shortcut = TestHelpers.OpenTestShortcutFile();

        Assert.Equal(@"%SystemRoot%\System32\shell32.dll", shortcut.IconLocation.Name);
        Assert.Equal(23, shortcut.IconLocation.Index);
    }

    [Fact]
    public void SetIconLocationToDisposedObject()
    {
        var shortcut = TestHelpers.OpenTestShortcutFile();
        shortcut.Dispose();

        Assert.Throws<ObjectDisposedException>(() => shortcut.IconLocation = IconLocation.Default);
    }

    [Fact]
    public void SetIconLocation()
    {
        using var shortcut = TestHelpers.OpenTestShortcutFile();
        shortcut.IconLocation = IconLocation.Default;

        Assert.Equal(IconLocation.Default.Name, shortcut.IconLocation.Name);
        Assert.Equal(IconLocation.Default.Index, shortcut.IconLocation.Index);
    }
}
