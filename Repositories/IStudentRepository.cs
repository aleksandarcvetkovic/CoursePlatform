using System.Collections.Generic;
using System.Threading.Tasks;
using CoursePlatform.Models;

namespace CoursePlatform.Repositories;

public interface IStudentRepository : IGenericRepository<Student>
{
    Task<Student?> GetWithEnrollmentsAsync(string id);

    Task<IEnumerable<StudentResponseDTO>> GetAllStudentDTOsAsync();
    Task<StudentResponseDTO?> GetStudentDTOByIdAsync(string id);
    Task<StudentWithEnrolmentsDTO?> GetStudentWithEnrollmentsDTOAsync(string id);
}

