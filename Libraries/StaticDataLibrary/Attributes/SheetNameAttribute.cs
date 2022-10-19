namespace StaticDataLibrary.Attributes;

[AttributeUsage(System.AttributeTargets.Class)]
public class SheetNameAttribute : Attribute
{
    public SheetNameAttribute(string name)
    {
        Name = name;
    }

    public string Name { get; }
}