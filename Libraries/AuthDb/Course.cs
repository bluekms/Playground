using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AuthDb;

public class Course
{
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    [Display(Name = "Number")]
    public int CourseId { get; set; }
    
    [StringLength(50, MinimumLength = 3)]
    public string Title { get; set; } = null!;
    
    [Range(0, 5)]
    public int Credits { get; set; }
    
    /// <summary>
    /// ef는 기본적으로는 외래 키 속성을 모델에 추가하지 않아도 된다
    /// 자동으로 DB에 외래 키를 생성하고, 그에 따른 그림자 속성을 만든다
    /// 그러나 관리 측면에서 외래키가 있는 쪽이 낫다
    /// 외래 키가 있다면 Course를 업데이트 하기 위해 굳이 Department를 가져올 필요가 없다
    /// ps. 그냥 잘 있다고 믿는건가?
    /// </summary>
    public int DepartmentId { get; set; }
    
    public Department Department { get; set; } = null!;
    public ICollection<Enrollment> Enrollments { get; set; } = null!;
    public ICollection<CourseAssignment> CourseAssignments { get; set; } = null!;
}

internal sealed class CourseConfiguration : IEntityTypeConfiguration<Course>
{
    public void Configure(EntityTypeBuilder<Course> builder)
    {
        builder.ToTable("Course");
    }
}