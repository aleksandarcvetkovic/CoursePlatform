using CoursePlatform.Domain.Entities;

namespace CoursePlatform.Domain.Repositories;

public interface ICourseRepository : IGenericRepository<Course>
{
    /// <summary>
    /// Retrieves a course by its ID, including the instructor details.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<Course?> GetByIdWithInstructorAsync(string id, CancellationToken cancellationToken = default);
}
