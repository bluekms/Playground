using System.Reflection;
using CsvHelper.Configuration.Attributes;

namespace StaticDataLibrary;

public sealed class RecordMapper
{
    public static object? Map(Type t, string csvLine)
    {
        if (string.IsNullOrWhiteSpace(csvLine))
        {
            return null;
        }

        var properties = GetProperties(t);
        var values = csvLine.Split(',');
        if (properties.Count != values.Length)
        {
            throw new ArgumentOutOfRangeException($"{t.Name}'s Property: {properties.Count}. but csv arguments: {values.Length}");
        }

        var instance = Activator.CreateInstance(t);
        for (int i = 0; i < properties.Count; ++i)
        {
            var isNullable = properties[i].PropertyType.IsGenericType &&
                             properties[i].PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>);

            if (isNullable && string.IsNullOrWhiteSpace(values[i]))
            {
                properties[i].SetValue(instance, null, null);
                continue;
            }

            var realType = isNullable
                ? Nullable.GetUnderlyingType(properties[i].PropertyType)
                : properties[i].PropertyType;
            
            var value = Convert.ChangeType(values[i], realType!);
            properties[i].SetValue(instance, value, null);
        }

        return instance;
    }
    
    private static List<PropertyInfo> GetProperties(Type t)
    {
        return t.GetProperties()
            .Where(x => x.CanWrite)
            .Where(x => Attribute.IsDefined(x, typeof(IndexAttribute)))
            .OrderBy(x => ((IndexAttribute) x
                .GetCustomAttributes(typeof(IndexAttribute), false)
                .Single()).Index)
            .ToList();
    }
}