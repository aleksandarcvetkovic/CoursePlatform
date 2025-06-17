namespace CoursePlatform.Models;

public class EnrollmentRequestDTO
{

    public string CourseId { get; set; }

    public string StudentId { get; set; }

    public DateTime EnrolledOn { get; set; }
    public double? Grade { get; set; } // Optional progress tracking
}