using CoursePlatform.Application.DTOs;
using CoursePlatform.Application.Services.Internal;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Net.Http.Json;

namespace CoursePlatform.Application.Services;

public class StudentValidationService : IStudentValidationService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<StudentValidationService> _logger;
    private readonly StudentValidationOptions _options;

    public StudentValidationService(
        HttpClient httpClient,
        ILogger<StudentValidationService> logger,
        IOptions<StudentValidationOptions> options)
    {
        _httpClient = httpClient;
        _logger = logger;
        _options = options.Value;
    }

    public async Task<StudentValidationResult> ValidateStudentAsync(
        StudentRequestDTO student, 
        CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Validating student: {Email}", student.Email);

            var request = new { student.Name, student.Email };

            var response = await _httpClient.PostAsJsonAsync(
                _options.ValidationEndpoint, 
                request, 
                cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                var errorMessage = $"Validation service returned {response.StatusCode}";
                _logger.LogError("Validation failed with status: {StatusCode}", response.StatusCode);
                return StudentValidationResult.Failure(errorMessage, new List<string> { errorMessage });
            }

            var result = await response.Content.ReadFromJsonAsync<ValidationApiResponse>(cancellationToken);
            
            if (result == null)
            {
                const string errorMessage = "Received null response from validation service";
                _logger.LogWarning(errorMessage);
                return StudentValidationResult.Failure(errorMessage, new List<string> { errorMessage });
            }

            _logger.LogInformation("Validation completed for {Email}: {IsValid}", student.Email, result.IsValid);

            if (result.IsValid)
            {
                return StudentValidationResult.Success();
            }
            else
            {
                return StudentValidationResult.Failure(
                    result.ErrorMessage ?? "Validation failed",
                    result.ValidationErrors ?? new List<string> { result.ErrorMessage ?? "Validation failed" });
            }
        }
        catch (Exception ex)
        {
            var errorMessage = $"Error validating student {student.Email}: {ex.Message}";
            _logger.LogError(ex, "Error validating student {Email}", student.Email);
            return StudentValidationResult.Failure(errorMessage, new List<string> { errorMessage });
        }
    }
}
