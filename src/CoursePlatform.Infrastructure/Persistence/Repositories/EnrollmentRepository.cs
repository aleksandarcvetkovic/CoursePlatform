using CoursePlatform.Domain.Entities;
using CoursePlatform.Domain.Repositories;
using CoursePlatform.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace CoursePlatform.Infrastructure.Persistence.Repositories;

public class EnrollmentRepository : GenericRepository<Enrollment>, IEnrollmentRepository
{
    public EnrollmentRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<bool> StudentAlreadyEnrolledAsync(string studentId, string courseId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .AnyAsync(e => e.StudentId == studentId && e.CourseId == courseId, cancellationToken);
    }
}
