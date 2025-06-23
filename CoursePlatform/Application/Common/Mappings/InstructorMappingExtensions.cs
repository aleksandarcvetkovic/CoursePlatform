using CoursePlatform.Domain.Entities;
using CoursePlatform.Application.DTOs;

namespace CoursePlatform.Application.Common.Mappings;

public static class InstructorMappingExtensions
{
    public static InstructorResponseDTO ToInstructorResponseDTO(this Instructor instructor)
    {
        return new InstructorResponseDTO
        {
            Id = instructor.Id,
            Name = instructor.Name,
            Email = instructor.Email
        };
    }

    public static Instructor ToInstructor(this InstructorRequestDTO instructorDTO)
    {
        return new Instructor
        {
            Name = instructorDTO.Name,
            Email = instructorDTO.Email
        };
    }

    public static InstructorWithCoursesDTO ToInstructorWithCoursesDTO(this Instructor instructor)
    {
        return new InstructorWithCoursesDTO
        {
            Id = instructor.Id,
            Name = instructor.Name,
            Email = instructor.Email,
            Courses = instructor.Courses?.Select(c => c.ToCourseResponseDTO()) ?? new List<CourseResponseDTO>()
        };
    }

    public static void UpdateFromDTO(this Instructor instructor, InstructorRequestDTO instructorDTO)
    {
        instructor.Name = instructorDTO.Name;
        instructor.Email = instructorDTO.Email;
    }

    public static IEnumerable<InstructorResponseDTO> ToInstructorResponseDTOs(this IEnumerable<Instructor> instructors)
    {
        return instructors.Select(i => i.ToInstructorResponseDTO());
    }
}
