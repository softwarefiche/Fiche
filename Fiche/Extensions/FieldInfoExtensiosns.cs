using System.Reflection;

namespace Fiche.Extensions
{
    public static class FieldInfoExtensiosns
    {
        public static bool IsBackingField(this FieldInfo fieldInfo)
            => fieldInfo.Name.Contains(">k__BackingField"); // && fieldInfo.Name.Replace("<", string.Empty).Replace(">k__BackingField", string.Empty).Length == fieldInfo.Name.Length - 17;
    }
}
