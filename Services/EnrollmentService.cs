using CoursePlatform.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CoursePlatform.Services
{
    public class EnrollmentService : IEnrollmentService
    {
        private readonly CoursePlatformContext _context;

        public EnrollmentService(CoursePlatformContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<EnrollmentResponseDTO>> GetAllAsync()
        {
            return await _context.Enrollments
                .Select(e => e.ToEnrolmentResponseDTO())
                .ToListAsync();
        }

        public async Task<EnrollmentResponseDTO> GetByIdAsync(string id)
        {
            var enrollment = await _context.Enrollments
                .Select(e => e.ToEnrolmentResponseDTO())
                .FirstOrDefaultAsync(e => e.Id == id);

            if (enrollment == null)
                throw new NotFoundException($"Enrollment with id {id} not found.");

            return enrollment;
        }

        public async Task<EnrollmentWithStudentCourseDTO> GetWithStudentCourseAsync(string id)
        {
            var enrollment = await _context.Enrollments
                .Include(e => e.Course)
                .Include(e => e.Student)
                .Select(e => e.ToEnrollmentWithStudentCourseDTO())
                .FirstOrDefaultAsync(e => e.Id == id);

            if (enrollment == null)
                throw new NotFoundException($"Enrollment with id {id} not found.");

            return enrollment;
        }

        public async Task UpdateGradeAsync(string id, int grade)
        {
            var enrollment = await _context.Enrollments.FindAsync(id);
            if (enrollment == null)
                throw new NotFoundException($"Enrollment with id {id} not found.");

            enrollment.UpdateFromDTO(new EnrollmentGradeRequestDTO
            {
                Id = id,
                Grade = grade
            });

            await _context.SaveChangesAsync();
        }

        public async Task<EnrollmentResponseDTO> CreateAsync(EnrollmentRequestDTO dto)
        {
            // Check if Student exists
            var studentExists = await _context.Students.AnyAsync(s => s.Id == dto.StudentId);
            if (!studentExists)
            throw new NotFoundException($"Student with id {dto.StudentId} not found.");

            // Check if Course exists
            var courseExists = await _context.Courses.AnyAsync(c => c.Id == dto.CourseId);
            if (!courseExists)
            throw new NotFoundException($"Course with id {dto.CourseId} not found.");

            var newEnrollment = dto.ToEnrolment();
            newEnrollment.EnrolledOn = DateTime.Now;

            _context.Enrollments.Add(newEnrollment);
            await _context.SaveChangesAsync();

            return newEnrollment.ToEnrolmentResponseDTO();
        }

        public async Task DeleteAsync(string id)
        {
            var enrollment = await _context.Enrollments.FindAsync(id);
            if (enrollment == null)
                throw new NotFoundException($"Enrollment with id {id} not found.");

            _context.Enrollments.Remove(enrollment);
            await _context.SaveChangesAsync();
        }
    }
}