using System.Diagnostics.CodeAnalysis;

namespace StaticDataLibrary.RecordLibrary;

public class PropertyAttributeFinder
{
    public static void ValidateSingle<T>(TableInfo tableInfo)
        where T : Attribute
    {
        var propertyAttributeList = tableInfo.RecordType
            .GetProperties()
            .Select(x => x.GetCustomAttributesData())
            .ToList();

        var exists = false;
        foreach (var property in propertyAttributeList)
        {
            var hasAttribute = property.Any(x => x.AttributeType == typeof(T));
            if (!hasAttribute)
            {
                continue;
            }

            if (!exists)
            {
                exists = true;
            }
            else
            {
                throw new InvalidOperationException($"{tableInfo.RecordType.Name}에 {typeof(T).Name} 없거나 두 개 이상 있습니다.");
            }
        }

        if (!exists)
        {
            throw new InvalidOperationException($"{tableInfo.RecordType.Name}에 {typeof(T).Name} 없거나 두 개 이상 있습니다.");
        }
    }

    public static int Count<T>(TableInfo tableInfo)
        where T : Attribute
    {
        var propertyAttributeList = tableInfo.RecordType
            .GetProperties()
            .Select(x => x.GetCustomAttributesData())
            .ToList();

        var total = 0;
        foreach (var property in propertyAttributeList)
        {
            total += property.Count(x => x.AttributeType == typeof(T));
        }

        return total;
    }

    public static T? Find<T>(TableInfo tableInfo, string propertyName)
        where T : Attribute
    {
        var propertyInfo = tableInfo.RecordType
            .GetProperties()
            .Single(x => x.Name == propertyName);

        return propertyInfo.GetCustomAttributes(typeof(T), false).SingleOrDefault() as T;
    }
}
