using CoursePlatform.Domain.Repositories;
using CoursePlatform.Infrastructure.Common.Options;
using CoursePlatform.Infrastructure.Persistence;
using CoursePlatform.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace CoursePlatform.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureExtension(this IServiceCollection services, IConfiguration configuration)
    {
        // Configure the DatabaseOptions options
        services.Configure<DatabaseOptions>(configuration.GetSection("DatabaseOptions"));
        services.AddSingleton<IValidateOptions<DatabaseOptions>, DatabaseSettingsValidator>();
        
        services.AddDbContext<ApplicationDbContext>((serviceProvider, options) =>
        {
            var databaseSettings = serviceProvider.GetRequiredService<IOptions<DatabaseOptions>>().Value;
            
            if (string.IsNullOrEmpty(databaseSettings.ConnectionString))
            {
                throw new InvalidOperationException("Database connection string is null or empty! Check your appsettings.json configuration.");
            }
            
            if (databaseSettings.EnableRetryOnFailure)
            {
                options.UseSqlServer(databaseSettings.ConnectionString,
                    sqlOptions => sqlOptions.EnableRetryOnFailure(
                        maxRetryCount: databaseSettings.MaxRetryCount,
                        maxRetryDelay: TimeSpan.FromSeconds(databaseSettings.MaxRetryDelaySeconds),
                        errorNumbersToAdd: null));
            }
            else
            {
                options.UseSqlServer(databaseSettings.ConnectionString);
            }
        });

        // Register repositories
        services.AddScoped<IStudentRepository, StudentRepository>();
        services.AddScoped<ICourseRepository, CourseRepository>();
        services.AddScoped<IInstructorRepository, InstructorRepository>();
        services.AddScoped<IEnrollmentRepository, EnrollmentRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        return services;
    }
}
