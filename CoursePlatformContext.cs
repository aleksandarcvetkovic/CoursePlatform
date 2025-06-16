using Microsoft.EntityFrameworkCore;
using CoursePlatform.Models;

namespace CoursePlatform.Models;

public class CoursePlatformContext : DbContext
{
    public CoursePlatformContext(DbContextOptions<CoursePlatformContext> options)
        : base(options)
    {

    }

        public DbSet<Course> Courses { get; set; }
        public DbSet<Instructor> Instructors { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Enrollment> Enrollments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            
        }
        
}