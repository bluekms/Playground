using System.Runtime.CompilerServices;

namespace StaticDataLibrary.Attributes;

[AttributeUsage(System.AttributeTargets.Property)]
public class ForeignKeyAttribute : Attribute
{
    public ForeignKeyAttribute(string dbSetName, [CallerMemberName] string columnName = null)
    {
        DbSetName = dbSetName;
        ColumnName = columnName;
    }
    
    public string DbSetName { get; }
    public string ColumnName { get; }
}