using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

using Fiche.Misc;

namespace Fiche.Collections.Extensions
{
    public static class IListExtensions
    {
        private static IEqualityComparer GenericComparerFactory(object comparedValue)
        {
            if (comparedValue is null)
                return EqualityComparer<object>.Default;
            else
            {
                Type valueType = comparedValue.GetType();
                Type equalityComparerType = typeof(EqualityComparer<>).MakeGenericType(valueType);
                PropertyInfo defaultProperty = equalityComparerType.GetProperty("Default", BindingFlags.Public | BindingFlags.Static);
                return defaultProperty.GetValue(null) as IEqualityComparer;
            }
        }
        /// <summary>
        /// Synchronizes original collection with the updated collection. Updated collection sort order is likely (but not guaranteed) to be applied on the source collection; thus, it is applied only if the original collection implements <see cref="IList{T}"/>.
        /// </summary>
        /// <param name="list">the collection to be modified</param>
        /// <param name="updated">an updated version of the collection to be used to modify current collection</param>
        /// <param name="comparer">the equality comparer to be used. if null, the default equality comparer for type <typeparamref name="T"/> will be used</param>
        /// <exception cref="ArgumentNullException"/>
        /// <exception cref="InvalidOperationException"/>
        /// <exception cref="ArgumentOutOfRangeException"/>
        /// <exception cref="NotSupportedException"/>
        public static void Update(this IList list, IEnumerable updated, IEqualityComparer comparer)
        {
            Error.ThrowIfNull(list, nameof(list));
            Error.ThrowIfNull(updated, nameof(updated));
            Error.ThrowInvalidOperation(list.IsReadOnly, "Cannot update a read-only list.");
            foreach (object item in list)
            {
                if (!updated.Contains(item, comparer ?? GenericComparerFactory(item)))
                    list.Remove(item);
            }
            IEnumerable listEnumerable = list as IEnumerable;
            foreach (object item in updated)
            {
                if (!listEnumerable.Contains(item, comparer ?? GenericComparerFactory(item)))
                    list.Insert(updated.EnumerableIndexOf(item, comparer ?? GenericComparerFactory(item)), item);
            }
        }
        /// <summary>
        /// Synchronizes original collection with the updated collection. Updated collection sort order is likely (but not guaranteed) to be applied on the source collection; thus, it is applied only if the original collection implements <see cref="IList{T}"/>.
        /// </summary>
        /// <param name="list">the collection to be modified</param>
        /// <param name="updated">an updated version of the collection to be used to modify current collection</param>
        /// <exception cref="ArgumentNullException"/>
        /// <exception cref="InvalidOperationException"/>
        /// <exception cref="ArgumentOutOfRangeException"/>
        /// <exception cref="NotSupportedException"/>
        public static void Update(this IList list, IEnumerable updated) => list.Update(updated, null);
    }
}
