using System;
using Xunit;

namespace ShortcutFiles.Tests;

public class ShortcutFileWorkingDirectoryTests
{
    [Fact]
    public void GetWorkingDirectoryFromDisposedObject()
    {
        var shortcut = TestHelpers.OpenTestShortcutFile();
        shortcut.Dispose();

        Assert.Throws<ObjectDisposedException>(() => _ = shortcut.WorkingDirectory);
    }

    [Fact]
    public void GetWorkingDirectory()
    {
        using var shortcut = TestHelpers.OpenTestShortcutFile();

        Assert.Equal("%USERPROFILE%", shortcut.WorkingDirectory);
    }

    [Fact]
    public void SetWorkingDirectoryToDisposedObject()
    {
        var shortcut = TestHelpers.OpenTestShortcutFile();
        shortcut.Dispose();

        Assert.Throws<ObjectDisposedException>(() => shortcut.WorkingDirectory = "a");
    }

    [Fact]
    public void SetWorkingDirectoryWithNull()
    {
        using var shortcut = TestHelpers.OpenTestShortcutFile();

        Assert.Throws<ArgumentNullException>(() => shortcut.WorkingDirectory = null);
    }

    [Theory]
    [InlineData("")]
    [InlineData(@"C:\Windows")]
    [InlineData(@"%windir%")]
    public void SetWorkingDirectory(string workingDirectory)
    {
        using var shortcut = TestHelpers.OpenTestShortcutFile();
        shortcut.WorkingDirectory = workingDirectory;

        Assert.Equal(workingDirectory, shortcut.WorkingDirectory);
    }
}
