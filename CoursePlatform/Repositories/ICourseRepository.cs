using CoursePlatform.Models;

namespace CoursePlatform.Repositories;
public interface ICourseRepository : IGenericRepository<Course>
{
    Task<IEnumerable<CourseResponseDTO>> GetAllDTOAsync();
    Task<CourseWithInstructorDTO?> GetWithInstructorAsync(string id);
    Task<IEnumerable<CourseWithInstructorDTO>> GetAllWithInstructorAsync();
    Task<bool> ExistsAsync(string id);
}