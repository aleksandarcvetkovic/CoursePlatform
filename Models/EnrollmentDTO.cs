namespace CoursePlatform.Models;

public class EnrollmentDTO
{
    public int Id { get; set; }

    public int CourseId { get; set; }

    public int StudentId { get; set; }

    public DateTime EnrolledOn { get; set; }
    public double? Grade { get; set; } // Optional progress tracking
}