namespace StaticDataLibrary.Attributes;

[AttributeUsage(System.AttributeTargets.Property)]
public class ColumnName : Attribute
{
    public ColumnName(string name)
    {
        Name = name;
    }

    public string Name { get; }
}