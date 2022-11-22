using System;
using Xunit;

namespace ShortcutFiles.Tests;

public class ShortcutFileArgumentsTests
{
    [Fact]
    public void GetArgumentsFromDisposedObject()
    {
        var shortcut = TestHelpers.OpenTestShortcutFile();
        shortcut.Dispose();

        Assert.Throws<ObjectDisposedException>(() => _ = shortcut.Arguments);
    }

    [Fact]
    public void GetArguments()
    {
        using var shortcut = TestHelpers.OpenTestShortcutFile();

        Assert.Equal("\"%USERPROFILE%\\Documents\\New.txt\"", shortcut.Arguments);
    }

    [Fact]
    public void SetArgumentsToDisposedObject()
    {
        var shortcut = TestHelpers.OpenTestShortcutFile();
        shortcut.Dispose();

        Assert.Throws<ObjectDisposedException>(() => shortcut.Arguments = string.Empty);
    }

    [Fact]
    public void SetArgumentsWithNull()
    {
        using var shortcut = TestHelpers.OpenTestShortcutFile();

        Assert.Throws<ArgumentNullException>(() => shortcut.Arguments = null);
    }

    [Theory]
    [InlineData("")]
    [InlineData("Hello World")]
    [InlineData("こんにちは世界")]
    public void SetArguments(string Arguments)
    {
        using var shortcut = TestHelpers.OpenTestShortcutFile();
        shortcut.Arguments = Arguments;

        Assert.Equal(Arguments, shortcut.Arguments);
    }
}
