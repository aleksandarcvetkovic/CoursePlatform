using CoursePlatform.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace CoursePlatform.Repositories;

public class InstructorRepository : IInstructorRepository
{
    private readonly CoursePlatformContext _context;

    public InstructorRepository(CoursePlatformContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Instructor>> GetAllAsync()
    {
        return await _context.Instructors.ToListAsync();
    }

    public async Task<Instructor?> GetByIdAsync(string id)
    {
        return await _context.Instructors.FindAsync(id);
    }

    public async Task<Instructor?> GetWithCoursesAsync(string id)
    {
        return await _context.Instructors
            .Include(i => i.Courses)
            .FirstOrDefaultAsync(i => i.Id == id);
    }

    public async Task<IEnumerable<InstructorResponseDTO>> GetAllDTOsAsync()
    {
        return await _context.Instructors
            .Select(i => i.ToInstructorResponseDTO())
            .ToListAsync();
    }

    public async Task<InstructorResponseDTO?> GetDTOByIdAsync(string id)
    {
        var instructor = await _context.Instructors.FindAsync(id);
        return instructor?.ToInstructorResponseDTO();
    }

    public async Task<InstructorWithCoursesDTO?> GetWithCoursesDTOAsync(string id)
    {
        var instructor = await _context.Instructors
            .Include(i => i.Courses)
            .FirstOrDefaultAsync(i => i.Id == id);
        return instructor?.ToInstructorWithCoursesDTO();
    }

    public async Task AddAsync(Instructor instructor)
    {
        await _context.Instructors.AddAsync(instructor);
    }

    public async Task UpdateAsync(Instructor instructor)
    {
        _context.Instructors.Update(instructor);
    }

    public async Task DeleteAsync(Instructor instructor)
    {
        _context.Instructors.Remove(instructor);
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}