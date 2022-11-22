using System;
using System.IO;
using Xunit;

namespace ShortcutFiles.Tests;

public class ShortcutFileSaveAsTests
{
    [Fact]
    public void SaveAsDisposedObject()
    {
        var shortcut = TestHelpers.OpenTestShortcutFile();
        shortcut.Dispose();

        Assert.Throws<ObjectDisposedException>(() => shortcut.SaveAs(TestHelpers.GetTempFileName()));
    }

    [Fact]
    public void SaveAsNull()
    {
        using var shortcut = TestHelpers.OpenTestShortcutFile();
        Assert.Throws<ArgumentNullException>(() => shortcut.SaveAs(null));
    }

    [Fact]
    public void SaveAsEmpty()
    {
        using var shortcut = TestHelpers.OpenTestShortcutFile();
        Assert.Throws<ArgumentException>(() => shortcut.SaveAs(string.Empty));
    }

    [Fact]
    public void SaveAsExists()
    {
        using var shortcut = TestHelpers.OpenTestShortcutFile();
        Assert.Throws<IOException>(() => shortcut.SaveAs(TestHelpers.GetTestFileName()));
    }

    [Fact]
    public void SaveAs()
    {
        string path = TestHelpers.GetTempFileName();
        try
        {
            using var shortcut = TestHelpers.OpenTestShortcutFile();
            shortcut.SaveAs(path);

            Assert.Equal(path, shortcut.Name, ignoreCase: true);
        }
        finally
        {
            TestHelpers.DeleteFileIfExists(path);
        }
    }
}
