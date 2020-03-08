namespace Fiche
{
    /// <summary>
    /// Categorization of .Net primitive types.
    /// </summary>
    public enum PrimitiveTypeSet : byte
    {
        /// <summary>
        /// Primitive types other than numeric types.
        /// </summary>
        PrimitiveType = 0,
        /// <summary>
        /// Numeric types.
        /// </summary>
        NumericType = 1,
        /// <summary>
        /// Integral types.
        /// </summary>
        IntegralType = 2,
        /// <summary>
        /// Floating-point types.
        /// </summary>
        FloatingPointType = 3,
    }
}
