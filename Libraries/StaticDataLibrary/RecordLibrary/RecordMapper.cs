using System.Reflection;
using System.Text;
using StaticDataLibrary.Attributes;

namespace StaticDataLibrary.RecordLibrary;

public static class RecordMapper
{
    public static object? Map(Type t, string csvLine)
    {
        if (string.IsNullOrWhiteSpace(csvLine))
        {
            return null;
        }

        var properties = GetProperties(t);
        var values = CsvLineParser(csvLine);
        if (properties.Count != values.Count)
        {
            throw new ArgumentOutOfRangeException($"{t.Name}'s Property: {properties.Count}. but csv arguments: {values.Count}\n{string.Concat(values, ',')}");
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
            
            var value = realType!.IsEnum 
                ? Enum.Parse(realType, values[i])
                : Convert.ChangeType(values[i], realType!);
            
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

    private static List<string> CsvLineParser(string csvLine)
    {
        var values = csvLine.Split(',');
        
        var list = new List<string>(values.Length);
        var merging = false;
        var sb = new StringBuilder();
        foreach (var value in values)
        {
            if (!merging && IsMergeStartColumn(value))
            {
                merging = true;
                sb.Clear();
                sb.Append($"{value},");
            }
            else if (merging && IsMergeEndColumn(value))
            {
                merging = false;
                sb.Append(value);
                list.Add(sb.ToString());
            }
            else
            {
                if (merging)
                {
                    sb.Append($"{value},");
                }
                else
                {
                    list.Add(value);    
                }
            }
        }

        return list;
    }

    private static bool IsMergeStartColumn(string column)
    {
        if (!string.IsNullOrWhiteSpace(column))
        {
            if (column.First() == '"')
            {
                return true;
            }
        }

        return false;
    }
    
    private static bool IsMergeEndColumn(string column)
    {
        if (!string.IsNullOrWhiteSpace(column))
        {
            if (column.Last() == '"')
            {
                return true;
            }
        }

        return false;
    }
}