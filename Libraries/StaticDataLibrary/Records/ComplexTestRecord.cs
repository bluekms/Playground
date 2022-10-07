using System.ComponentModel.DataAnnotations;
using CsvHelper.Configuration.Attributes;
using StaticDataLibrary.Commons;

namespace StaticDataLibrary.Records;

public class ComplexTestRecord
{
    [Key]
    [Index(1)]
    public string Id { get; set; } = null!;
    
    [Index(2)]
    public string StudentId1 { get; set; } = null!;
    
    [Index(3)]
    public string? Name1 { get; set; }
    
    [Index(4)]
    public string? Subject1_1 { get; set; }
    
    [Index(5)]
    public Grades? Grade1_1 { get; set; }
    
    [Index(6)]
    public string? Subject1_2 { get; set; }
    
    [Index(7)]
    public Grades? Grade1_2 { get; set; }
    
    [Index(8)]
    public string? Subject1_3 { get; set; }
    
    [Index(9)]
    public Grades? Grade1_3 { get; set; }
    
    [Index(10)]
    public string? Note1 { get; set; }
    
    [Index(11)]
    public string? StudentId2 { get; set; }
    
    [Index(12)]
    public string? Name2 { get; set; }
    
    [Index(13)]
    public string? Subject2_1 { get; set; }
    
    [Index(14)]
    public Grades? Grade2_1 { get; set; }
    
    [Index(15)]
    public string? Subject2_2 { get; set; }
    
    [Index(16)]
    public Grades? Grade2_2 { get; set; }
    
    [Index(17)]
    public string? Subject2_3 { get; set; }
    
    [Index(18)]
    public Grades? Grade2_3 { get; set; }
    
    [Index(19)]
    public string? Note2 { get; set; }
    
    [Index(20)]
    public string? StudentId3 { get; set; }
    
    [Index(21)]
    public string? Name3 { get; set; }
    
    [Index(22)]
    public string? Subject3_1 { get; set; }
    
    [Index(23)]
    public Grades? Grade3_1 { get; set; }
    
    [Index(24)]
    public string? Subject3_2 { get; set; }
    
    [Index(25)]
    public Grades? Grade3_2 { get; set; }
    
    [Index(26)]
    public string? Subject3_3 { get; set; }
    
    [Index(27)]
    public Grades? Grade3_3 { get; set; }
    
    [Index(28)]
    public string? Note3 { get; set; }
    
    [Index(29)]
    public string? ClassNote { get; set; }
}