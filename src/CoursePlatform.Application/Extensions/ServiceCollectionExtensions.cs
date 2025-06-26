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
        // Configure options with validation
        services.Configure<StudentValidationOptions>(configuration.GetSection(StudentValidationOptions.SectionName));

        // Configure HTTP client with typed client pattern and best practices
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
        .ConfigurePrimaryHttpMessageHandler(() =>
        {
            return new HttpClientHandler()
            {
                // Enable compression
                AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate,
                // Connection management
                MaxConnectionsPerServer = 10,
                // Security
                CheckCertificateRevocationList = true
            };
        })
        .SetHandlerLifetime(TimeSpan.FromMinutes(5)); // Proper handler lifecycle

        return services;
    }
}
