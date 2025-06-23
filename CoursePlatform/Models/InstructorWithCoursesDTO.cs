using System.Text.Json.Serialization;

namespace CoursePlatform.Models;

public class InstructorWithCoursesDTO
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }

   
    public ICollection<CourseResponseDTO> InstructorCourses { get; set; }
}