using CoursePlatform.Domain.Entities;
using CoursePlatform.Application.DTOs;

namespace CoursePlatform.Application.Common.Mappings;

public static class CourseMappingExtensions
{
    public static CourseResponseDTO ToCourseResponseDTO(this Course course)
    {
        return new CourseResponseDTO
        {
            Id = course.Id,
            Title = course.Title,
            Description = course.Description,
            InstructorId = course.InstructorId
        };
    }
    public static Course ToCourse(this CourseRequestDTO courseDTO)
    {
        return Course.Create(courseDTO.Title, courseDTO.Description, courseDTO.InstructorId);
    }

    public static CourseWithInstructorDTO ToCourseWithInstructorDTO(this Course course)
    {
        return new CourseWithInstructorDTO
        {
            Id = course.Id,
            Title = course.Title,
            Description = course.Description,
            InstructorId = course.InstructorId,
            InstructorName = course.Instructor?.Name ?? string.Empty,
            InstructorEmail = course.Instructor?.Email ?? string.Empty
        };
    }    public static void UpdateFromDTO(this Course course, CourseRequestDTO courseDTO)
    {
        course.Update(courseDTO.Title, courseDTO.Description, courseDTO.InstructorId);
    }

    public static IEnumerable<CourseResponseDTO> ToCourseResponseDTOs(this IEnumerable<Course> courses)
    {
        return courses.Select(c => c.ToCourseResponseDTO());
    }
}
