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
    /// Error message from the external service if validation failed
    /// </summary>
    public string? ErrorMessage { get; init; }
    
    /// <summary>
    /// List of specific validation errors from the external service
    /// </summary>
    public List<string> ValidationErrors { get; init; } = new();
}
