using CoursePlatform.Domain.Entities;

namespace CoursePlatform.Domain.Repositories;

public interface IInstructorRepository : IGenericRepository<Instructor>
{
    // Entity-specific methods that are not covered by the generic repository
    Task<Instructor?> GetByIdWithCoursesAsync(string id, CancellationToken cancellationToken = default);
    Task<Instructor?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);
    Task<IEnumerable<Instructor>> GetInstructorsWithActiveCoursesAsync(CancellationToken cancellationToken = default);
}
