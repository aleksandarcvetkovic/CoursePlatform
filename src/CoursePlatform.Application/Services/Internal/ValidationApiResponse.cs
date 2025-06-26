namespace CoursePlatform.Application.Services.Internal;

/// <summary>
/// Internal model representing the response from the external validation API
/// </summary>
internal record ValidationApiResponse
{
    /// <summary>
    /// Indicates if the student data is valid according to the external service
    /// </summary>
    public bool IsValid { get; init; }
    
    /// <summary>
    /// List of field-specific validation errors from the external service
    /// </summary>
    public List<ValidationApiError> Errors { get; init; } = new();
}

/// <summary>
/// Represents a validation error from the external API
/// </summary>
internal record ValidationApiError
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
