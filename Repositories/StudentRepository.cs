using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using CoursePlatform.Models;
using System.Linq;
using CoursePlatform.Mappers;


namespace CoursePlatform.Repositories;

public class StudentRepository : GenericRepository<Student>, IStudentRepository
{
    private readonly CoursePlatformContext _context;

    public StudentRepository(CoursePlatformContext context) : base(context)
    {
        _context = context;
    }

    public async Task<Student?> GetWithEnrollmentsAsync(string id)
    {
        return await _context.Students
            .Include(s => s.Enrollments)
            .FirstOrDefaultAsync(s => s.Id == id);
    }

    public async Task<IEnumerable<StudentResponseDTO>> GetAllStudentDTOsAsync()
    {
        return await _context.Students
            .Select(s => s.ToStudentResponseDTO())
            .ToListAsync();
    }

    public async Task<StudentResponseDTO?> GetStudentDTOByIdAsync(string id)
    {
        var student = await _context.Students
            .Where(s => s.Id == id)
            .Select(s => s.ToStudentResponseDTO())
            .FirstOrDefaultAsync();
        return student;
    }

    public async Task<StudentWithEnrolmentsDTO?> GetStudentWithEnrollmentsDTOAsync(string id)
    {
        var student = await _context.Students
            .Include(s => s.Enrollments)
            .Where(s => s.Id == id)
            .Select(s => s.ToStudentWithEnrolmentsDTO())
            .FirstOrDefaultAsync();
        return student;
    }

}


