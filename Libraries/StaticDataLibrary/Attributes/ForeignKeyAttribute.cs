using System.Globalization;
using StaticDataLibrary.RecordLibrary;

namespace StaticDataLibrary.Attributes;

[AttributeUsage(AttributeTargets.Property)]
public class ForeignKeyAttribute : Attribute
{
    public ForeignKeyAttribute(string recordTypeName, string columnName)
    {
        DbSetName = RecordTypeNameToDbSetName(recordTypeName);
        ColumnName = columnName;
    }
    
    public string DbSetName { get; }
    public string ColumnName { get; }

    private string RecordTypeNameToDbSetName(string recordTypeName)
    {
        var compareInfo = CultureInfo.InvariantCulture.CompareInfo;
        if (!compareInfo.IsSuffix(recordTypeName, TableInfo.RecordTypeNameSuffix))
        {
            throw new InvalidOperationException($"{recordTypeName} is not RecordType for StaticData");
        }
        var pureName = recordTypeName.Substring(0, recordTypeName.Length - TableInfo.RecordTypeNameSuffix.Length);
        return $"{pureName}{TableInfo.DbSetNameSuffix}";
    }
}