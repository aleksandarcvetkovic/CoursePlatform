using System.ComponentModel.DataAnnotations;

namespace CoursePlatform.Models;

public class Enrollment
{
    [Key]
    public string Id { get; set; } = Guid.NewGuid().ToString();

    public string CourseId { get; set; }
    public Course Course { get; set; }

    public string StudentId { get; set; }
    public Student Student { get; set; }

    public DateTime EnrolledOn { get; set; }
    public double? Grade { get; set; } // Optional progress tracking
}