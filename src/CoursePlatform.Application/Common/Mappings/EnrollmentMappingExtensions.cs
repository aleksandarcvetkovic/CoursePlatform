using CoursePlatform.Domain.Entities;
using CoursePlatform.Application.DTOs;

namespace CoursePlatform.Application.Common.Mappings;

public static class EnrollmentMappingExtensions
{
    public static EnrollmentResponseDTO ToEnrollmentResponseDTO(this Enrollment enrollment)
    {
        return new EnrollmentResponseDTO
        {
            Id = enrollment.Id,
            CourseId = enrollment.CourseId,
            StudentId = enrollment.StudentId,
            EnrolledOn = enrollment.EnrolledOn,
            Grade = enrollment.Grade
        };
    }

    public static Enrollment ToEnrollment(this EnrollmentRequestDTO enrollmentDTO)
    {
        return new Enrollment
        {
            CourseId = enrollmentDTO.CourseId,
            StudentId = enrollmentDTO.StudentId,
            EnrolledOn = DateTime.UtcNow,
            Grade = null
        };
    }

    public static EnrollmentWithStudentCourseDTO ToEnrollmentWithStudentCourseDTO(this Enrollment enrollment)
    {
        return new EnrollmentWithStudentCourseDTO
        {
            Id = enrollment.Id,
            StudentId = enrollment.StudentId,
            StudentName = enrollment.Student?.Name ?? string.Empty,
            CourseId = enrollment.CourseId,
            CourseTitle = enrollment.Course?.Title ?? string.Empty,
            EnrolledOn = enrollment.EnrolledOn,
            Grade = enrollment.Grade
        };
    }

    public static void UpdateGrade(this Enrollment enrollment, double grade)
    {
        enrollment.Grade = grade;
    }

    public static IEnumerable<EnrollmentResponseDTO> ToEnrollmentResponseDTOs(this IEnumerable<Enrollment> enrollments)
    {
        return enrollments.Select(e => e.ToEnrollmentResponseDTO());
    }
}
