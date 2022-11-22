using System;
using System.Runtime.CompilerServices;
using System.Text;

namespace ShortcutFiles;

/// <summary>
/// Represents the hot key for a shortcut.
/// </summary>
public sealed class HotKey
{
    /// <summary>
    /// Returns an instance of the <see cref="HotKey"/> with no keys specified.
    /// </summary>
    public static readonly HotKey None = new();

    /// <summary>
    /// Gets the virtual key of the <see cref="HotKey"/>.
    /// </summary>
    public VirtualKey Key
    {
        get
        {
            return _virtualKey;
        }
        set
        {
            SetRawValue(value, _modifierKeys);
            _virtualKey = value;
        }
    }

    /// <summary>
    /// Gets modifier keys of the <see cref="HotKey"/>.
    /// </summary>
    public ModifierKey ModifierKeys
    {
        get
        {
            return _modifierKeys;
        }
        set
        {
            SetRawValue(_virtualKey, value);
            _modifierKeys = value;
        }
    }

    internal ushort RawValue => _rawValue;

    private VirtualKey _virtualKey;
    private ModifierKey _modifierKeys;
    private ushort _rawValue;

    /// <summary>
    /// Initializes a new instance of the <see cref="HotKey"/> class.
    /// </summary>
    public HotKey()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="HotKey"/> class from a hot key with the specified value.
    /// </summary>
    /// <param name="hotKey">A value that indicates a hot key.</param>
    /// <exception cref="ArgumentOutOfRangeException">
    /// The specified value for <paramref name="hotKey"/> is invalid range.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// The specified value for <paramref name="hotKey"/> is invalid as a virtual or modifier keys.
    /// </exception>
    public HotKey(int hotKey)
    {
        if (hotKey < ushort.MinValue || hotKey > ushort.MaxValue)
        {
            string message = $"The specified value must be between {ushort.MinValue} and {ushort.MaxValue}.";
            throw new ArgumentOutOfRangeException(nameof(hotKey), hotKey, message);
        }

        SetKeys(hotKey);
        _rawValue = (ushort)hotKey;
    }

    /// <summary>
    /// Returns a string representation of the <see cref="HotKey"/>.
    /// </summary>
    /// <returns>a string representation of the hot key.</returns>
    public override string ToString()
    {
        if (_rawValue == 0)
        {
            return "None";
        }

        var str = new StringBuilder();

        if (_modifierKeys != ModifierKey.None)
        {
            str.Append(_modifierKeys);
            str.Replace(", ", " + ");
        }

        if (_virtualKey != VirtualKey.None)
        {
            if (str.Length > 0)
            {
                str.Append(" + ");
            }
            str.Append(_virtualKey);
        }

        return str.ToString();
    }

    /// <summary>
    /// Sets a raw value to the result computed from the specified keys.
    /// </summary>
    /// <param name="vKey">The virtual key.</param>
    /// <param name="mKey">The modifier keys.</param>
    private void SetRawValue(VirtualKey vKey, ModifierKey mKey)
    {
        int lowByte = (int)vKey;
        int highByte = mKey == ModifierKey.None ? 0 : (int)mKey << 8;
        _rawValue = (ushort)(highByte | lowByte);
    }

    /// <summary>
    /// Sets a virtual key and modifier key to results computed from the specified value.
    /// </summary>
    /// <param name="value">The value to set as keys.</param>
    /// <param name="paramName">The name of the parameter with which <paramref name="value"/> corresponds.</param>
    /// <exception cref="ArgumentException">
    /// <paramref name="value"/> is invalid.
    /// </exception>
    private void SetKeys(int value, [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        if (value == 0)
        {
            _virtualKey = VirtualKey.None;
            _modifierKeys = ModifierKey.None;
            return;
        }

        int lowByte = value & 0x00FF;
        if (!Enum.IsDefined(typeof(VirtualKey), lowByte))
        {
            string message = $"The low-order byte of the value {value} is an invalid value as a virtual key.";
            throw new ArgumentException(message, paramName);
        }

        int highByte = (value & 0xFF00) >> 8;
        if (highByte > 0xF)
        {
            string message = $"The high-order byte of the value {value} is an invalid value as modifier keys.";
            throw new ArgumentException(message, paramName);
        }

        _virtualKey = (VirtualKey)lowByte;
        _modifierKeys = (ModifierKey)highByte;
    }
}
