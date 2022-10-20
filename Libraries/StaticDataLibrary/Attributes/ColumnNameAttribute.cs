namespace StaticDataLibrary.Attributes;

[AttributeUsage(System.AttributeTargets.Property)]
public class ColumnNameAttribute : Attribute
{
    public ColumnNameAttribute(string name)
    {
        Name = name;
    }

    public string Name { get; }
}