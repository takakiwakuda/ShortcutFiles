using System;
using System.IO;

namespace ShortcutFiles.Tests;

public static class TestHelpers
{
    private const string TestFileName = "TestFile.lnk";

    public static void DeleteFileIfExists(string path)
    {
        if (File.Exists(path))
        {
            File.Delete(path);
        }
    }

    public static ModifierKey GetModifierKey(int value)
    {
        return (ModifierKey)(value == 0 ? 0 : (value & 0xFF00) >> 8);
    }

    public static string GetTempFileName()
    {
        string temp = Path.GetTempFileName();
        DeleteFileIfExists(temp);
        return temp.Replace(".tmp", ".lnk");
    }

    public static string GetTestFileName()
    {
        return Environment.CurrentDirectory + '\\' + TestFileName;
    }

    public static VirtualKey GetVirtualKey(int value)
    {
        return (VirtualKey)(value == 0 ? 0 : value & 0x00FF);
    }

    public static ShortcutFile OpenTestShortcutFile()
    {
        return ShortcutFile.Open(GetTestFileName());
    }
}
