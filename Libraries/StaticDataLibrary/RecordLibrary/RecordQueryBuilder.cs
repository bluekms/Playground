using System.Reflection;
using System.Text;
using StaticDataLibrary.Attributes;

namespace StaticDataLibrary.RecordLibrary;

public static class RecordQueryBuilder
{
    public static string InsertQuery(Type t, string tableName, out List<string> parameters)
    {
        var sb = new StringBuilder($"INSERT INTO {tableName} VALUES (");
        
        var properties = GetProperties(t);
        
        parameters = new(properties.Count);
        
        for (int i = 0; i < properties.Count; ++i)
        {
            var property = properties[i];
            var parameter = $"@{property.Name}";
            
            if (i != properties.Count - 1)
            {
                sb.Append($"{parameter},");
            }
            else
            {
                sb.Append($"{parameter});");
            }

            parameters.Add(property.Name);
        }
        
        return sb.ToString();
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