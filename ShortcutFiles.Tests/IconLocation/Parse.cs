using System;
using Xunit;

namespace ShortcutFiles.Tests;

public class IconLocationParseTests
{
    [Fact]
    public void ParseNull()
    {
        string? str = null;

        Assert.Throws<ArgumentNullException>(() => IconLocation.Parse(str));
    }

    [Fact]
    public void ParseEmpty()
    {
        Assert.Throws<ArgumentException>(() => IconLocation.Parse(string.Empty));
    }

    [Theory]
    [InlineData(@"NoCommaExists")]
    [InlineData(@"C:\Windows\System32\shell32.dll,NotNumber")]
    public void ParseInvalidString(string str)
    {
        Assert.Throws<FormatException>(() => IconLocation.Parse(str));
    }

    [Theory]
    [InlineData(@"C:\Windows\System32\shell32.dll,0")]
    [InlineData(@"%windir%\System32\shell32.dll,0")]
    public void Parse(string str)
    {
        var iconLocation = IconLocation.Parse(str);

        Assert.Equal(str, iconLocation.ToString());
    }
}
