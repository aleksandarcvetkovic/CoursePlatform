using CoursePlatform.Services;
using CoursePlatform.Models;

namespace CoursePlatform.Endpoints;

public static class StudentEndpoints
{
    private const string RoutePrefix = "/api/student";

    public static void MapStudentEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet($"{RoutePrefix}", GetAllStudentsAsync);
        app.MapGet($"{RoutePrefix}/{{id}}", GetStudentByIdAsync);
        app.MapGet($"{RoutePrefix}/StudentWithEnrollments/{{id}}", GetStudentWithEnrollmentsAsync);
        app.MapPut($"{RoutePrefix}/{{id}}", UpdateStudentAsync);
        app.MapPost($"{RoutePrefix}", CreateStudentAsync);
        app.MapDelete($"{RoutePrefix}/{{id}}", DeleteStudentAsync);
    }

    private static async Task<IResult> GetAllStudentsAsync(IStudentService service)
    {
        var studentsDTO = await service.GetAllStudentsAsync();
        return Results.Ok(studentsDTO);
    }

    private static async Task<IResult> GetStudentByIdAsync(IStudentService service, string id)
    {
        var student = await service.GetStudentByIdAsync(id);
        return student is not null ? Results.Ok(student) : Results.NotFound();
    }

    private static async Task<IResult> GetStudentWithEnrollmentsAsync(IStudentService service, string id)
    {
        var student = await service.GetStudentWithEnrollmentsAsync(id);
        return student is not null ? Results.Ok(student) : Results.NotFound();
    }

    private static async Task<IResult> UpdateStudentAsync(IStudentService service, string id, StudentRequestDTO dto)
    {
        await service.UpdateStudentAsync(id, dto);
        return Results.NoContent();
    }

    private static async Task<IResult> CreateStudentAsync(IStudentService service, StudentRequestDTO dto)
    {
        var created = await service.CreateStudentAsync(dto);
        return Results.Created($"{RoutePrefix}/{created.Id}", created);
    }

    private static async Task<IResult> DeleteStudentAsync(IStudentService service, string id)
    {
        await service.DeleteStudentAsync(id);
        return Results.NoContent();
    }
}

