using CoursePlatform.Domain.Entities;

namespace CoursePlatform.Domain.Repositories;

public interface IInstructorRepository : IGenericRepository<Instructor>
{
    Task<Instructor?> GetByIdWithCoursesAsync(string id, CancellationToken cancellationToken = default);
}
