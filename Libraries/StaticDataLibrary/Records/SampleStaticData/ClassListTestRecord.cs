using System.ComponentModel.DataAnnotations;
using StaticDataLibrary.Attributes;

namespace StaticDataLibrary.Records;

public enum Grades
{
    A,
    B,
    C,
    D,
    F,
}

public sealed record SubjectInfo
{
    public string Subject { get; } = null!;
    public Grades Grade { get; }
}

[SheetName("ClassListTest")]
public sealed record ClassListTestRecord
{
    [Key]
    public int StudentId { get; }
    public string Name { get; }
    public HashSet<SubjectInfo> SubjectList { get; } = new();
    public string? Note { get; } = null;
}