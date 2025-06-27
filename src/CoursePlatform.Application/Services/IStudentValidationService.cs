using CoursePlatform.Application.DTOs;

namespace CoursePlatform.Application.Services;
public interface IStudentValidationService
{
    Task<StudentValidationResult> ValidateStudentAsync(StudentRequestDTO student, CancellationToken cancellationToken = default);
}