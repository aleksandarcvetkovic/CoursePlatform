using System.Reflection.Metadata.Ecma335;
using CoursePlatform.Models;
using Microsoft.AspNetCore.Components.Web;

public static class EnrolmentExtensions
{
    public static EnrollmentDTO ToEnrolmentDTO(this Enrollment enrollment)
    {
        return new EnrollmentDTO
        {
            Id = enrollment.Id,
            CourseId = enrollment.CourseId,
            StudentId = enrollment.StudentId,
            EnrolledOn = enrollment.EnrolledOn,
            Grade = enrollment.Grade

        };


    }
    public static EnrollmentWithStudentCourseDTO ToEnrollmentWithStudentCourseDTO(this Enrollment enrollment)
    {
        return new EnrollmentWithStudentCourseDTO
        {
            Id = enrollment.Id,
            EnrolmentCourse = enrollment.Course.ToRespondeDTO(),
            EnrolmentStudent = enrollment.Student.ToStudentDTO(),
            EnrolledOn = enrollment.EnrolledOn,
            Grade = enrollment.Grade,
            

        };


    }
    public static Enrollment ToEnrolment(this EnrollmentDTO enrollmentDTO)
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


    public static void UpdateFromDTO(this Enrollment enrollment, EnrollmentDTO enrollmentDTO)
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

    

    public static IEnumerable<EnrollmentDTO> ToDTOs(this IEnumerable<Enrollment> enrollments)
    {
        return enrollments.Select(e => e.ToEnrolmentDTO());
    }
}