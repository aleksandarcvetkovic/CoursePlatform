using CoursePlatform.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CoursePlatform.Repositories;

public interface IEnrollmentRepository
{
    Task<IEnumerable<Enrollment>> GetAllAsync();
    Task<Enrollment?> GetByIdAsync(string id);
    Task<Enrollment?> GetWithStudentCourseAsync(string id);
    Task AddAsync(Enrollment enrollment);
    Task UpdateAsync(Enrollment enrollment);
    Task DeleteAsync(Enrollment enrollment);
}
