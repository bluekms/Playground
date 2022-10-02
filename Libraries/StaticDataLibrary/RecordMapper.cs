using System.Reflection;
using StaticDataLibrary.Attributes;

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
            var value = Convert.ChangeType(values[i], properties[i].PropertyType);
            properties[i].SetValue(instance, value, null);
        }

        return instance;
    }
    
    private static List<PropertyInfo> GetProperties(Type t)
    {
        return t.GetProperties()
            .Where(x => x.CanWrite)
            .Where(x => Attribute.IsDefined(x, typeof(OrderAttribute)))
            .OrderBy(x => ((OrderAttribute) x
                .GetCustomAttributes(typeof(OrderAttribute), false)
                .Single()).Order)
            .ToList();
    }
}