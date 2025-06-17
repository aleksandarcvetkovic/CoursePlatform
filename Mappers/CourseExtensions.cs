using System.Reflection.Metadata.Ecma335;
using CoursePlatform.Models;

public static class CourseExtensions
{
    public static CourseResponseDTO ToResponseDTO(this Course course)
    {
        return new CourseResponseDTO
        {
            Id = course.Id,
            Title = course.Title,
            Description = course.Description,

        };


    }
    public static Course ToCourse(this CourseResponseDTO courseDTO)
    {
        return new Course
        {
            Id = courseDTO.Id,
            Title = courseDTO.Title,
            Description = courseDTO.Description

        };
    }

    public static void UpdateFromDTO(this Course course, CourseResponseDTO courseDTO)
    {
        course.Id = courseDTO.Id;
        course.Title = courseDTO.Title;
        course.Description = courseDTO.Description;
        

    }

    public static void UpdateFromDTO(this Course course, CourseRequestDTO courseDTO)
    {
        course.Title = courseDTO.Title;
        course.Description = courseDTO.Description;
        course.InstructorId = courseDTO.InstructorId;

    }
    public static CourseWithInstructorDTO ToCourseWithInstructor(this Course course)
    {
        return new CourseWithInstructorDTO
        {
            Id = course.Id,
            Title = course.Title,
            Description = course.Description,
            CourseInstructor = new InstructorResponseDTO
            {
                Id = course.InstructorId,
                Name = course.Instructor.Name,
                Email = course.Instructor.Email
            }

        };
    }
    public static Course ToCourse(this CourseRequestDTO courseDTO)
    {
        return new Course
        {
            Title = courseDTO.Title,
            Description = courseDTO.Description,
            InstructorId = courseDTO.InstructorId

        };
    }

    public static IEnumerable<CourseResponseDTO> ToRespondeDTOs(this IEnumerable<Course> courses)
    {
        return courses.Select(c => c.ToResponseDTO());
    }
}