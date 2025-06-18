using CoursePlatform.Models;
using Microsoft.EntityFrameworkCore;

using System.Collections.Generic;
using System.Threading.Tasks;

namespace CoursePlatform.Repositories;

public class EnrollmentRepository : IEnrollmentRepository
{
    private readonly CoursePlatformContext _context;

    public EnrollmentRepository(CoursePlatformContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Enrollment>> GetAllAsync()
    {
        return await _context.Enrollments.ToListAsync();
    }

    public async Task<Enrollment?> GetByIdAsync(string id)
    {
        return await _context.Enrollments.FindAsync(id);
    }

    public async Task<Enrollment?> GetWithStudentCourseAsync(string id)
    {
        return await _context.Enrollments
            .Include(e => e.Course)
            .Include(e => e.Student)
            .FirstOrDefaultAsync(e => e.Id == id);
    }

    public async Task AddAsync(Enrollment enrollment)
    {
        await _context.Enrollments.AddAsync(enrollment);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Enrollment enrollment)
    {
        _context.Enrollments.Update(enrollment);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Enrollment enrollment)
    {
        _context.Enrollments.Remove(enrollment);
        await _context.SaveChangesAsync();
    }

    
}
