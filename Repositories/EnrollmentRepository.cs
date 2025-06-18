using CoursePlatform.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using CoursePlatform.Mappers;

namespace CoursePlatform.Repositories;

public class EnrollmentRepository : GenericRepository<Enrollment>, IEnrollmentRepository
{
    private readonly CoursePlatformContext _context;

    public EnrollmentRepository(CoursePlatformContext context) : base(context)
    {
        _context = context;
    }

    public async Task<IEnumerable<EnrollmentResponseDTO>> GetAllDTOsAsync()
    {
        return await _context.Enrollments
            .Include(e => e.Course)
            .Include(e => e.Student)
            .Select(e => e.ToEnrollmentResponseDTO())
            .ToListAsync();
        
    }

    public async Task<EnrollmentWithStudentCourseDTO?> GetWithStudentCourseAsync(string id)
    {
        return await _context.Enrollments
            .Include(e => e.Course)
            .Include(e => e.Student)
            .Select(e => e.ToEnrollmentWithStudentCourse())
            .FirstOrDefaultAsync(e => e.Id == id);
    }

    
}
