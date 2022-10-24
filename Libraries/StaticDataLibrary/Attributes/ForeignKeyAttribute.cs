namespace StaticDataLibrary.Attributes;

[AttributeUsage(System.AttributeTargets.Property)]
public class ForeignKeyAttribute : Attribute
{
    public ForeignKeyAttribute(string dbSetName)
    {
        DbSetName = dbSetName;
    }
    
    public string DbSetName { get; }
}