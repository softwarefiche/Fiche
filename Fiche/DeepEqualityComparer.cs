using System.Collections;
using System.Collections.Generic;

using Fiche.Extensions;

namespace Fiche
{
    /// <summary>
    /// Implements a generic equality comparer that uses <see cref="ObjectExtensions.DeepEquals{T}(T, T)"/> to compare.
    /// </summary>
    public class DeepEqualityComparer<T> : IEqualityComparer<T>, IEqualityComparer
    {
        static DeepEqualityComparer() => Default = new DeepEqualityComparer<T>();
        public static DeepEqualityComparer<T> Default { get; }
        public bool Equals(T x, T y) => x.DeepEquals(y);

        bool IEqualityComparer.Equals(object x, object y) => Equals((T)x, (T)y);

        public int GetHashCode(T obj) => obj?.GetHashCode() ?? default;

        int IEqualityComparer.GetHashCode(object obj) => obj?.GetHashCode() ?? default;
    }
    /// <summary>
    /// Implements an object equality comparer that uses <see cref="ObjectExtensions.DeepEquals{T}(T, T)"/> to compare.
    /// </summary>
    public class DeepEqualityComparer : IEqualityComparer<object>, IEqualityComparer
    {
        static DeepEqualityComparer() => Default = new DeepEqualityComparer();
        public static DeepEqualityComparer Default { get; }
        public new bool Equals(object x, object y) => x.DeepEquals(y);
        public int GetHashCode(object obj) => obj?.GetHashCode() ?? default;
    }
}
