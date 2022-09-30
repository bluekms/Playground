using System.ComponentModel.DataAnnotations;
using StaticDataLibrary.Commons;

namespace StaticDataLibrary.Records;

public class ComplexTestRecord
{
    [Key]
    public string Id { get; set; } = null!;
    public string StudentId1 { get; set; } = null!;
    public string? Name1 { get; set; }
    public string? Subject1_1 { get; set; }
    public Grades? Grade1_1 { get; set; }
    public string? Subject1_2 { get; set; }
    public Grades? Grade1_2 { get; set; }
    public string? Subject1_3 { get; set; }
    public Grades? Grade1_3 { get; set; }
    public string? Note1 { get; set; }
    public string? StudentId2 { get; set; }
    public string? Name2 { get; set; }
    public string? Subject2_1 { get; set; }
    public Grades? Grade2_1 { get; set; }
    public string? Subject2_2 { get; set; }
    public Grades? Grade2_2 { get; set; }
    public string? Subject2_3 { get; set; }
    public Grades? Grade2_3 { get; set; }
    public string? Note2 { get; set; }
    public string? StudentId3 { get; set; }
    public string? Name3 { get; set; }
    public string? Subject3_1 { get; set; }
    public Grades? Grade3_1 { get; set; }
    public string? Subject3_2 { get; set; }
    public Grades? Grade3_2 { get; set; }
    public string? Subject3_3 { get; set; }
    public Grades? Grade3_3 { get; set; }
    public string? Note3 { get; set; }
    public string? ClassNote { get; set; }
}