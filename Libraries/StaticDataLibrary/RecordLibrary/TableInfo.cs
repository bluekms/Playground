using System.Reflection;
using StaticDataLibrary.Attributes;

namespace StaticDataLibrary.RecordLibrary;

public sealed class TableInfo
{
    public const string TypeNameSuffix = "Record";
    public const string DbSetNameSuffix = "Table";
    
    public Type RecordType { get; }
    public string SheetName { get; }
    public string DbSetName { get; }

    public IList<string> ColumnNameList { get; private set; } = null!;
    
    public TableInfo(PropertyInfo pi)
    {
        RecordType = pi.PropertyType.GetGenericArguments().First();
        SheetName = RecordType.Name.Replace(TypeNameSuffix, string.Empty);
        DbSetName = $"{SheetName}{DbSetNameSuffix}";
        
        var sheetNameAttribute = RecordType
            .GetCustomAttributes()
            .SingleOrDefault(x => x.GetType() == typeof(SheetNameAttribute));
            
        if(sheetNameAttribute != null)
        {
            SheetName = (sheetNameAttribute as SheetNameAttribute)!.Name;
        }

        SetColumnNameList();
    }

    private void SetColumnNameList()
    {
        var propertyList = OrderedPropertySelector.GetList(RecordType).ToList();
        var columnNameList = new List<string>(propertyList.Count);
        
        foreach (var property in propertyList)
        {
            var columnNameAttribute = property
                .GetCustomAttributes()
                .SingleOrDefault(x => x.GetType() == typeof(ColumnNameAttribute));

            if (columnNameAttribute == null)
            {
                columnNameList.Add(property.Name);
            }
            else
            {
                var columName = (columnNameAttribute as ColumnNameAttribute)!.Name;
                columnNameList.Add(columName);
            }
        }

        ColumnNameList = columnNameList.AsReadOnly();
    }
}