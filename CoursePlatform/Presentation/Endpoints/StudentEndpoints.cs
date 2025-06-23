using CoursePlatform.Application.Features.Students.Commands.CreateStudent;
using CoursePlatform.Application.Features.Students.Commands.DeleteStudent;
using CoursePlatform.Application.Features.Students.Commands.UpdateStudent;
using CoursePlatform.Application.Features.Students.Queries.GetAllStudents;
using CoursePlatform.Application.Features.Students.Queries.GetStudentById;
using CoursePlatform.Application.Features.Students.Queries.GetStudentWithEnrollments;
using CoursePlatform.Application.DTOs;
using MediatR;

namespace CoursePlatform.Presentation.Endpoints;

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

    private static async Task<IResult> GetAllStudentsAsync(IMediator mediator)
    {
        var students = await mediator.Send(new GetAllStudentsQuery());
        return Results.Ok(students);
    }

    private static async Task<IResult> GetStudentByIdAsync(IMediator mediator, string id)
    {
        var student = await mediator.Send(new GetStudentByIdQuery(id));
        return student is not null ? Results.Ok(student) : Results.NotFound();
    }

    private static async Task<IResult> GetStudentWithEnrollmentsAsync(IMediator mediator, string id)
    {
        var student = await mediator.Send(new GetStudentWithEnrollmentsQuery(id));
        return student is not null ? Results.Ok(student) : Results.NotFound();
    }

    private static async Task<IResult> CreateStudentAsync(IMediator mediator, StudentRequestDTO studentRequest)
    {
        var student = await mediator.Send(new CreateStudentCommand(studentRequest));
        return Results.CreatedAtRoute("GetStudentById", new { id = student.Id }, student);
    }

    private static async Task<IResult> UpdateStudentAsync(IMediator mediator, string id, StudentRequestDTO studentRequest)
    {
        var student = await mediator.Send(new UpdateStudentCommand(id, studentRequest));
        return Results.Ok(student);
    }

    private static async Task<IResult> DeleteStudentAsync(IMediator mediator, string id)
    {
        await mediator.Send(new DeleteStudentCommand(id));
        return Results.NoContent();
    }
}
