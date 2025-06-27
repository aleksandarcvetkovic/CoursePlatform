using CoursePlatform.Application.Common.Options;
using CoursePlatform.Application.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Net;


namespace CoursePlatform.Application.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddStudentValidationHttpClient(
        this IServiceCollection services, 
        IConfiguration configuration)
    {
        
        services.Configure<StudentValidationOptions>(configuration.GetSection(StudentValidationOptions.SectionName));

        services.AddHttpClient<IStudentValidationService, StudentValidationService>((serviceProvider, client) =>
        {
            var options = serviceProvider.GetRequiredService<IOptions<StudentValidationOptions>>().Value;
            
            client.BaseAddress = new Uri(options.BaseUrl);
            client.Timeout = TimeSpan.FromSeconds(options.TimeoutSeconds);
            
            // Authentication
            if (!string.IsNullOrEmpty(options.ApiKey))
            {
                client.DefaultRequestHeaders.Add("X-API-Key", options.ApiKey);
            }
            
            // Standard headers
            client.DefaultRequestHeaders.Add("Accept", "application/json");
            client.DefaultRequestHeaders.Add("User-Agent", "CoursePlatform/1.0");
        })
        .SetHandlerLifetime(TimeSpan.FromMinutes(5)); // Proper handler lifecycle

        return services;
    }
}
