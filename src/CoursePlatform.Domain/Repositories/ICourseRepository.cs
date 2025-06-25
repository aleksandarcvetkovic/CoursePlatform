using CoursePlatform.Domain.Entities;

namespace CoursePlatform.Domain.Repositories;

public interface ICourseRepository : IGenericRepository<Course>
{
    Task<Course?> GetByIdWithInstructorAsync(string id, CancellationToken cancellationToken = default);
}
