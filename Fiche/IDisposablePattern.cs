using System;

namespace Fiche
{
    /// <summary>
    /// An interface for the <see cref="IDisposable"/> pattern suggested by Microsoft.
    /// </summary>
    public interface IDisposablePattern : IDisposable
    {
        bool Disposed { get; }
        void Dispose(bool disposing);
    }
}
