namespace StaticDataLibrary.Attributes;

[AttributeUsage(System.AttributeTargets.Property)]
public sealed class ColumnNameAttribute : Attribute
{
    public ColumnNameAttribute(string name)
    {
        Name = name;
    }

    public string Name { get; }
}
