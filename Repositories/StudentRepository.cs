using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using CoursePlatform.Models;
using System.Linq;
using CoursePlatform.Mappers;


namespace CoursePlatform.Repositories;

public class StudentRepository : GenericRepository<Student>, IStudentRepository
{
    public StudentRepository(CoursePlatformContext context) : base(context)
    {
    }

    public async Task<Student?> GetWithEnrollmentsAsync(string id)
    {
        return await _dbSet
            .Include(s => s.Enrollments)
            .FirstOrDefaultAsync(s => s.Id == id);
    }

    public async Task<IEnumerable<StudentResponseDTO>> GetAllStudentDTOsAsync()
    {
        return await _dbSet
            .Select(s => s.ToStudentResponseDTO())
            .ToListAsync();
    }

    public async Task<StudentResponseDTO?> GetStudentDTOByIdAsync(string id)
    {
        var student = await _dbSet
            .Where(s => s.Id == id)
            .Select(s => s.ToStudentResponseDTO())
            .FirstOrDefaultAsync();
        return student;
    }

    public async Task<StudentWithEnrolmentsDTO?> GetStudentWithEnrollmentsDTOAsync(string id)
    {
        var student = await _dbSet
            .Include(s => s.Enrollments)
            .Where(s => s.Id == id)
            .Select(s => s.ToStudentWithEnrolmentsDTO())
            .FirstOrDefaultAsync();
        return student;
    }

}


