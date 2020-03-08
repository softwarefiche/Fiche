using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

using Fiche.Misc;

namespace Fiche.Extensions
{
    public static class MethodInfoExtensions
    {
        /// <summary>
        /// Creates a delegate out of the specified <see cref="MethodInfo"/>.
        /// </summary>
        /// <param name="target">the delegate target. null for static methods.</param>
        /// <exception cref="ArgumentNullException"/>
        /// <exception cref="ArgumentException"/>
        /// <exception cref="MissingMethodException"/>
        /// <exception cref="MethodAccessException"/>
        public static Delegate CreateDelegate(this MethodInfo methodInfo, object target)
        {
            Error.ThrowIfNull(methodInfo, nameof(methodInfo));
            Func<Type[], Type> getType;
            bool isAction = methodInfo.ReturnType.Equals(StaticTypes.voidType);
            IEnumerable<Type> types = methodInfo.GetParameters().Select(p => p.ParameterType);

            if (isAction)
            {
                getType = Expression.GetActionType;
            }
            else
            {
                getType = Expression.GetFuncType;
                types = types.Concat(new[] { methodInfo.ReturnType });
            }

            if (methodInfo.IsStatic)
                return Delegate.CreateDelegate(getType(types.ToArray()), methodInfo);

            return Delegate.CreateDelegate(getType(types.ToArray()), target, methodInfo.Name);
        }
        /// <summary>
        /// Creates a delegate out of the specified static <see cref="MethodInfo"/>.
        /// </summary>
        /// <exception cref="ArgumentNullException"/>
        /// <exception cref="ArgumentException"/>
        /// <exception cref="MissingMethodException"/>
        /// <exception cref="MethodAccessException"/>
        public static Delegate CreateDelegate(this MethodInfo methodInfo) => CreateDelegate(methodInfo, null); //static methods
    }
}
