using System;

using Fiche.Misc;

namespace Fiche.Generic.Extensions
{
    public static class IComparableExtensions
    {
        public static bool IsEqualTo<T>(this T obj, T sbj)
            where T : IComparable<T>
        {
            Error.ThrowIfNull(obj, nameof(obj));
            return obj.CompareTo(sbj).Equals(0);
        }
        public static bool IsLessThan<T>(this T obj, T sbj)
            where T : IComparable<T>
        {
            Error.ThrowIfNull(obj, nameof(obj));
            return obj.CompareTo(sbj) < 0;
        }
        public static bool IsLessThanOrEqualTo<T>(this T obj, T sbj)
            where T : IComparable<T>
        {
            Error.ThrowIfNull(obj, nameof(obj));
            return obj.CompareTo(sbj) <= 0;
        }
        public static bool IsGreaterThan<T>(this T obj, T sbj)
            where T : IComparable<T>
        {
            Error.ThrowIfNull(obj, nameof(obj));
            return obj.CompareTo(sbj) > 0;
        }
        public static bool IsGreaterThanOrEqualTo<T>(this T obj, T sbj)
            where T : IComparable<T>
        {
            Error.ThrowIfNull(obj, nameof(obj));
            return obj.CompareTo(sbj) >= 0;
        }
        public static bool IsBetween<T>(this T obj, T lowerBoundSbj, T higherBoundSbj, bool excludeBounds)
            where T : IComparable<T>
        {
            Error.ThrowIfNull(obj, nameof(obj));
            return excludeBounds ?
                obj.CompareTo(lowerBoundSbj) > 0 && obj.CompareTo(higherBoundSbj) < 0
                : obj.CompareTo(lowerBoundSbj) >= 0 && obj.CompareTo(higherBoundSbj) <= 0;
        }
        public static bool IsBetween<T>(this T obj, T lowerBoundSbj, T higherBoundSbj)
            where T : IComparable<T>
            => obj.IsBetween(lowerBoundSbj, higherBoundSbj, false);
    }
}
