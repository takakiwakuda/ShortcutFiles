using System;
using System.IO;
using Xunit;

namespace ShortcutFiles.Tests;

public class ShortcutFileCreateTests
{
    [Fact]
    public void CreatePathIsNull()
    {
        Assert.Throws<ArgumentNullException>(() => ShortcutFile.Create(null, TargetPath));
    }

    [Fact]
    public void CreatePathIsEmpty()
    {
        Assert.Throws<ArgumentException>(() => ShortcutFile.Create(string.Empty, TargetPath));
    }

    [Fact]
    public void CreatePathExists()
    {
        string path = TestHelpers.GetTempFileName();
        try
        {
            File.Create(path).Dispose();
            Assert.Throws<IOException>(() => ShortcutFile.Create(path, TargetPath));
        }
        finally
        {
            TestHelpers.DeleteFileIfExists(path);
        }
    }

    [Fact]
    public void CreateTargetPathIsNull()
    {
        Assert.Throws<ArgumentNullException>(() => ShortcutFile.Create(TestHelpers.GetTempFileName(), null));
    }

    [Fact]
    public void CreateTargetPathIsEmpty()
    {
        Assert.Throws<ArgumentException>(() => ShortcutFile.Create(TestHelpers.GetTempFileName(), string.Empty));
    }

    [Fact]
    public void Create()
    {
        string path = TestHelpers.GetTempFileName();
        try
        {
            using var shortcut = ShortcutFile.Create(path, TargetPath);

            Assert.Equal(path, shortcut.Name, ignoreCase: true);
            Assert.Equal(TargetPath, shortcut.TargetPath, ignoreCase: true);
        }
        finally
        {
            TestHelpers.DeleteFileIfExists(path);
        }
    }

    private const string TargetPath = @"C:\Windows\System32\cmd.exe";
}
