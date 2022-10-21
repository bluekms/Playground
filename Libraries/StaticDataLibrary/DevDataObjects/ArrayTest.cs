using System.Text;
using StaticDataLibrary.DevRecords.TestRecords;

namespace StaticDataLibrary.DevDataObjects;

public sealed class ArrayTest
{
    public ArrayTest(ArrayTestRecord record)
    {
        Id = record.Id;
        
        if (record.Value1 != null) { ValueList.Add((int)record.Value1); }
        if (record.Value2 != null) { ValueList.Add((int)record.Value2); }
        if (record.Value3 != null) { ValueList.Add((int)record.Value3); }
        if (record.Value4 != null) { ValueList.Add((int)record.Value4); }
        if (record.Value5 != null) { ValueList.Add((int)record.Value5); }

        Info = record.Info;
    }

    public override string ToString()
    {
        var sb = new StringBuilder();
        sb.Append($"Id: {Id}");
        foreach (var v in ValueList)
        {
            sb.Append($",{v}");
        }

        sb.Append($",{Info}");
        
        return sb.ToString();
    }

    public int Id { get; init; }
    public List<int> ValueList { get; init; } = new();
    public string? Info { get; init; }
}