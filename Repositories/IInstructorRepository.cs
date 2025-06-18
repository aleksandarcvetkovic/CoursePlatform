using CoursePlatform.Models;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace CoursePlatform.Repositories;

public interface IInstructorRepository
{
    Task<IEnumerable<Instructor>> GetAllAsync();
    Task<Instructor?> GetByIdAsync(string id);
    Task<Instructor?> GetWithCoursesAsync(string id);
    Task<IEnumerable<InstructorResponseDTO>> GetAllDTOsAsync();
    Task<InstructorResponseDTO?> GetDTOByIdAsync(string id);
    Task<InstructorWithCoursesDTO?> GetWithCoursesDTOAsync(string id);
    Task AddAsync(Instructor instructor);
    Task UpdateAsync(Instructor instructor);
    Task DeleteAsync(Instructor instructor);
    Task SaveChangesAsync();
}