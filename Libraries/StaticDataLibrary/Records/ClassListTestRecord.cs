using System.ComponentModel.DataAnnotations;
using StaticDataLibrary.Commons;

namespace StaticDataLibrary.Records;

public class ClassListTestRecord
{
    [Key]
    public string StudentId { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string? Subject1 { get; set; }
    public Grades? Grade1 { get; set; }
    public string? Subject2 { get; set; }
    public Grades? Grade2 { get; set; }
    public string? Subject3 { get; set; }
    public Grades? Grade3 { get; set; }
    public string? Note { get; set; }
}