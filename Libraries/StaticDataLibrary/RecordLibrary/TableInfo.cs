using System.Reflection;
using StaticDataLibrary.Attributes;

namespace StaticDataLibrary.RecordLibrary;

public sealed class TableInfo
{
    public const string RecordTypeNameSuffix = "Record";
    public const string DbSetNameSuffix = "Table";

    public sealed record ForeignInfo(
        string CurrentTableName,
        string CurrentColumnName,
        string ForeignTableName,
        string ForeignColumnName);
    
    public Type RecordType { get; }
    public string SheetName { get; }
    public string DbSetName { get; }
    public IList<string> ColumnNameList { get; private set; } = null!;
    public IList<ForeignInfo>? ForeignInfoList { get; private set; }

    public TableInfo(PropertyInfo pi)
    {
        RecordType = pi.PropertyType.GetGenericArguments().First();
        SheetName = RecordType.Name.Replace(RecordTypeNameSuffix, string.Empty);
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
            if (TryExtractColumnName(property, out var columnName))
            {
                columnNameList.Add(columnName);    
            }

            if (TryExtractForeignDbSetName(property, out var foreignDbSetName, out var foreignColumnName))
            {
                foreignDbSetNameList.Add(new(
                    DbSetName,
                    property.Name,
                    foreignDbSetName,
                    foreignColumnName));
            }
        }

        ColumnNameList = columnNameList.AsReadOnly();
        
        ForeignInfoList = foreignDbSetNameList.Count == 0
            ? null
            : foreignDbSetNameList.AsReadOnly();
    }

    private static bool TryExtractColumnName(PropertyInfo property, out string columnName)
    {
        columnName = string.Empty;
        
        var columnNameAttribute = property
            .GetCustomAttributes()
            .SingleOrDefault(x => x is ColumnNameAttribute);

        if (columnNameAttribute == null)
        {
            return false;
        }

        columnName = (columnNameAttribute as ColumnNameAttribute)!.Name;
        return true;
    }

    private static bool TryExtractForeignDbSetName(PropertyInfo property, out string foreignDbSetName, out string foreignColumnName)
    {
        foreignDbSetName = string.Empty;
        foreignColumnName = string.Empty;
        
        var foreignKeyAttribute = property
            .GetCustomAttributes()
            .SingleOrDefault(x => x is ForeignKeyAttribute);
        
        if (foreignKeyAttribute == null)
        {
            return false;
        }
        
        var foreignKey = (ForeignKeyAttribute)foreignKeyAttribute!;

        foreignDbSetName = foreignKey.DbSetName;
        foreignColumnName = foreignKey.ColumnName;
        return true;
    }
}