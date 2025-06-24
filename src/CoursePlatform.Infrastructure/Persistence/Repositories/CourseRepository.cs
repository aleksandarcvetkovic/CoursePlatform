using CoursePlatform.Domain.Entities;
using CoursePlatform.Domain.Repositories;
using CoursePlatform.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace CoursePlatform.Infrastructure.Persistence.Repositories;

public class CourseRepository : GenericRepository<Course>, ICourseRepository
{
    public CourseRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<Course?> GetByIdWithInstructorAsync(string id, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(c => c.Instructor)
            .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
    }

    public async Task<Course?> GetByIdWithEnrollmentsAsync(string id, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(c => c.Enrollments)
            .ThenInclude(e => e.Student)
            .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
    }

    public async Task<IEnumerable<Course>> GetCoursesByInstructorAsync(string instructorId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(c => c.InstructorId == instructorId)
            .Include(c => c.Instructor)
            .ToListAsync(cancellationToken);
    }    public async Task<IEnumerable<Course>> GetAvailableCoursesAsync(CancellationToken cancellationToken = default)
    {
        // For now, return all courses. You can add MaxStudents property to Course entity later
        return await _dbSet
            .Include(c => c.Instructor)
            .ToListAsync(cancellationToken);
    }
}
