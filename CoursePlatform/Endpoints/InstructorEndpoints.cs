using CoursePlatform.Services;
using CoursePlatform.Models;

namespace CoursePlatform.Endpoints;

public static class InstructorEndpoints
{
    private const string RoutePrefix = "/api/instructor";

    public static void MapInstructorEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet($"{RoutePrefix}", GetAllInstructorsAsync);
        app.MapGet($"{RoutePrefix}/{{id}}", GetInstructorByIdAsync);
        app.MapGet($"{RoutePrefix}/withCourses/{{id}}", GetInstructorWithCoursesAsync);
        app.MapPut($"{RoutePrefix}/{{id}}", UpdateInstructorAsync);
        app.MapPost($"{RoutePrefix}", CreateInstructorAsync);
        app.MapDelete($"{RoutePrefix}/{{id}}", DeleteInstructorAsync);
    }

    private static async Task<IResult> GetAllInstructorsAsync(IInstructorService service)
    {
        var instructorsDTOs = await service.GetAllInstructorsAsync();
        return Results.Ok(instructorsDTOs);
    }

    private static async Task<IResult> GetInstructorByIdAsync(IInstructorService service, string id)
    {
        var instructor = await service.GetInstructorByIdAsync(id);
        return instructor is not null ? Results.Ok(instructor) : Results.NotFound();
    }

    private static async Task<IResult> GetInstructorWithCoursesAsync(IInstructorService service, string id)
    {
        var instructor = await service.GetInstructorWithCoursesAsync(id);
        return instructor is not null ? Results.Ok(instructor) : Results.NotFound();
    }

    private static async Task<IResult> UpdateInstructorAsync(IInstructorService service, string id, InstructorRequestDTO dto)
    {
        await service.UpdateInstructorAsync(id, dto);
        return Results.NoContent();
    }

    private static async Task<IResult> CreateInstructorAsync(IInstructorService service, InstructorRequestDTO dto)
    {
        var created = await service.CreateInstructorAsync(dto);
        return Results.Created($"{RoutePrefix}/{created.Id}", created);
    }

    private static async Task<IResult> DeleteInstructorAsync(IInstructorService service, string id)
    {
        await service.DeleteInstructorAsync(id);
        return Results.NoContent();
    }
}

