using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace ShortcutFiles;

/// <summary>
/// Provides helper methods to throw exceptions.
/// </summary>
internal static class ThrowHelper
{
    /// <summary>
    /// Throws an <see cref="ArgumentException"/>.
    /// </summary>
    /// <param name="message">The error message that explains the reason for the exception.</param>
    /// <param name="paramName">The name of the parameter that caused the current exception.</param>
    /// <exception cref="ArgumentException"/>
    [DoesNotReturn]
    public static void ThrowArgumentException(string? message, string? paramName)
    {
        throw new ArgumentException(message, paramName);
    }

    /// <summary>
    /// Throws an <see cref="ArgumentNullException"/>.
    /// </summary>
    /// <param name="paramName">The name of the parameter that caused the exception.</param>
    /// <exception cref="ArgumentNullException"/>
    [DoesNotReturn]
    public static void ThrowArgumentNullException(string? paramName)
    {
        throw new ArgumentNullException(paramName);
    }

    /// <summary>
    /// Throws an exception if <paramref name="argument"/> is <see langword="null"/> or empty.
    /// </summary>
    /// <param name="argument">
    /// The string argument to validate as non-<see langword="null"/> and non-empty.
    /// </param>
    /// <param name="paramName">
    /// The name of the parameter with which <paramref name="argument"/> corresponds.
    /// </param>
    /// <exception cref="ArgumentException">
    /// <paramref name="argument"/> is <see langword="null"/>.
    /// </exception>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="argument"/> is empty.
    /// </exception>
    public static void ThrowIfNullOrEmpty(
        [NotNull] string? argument, [CallerArgumentExpression(nameof(argument))] string? paramName = null)
    {
#if NET7_0_OR_GREATER
        ArgumentException.ThrowIfNullOrEmpty(argument, paramName);
#else
        ThrowIfNull(argument, paramName);

        if (argument.Length == 0)
        {
            ThrowArgumentException("The value cannot be an empty string.", paramName);
        }
#endif
    }

    /// <summary>
    /// Throws an <see cref="ArgumentNullException"/> if <paramref name="argument"/> is <see langword="null"/>.
    /// </summary>
    /// <param name="argument">
    /// The reference type argument to validate as non-<see langword="null"/>.
    /// </param>
    /// <param name="paramName">
    /// The name of the parameter with which <paramref name="argument"/> corresponds.
    /// If you omit this parameter, the name of <paramref name="argument"/> is used.
    /// </param>
    /// <exception cref="ArgumentException">
    /// <paramref name="argument"/> is <see langword="null"/>.
    /// </exception>
    public static void ThrowIfNull(
        [NotNull] object? argument, [CallerArgumentExpression(nameof(argument))] string? paramName = null)
    {
#if NET6_0_OR_GREATER
        ArgumentNullException.ThrowIfNull(argument, paramName);
#else
        if (argument is null)
        {
            ThrowArgumentNullException(paramName);
        }
#endif
    }
}
