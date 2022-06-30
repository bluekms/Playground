using System.ComponentModel.DataAnnotations;

namespace AuthDb;

public class OfficeAssignment
{
    /// <summary>
    /// ID, OfficeAssignmentID가 아니기 때문에 ef 에서 Key로 생각하지 않는다
    /// 이 경우 [Key]를 사용할 수 있다
    /// </summary>
    [Key]
    public int InstructorId { get; set; }

    [StringLength(50)]
    [Display(Name = "Office Location")]
    public string Location { get; set; } = null!;
    
    public Instructor Instructor { get; set; } = null!;
}