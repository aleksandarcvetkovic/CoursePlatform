using CoursePlatform.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using CoursePlatform.Mappers;

namespace CoursePlatform.Repositories;

public class InstructorRepository : GenericRepository<Instructor>, IInstructorRepository
{
    private readonly CoursePlatformContext _context;

    public InstructorRepository(CoursePlatformContext context) : base(context)
    {
        _context = context;
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

}