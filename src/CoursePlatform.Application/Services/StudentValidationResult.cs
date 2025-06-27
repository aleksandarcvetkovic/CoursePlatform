namespace CoursePlatform.Application.Services;

/// <summary>
/// Represents the result of a student validation operation
/// </summary>
public record StudentValidationResult
{
    /// <summary>
    /// Indicates whether the student data passed validation
    /// </summary>
    public bool IsValid { get; init; }
    
    /// <summary>
    /// General error message if validation failed
    /// </summary>
    public string? ErrorMessage { get; init; }
    
    /// <summary>
    /// List of specific validation errors
    /// </summary>
    public List<string> ValidationErrors { get; init; } = new();
    
    /// <summary>
    /// Creates a successful validation result
    /// </summary>
    /// <returns>A validation result indicating success</returns>
    public static StudentValidationResult Success() => new() { IsValid = true };
    
    /// <summary>
    /// Creates a failed validation result with error details
    /// </summary>
    /// <param name="errorMessage">The main error message</param>
    /// <param name="validationErrors">Optional list of specific validation errors</param>
    /// <returns>A validation result indicating failure</returns>
    public static StudentValidationResult Failure(string errorMessage, List<string>? validationErrors = null) => 
        new() 
        { 
            IsValid = false, 
            ErrorMessage = errorMessage,
            ValidationErrors = validationErrors ?? new List<string>()
        };
}
