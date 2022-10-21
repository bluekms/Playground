using System.ComponentModel.DataAnnotations;
using StaticDataLibrary.Attributes;

namespace StaticDataLibrary.DevRecords;

public sealed class TypeTestRecord
{
    [Key]
    [Order]
    public int Id { get; set; }
    
    [Order]
    public uint UIntValue { get; set; }
    
    [Order]
    public short ShortValue { get; set; }
    
    [Order]
    public long LongValue { get; set; }
    
    [Order]
    public float FloatValue { get; set; }
    
    [Order]
    public double DoubleValue { get; set; }
    
    [Order]
    public char CharValue { get; set; }

    [Order]
    public string StringValue { get; set; } = null!;
    
    [Order]
    public DayOfWeek EnumValue { get; set; }
    
    [Order]
    public DateTime DateTimeValue { get; set; }
    
    [Order]
    public TimeSpan TimeSpanValue { get; set; }
}