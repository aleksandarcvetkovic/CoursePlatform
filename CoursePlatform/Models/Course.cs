using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace CoursePlatform.Models;

public class Course
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public string Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string InstructorId { get; set; }
    public Instructor Instructor { get; set; }
    public ICollection<Enrollment> Enrollments { get; set; }
}