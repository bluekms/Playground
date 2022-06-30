using System.ComponentModel.DataAnnotations;

namespace AuthDb;

public enum Grade
{
    A, B, C, D, F
}

public class Enrollment
{
    public int EnrollmentId { get; set; }
    
    [DisplayFormat(NullDisplayText = "No Grade")]
    public Grade? Grade { get; set; }
    
    public int CourseId { get; set; }
    public Course Course { get; set; } = null!;
    
    public int StudentId { get; set; }
    public Student Student { get; set; } = null!;
}