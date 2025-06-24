using CoursePlatform.Domain.Entities;

namespace CoursePlatform.Domain.Repositories;

public interface IEnrollmentRepository : IGenericRepository<Enrollment>
{
    // Entity-specific methods that are not covered by the generic repository
    Task<bool> StudentAlreadyEnrolledAsync(string studentId, string courseId, CancellationToken cancellationToken = default);
    Task<IEnumerable<Enrollment>> GetEnrollmentsByStudentAsync(string studentId, CancellationToken cancellationToken = default);
    Task<IEnumerable<Enrollment>> GetEnrollmentsByCourseAsync(string courseId, CancellationToken cancellationToken = default);
    Task<Enrollment?> GetEnrollmentWithDetailsAsync(string studentId, string courseId, CancellationToken cancellationToken = default);
}
