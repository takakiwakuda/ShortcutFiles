using System;
using Xunit;

namespace ShortcutFiles.Tests;

public class ShortcutFileTargetPathTests
{
    [Fact]
    public void GetTargetPathFromDisposedObject()
    {
        var shortcut = TestHelpers.OpenTestShortcutFile();
        shortcut.Dispose();

        Assert.Throws<ObjectDisposedException>(() => _ = shortcut.TargetPath);
    }

    [Fact]
    public void GetTargetPath()
    {
        using var shortcut = TestHelpers.OpenTestShortcutFile();

        Assert.Equal(@"C:\Windows\System32\notepad.exe", shortcut.TargetPath, ignoreCase: true);
    }

    [Fact]
    public void SetTargetPathToDisposedObject()
    {
        var shortcut = TestHelpers.OpenTestShortcutFile();
        shortcut.Dispose();

        Assert.Throws<ObjectDisposedException>(() => shortcut.TargetPath = "a");
    }

    [Fact]
    public void SetTargetPathWithNull()
    {
        using var shortcut = TestHelpers.OpenTestShortcutFile();

        Assert.Throws<ArgumentNullException>(() => shortcut.TargetPath = null);
    }

    [Fact]
    public void SetTargetPathWithEmpty()
    {
        using var shortcut = TestHelpers.OpenTestShortcutFile();

        Assert.Throws<ArgumentException>(() => shortcut.TargetPath = string.Empty);
    }

    [Theory]
    [InlineData(@"C:\Windows\System32\cmd.exe")]
    [InlineData(@"%windir%\System32\cmd.exe")]
    public void SetTargetPath(string path)
    {
        using var shortcut = TestHelpers.OpenTestShortcutFile();
        shortcut.TargetPath = path;

        Assert.Equal(path, shortcut.TargetPath, ignoreCase: true);
    }
}
