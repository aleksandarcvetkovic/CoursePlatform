using CoursePlatform.Application;
using CoursePlatform.Infrastructure;
using CoursePlatform.Api.Presentation.Endpoints;
using CoursePlatform.Api.Common.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddOpenApi();

// Add layers
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerUi(options =>
    {
        options.DocumentPath = "/openapi/v1.json";
    });
}

app.UseMiddleware<GlobalExceptionMiddleware>();

app.UseHttpsRedirection();

app.UseAuthorization();

// Map endpoints
app.MapStudentEndpoints();
app.MapInstructorEndpoints();
app.MapCourseEndpoints();
app.MapEnrollmentEndpoints();

app.Run();
