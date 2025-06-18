using CoursePlatform.Models;
using CoursePlatform.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CoursePlatform.Services;

public class EnrollmentService : IEnrollmentService
{
    private readonly IEnrollmentRepository _repository;
    private readonly IStudentRepository _studentRepository;
    private readonly ICourseRepository _courseRepository;

    public EnrollmentService(
        IEnrollmentRepository repository,
        IStudentRepository studentRepository,
        ICourseRepository courseRepository)
    {
        _repository = repository;
        _studentRepository = studentRepository;
        _courseRepository = courseRepository;
    }

    public async Task<IEnumerable<EnrollmentResponseDTO>> GetAllAsync()
    {
        var enrollments = await _repository.GetAllAsync();
        var result = new List<EnrollmentResponseDTO>();
        foreach (var e in enrollments)
        {
            result.Add(e.ToEnrolmentResponseDTO());
        }
        return result;
    }

    public async Task<EnrollmentResponseDTO> GetByIdAsync(string id)
    {
        var enrollment = await _repository.GetByIdAsync(id);
        if (enrollment == null)
            throw new NotFoundException($"Enrollment with id {id} not found.");

        return enrollment.ToEnrolmentResponseDTO();
    }

    public async Task<EnrollmentWithStudentCourseDTO> GetWithStudentCourseAsync(string id)
    {
        var enrollment = await _repository.GetWithStudentCourseAsync(id);
        if (enrollment == null)
            throw new NotFoundException($"Enrollment with id {id} not found.");

        return enrollment.ToEnrollmentWithStudentCourseDTO();
    }

    public async Task UpdateGradeAsync(string id, int grade)
    {
        var enrollment = await _repository.GetByIdAsync(id);
        if (enrollment == null)
            throw new NotFoundException($"Enrollment with id {id} not found.");

        enrollment.UpdateFromDTO(new EnrollmentGradeRequestDTO
        {
            Id = id,
            Grade = grade
        });

        await _repository.UpdateAsync(enrollment);
    }

    public async Task<EnrollmentResponseDTO> CreateAsync(EnrollmentRequestDTO dto)
    {
        // Check if Student exists using StudentRepository
        var student = await _studentRepository.GetByIdAsync(dto.StudentId);
        if (student == null)
            throw new NotFoundException($"Student with id {dto.StudentId} not found.");

        // Check if Course exists using CourseRepository
        var course = await _courseRepository.GetByIdAsync(dto.CourseId);
        if (course == null)
            throw new NotFoundException($"Course with id {dto.CourseId} not found.");

        var newEnrollment = dto.ToEnrolment();
        newEnrollment.EnrolledOn = DateTime.Now;

        await _repository.AddAsync(newEnrollment);

        return newEnrollment.ToEnrolmentResponseDTO();
    }

    public async Task DeleteAsync(string id)
    {
        var enrollment = await _repository.GetByIdAsync(id);
        if (enrollment == null)
            throw new NotFoundException($"Enrollment with id {id} not found.");

        await _repository.DeleteAsync(enrollment);
    }
}
