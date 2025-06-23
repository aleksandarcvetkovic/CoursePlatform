using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace CoursePlatform.Models;

public class Enrollment
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public string Id { get; set; }

    public string CourseId { get; set; }
    public Course Course { get; set; }

    public string StudentId { get; set; }
    public Student Student { get; set; }

    public DateTime EnrolledOn { get; set; }
    public double? Grade { get; set; } // Optional progress tracking
}