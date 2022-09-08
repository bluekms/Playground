using System.Data;

namespace ExcelToCsv;

public enum StaticDataTypes
{
    String,
    Int,
    Per100,
    Per10000,
    Class,
}

public enum ContainerTypes
{
    PrimitiveType,
    List,
    HashSet,
    Dictionary,
}

public sealed class StaticDataSchema
{
    private static string AllTArgets = "__AllTargets";
    
    public StaticDataSchema(string? source)
    {
        Source = source;
    }
    
    public string? Source { get; init; }
    public string Name { get; private set; }
    public ContainerTypes ContainerType { get; init; }
    public StaticDataTypes StaticDataType { get; init; }
    public bool IsKey { get; init; }
    
    // TODO 일단 타겟만 잘 작동하도록
    public List<string> Targets { get; set; }

    public void SetName(string name)
    {
        if (!string.IsNullOrEmpty(Name))
        {
            throw new ReadOnlyException($"Name({Name}) has already been set.");
        }

        Name = name;
    }

    public override string ToString()
    {
        return $"{Name}: {Source}";
    }
}