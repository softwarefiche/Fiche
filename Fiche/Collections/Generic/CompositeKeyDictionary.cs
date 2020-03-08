using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Fiche.Collections.Generic
{
    /// <summary>
    /// A <see cref="Dictionary{TKey, TValue}"/> that constraints the key type to an object of type <see cref="CompositeKey"/>.
    /// </summary>
    /// <typeparam name="TKey">key type (an object of type <see cref="CompositeKey"/>)</typeparam>
    /// <typeparam name="TValue">value type</typeparam>
    public class CompositeKeyDictionary<TKey, TValue>
        : Dictionary<TKey, TValue>
        where TKey : CompositeKey
    {
        public CompositeKeyDictionary() : base()
        {
        }

        /// <exception cref="ArgumentNullException"/>
        /// <exception cref="ArgumentException"/>
        public CompositeKeyDictionary(IDictionary<TKey, TValue> dictionary) : base(dictionary)
        {
        }

        public CompositeKeyDictionary(IEqualityComparer<TKey> comparer) : base(comparer)
        {
        }

        /// <exception cref="ArgumentOutOfRangeException"/>
        public CompositeKeyDictionary(int capacity) : base(capacity)
        {
        }

        /// <exception cref="ArgumentNullException"/>
        /// <exception cref="ArgumentException"/>
        public CompositeKeyDictionary(IDictionary<TKey, TValue> dictionary, IEqualityComparer<TKey> comparer) : base(dictionary, comparer)
        {
        }

        /// <exception cref="ArgumentOutOfRangeException"/>
        public CompositeKeyDictionary(int capacity, IEqualityComparer<TKey> comparer) : base(capacity, comparer)
        {
        }

        protected CompositeKeyDictionary(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
    /// <summary>
    /// A <see cref="Dictionary{TKey, TValue}"/> that constraints the key type to an object of type <see cref="CompositeKey{TKey1, TKey2}"/>.
    /// </summary>
    /// <typeparam name="TKey1">first key type</typeparam>
    /// <typeparam name="TKey1">second key type</typeparam>
    /// <typeparam name="TValue">value type</typeparam>
    public class CompositeKeyDictionary<TKey1, TKey2, TValue>
        : Dictionary<CompositeKey<TKey1, TKey2>, TValue>
    {
        public CompositeKeyDictionary() : base()
        {
        }

        /// <exception cref="ArgumentNullException"/>
        /// <exception cref="ArgumentException"/>
        public CompositeKeyDictionary(IDictionary<CompositeKey<TKey1, TKey2>, TValue> dictionary) : base(dictionary)
        {
        }

        public CompositeKeyDictionary(IEqualityComparer<CompositeKey<TKey1, TKey2>> comparer) : base(comparer)
        {
        }

        /// <exception cref="ArgumentOutOfRangeException"/>
        public CompositeKeyDictionary(int capacity) : base(capacity)
        {
        }

        /// <exception cref="ArgumentNullException"/>
        /// <exception cref="ArgumentException"/>
        public CompositeKeyDictionary(IDictionary<CompositeKey<TKey1, TKey2>, TValue> dictionary, IEqualityComparer<CompositeKey<TKey1, TKey2>> comparer) : base(dictionary, comparer)
        {
        }

        /// <exception cref="ArgumentOutOfRangeException"/>
        public CompositeKeyDictionary(int capacity, IEqualityComparer<CompositeKey<TKey1, TKey2>> comparer) : base(capacity, comparer)
        {
        }

        protected CompositeKeyDictionary(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
        public void Add(TKey1 key1, TKey2 key2, TValue value) => base.Add(new CompositeKey<TKey1, TKey2>(key1, key2), value);
        public void Remove(TKey1 key1, TKey2 key2) => base.Remove(new CompositeKey<TKey1, TKey2>(key1, key2));
    }
}
