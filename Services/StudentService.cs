using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using CoursePlatform.Models;

namespace CoursePlatform.Services
{
    public class StudentService : IStudentService
    {
        private readonly CoursePlatformContext _context;

        public StudentService(CoursePlatformContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<StudentResponseDTO>> GetAllStudentsAsync()
        {
            return await _context.Students.Select(s => s.ToStudentResponseDTO()).ToListAsync();
        }

        public async Task<StudentResponseDTO> GetStudentByIdAsync(string id)
        {
            var student = await _context.Students
                .Select(s => s.ToStudentResponseDTO())
                .FirstOrDefaultAsync(s => s.Id == id);

            if (student == null)
                throw new NotFoundException($"Student with ID '{id}' was not found.");

            return student;
        }

        public async Task<StudentWithEnrolmentsDTO> GetStudentWithEnrollmentsAsync(string id)
        {
            var student = await _context.Students
                .Include(s => s.Enrollments)
                .Select(s => s.ToStudentWithEnrolmentsDTO())
                .FirstOrDefaultAsync(s => s.Id == id);

            if (student == null)
                throw new NotFoundException($"Student with ID '{id}' was not found.");

            return student;
        }

        public async Task UpdateStudentAsync(string id, StudentRequestDTO studentDTO)
        {
            var studentEntry = await _context.Students.FindAsync(id);
            if (studentEntry == null)
                throw new NotFoundException($"Student with ID '{id}' was not found.");

            studentEntry.UpdateFromDTO(studentDTO);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw new BadRequestException($"Failed to update student with ID '{id}'.");
            }
        }

        public async Task<StudentResponseDTO> CreateStudentAsync(StudentRequestDTO studentDTO)
        {
            var student = studentDTO.ToStudent();
            _context.Students.Add(student);
            await _context.SaveChangesAsync();
            return student.ToStudentResponseDTO();
        }

        public async Task DeleteStudentAsync(string id)
        {
            var student = await _context.Students.FindAsync(id);
            if (student == null)
                throw new NotFoundException($"Student with ID '{id}' was not found.");

            _context.Students.Remove(student);
            await _context.SaveChangesAsync();
        }
    }
}