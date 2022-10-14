using System.Text;

namespace StaticDataLibrary.RecordLibrary;

public static class RecordQueryBuilder
{
    public static string InsertQuery(Type t, string tableName, out List<string> parameters)
    {
        var sb = new StringBuilder($"INSERT INTO {tableName} VALUES (");
        
        var properties = OrderedPropertySelector.GetList(t);
        
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
}