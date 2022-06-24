namespace AuthDb;

public class Student
{
    public int Id { get; set; }
    public string LastName { get; set; } = null!;
    public string FirstMidName { get; set; } = null!;
    public DateTime EnrollmentDate { get; set; }
    
    public ICollection<Enrollment> Enrollments { get; set; } = null!;
}