using System.Reflection;

namespace StaticDataLibrary.RecordLibrary;

public static class PropertyInfoExtension
{
    public static bool IsNullable(this PropertyInfo propertyInfo)
    {
        return propertyInfo.PropertyType.IsGenericType &&
               propertyInfo.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>);
    }
}