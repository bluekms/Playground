using System.ComponentModel;
using System.Globalization;
using System.Reflection;
using System.Text;

namespace StaticDataLibrary.RecordLibrary;

public static class RecordMapper
{
    public static object? Map(Type t, string csvLine)
    {
        if (string.IsNullOrWhiteSpace(csvLine))
        {
            return null;
        }

        var properties = OrderedPropertySelector.GetList(t);
        var values = CsvLineParser(csvLine);
        if (properties.Count != values.Count)
        {
            throw new ArgumentOutOfRangeException($"{t.Name}'s Property: {properties.Count}. but csv arguments: {values.Count}\n{string.Concat(values, ',')}");
        }

        var instance = Activator.CreateInstance(t);
        for (var i = 0; i < properties.Count; ++i)
        {
            var realType = GetRealType(properties[i]);

            if (string.IsNullOrWhiteSpace(values[i]))
            {
                if (properties[i].IsNullable())
                {
                    properties[i].SetValue(instance, null, null);
                    continue;
                }
                else
                {
                    throw new ArgumentException($"{t.Name}.{properties[i].Name} must not be null or empty");
                }
            }

            object value;
            try
            {
                if (realType!.IsEnum)
                {
                    value = Enum.Parse(realType, values[i]);
                }
                else
                {
                    var converter = TypeDescriptor.GetConverter(realType);
                    value = converter.ConvertFrom(values[i])!;
                }
            }
            catch (Exception e)
            {
                throw new ArgumentException($"Type Error. {t.Name}.{properties[i].Name} value({values[i]}) is not {realType!.Name}", e);
            }

            properties[i].SetValue(instance, value, null);
        }

        return instance;
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
                sb.Append(CultureInfo.InvariantCulture, $"{value},");
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
                    sb.Append(CultureInfo.InvariantCulture, $"{value},");
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

    private static Type GetRealType(PropertyInfo propertyInfo)
    {
        if (propertyInfo.PropertyType == typeof(string))
        {
            return propertyInfo.PropertyType;
        }

        return propertyInfo.IsNullable()
            ? Nullable.GetUnderlyingType(propertyInfo.PropertyType)!
            : propertyInfo.PropertyType;
    }
}
