using System.Collections.Generic;
using System.Threading.Tasks;
using CoursePlatform.Models;

namespace CoursePlatform.Services;

public interface IStudentService
{
    Task<IEnumerable<StudentResponseDTO>> GetAllStudentsAsync();
    Task<StudentResponseDTO> GetStudentByIdAsync(string id);
    Task<StudentWithEnrolmentsDTO> GetStudentWithEnrollmentsAsync(string id);
    Task UpdateStudentAsync(string id, StudentRequestDTO studentDTO);
    Task<StudentResponseDTO> CreateStudentAsync(StudentRequestDTO studentDTO);
    Task DeleteStudentAsync(string id);
}    

