using CoursePlatform.Domain.Entities;
using CoursePlatform.Domain.Repositories;
using CoursePlatform.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace CoursePlatform.Infrastructure.Persistence.Repositories;

public class StudentRepository : GenericRepository<Student>, IStudentRepository
{
    public StudentRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<Student?> GetByIdWithEnrollmentsAsync(string id, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(s => s.Enrollments)
            .ThenInclude(e => e.Course)
            .FirstOrDefaultAsync(s => s.Id == id, cancellationToken);
    }

    public async Task<Student?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .FirstOrDefaultAsync(s => s.Email == email, cancellationToken);
    }    public async Task<IEnumerable<Student>> GetActiveStudentsAsync(CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(s => s.Enrollments.Any(e => e.EnrolledOn > DateTime.UtcNow.AddMonths(-6)))
            .ToListAsync(cancellationToken);
    }
}
