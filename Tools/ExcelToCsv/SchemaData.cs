namespace ExcelToCsv;

public enum PrimitiveTypes
{
    String,
    Int,
    Per100,
    Per10000,
    Enum,
    Class,
}

public enum ContainerTypes
{
    List,
    HashSet,
    Dictionary,
}

public sealed class SchemaData
{
    public const string TargetAll = "__All";

    public List<string> OutputTargets { get; } = new();
    public bool IsKey { get; }
    public PrimitiveTypes? PrimitiveType { get; }
    public string? TypeName { get; }
    public string? ClassName { get; }
    public string? ForeignTable { get; }
    public string? ForeignKey { get; }

    // Set Name 이후 결정되는 것들
    public string? Name { get; private set; }
    public bool IsContainerItem { get; private set; }
    public ContainerTypes? ContainerType { get; private set; }
    public int? ContainerIndex { get; private set; }

    public SchemaData(SchemaOptions options)
    {
        if (options.Targets == null)
        {
            throw new ArgumentNullException(nameof(options), "Must be use CommandLineParser");
        }
        
        if (options.Targets.Any())
        {
            OutputTargets.AddRange(options.Targets);
        }
        else
        {
            OutputTargets.Add(TargetAll);
        }
        
        IsKey = options.IsKey;
        PrimitiveType = (PrimitiveTypes)Enum.Parse(typeof(PrimitiveTypes), options.Type!, true);
        
        if (PrimitiveType == PrimitiveTypes.Enum)
        {
            if (options.TypeName == null)
            {
                throw new ArgumentNullException($"{nameof(PrimitiveType)}가 enum이면 반드시 TypeName이 있어야 합니다.");
            }
            
            TypeName = options.TypeName;    
        }
        
        ClassName = options.ClassName;
        ForeignTable = options.ForeignTable;
        ForeignKey = options.ForeignKey;
    }

    public void SetName(string sourceName)
    {
        var parser = new ContainerNameParser(sourceName)!;
        if (!parser.IsContainerItem)
        {
            Name = sourceName;
        }
        else
        {
            Name = parser.PureName;
            IsContainerItem = parser.IsContainerItem;
            ContainerType = ContainerTypes.List;    // TODO 아직 컨테이너Key가 있는지 알 수 없다
            ContainerIndex = parser.ContainerIndex;
        }
    }
}