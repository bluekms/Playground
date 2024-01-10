using System.ComponentModel.DataAnnotations;
using StaticDataLibrary.Attributes;
using StaticDataLibrary.DevCommons;

namespace StaticDataLibrary.DevRecords.TestRecords;

public sealed class ComplexTestRecord
{
    [Key]
    [Order]
    public string Id { get; set; } = null!;

    [Order]
    public string StudentId1 { get; set; } = null!;

    [Order]
    public string? Name1 { get; set; }

    [Order]
    public string? Subject1_1 { get; set; }

    [Order]
    public Grades? Grade1_1 { get; set; }

    [Order]
    public string? Subject1_2 { get; set; }

    [Order]
    public Grades? Grade1_2 { get; set; }

    [Order]
    public string? Subject1_3 { get; set; }

    [Order]
    public Grades? Grade1_3 { get; set; }

    [Order]
    public string? Note1 { get; set; }

    [Order]
    public string? StudentId2 { get; set; }

    [Order]
    public string? Name2 { get; set; }

    [Order]
    public string? Subject2_1 { get; set; }

    [Order]
    public Grades? Grade2_1 { get; set; }

    [Order]
    public string? Subject2_2 { get; set; }

    [Order]
    public Grades? Grade2_2 { get; set; }

    [Order]
    public string? Subject2_3 { get; set; }

    [Order]
    public Grades? Grade2_3 { get; set; }

    [Order]
    public string? Note2 { get; set; }

    [Order]
    public string? StudentId3 { get; set; }

    [Order]
    public string? Name3 { get; set; }

    [Order]
    public string? Subject3_1 { get; set; }

    [Order]
    public Grades? Grade3_1 { get; set; }

    [Order]
    public string? Subject3_2 { get; set; }

    [Order]
    public Grades? Grade3_2 { get; set; }

    [Order]
    public string? Subject3_3 { get; set; }

    [Order]
    public Grades? Grade3_3 { get; set; }

    [Order]
    public string? Note3 { get; set; }

    [Order]
    public string? ClassNote { get; set; }
}
