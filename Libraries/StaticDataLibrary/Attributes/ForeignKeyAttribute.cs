using System.Runtime.CompilerServices;

namespace StaticDataLibrary.Attributes;

[AttributeUsage(System.AttributeTargets.Property)]
public class ForeignKeyAttribute : Attribute
{
    public ForeignKeyAttribute(string dbSetName, string columnName)
    {
        DbSetName = dbSetName;
        ColumnName = columnName;
    }
    
    public string DbSetName { get; }
    public string ColumnName { get; }
}