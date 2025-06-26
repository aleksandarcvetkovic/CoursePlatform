using CoursePlatform.Application.DTOs;

namespace CoursePlatform.Application.Services;

/// <summary>
/// Service for validating student data with an external validation API
/// </summary>
public interface IStudentValidationService
{
    /// <summary>
    /// Validates student data asynchronously using an external service
    /// </summary>
    /// <param name="student">The student data to validate</param>
    /// <param name="cancellationToken">Cancellation token for the operation</param>
    /// <returns>A StudentValidationResult containing validation status and any error messages</returns>
    Task<StudentValidationResult> ValidateStudentAsync(StudentRequestDTO student, CancellationToken cancellationToken = default);
}