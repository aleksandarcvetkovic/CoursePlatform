using System.ComponentModel.DataAnnotations;

namespace CoursePlatform.Models;

public class Course
{
    [Key]
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string Title { get; set; }
    public string Description { get; set; }
    public string InstructorId { get; set; }
    public Instructor Instructor { get; set; }
    public ICollection<Enrollment> Enrollments { get; set; }
}