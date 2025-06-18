using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using CoursePlatform.Models;
using System.Linq;

namespace CoursePlatform.Repositories;

public class StudentRepository : IStudentRepository
{
    private readonly CoursePlatformContext _context;

    public StudentRepository(CoursePlatformContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Student>> GetAllAsync()
    {
        return await _context.Students.ToListAsync();
    }

    public async Task<Student?> GetByIdAsync(string id)
    {
        return await _context.Students.FindAsync(id);
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

    public async Task AddAsync(Student student)
    {
        await _context.Students.AddAsync(student);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Student student)
    {
        _context.Students.Update(student);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Student student)
    {
        _context.Students.Remove(student);
        await _context.SaveChangesAsync();
    }
}
    

