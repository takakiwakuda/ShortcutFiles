namespace ShortcutFiles;

/// <summary>
/// Specifies how a window will appear when the shortcut is executed.
/// </summary>
public enum ShortcutWindowStyle
{
    /// <summary>
    /// Activates and displays a window.
    /// If the window is minimized or maximized, the system restores it to its original size and position.
    /// </summary>
    Normal = 1,

    /// <summary>
    /// Activates the window and displays it as a maximized window.
    /// </summary>
    Maximized = 3,

    /// <summary>
    /// Displays the window in its minimized state, leaving the currently active window as active.
    /// </summary>
    Minimized = 7
}
