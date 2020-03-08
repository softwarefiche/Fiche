using System;
using System.Collections.Generic;
using System.Linq;

using Fiche.Misc;

namespace Fiche.Extensions
{
    public static class TypeExtensions
    {
        private static readonly HashSet<Type> floatingPointTypes;
        private static readonly HashSet<Type> integralTypes;
        private static readonly HashSet<Type> numericTypes;
        private static readonly HashSet<Type> primitiveTypeSet;

        #region static ctor and dependencies
        static TypeExtensions()
        {
            floatingPointTypes = new HashSet<Type>()
            {
                StaticTypes.floatType,
                StaticTypes.doubleType,
                StaticTypes.decimalType
            };
            integralTypes = new HashSet<Type>()
            {

                StaticTypes.sbyteType,
                StaticTypes.byteType,
                StaticTypes.shortType,
                StaticTypes.ushortType,
                StaticTypes.intType,
                StaticTypes.uintType,
                StaticTypes.longType,
                StaticTypes.ulongType,
            };
            numericTypes = new HashSet<Type>(integralTypes.Concat(floatingPointTypes));
            IEnumerable<Type> primitiveTypes = new[]
                {
                    StaticTypes.objectType,
                    StaticTypes.voidType,
                    StaticTypes.enumType,
                    StaticTypes.nullableType,
                    StaticTypes.stringType,
                    StaticTypes.charType,
                    StaticTypes.boolType,
                    StaticTypes.dateTimeType,
                    StaticTypes.dateTimeOffsetType,
                    StaticTypes.timeSpanType,
                    StaticTypes.guidType
                }.Concat(numericTypes);
            primitiveTypeSet = new HashSet<Type>(primitiveTypes.Concat(primitiveTypes.Where(t => t.IsValueType && t != StaticTypes.voidType).Select(t => StaticTypes.nullableTypeDefinition.MakeGenericType(t))));
            PrimitiveTypes = new HashSet<Type>(primitiveTypeSet);
        }
        #endregion
        /// <summary>
        /// Types that would be considered primitive in a <see cref="TypeExtensions.IsPrimitive(Type)"/> call.
        /// <para>
        /// Typically:
        /// string, char, bool, object, void, Enum, DateTime, DateTimeOffset, TimeSpan, Guid, Nullable, sbyte, byte, short, ushort, int, uint, long, ulong, float (Single in Visual Basic), double, decimal, and the nullable version of each of precedent types.
        /// </para>
        /// </summary>
        public static HashSet<Type> PrimitiveTypes { get; private set; }
        /// <summary>
        /// Indicates whether the type is primitive. Unlike <see cref="Type.IsPrimitive"/>'s default implementation, returns whether or not the specified type was added to <see cref="PrimitiveTypes"/>. When <paramref name="useDefaultSet"/> is set to true, only the following types will be considered primitive:
        /// <para>
        /// string, char, bool, object, void, Enum, DateTime, DateTimeOffset, TimeSpan, Guid, Nullable, sbyte, byte, short, ushort, int, uint, long, ulong, float (Single in Visual Basic), double, decimal, and the nullable version of each of precedent types.
        /// </para>
        /// </summary>
        public static bool IsPrimitive(this Type type, bool useDefaultSet)
        {
            Error.ThrowIfNull(type, nameof(type));
            return useDefaultSet ? primitiveTypeSet.Contains(type) : PrimitiveTypes.Contains(type);
        }
        /// <summary>
        /// Indicates whether the type is primitive. Unlike <see cref="Type.IsPrimitive"/>'s default implementation, returns whether or not the specified type was added to <see cref="PrimitiveTypes"/>.
        /// </summary>
        public static bool IsPrimitive(this Type type) => type.IsPrimitive(useDefaultSet: false);
        public static HashSet<Type> GetFloatingPointTypes() => floatingPointTypes;
        public static HashSet<Type> GetIntegralTypes() => integralTypes;
        public static HashSet<Type> GetNumericTypes() => numericTypes;
        /// <summary>
        /// Indicates whether or not the specified type is within the specified type-set; while allowing the option to include nullable types in the search.
        /// </summary>
        /// <exception cref="ArgumentNullException"/>
        /// <exception cref="ArgumentOutOfRangeException"/>
        public static bool IsInTypeSet(this Type type, PrimitiveTypeSet primitiveTypeSet, bool includeNullableTypes = true)
        {
            Error.ThrowIfNull(type, nameof(type));
            if (!type.IsPrimitive() && (!includeNullableTypes || (type.IsGenericType && type.GetGenericTypeDefinition() != typeof(Nullable<>))))
                return false;
            switch (primitiveTypeSet)
            {
                case PrimitiveTypeSet.FloatingPointType:
                    return floatingPointTypes.Contains(type) || (includeNullableTypes && floatingPointTypes.Contains(Nullable.GetUnderlyingType(type)));
                case PrimitiveTypeSet.IntegralType:
                    return integralTypes.Contains(type) || (includeNullableTypes && integralTypes.Contains(Nullable.GetUnderlyingType(type)));
                case PrimitiveTypeSet.NumericType:
                    return numericTypes.Contains(type) || (includeNullableTypes && numericTypes.Contains(Nullable.GetUnderlyingType(type)));
                case PrimitiveTypeSet.PrimitiveType:
                    return type.IsPrimitive() || (includeNullableTypes && Nullable.GetUnderlyingType(type).IsPrimitive());
                default:
                    Error.ThrowArgumentOutOfRange(nameof(primitiveTypeSet));
                    return default; //unreachable code
            }
        }
        /// <summary>
        /// Indicates whether or not the specified type is a floating-point type; while allowing the option to include nullable types in the search.
        /// </summary>
        /// <exception cref="ArgumentNullException"/>
        public static bool IsFloatingPointType(this Type type, bool includeNullableTypes = true)
        {
            Error.ThrowIfNull(type, nameof(type));
            return IsInTypeSet(type, PrimitiveTypeSet.FloatingPointType, includeNullableTypes);
        }
        /// <summary>
        /// Indicates whether or not the specified type is an integral type; while allowing the option to include nullable types in the search.
        /// </summary>
        /// <exception cref="ArgumentNullException"/>
        public static bool IsIntegralType(this Type type, bool includeNullableTypes = true)
        {
            Error.ThrowIfNull(type, nameof(type));
            return IsInTypeSet(type, PrimitiveTypeSet.IntegralType, includeNullableTypes);
        }
        /// <summary>
        /// Indicates whether or not the specified type is a numeric type; while allowing the option to include nullable types in the search.
        /// </summary>
        /// <exception cref="ArgumentNullException"/>
        public static bool IsNumericType(this Type type, bool includeNullableTypes = true)
        {
            Error.ThrowIfNull(type, nameof(type));
            return IsInTypeSet(type, PrimitiveTypeSet.NumericType, includeNullableTypes);
        }
        /// <summary>
        /// Gets whether this type (or any of its parents) implements the specified interface type.
        /// </summary>
        /// <exception cref="ArgumentNullException"/>
        /// <exception cref="ArgumentException"/>
        public static bool ImplementsInterface(this Type type, Type interfaceType)
        {
            Error.ThrowIfNull(type, nameof(type));
            Error.ThrowIfNull(interfaceType, nameof(interfaceType));
            Error.ThrowArgumentException(!interfaceType.IsInterface, nameof(interfaceType), "The specified type is not an interface.");
            return interfaceType.IsAssignableFrom(type);
        }
        /// <summary>
        /// Gets the default CLR value for the specified type.
        /// </summary>
        /// <exception cref="ArgumentNullException"/>
        /// <exception cref="ArgumentException"/>
        public static object DefaultValue(this Type type)
        {
            Error.ThrowIfNull(type, nameof(type));
            if (type.IsClass || type.IsInterface)
                return null;

            //Generic structs' generic type definitions; classes and interfaces should've been filtered out by now - considering above condition
            Error.ThrowArgumentException(type.IsGenericType && !type.IsConstructedGenericType, nameof(type), $"Cannot create instance of generic class {type} without the necessary generic parameters.");

            return Activator.CreateInstance(type); //Structs
        }
        /// <summary>
        /// Given a generic type definition of a base (or self) type, returns the corresponding generic base type with constructed generic arguments. Returns null of no such base type found.
        /// </summary>
        /// <exception cref="ArgumentNullException"/>
        /// <exception cref="ArgumentOutOfRangeException"/>
        public static Type FindGenericBase(this Type type, Type genericBaseTypeDefinition)
        {
            Error.ThrowIfNull(type, nameof(type));
            Error.ThrowArgumentOutOfRange(!genericBaseTypeDefinition.IsGenericTypeDefinition, nameof(genericBaseTypeDefinition));
            Type currentType = null;
            while ((currentType = currentType?.BaseType ?? type) != StaticTypes.objectType)
            {
                if (currentType.IsGenericType && currentType.GetGenericTypeDefinition() == genericBaseTypeDefinition)
                    return currentType;
            }
            return null;
        }
        /// <summary>
        /// Given a generic type definition of a base (or self) type, returns generic arguments of the corresponding constructed generic base type. Returns null of no such base type found.
        /// </summary>
        /// <exception cref="ArgumentNullException"/>
        /// <exception cref="ArgumentOutOfRangeException"/>
        public static Type[] FindGenericBaseTypeParameters(this Type type, Type genericBaseTypeDefinition) => type.FindGenericBase(genericBaseTypeDefinition)?.GetGenericArguments();
        /// <summary>
        /// Given a generic type definition of a base (or self) type, returns the corresponding generic base interface type with constructed generic arguments. Returns null of no such base type found.
        /// </summary>
        /// <exception cref="ArgumentNullException"/>
        /// <exception cref="ArgumentOutOfRangeException"/>
        public static Type FindGenericBaseInterface(this Type type, Type genericBaseInterfaceTypeDefinition)
        {
            Error.ThrowIfNull(type, nameof(type));
            Error.ThrowArgumentOutOfRange(!genericBaseInterfaceTypeDefinition.IsGenericTypeDefinition || !genericBaseInterfaceTypeDefinition.IsInterface, nameof(genericBaseInterfaceTypeDefinition));
            foreach (Type genericInterfaceType in type.GetInterfaces().Where(ift => ift.IsGenericType))
            {
                if (genericInterfaceType.GetGenericTypeDefinition() == genericBaseInterfaceTypeDefinition)
                    return genericInterfaceType;
            }
            return null;
        }
        /// <summary>
        /// Given a generic type definition of a base (or self) type, returns generic arguments of the corresponding constructed generic base interface type. Returns null of no such base type found.
        /// </summary>
        /// <exception cref="ArgumentNullException"/>
        /// <exception cref="ArgumentOutOfRangeException"/>
        public static Type[] FindGenericBaseInterfaceTypeParameters(this Type type, Type genericBaseInterfaceTypeDefinition) => type.FindGenericBaseInterface(genericBaseInterfaceTypeDefinition)?.GetGenericArguments();
        /// <summary>
        /// Returns the C# alias for the specified type if it has one; otherwise, returns original type name. Could be useful in code generation.
        /// </summary>
        public static string GetTypeNameOrCSharpAlias(this Type type)
        {
            Error.ThrowIfNull(type, nameof(type));
            if (type == StaticTypes.objectType)
                return "object";
            else if (type == StaticTypes.stringType)
                return "string";
            else if (type == StaticTypes.charType)
                return "char";
            else if (type == StaticTypes.boolType)
                return "bool";
            else if (type == StaticTypes.sbyteType)
                return "sbyte";
            else if (type == StaticTypes.byteType)
                return "byte";
            else if (type == StaticTypes.shortType)
                return "short";
            else if (type == StaticTypes.ushortType)
                return "ushort";
            else if (type == StaticTypes.intType)
                return "int";
            else if (type == StaticTypes.uintType)
                return "uint";
            else if (type == StaticTypes.longType)
                return "long";
            else if (type == StaticTypes.ulongType)
                return "ulong";
            else if (type == StaticTypes.floatType)
                return "float";
            else if (type == StaticTypes.doubleType)
                return "double";
            else if (type == StaticTypes.decimalType)
                return "decimal";
            else if (type == StaticTypes.voidType)
                return "void";
            else if (type.ImplementsInterface(StaticTypes.idynamicMetaObjectProvider))
                return "dynamic";
            else
                return type.Name;
        }
    }
}
