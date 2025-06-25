using CoursePlatform.Application.Features.Instructors.Commands;
using CoursePlatform.Application.Features.Instructors.Commands.CreateInstructor;
using CoursePlatform.Application.Features.Instructors.Queries;
using CoursePlatform.Application.Features.Instructors.Queries.GetAllInstructors;
using CoursePlatform.Application.Features.Instructors.Queries.GetInstructorById;
using CoursePlatform.Application.DTOs;
using MediatR;
using PaymentProcessing.Api.Endpoints.Internal;

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
