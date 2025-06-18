using CoursePlatform.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using CoursePlatform.Repositories;
namespace CoursePlatform.Services;

public class InstructorService : IInstructorService
{
    private readonly IInstructorRepository _repository;

    public InstructorService(IInstructorRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<InstructorResponseDTO>> GetAllInstructorsAsync()
    {
        return await _repository.GetAllDTOsAsync();
    }

    public async Task<InstructorResponseDTO> GetInstructorByIdAsync(string id)
    {
        var instructorDTO = await _repository.GetDTOByIdAsync(id);
        if (instructorDTO == null)
            throw new NotFoundException("Instructor not found");
        return instructorDTO;
    }

    public async Task<InstructorWithCoursesDTO> GetInstructorWithCoursesAsync(string id)
    {
        var instructorDTO = await _repository.GetWithCoursesDTOAsync(id);
        if (instructorDTO == null)
            throw new NotFoundException("Instructor not found");
        return instructorDTO;
    }

    public async Task<InstructorResponseDTO> CreateInstructorAsync(InstructorRequestDTO instructorDTO)
    {
        var instructor = instructorDTO.ToInstructor();
        await _repository.AddAsync(instructor);
        await _repository.SaveChangesAsync();
        return instructor.ToInstructorResponseDTO();
    }

    public async Task UpdateInstructorAsync(string id, InstructorRequestDTO instructorDTO)
    {
        var instructor = await _repository.GetByIdAsync(id);
        if (instructor == null)
            throw new NotFoundException("Instructor not found");

        instructor.UpdateFromDTO(instructorDTO);

        try
        {
            await _repository.UpdateAsync(instructor);
            await _repository.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            throw new BadRequestException("Failed to update instructor.");
        }
    }

    public async Task DeleteInstructorAsync(string id)
    {
        var instructor = await _repository.GetByIdAsync(id);
        if (instructor == null)
            throw new NotFoundException("Instructor not found");

        await _repository.DeleteAsync(instructor);
        await _repository.SaveChangesAsync();
    }
}