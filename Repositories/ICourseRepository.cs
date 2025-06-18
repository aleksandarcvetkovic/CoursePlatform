using CoursePlatform.Models;

namespace CoursePlatform.Repositories;
public interface ICourseRepository
{
    Task<IEnumerable<CourseResponseDTO>> GetAllAsync();
    Task<Course?> GetByIdAsync(string id);
    Task<CourseWithInstructorDTO?> GetWithInstructorAsync(string id);
    Task<IEnumerable<CourseWithInstructorDTO>> GetAllWithInstructorAsync();
    Task AddAsync(Course course);
    Task UpdateAsync(Course course);
    Task DeleteAsync(Course course);
    Task<bool> ExistsAsync(string id);
}