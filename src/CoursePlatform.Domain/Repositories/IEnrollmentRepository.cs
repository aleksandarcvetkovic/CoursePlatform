using CoursePlatform.Domain.Entities;

namespace CoursePlatform.Domain.Repositories;

public interface IEnrollmentRepository : IGenericRepository<Enrollment>
{
    Task<bool> StudentAlreadyEnrolledAsync(string studentId, string courseId, CancellationToken cancellationToken = default);
}
