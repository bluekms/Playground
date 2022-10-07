using System.ComponentModel.DataAnnotations;
using CsvHelper.Configuration.Attributes;
using StaticDataLibrary.Commons;

namespace StaticDataLibrary.Records;

public class ClassListTestRecord
{
    [Key]
    [Index(0)]
    public string StudentId { get; set; } = null!;
    
    [Index(1)]
    public string Name { get; set; } = null!;
    
    [Index(2)]
    public string? Subject1 { get; set; }
    
    [Index(3)]
    public Grades? Grade1 { get; set; }
    
    [Index(4)]
    public string? Subject2 { get; set; }
    
    [Index(5)]
    public Grades? Grade2 { get; set; }
    
    [Index(6)]
    public string? Subject3 { get; set; }
    
    [Index(7)]
    public Grades? Grade3 { get; set; }
    
    [Index(8)]
    public string? Note { get; set; }
}