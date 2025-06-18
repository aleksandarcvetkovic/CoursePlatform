using CoursePlatform.Models;
using Microsoft.EntityFrameworkCore;
using CoursePlatform.Services;
using CoursePlatform.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddDbContext<CoursePlatformContext>(options =>
    options.UseSqlServer(builder.Configuration["DefaultConnection"]));

builder.Services.AddScoped<IStudentService, StudentService>();
builder.Services.AddScoped<IInstructorService, InstructorService>();
builder.Services.AddScoped<ICourseService, CourseService>();
builder.Services.AddScoped<IEnrollmentService, EnrollmentService>();
builder.Services.AddScoped<IStudentRepository, StudentRepository>();
builder.Services.AddScoped<IInstructorRepository, InstructorRepository>();
builder.Services.AddScoped<ICourseRepository, CourseRepository>();
builder.Services.AddScoped<IEnrollmentRepository, EnrollmentRepository>();

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

app.UseMiddleware<CoursePlatform.Middleware.GlobalExceptionMiddleware>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
