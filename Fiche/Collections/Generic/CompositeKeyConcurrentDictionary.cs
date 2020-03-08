using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Fiche.Collections.Generic
{
    /// <summary>
    /// A <see cref="ConcurrentDictionary{TKey, TValue}"/> that constraints the key type to an object of type <see cref="CompositeKey"/>.
    /// </summary>
    /// <typeparam name="TKey">key type (an object of type <see cref="CompositeKey"/>)</typeparam>
    /// <typeparam name="TValue">value type</typeparam>
    public class CompositeKeyConcurrentDictionary<TKey, TValue>
        : ConcurrentDictionary<TKey, TValue>
        where TKey : CompositeKey
    {
        public CompositeKeyConcurrentDictionary() : base()
        {

        }
        /// <exception cref="ArgumentNullException"/>
        /// <exception cref="ArgumentException"/>
        public CompositeKeyConcurrentDictionary(IEnumerable<KeyValuePair<TKey, TValue>> collection) : base(collection)
        {
        }

        /// <exception cref="ArgumentNullException"/>
        public CompositeKeyConcurrentDictionary(IEqualityComparer<TKey> comparer) : base(comparer)
        {
        }

        /// <exception cref="ArgumentNullException"/>
        public CompositeKeyConcurrentDictionary(IEnumerable<KeyValuePair<TKey, TValue>> collection, IEqualityComparer<TKey> comparer) : base(collection, comparer)
        {
        }

        /// <exception cref="ArgumentOutOfRangeException"/>
        public CompositeKeyConcurrentDictionary(int concurrencyLevel, int capacity) : base(concurrencyLevel, capacity)
        {
        }

        /// <exception cref="ArgumentNullException"/>
        /// <exception cref="ArgumentOutOfRangeException"/>
        /// <exception cref="ArgumentException"/>
        public CompositeKeyConcurrentDictionary(int concurrencyLevel, IEnumerable<KeyValuePair<TKey, TValue>> collection, IEqualityComparer<TKey> comparer) : base(concurrencyLevel, collection, comparer)
        {
        }

        /// <exception cref="ArgumentNullException"/>
        /// <exception cref="ArgumentOutOfRangeException"/>
        public CompositeKeyConcurrentDictionary(int concurrencyLevel, int capacity, IEqualityComparer<TKey> comparer) : base(concurrencyLevel, capacity, comparer)
        {
        }
    }
    /// <summary>
    /// A <see cref="ConcurrentDictionary{TKey, TValue}"/> that constraints the key type to an object of type <see cref="CompositeKey{TKey1, TKey2}"/>.
    /// </summary>
    /// <typeparam name="TKey1">first key type</typeparam>
    /// <typeparam name="TKey1">second key type</typeparam>
    /// <typeparam name="TValue">value type</typeparam>
    public class CompositeKeyConcurrentDictionary<TKey1, TKey2, TValue>
        : ConcurrentDictionary<CompositeKey<TKey1, TKey2>, TValue>
    {
        public CompositeKeyConcurrentDictionary() : base()
        {
        }
        /// <exception cref="ArgumentNullException"/>
        /// <exception cref="ArgumentException"/>
        public CompositeKeyConcurrentDictionary(IEnumerable<KeyValuePair<CompositeKey<TKey1, TKey2>, TValue>> collection) : base(collection)
        {
        }

        /// <exception cref="ArgumentNullException"/>
        public CompositeKeyConcurrentDictionary(IEqualityComparer<CompositeKey<TKey1, TKey2>> comparer) : base(comparer)
        {
        }

        /// <exception cref="ArgumentNullException"/>
        public CompositeKeyConcurrentDictionary(IEnumerable<KeyValuePair<CompositeKey<TKey1, TKey2>, TValue>> collection, IEqualityComparer<CompositeKey<TKey1, TKey2>> comparer) : base(collection, comparer)
        {
        }

        /// <exception cref="ArgumentOutOfRangeException"/>
        public CompositeKeyConcurrentDictionary(int concurrencyLevel, int capacity) : base(concurrencyLevel, capacity)
        {
        }

        /// <exception cref="ArgumentNullException"/>
        /// <exception cref="ArgumentOutOfRangeException"/>
        /// <exception cref="ArgumentException"/>
        public CompositeKeyConcurrentDictionary(int concurrencyLevel, IEnumerable<KeyValuePair<CompositeKey<TKey1, TKey2>, TValue>> collection, IEqualityComparer<CompositeKey<TKey1, TKey2>> comparer) : base(concurrencyLevel, collection, comparer)
        {
        }

        /// <exception cref="ArgumentNullException"/>
        /// <exception cref="ArgumentOutOfRangeException"/>
        public CompositeKeyConcurrentDictionary(int concurrencyLevel, int capacity, IEqualityComparer<CompositeKey<TKey1, TKey2>> comparer) : base(concurrencyLevel, capacity, comparer)
        {
        }

        /// <exception cref="ArgumentNullException"/>
        /// <exception cref="OverflowException"/>
        public TValue AddOrUpdate(TKey1 key, Func<TKey1, TValue> addValueFactory, Func<TKey1, TValue, TValue> updateValueFactory) => base.AddOrUpdate(key, (k) => addValueFactory(k), (k, v) => updateValueFactory(k, v));
        /// <exception cref="ArgumentNullException"/>
        /// <exception cref="OverflowException"/>
        public TValue AddOrUpdate(TKey1 key, TValue addValue, Func<TKey1, TValue, TValue> updateValueFactory) => base.AddOrUpdate(key, addValue, (k, v) => updateValueFactory(k, v));
        /// <exception cref="ArgumentNullException"/>
        /// <exception cref="OverflowException"/>
        public TValue GetOrAdd(TKey1 key, TValue value) => base.GetOrAdd(key, value);
        /// <exception cref="ArgumentNullException"/>
        /// <exception cref="OverflowException"/>
        public TValue GetOrAdd(TKey1 key, Func<TKey1, TValue> valueFactory) => base.GetOrAdd(key, (k) => valueFactory(k));
        /// <exception cref="ArgumentNullException"/>
        /// <exception cref="OverflowException"/>
        public bool TryAdd(TKey1 key, TValue value) => base.TryAdd(key, value);
        /// <exception cref="ArgumentNullException"/>
        public bool TryGetValue(TKey1 key, out TValue value) => base.TryGetValue(key, out value);
        /// <exception cref="ArgumentNullException"/>
        public bool TryRemove(TKey1 key, out TValue value) => base.TryRemove(key, out value);
        /// <exception cref="ArgumentNullException"/>
        public bool TryUpdate(TKey1 key, TValue newValue, TValue comparisonValue) => base.TryUpdate(key, newValue, comparisonValue);


        /// <exception cref="ArgumentNullException"/>
        /// <exception cref="OverflowException"/>
        public TValue AddOrUpdate(TKey2 key, Func<TKey2, TValue> addValueFactory, Func<TKey2, TValue, TValue> updateValueFactory) => base.AddOrUpdate(key, (k) => addValueFactory(k), (k, v) => updateValueFactory(k, v));
        /// <exception cref="ArgumentNullException"/>
        /// <exception cref="OverflowException"/>
        public TValue AddOrUpdate(TKey2 key, TValue addValue, Func<TKey2, TValue, TValue> updateValueFactory) => base.AddOrUpdate(key, addValue, (k, v) => updateValueFactory(k, v));
        /// <exception cref="ArgumentNullException"/>
        /// <exception cref="OverflowException"/>
        public TValue GetOrAdd(TKey2 key, TValue value) => base.GetOrAdd(key, value);
        /// <exception cref="ArgumentNullException"/>
        /// <exception cref="OverflowException"/>
        public TValue GetOrAdd(TKey2 key, Func<TKey2, TValue> valueFactory) => base.GetOrAdd(key, (k) => valueFactory(k));
        /// <exception cref="ArgumentNullException"/>
        /// <exception cref="OverflowException"/>
        public bool TryAdd(TKey2 key, TValue value) => base.TryAdd(key, value);
        /// <exception cref="ArgumentNullException"/>
        public bool TryGetValue(TKey2 key, out TValue value) => base.TryGetValue(key, out value);
        /// <exception cref="ArgumentNullException"/>
        public bool TryRemove(TKey2 key, out TValue value) => base.TryRemove(key, out value);
        /// <exception cref="ArgumentNullException"/>
        public bool TryUpdate(TKey2 key, TValue newValue, TValue comparisonValue) => base.TryUpdate(key, newValue, comparisonValue);
    }
}
