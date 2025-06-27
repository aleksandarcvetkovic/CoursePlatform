using CoursePlatform.Domain.Entities;
using CoursePlatform.Application.DTOs;

namespace CoursePlatform.Application.Common.Mappings;

public static class StudentMappingExtensions
{
    public static StudentResponseDTO ToStudentResponseDTO(this Student student)
    {
        return new StudentResponseDTO
        {
            Id = student.Id,
            Name = student.Name,
            Email = student.Email
        };
    }

    public static Student ToStudent(this StudentResponseDTO studentDTO)
    {
        return new Student
        {
            Id = studentDTO.Id,
            Name = studentDTO.Name,
            Email = studentDTO.Email
        };
    }

    public static Student ToStudent(this StudentRequestDTO studentDTO)
    {
        return Student.Create(studentDTO.Name, studentDTO.Email);
    }

    public static StudentWithEnrollmentsDTO ToStudentWithEnrollmentsDTO(this Student student)
    {
        return new StudentWithEnrollmentsDTO
        {
            Id = student.Id,
            Name = student.Name,
            Email = student.Email,
            Enrollments = student.Enrollments?.Select(e => e.ToEnrollmentResponseDTO()) ?? new List<EnrollmentResponseDTO>()
        };
    }

    public static void UpdateFromDTO(this Student student, StudentResponseDTO studentDTO)
    {
        student.Id = studentDTO.Id;
        student.Name = studentDTO.Name;
        student.Email = studentDTO.Email;
    }

    public static void UpdateFromDTO(this Student student, StudentRequestDTO studentDTO)
    {
        student.UpdateInfo(studentDTO.Name, studentDTO.Email);
    }

    public static IEnumerable<StudentResponseDTO> ToStudentResponseDTOs(this IEnumerable<Student> students)
    {
        return students.Select(s => s.ToStudentResponseDTO());
    }
}
