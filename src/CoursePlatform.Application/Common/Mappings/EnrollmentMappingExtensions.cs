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
        return Enrollment.Create(enrollmentDTO.StudentId, enrollmentDTO.CourseId);
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

    public static void UpdateGradeFromDTO(this Enrollment enrollment, double grade)
    {
        enrollment.UpdateGrade(grade);
    }

    public static IEnumerable<EnrollmentResponseDTO> ToEnrollmentResponseDTOs(this IEnumerable<Enrollment> enrollments)
    {
        return enrollments.Select(e => e.ToEnrollmentResponseDTO());
    }
}
