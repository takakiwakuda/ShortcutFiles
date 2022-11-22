using System;
using Xunit;

namespace ShortcutFiles.Tests;

public class ShortcutFileDescriptionTests
{
    [Fact]
    public void GetDescriptionFromDisposedObject()
    {
        var shortcut = TestHelpers.OpenTestShortcutFile();
        shortcut.Dispose();

        Assert.Throws<ObjectDisposedException>(() => _ = shortcut.Description);
    }

    [Fact]
    public void GetDescription()
    {
        using var shortcut = TestHelpers.OpenTestShortcutFile();

        Assert.Equal("Test File", shortcut.Description);
    }

    [Fact]
    public void SetDescriptionToDisposedObject()
    {
        var shortcut = TestHelpers.OpenTestShortcutFile();
        shortcut.Dispose();

        Assert.Throws<ObjectDisposedException>(() => shortcut.Description = string.Empty);
    }

    [Fact]
    public void SetDescriptionWithNull()
    {
        using var shortcut = TestHelpers.OpenTestShortcutFile();

        Assert.Throws<ArgumentNullException>(() => shortcut.Description = null);
    }

    [Theory]
    [InlineData("")]
    [InlineData("Hello World")]
    [InlineData("こんにちは世界")]
    public void SetDescription(string description)
    {
        using var shortcut = TestHelpers.OpenTestShortcutFile();
        shortcut.Description = description;

        Assert.Equal(description, shortcut.Description);
    }
}
