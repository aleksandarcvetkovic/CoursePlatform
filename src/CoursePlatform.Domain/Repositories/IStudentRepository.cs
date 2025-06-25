using CoursePlatform.Domain.Entities;

namespace CoursePlatform.Domain.Repositories;

public interface IStudentRepository : IGenericRepository<Student>
{
    Task<Student?> GetByIdWithEnrollmentsAsync(string id, CancellationToken cancellationToken = default);
}
