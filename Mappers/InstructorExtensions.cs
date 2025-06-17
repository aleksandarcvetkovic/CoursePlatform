using System.Reflection.Metadata.Ecma335;
using CoursePlatform.Models;
using Microsoft.AspNetCore.Components.Web;

public static class InstructorExtensions
{
    public static InstructorDTO ToInstructorDTO(this Instructor instructor)
    {
        return new InstructorDTO
        {
            Id = instructor.Id,
            Name = instructor.Name,
            Email = instructor.Email,

        };


    }
    public static Instructor ToInstructor(this InstructorDTO instructorDTO)
    {
        return new Instructor
        {
            Id = instructorDTO.Id,
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

    public static void UpdateFromDTO(this Instructor instructor, InstructorDTO instructorDTO)
    {
        instructor.Id = instructorDTO.Id;
        instructor.Name = instructorDTO.Name;
        instructor.Email = instructorDTO.Email;
    }

    

    public static IEnumerable<InstructorDTO> ToDTOs(this IEnumerable<Instructor> instructors)
    {
        return instructors.Select(c => c.ToInstructorDTO());
    }
}