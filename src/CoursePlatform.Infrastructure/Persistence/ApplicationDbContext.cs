using CoursePlatform.Domain.Entities;
using CoursePlatform.Domain.Common;
using Microsoft.EntityFrameworkCore;

namespace CoursePlatform.Infrastructure.Persistence;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Course> Courses => Set<Course>();
    public DbSet<Instructor> Instructors => Set<Instructor>();
    public DbSet<Student> Students => Set<Student>();
    public DbSet<Enrollment> Enrollments => Set<Enrollment>();
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Configure GUID generation for all entities that inherit from BaseEntity
        ConfigureBaseEntity<Course>(modelBuilder);
        ConfigureBaseEntity<Instructor>(modelBuilder);
        ConfigureBaseEntity<Student>(modelBuilder);
        ConfigureBaseEntity<Enrollment>(modelBuilder);

        // Configure relationships
        ConfigureRelationships(modelBuilder);
    }

    private void ConfigureBaseEntity<T>(ModelBuilder modelBuilder) where T : BaseEntity
    {
        modelBuilder.Entity<T>()
            .Property(e => e.Id)
            .HasDefaultValueSql("NEWID()") // For SQL Server
            .ValueGeneratedOnAdd();
    }

    private void ConfigureRelationships(ModelBuilder modelBuilder)
    {
        
    }
}
