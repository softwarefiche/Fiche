using System;
using System.Collections;

namespace Fiche.Collections
{

    /// <summary>
    /// Represents a sorted sequence. This is a non-generic version of <see cref="System.Linq.IOrderedEnumerable{TElement}"/>.
    /// </summary>
    public interface IOrderedEnumerable : IEnumerable
    {
        IOrderedEnumerable CreateOrderedEnumerable<TKey>(Func<object, TKey> keySelector, IComparer comparer, bool descending);
    }
}
