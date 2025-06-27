
namespace CoursePlatform.Application.Common.Options;

public class StudentValidationOptions
{
    public const string SectionName = "StudentValidation";

    public string BaseUrl { get; set; } = string.Empty;
    public string ValidationEndpoint { get; set; } = "/api/students/validate";
    public int TimeoutSeconds { get; set; } = 30;
    public string? ApiKey { get; set; }
}