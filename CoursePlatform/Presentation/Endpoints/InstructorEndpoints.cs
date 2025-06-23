using CoursePlatform.Application.Features.Instructors.Commands;
using CoursePlatform.Application.Features.Instructors.Commands.CreateInstructor;
using CoursePlatform.Application.Features.Instructors.Queries;
using CoursePlatform.Application.Features.Instructors.Queries.GetAllInstructors;
using CoursePlatform.Application.Features.Instructors.Queries.GetInstructorById;
using CoursePlatform.Application.DTOs;
using MediatR;

namespace CoursePlatform.Presentation.Endpoints;

public static class InstructorEndpoints
{
    private const string RoutePrefix = "/api/instructor";

    public static void MapInstructorEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet($"{RoutePrefix}", GetAllInstructorsAsync);
        app.MapGet($"{RoutePrefix}/{{id}}", GetInstructorByIdAsync).WithName("GetInstructorById");
        app.MapGet($"{RoutePrefix}/{{id}}/courses", GetInstructorWithCoursesAsync);
        app.MapPost($"{RoutePrefix}", CreateInstructorAsync);
        app.MapPut($"{RoutePrefix}/{{id}}", UpdateInstructorAsync);
        app.MapDelete($"{RoutePrefix}/{{id}}", DeleteInstructorAsync);
    }

    private static async Task<IResult> GetAllInstructorsAsync(IMediator mediator)
    {
        var instructors = await mediator.Send(new GetAllInstructorsQuery());
        return Results.Ok(instructors);
    }    private static async Task<IResult> GetInstructorByIdAsync(IMediator mediator, string id)
    {
        var instructor = await mediator.Send(new GetInstructorByIdQuery(id));
        return Results.Ok(instructor);
    }

    private static async Task<IResult> GetInstructorWithCoursesAsync(IMediator mediator, string id)
    {
        var instructor = await mediator.Send(new GetInstructorWithCoursesQuery(id));
        return Results.Ok(instructor);
    }

    private static async Task<IResult> CreateInstructorAsync(IMediator mediator, InstructorRequestDTO instructorRequest)
    {
        var instructor = await mediator.Send(new CreateInstructorCommand(instructorRequest));
        return Results.CreatedAtRoute("GetInstructorById", new { id = instructor.Id }, instructor);
    }

    private static async Task<IResult> UpdateInstructorAsync(IMediator mediator, string id, InstructorRequestDTO instructorRequest)
    {
        var instructor = await mediator.Send(new UpdateInstructorCommand(id, instructorRequest));
        return Results.Ok(instructor);
    }

    private static async Task<IResult> DeleteInstructorAsync(IMediator mediator, string id)
    {
        await mediator.Send(new DeleteInstructorCommand(id));
        return Results.NoContent();
    }
}
