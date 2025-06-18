using CoursePlatform.Models;
using Microsoft.EntityFrameworkCore;
using CoursePlatform.Mappers;
namespace CoursePlatform.Repositories;

public class CourseRepository : GenericRepository<Course>, ICourseRepository
{
    private readonly CoursePlatformContext _context;

    public CourseRepository(CoursePlatformContext context) : base(context)
    {
        _context = context;
    }

    public async Task<IEnumerable<CourseResponseDTO>> GetAllDTOAsync()
    {
        return await _context.Courses
            .Include(c => c.Instructor)
            .Select(c => c.ToResponseDTO())
            .ToListAsync();
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

    public async Task<bool> ExistsAsync(string id)
    {
        return await _context.Courses.AnyAsync(c => c.Id == id);
    }
}