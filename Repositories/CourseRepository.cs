using CoursePlatform.Models;
using Microsoft.EntityFrameworkCore;
using CoursePlatform.Mappers;
using Microsoft.EntityFrameworkCore.Storage;
namespace CoursePlatform.Repositories;

public class CourseRepository : GenericRepository<Course>, ICourseRepository
{

    public CourseRepository(CoursePlatformContext context) : base(context)
    {

    }

    public async Task<IEnumerable<CourseResponseDTO>> GetAllDTOAsync()
    {
        return await _dbSet
            .Include(c => c.Instructor)
            .Select(c => c.ToResponseDTO())
            .ToListAsync();
    }

    public async Task<CourseWithInstructorDTO?> GetWithInstructorAsync(string id)
    {
        return await _dbSet
            .Include(c => c.Instructor)
            .Where(c => c.Id == id)
            .Select(c => c.ToCourseWithInstructor())
            .FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<CourseWithInstructorDTO>> GetAllWithInstructorAsync()
    {
        return await _dbSet
            .Include(c => c.Instructor)
            .Select(c => c.ToCourseWithInstructor())
            .ToListAsync();
    }

    public async Task<bool> ExistsAsync(string id)
    {
        return await _dbSet.AnyAsync(c => c.Id == id);
    }
}