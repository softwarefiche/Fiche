using System;

namespace Fiche.Collections
{
    /// <summary>
    /// Represents an object that will be equal to itself or either of its two keys.
    /// </summary>
    public class CompositeKey : IEquatable<object>
    {
        public CompositeKey(object key1, object key2)
        {
            Key1 = key1;
            Key2 = key2;
        }
        public object Key1 { get; }
        public object Key2 { get; }
        public override bool Equals(object obj) =>
                (
                    ReferenceEquals(this, obj)
                    ||
                    (
                    obj is CompositeKey ck && Equals(ck.Key1) && Equals(ck.Key2)
                    )
                    ||
                    ((Key1?.Equals(obj) ?? false) || ((Key1 is null) && (obj is null)))
                    ||
                    ((Key2?.Equals(obj) ?? false) || ((Key2 is null) && (obj is null)))
                );
        public override int GetHashCode()
        {
            unchecked
            {
                int h = 0;
                h = (h << 5) + 3 + h ^ (Key1 is null ? 0 : Key1.GetHashCode());
                h = (h << 5) + 3 + h ^ (Key2 is null ? 0 : Key2.GetHashCode());
                return h;
            }
        }
        public static bool operator ==(CompositeKey compositeKey, object key) => ReferenceEquals(compositeKey, key) || (!(compositeKey is null) && compositeKey.Equals(key)) || (!(key is null) && key.Equals(compositeKey));
        public static bool operator !=(CompositeKey compositeKey, object key) => !(compositeKey == key);
    }
}
