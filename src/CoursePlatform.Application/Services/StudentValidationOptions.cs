namespace CoursePlatform.Application.Services;

/// <summary>
/// Configuration options for the student validation service
/// </summary>
public class StudentValidationOptions
{
    /// <summary>
    /// The configuration section name in appsettings.json
    /// </summary>
    public const string SectionName = "StudentValidation";
    
    /// <summary>
    /// Base URL of the external validation service
    /// </summary>
    public string BaseUrl { get; set; } = string.Empty;
    
    /// <summary>
    /// API endpoint for student validation
    /// </summary>
    public string ValidationEndpoint { get; set; }
    
    /// <summary>
    /// HTTP client timeout in seconds
    /// </summary>
    public int TimeoutSeconds { get; set; } = 30;
    
    /// <summary>
    /// Optional API key for authentication
    /// </summary>
    public string? ApiKey { get; set; }
    
    /// <summary>
    /// Whether to enable retry logic for failed requests
    /// </summary>
    public bool EnableRetry { get; set; } = true;
    
    /// <summary>
    /// Maximum number of retry attempts
    /// </summary>
    public int MaxRetryAttempts { get; set; } = 3;
    
    /// <summary>
    /// Delay between retry attempts in seconds
    /// </summary>
    public int RetryDelaySeconds { get; set; } = 2;
}
