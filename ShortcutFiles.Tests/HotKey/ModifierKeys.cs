using Xunit;

namespace ShortcutFiles.Tests;

public class HotKeyModifierKeysTests
{
    [Fact]
    public void GetModifierKeys()
    {
        var hotKey = new HotKey(0x0400);

        Assert.Equal(ModifierKey.Alt, hotKey.ModifierKeys);
    }

    [Fact]
    public void SetModifierKeys()
    {
        var hotKey = new HotKey()
        {
            ModifierKeys = ModifierKey.Control | ModifierKey.Shift
        };

        Assert.Equal(ModifierKey.Control | ModifierKey.Shift, hotKey.ModifierKeys);
    }
}
