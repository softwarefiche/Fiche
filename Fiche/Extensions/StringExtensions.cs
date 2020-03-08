using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Fiche.Collections.Generic.Extensions;
using Fiche.Misc;

namespace Fiche.Extensions
{
    public static class StringExtensions
    {
        #region String static methods
        /// <summary>
        /// Indicates wether the specified string is null or <see langword="string"/>.Empty string.
        /// </summary>
        public static bool IsNullOrEmpty(this string str) => string.IsNullOrEmpty(str);
        /// <summary>
        /// Concatenates the members of the specified sequence of string to the end of the current string.
        /// </summary>
        /// <exception cref="ArgumentNullException"/>
        public static string Concat(this string str, IEnumerable<string> strs) => string.Concat((strs ?? Enumerable.Empty<string>()).EnumerableInsert(0, str));
        /// <summary>
        /// Concatenates the members of the specified sequence of string to the end of the current string.
        /// </summary>
        /// <exception cref="ArgumentNullException"/>
        public static string Concat(this string str, params string[] strs) => str.Concat(strs?.AsEnumerable());
        /// <summary>
        /// Creates a new instance of <see langword="string"/> with the same value as the specified <see langword="string"/>.
        /// </summary>
        /// <exception cref="ArgumentNullException"/>
        public static string Copy(this string str) => string.Copy(str);
        /// <summary>
        ///  Replaces the format item in a specified string with the string representation of a corresponding object in a specified array.
        /// </summary>
        /// <exception cref="ArgumentNullException"/>
        /// <exception cref="FormatException"/>
        public static string Format(this string format, params object[] args) => string.Format(format, args);
        /// <summary>
        /// Replaces the format items in a specified string with the string representations of corresponding objects in a specified array. A parameter supplies culture-specific formatting information.
        /// </summary>
        /// <exception cref="ArgumentNullException"/>
        /// <exception cref="FormatException"/>
        public static string Format(this string format, IFormatProvider provider, params object[] args) => string.Format(provider, format, args);
        #endregion
        /// <summary>
        /// Indicates whether or not the given string is equal to an empty string.
        /// </summary>
        public static bool IsEmpty(this string str)
            => !(str is null) && string.Empty.Equals(str);
        /// <summary>
        /// Adds the specified indentation white-space character to the beginning of each line in the specified string times the number specified in indentation parameter.
        /// </summary>
        /// <exception cref="ArgumentNullException"/>
        /// <exception cref="ArgumentOutOfRangeException"/>
        /// <exception cref="ArgumentException"/>
        public static string Indent(this string str, int indentation, char indentationChar)
        {
            Error.ThrowIfNull(str, nameof(str));
            Error.ThrowIfBelowRange(indentation, nameof(indentation), 0, $"Indentaion must be greater than or equal to 0.{Environment.NewLine}Current value: {indentation}.");
            Error.ThrowArgumentException(!char.IsWhiteSpace(indentationChar), nameof(indentationChar), $"Indentation character must be a white-space character.{Environment.NewLine}Current value: '{indentationChar}'.");
            if (indentation == 0) return str;
            if (str.IsEmpty()) return string.Empty;
            StringBuilder sb = new StringBuilder(str);
            string[] lines = str.Split(new string[] { "\r\n", "\n" }, StringSplitOptions.None);
            bool isFirst = true;
            foreach (string line in lines)
            {
                sb.AppendFormat("{0}{1}{2}", isFirst.If(string.Empty, Environment.NewLine), new string(indentationChar, indentation), line);
                if (isFirst)
                    isFirst = false;
            }
            return sb.ToString();
        }
        /// <summary>
        /// Adds a tab character to the beginning of each line in the specified string times the number specified in indentation parameter.
        /// </summary>
        /// <exception cref="ArgumentNullException"/>
        /// <exception cref="ArgumentOutOfRangeException"/>
        /// <exception cref="ArgumentException"/>
        public static string IndentTab(this string str, int indentation) => str.Indent(indentation, '\t');
        /// <summary>
        /// Adds a space character to the beginning of each line in the specified string times the number specified in indentation parameter.
        /// </summary>
        /// <exception cref="ArgumentNullException"/>
        /// <exception cref="ArgumentOutOfRangeException"/>
        /// <exception cref="ArgumentException"/>
        public static string IndentSpace(this string str, int indentation) => str.Indent(indentation, ' ');
        /// <summary>
        /// Trims the specified string's end to the maxLength specified trying to keep the string given in endsWith parameter at the end.
        /// <para>For long text preview purposes. e.g</para>
        /// <para>The quick brown fox jum...</para>
        /// </summary>
        /// <exception cref="ArgumentNullException"/>
        /// <exception cref="ArgumentOutOfRangeException"/>
        public static string WithMaxLength(this string str, int maxLength, string endsWith)
        {
            Error.ThrowIfNull(str, nameof(str));
            Error.ThrowIfBelowRange(maxLength, nameof(maxLength), 1, $"'{nameof(maxLength)}' must be a value greater than 0.");
            if (str.Length <= maxLength) return str;
            int endsWithLength = endsWith?.Length ?? 0;
            int length = maxLength - endsWithLength;
            if (length <= 0)
                return endsWith.Substring(length);
            string subStr = str.Substring(0, length);
            if (!string.IsNullOrEmpty(endsWith))
                subStr += endsWith;
            return subStr;
        }
        /// <summary>
        /// Trims the specified string's end to the maxLength specified trying to keep "..." at the end.
        /// <para>For long text preview purposes. e.g</para>
        /// <para>The quick brown fox jum...</para>
        /// </summary>
        /// <exception cref="ArgumentNullException"/>
        /// <exception cref="ArgumentOutOfRangeException"/>
        public static string WithMaxLength(this string str, int maxLength) => str.WithMaxLength(maxLength, "...");
        #region Surround string by
        /// <summary>
        /// Returns a new string consisting of the original string with the specified prefix.
        /// </summary>
        /// <exception cref="ArgumentNullException"/>
        public static string InsertPrefix(this string str, string prefix)
        {
            Error.ThrowIfNull(str, nameof(str));
            if (prefix.IsEmpty())
                return str;
            return $"{prefix}{str}";
        }
        /// <summary>
        /// Returns a new string consisting of the original string with the specified suffix.
        /// </summary>
        /// <exception cref="ArgumentNullException"/>
        public static string InsertSuffix(this string str, string suffix)
        {
            Error.ThrowIfNull(str, nameof(str));
            if (suffix.IsEmpty())
                return str;
            return $"{str}{suffix}";
        }
        /// <summary>
        /// Returns a new string consisting of the original string with the specified prefix and suffix.
        /// </summary>
        /// <exception cref="ArgumentNullException"/>
        public static string SurroundBy(this string str, string prefix, string suffix)
        {
            Error.ThrowIfNull(str, nameof(str));
            if (prefix.IsNullOrEmpty() && suffix.IsNullOrEmpty())
                return str;
            return $"{prefix ?? string.Empty}{str}{suffix ?? string.Empty}";
        }
        /// <summary>
        /// Surrounds the specified string with two square brackets.
        /// </summary>
        /// <exception cref="ArgumentNullException"/>
        public static string SurroundBySquareBrackets(this string str) => str.SurroundBy("[", "]");
        /// <summary>
        /// Surrounds the specified string with two curly brackets.
        /// </summary>
        /// <exception cref="ArgumentNullException"/>
        public static string SurroundByCurlyBrackets(this string str) => str.SurroundBy("{", "}");
        /// <summary>
        /// Surrounds the specified string with two rounded brackets.
        /// </summary>
        /// <exception cref="ArgumentNullException"/>
        public static string SurroundByRoundedBrackets(this string str) => str.SurroundBy("(", ")");
        /// <summary>
        /// Adds the specified surroundings at the beginning of and at the end of the specified string.
        /// </summary>
        /// <exception cref="ArgumentNullException"/>
        public static string SurroundBy(this string str, string surroundings) => str.SurroundBy(surroundings, surroundings);
        /// <summary>
        /// Surrounds the specified string with single-quote characters.
        /// </summary>
        /// <exception cref="ArgumentNullException"/>
        public static string SurroundBySingleQuotes(this string str) => str.SurroundBy("'");
        /// <summary>
        /// Surrounds the specified string with double-quote characters.
        /// </summary>
        /// <exception cref="ArgumentNullException"/>
        public static string SurroundByDoubleQuotes(this string str) => str.SurroundBy("\"");
        #endregion

        #region Numeric (precision, scale) floating-point conversions
        /// <summary>
        /// Converts the provided string to <see langword="float"/> with the specified precision and scale.
        /// <para>
        /// "0109123456".ToFloat(9, 6) returns 109.123456F
        /// </para>
        /// </summary>
        /// <exception cref="ArgumentNullException"/>
        /// <exception cref="ArgumentException"/>
        /// <exception cref="FormatException"/>
        /// <exception cref="OverflowException"/>
        public static float ToFloat(this string str, int precision, int scale)
        {
            Error.ThrowIfNull(str);
            Error.ThrowArgumentException(str.Length < precision, nameof(precision), "Precision cannot be greater than string length.");
            return float
                .Parse(
                    str.Substring(str.Length - precision)
                    .Insert(precision - scale, ".")
                    .ToString(),
                System.Globalization.NumberStyles.AllowDecimalPoint,
                System.Globalization.NumberFormatInfo.InvariantInfo);
        }
        /// <summary>
        /// Converts the provided string to <see langword="double"/> with the specified precision and scale.
        /// <para>
        /// "0109123456".ToDouble(9, 6) returns 109.123456D
        /// </para>
        /// </summary>
        /// <exception cref="ArgumentNullException"/>
        /// <exception cref="ArgumentException"/>
        /// <exception cref="FormatException"/>
        /// <exception cref="OverflowException"/>
        public static double ToDouble(this string str, int precision, int scale)
        {
            Error.ThrowIfNull(str);
            Error.ThrowArgumentException(str.Length < precision, nameof(precision), "Precision cannot be greater than string length.");
            return double
                .Parse(
                    str.Substring(str.Length - precision)
                    .Insert(precision - scale, ".")
                    .ToString(),
                System.Globalization.NumberStyles.AllowDecimalPoint,
                System.Globalization.NumberFormatInfo.InvariantInfo);
        }
        /// <summary>
        /// Converts the provided string to <see langword="decimal"/> with the specified precision and scale.
        /// <para>
        /// "0109123456".ToDecimal(9, 6) returns 109.123456M
        /// </para>
        /// </summary>
        /// <exception cref="ArgumentNullException"/>
        /// <exception cref="ArgumentException"/>
        /// <exception cref="FormatException"/>
        /// <exception cref="OverflowException"/>
        public static decimal ToDecimal(this string str, int precision, int scale)
        {
            Error.ThrowIfNull(str);
            Error.ThrowArgumentException(str.Length < precision, nameof(precision), "Precision cannot be greater than string length.");
            return decimal
                .Parse(
                    str.Substring(str.Length - precision)
                    .Insert(precision - scale, ".")
                    .ToString(),
                System.Globalization.NumberStyles.AllowDecimalPoint,
                System.Globalization.NumberFormatInfo.InvariantInfo);
        }
        #endregion
    }
}
