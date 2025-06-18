using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using CoursePlatform.Models;
using CoursePlatform.Repositories;

namespace CoursePlatform.Services;

public class StudentService : IStudentService
{
    private readonly IStudentRepository _studentRepository;

    public StudentService(IStudentRepository studentRepository)
    {
        _studentRepository = studentRepository;
    }

    public async Task<IEnumerable<StudentResponseDTO>> GetAllStudentsAsync()
    {
        return await _studentRepository.GetAllStudentDTOsAsync();
    }

    public async Task<StudentResponseDTO> GetStudentByIdAsync(string id)
    {
        var studentDTO = await _studentRepository.GetStudentDTOByIdAsync(id);

        if (studentDTO == null)
            throw new NotFoundException($"Student with ID '{id}' was not found.");

        return studentDTO;
    }

    public async Task<StudentWithEnrolmentsDTO> GetStudentWithEnrollmentsAsync(string id)
    {
        var studentDTO = await _studentRepository.GetStudentWithEnrollmentsDTOAsync(id);

        if (studentDTO == null)
            throw new NotFoundException($"Student with ID '{id}' was not found.");

        return studentDTO;
    }

    public async Task UpdateStudentAsync(string id, StudentRequestDTO studentDTO)
    {
        var student = await _studentRepository.GetByIdAsync(id);
        if (student == null)
            throw new NotFoundException($"Student with ID '{id}' was not found.");

        student.UpdateFromDTO(studentDTO);

        try
        {
            await _studentRepository.UpdateAsync(student);
        }
        catch (DbUpdateConcurrencyException)
        {
            throw new BadRequestException($"Failed to update student with ID '{id}'.");
        }
    }

    public async Task<StudentResponseDTO> CreateStudentAsync(StudentRequestDTO studentDTO)
    {
        var student = studentDTO.ToStudent();
        await _studentRepository.AddAsync(student);
        return student.ToStudentResponseDTO();
    }

    public async Task DeleteStudentAsync(string id)
    {
        var student = await _studentRepository.GetByIdAsync(id);
        if (student == null)
            throw new NotFoundException($"Student with ID '{id}' was not found.");

        await _studentRepository.DeleteAsync(student);
    }
}
