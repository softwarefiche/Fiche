using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Fiche.Extensions;
using Fiche.Misc;

namespace Fiche.Collections.Generic.Extensions
{
    public static class IEnumerableExtensions
    {
        #region Iteration and Tasks
        /// <summary>
        /// Executes the specified action for each element in the collection
        /// </summary>
        /// <param name="source">the collection to be iterated</param>
        /// <param name="action">the action to be executed</param>
        /// <exception cref="ArgumentNullException"/>
        public static void ForEach<T>(this IEnumerable<T> source, Action<T> action)
        {
            Error.ThrowIfNull(source, nameof(source));
            Error.ThrowIfNull(action, nameof(action));
            foreach (T item in source)
                action(item);
        }
        /// <summary>
        /// Awaits the execution of the specified action for each element in the collection
        /// </summary>
        /// <param name="source">the collection to be iterated</param>
        /// <param name="asyncAction">the action to be executed</param>
        /// <exception cref="ArgumentNullException"/>
        public static async Task ForEach<T>(this IEnumerable<T> source, Func<T, Task> asyncAction)
        {
            Error.ThrowIfNull(source, nameof(source));
            Error.ThrowIfNull(asyncAction, nameof(asyncAction));
            foreach (T item in source)
                await asyncAction(item);
        }
        /// <summary>
        /// Short-hand for <code>Task.WhenAll(source.Select(item => asyncAction(item)))</code>
        /// </summary>
        /// <exception cref="ArgumentNullException"/>
        /// <exception cref="ArgumentException"/>
        public static Task WhenAll<T>(this IEnumerable<T> source, Func<T, Task> asyncAction)
        {
            Error.ThrowIfNull(source, nameof(source));
            Error.ThrowIfNull(asyncAction, nameof(asyncAction));
            return Task.WhenAll(source.Select(item => asyncAction(item)));
        }
        /// <summary>
        /// Short-hand for <code>Task.WhenAny(source.Select(item => asyncAction(item)))</code>
        /// </summary>
        /// <exception cref="ArgumentNullException"/>
        /// <exception cref="ArgumentException"/>
        public static Task WhenAny<T>(this IEnumerable<T> source, Func<T, Task> asyncAction)
        {
            Error.ThrowIfNull(source, nameof(source));
            Error.ThrowIfNull(asyncAction, nameof(asyncAction));
            return Task.WhenAny(source.Select(item => asyncAction(item)));
        }
        #endregion

        /// <summary>
        /// Iterates over the current collection then yields the item specified.
        /// </summary>
        /// <param name="item">the item to be added</param>
        /// <exception cref="ArgumentNullException"/>
        public static IEnumerable<T> EnumerableAdd<T>(this IEnumerable<T> source, T item)
        {
            Error.ThrowIfNull(source, nameof(source));
            foreach (T sourceItem in source)
                yield return sourceItem;
            yield return item;
        }
        /// <summary>
        /// Iterates over the current collection then over the collection specified.
        /// </summary>
        /// <param name="items">the collection to be added</param>
        /// <exception cref="ArgumentNullException"/>
        public static IEnumerable<T> EnumerableAdd<T>(this IEnumerable<T> source, IEnumerable<T> items)
        {
            Error.ThrowIfNull(source, nameof(source));
            Error.ThrowIfNull(items, nameof(items));
            foreach (T sourceItem in source)
                yield return sourceItem;

            foreach (T item in items)
                yield return item;
        }
        /// <summary>
        /// Returns the specified collection while inserting the item specified at the specified index.
        /// </summary>
        /// <param name="item">the item to be inserted</param>
        /// <exception cref="ArgumentNullException"/>
        /// <exception cref="ArgumentOutOfRangeException"/>
        /// <exception cref="InvalidOperationException"/>
        public static IEnumerable<T> EnumerableInsert<T>(this IEnumerable<T> source, int index, T item)
        {
            Error.ThrowIfNull(source, nameof(source));
            Error.ThrowIfBelowRange(index, nameof(index), 0);
            int counter = 0;
            using (IEnumerator<T> enumerator = source.GetEnumerator())
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
        public static IEnumerable<T> EnumerableInsert<T>(this IEnumerable<T> source, int index, IEnumerable<T> items)
        {
            Error.ThrowIfNull(source, nameof(source));
            Error.ThrowIfBelowRange(index, nameof(index), 0);
            int counter = 0;
            using (IEnumerator<T> enumerator = source.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    if (counter == index)
                    {
                        foreach (T item in items)
                            yield return item;
                    }
                    yield return enumerator.Current;
                    ++counter;
                }
            }
            if (counter == index)
            {
                foreach (T item in items)
                    yield return item;
            }
            else if (counter < index)
                Error.ThrowArgumentOutOfRange(nameof(index));
        }
        /// <summary>
        /// Returns a collection of types for each of the elements in the collection. If an element is null, a null is returned in place of its type
        /// </summary>
        public static IEnumerable<Type> GetTypes<T>(this IEnumerable<T> enumerable)
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
        public static bool AreElementsUnique<T>(this IEnumerable<T> source, IEqualityComparer<T> comparer)
        {
            Error.ThrowIfNull(source, nameof(source));
            HashSet<T> set = new HashSet<T>(comparer);
            foreach (T item in source)
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
        public static bool AreElementsUnique<T>(this IEnumerable<T> source) => source.AreElementsUnique(null);
        /// <summary>
        /// Uses the specified equality comparer to return the current collection without the specified item.
        /// </summary>
        /// <exception cref="ArgumentNullException"/>
        public static IEnumerable<T> Except<T>(this IEnumerable<T> source, T item, IEqualityComparer<T> comparer) => source.Except(item.IterateSelf(), comparer);
        /// <summary>
        /// Uses the default equality comparer to return the current collection without the specified item.
        /// </summary>
        /// <exception cref="ArgumentNullException"/>
        public static IEnumerable<T> Except<T>(this IEnumerable<T> source, T item) => source.Except<T>(item.IterateSelf());
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
        /// Gets whether the collection is null or does not have any elements.
        /// <para>
        /// Short-hand for
        /// <code>
        /// (source is null || !source.Any())
        /// </code>
        /// </para>
        /// </summary>
        public static bool IsNullOrEmpty<T>(this IEnumerable<T> source) => source is null || !source.Any();
        /// <summary>
        /// Uses the specified equality comparer to determine the index of the given item.
        /// <para>This method does not use <see cref="IList{T}"/> optimization; thus, even if the source was a list, it is going to be iterated anyway to guarantee the usage of the specified comparer.</para>
        /// </summary>
        /// <param name="item">the item which index is to be determined</param>
        /// <param name="comparer">the equality comparer to be used</param>
        /// <exception cref="ArgumentNullException"/>
        public static int EnumerableIndexOf<T>(this IEnumerable<T> source, T item, IEqualityComparer<T> comparer)
        {
            Error.ThrowIfNull(source, nameof(source));
            if (comparer is null)
                comparer = EqualityComparer<T>.Default;

            int index = -1;
            foreach (T element in source)
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
        public static int EnumerableIndexOf<T>(this IEnumerable<T> source, T item) => source.EnumerableIndexOf(item, null);
        /// <summary>
        /// Uses the specified equality comparer to determine the last index of the given item.
        /// <para>This method does not use <see cref="IList{T}"/> optimization; thus, even if the source was a list, it is going to be iterated anyway to guarantee the usage of the specified comparer.</para>
        /// </summary>
        /// <param name="item">the item which last index of its occurrance is to be determined</param>
        /// <param name="comparer">the equality comparer to be used</param>
        /// <exception cref="ArgumentNullException"/>
        public static int EnumerableLastIndexOf<T>(this IEnumerable<T> source, T item, IEqualityComparer<T> comparer)
        {
            Error.ThrowIfNull(source, nameof(source));
            if (comparer is null)
                comparer = EqualityComparer<T>.Default;

            int index = -1;
            int lastIndex = -1;
            foreach (T element in source)
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
        public static int EnumerableLastIndexOf<T>(this IEnumerable<T> source, T item) => source.EnumerableLastIndexOf(item, null);

        /// <summary>
        /// Returns a sequence of sequences; each is for one permutation of the input; thus, each child element might match another in items or order but may never match another in both items and order. Each child sequence returned by this method have the number of elements specified in count parameter.
        /// </summary>
        /// <param name="count">the number of elements in each child sequence</param>
        /// <exception cref="ArgumentNullException"/>
        /// <exception cref="ArgumentOutOfRangeException"/>
        public static IEnumerable<IEnumerable<T>> Permute<T>(this IEnumerable<T> source, int count)
        {
            Error.ThrowIfNull(source, nameof(source));
            Error.ThrowIfBelowRange(count, nameof(count), 0);
            if (count == 0)
                yield return Enumerable.Empty<T>();
            else
            {
                int startingElementIndex = 0;
                foreach (T startingElement in source)
                {
                    IEnumerable<T> remainingItems = source.SkipIndex(startingElementIndex);
                    foreach (IEnumerable<T> permutationOfRemainder in remainingItems.Permute(count - 1))
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
        public static IEnumerable<IEnumerable<T>> Permute<T>(this IEnumerable<T> source)
            => source.Permute(source.Count());

        /// <summary>
        /// Randomizes the order of the source sequence in a new sequence.
        /// </summary>
        /// <param name="randomProvider">the random provider used to shuffle. if null, a new instance of <see cref="SafeRandom"/> is used</param>
        /// <exception cref="ArgumentNullException"/>
        public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> source, Random randomProvider)
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
        public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> source)
            => source.Shuffle(new SafeRandom());

        /// <summary>
        /// Returns the sequence skipping the specified index.
        /// </summary>
        /// <exception cref="ArgumentNullException"/>
        /// <exception cref="ArgumentOutOfRangeException"/>
        public static IEnumerable<T> SkipIndex<T>(this IEnumerable<T> source, int index)
        {
            Error.ThrowIfNull(source, nameof(source));
            Error.ThrowIfBelowRange(index, nameof(index), 0);
            int currentIndex = -1;
            bool skipped = false;
            using IEnumerator<T> enumerator = source.GetEnumerator();
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
    }
}
