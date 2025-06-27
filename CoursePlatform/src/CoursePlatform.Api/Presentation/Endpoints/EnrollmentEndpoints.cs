using CoursePlatform.Application.Features.Enrollments.Commands;
using CoursePlatform.Application.Features.Enrollments.Commands.CreateEnrollment;
using CoursePlatform.Application.Features.Enrollments.Commands.UpdateEnrollmentGrade;
using CoursePlatform.Application.Features.Enrollments.Queries.GetAllEnrollments;
using CoursePlatform.Application.DTOs;
using MediatR;
using CoursePlatform.Api.Presentation.Endpoints.Internal;

namespace CoursePlatform.Api.Presentation.Endpoints;
public class EnrollmentEndpoints : IEndpoint
{
    public static string BaseRoute =>"/api/enrollment";


    public static void DefineEndpoints(IEndpointRouteBuilder app)
    {
        app.MapGet($"{BaseRoute}", GetAllEnrollmentsAsync)
            .DefineDefaultResponseCodes()
            .Produces<List<EnrollmentResponseDTO>>();

        app.MapPost($"{BaseRoute}", CreateEnrollmentAsync)
            .DefineDefaultResponseCodes()
            .Produces<EnrollmentResponseDTO>(StatusCodes.Status201Created);

        app.MapPut($"{BaseRoute}/{{id}}/grade", UpdateEnrollmentGradeAsync)
            .DefineDefaultResponseCodes()
            .Produces<EnrollmentResponseDTO>()
            .Produces(StatusCodes.Status404NotFound);

        app.MapDelete($"{BaseRoute}/{{id}}", DeleteEnrollmentAsync)
            .DefineDefaultResponseCodes()
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound);
    }

    [EndpointSummary("Get all enrollments")]
    [EndpointDescription("Retrieves a list of all student enrollments in the system.")]
    private static async Task<IResult> GetAllEnrollmentsAsync(IMediator mediator)
    {
        var enrollments = await mediator.Send(new GetAllEnrollmentsQuery());
        return Results.Ok(enrollments);
    }

    [EndpointSummary("Create a new enrollment")]
    [EndpointDescription("Creates a new enrollment for a student in a course.")]
    private static async Task<IResult> CreateEnrollmentAsync(IMediator mediator, EnrollmentRequestDTO enrollmentRequest)
    {
        var enrollment = await mediator.Send(new CreateEnrollmentCommand(enrollmentRequest));
        return Results.Created($"{BaseRoute}/{enrollment.Id}", enrollment);
    }

    [EndpointSummary("Update enrollment grade")]
    [EndpointDescription("Updates the grade for an existing enrollment by enrollment ID.")]
    private static async Task<IResult> UpdateEnrollmentGradeAsync(IMediator mediator, string id, EnrollmentGradeRequestDTO gradeRequest)
    {
        var enrollment = await mediator.Send(new UpdateEnrollmentGradeCommand(id, gradeRequest));
        return Results.Ok(enrollment);
    }

    [EndpointSummary("Delete an enrollment")]
    [EndpointDescription("Deletes an existing enrollment by its ID.")]
    private static async Task<IResult> DeleteEnrollmentAsync(IMediator mediator, string id)
    {
        await mediator.Send(new DeleteEnrollmentCommand(id));
        return Results.NoContent();
    }
}
