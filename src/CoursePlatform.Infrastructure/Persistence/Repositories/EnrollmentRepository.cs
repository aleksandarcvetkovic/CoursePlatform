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

    public async Task<IEnumerable<Enrollment>> GetEnrollmentsByStudentAsync(string studentId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(e => e.StudentId == studentId)
            .Include(e => e.Course)
            .ThenInclude(c => c.Instructor)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Enrollment>> GetEnrollmentsByCourseAsync(string courseId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(e => e.CourseId == courseId)
            .Include(e => e.Student)
            .ToListAsync(cancellationToken);
    }

    public async Task<Enrollment?> GetEnrollmentWithDetailsAsync(string studentId, string courseId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(e => e.Student)
            .Include(e => e.Course)
            .ThenInclude(c => c.Instructor)
            .FirstOrDefaultAsync(e => e.StudentId == studentId && e.CourseId == courseId, cancellationToken);
    }
}
