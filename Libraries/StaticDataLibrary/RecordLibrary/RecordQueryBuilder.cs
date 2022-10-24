using System.Text;

namespace StaticDataLibrary.RecordLibrary;

public static class RecordQueryBuilder
{
    public static string InsertQuery(TableInfo tableInfo, out List<string> parameters)
    {
        var sb = new StringBuilder($"INSERT INTO {tableInfo.DbSetName} VALUES (");
        
        var properties = OrderedPropertySelector.GetList(tableInfo.RecordType);
        
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

    public static List<string> SelectForeignKeyListQuery(TableInfo tableInfo)
    {
        var list = new List<string>(tableInfo.ForeignInfoList.Count);
        foreach (var foreignInfo in tableInfo.ForeignInfoList)
        {
            list.Add($"SELECT {foreignInfo.ColumnName} FROM {tableInfo.DbSetName} GROUP BY {foreignInfo.ColumnName};");
        }

        return list;
    }
    
    public static string SelectForeignKeyListQuery(TableInfo tableInfo, TableInfo.ForeignInfo foreignInfo)
    {
        return $"SELECT {foreignInfo.ColumnName} FROM {tableInfo.DbSetName} GROUP BY {foreignInfo.ColumnName};";
    }

    public static string SelectCountQuery(TableInfo.ForeignInfo foreignInfo, object expected)
    {
        return $"SELECT COUNT(1) FROM {foreignInfo.ForeignTableName} WHERE {foreignInfo.ColumnName} = '{expected}'";
    }
}