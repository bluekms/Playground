using System.Collections.ObjectModel;
using System.Reflection;

namespace StaticDataLibrary.RecordLibrary;

public static class PropertyInfoExtension
{
    public static bool IsNullable(this PropertyInfo propertyInfo)
    {
        if (propertyInfo.PropertyType.IsValueType)
            return Nullable.GetUnderlyingType(propertyInfo.PropertyType) != null;

        var nullable = propertyInfo.CustomAttributes
            .FirstOrDefault(x => x.AttributeType.FullName == "System.Runtime.CompilerServices.NullableAttribute");
        
        if (nullable != null && nullable.ConstructorArguments.Count == 1)
        {
            var attributeArgument = nullable.ConstructorArguments[0];
            if (attributeArgument.ArgumentType == typeof(byte[]))
            {
                var args = (ReadOnlyCollection<CustomAttributeTypedArgument>)attributeArgument.Value!;
                if (args.Count > 0 && args[0].ArgumentType == typeof(byte))
                {
                    return (byte)args[0].Value! == 2;
                }
            }
            else if (attributeArgument.ArgumentType == typeof(byte))
            {
                return (byte)attributeArgument.Value! == 2;
            }
        }

        for (var type = propertyInfo.DeclaringType; type != null; type = type.DeclaringType)
        {
            var context = type.CustomAttributes
                .FirstOrDefault(x => x.AttributeType.FullName == "System.Runtime.CompilerServices.NullableContextAttribute");
            
            if (context != null &&
                context.ConstructorArguments.Count == 1 &&
                context.ConstructorArguments[0].ArgumentType == typeof(byte))
            {
                return (byte)context.ConstructorArguments[0].Value! == 2;
            }
        }

        return false;
    }
}