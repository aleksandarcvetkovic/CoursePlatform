using CoursePlatform.Models;
namespace CoursePlatform.Services;

public interface ICourseService
{
    Task<IEnumerable<CourseResponseDTO>> GetAllCoursesAsync();
    Task<CourseResponseDTO> GetCourseAsync(string id);
    Task<CourseWithInstructorDTO> GetCourseWithInstructorAsync(string id);
    Task<CourseResponseDTO> CreateCourseAsync(CourseRequestDTO courseDTO);
    Task UpdateCourseAsync(string id, CourseRequestDTO courseDTO);
    Task DeleteCourseAsync(string id);
   
}