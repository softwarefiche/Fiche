using System;
using System.Collections.Generic;
using System.Linq;

using Fiche.Misc;

namespace Fiche.Collections.Generic.Extensions
{
    public static class ICollectionExtensions
    {
        /// <summary>
        /// Modifies the <see cref="ICollection{T}"/> to insert the specified item at the specified index.
        /// </summary>
        /// <param name="collection">the collection to be modified</param>
        /// <param name="item">the item to be inserted</param>
        /// <exception cref="ArgumentNullException"/>
        /// <exception cref="InvalidOperationException"/>
        /// <exception cref="ArgumentOutOfRangeException"/>
        /// <exception cref="NotSupportedException"/>
        public static void CollectionInsert<T>(this ICollection<T> collection, int index, T item)
        {
            Error.ThrowIfNull(collection, nameof(collection));
            Error.ThrowInvalidOperation(collection.IsReadOnly, "Cannot insert into a read-only collection.");
            Error.ThrowIfOutOfRange(index, nameof(index), 0, collection.Count);
            if (index == collection.Count)
                collection.Add(item);
            else if (collection is IList<T> list)
                list.Insert(index, item);
            else
            {
                List<T> copy = collection.ToList();
                collection.Clear();
                int currentIndex = -1;
                using IEnumerator<T> enumerator = copy.GetEnumerator();
                while (enumerator.MoveNext())
                {
                    ++currentIndex;
                    if (currentIndex == index)
                        collection.Add(item);
                    collection.Add(enumerator.Current);
                }
            }
        }
        /// <summary>
        /// Synchronizes original collection with the updated collection. Updated collection sort order is likely (but not guaranteed) to be applied on the source collection; thus, it is applied only if the original collection implements <see cref="IList{T}"/>.
        /// </summary>
        /// <param name="collection">the collection to be modified</param>
        /// <param name="updated">an updated version of the collection to be used to modify current collection</param>
        /// <param name="comparer">the equality comparer to be used. if null, the default equality comparer for type <typeparamref name="T"/> will be used</param>
        /// <exception cref="ArgumentNullException"/>
        /// <exception cref="InvalidOperationException"/>
        /// <exception cref="ArgumentOutOfRangeException"/>
        /// <exception cref="NotSupportedException"/>
        public static void Update<T>(this ICollection<T> collection, IEnumerable<T> updated, IEqualityComparer<T> comparer)
        {
            Error.ThrowIfNull(collection, nameof(collection));
            Error.ThrowIfNull(updated, nameof(updated));
            Error.ThrowInvalidOperation(collection.IsReadOnly, "Cannot update a read-only collection.");
            if (comparer is null)
                comparer = EqualityComparer<T>.Default;
            foreach (T item in collection)
            {
                if (!updated.Contains(item, comparer))
                    collection.Remove(item);
            }
            IEnumerable<T> collectionEnumerable = collection as IEnumerable<T>;
            foreach (T item in updated)
            {
                if (!collectionEnumerable.Contains(item, comparer))
                {
                    if (collection is IList<T> list)
                        list.Insert(updated.EnumerableIndexOf(item, comparer), item);
                    else
                        collection.CollectionInsert(updated.EnumerableIndexOf(item, comparer), item);
                }
            }
        }
        /// <summary>
        /// Synchronizes original collection with the updated collection. Updated collection sort order is likely (but not guaranteed) to be applied on the source collection; thus, it is applied only if the original collection implements <see cref="IList{T}"/>.
        /// </summary>
        /// <param name="collection">the collection to be modified</param>
        /// <param name="updated">an updated version of the collection to be used to modify current collection</param>
        /// <exception cref="ArgumentNullException"/>
        /// <exception cref="InvalidOperationException"/>
        /// <exception cref="ArgumentOutOfRangeException"/>
        /// <exception cref="NotSupportedException"/>
        public static void Update<T>(this ICollection<T> collection, ICollection<T> updated) => collection.Update(updated, null);
    }
}