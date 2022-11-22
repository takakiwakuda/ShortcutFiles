using System;
using Xunit;

namespace ShortcutFiles.Tests;

public class ShortcutFileHotKeyTests
{
    [Fact]
    public void GetHotKeyFromDisposedObject()
    {
        var shortcut = TestHelpers.OpenTestShortcutFile();
        shortcut.Dispose();

        Assert.Throws<ObjectDisposedException>(() => _ = shortcut.HotKey);
    }

    [Fact]
    public void GetHotKey()
    {
        using var shortcut = TestHelpers.OpenTestShortcutFile();

        Assert.Equal(VirtualKey.None, shortcut.HotKey.Key);
        Assert.Equal(ModifierKey.None, shortcut.HotKey.ModifierKeys);
    }

    [Fact]
    public void SetHotKeyToDisposedObject()
    {
        var shortcut = TestHelpers.OpenTestShortcutFile();
        shortcut.Dispose();

        Assert.Throws<ObjectDisposedException>(() => shortcut.HotKey = HotKey.None);
    }

    [Theory]
    [InlineData(0x0000)] // None
    [InlineData(0x0341)] // Ctrl + Shift + A
    [InlineData(0x0060)] // Num0
    public void SetHotKey(int hotKey)
    {
        using var shortcut = TestHelpers.OpenTestShortcutFile();
        shortcut.HotKey = new HotKey(hotKey);

        Assert.Equal(TestHelpers.GetVirtualKey(hotKey), shortcut.HotKey.Key);
        Assert.Equal(TestHelpers.GetModifierKey(hotKey), shortcut.HotKey.ModifierKeys);
    }
}
