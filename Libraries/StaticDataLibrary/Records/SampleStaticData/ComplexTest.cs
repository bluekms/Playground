using System.ComponentModel.DataAnnotations;
using StaticDataLibrary.Attributes;

namespace StaticDataLibrary.Records;

[SheetName("ComplexTest")]
public sealed record ComplexTest
{
    [Key] public string Id { get; } = null!;
    public HashSet<ClassListTestRecord> Students { get; } = new();
    public string? ClassNote { get; } = null;
}