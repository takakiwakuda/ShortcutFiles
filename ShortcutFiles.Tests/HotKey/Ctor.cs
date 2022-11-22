using System;
using Xunit;

namespace ShortcutFiles.Tests;

public class HotKeyCtorTests
{
    [Fact]
    public void CtorMinimumExceeded()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => new HotKey(int.MinValue));
    }

    [Fact]
    public void CtorMaximumExceeded()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => new HotKey(int.MaxValue));
    }

    [Fact]
    public void CtorInvalidVirtualKey()
    {
        var exception = Assert.Throws<ArgumentException>(() => new HotKey(0x000A));
        Assert.StartsWith("The low-order byte of the value 10", exception.Message);
    }

    [Fact]
    public void CtorInvalidModifierKeys()
    {
        var exception = Assert.Throws<ArgumentException>(() => new HotKey(0x1000));
        Assert.StartsWith("The high-order byte of the value 4096", exception.Message);
    }

    [Fact]
    public void CtorNotSpecified()
    {
        var hotKey = new HotKey();
        Assert.Equal(VirtualKey.None, hotKey.Key);
        Assert.Equal(ModifierKey.None, hotKey.ModifierKeys);
    }

    [Theory]
    [InlineData(0x0000)] // None
    [InlineData(0x0341)] // Ctrl + Shift + A
    [InlineData(0x0060)] // Num0
    public void Ctor(int value)
    {
        var hotKey = new HotKey(value);

        Assert.Equal(TestHelpers.GetVirtualKey(value), hotKey.Key);
        Assert.Equal(TestHelpers.GetModifierKey(value), hotKey.ModifierKeys);
    }
}
