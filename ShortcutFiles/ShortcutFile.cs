using System;
using System.ComponentModel;
using System.IO;
using System.Runtime.InteropServices;

namespace ShortcutFiles;

/// <summary>
/// Provides access to a shortcut file via Component Object Module (COM).
/// </summary>
public sealed class ShortcutFile : IDisposable
{
    /// <summary>
    /// Gets the path for the <see cref="ShortcutFile"/>.
    /// </summary>
    /// <exception cref="ObjectDisposedException">
    /// The shortcut object was disposed.
    /// </exception>
    public string Name
    {
        get
        {
            ThrowIfDisposed();

            PersistFile.GetCurFile(out string name);
            return name;
        }
    }

    /// <summary>
    /// Gets or sets the path of the target to the <see cref="ShortcutFile"/>.
    /// </summary>
    /// <exception cref="ArgumentException">
    /// The specified value is empty.
    /// </exception>
    /// <exception cref="ArgumentNullException">
    /// The specified value is <see langword="null"/>.
    /// </exception>
    /// <exception cref="ObjectDisposedException">
    /// The shortcut object was disposed.
    /// </exception>
    /// <exception cref="COMException">
    /// An error occurred while setting the target.
    /// </exception>
    public string TargetPath
    {
        get
        {
            ThrowIfDisposed();

            Span<char> buffer = stackalloc char[Interop.MAX_PATH];
            ShellLink.GetPath(ref MemoryMarshal.GetReference(buffer), buffer.Length, out _, Interop.SLGP_FLAGS.SLGP_RAWPATH);
            return buffer.Slice(0, buffer.IndexOf('\0')).ToString();
        }
        set
        {
            ThrowIfDisposed();
            ThrowHelper.ThrowIfNullOrEmpty(value);

            // TODO migth validate the specified path...
            ShellLink.SetPath(value);
        }
    }

    /// <summary>
    /// Gets or sets the description of the <see cref="ShortcutFile"/>.
    /// </summary>
    /// <exception cref="ArgumentNullException">
    /// The specified value is <see langword="null"/>.
    /// </exception>
    /// <exception cref="ObjectDisposedException">
    /// The shortcut object was disposed.
    /// </exception>
    public string Description
    {
        get
        {
            ThrowIfDisposed();

            Span<char> buffer = stackalloc char[Interop.INFOTIPSIZE];
            ShellLink.GetDescription(ref MemoryMarshal.GetReference(buffer), buffer.Length);
            return buffer.Slice(0, buffer.IndexOf('\0')).ToString();
        }
        set
        {
            ThrowIfDisposed();
            ThrowHelper.ThrowIfNull(value);

            ShellLink.SetDescription(value);
        }
    }

    /// <summary>
    /// Gets or sets the working directory to run the <see cref="ShortcutFile"/>.
    /// </summary>
    /// <exception cref="ArgumentNullException">
    /// The specified value is <see langword="null"/>.
    /// </exception>
    /// <exception cref="ObjectDisposedException">
    /// The shortcut object was disposed.
    /// </exception>
    public string WorkingDirectory
    {
        get
        {
            ThrowIfDisposed();

            Span<char> buffer = stackalloc char[Interop.MAX_PATH];
            ShellLink.GetWorkingDirectory(ref MemoryMarshal.GetReference(buffer), buffer.Length);
            return buffer.Slice(0, buffer.IndexOf('\0')).ToString();
        }
        set
        {
            ThrowIfDisposed();
            ThrowHelper.ThrowIfNull(value);

            // TODO migth validate the specified path...
            ShellLink.SetWorkingDirectory(value);
        }
    }

    /// <summary>
    /// Gets or sets a string that represents arguments for the <see cref="ShortcutFile"/>.
    /// </summary>
    /// <exception cref="ArgumentNullException">
    /// The specified value is <see langword="null"/>.
    /// </exception>
    /// <exception cref="ObjectDisposedException">
    /// The shortcut object was disposed.
    /// </exception>
    public string Arguments
    {
        get
        {
            ThrowIfDisposed();

            Span<char> buffer = stackalloc char[Interop.INFOTIPSIZE];
            ShellLink.GetArguments(ref MemoryMarshal.GetReference(buffer), buffer.Length);
            return buffer.Slice(0, buffer.IndexOf('\0')).ToString();
        }
        set
        {
            ThrowIfDisposed();
            ThrowHelper.ThrowIfNull(value);

            ShellLink.SetArguments(value);
        }
    }

    /// <summary>
    /// Gets or sets a value that represents the shortcut-key to run the <see cref="ShortcutFile"/>.
    /// </summary>
    /// <exception cref="ObjectDisposedException">
    /// The shortcut object was disposed.
    /// </exception>
    public HotKey HotKey
    {
        get
        {
            ThrowIfDisposed();

            ShellLink.GetHotkey(out ushort hotkey);
            return new(hotkey);
        }
        set
        {
            ThrowIfDisposed();

            ShellLink.SetHotkey(value.RawValue);
        }
    }

    /// <summary>
    /// Gets or sets the window style when the <see cref="ShortcutFile"/> is executed.
    /// </summary>
    /// <exception cref="InvalidEnumArgumentException">
    /// The specified value is not one of the <see cref="ShortcutWindowStyle"/> members.
    /// </exception>
    /// <exception cref="ObjectDisposedException">
    /// The shortcut object was disposed.
    /// </exception>
    public ShortcutWindowStyle WindowStyle
    {
        get
        {
            ThrowIfDisposed();

            ShellLink.GetShowCmd(out int windowStyle);
            return (ShortcutWindowStyle)windowStyle;
        }
        set
        {
            ThrowIfDisposed();
            if (!Enum.IsDefined(typeof(ShortcutWindowStyle), value))
            {
                throw new InvalidEnumArgumentException(nameof(value), (int)value, typeof(ShortcutWindowStyle));
            }

            ShellLink.SetShowCmd((int)value);
        }
    }

    /// <summary>
    /// Gets or sets the location of the icon for the <see cref="ShortcutFile"/>.
    /// </summary>
    /// <exception cref="ObjectDisposedException">
    /// The shortcut object was disposed.
    /// </exception>
    public IconLocation IconLocation
    {
        get
        {
            ThrowIfDisposed();

            Span<char> buffer = stackalloc char[Interop.MAX_PATH];
            ShellLink.GetIconLocation(ref MemoryMarshal.GetReference(buffer), buffer.Length, out int index);

            int i = buffer.IndexOf('\0');
            return i == 0 ? new(index) : new(buffer.Slice(0, i), index);
        }
        set
        {
            ThrowIfDisposed();

            ShellLink.SetIconLocation(value.Name, value.Index);
        }
    }

    /// <summary>
    /// Gets a value that whether the shortcut has changed since it was last saved to file.
    /// </summary>
    /// <exception cref="ObjectDisposedException">
    /// The shortcut object was disposed.
    /// </exception>
    public bool Changed
    {
        get
        {
            ThrowIfDisposed();

            return PersistFile.IsDirty() == 0;
        }
    }

    private Interop.IShellLink ShellLink => _shellLink.Target;
    private Interop.IPersistFile PersistFile => _persistFile.Target;
    private ComHandle<Interop.IShellLink> _shellLink;
    private ComHandle<Interop.IPersistFile> _persistFile;
    private bool _disposed;

    /// <summary>
    /// Initializes a new instance of the <see cref="ShortcutFile"/> class.
    /// </summary>
    private ShortcutFile()
    {
        _shellLink = new ComHandle<Interop.IShellLink>(new());
        _persistFile = new ComHandle<Interop.IPersistFile>((Interop.IPersistFile)ShellLink);
    }

    /// <summary>
    /// Creates a new <see cref="ShortcutFile"/> to the specified path that links the specified target path.
    /// </summary>
    /// <param name="path">The file to create.</param>
    /// <param name="targetPath">The file or directory to link the <paramref name="path"/>.</param>
    /// <returns>A <see cref="ShortcutFile"/> created at the specified path.</returns>
    /// <exception cref="ArgumentException">
    /// <paramref name="path"/> or <paramref name="targetPath"/> is empty.
    /// </exception>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="path"/> or <paramref name="targetPath"/> is <see langword="null"/>.
    /// </exception>
    /// <exception cref="UnauthorizedAccessException">
    /// <paramref name="path"/> specified a directory.
    /// </exception>
    /// <exception cref="DirectoryNotFoundException">
    /// The specified path is invalid.
    /// </exception>
    /// <exception cref="IOException">
    /// The file already exists.
    /// </exception>
    /// <exception cref="COMException">
    /// An error occurred while creating the file.
    /// </exception>
    public static ShortcutFile Create(string path, string targetPath)
    {
        ThrowHelper.ThrowIfNullOrEmpty(path);
        ThrowHelper.ThrowIfNullOrEmpty(targetPath);

        // TODO migth normalize the specified paths...
        if (File.Exists(path))
        {
            throw new IOException($"The file '{path}' already exists.");
        }

        ShortcutFile shortcut = new();
        try
        {
            shortcut.TargetPath = targetPath;
            shortcut.PersistFile.Save(path, true);
        }
        catch (Exception)
        {
            shortcut.Dispose();
            throw;
        }

        return shortcut;
    }

    /// <summary>
    /// Opens a <see cref="ShortcutFile"/> at the specified path.
    /// </summary>
    /// <param name="path">The file to open.</param>
    /// <returns>A <see cref="ShortcutFile"/> opened for the specified path.</returns>
    /// <exception cref="ArgumentException">
    /// <paramref name="path"/> is empty.
    /// </exception>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="path"/> is <see langword="null"/>.
    /// </exception>
    /// <exception cref="UnauthorizedAccessException">
    /// <paramref name="path"/> specified a directory.
    /// </exception>
    /// <exception cref="DirectoryNotFoundException">
    /// The specified path is invalid.
    /// </exception>
    /// <exception cref="FileNotFoundException">
    /// The file cannot be found.
    /// </exception>
    /// <exception cref="PathTooLongException">
    /// The specified path exceed the system-defied maximum length.
    /// </exception>
    /// <exception cref="COMException">
    /// An error occurred while opening the file.
    /// </exception>
    public static ShortcutFile Open(string path)
    {
        ThrowHelper.ThrowIfNullOrEmpty(path);

        ShortcutFile shortcut = new();
        try
        {
            shortcut.PersistFile.Load(path, /* STGM_READ */ 0);
        }
        catch (Exception)
        {
            shortcut.Dispose();
            throw;
        }

        return shortcut;
    }

    /// <summary>
    /// Releases all resources used by the <see cref="ShortcutFile"/> class.
    /// </summary>
    public void Dispose()
    {
        if (_disposed)
        {
            return;
        }

        _persistFile.Dispose();
        _persistFile = null!;

        _shellLink.Dispose();
        _shellLink = null!;

        _disposed = true;
    }

    /// <summary>
    /// Saves the changes to the file.
    /// </summary>
    /// <remarks>
    /// <see cref="Save"/> be able to save a shortcut even in dangerous configurations.
    /// </remarks>
    /// <exception cref="ObjectDisposedException">
    /// The shortcut object was disposed.
    /// </exception>
    /// <exception cref="UnauthorizedAccessException">
    /// The file is raed-only.
    /// </exception>
    /// <exception cref="DirectoryNotFoundException">
    /// The specified path is invalid.
    /// </exception>
    public void Save()
    {
        ThrowIfDisposed();

        PersistFile.Save(null, true);
    }

    /// <summary>
    /// Saves the shortcut to the specified path.
    /// </summary>
    /// <remarks>
    /// <see cref="SaveAs"/> be able to save a shortcut even in dangerous configurations.
    /// </remarks>
    /// <param name="path">The file to write to.</param>
    /// <exception cref="ArgumentException">
    /// <paramref name="path"/> is empty.
    /// </exception>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="path"/> is <see langword="null"/>.
    /// </exception>
    /// <exception cref="ObjectDisposedException">
    /// The shortcut object was disposed.
    /// </exception>
    /// <exception cref="UnauthorizedAccessException">
    /// <paramref name="path"/> specified a directory.
    /// </exception>
    /// <exception cref="DirectoryNotFoundException">
    /// The specified path is invalid.
    /// </exception>
    /// <exception cref="IOException">
    /// The file already exists.
    /// </exception>
    public void SaveAs(string path)
    {
        ThrowIfDisposed();
        ThrowHelper.ThrowIfNullOrEmpty(path);

        // TODO migth normalize the specified path...

        if (File.Exists(path))
        {
            throw new IOException($"The file '{path}' already exists.");
        }

        PersistFile.Save(path, true);
    }

    /// <summary>
    /// Throws an <see cref="ObjectDisposedException"/> if the current object has already been disposed.
    /// </summary>
    /// <exception cref="ObjectDisposedException">
    /// The shortcut object was disposed.
    /// </exception>
    private void ThrowIfDisposed()
    {
        if (_disposed)
        {
            throw new ObjectDisposedException(nameof(ShortcutFile));
        }
    }
}
