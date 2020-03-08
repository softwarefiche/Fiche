using System.Collections;
using System.Collections.Generic;

using Fiche.Extensions;

namespace Fiche.Tests.Stubs
{
#pragma warning disable CA1063 // Implement IDisposable Correctly
    public class DisposedEnumerator<T> : IEnumerator<T>, IDisposablePattern
#pragma warning restore CA1063 // Implement IDisposable Correctly
    {
        private bool disposed;
        public T Current => default;

        object IEnumerator.Current => default(T);

        bool IDisposablePattern.Disposed => this.disposed;

#pragma warning disable CA1063 // Implement IDisposable Correctly
        public void Dispose() => this.disposed = true;
#pragma warning restore CA1063 // Implement IDisposable Correctly

        public bool MoveNext()
        {
            this.AssertNotDisposed();
            return default;
        }
        public void Reset() => this.AssertNotDisposed();
        void IDisposablePattern.Dispose(bool disposing) => this.AssertNotDisposed();
    }
}
