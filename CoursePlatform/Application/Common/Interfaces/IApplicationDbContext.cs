using CoursePlatform.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CoursePlatform.Application.Common.Interfaces;

public interface IApplicationDbContext
{
    DbSet<Student> Students { get; }
    DbSet<Instructor> Instructors { get; }
    DbSet<Course> Courses { get; }
    DbSet<Enrollment> Enrollments { get; }
    
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
