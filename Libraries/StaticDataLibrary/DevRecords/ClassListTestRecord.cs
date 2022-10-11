using System.ComponentModel.DataAnnotations;
using StaticDataLibrary.Attributes;
using StaticDataLibrary.DevCommons;

namespace StaticDataLibrary.DevRecords;

public class ClassListTestRecord
{
    [Key]
    [Order]
    public string StudentId { get; set; } = null!;
    
    [Order]
    public string Name { get; set; } = null!;
    
    [Order]
    public string? Subject1 { get; set; }
    
    [Order]
    public Grades? Grade1 { get; set; }
    
    [Order]
    public string? Subject2 { get; set; }
    
    [Order]
    public Grades? Grade2 { get; set; }
    
    [Order]
    public string? Subject3 { get; set; }
    
    [Order]
    public Grades? Grade3 { get; set; }
    
    [Order]
    public string? Note { get; set; }
}