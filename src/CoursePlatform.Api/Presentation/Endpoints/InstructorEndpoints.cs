using CoursePlatform.Application.Features.Instructors.Commands;
using CoursePlatform.Application.Features.Instructors.Commands.CreateInstructor;
using CoursePlatform.Application.Features.Instructors.Queries;
using CoursePlatform.Application.Features.Instructors.Queries.GetAllInstructors;
using CoursePlatform.Application.Features.Instructors.Queries.GetInstructorById;
using CoursePlatform.Application.DTOs;
using MediatR;
using CoursePlatform.Api.Presentation.Endpoints.Internal;

namespace CoursePlatform.Api.Presentation.Endpoints;

public class InstructorEndpoints : IEndpoint
{
    public static string BaseRoute => "/api/instructor";

    public static void DefineEndpoints(IEndpointRouteBuilder app)
    {
        app.MapGet($"{BaseRoute}", GetAllInstructorsAsync)
            .DefineDefaultResponseCodes()
            .Produces<List<InstructorResponseDTO>>();

        app.MapGet($"{BaseRoute}/{{id}}", GetInstructorByIdAsync).WithName("GetInstructorById")
            .DefineDefaultResponseCodes()
            .Produces(StatusCodes.Status404NotFound)
            .Produces<InstructorResponseDTO>();

        app.MapGet($"{BaseRoute}/{{id}}/courses", GetInstructorWithCoursesAsync)
            .DefineDefaultResponseCodes()
            .Produces(StatusCodes.Status404NotFound)
            .Produces<InstructorWithCoursesDTO>();

        app.MapPost($"{BaseRoute}", CreateInstructorAsync)
            .DefineDefaultResponseCodes()
            .Produces<InstructorResponseDTO>(StatusCodes.Status201Created);

        app.MapPut($"{BaseRoute}/{{id}}", UpdateInstructorAsync)
            .DefineDefaultResponseCodes()
            .Produces<InstructorResponseDTO>()
            .Produces(StatusCodes.Status404NotFound);

        app.MapDelete($"{BaseRoute}/{{id}}", DeleteInstructorAsync)
            .DefineDefaultResponseCodes()
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound);
    }

    [EndpointSummary("Get all instructors")]
    [EndpointDescription("Retrieves a list of all instructors in the system.")]
    private static async Task<IResult> GetAllInstructorsAsync(IMediator mediator)
    {
        var instructors = await mediator.Send(new GetAllInstructorsQuery());
        return Results.Ok(instructors);
    }

    [EndpointSummary("Get instructor by ID")]
    [EndpointDescription("Retrieves an instructor by their unique identifier.")]
    private static async Task<IResult> GetInstructorByIdAsync(IMediator mediator, string id)
    {
        var instructor = await mediator.Send(new GetInstructorByIdQuery(id));
        return Results.Ok(instructor);
    }

    [EndpointSummary("Get instructor with courses")]
    [EndpointDescription("Retrieves an instructor along with all their associated courses by instructor ID.")]
    private static async Task<IResult> GetInstructorWithCoursesAsync(IMediator mediator, string id)
    {
        var instructor = await mediator.Send(new GetInstructorWithCoursesQuery(id));
        return Results.Ok(instructor);
    }

    [EndpointSummary("Create a new instructor")]
    [EndpointDescription("Creates a new instructor with the provided information.")]
    private static async Task<IResult> CreateInstructorAsync(IMediator mediator, InstructorRequestDTO instructorRequest)
    {
        var instructor = await mediator.Send(new CreateInstructorCommand(instructorRequest));
        return Results.CreatedAtRoute("GetInstructorById", new { id = instructor.Id }, instructor);
    }

    [EndpointSummary("Update an existing instructor")]
    [EndpointDescription("Updates an existing instructor's information using their ID.")]
    private static async Task<IResult> UpdateInstructorAsync(IMediator mediator, string id, InstructorRequestDTO instructorRequest)
    {
        var instructor = await mediator.Send(new UpdateInstructorCommand(id, instructorRequest));
        return Results.Ok(instructor);
    }

    [EndpointSummary("Delete an instructor")]
    [EndpointDescription("Deletes an existing instructor by their ID.")]
    private static async Task<IResult> DeleteInstructorAsync(IMediator mediator, string id)
    {
        await mediator.Send(new DeleteInstructorCommand(id));
        return Results.NoContent();
    }
}
