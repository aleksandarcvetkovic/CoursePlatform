using System.Reflection.Metadata.Ecma335;
using CoursePlatform.Models;
using Microsoft.AspNetCore.Components.Web;

namespace CoursePlatform.Mappers;
public static class EnrollmentExtensions
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
    public static EnrollmentWithStudentCourseDTO ToEnrollmentWithStudentCourse(this Enrollment enrollment)
    {
        return new EnrollmentWithStudentCourseDTO
        {
            Id = enrollment.Id,
            EnrolmentCourse = enrollment.Course.ToResponseDTO(),
            EnrolmentStudent = enrollment.Student.ToStudentResponseDTO(),
            EnrolledOn = enrollment.EnrolledOn,
            Grade = enrollment.Grade,


        };


    }
    public static Enrollment ToEnrollment(this EnrollmentResponseDTO enrollmentDTO)
    {
        return new Enrollment
        {
            Id = enrollmentDTO.Id,
            CourseId = enrollmentDTO.CourseId,
            StudentId = enrollmentDTO.StudentId,
            EnrolledOn = enrollmentDTO.EnrolledOn,
            Grade = enrollmentDTO.Grade

        };
    }
    public static Enrollment ToEnrollment(this EnrollmentRequestDTO enrollmentDTO)
    {
        return new Enrollment
        {
            CourseId = enrollmentDTO.CourseId,
            StudentId = enrollmentDTO.StudentId,
            EnrolledOn = DateTime.UtcNow, // Set current date as enrolled date
            Grade = enrollmentDTO.Grade

        };
    }


    public static void UpdateFromDTO(this Enrollment enrollment, EnrollmentResponseDTO enrollmentDTO)
    {
        enrollment.Id = enrollmentDTO.Id;
        enrollment.CourseId = enrollmentDTO.CourseId;
        enrollment.StudentId = enrollmentDTO.StudentId;
        enrollment.EnrolledOn = enrollmentDTO.EnrolledOn;
        enrollment.Grade = enrollmentDTO.Grade;
    }

    public static void UpdateFromDTO(this Enrollment enrollment, EnrollmentGradeRequestDTO enrollmentDTO)
    {
        enrollment.Id = enrollmentDTO.Id;
        enrollment.Grade = enrollmentDTO.Grade;
    }



    public static IEnumerable<EnrollmentResponseDTO> ToDTOs(this IEnumerable<Enrollment> enrollments)
    {
        return enrollments.Select(e => e.ToEnrollmentResponseDTO());
    }
}