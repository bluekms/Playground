using System.Text.RegularExpressions;

namespace StaticDataLibrary.ExcelLibrary;

public sealed class ContainerNameParser
{
    private const string ContainerNamePattern = @"^([a-zA-Z]\w+)\[(\d+)\]";

    public bool IsContainerItem { get; }
    public string? PureName { get; }
    public int? ContainerIndex { get; }
    
    public ContainerNameParser(string columnName)
    {
        var reg = new Regex(ContainerNamePattern);

        if (!reg.IsMatch(columnName))
        {
            return;
        }
        
        var matchCollection = reg.Matches(columnName);
        var match = matchCollection[0];

        IsContainerItem = true;
        PureName = match.Groups[1].Value;
        
        // 음수는 IsMatch에서 걸러진다
        ContainerIndex = int.Parse(match.Groups[2].Value);
    }
}