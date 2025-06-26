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
    /// List of field-specific validation errors
    /// </summary>
    public List<ValidationError> Errors { get; init; } = new();
    
    /// <summary>
    /// Creates a successful validation result
    /// </summary>
    /// <returns>A validation result indicating success</returns>
    public static StudentValidationResult Success() => new() { IsValid = true };
    
    /// <summary>
    /// Creates a failed validation result with field-specific errors
    /// </summary>
    /// <param name="errors">List of validation errors</param>
    /// <returns>A validation result indicating failure</returns>
    public static StudentValidationResult Failure(params ValidationError[] errors) => 
        new() 
        { 
            IsValid = false, 
            Errors = errors.ToList()
        };
    
    /// <summary>
    /// Creates a failed validation result with a single field error
    /// </summary>
    /// <param name="field">The field name</param>
    /// <param name="message">The error message</param>
    /// <returns>A validation result indicating failure</returns>
    public static StudentValidationResult Failure(string field, string message) => 
        new() 
        { 
            IsValid = false, 
            Errors = new List<ValidationError> { new() { Field = field, Message = message } }
        };
}

/// <summary>
/// Represents a specific validation error for a field
/// </summary>
public record ValidationError
{
    /// <summary>
    /// The field name that has the validation error
    /// </summary>
    public string Field { get; init; } = string.Empty;
    
    /// <summary>
    /// The validation error message
    /// </summary>
    public string Message { get; init; } = string.Empty;
}
