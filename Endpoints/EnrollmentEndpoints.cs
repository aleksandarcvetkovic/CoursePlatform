using CoursePlatform.Services;
using CoursePlatform.Models;

namespace CoursePlatform.Endpoints;

public static class EnrollmentEndpoints
{
    private const string RoutePrefix = "/api/enrollment";

    public static void MapEnrollmentEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet($"{RoutePrefix}", GetAllEnrollmentsAsync);
        app.MapGet($"{RoutePrefix}/{{id}}", GetEnrollmentByIdAsync);
        app.MapGet($"{RoutePrefix}/EnrollmentStudentCourse/{{id}}", GetEnrollmentWithStudentCourseAsync);
        app.MapPut($"{RoutePrefix}/{{id}}/{{grade}}", UpdateEnrollmentGradeAsync);
        app.MapPost($"{RoutePrefix}", CreateEnrollmentAsync);
        app.MapDelete($"{RoutePrefix}/{{id}}", DeleteEnrollmentAsync);
    }

    private static async Task<IResult> GetAllEnrollmentsAsync(IEnrollmentService service)
    {
        var enrollments = await service.GetAllAsync();
        return Results.Ok(enrollments);
    }

    private static async Task<IResult> GetEnrollmentByIdAsync(IEnrollmentService service, string id)
    {
        var enrollment = await service.GetByIdAsync(id);
        return enrollment is not null ? Results.Ok(enrollment) : Results.NotFound();
    }

    private static async Task<IResult> GetEnrollmentWithStudentCourseAsync(IEnrollmentService service, string id)
    {
        var enrollment = await service.GetWithStudentCourseAsync(id);
        return enrollment is not null ? Results.Ok(enrollment) : Results.NotFound();
    }

    private static async Task<IResult> UpdateEnrollmentGradeAsync(IEnrollmentService service, string id, int grade)
    {
        await service.UpdateGradeAsync(id, grade);
        return Results.NoContent();
    }

    private static async Task<IResult> CreateEnrollmentAsync(IEnrollmentService service, EnrollmentRequestDTO dto)
    {
        var created = await service.CreateAsync(dto);
        return Results.Created($"{RoutePrefix}/{created.Id}", created);
    }

    private static async Task<IResult> DeleteEnrollmentAsync(IEnrollmentService service, string id)
    {
        await service.DeleteAsync(id);
        return Results.NoContent();
    }
}

