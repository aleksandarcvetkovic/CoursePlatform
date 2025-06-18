using CoursePlatform.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

public class InstructorService : IInstructorService
{
    private readonly CoursePlatformContext _context;

    public InstructorService(CoursePlatformContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<InstructorResponseDTO>> GetAllInstructorsAsync()
    {
        return await _context.Instructors
            .Select(i => i.ToInstructorResponseDTO())
            .ToListAsync();
    }

    public async Task<InstructorResponseDTO> GetInstructorByIdAsync(string id)
    {
        var instructor = await _context.Instructors
            .Select(i => i.ToInstructorResponseDTO())
            .FirstOrDefaultAsync(i => i.Id == id);

        if (instructor == null)
            throw new NotFoundException("Instructor not found");

        return instructor;
    }

    public async Task<InstructorWithCoursesDTO> GetInstructorWithCoursesAsync(string id)
    {
        var instructor = await _context.Instructors
            .Include(i => i.Courses)
            .Select(i => i.ToInstructorWithCoursesDTO())
            .FirstOrDefaultAsync(i => i.Id == id);

        if (instructor == null)
            throw new NotFoundException("Instructor not found");

        return instructor;
    }

    public async Task<InstructorResponseDTO> CreateInstructorAsync(InstructorRequestDTO instructorDTO)
    {
        var instructor = instructorDTO.ToInstructor();
        _context.Instructors.Add(instructor);
        await _context.SaveChangesAsync();
        return instructor.ToInstructorResponseDTO();
    }

    public async Task UpdateInstructorAsync(string id, InstructorRequestDTO instructorDTO)
    {
        var instructor = await _context.Instructors.FindAsync(id);
        if (instructor == null)
            throw new NotFoundException("Instructor not found");

        instructor.UpdateFromDTO(instructorDTO);

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            throw new BadRequestException("Failed to update instructor.");
        }
    }

    public async Task DeleteInstructorAsync(string id)
    {
        var instructor = await _context.Instructors.FindAsync(id);
        if (instructor == null)
            throw new NotFoundException("Instructor not found");

        _context.Instructors.Remove(instructor);
        await _context.SaveChangesAsync();
    }
}