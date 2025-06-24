namespace CoursePlatform.Application.DTOs;

public class EnrollmentRequestDTO
{
    public string CourseId { get; set; } = string.Empty;
    public string StudentId { get; set; } = string.Empty;
}

public class EnrollmentResponseDTO
{
    public string Id { get; set; } = string.Empty;
    public string CourseId { get; set; } = string.Empty;
    public string StudentId { get; set; } = string.Empty;
    public DateTime EnrolledOn { get; set; }
    public double? Grade { get; set; }
}

public class EnrollmentGradeRequestDTO
{
    public double Grade { get; set; }
}

public class EnrollmentWithStudentCourseDTO
{
    public string Id { get; set; } = string.Empty;
    public string StudentId { get; set; } = string.Empty;
    public string StudentName { get; set; } = string.Empty;
    public string CourseId { get; set; } = string.Empty;
    public string CourseTitle { get; set; } = string.Empty;
    public DateTime EnrolledOn { get; set; }
    public double? Grade { get; set; }
}
