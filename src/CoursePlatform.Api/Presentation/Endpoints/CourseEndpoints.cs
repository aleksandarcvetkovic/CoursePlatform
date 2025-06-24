using CoursePlatform.Application.Features.Courses.Commands;
using CoursePlatform.Application.Features.Courses.Queries;
using CoursePlatform.Application.DTOs;
using MediatR;

namespace CoursePlatform.Api.Presentation.Endpoints;

public static class CourseEndpoints
{
    private const string RoutePrefix = "/api/course";

    public static void MapCourseEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet($"{RoutePrefix}", GetAllCoursesAsync);
        app.MapGet($"{RoutePrefix}/{{id}}", GetCourseByIdAsync);
        app.MapGet($"{RoutePrefix}/CourseWithInstructor/{{id}}", GetCourseWithInstructorAsync);
        app.MapPut($"{RoutePrefix}/{{id}}", UpdateCourseAsync);
        app.MapPost($"{RoutePrefix}", CreateCourseAsync);
        app.MapDelete($"{RoutePrefix}/{{id}}", DeleteCourseAsync);
    }

    private static async Task<IResult> GetAllCoursesAsync(IMediator mediator)
    {
        var courses = await mediator.Send(new GetAllCoursesQuery());
        return Results.Ok(courses);
    }

    private static async Task<IResult> GetCourseByIdAsync(IMediator mediator, string id)
    {
        var course = await mediator.Send(new GetCourseByIdQuery(id));
        return course is not null ? Results.Ok(course) : Results.NotFound();
    }

    private static async Task<IResult> GetCourseWithInstructorAsync(IMediator mediator, string id)
    {
        var course = await mediator.Send(new GetCourseWithInstructorQuery(id));
        return course is not null ? Results.Ok(course) : Results.NotFound();
    }

    private static async Task<IResult> CreateCourseAsync(IMediator mediator, CourseRequestDTO courseRequest)
    {
        var course = await mediator.Send(new CreateCourseCommand(courseRequest));
        return Results.CreatedAtRoute("GetCourseById", new { id = course.Id }, course);
    }

    private static async Task<IResult> UpdateCourseAsync(IMediator mediator, string id, CourseRequestDTO courseRequest)
    {
        var course = await mediator.Send(new UpdateCourseCommand(id, courseRequest));
        return Results.Ok(course);
    }

    private static async Task<IResult> DeleteCourseAsync(IMediator mediator, string id)
    {
        await mediator.Send(new DeleteCourseCommand(id));
        return Results.NoContent();
    }
}
