using CoursePlatform.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CoursePlatform.Services;

public interface IInstructorService
{
    Task<IEnumerable<InstructorResponseDTO>> GetAllInstructorsAsync();
    Task<InstructorResponseDTO> GetInstructorByIdAsync(string id);
    Task<InstructorWithCoursesDTO> GetInstructorWithCoursesAsync(string id);
    Task<InstructorResponseDTO> CreateInstructorAsync(InstructorRequestDTO instructorDTO);
    Task UpdateInstructorAsync(string id, InstructorRequestDTO instructorDTO);
    Task DeleteInstructorAsync(string id);
}