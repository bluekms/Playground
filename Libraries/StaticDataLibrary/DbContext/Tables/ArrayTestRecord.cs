namespace StaticDataLibrary.DbContext.Tables;

public class ArrayTestRecord
{
    public int Id { get; set; }
    public List<int> ValueList { get; set; }
    public string? Info { get; set; }
}