using CoursePlatform.Models;
using Microsoft.EntityFrameworkCore;

namespace CoursePlatform.Repositories;

public class CourseRepository : ICourseRepository
{
    private readonly CoursePlatformContext _context;

    public CourseRepository(CoursePlatformContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<CourseResponseDTO>> GetAllAsync()
    {
        return await _context.Courses
            .Include(c => c.Instructor)
            .Select(c => c.ToResponseDTO())
            .ToListAsync();
    }

    public async Task<Course?> GetByIdAsync(string id)
    {
        var course = await _context.Courses.FindAsync(id);
        return course;
    }

    public async Task<CourseWithInstructorDTO?> GetWithInstructorAsync(string id)
    {
        return await _context.Courses
            .Include(c => c.Instructor)
            .Where(c => c.Id == id)
            .Select(c => c.ToCourseWithInstructor())
            .FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<CourseWithInstructorDTO>> GetAllWithInstructorAsync()
    {
        return await _context.Courses
            .Include(c => c.Instructor)
            .Select(c => c.ToCourseWithInstructor())
            .ToListAsync();
    }

    public async Task AddAsync(Course course)
    {
        await _context.Courses.AddAsync(course);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Course course)
    {
        _context.Courses.Update(course);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Course course)
    {
        _context.Courses.Remove(course);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> ExistsAsync(string id)
    {
        return await _context.Courses.AnyAsync(c => c.Id == id);
    }
}