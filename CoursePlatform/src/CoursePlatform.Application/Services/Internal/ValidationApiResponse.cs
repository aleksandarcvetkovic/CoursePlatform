namespace CoursePlatform.Application.Services.Internal;

/// <summary>
/// Internal model representing the response from the external validation API
/// </summary>
internal record ValidationApiResponse
{
 
    public bool IsValid { get; init; }

    public string? Message { get; init; }
}


