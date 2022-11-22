using System;
using System.Globalization;

namespace ShortcutFiles;

/// <summary>
/// Represents the location of the icon for a shortcut.
/// </summary>
public sealed class IconLocation
{
    /// <summary>
    /// Gets the default location of the icon.
    /// </summary>
    public static readonly IconLocation Default = new(0);

    // TODO migth add setter...
    /// <summary>
    /// Gets the path or empty string of the <see cref="IconLocation"/>.
    /// </summary>
    public string Name => _name;

    // TODO migth add setter...
    /// <summary>
    /// Gets the index number of the icon in the file.
    /// </summary>
    public int Index => _index;

    private readonly string _name;
    private readonly int _index;

    /// <summary>
    /// Initializes a new instance of the <see cref="IconLocation"/> class with the specified index number.
    /// </summary>
    /// <param name="index">An index number to specify the icon in the file.</param>
    public IconLocation(int index)
    {
        _name = string.Empty;
        _index = index;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="IconLocation"/> class with the specified path and index number.
    /// </summary>
    /// <param name="path">A path for the file that contains icons.</param>
    /// <param name="index">An index number to specify the icon in the <paramref name="path"/>.</param>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="path"/> is <see langword="null"/>.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// <paramref name="path"/> is empty.
    /// </exception>
    public IconLocation(string path, int index)
    {
        ThrowHelper.ThrowIfNullOrEmpty(path);

        // TODO migth validate the specified path...
        _name = path;
        _index = index;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="IconLocation"/> class with the specified path and index number.
    /// </summary>
    /// <param name="path">A path for the file that contains icons.</param>
    /// <param name="index">An index number to specify the icon in the <paramref name="path"/>.</param>
    /// <exception cref="ArgumentException">
    /// <paramref name="path"/> is empty.
    /// </exception>
    public IconLocation(ReadOnlySpan<char> path, int index) : this(path.ToString(), index)
    {
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="str"></param>
    /// <returns>A <see cref="IconLocation"/> instance with the <paramref name="str"/>.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="str"/> is <see langword="null"/>
    /// </exception>
    /// <exception cref="ArgumentException">
    /// <paramref name="str"/> is empty.
    /// </exception>
    /// <exception cref="FormatException">
    /// <paramref name="str"/> is in incorrect format.
    /// </exception>
    public static IconLocation Parse(string str)
    {
        ThrowHelper.ThrowIfNullOrEmpty(str);

        ReadOnlySpan<char> span = str.AsSpan();
        int comma = span.LastIndexOf(',');

#if NETCOREAPP2_1_OR_GREATER
        if (comma == -1 ||
            !int.TryParse(span.Slice(comma + 1), NumberStyles.Integer, NumberFormatInfo.InvariantInfo, out int index))
#else
        if (comma == -1 ||
            !int.TryParse(
                span.Slice(comma + 1).ToString(), NumberStyles.Integer, NumberFormatInfo.InvariantInfo, out int index))
#endif
        {
            throw new FormatException($"The specified value '{str}' is in incorrect format.");
        }

        return comma == 0 ? new(index) : new(span.Slice(0, comma), index);
    }

    /// <summary>
    /// Returns a string representation of the <see cref="IconLocation"/>.
    /// </summary>
    /// <returns>A string representing the location of the icon.</returns>
    public override string ToString() => $"{_name},{_index}";
}
