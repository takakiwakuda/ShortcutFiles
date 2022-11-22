using System;
using Xunit;

namespace ShortcutFiles.Tests;

public class IconLocationCtorTests
{
    [Fact]
    public void CtorNull()
    {
        string? path = null;

        Assert.Throws<ArgumentNullException>(() => new IconLocation(path, 0));
    }

    [Fact]
    public void CtorEmptyString()
    {
        Assert.Throws<ArgumentException>(() => new IconLocation(string.Empty, 0));
    }

    [Fact]
    public void CtorEmptySpan()
    {
        Assert.Throws<ArgumentException>(() => new IconLocation(ReadOnlySpan<char>.Empty, 0));
    }

    [Fact]
    public void CtorIndex()
    {
        int index = 0;
        var iconLocation = new IconLocation(index);

        Assert.Equal(string.Empty, iconLocation.Name);
        Assert.Equal(index, iconLocation.Index);
    }

    [Theory]
    [InlineData(@"C:\Windows\System32\shell32.dll", 0)]
    [InlineData(@"%windir%\System32\shell32.dll", 0)]
    public void CtorPathStringAndIndex(string path, int index)
    {
        var iconLocation = new IconLocation(path, index);

        Assert.Equal(path, iconLocation.Name);
        Assert.Equal(index, iconLocation.Index);
    }

    [Theory]
    [InlineData(@"C:\Windows\System32\shell32.dll", 0)]
    [InlineData(@"%windir%\System32\shell32.dll", 0)]
    public void CtorPathSpanAndIndex(string path, int index)
    {
        var span = path.AsSpan();
        var iconLocation = new IconLocation(span, index);

        Assert.True(span.SequenceEqual(iconLocation.Name.AsSpan()));
        Assert.Equal(index, iconLocation.Index);
    }
}
