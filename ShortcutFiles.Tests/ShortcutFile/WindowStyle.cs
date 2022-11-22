using System;
using System.ComponentModel;
using Xunit;

namespace ShortcutFiles.Tests;

public class ShortcutFileWindowStyleTests
{
    [Fact]
    public void GetWindowStyleFromDisposedObject()
    {
        var shortcut = TestHelpers.OpenTestShortcutFile();
        shortcut.Dispose();

        Assert.Throws<ObjectDisposedException>(() => _ = shortcut.WindowStyle);
    }

    [Fact]
    public void GetWindowStyle()
    {
        using var shortcut = TestHelpers.OpenTestShortcutFile();

        Assert.Equal(ShortcutWindowStyle.Normal, shortcut.WindowStyle);
    }

    [Fact]
    public void SetWindowStyleToDisposedObject()
    {
        var shortcut = TestHelpers.OpenTestShortcutFile();
        shortcut.Dispose();

        Assert.Throws<ObjectDisposedException>(() => shortcut.WindowStyle = ShortcutWindowStyle.Normal);
    }

    [Fact]
    public void SetWindowStyleWithInvalidValue()
    {
        using var shortcut = TestHelpers.OpenTestShortcutFile();

        Assert.Throws<InvalidEnumArgumentException>(() => shortcut.WindowStyle = 0);
    }

    [Fact]
    public void SetWindowStyle()
    {
        using var shortcut = TestHelpers.OpenTestShortcutFile();
        shortcut.WindowStyle = ShortcutWindowStyle.Maximized;

        Assert.Equal(ShortcutWindowStyle.Maximized, shortcut.WindowStyle);
    }
}
