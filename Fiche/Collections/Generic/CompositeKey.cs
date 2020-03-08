using System;

namespace Fiche.Collections.Generic
{
    /// <summary>
    /// <para>A strongly-typed version of <see cref="CompositeKey"/>.</para>
    /// <para><see cref="CompositeKey{TKey1, TKey2}"/> does inherit <see cref="CompositeKey"/></para>
    /// <para>Bi-directional implicit conversions could be performed on this class to and from the following types: <typeparamref name="TKey1"/>, <typeparamref name="TKey2"/>, <see cref="Tuple{TKey1, TKey2}"/>, and <see cref="ValueTuple{TKey1, TKey2}"/></para>
    /// </summary>
    /// <typeparam name="TKey1">the type of the first key</typeparam>
    /// <typeparam name="TKey2">the type of the second key</typeparam>
    public class CompositeKey<TKey1, TKey2> : CompositeKey
    {
        public CompositeKey(TKey1 key1, TKey2 key2) : base(key1, key2)
        {
        }

        public new TKey1 Key1 => (TKey1)base.Key1;
        public new TKey2 Key2 => (TKey2)base.Key2;
        public static implicit operator CompositeKey<TKey1, TKey2>(TKey1 key) => new CompositeKey<TKey1, TKey2>(key, default);
        public static implicit operator CompositeKey<TKey1, TKey2>(TKey2 key) => new CompositeKey<TKey1, TKey2>(default, key);
        public static implicit operator CompositeKey<TKey1, TKey2>(Tuple<TKey1, TKey2> tuple) => new CompositeKey<TKey1, TKey2>(tuple.Item1, tuple.Item2);
        public static implicit operator CompositeKey<TKey1, TKey2>(ValueTuple<TKey1, TKey2> valueTuple) => new CompositeKey<TKey1, TKey2>(valueTuple.Item1, valueTuple.Item2);
        public static implicit operator TKey1(CompositeKey<TKey1, TKey2> key) => key is null ? default : key.Key1;
        public static implicit operator TKey2(CompositeKey<TKey1, TKey2> key) => key is null ? default : key.Key2;
        public static implicit operator Tuple<TKey1, TKey2>(CompositeKey<TKey1, TKey2> key) => new Tuple<TKey1, TKey2>(key.Key1, key.Key2);
        public static implicit operator ValueTuple<TKey1, TKey2>(CompositeKey<TKey1, TKey2> key) => (key.Key1, key.Key2);
    }
}
