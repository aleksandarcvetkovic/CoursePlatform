using System.Reflection.Metadata.Ecma335;
using CoursePlatform.Models;
using Microsoft.AspNetCore.Components.Web;

public static class InstructorExtensions
{
    public static InstructorResponseDTO ToInstructorResponseDTO(this Instructor instructor)
    {
        return new InstructorResponseDTO
        {
            Id = instructor.Id,
            Name = instructor.Name,
            Email = instructor.Email,

        };


    }
    public static Instructor ToInstructor(this InstructorResponseDTO instructorDTO)
    {
        return new Instructor
        {
            Id = instructorDTO.Id,
            Name = instructorDTO.Name,
            Email = instructorDTO.Email,

        };
    }
    public static Instructor ToInstructor(this InstructorRequestDTO instructorDTO)
    {
        return new Instructor
        {
            Name = instructorDTO.Name,
            Email = instructorDTO.Email,

        };
    }
  
    public static InstructorWithCoursesDTO ToInstructorWithCoursesDTO(this Instructor instructor)
    {

        return new InstructorWithCoursesDTO
        {
            Id = instructor.Id,
            Name = instructor.Name,
            Email = instructor.Email,
            InstructorCourses = instructor.Courses.ToRespondeDTOs().ToList()
        };

    }

    public static void UpdateFromDTO(this Instructor instructor, InstructorResponseDTO instructorDTO)
    {
        instructor.Id = instructorDTO.Id;
        instructor.Name = instructorDTO.Name;
        instructor.Email = instructorDTO.Email;
    }

    public static void UpdateFromDTO(this Instructor instructor, InstructorRequestDTO instructorDTO)
    {
        instructor.Name = instructorDTO.Name;
        instructor.Email = instructorDTO.Email;
    }


    public static IEnumerable<InstructorResponseDTO> ToDTOs(this IEnumerable<Instructor> instructors)
    {
        return instructors.Select(c => c.ToInstructorResponseDTO());
    }
}