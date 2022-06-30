using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AuthDb;

public class Instructor
{
    public int Id { get; set; }

    [Required]
    [Display(Name = "Last Name")]
    [StringLength(50)]
    public string LastName { get; set; } = null!;
    
    [Required]
    [Column("FirstName")]
    [Display(Name = "First Name")]
    [StringLength(50)]
    public string FirstMidName { get; set; } = null!;
    
    [DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true), Display(Name = "Hire Date")]
    public DateTime HireDate { get; set; }

    [Display(Name = "Full Name")]
    public string FullName
    {
        get
        {
            return $"{LastName}, {FirstMidName}";
        }
    }
    
    public ICollection<CourseAssignment> CourseAssignments { get; set; } = null!;
    public OfficeAssignment OfficeAssignment { get; set; } = null!;
}