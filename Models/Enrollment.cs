namespace CoursePlatform.Models;

public class Enrollment
{
    public string Id { get; set; }

    public string CourseId { get; set; }
    public Course Course { get; set; }

    public string StudentId { get; set; }
    public Student Student { get; set; }

    public DateTime EnrolledOn { get; set; }
    public double? Grade { get; set; } // Optional progress tracking
}