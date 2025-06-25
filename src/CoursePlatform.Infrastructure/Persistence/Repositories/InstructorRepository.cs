using CoursePlatform.Domain.Entities;
using CoursePlatform.Domain.Repositories;
using CoursePlatform.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace CoursePlatform.Infrastructure.Persistence.Repositories;

public class InstructorRepository : GenericRepository<Instructor>, IInstructorRepository
{
    public InstructorRepository(ApplicationDbContext context) : base(context)
    {
    }    public async Task<Instructor?> GetByIdWithCoursesAsync(string id, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(i => i.Courses)
            .ThenInclude(c => c.Enrollments)
            .FirstOrDefaultAsync(i => i.Id == id, cancellationToken);
    }
}
