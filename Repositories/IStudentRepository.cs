using System.Collections.Generic;
using System.Threading.Tasks;
using CoursePlatform.Models;

namespace CoursePlatform.Repositories;

public interface IStudentRepository
{
    Task<IEnumerable<Student>> GetAllAsync();
    Task<Student?> GetByIdAsync(string id);
    Task<Student?> GetWithEnrollmentsAsync(string id);

    Task<IEnumerable<StudentResponseDTO>> GetAllStudentDTOsAsync();
    Task<StudentResponseDTO?> GetStudentDTOByIdAsync(string id);
    Task<StudentWithEnrolmentsDTO?> GetStudentWithEnrollmentsDTOAsync(string id);

    Task AddAsync(Student student);
    Task UpdateAsync(Student student);
    Task DeleteAsync(Student student);
    Task SaveChangesAsync();
}
