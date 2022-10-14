namespace StaticDataLibrary.RecordLibrary;

public static class PropertyAttributeFinder
{
    public static void Single<T>(TableInfo tableInfo) where T : Attribute
    {
        var propertyAttributeList = tableInfo.RecordType
            .GetProperties()
            .Select(x => x.GetCustomAttributesData())
            .ToList();

        var alreadyExists = false;
        foreach (var property in propertyAttributeList)
        {
            var hasAttribute = property.Any(x => x.AttributeType == typeof(T));
            if (!hasAttribute)
            {
                continue;
            }
            else
            {
                if (!alreadyExists)
                {
                    alreadyExists = true;
                }
                else
                {
                    throw new InvalidOperationException($"{tableInfo.RecordType.Name}에 {typeof(T).Name} 없거나 두 개 이상 있습니다.");
                }
            }
        }
    }

    public static int Count<T>(TableInfo tableInfo) where T : Attribute
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

    public static T? Find<T>(TableInfo tableInfo, string propertyName) where T : Attribute
    {
        var propertyInfo = tableInfo.RecordType
            .GetProperties()
            .Single(x => x.Name == propertyName);

        return propertyInfo.GetCustomAttributes(typeof(T), false).SingleOrDefault() as T;
    }
}