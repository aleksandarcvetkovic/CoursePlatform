using CoursePlatform.Application.Features.Enrollments.Commands;
using CoursePlatform.Application.Features.Enrollments.Commands.CreateEnrollment;
using CoursePlatform.Application.Features.Enrollments.Commands.UpdateEnrollmentGrade;
using CoursePlatform.Application.Features.Enrollments.Queries.GetAllEnrollments;
using CoursePlatform.Application.DTOs;
using MediatR;

namespace CoursePlatform.Api.Presentation.Endpoints;

public static class EnrollmentEndpoints
{
    private const string RoutePrefix = "/api/enrollment";

    public static void MapEnrollmentEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet($"{RoutePrefix}", GetAllEnrollmentsAsync);
        app.MapPost($"{RoutePrefix}", CreateEnrollmentAsync);
        app.MapPut($"{RoutePrefix}/{{id}}/grade", UpdateEnrollmentGradeAsync);
        app.MapDelete($"{RoutePrefix}/{{id}}", DeleteEnrollmentAsync);
    }

    private static async Task<IResult> GetAllEnrollmentsAsync(IMediator mediator)
    {
        var enrollments = await mediator.Send(new GetAllEnrollmentsQuery());
        return Results.Ok(enrollments);
    }    private static async Task<IResult> CreateEnrollmentAsync(IMediator mediator, EnrollmentRequestDTO enrollmentRequest)
    {
        var enrollment = await mediator.Send(new CreateEnrollmentCommand(enrollmentRequest));
        return Results.Created($"{RoutePrefix}/{enrollment.Id}", enrollment);
    }

    private static async Task<IResult> UpdateEnrollmentGradeAsync(IMediator mediator, string id, EnrollmentGradeRequestDTO gradeRequest)
    {
        var enrollment = await mediator.Send(new UpdateEnrollmentGradeCommand(id, gradeRequest));
        return Results.Ok(enrollment);
    }    private static async Task<IResult> DeleteEnrollmentAsync(IMediator mediator, string id)
    {
        await mediator.Send(new DeleteEnrollmentCommand(id));
        return Results.NoContent();
    }
}
