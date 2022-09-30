using StaticDataLibrary.Records;

namespace StaticDataLibrary.DataObjects;

// TODO 이걸 이름만 보고 예쁘게 생성해주면 좋을것같은데 가능할지
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

    public int Id { get; init; }
    public List<int> ValueList { get; init; } = new();
    public string? Info { get; init; }
}