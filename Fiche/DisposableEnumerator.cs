using System;
using System.Collections;

namespace Fiche
{
    /// <summary>
    /// Allows using <see cref="IEnumerator"/> in a <see langword="using"/> clause since <see cref="IEnumerator"/> does not implement <see cref="IDisposable"/> by default.
    /// </summary>
    public struct DisposableEnumerator : IDisposable, IEnumerator
    {
        private readonly IEnumerator enumerator;
        public DisposableEnumerator(IEnumerator enumerator)
            => this.enumerator = enumerator;

        /// <summary>
        /// Returns <see cref="IEnumerator.Current"/> of the wrapped <see cref="IEnumerator"/>.
        /// </summary>
        public object Current => this.enumerator.Current;

        /// <summary>
        /// Calls <see cref="IDisposable.Dispose"/> for the wrapped <see cref="IEnumerator"/> if it implements <see cref="IDisposable"/>.
        /// </summary>
        public void Dispose()
        {
            if (this.enumerator is IDisposable disposable)
                disposable.Dispose();
        }

        /// <summary>
        /// Calls <see cref="IEnumerator.MoveNext"/> of the wrapped <see cref="IEnumerator"/>.
        /// </summary>
        /// <exception cref="InvalidOperationException"/>
        public bool MoveNext() => this.enumerator.MoveNext();

        /// <summary>
        /// Calls <see cref="IEnumerator.MoveNext"/> of the wrapped <see cref="IEnumerator"/>.
        /// </summary>
        /// <exception cref="InvalidOperationException"/>
        public void Reset() => this.enumerator.Reset();
    }
}
