namespace ExcelLibrary;

public sealed class PropertyNode
{
    public enum NodeTypes
    {
        PrimitiveTypes = 0,
        PrimitiveTypeList = 1,
        ClassList = 2,
        HashSet = 3,
        Dictionary = 4,
    }

    public PropertyNode(NodeTypes nodeType, string sourceName, string? name)
    {
        NodeType = nodeType;
        SourceName = sourceName;
        Name = name;
    }

    public NodeTypes NodeType { get; }
    public string SourceName { get; }
    public string? Name { get; }

    public override string ToString()
    {
        return $"{Name ?? SourceName}";
    }
}