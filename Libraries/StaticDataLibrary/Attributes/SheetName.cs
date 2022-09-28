namespace StaticDataLibrary.Attributes;

[AttributeUsage(System.AttributeTargets.Class)]
public class SheetName : Attribute
{
    public SheetName(string name)
    {
        Name = name;
    }

    public string Name { get; }
}