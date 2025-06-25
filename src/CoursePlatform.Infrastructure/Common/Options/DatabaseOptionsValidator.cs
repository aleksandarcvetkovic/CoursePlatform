using Microsoft.Extensions.Options;

namespace CoursePlatform.Infrastructure.Common.Options;

public class DatabaseSettingsValidator : IValidateOptions<DatabaseOptions>
{
    public ValidateOptionsResult Validate(string? name, DatabaseOptions options)
    {
        if (string.IsNullOrEmpty(options.ConnectionString))
        {
            return ValidateOptionsResult.Fail("Connection string is required.");
        }

        if (options.MaxRetryCount < 0)
        {
            return ValidateOptionsResult.Fail("MaxRetryCount must be non-negative.");
        }

        if (options.MaxRetryDelaySeconds <= 0)
        {
            return ValidateOptionsResult.Fail("MaxRetryDelaySeconds must be positive.");
        }

        return ValidateOptionsResult.Success;
    }
}
