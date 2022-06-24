using System.ComponentModel.DataAnnotations.Schema;

namespace AuthDb;

public class Course
{
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public int CourseId { get; set; }
    public string Title { get; set; } = null!;
    public int Credits { get; set; }
    
    public ICollection<Enrollment> Enrollments { get; set; } = null!;
}