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

public DbSet<CoursePlatform.Models.EnrollmentDTO> EnrollmentDTO { get; set; } = default!;

public DbSet<InstructorDTO> InstructorDTO { get; set; } = default!;

public DbSet<CoursePlatform.Models.StudentDTO> StudentDTO { get; set; } = default!;
/*
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Enrollment>()
                .HasOne(e => e.Course)
                .WithMany(c => c.Enrollments)
                .HasForeignKey(e => e.CourseId);

            modelBuilder.Entity<Enrollment>()
                .HasOne(e => e.Student)
                .WithMany(s => s.Enrollments)
                .HasForeignKey(e => e.StudentId);

            modelBuilder.Entity<Course>()
                .HasOne(c => c.Instructor)
                .WithMany(i => i.Courses)
                .HasForeignKey(c => c.InstructorId);
        }
        */
}