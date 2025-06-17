using CoursePlatform.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CoursePlatform.Services
{
    public interface IEnrollmentService
    {
        Task<IEnumerable<EnrollmentResponseDTO>> GetAllAsync();
        Task<EnrollmentResponseDTO> GetByIdAsync(string id);
        Task<EnrollmentWithStudentCourseDTO> GetWithStudentCourseAsync(string id);
        Task UpdateGradeAsync(string id, int grade);
        Task<EnrollmentResponseDTO> CreateAsync(EnrollmentRequestDTO dto);
        Task DeleteAsync(string id);
    }
}