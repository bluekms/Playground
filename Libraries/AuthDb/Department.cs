using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AuthDb;

// https://docs.microsoft.com/ko-kr/aspnet/core/data/ef-mvc/complex-data-model?view=aspnetcore-6.0

public class Department
{
    public int DepartmentId { get; set; }

    [StringLength(50, MinimumLength = 3)] public string Name { get; set; } = null!;
    
    [DataType(DataType.Currency)]
    [Column(TypeName = "decimal")]
    public decimal Budget { get; set; }
    
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
    [Display(Name = "Start Date")]
    public DateTime StartDate { get; set; }
    
    /// <summary>
    /// InstructorId가 nullable이 아닐 경우 하위 삭제가 일어날 수 있다.
    /// 의도적으로 항상 값이 필요하면서, 하위 삭제가 일어나지 않게 하려면
    /// modelBuilder.Entity<Department>().HasOne(d => d.Administrator).WithMany().OnDelete(DeleteBehavior.Restrict) 
    /// </summary>
    public int? InstructorId { get; set; }
    public Instructor Adminstrator { get; set; } = null!;
    
    public ICollection<Course> Courses { get; set; } = null!;
}

internal sealed class DepartmentConfiguration : IEntityTypeConfiguration<Department>
{
    public void Configure(EntityTypeBuilder<Department> builder)
    {
        builder.ToTable("Department");
    }
}