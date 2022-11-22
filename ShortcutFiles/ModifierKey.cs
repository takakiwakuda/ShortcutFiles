using System;

namespace ShortcutFiles;

/// <summary>
/// Specifies a modifier key.
/// </summary>
/// <remarks>
/// Defined in the CommCtrl.h
/// </remarks>
[Flags]
public enum ModifierKey
{
    /// <summary>
    /// Not specified
    /// </summary>
    None = 0x00,

    /// <summary>
    /// SHIFT key
    /// </summary>
    Shift = 0x01,

    /// <summary>
    /// CTRL key
    /// </summary>
    Control = 0x02,

    /// <summary>
    /// ALT key
    /// </summary>
    Alt = 0x04,

    /// <summary>
    /// Extended key
    /// </summary>
    Extended = 0x08
}
