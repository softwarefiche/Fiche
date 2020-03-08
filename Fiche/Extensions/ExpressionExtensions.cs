using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

using Fiche.Misc;

namespace Fiche.Extensions
{
    public static class ExpressionExtensions
    {
        /// <summary>
        /// Extracts the <see cref="MemberInfo"/> out of the specified <see cref="LambdaExpression"/>.
        /// </summary>
        public static MemberInfo GetMember<TIn, TOut>(this Expression<Func<TIn, TOut>> expression)
        {
            Error.ThrowIfNull(expression);
            MemberExpression me = expression.Body as MemberExpression;
            Expression currentExpression = null;
            while ((currentExpression = (currentExpression as MemberExpression).Expression ?? me) is MemberExpression) ;
            if (currentExpression is ParameterExpression)
                return me.Member;
            Error.ThrowInvalidOperation("The specified expression does not constitute a member of the given parameter expression tree.");
            return default; //unreachable code;
        }
        private static TMember GetMember<TMember, TIn, TOut>(this Expression<Func<TIn, TOut>> expression) where TMember : MemberInfo => GetMember(expression) as TMember;
        /// <summary>
        /// Extracts the <see cref="PropertyInfo"/> out of the specified <see cref="LambdaExpression"/>.
        /// </summary>
        public static PropertyInfo GetProperty<TIn, TOut>(this Expression<Func<TIn, TOut>> expression) => GetMember<PropertyInfo, TIn, TOut>(expression);
        /// <summary>
        /// Extracts the <see cref="FieldInfo"/> out of the specified <see cref="LambdaExpression"/>.
        /// </summary>
        public static FieldInfo GetField<TIn, TOut>(this Expression<Func<TIn, TOut>> expression) => GetMember<FieldInfo, TIn, TOut>(expression);
        /// <summary>
        /// Extracts the <see cref="MemberInfo"/> out of the specified <see cref="LambdaExpression"/>.
        /// </summary>
        public static MemberInfo GetMember(this LambdaExpression source)
        {
            MemberExpression me = source?.Body?.DiscardUnaryExpressions<MemberExpression>();
            return me?.Member as MemberInfo;
        }
        private static TMember GetMember<TMember>(this LambdaExpression source) where TMember : MemberInfo => GetMember(source) as TMember;
        /// <summary>
        /// Extracts the <see cref="PropertyInfo"/> out of the specified <see cref="LambdaExpression"/>.
        /// </summary>
        public static PropertyInfo GetProperty(this LambdaExpression source) => GetMember<PropertyInfo>(source);
        /// <summary>
        /// Extracts the <see cref="FieldInfo"/> out of the specified <see cref="LambdaExpression"/>.
        /// </summary>
        public static FieldInfo GetField(this LambdaExpression source) => GetMember<FieldInfo>(source);
        /// <summary>
        /// Gets the <see cref="ConstantExpression"/> value out of the specified <see cref="LambdaExpression"/> using the specified source object.
        /// </summary>
        public static object GetValue<T>(this LambdaExpression source, T src)
        {
            ConstantExpression constExp = source?.Body?.DiscardUnaryExpressions<ConstantExpression>();
            if (constExp != null)
                return constExp.Value;
            MemberExpression me = source?.Body?.DiscardUnaryExpressions<MemberExpression>();
            List<MemberInfo> memberPath = new List<MemberInfo>();
            MemberInfo currentMember;
            MemberExpression lastMe = me;
            while ((currentMember = me?.Member as MemberInfo) != null)
            {
                memberPath.Add(currentMember);
                lastMe = me;
                me = me.Expression?.DiscardUnaryExpressions<MemberExpression>();
            }
            constExp = lastMe?.Expression?.DiscardUnaryExpressions<ConstantExpression>();
            object currentObj;
            if (constExp == null)
                currentObj = src;
            else
                currentObj = constExp.GetValue(me); //constExp.Value.GetType().InvokeMember(me.Member.Name, BindingFlags.GetField, null, constExp.Value, null);
            try
            {
                for (int x = memberPath.Count - 1; x >= 0; --x)
                {
                    if (memberPath[x] is PropertyInfo property)
                        currentObj = property.GetValue(currentObj);
                    else if (memberPath[x] is FieldInfo field)
                    {
                        currentObj = field.GetValue(currentObj);
                    }
                    else if (memberPath[x] is MethodInfo method)
                    {
                        currentObj = method.Invoke(currentObj, null);
                    }
                    else
                    {
                        ConstructorInfo constructor = memberPath[x] as ConstructorInfo;
                        currentObj = constructor.Invoke(null);
                    }
                }
            }
            catch //Exception is either type mismatch because of invalid expression, or argument null because src is null
            {
                return currentObj;
            }
            return currentObj;
        }
        /// <summary>
        /// Gets the <see cref="ConstantExpression"/> value out of the specified <see cref="LambdaExpression"/>.
        /// </summary>
        public static object GetValue(this LambdaExpression source)
        {
            ConstantExpression constExp = source?.Body?.DiscardUnaryExpressions<ConstantExpression>();
            if (constExp != null)
                return constExp.Value;
            MemberExpression me = source?.Body?.DiscardUnaryExpressions<MemberExpression>();
            List<MemberInfo> memberPath = new List<MemberInfo>();
            MemberInfo currentMember;
            MemberExpression lastMe = me;
            while ((currentMember = me?.Member as MemberInfo) != null)
            {
                memberPath.Add(currentMember);
                lastMe = me;
                me = me.Expression?.DiscardUnaryExpressions<MemberExpression>();
            }
            constExp = lastMe?.Expression?.DiscardUnaryExpressions<ConstantExpression>();
            if (constExp == null)
                return null;
            object currentObj = constExp.GetValue(); ;
            try
            {
                for (int x = memberPath.Count - 1; x >= 0; --x)
                {
                    if (memberPath[x] is PropertyInfo property)
                        currentObj = property.GetValue(currentObj);
                    else if (memberPath[x] is FieldInfo field)
                    {
                        currentObj = field.GetValue(currentObj);
                    }
                    else if (memberPath[x] is MethodInfo method)
                    {
                        currentObj = method.Invoke(currentObj, null);
                    }
                    else
                    {
                        ConstructorInfo constructor = memberPath[x] as ConstructorInfo;
                        currentObj = constructor.Invoke(null);
                    }
                }
            }
            catch //Exception is either type mismatch because of invalid expression, or argument null because src is null
            {
                return currentObj;
            }
            return currentObj;
        }
        /// <summary>
        /// Discards any <see cref="UnaryExpression"/>s wraps the current expression and tries to return the wrapped expression in the form of the given expression type parameter.
        /// </summary>
        public static TExpression DiscardUnaryExpressions<TExpression>(this Expression source) where TExpression : Expression
        {
            Expression lastExp = source;
            while ((source = ((source as UnaryExpression)?.Operand)) != null)
                lastExp = source;
            return lastExp as TExpression;
        }
        private static object GetValue(this ConstantExpression source, MemberExpression member)
        {
            if (member == null)
                return source.Value;
            return source?.Value?.GetType()?.InvokeMember(member.Member.Name, BindingFlags.GetField | BindingFlags.GetProperty | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, source.Value, null);
        }
        private static object GetValue(this ConstantExpression source) => GetValue(source, null);
    }
}
