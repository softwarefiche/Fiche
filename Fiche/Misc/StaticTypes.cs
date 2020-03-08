using System;

namespace Fiche.Misc
{
    internal static class StaticTypes
    {
        internal static readonly Type voidType = typeof(void);
        internal static readonly Type enumType = typeof(Enum);
        internal static readonly Type objectType = typeof(object);
        internal static readonly Type nullableType = typeof(Nullable);
        internal static readonly Type nullableTypeDefinition = typeof(Nullable<>);
        internal static readonly Type taskType = typeof(System.Threading.Tasks.Task);

        internal static readonly Type stringType = typeof(string);
        internal static readonly Type charType = typeof(char);
        internal static readonly Type boolType = typeof(bool);
        internal static readonly Type sbyteType = typeof(sbyte);
        internal static readonly Type byteType = typeof(byte);
        internal static readonly Type shortType = typeof(short);
        internal static readonly Type ushortType = typeof(ushort);
        internal static readonly Type intType = typeof(int);
        internal static readonly Type uintType = typeof(uint);
        internal static readonly Type longType = typeof(long);
        internal static readonly Type ulongType = typeof(ulong);
        internal static readonly Type floatType = typeof(float);
        internal static readonly Type doubleType = typeof(double);
        internal static readonly Type decimalType = typeof(decimal);

        internal static readonly Type guidType = typeof(Guid);
        internal static readonly Type dateTimeType = typeof(DateTime);
        internal static readonly Type dateTimeOffsetType = typeof(DateTimeOffset);
        internal static readonly Type timeSpanType = typeof(TimeSpan);

        internal static readonly Type delegateType = typeof(Delegate);

        internal static readonly Type idynamicMetaObjectProvider = typeof(System.Dynamic.IDynamicMetaObjectProvider);

        internal static readonly Type ienumerableType = typeof(System.Collections.IEnumerable);
        internal static readonly Type ienumerableTypeDefinition = typeof(System.Collections.Generic.IEnumerable<>);

    }
}
