using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

using Fiche.Misc;

namespace Fiche.Extensions
{
    public static class ObjectExtensions
    {
        /// <summary>
        /// Determines whether the specified object is null.
        /// </summary>
        public static bool IsNull<T>(this T arg)
        {
#pragma warning disable IDE0041 // Use 'is null' check
            if (ReferenceEquals(null, arg))
                return true;
#pragma warning restore IDE0041 // Use 'is null' check
            Type t = typeof(T);
            if (t.IsGenericType && ((t.IsGenericTypeDefinition && t.Equals(StaticTypes.nullableTypeDefinition)) || t.GetGenericTypeDefinition().Equals(StaticTypes.nullableTypeDefinition)))
            {
                PropertyInfo hasValueProperty = t.GetProperty("HasValue");
                return !(bool)hasValueProperty.GetValue(arg);
            }
            if (NullTypes.Any(type => !(type is null) && t.IsAssignableFrom(type)))
                return true;
            if (NullValues.Any(value => ReferenceEquals(value, arg) || arg.Equals(value) || (value is IEquatable<T> equatable && equatable.Equals(arg))))
                return true;
            return false;
        }
        /// <summary>
        /// Unique set of types that will be considered null with <see cref="IsNull{T}(T)"/> if any of them matched the type of the argument passed.
        /// <para>For instance, <see cref="System.Data.SqlTypes.INullable"/> could well fit in that list.</para>
        /// </summary>
        public static HashSet<Type> NullTypes { get; } = new HashSet<Type>();
        /// <summary>
        /// Unique set of values that will be considered null with <see cref="IsNull{T}(T)"/> if they matched the argument passed.
        /// <para>For instance, <see cref="DBNull.Value"/> could well fit in that list.</para>
        /// </summary>
        public static HashSet<object> NullValues { get; } = new HashSet<object>();
        /// <summary>
        /// Executes the specified action with the specified object as its parameter.
        /// </summary>
        /// <exception cref="ArgumentNullException"/>
        public static void Act<T>(this T obj, Action<T> action)
        {
            Error.ThrowIfNull(action, nameof(action));
            action(obj);
        }
        /// <summary>
        /// Returns the value of the specified function when executed with the specified object as its parameter.
        /// </summary>
        /// <exception cref="ArgumentNullException"/>
        public static TResult Project<T, TResult>(this T obj, Func<T, TResult> func)
        {
            Error.ThrowIfNull(func, nameof(func));
            return func(obj);
        }
        /// <summary>
        /// Uses the specified equality comparer to determine whether the specified object is equal to any of the elements given.
        /// </summary>
        /// <exception cref="ArgumentNullException"/>
        public static bool In<T>(this T obj, IEnumerable<T> values, IEqualityComparer<T> equalityComparer)
        {
            Error.ThrowIfNull(obj, nameof(obj));
            if (equalityComparer is null)
                equalityComparer = EqualityComparer<T>.Default;
            return (values ?? Enumerable.Empty<T>()).Any(val => equalityComparer.Equals(obj, val));
        }
        /// <summary>
        /// Uses the default equality comparer to determine whether the specified object is equal to any of the elements given.
        /// </summary>
        /// <exception cref="ArgumentNullException"/>
        public static bool In<T>(this T obj, IEnumerable<T> values) => obj.In(values, EqualityComparer<T>.Default);
        /// <summary>
        /// Uses the default equality comparer to determine whether the specified object is equal to any of the elements given.
        /// </summary>
        /// <exception cref="ArgumentNullException"/>
        public static bool In<T>(this T obj, params T[] values) => obj.In(values?.AsEnumerable() ?? Enumerable.Empty<T>());
        #region Deep clone
        private static readonly MethodInfo cloneMethod = StaticTypes.objectType.GetMethod("MemberwiseClone", BindingFlags.NonPublic | BindingFlags.Instance);
        /// <summary>
        /// Creates and returns a deep-copy of the specified object.
        /// </summary>
        public static T DeepClone<T>(this T obj) => Copy(obj, null, string.Empty, null);
        /// <summary>
        /// Creates and returns a deep-copy of the specified object while setting the value of the member specified by the expression to the value specified.
        /// <para>
        /// This is useful while setting nested struct values (Struct1.Struct2.Property = value). This also is an implementation for the setter part of a 'Lens' in functional programming. Beware that even if an expression to a read-only property/field was specified, the corresponding value will be changed anyway. Properties that contain logic in their getters/setters without a backing-field will be ignored even if specified in an expression. Null expressions are ignored.
        /// </para>
        /// </summary>
        public static T Copy<T, TValue>(this T obj, Expression<Func<T, TValue>> target, TValue value) => Copy(obj, null, string.Empty, target is null ? null : new Dictionary<string, object>() { { ExpressionPath(target), value } });
        /// <summary>
        /// Creates and returns a deep-copy of the specified object while setting the value of the member specified by each expression to its specified value.
        /// <para>
        /// This is useful while setting nested struct values (Struct1.Struct2.Property = value). This also is an implementation for the setter part of a 'Lens' in functional programming. Beware that even if an expression to a read-only property/field was specified, the corresponding value will be changed anyway. Properties that contain logic in their getters/setters without a backing-field will be ignored even if specified in an expression. Null expressions are ignored.
        /// </para>
        /// </summary>
        public static T Copy<T>(this T obj, IDictionary<Expression<Func<T, object>>, object> targetValues) => Copy(obj, null, string.Empty, targetValues?.Where(kvp => kvp.Key != null)?.ToDictionary(kvp => ExpressionPath(kvp.Key), kvp => kvp.Value));
        private static T Copy<T>(T obj, IDictionary<object, object> visited, string currentPath, IDictionary<string, object> targetPathValues)
        {
            if (obj == null) return default;
            Type typeToReflect = obj.GetType();
            //if (StaticTypes.delegateType.IsAssignableFrom(typeToReflect)) return default;
            if (!typeToReflect.IsClass && typeToReflect.IsPrimitive()) return obj;
            if (typeToReflect.IsClass)
            {
                if (visited is null)
                    visited = new Dictionary<object, object>(new ReferenceEqualityComparer());
                else if (visited.ContainsKey(obj))
                    return (T)visited[obj];
            }
            T cloneObject = (T)cloneMethod.Invoke(obj, null);
            if (StaticTypes.stringType.Equals(typeToReflect)) return cloneObject;
            if (typeToReflect.IsArray)
            {
                Type arrayType = typeToReflect.GetElementType();
                Array clonedArray = cloneObject as Array;
                clonedArray.ForEach((array, indices) => array.SetValue(Copy(clonedArray.GetValue(indices), visited, currentPath, targetPathValues), indices));
            }
            if (typeToReflect.IsClass)
                visited.Add(obj, cloneObject);
            CopyFields(ref obj, visited, currentPath, targetPathValues, ref cloneObject, typeToReflect);
            RecursiveCopyBaseTypePrivateFields(ref obj, visited, currentPath, targetPathValues, ref cloneObject, typeToReflect);
            return cloneObject;
        }
        private static void RecursiveCopyBaseTypePrivateFields<T>(ref T obj, IDictionary<object, object> visited, string currentPath, IDictionary<string, object> targetPathValues, ref T cloned, Type typeToReflect)
        {
            if (typeToReflect.BaseType != null)
            {
                if (!string.Empty.Equals(currentPath))
                    currentPath += '.';
                currentPath += "base";
                RecursiveCopyBaseTypePrivateFields(ref obj, visited, currentPath, targetPathValues, ref cloned, typeToReflect.BaseType);
                CopyFields(ref obj, visited, currentPath, targetPathValues, ref cloned, typeToReflect.BaseType, BindingFlags.Instance | BindingFlags.NonPublic, info => info.IsPrivate);
            }
        }
        private static void CopyFields<T>(ref T obj, IDictionary<object, object> visited, string currentPath, IDictionary<string, object> targetPathValues, ref T cloned, Type typeToReflect, BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.FlattenHierarchy, Func<FieldInfo, bool> filter = null)
        {
            string originalCurrentPath = currentPath;
            foreach (FieldInfo fieldInfo in typeToReflect.GetFields(bindingFlags))
            {
                if (!originalCurrentPath.Equals(currentPath))
                    currentPath = originalCurrentPath;
                if (!string.Empty.Equals(currentPath))
                    currentPath += '.';
                currentPath += PropertyNameIfBackingField(fieldInfo);
                //string comparablePath = ComparablePath(currentPath);
                if (targetPathValues != null && targetPathValues.Keys.SingleOrDefault(path => currentPath.Equals(path)) is string targetPath)
                {
                    if (typeToReflect.IsClass)
                        fieldInfo.SetValue(cloned, targetPathValues[targetPath]);
                    else
                        fieldInfo.SetValueDirect(__makeref(cloned), targetPathValues[targetPath]);
                }
                else if (filter != null && filter(fieldInfo) == false)
                { continue; }
                else if (fieldInfo.FieldType.IsClass || !fieldInfo.FieldType.IsPrimitive())
                {
                    object originalFieldValue = fieldInfo.GetValue(obj);
                    object clonedFieldValue = Copy(originalFieldValue, visited, currentPath, targetPathValues);
                    if (typeToReflect.IsClass)
                        fieldInfo.SetValue(cloned, clonedFieldValue);
                    else
                        fieldInfo.SetValueDirect(__makeref(cloned), clonedFieldValue);
                }
            }
        }
        private static string PropertyNameIfBackingField(FieldInfo fieldInfo)
        {
            string finalName = fieldInfo.Name.Replace("<", string.Empty).Replace(">k__BackingField", string.Empty);
            if (finalName.Length == fieldInfo.Name.Length - 17)
            {
                int lastDotIndex = finalName.LastIndexOf('.');
                if (lastDotIndex > -1)
                    finalName = finalName.Substring(lastDotIndex + 1);
                return finalName;
            }
            return fieldInfo.Name;
        }
        private static string ComparablePath(string path)
        {
            for (int i = path.IndexOf('<'); i > -1; i = path.IndexOf('<'))
            {
                int closingTag = path.IndexOf('>');
                string propFullName = path.Substring(i + 1, closingTag - i - 1); //includes namespace and interface name in interface explicitly implemented auto-properties
                int lastDotIndex = propFullName.LastIndexOf('.');
                if (lastDotIndex > -1)
                    propFullName = propFullName.Substring(lastDotIndex + 1);
                path = $"{path.Substring(0, i)}{propFullName}{path.Substring(path.IndexOf(">k__BackingField") + 16)}"; //"k__BackingField"'s length is 16
            }
            return path;
        }
        private static string ExpressionPath<T, TResult>(Expression<Func<T, TResult>> expression)
        {
            if (expression.Body.NodeType == ExpressionType.Parameter)
                return string.Empty;
            MemberExpression me = expression.Body.DiscardUnaryExpressions<MemberExpression>();
            string currentPath = me?.Member?.Name;
            MemberExpression lastExpression = me;
            while ((me = me?.Expression?.DiscardUnaryExpressions<MemberExpression>()) != null)
            {
                currentPath = $"{me.Member.Name}.{currentPath}";
                lastExpression = me;
            }
            Error.ThrowArgumentException(lastExpression?.Expression?.DiscardUnaryExpressions<ParameterExpression>() is null,
                nameof(expression),
                $"The body of the specified lambda expression '{expression}' does not constitute a valid expression tree of '{typeof(MemberExpression)}'s.");
            return currentPath;
        }
        //private static void RecursiveCopyBaseTypePrivateFields(object originalObject, IDictionary<object, object> visited, object cloneObject, Type typeToReflect)
        //{
        //    if (typeToReflect.BaseType != null)
        //    {
        //        RecursiveCopyBaseTypePrivateFields(originalObject, visited, cloneObject, typeToReflect.BaseType);
        //        CopyFields(originalObject, visited, cloneObject, typeToReflect.BaseType, BindingFlags.Instance | BindingFlags.NonPublic, info => info.IsPrivate);
        //    }
        //}
        //private static void CopyFields(object originalObject, IDictionary<object, object> visited, object cloneObject, Type typeToReflect, BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.FlattenHierarchy, Func<FieldInfo, bool> filter = null)
        //{
        //    foreach (FieldInfo fieldInfo in typeToReflect.GetFields(bindingFlags))
        //    {
        //        if (filter != null && filter(fieldInfo) == false) continue;
        //        if (!fieldInfo.FieldType.IsClass && fieldInfo.FieldType.IsPrimitive()) continue;
        //        object originalFieldValue = fieldInfo.GetValue(originalObject);
        //        object clonedFieldValue = InternalCopy(originalFieldValue, visited);
        //        fieldInfo.SetValue(cloneObject, clonedFieldValue);
        //    }
        //}
        private class ReferenceEqualityComparer : EqualityComparer<object>
        {
            public override bool Equals(object x, object y) => ReferenceEquals(x, y);
            public override int GetHashCode(object obj)
            {
                if (obj == null) return 0;
                return obj.GetHashCode();
            }
        }
        private static void ForEach(this Array array, Action<Array, int[]> action)
        {
            if (array.LongLength == 0) return;
            ArrayTraverse walker = new ArrayTraverse(array);
            do action(array, walker.Position);
            while (walker.Step());
        }
        private class ArrayTraverse
        {
            public int[] Position;
            private readonly int[] _maxLengths;

            public ArrayTraverse(Array array)
            {
                this._maxLengths = new int[array.Rank];
                for (int i = 0; i < array.Rank; ++i)
                {
                    this._maxLengths[i] = array.GetLength(i) - 1;
                }
                this.Position = new int[array.Rank];
            }

            public bool Step()
            {
                for (int i = 0; i < this.Position.Length; ++i)
                {
                    if (this.Position[i] < this._maxLengths[i])
                    {
                        this.Position[i]++;
                        for (int j = 0; j < i; j++)
                        {
                            this.Position[j] = 0;
                        }
                        return true;
                    }
                }
                return false;
            }
        }
        #endregion
        /// <summary>
        /// Returns a sequence with the specified object as its sole element.
        /// </summary>
        public static IEnumerable<T> IterateSelf<T>(this T obj) { yield return obj; }
        /// <summary>
        /// If the object was a sequence, it is returned; otherwise, returns a sequence with the specified object as its sole element.
        /// </summary>
        public static IEnumerable<T> Iterate<T>(this T obj)
        {
            if (obj is IEnumerable<T> enumerable)
            {
                foreach (T item in enumerable)
                    yield return item;
            }
            else
            {
                yield return obj;
            }
        }
        /// <summary>
        /// Uses reflection and recursion down to object's primitive-type fields to determine whether two objects are equal to each other.
        /// </summary>
        public static bool DeepEquals<T>(this T obj, T sbj) => InternalDeepEquals(typeof(T), obj, sbj);
        private static bool InternalDeepEquals<T>(Type type, T obj, T sbj) //Generic, trying to avoid boxing/unboxing as much as possible
        {
            if (ReferenceEquals(obj, sbj))
                return true;
#pragma warning disable IDE0041 // Use 'is null' check
            if (ReferenceEquals(obj, null) ^ ReferenceEquals(sbj, null))
                return false;
#pragma warning restore IDE0041 // Use 'is null' check
            if (type == StaticTypes.objectType)
                type = obj.GetType();
            if (type.IsPrimitive())
                return obj.Equals(sbj);
            if ((type = obj.GetType()) != sbj.GetType()) //In case T is the base type of the actual given object
                return false;
            if (type.ImplementsInterface(typeof(IEnumerable)))
            {
                bool objEnumeratorFailed = false, sbjEnumeratorFailed = false;
                IEnumerator eObj = null;
                IEnumerator eSbj = null;
                try { eObj = (obj as IEnumerable).GetEnumerator(); } catch { objEnumeratorFailed = true; }
                try { eSbj = (sbj as IEnumerable).GetEnumerator(); }
                catch
                {
                    sbjEnumeratorFailed = true;
                    if (eObj is IDisposable disposable)
                        disposable.Dispose();
                }
                if (objEnumeratorFailed ^ sbjEnumeratorFailed)
                    return false;
                else if (objEnumeratorFailed)
                    return true;
                bool nextObj, nextSbj;
                //Bitwise and is intended here - to evaluate both iterators
                while ((nextObj = eObj.MoveNext()) & (nextSbj = eSbj.MoveNext()))
                {
                    Type tObj = eObj.Current?.GetType(), tSbj = eSbj.Current?.GetType();
                    if (!(tObj == tSbj) || !InternalDeepEquals(tObj, eObj.Current, eSbj.Current))
                        return false;
                }
                if (nextObj ^ nextSbj)
                    return false;
            }
            else
            {
                foreach (FieldInfo field in type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
                {
                    if (!InternalDeepEquals(field.FieldType, field.GetValue(obj), field.GetValue(sbj)))
                        return false;
                }
            }
            return true;
        }
        /// <summary>
        /// Allows overriding the default implementation of the <see cref="ObjectExtensions.ExposingToString(object)"/> extension.
        /// </summary>
        public static Func<object, string> ExposingToStringImplementation { get; set; } = DefaultExposingToString;
        private static string DefaultExposingToString(object obj)
        {
            Error.ThrowIfNull(obj, nameof(obj));
            Type type = obj.GetType();
            if (type == typeof(string) || (type.IsValueType & type.IsPrimitive()))
                return obj.ToString();
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"{type.Name} {{");
            foreach (PropertyInfo prop in type.GetProperties())
                sb.AppendLine($"\t\"{prop.Name}\": \"{prop.GetValue(obj)?.ToString()}\"");
            sb.AppendLine("}");
            return sb.ToString();
        }
        /// <summary>
        /// Returns the specified object in the form of a JSON-like string using reflection - in its default implementation. Only public properties are included.
        /// <para>
        /// Implementation of this method could be overridden through <see cref="ObjectExtensions.ExposingToStringImplementation"/>. If <see cref="ObjectExtensions.ExposingToStringImplementation"/> is set to null, the default implementation is then used anyway.
        /// </para>
        /// </summary>
        public static string ExposingToString(this object obj)
            => ExposingToStringImplementation is null ? DefaultExposingToString(obj) : ExposingToStringImplementation(obj);
    }
}
