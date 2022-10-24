using System.ComponentModel.DataAnnotations;
using StaticDataLibrary.Attributes;

namespace StaticDataLibrary.DevRecords.TestRecords;

public sealed class GroupTestRecord
{
    [Key]
    [Order]
    public int Id { get; set; }
    
    [Order]
    public string GroupId { get; set; } = null!;
}