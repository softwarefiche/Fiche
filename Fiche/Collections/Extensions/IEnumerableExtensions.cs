using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;

using Fiche.Extensions;
using Fiche.Misc;

namespace Fiche.Collections.Extensions
{
    public static class EnumerableExtension
    {
        #region Parellelism
        /// <summary>
        /// Executes the specified action for each element in the collection
        /// </summary>
        /// <param name="source">the collection to be iterated</param>
        /// <param name="action">the action to be executed</param>
        /// <exception cref="ArgumentNullException"/>
        public static void ForEach(this IEnumerable source, Action<object> action)
        {
            Error.ThrowIfNull(source, nameof(source));
            foreach (object item in source)
                action(item);
        }
        /// <summary>
        /// Awaits the execution of the specified action for each element in the collection
        /// </summary>
        /// <param name="source">the collection to be iterated</param>
        /// <param name="asyncAction">the action to be executed</param>
        /// <exception cref="ArgumentNullException"/>
        public static async Task ForEach(this IEnumerable source, Func<object, Task> asyncAction)
        {
            Error.ThrowIfNull(source, nameof(source));
            foreach (object item in source)
                await asyncAction(item);
        }
        /// <summary>
        /// Short-hand for <code>Task.WhenAll(source.Select(item => asyncAction(item)))</code>
        /// </summary>
        /// <exception cref="ArgumentNullException"/>
        /// <exception cref="ArgumentException"/>
        public static Task WhenAll(this IEnumerable source, Func<object, Task> asyncAction)
        {
            Error.ThrowIfNull(source, nameof(source));
            return Task.WhenAll(source.Select(item => asyncAction(item)));
        }
        /// <summary>
        /// Short-hand for <code>Task.WhenAny(source.Select(item => asyncAction(item)))</code>
        /// </summary>
        /// <exception cref="ArgumentNullException"/>
        /// <exception cref="ArgumentException"/>
        public static Task WhenAny(this IEnumerable source, Func<object, Task> asyncAction)
        {
            Error.ThrowIfNull(source, nameof(source));
            return Task.WhenAny(source.Select(item => asyncAction(item)));
        }
        #endregion

        /// <summary>
        /// Iterates over the current collection then yields the item specified.
        /// </summary>
        /// <param name="item">the item to be added</param>
        /// <exception cref="ArgumentNullException"/>
        public static IEnumerable EnumerableAdd(this IEnumerable source, object item)
        {
            Error.ThrowIfNull(source, nameof(source));
            foreach (object sourceItem in source)
                yield return sourceItem;

            yield return item;
        }
        /// <summary>
        /// Iterates over the current collection then over the collection specified.
        /// </summary>
        /// <param name="items">the collection to be added</param>
        /// <exception cref="ArgumentNullException"/>
        public static IEnumerable EnumerableAdd(this IEnumerable source, IEnumerable items)
        {
            Error.ThrowIfNull(source, nameof(source));
            foreach (object sourceItem in source)
                yield return sourceItem;

            foreach (object item in items)
                yield return item;
        }
        /// <summary>
        /// Returns the specified collection while inserting the item specified at the specified index.
        /// </summary>
        /// <param name="item">the item to be inserted</param>
        /// <exception cref="ArgumentNullException"/>
        /// <exception cref="ArgumentOutOfRangeException"/>
        /// <exception cref="InvalidOperationException"/>
        public static IEnumerable EnumerableInsert(this IEnumerable source, int index, object item)
        {
            Error.ThrowIfNull(source, nameof(source));
            Error.ThrowIfBelowRange(index, nameof(index), 0);
            int counter = 0;
            using (DisposableEnumerator enumerator = new DisposableEnumerator(source.GetEnumerator()))
            {
                while (enumerator.MoveNext())
                {
                    if (counter == index)
                        yield return item;

                    yield return enumerator.Current;
                    ++counter;
                }
            }
            if (counter == index) //Insert to the end - same as EnumerableAdd
                yield return item;
            else if (counter < index) //Index was greater than the last index if the item was to be added to the end
                Error.ThrowArgumentOutOfRange(nameof(index));
        }
        /// <summary>
        /// Returns the specified collection while inserting the collection specified starting from the specified index.
        /// </summary>
        /// <param name="items">the collection to be inserted</param>
        /// <exception cref="ArgumentNullException"/>
        /// <exception cref="ArgumentOutOfRangeException"/>
        /// <exception cref="InvalidOperationException"/>
        public static IEnumerable EnumerableInsert(this IEnumerable source, int index, IEnumerable items)
        {
            Error.ThrowIfNull(source, nameof(source));
            Error.ThrowIfBelowRange(index, nameof(index), 0);
            int counter = 0;
            using (DisposableEnumerator enumerator = new DisposableEnumerator(source.GetEnumerator()))
            {
                while (enumerator.MoveNext())
                {
                    if (counter == index)
                    {
                        foreach (object item in items)
                            yield return item;
                    }
                    yield return enumerator.Current;
                    ++counter;
                }
            }
            if (counter == index)
            {
                foreach (object item in items)
                    yield return item;
            }
            else if (counter < index)
            {
                Error.ThrowArgumentOutOfRange(nameof(index));
            }
        }
        /// <summary>
        /// Returns a collection of types for each of the elements in the collection. If an element is null, a null is returned in place of its type
        /// </summary>
        public static IEnumerable<Type> GetTypes(this IEnumerable enumerable)
        {
            Error.ThrowIfNull(enumerable, nameof(enumerable));
            foreach (object item in enumerable)
                yield return item?.GetType();
        }
        /// <summary>
        /// Uses the specified equality comparer to determine whether or not the elements in the collection are unique.
        /// </summary>
        /// <param name="comparer">the equality comparer to be used. If null, the default equality comparer for type <typeparamref name="T"/> will be used.</param>
        /// <exception cref="ArgumentNullException"/>
        public static bool AreElementsUnique(this IEnumerable source, IEqualityComparer comparer)
        {
            Error.ThrowIfNull(source, nameof(source));
            Set set = new Set(comparer);
            foreach (object item in source)
            {
                if (!set.Add(item))
                    return false;
            }
            return true;
        }
        /// <summary>
        /// Uses the default equality comparer to determine whether or not the elements in the collection are unique.
        /// </summary>
        /// <exception cref="ArgumentNullException"/>
        public static bool AreElementsUnique(this IEnumerable source) => source.AreElementsUnique(null);
        /// <summary>
        /// Uses the specified equality comparer to return the current collection without the specified collection.
        /// </summary>
        /// <param name="enumerable">the collection to be ommitted</param>
        /// <param name="comparer">the equality comparer to be used. If null, the default equality comparer for type <typeparamref name="T"/> will be used.</param>
        /// <exception cref="ArgumentNullException"/>
        public static IEnumerable Except(this IEnumerable source, IEnumerable enumerable, IEqualityComparer comparer)
        {
            Error.ThrowIfNull(source, nameof(source));
            Error.ThrowIfNull(enumerable, nameof(enumerable));
            Set set = new Set(comparer);
            foreach (object item in enumerable)
                set.Add(item);

            foreach (object item in source)
            {
                if (set.Add(item))
                    yield return item;
            }
        }
        /// <summary>
        /// Uses the default equality comparer to return the current collection without the specified collection.
        /// </summary>
        /// <param name="enumerable">the collection to be ommitted</param>
        /// <exception cref="ArgumentNullException"/>
        public static IEnumerable Except(this IEnumerable source, IEnumerable enumerable) => source.Except(enumerable, null);
        /// <summary>
        /// Uses the specified equality comparer to return the current collection without the specified item.
        /// </summary>
        /// <param name="item">the item to be ommitted</param>
        /// <param name="comparer">the equality comparer to be used. If null, the default equality comparer for type <typeparamref name="T"/> will be used.</param>
        /// <exception cref="ArgumentNullException"/>
        public static IEnumerable Except(this IEnumerable source, object item, IEqualityComparer comparer) => source.Except(item.IterateSelf(), comparer);
        /// <summary>
        /// Uses the default equality comparer to return the current collection without the specified item.
        /// </summary>
        /// <param name="item">the item to be ommitted</param>
        /// <exception cref="ArgumentNullException"/>
        public static IEnumerable Except(this IEnumerable source, object item) => source.Except(item.IterateSelf(), null);
        /// <summary>
        /// Gets whether the collection is null or does not have any elements.
        /// <para>
        /// Short-hand for
        /// <code>
        /// (source is null || !source.Any())
        /// </code>
        /// </para>
        /// </summary>
        public static bool IsNullOrEmpty(this IEnumerable source) => source is null || !source.Any();
        private class Set
        {
            private int[] buckets;
            private Slot[] slots;
            private int count;
            private int freeList;
            private readonly IEqualityComparer comparer;

            public Set() : this(null) { }

            public Set(IEqualityComparer comparer)
            {
                if (comparer == null)
                    comparer = EqualityComparer<object>.Default;

                this.comparer = comparer;
                this.buckets = new int[7];
                this.slots = new Slot[7];
                this.freeList = -1;
            }

            // If value is not in set, add it and return true; otherwise return false
            public bool Add(object value) => !find(value, true);

            // Check whether value is in set
            public bool Contains(object value) => find(value, false);

            // If value is in set, remove it and return true; otherwise return false
            public bool Remove(object value)
            {
                int hashCode = InternalGetHashCode(value);
                int bucket = hashCode % this.buckets.Length;
                int last = -1;
                for (int i = this.buckets[bucket] - 1; i >= 0; last = i, i = this.slots[i].next)
                {
                    if (this.slots[i].hashCode == hashCode && this.comparer.Equals(this.slots[i].value, value))
                    {
                        if (last < 0)
                            this.buckets[bucket] = this.slots[i].next + 1;
                        else
                            this.slots[last].next = this.slots[i].next;
                        this.slots[i].hashCode = -1;
                        this.slots[i].value = null;
                        this.slots[i].next = this.freeList;
                        this.freeList = i;
                        return true;
                    }
                }
                return false;
            }

            private bool find(object value, bool add)
            {
                int hashCode = InternalGetHashCode(value);
                for (int i = this.buckets[hashCode % this.buckets.Length] - 1; i >= 0; i = this.slots[i].next)
                {
                    if (this.slots[i].hashCode == hashCode && this.comparer.Equals(this.slots[i].value, value))
                        return true;
                }
                if (add)
                {
                    int index;
                    if (this.freeList >= 0)
                    {
                        index = this.freeList;
                        this.freeList = this.slots[index].next;
                    }
                    else
                    {
                        if (this.count == this.slots.Length)
                            resize();

                        index = this.count;
                        this.count++;
                    }
                    int bucket = hashCode % this.buckets.Length;
                    this.slots[index].hashCode = hashCode;
                    this.slots[index].value = value;
                    this.slots[index].next = this.buckets[bucket] - 1;
                    this.buckets[bucket] = index + 1;
                }
                return false;
            }

            private void resize()
            {
                int newSize = checked(this.count * 2 + 1);
                int[] newBuckets = new int[newSize];
                Slot[] newSlots = new Slot[newSize];
                Array.Copy(this.slots, 0, newSlots, 0, this.count);
                for (int i = 0; i < this.count; i++)
                {
                    int bucket = newSlots[i].hashCode % newSize;
                    newSlots[i].next = newBuckets[bucket] - 1;
                    newBuckets[bucket] = i + 1;
                }
                this.buckets = newBuckets;
                this.slots = newSlots;
            }

            internal int InternalGetHashCode(object value) =>
                //Microsoft DevDivBugs 171937. work around comparer implementations that throw when passed null
                (value == null) ? 0 : this.comparer.GetHashCode(value) & 0x7FFFFFFF;

            internal struct Slot
            {
                internal int hashCode;
                internal object value;
                internal int next;
            }
        }
        /// <summary>
        /// Uses the specified equality comparer to determine the index of the given item.
        /// <para>This method does not use <see cref="IList"/> optimization; thus, even if the source was a list, it is going to be iterated anyway to guarantee the usage of the specified comparer.</para>
        /// </summary>
        /// <param name="item">the item which index is to be determined</param>
        /// <param name="comparer">the equality comparer to be used</param>
        /// <exception cref="ArgumentNullException"/>
        public static int EnumerableIndexOf(this IEnumerable source, object item, IEqualityComparer comparer)
        {
            Error.ThrowIfNull(source, nameof(source));
            if (comparer is null)
                comparer = GenericComparerFactory(item);

            int index = -1;
            foreach (object element in source)
            {
                ++index;
                if (comparer.Equals(item, element))
                    return index;
            }
            return -1;
        }
        /// <summary>
        /// Uses the default equality comparer to determine the index of the given item.
        /// <para>This method does not use <see cref="IList{T}"/> optimization; thus, even if the source was a list, it is going to be iterated anyway to guarantee the usage of the default comparer.</para>
        /// </summary>
        /// <param name="item">the item which index is to be determined</param>
        /// <exception cref="ArgumentNullException"/>
        public static int EnumerableIndexOf(this IEnumerable source, object item) => source.EnumerableIndexOf(item, null);
        /// <summary>
        /// Uses the specified equality comparer to determine the last index of the given item.
        /// <para>This method does not use <see cref="IList"/> optimization; thus, even if the source was a list, it is going to be iterated anyway to guarantee the usage of the specified comparer.</para>
        /// </summary>
        /// <param name="item">the item which last index of its occurrance is to be determined</param>
        /// <param name="comparer">the equality comparer to be used</param>
        /// <exception cref="ArgumentNullException"/>
        public static int EnumerableLastIndexOf(this IEnumerable source, object item, IEqualityComparer comparer)
        {
            Error.ThrowIfNull(source, nameof(source));
            if (comparer is null)
                comparer = GenericComparerFactory(item);

            int index = -1;
            int lastIndex = -1;
            foreach (object element in source)
            {
                ++index;
                if (comparer.Equals(item, element))
                    lastIndex = index;
            }
            return lastIndex;
        }
        /// <summary>
        /// Uses the default equality comparer to determine the last index of the given item.
        /// <para>This method does not use <see cref="IList{T}"/> optimization; thus, even if the source was a list, it is going to be iterated anyway to guarantee the usage of the default comparer.</para>
        /// </summary>
        /// <param name="item">the item which last index of its occurrance is to be determined</param>
        /// <exception cref="ArgumentNullException"/>
        public static int EnumerableLastIndexOf(this IEnumerable source, object item) => source.EnumerableLastIndexOf(item, null);

        /// <summary>
        /// Returns a sequence of sequences; each is for one permutation of the input; thus, each child element might match another in items or order but may never match another in both items and order. Each child sequence returned by this method have the number of elements specified in count parameter.
        /// </summary>
        /// <param name="count">the number of elements in each child sequence</param>
        /// <exception cref="ArgumentNullException"/>
        /// <exception cref="ArgumentOutOfRangeException"/>
        public static IEnumerable<IEnumerable> Permute(this IEnumerable source, int count)
        {
            Error.ThrowIfNull(source, nameof(source));
            Error.ThrowIfBelowRange(count, nameof(count), 0);
            if (count == 0)
                yield return EnumerableExtension.Empty();
            else
            {
                int startingElementIndex = 0;
                foreach (object startingElement in source)
                {
                    IEnumerable remainingItems = source.SkipIndex(startingElementIndex);
                    foreach (IEnumerable permutationOfRemainder in remainingItems.Permute(count - 1))
                    {
                        yield return startingElement
                            .IterateSelf()
                            .EnumerableAdd(permutationOfRemainder);
                    }
                    ++startingElementIndex;
                }
            }
        }
        /// <summary>
        /// Returns a sequence of sequences; each is for one permutation of the input; thus, each child element might match another in items or order but may never match another in both items and order. Each child sequence returned by this method have the same number of elements as the original sequence. This is expected to return n number of sequences where n is the factorial of the items count of the original sequence.
        /// </summary>
        /// <exception cref="ArgumentNullException"/>
        /// <exception cref="OverflowException"/>
        public static IEnumerable<IEnumerable> Permute(this IEnumerable source)
            => source.Permute(source.Count());

        /// <summary>
        /// Randomizes the order of the source sequence in a new sequence.
        /// </summary>
        /// <param name="randomProvider">the random provider used to shuffle. if null, a new instance of <see cref="SafeRandom"/> is used</param>
        /// <exception cref="ArgumentNullException"/>
        public static IEnumerable Shuffle(this IEnumerable source, Random randomProvider)
        {
            Error.ThrowIfNull(source, nameof(source));
            if (randomProvider is null)
                randomProvider = new SafeRandom();
            return source.OrderBy(_ => randomProvider.Next());
        }
        /// <summary>
        /// Randomizes the order of the source sequence in a new sequence using a new instance of <see cref="SafeRandom"/> as the random provider.
        /// </summary>
        /// <exception cref="ArgumentNullException"/>
        public static IEnumerable Shuffle(this IEnumerable source)
            => source.Shuffle(new SafeRandom());

        /// <summary>
        /// Returns the sequence skipping the specified index.
        /// </summary>
        /// <exception cref="ArgumentNullException"/>
        /// <exception cref="ArgumentOutOfRangeException"/>
        public static IEnumerable SkipIndex(this IEnumerable source, int index)
        {
            Error.ThrowIfNull(source, nameof(source));
            Error.ThrowIfBelowRange(index, nameof(index), 0);
            int currentIndex = -1;
            bool skipped = false;
            using DisposableEnumerator enumerator = new DisposableEnumerator(source.GetEnumerator());
            while (enumerator.MoveNext())
            {
                ++currentIndex;
                if (currentIndex != index)
                    yield return enumerator.Current;
                else
                    skipped = true;
            }
            if (!skipped)
                Error.ThrowArgumentOutOfRange(nameof(index));
        }

        #region LINQ-like extensions for the non-generic version of IEnumerable
        public static IEnumerable Empty() { yield break; }
        public static IEnumerable<TResult> Select<TResult>(this IEnumerable source, Func<object, TResult> selector)
        {
            Error.ThrowIfNull(source, nameof(source));
            Error.ThrowIfNull(selector, nameof(selector));
            foreach (object element in source)
                yield return selector(element);
        }

        public static IEnumerable<TResult> Select<TResult>(this IEnumerable source, Func<object, int, TResult> selector)
        {
            Error.ThrowIfNull(source, nameof(source));
            Error.ThrowIfNull(selector, nameof(selector));
            return SelectIterator(source, selector);
        }

        private static IEnumerable<TResult> SelectIterator<TResult>(IEnumerable source, Func<object, int, TResult> selector)
        {
            int index = -1;
            foreach (object element in source)
            {
                checked { index++; }
                yield return selector(element, index);
            }
        }

        public static bool Any(this IEnumerable source)
        {
            Error.ThrowIfNull(source, nameof(source));
            using DisposableEnumerator e = new DisposableEnumerator(source.GetEnumerator());
            if (e.MoveNext())
                return true;

            return false;
        }
        public static bool Any(this IEnumerable source, Func<object, bool> predicate)
        {
            Error.ThrowIfNull(source, nameof(source));
            Error.ThrowIfNull(predicate, nameof(predicate));
            foreach (object element in source)
            {
                if (predicate(element))
                    return true;
            }
            return false;
        }

        public static bool All(this IEnumerable source, Func<object, bool> predicate)
        {
            Error.ThrowIfNull(source, nameof(source));
            Error.ThrowIfNull(predicate, nameof(predicate));
            foreach (object element in source)
            {
                if (!predicate(element))
                    return false;
            }
            return true;
        }

        public static int Count(this IEnumerable source)
        {
            Error.ThrowIfNull(source, nameof(source));

            if (source is ICollection collection)
                return collection.Count;

            int count = 0;
            using DisposableEnumerator e = new DisposableEnumerator(source.GetEnumerator());
            checked
            {
                while (e.MoveNext())
                    ++count;
            }
            return count;
        }

        public static int Count(this IEnumerable source, Func<object, bool> predicate)
        {
            Error.ThrowIfNull(source, nameof(source));
            Error.ThrowIfNull(predicate, nameof(predicate));
            int count = 0;
            foreach (object element in source)
            {
                checked
                {
                    if (predicate(element))
                        ++count;
                }
            }
            return count;
        }

        public static long LongCount(this IEnumerable source)
        {
            Error.ThrowIfNull(source, nameof(source));
            long count = 0;
            using DisposableEnumerator e = new DisposableEnumerator(source.GetEnumerator());
            checked
            {
                while (e.MoveNext())
                    ++count;
            }
            return count;
        }

        public static long LongCount(this IEnumerable source, Func<object, bool> predicate)
        {
            Error.ThrowIfNull(source, nameof(source));
            Error.ThrowIfNull(predicate, nameof(predicate));
            long count = 0;
            foreach (object element in source)
            {
                checked
                {
                    if (predicate(element))
                        ++count;
                }
            }
            return count;
        }

        public static bool Contains(this IEnumerable source, object value) => Contains(source, value, null);
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
        public static bool Contains(this IEnumerable source, object value, IEqualityComparer comparer)
        {
            Error.ThrowIfNull(source, nameof(source));
            if (comparer is null)
                comparer = GenericComparerFactory(value);
            foreach (object element in source)
            {
                if (comparer.Equals(element, value))
                    return true;
            }

            return false;
        }

        #region OrderBy

        public static IOrderedEnumerable OrderBy<TKey>(this IEnumerable source, Func<object, TKey> keySelector) => new OrderedEnumerable<TKey>(source, keySelector, null, false);

        public static IOrderedEnumerable OrderBy<TKey>(this IEnumerable source, Func<object, TKey> keySelector, IComparer comparer) => new OrderedEnumerable<TKey>(source, keySelector, comparer, false);

        public static IOrderedEnumerable OrderByDescending<TKey>(this IEnumerable source, Func<object, TKey> keySelector) => new OrderedEnumerable<TKey>(source, keySelector, null, true);

        public static IOrderedEnumerable OrderByDescending<TKey>(this IEnumerable source, Func<object, TKey> keySelector, IComparer comparer) => new OrderedEnumerable<TKey>(source, keySelector, comparer, true);

        public static IOrderedEnumerable ThenBy<TKey>(this IOrderedEnumerable source, Func<object, TKey> keySelector)
        {
            Error.ThrowIfNull(source, nameof(source));
            return source.CreateOrderedEnumerable<TKey>(keySelector, null, false);
        }

        public static IOrderedEnumerable ThenBy<TKey>(this IOrderedEnumerable source, Func<object, TKey> keySelector, IComparer comparer)
        {
            Error.ThrowIfNull(source, nameof(source));
            return source.CreateOrderedEnumerable<TKey>(keySelector, comparer, false);
        }

        public static IOrderedEnumerable ThenByDescending<TKey>(this IOrderedEnumerable source, Func<object, TKey> keySelector)
        {
            Error.ThrowIfNull(source, nameof(source));
            return source.CreateOrderedEnumerable<TKey>(keySelector, null, true);
        }

        public static IOrderedEnumerable ThenByDescending<TKey>(this IOrderedEnumerable source, Func<object, TKey> keySelector, IComparer comparer)
        {
            Error.ThrowIfNull(source, nameof(source));
            return source.CreateOrderedEnumerable<TKey>(keySelector, comparer, true);
        }

        private abstract class OrderedEnumerable : IOrderedEnumerable
        {
            internal IEnumerable source;

            public IEnumerator GetEnumerator()
            {
                Buffer buffer = new Buffer(this.source);
                if (buffer.count > 0)
                {
                    EnumerableSorter sorter = GetEnumerableSorter(null);
                    int[] map = sorter.Sort(buffer.items, buffer.count);
                    for (int i = 0; i < buffer.count; i++) yield return buffer.items[map[i]];
                }
            }

            internal abstract EnumerableSorter GetEnumerableSorter(EnumerableSorter next);

            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

            IOrderedEnumerable IOrderedEnumerable.CreateOrderedEnumerable<TKey>(Func<object, TKey> keySelector, IComparer comparer, bool descending)
            {
                OrderedEnumerable<TKey> result = new OrderedEnumerable<TKey>(this.source, keySelector, comparer, descending)
                {
                    parent = this
                };
                return result;
            }
        }

        private class OrderedEnumerable<TKey> : OrderedEnumerable
        {
            internal OrderedEnumerable parent;
            internal Func<object, TKey> keySelector;
            internal IComparer comparer;
            internal bool descending;

            internal OrderedEnumerable(IEnumerable source, Func<object, TKey> keySelector, IComparer comparer, bool descending)
            {
                Error.ThrowIfNull(source, nameof(source));
                Error.ThrowIfNull(keySelector, nameof(keySelector));
                this.source = source;
                this.parent = null;
                this.keySelector = keySelector;
                this.comparer = comparer ?? Comparer<TKey>.Default;
                this.descending = descending;
            }

            internal override EnumerableSorter GetEnumerableSorter(EnumerableSorter next)
            {
                EnumerableSorter sorter = new EnumerableSorter<TKey>(this.keySelector, this.comparer, this.descending, next);
                if (this.parent != null) sorter = this.parent.GetEnumerableSorter(sorter);
                return sorter;
            }
        }

        private abstract class EnumerableSorter
        {
            internal abstract void ComputeKeys(object[] elements, int count);

            internal abstract int CompareKeys(int index1, int index2);

            internal int[] Sort(object[] elements, int count)
            {
                ComputeKeys(elements, count);
                int[] map = new int[count];
                for (int i = 0; i < count; i++) map[i] = i;
                quickSort(map, 0, count - 1);
                return map;
            }

            private void quickSort(int[] map, int left, int right)
            {
                do
                {
                    int i = left;
                    int j = right;
                    int x = map[i + ((j - i) >> 1)];
                    do
                    {
                        while (i < map.Length && CompareKeys(x, map[i]) > 0) i++;
                        while (j >= 0 && CompareKeys(x, map[j]) < 0) j--;
                        if (i > j) break;
                        if (i < j)
                        {
                            int temp = map[i];
                            map[i] = map[j];
                            map[j] = temp;
                        }
                        i++;
                        j--;
                    } while (i <= j);
                    if (j - left <= right - i)
                    {
                        if (left < j) quickSort(map, left, j);
                        left = i;
                    }
                    else
                    {
                        if (i < right) quickSort(map, i, right);
                        right = j;
                    }
                } while (left < right);
            }
        }

        private class EnumerableSorter<TKey> : EnumerableSorter
        {
            internal Func<object, TKey> keySelector;
            internal IComparer comparer;
            internal bool descending;
            internal EnumerableSorter next;
            internal TKey[] keys;

            internal EnumerableSorter(Func<object, TKey> keySelector, IComparer comparer, bool descending, EnumerableSorter next)
            {
                this.keySelector = keySelector;
                this.comparer = comparer;
                this.descending = descending;
                this.next = next;
            }

            internal override void ComputeKeys(object[] elements, int count)
            {
                this.keys = new TKey[count];
                for (int i = 0; i < count; i++) this.keys[i] = this.keySelector(elements[i]);
                if (this.next != null) this.next.ComputeKeys(elements, count);
            }

            internal override int CompareKeys(int index1, int index2)
            {
                int c = this.comparer.Compare(this.keys[index1], this.keys[index2]);
                if (c == 0)
                {
                    if (this.next == null) return index1 - index2;
                    return this.next.CompareKeys(index1, index2);
                }
                return this.descending ? -c : c;
            }
        }

        private struct Buffer
        {
            internal object[] items;
            internal int count;

            internal Buffer(IEnumerable source)
            {
                object[] items = null;
                int count = 0;
                if (source is ICollection collection)
                {
                    count = collection.Count;
                    if (count > 0)
                    {
                        items = new object[count];
                        collection.CopyTo(items, 0);
                    }
                }
                else
                {
                    foreach (object item in source)
                    {
                        if (items == null)
                        {
                            items = new object[4];
                        }
                        else if (items.Length == count)
                        {
                            object[] newItems = new object[checked(count * 2)];
                            Array.Copy(items, 0, newItems, 0, count);
                            items = newItems;
                        }
                        items[count] = item;
                        count++;
                    }
                }
                this.items = items;
                this.count = count;
            }

            internal Array ToArray()
            {
                if (this.count == 0) return Array.Empty<object>();
                if (this.items.Length == this.count) return this.items;
                object[] result = new object[this.count];
                Array.Copy(this.items, 0, result, 0, this.count);
                return result;
            }
        }
        #endregion

        #endregion

    }
}
