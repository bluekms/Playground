using System.Collections;

namespace StaticDataLibrary;

public sealed class ConstData
{
    public const string RecordsNamespace = "StaticDataLibrary.Records";
    public const string ListSuffix = "List";
    
    public static readonly Type[] TerminalTypes = new[]
    {
        typeof(Int32),
        typeof(String),
    };
}