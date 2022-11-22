using System;
using System.Runtime.InteropServices;

namespace ShortcutFiles;

/// <summary>
/// Provides classes and structures for COM interoperability.
/// </summary>
internal static class Interop
{
    internal const int INFOTIPSIZE = 1024;
    internal const int MAX_PATH = 260;

    [Flags]
    internal enum SLGP_FLAGS : uint
    {
        SLGP_SHORTPATH = 0x1,
        SLGP_UNCPRIORITY = 0x2,
        SLGP_RAWPATH = 0x4,
        SLGP_RELATIVEPRIORITY = 0x8
    }

    [Flags]
    internal enum SLR_FLAGS : uint
    {
        SLR_NONE = 0,
        SLR_NO_UI = 0x0001,
        SLR_ANY_MATCH = 0x0002,
        SLR_UPDATE = 0x0004,
        SLR_NOUPDATE = 0x0008,
        SLR_NOSEARCH = 0x0010,
        SLR_NOTRACK = 0x0020,
        SLR_NOLINKINFO = 0x0040,
        SLR_INVOKE_MSI = 0x0080,
        SLR_NO_UI_WITH_MSG_PUMP = 0x0101,
        SLR_OFFER_DELETE_WITHOUT_FILE = 0x0200,
        SLR_KNOWNFOLDER = 0x0400,
        SLR_MACHINE_IN_LOCAL_TARGET = 0x0800,
        SLR_UPDATE_MACHINE_AND_SID = 0x1000,
        SLR_NO_OBJECT_ID = 0x2000
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct FILETIME
    {
        internal uint dwLowDateTime;
        internal uint dwHighDateTime;
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct ITEMIDLIST
    {
        internal SHITEMID mkid;
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct SHITEMID
    {
        internal ushort cb;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1)]
        internal byte[] abID;
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    internal struct WIN32_FIND_DATA
    {
        internal uint dwFileAttributes;
        internal FILETIME ftCreationTime;
        internal FILETIME ftLastAccessTime;
        internal FILETIME ftLastWriteTime;
        internal uint nFileSizeHigh;
        internal uint nFileSizeLow;
        internal uint dwReserved0;
        internal uint dwReserved1;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MAX_PATH)]
        internal string cFileName;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 14)]
        internal string cAlternateFileName;
    }

    // https://learn.microsoft.com/en-us/windows/win32/api/objidl/nn-objidl-ipersistfile
    [ComImport]
    [Guid("0000010b-0000-0000-C000-000000000046")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    internal interface IPersistFile
    {
        void GetClassID(out Guid pClassID);
        [PreserveSig]
        int IsDirty();
        void Load([MarshalAs(UnmanagedType.LPWStr)] string pszFileName, uint dwMode);
        void Save([MarshalAs(UnmanagedType.LPWStr)] string? pszFileName, [MarshalAs(UnmanagedType.Bool)] bool fRemember);
        void SaveCompleted([MarshalAs(UnmanagedType.LPWStr)] string pszFileName);
        void GetCurFile([MarshalAs(UnmanagedType.LPWStr)] out string ppszFileName);
    }

    // https://learn.microsoft.com/en-us/windows/win32/api/shobjidl_core/nn-shobjidl_core-ishelllinkw
    [ComImport]
    [Guid("000214F9-0000-0000-C000-000000000046")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [CoClass(typeof(ShellLinkClass))]
    internal interface IShellLink
    {
        void GetPath(ref char pszFile, int cch, out WIN32_FIND_DATA pfd, SLGP_FLAGS fFlags);
        void GetIDList(out nint ppidl);
        void SetIDList(ITEMIDLIST pidl);
        void GetDescription(ref char pszName, int cch);
        void SetDescription([MarshalAs(UnmanagedType.LPWStr)] string pszName);
        void GetWorkingDirectory(ref char pszDir, int cch);
        void SetWorkingDirectory([MarshalAs(UnmanagedType.LPWStr)] string pszDir);
        void GetArguments(ref char pszArgs, int cch);
        void SetArguments([MarshalAs(UnmanagedType.LPWStr)] string pszArgs);
        void GetHotkey(out ushort pwHotkey);
        void SetHotkey(ushort wHotkey);
        void GetShowCmd(out int piShowCmd);
        void SetShowCmd(int iShowCmd);
        void GetIconLocation(ref char pszIconPath, int cch, out int piIcon);
        void SetIconLocation([MarshalAs(UnmanagedType.LPWStr)] string pszIconPath, int iIcon);
        void SetRelativePath([MarshalAs(UnmanagedType.LPWStr)] string pszPathRel, uint dwReserved);
        void Resolve(nint hwnd, SLR_FLAGS fFlags);
        void SetPath([MarshalAs(UnmanagedType.LPWStr)] string pszFile);
    }

    [ComImport]
    [Guid("00021401-0000-0000-C000-000000000046")]
    internal class ShellLinkClass
    {
    }
}
