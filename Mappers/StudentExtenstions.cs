using System.Reflection.Metadata.Ecma335;
using CoursePlatform.Models;
using Microsoft.AspNetCore.Components.Web;

public static class StudentExtensions
{
    public static StudentResponseDTO ToStudentResponseDTO(this Student student)
    {
        return new StudentResponseDTO
        {
            Id = student.Id,
            Name = student.Name,
            Email = student.Email,

        };


    }
    public static Student ToStudent(this StudentResponseDTO studentDTO)
    {
        return new Student
        {
            Id = studentDTO.Id,
            Name = studentDTO.Name,
            Email = studentDTO.Email,

        };
    }
    public static Student ToStudent(this StudentRequestDTO studentDTO)
    {
        return new Student
        {
            Name = studentDTO.Name,
            Email = studentDTO.Email,

        };
    }

    public static StudentWithEnrolmentsDTO ToStudentWithEnrolmentsDTO(this Student student)
    {

        return new StudentWithEnrolmentsDTO
        {
            Id = student.Id,
            Name = student.Name,
            Email = student.Email,
            StudentEnrollments = student.Enrollments.ToDTOs().ToList()
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
        student.Name = studentDTO.Name;
        student.Email = studentDTO.Email;
    }


    public static IEnumerable<StudentResponseDTO> ToDTOs(this IEnumerable<Student> students)
    {
        return students.Select(c => c.ToStudentResponseDTO());
    }
    
}