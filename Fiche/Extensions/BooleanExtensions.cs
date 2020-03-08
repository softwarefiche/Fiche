namespace Fiche.Extensions
{
    public static class BooleanExtensions
    {
        /// <summary>
        /// Short-hand for the ternary conditional operator '?:'
        /// <para>
        /// <code>
        /// condition ? trueResult : falseResult
        /// </code>
        /// </para>
        /// </summary>
        public static T If<T>(this bool condition, T trueResult, T falseResult)
            => condition ? trueResult : falseResult;
        /// <summary>
        /// Short-hand for the ternary conditional operator '?:'; except for allowing incompatible types. Could be useful for non-generic collections conditional manipulation.
        /// <para>
        /// <code>
        /// condition ? trueResult : falseResult
        /// </code>
        /// </para>
        /// </summary>
        public static object If(this bool condition, object trueResult, object falseResult)
            => condition.If<object>(trueResult, falseResult);
    }
}
