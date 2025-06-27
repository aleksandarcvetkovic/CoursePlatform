using FluentValidation;
using ValidatorService.Models;
using ValidatorService.Services;
using ValidatorService.Validators;

var builder = WebApplication.CreateBuilder(args);

// Add services
builder.Services.AddScoped<IValidator<Student>, StudentValidator>();
builder.Services.AddScoped<IValidatorService, StudentValidatorService>();

// Add Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add CORS
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();

// Configure Swagger (available in all environments for testing)
app.UseSwagger();
app.UseSwaggerUI();

app.UseCors();

// Validation endpoints
app.MapPost("/api/validate/student", async (Student student, IValidatorService validatorService) =>
{
    var result = await validatorService.ValidateStudentAsync(student);
    
    if (result.IsValid)
    {
        Console.WriteLine("Student data is valid.");
        Console.WriteLine($"Name: {student.Name}, Email: {student.Email}");
        return Results.Ok(new { IsValid = true, Message = "Student data is valid" });
    }
    Console.WriteLine("Student data is invalid!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
    Console.WriteLine($"Name: {student.Name}, Email: {student.Email}");
    var errorMessages = string.Join(";   ", result.Errors.Select(e => $"Validation error on atribute {e.PropertyName} : {e.ErrorMessage}"));
    return Results.Ok(new
    {
        IsValid = false,
        Message = errorMessages
    });
})
.WithName("ValidateStudent");

app.MapGet("/api/health", () => Results.Ok(new { Status = "Healthy", Service = "Validator Service" }))
.WithName("HealthCheck");

app.Run();
