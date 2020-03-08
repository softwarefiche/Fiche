using System.Linq;
using System.Reflection;

using Fiche.Misc;

namespace Fiche.Extensions
{
    public static class PropertyInfoExtensions
    {
        /// <summary>
        /// Indicates whether the specified property is an auto-property in C#.
        /// </summary>
        public static bool IsAutoProperty(this PropertyInfo pi) => !(pi.GetBackingFieldOrDefault() is null);
        /// <summary>
        /// Returns the backing-field for the specified C# auto-property.
        /// </summary>
        public static FieldInfo GetBackingField(this PropertyInfo pi)
        {
            if (pi.GetBackingFieldOrDefault() is FieldInfo fieldInfo)
                return fieldInfo;
            Error.ThrowInvalidOperation($"The specified {typeof(PropertyInfo)} is not an auto-property and hence not eligible to get a backing-field from.");
            return default; //unreachable code
        }
        /// <summary>
        /// If a C# auto-property, returns the backing-field of that property; otherwise, returns null.
        /// </summary>
        public static FieldInfo GetBackingFieldOrDefault(this PropertyInfo pi) => pi.DeclaringType.GetFields(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static)
                             .SingleOrDefault(f => f.Name.Equals($"<{pi.Name}>k__BackingField"));
    }
}
