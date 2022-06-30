using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AuthDb;

/// <summary>
/// 일반적으로 JoinEntity 이름은 EntityName1EntityName2이라
/// CourseInstructor 이지만 더 자연스러운 이름을 선택
/// </summary>
public class CourseAssignment
{
    public int InstructorId { get; set; }
    public Instructor Instructor { get; set; } = null!;
    
    public int CourseId { get; set; }
    public Course Course { get; set; } = null!;
}

internal sealed class CourseAssignmentConfiguration : IEntityTypeConfiguration<CourseAssignment>
{
    public void Configure(EntityTypeBuilder<CourseAssignment> builder)
    {
        builder.HasKey(t => new { t.CourseId, t.InstructorId });
    }
}