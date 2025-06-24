using CoursePlatform.Domain.Entities;

namespace CoursePlatform.Domain.Repositories;

public interface ICourseRepository : IGenericRepository<Course>
{
    // Entity-specific methods that are not covered by the generic repository
    Task<Course?> GetByIdWithInstructorAsync(string id, CancellationToken cancellationToken = default);
    Task<Course?> GetByIdWithEnrollmentsAsync(string id, CancellationToken cancellationToken = default);
    Task<IEnumerable<Course>> GetCoursesByInstructorAsync(string instructorId, CancellationToken cancellationToken = default);
    Task<IEnumerable<Course>> GetAvailableCoursesAsync(CancellationToken cancellationToken = default);
}
