namespace CoursePlatform.Models;

public class EnrollmentWithStudentCourseDTO
{
    public string Id { get; set; }

    public CourseResponseDTO EnrolmentCourse { get; set; }

    public StudentResponseDTO EnrolmentStudent { get; set; }

    public DateTime EnrolledOn { get; set; }
    public double? Grade { get; set; } // Optional progress tracking
}