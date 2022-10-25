using System.Reflection;
using StaticDataLibrary.Attributes;

namespace StaticDataLibrary.RecordLibrary;

public sealed class TableInfo
{
    public const string TypeNameSuffix = "Record";
    public const string DbSetNameSuffix = "Table";

    public sealed record ForeignInfo(string ColumnName, string ForeignTableName);
    
    public Type RecordType { get; }
    public string SheetName { get; }
    public string DbSetName { get; }
    public IList<string> ColumnNameList { get; private set; } = null!;
    public IList<ForeignInfo>? ForeignInfoList { get; private set; }
    
    public TableInfo(PropertyInfo pi)
    {
        RecordType = pi.PropertyType.GetGenericArguments().First();
        SheetName = RecordType.Name.Replace(TypeNameSuffix, string.Empty);
        DbSetName = $"{SheetName}{DbSetNameSuffix}";
        
        var sheetNameAttribute = RecordType
            .GetCustomAttributes()
            .SingleOrDefault(x => x is SheetNameAttribute);
            
        if(sheetNameAttribute != null)
        {
            SheetName = (sheetNameAttribute as SheetNameAttribute)!.Name;
        }

        TraversePropertyAttributes();
    }

    private void TraversePropertyAttributes()
    {
        var propertyList = OrderedPropertySelector.GetList(RecordType).ToList();
        var columnNameList = new List<string>(propertyList.Count);
        var foreignDbSetNameList = new List<ForeignInfo>(propertyList.Count);
        
        foreach (var property in propertyList)
        {
            var columnName = ExtractColumnName(property);
            columnNameList.Add(columnName);

            var dbSetName = ExtractForeignDbSetName(property);
            if (!string.IsNullOrWhiteSpace(dbSetName))
            {
                foreignDbSetNameList.Add(new(property.Name, dbSetName));    
            }
        }

        ColumnNameList = columnNameList.AsReadOnly();
        
        ForeignInfoList = foreignDbSetNameList.Count == 0
            ? null
            : foreignDbSetNameList.AsReadOnly();
    }

    private static string ExtractColumnName(PropertyInfo property)
    {
        var columnNameAttribute = property
            .GetCustomAttributes()
            .SingleOrDefault(x => x is ColumnNameAttribute);

        return columnNameAttribute == null
            ? property.Name
            : (columnNameAttribute as ColumnNameAttribute)!.Name;
    }

    private static string? ExtractForeignDbSetName(PropertyInfo property)
    {
        var foreignKeyAttribute = property
            .GetCustomAttributes()
            .SingleOrDefault(x => x is ForeignKeyAttribute);

        return foreignKeyAttribute == null
            ? null
            : (foreignKeyAttribute as ForeignKeyAttribute)!.DbSetName;
    }
}