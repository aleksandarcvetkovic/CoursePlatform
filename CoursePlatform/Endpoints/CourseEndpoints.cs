using CoursePlatform.Services;
using CoursePlatform.Models;

namespace CoursePlatform.Endpoints;

public static class CourseEndpoints
{
    private const string RoutePrefix = "/api/course";

    public static void MapCourseEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet($"{RoutePrefix}", GetAllCoursesAsync);
        app.MapGet($"{RoutePrefix}/{{id}}", GetCourseByIdAsync);
        app.MapGet($"{RoutePrefix}/withInstructor/{{id}}", GetCourseWithInstructorAsync);
        app.MapPost($"{RoutePrefix}", CreateCourseAsync);
        app.MapPut($"{RoutePrefix}/{{id}}", UpdateCourseAsync);
        app.MapDelete($"{RoutePrefix}/{{id}}", DeleteCourseAsync);
    }

    private static async Task<IResult> GetAllCoursesAsync(ICourseService service)
    {
        var coursesDTOs = await service.GetAllCoursesAsync();
        return Results.Ok(coursesDTOs);
    }

    private static async Task<IResult> GetCourseByIdAsync(ICourseService service, string id)
    {
        var courseDTO = await service.GetCourseAsync(id);
        return courseDTO is not null ? Results.Ok(courseDTO) : Results.NotFound();
    }

    private static async Task<IResult> GetCourseWithInstructorAsync(ICourseService service, string id)
    {
        var course = await service.GetCourseWithInstructorAsync(id);
        return course is not null ? Results.Ok(course) : Results.NotFound();
    }

    private static async Task<IResult> CreateCourseAsync(ICourseService service, CourseRequestDTO dto)
    {
        var createdCourse = await service.CreateCourseAsync(dto);
        return Results.Created($"{RoutePrefix}/{createdCourse.Id}", createdCourse);
    }

    private static async Task<IResult> UpdateCourseAsync(ICourseService service, string id, CourseRequestDTO dto)
    {
        await service.UpdateCourseAsync(id, dto);
        return Results.NoContent();
    }

    private static async Task<IResult> DeleteCourseAsync(ICourseService service, string id)
    {
        await service.DeleteCourseAsync(id);
        return Results.NoContent();
    }
}

