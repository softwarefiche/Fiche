using System;
using System.Collections.Generic;
using System.Linq;

namespace Fiche.Extensions
{
    public static class EnumExtensions
    {
        /// <summary>
        /// Determines whether or not the current value contains the specified flag.
        /// </summary>
        public static bool HasFlag<T>(this T source, T flag)
            where T : Enum => source.HasFlag(flag);
        /// <summary>
        /// Gets all the flags included in the current value.
        /// </summary>
        public static IEnumerable<T> GetFlags<T>(this T source)
            where T : Enum
        {
            Type t = typeof(T);
            foreach (T value in Enum.GetValues(t))
            {
                if (source.HasFlag(value))
                    yield return value;
            }
        }
        /// <summary>
        /// Combines each flag of the specified sequence into a single value.
        /// </summary>
        public static T CombineFlags<T>(this IEnumerable<T> source)
            where T : Enum
        {
            long result = 0L;
            foreach (T flag in source)
            {
                long lFlag = Convert.ToInt64(flag);
                result |= lFlag;
            }
            return (T)Convert.ChangeType(result, typeof(T));
        }
        /// <summary>
        /// Determines whether or not the current value has any of the given flags.
        /// </summary>
        public static bool HasAny<T>(this T source, IEnumerable<T> flags)
            where T : Enum => source.GetFlags().Any(f => flags.Any(sf => f.Equals(sf)));
        /// <summary>
        /// Determines whether or not the current value has any of the given flags.
        /// </summary>
        public static bool HasAny<T>(this T source, params T[] flags)
            where T : Enum => source.HasAny(flags.AsEnumerable());
        /// <summary>
        /// Determines whether or not the current value has any of the given flags.
        /// </summary>
        public static bool HasAny<T>(this T source, T flags)
            where T : Enum => source.HasAny(flags.GetFlags());
        /// <summary>
        /// Determines whether or not the current value is equal to any of the given flags.
        /// </summary>
        public static bool IsAny<T>(this T source, IEnumerable<T> flags)
            where T : Enum => flags.Any(flag => flag.Equals(source));
        /// <summary>
        /// Determines whether or not the current value is equal to any of the given flags.
        /// </summary>
        public static bool IsAny<T>(this T source, params T[] flags)
            where T : Enum => source.IsAny(flags.AsEnumerable());
        /// <summary>
        /// Determines whether or not the current value is equal to any of the given flags.
        /// </summary>
        public static bool IsAny<T>(this T source, T flags)
            where T : Enum => source.IsAny(flags.GetFlags());
        /// <summary>
        /// Determines whether or not the current value has all of the given flags.
        /// </summary>
        public static bool HasAll<T>(this T source, IEnumerable<T> flags)
            where T : Enum => source.GetFlags().All(f => flags.Any(sf => f.Equals(sf)));
        /// <summary>
        /// Determines whether or not the current value has all of the given flags.
        /// </summary>
        public static bool HasAll<T>(this T source, params T[] flags)
            where T : Enum => source.HasAll(flags.AsEnumerable());
        /// <summary>
        /// Determines whether or not the current value has all of the given flags.
        /// </summary>
        public static bool HasAll<T>(this T source, T flags)
            where T : Enum => source.HasAll(flags.GetFlags());
        /// <summary>
        /// Returns the current value with the specified flags turned on or off.
        /// </summary>
        public static T SetFlags<T>(this T source, bool on, IEnumerable<T> flags)
            where T : Enum
        {
            long result = Convert.ToInt64(source);
            foreach (T flag in flags)
            {
                long lFlag = Convert.ToInt64(flag);
                if (on)
                    result |= lFlag;
                else
                    result &= ~lFlag;
            }
            return (T)Convert.ChangeType(result, typeof(T));
        }
        /// <summary>
        /// Returns the current value with the specified flags turned on or off.
        /// </summary>
        public static T SetFlags<T>(this T source, bool on, params T[] flags)
            where T : Enum => source.SetFlags(on, flags.AsEnumerable());
        /// <summary>
        /// Returns the current value with the specified flags turned on.
        /// </summary>
        public static T SetFlags<T>(this T source, IEnumerable<T> flags)
            where T : Enum => source.SetFlags(true, flags);
        /// <summary>
        /// Returns the current value with the specified flags turned on.
        /// </summary>
        public static T SetFlags<T>(this T source, params T[] flags)
            where T : Enum => source.SetFlags(true, flags.AsEnumerable());
        /// <summary>
        /// Returns the current value with the specified flags turned off.
        /// </summary>
        public static T ClearFlags<T>(this T source, IEnumerable<T> flags)
            where T : Enum => source.SetFlags(false, flags);
        /// <summary>
        /// Returns the current value with the specified flags turned off.
        /// </summary>
        public static T ClearFlags<T>(this T source, params T[] flags)
            where T : Enum => source.SetFlags(false, flags.AsEnumerable());
        /// <summary>
        /// Returns the current value with the specified combined-flags turned on or off.
        /// </summary>
        public static T SetFlags<T>(this T source, T flags, bool on)
        {
            long result = Convert.ToInt64(source);
            long lFlags = Convert.ToInt64(flags);
            if (on)
                result |= lFlags;
            else
                result &= ~lFlags;
            return (T)Convert.ChangeType(result, typeof(T));
        }
        /// <summary>
        /// Returns the current value with the specified combined-flags turned on.
        /// </summary>
        public static T SetFlags<T>(this T source, T flags)
            where T : Enum => source.SetFlags(flags, true);
        /// <summary>
        /// Returns the current value with the specified combined-flags turned off.
        /// </summary>
        public static T ClearFlags<T>(this T source, T flags)
            where T : Enum => source.SetFlags(flags, false);
        /// <summary>
        /// Returns the current value in the type of <typeparamref name="T"/>'s underlying type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static dynamic UnderlyingValue<T>(this T source)
            where T : Enum => Convert.ChangeType(source, Enum.GetUnderlyingType(typeof(T)));
    }
}
