using System;
using System.IO;
using Xunit;

namespace ShortcutFiles.Tests;

public class ShortcutFileSaveTests
{
    [Fact]
    public void SaveAsDisposedObject()
    {
        var shortcut = TestHelpers.OpenTestShortcutFile();
        shortcut.Dispose();

        void f() => shortcut.Save();

        Assert.Throws<ObjectDisposedException>(f);
    }

    [Fact]
    public void Save()
    {
        string path = TestHelpers.GetTempFileName();
        File.Copy(TestHelpers.GetTestFileName(), path);
        try
        {
            using var shortcut = ShortcutFile.Open(path);
            shortcut.TargetPath = @"C:\Windows\SysWOW64\notepad.exe";
            shortcut.Save();

            Assert.False(shortcut.Changed);
            Assert.Equal(@"C:\Windows\SysWOW64\notepad.exe", shortcut.TargetPath, ignoreCase: true);
        }
        finally
        {
            TestHelpers.DeleteFileIfExists(path);
        }
    }
}
