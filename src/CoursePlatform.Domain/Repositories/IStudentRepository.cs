using CoursePlatform.Domain.Entities;

namespace CoursePlatform.Domain.Repositories;

public interface IStudentRepository : IGenericRepository<Student>
{
    // Entity-specific methods that are not covered by the generic repository
    Task<Student?> GetByIdWithEnrollmentsAsync(string id, CancellationToken cancellationToken = default);
    Task<Student?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);
    Task<IEnumerable<Student>> GetActiveStudentsAsync(CancellationToken cancellationToken = default);
}
