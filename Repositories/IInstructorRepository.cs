using CoursePlatform.Models;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace CoursePlatform.Repositories;

public interface IInstructorRepository : IGenericRepository<Instructor>
{
    Task<Instructor?> GetWithCoursesAsync(string id);
    Task<IEnumerable<InstructorResponseDTO>> GetAllDTOsAsync();
    Task<InstructorResponseDTO?> GetDTOByIdAsync(string id);
    Task<InstructorWithCoursesDTO?> GetWithCoursesDTOAsync(string id);

}