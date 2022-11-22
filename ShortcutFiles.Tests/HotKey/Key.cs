using Xunit;

namespace ShortcutFiles.Tests;

public class HotKeyKeyTests
{
    [Fact]
    public void GetKey()
    {
        var hotKey = new HotKey(0x0041);

        Assert.Equal(VirtualKey.A, hotKey.Key);
    }

    [Fact]
    public void SetKey()
    {
        var hotKey = new HotKey()
        {
            Key = VirtualKey.Z
        };

        Assert.Equal(VirtualKey.Z, hotKey.Key);
    }
}
