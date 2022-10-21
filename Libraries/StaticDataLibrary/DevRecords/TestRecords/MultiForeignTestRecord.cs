using System.ComponentModel.DataAnnotations;
using StaticDataLibrary.Attributes;

namespace StaticDataLibrary.DevRecords.TestRecords;

public sealed class MultiForeignTestRecord
{
    [Key]
    [Order]
    public int Id { get; set; }
    
    [Order]
    public int TargetId { get; set; }
    
    [Order]
    public string? Info { get; set; }
}