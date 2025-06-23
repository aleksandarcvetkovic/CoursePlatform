using CoursePlatform.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using CoursePlatform.Mappers;

namespace CoursePlatform.Repositories;

public class InstructorRepository : GenericRepository<Instructor>, IInstructorRepository
{

    public InstructorRepository(CoursePlatformContext context) : base(context)
    {
    }

    public async Task<Instructor?> GetWithCoursesAsync(string id)
    {
        return await _dbSet
            .Include(i => i.Courses)
            .FirstOrDefaultAsync(i => i.Id == id);
    }

    public async Task<IEnumerable<InstructorResponseDTO>> GetAllDTOsAsync()
    {
        return await _dbSet
            .Select(i => i.ToInstructorResponseDTO())
            .ToListAsync();
    }

    public async Task<InstructorResponseDTO?> GetDTOByIdAsync(string id)
    {
        var instructor = await _dbSet.FindAsync(id);
        return instructor?.ToInstructorResponseDTO();
    }

    public async Task<InstructorWithCoursesDTO?> GetWithCoursesDTOAsync(string id)
    {
        var instructor = await _dbSet
            .Include(i => i.Courses)
            .FirstOrDefaultAsync(i => i.Id == id);
        return instructor?.ToInstructorWithCoursesDTO();
    }

}