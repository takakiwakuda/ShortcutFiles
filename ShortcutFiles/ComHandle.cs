using System;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;

namespace ShortcutFiles;

internal class ComHandle<T> : CriticalFinalizerObject, IDisposable
{
    /// <summary>
    /// Gets a wrapped COM object.
    /// </summary>
    /// <exception cref="ObjectDisposedException">
    /// The object was disposed.
    /// </exception>
    public T Target
    {
        get
        {
            if (!_handle.IsAllocated || _handle.Target is null)
            {
                throw new ObjectDisposedException(nameof(ComHandle<T>));
            }

            return (T)_handle.Target;
        }
    }

    private GCHandle _handle;

    /// <summary>
    /// Initializes a new instance of the <see cref="ComHandle{T}"/> class that wraps the specified COM object.
    /// </summary>
    /// <param name="obj">A COM object to use.</param>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="obj"/> is <see langword="null"/>.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// <paramref name="obj"/> is not a COM object.
    /// </exception>
    public ComHandle(T obj)
    {
        ThrowHelper.ThrowIfNull(obj);

        if (!Marshal.IsComObject(obj))
        {
            throw new ArgumentException("The object must be a COM object.", nameof(obj));
        }

        _handle = GCHandle.Alloc(obj);
    }

    ~ComHandle()
    {
        Dispose(false);
    }

    /// <summary>
    /// Releases all resources used by the <see cref="ComHandle{T}"/> class.
    /// </summary>
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Releases the unmanaged resources used by the <see cref="ComHandle{T}"/> class and
    /// optionally releases the managed resources.
    /// </summary>
    /// <param name="disposing">
    /// <see langword="true"/> to release both managed and unmanaged resources;
    /// <see langword="false"/> to release only unmanaged resources.
    /// </param>
    protected virtual void Dispose(bool disposing)
    {
        if (_handle.IsAllocated)
        {
            if (_handle.Target is not null)
            {
                Marshal.FinalReleaseComObject(_handle.Target);

                if (disposing)
                {
                    _handle.Target = null;
                }
            }

            _handle.Free();
        }
    }
}
