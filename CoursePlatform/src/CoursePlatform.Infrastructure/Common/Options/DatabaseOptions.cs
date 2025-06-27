namespace CoursePlatform.Infrastructure.Common.Options;

public class DatabaseOptions
{
    public const string SectionName = "DatabaseOptions";
    
    public string ConnectionString { get; set; } = string.Empty;
    public int MaxRetryCount { get; set; } = 5;
    public int MaxRetryDelaySeconds { get; set; } = 30;
    public bool EnableRetryOnFailure { get; set; } = true;
}
