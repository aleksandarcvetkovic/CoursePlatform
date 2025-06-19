using CoursePlatform.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CoursePlatform.Repositories;

public interface IEnrollmentRepository : IGenericRepository<Enrollment>
{
    Task<IEnumerable<EnrollmentResponseDTO>> GetAllDTOsAsync();
    Task<EnrollmentWithStudentCourseDTO?> GetWithStudentCourseAsync(string id);
    Task AddAsync(Enrollment enrollment);
    Task UpdateAsync(Enrollment enrollment);
    Task DeleteAsync(Enrollment enrollment);
}
