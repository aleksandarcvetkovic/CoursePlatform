using CoursePlatform.Application.Features.Students.Commands.CreateStudent;
using CoursePlatform.Application.Features.Students.Commands.DeleteStudent;
using CoursePlatform.Application.Features.Students.Commands.UpdateStudent;
using CoursePlatform.Application.Features.Students.Queries.GetAllStudents;
using CoursePlatform.Application.Features.Students.Queries.GetStudentById;
using CoursePlatform.Application.Features.Students.Queries.GetStudentWithEnrollments;
using CoursePlatform.Application.DTOs;
using MediatR;
using CoursePlatform.Api.Presentation.Endpoints.Internal;

namespace CoursePlatform.Api.Presentation.Endpoints;

public class StudentEndpoints : IEndpoint
{
    public static string BaseRoute => "/api/student";

    public static void DefineEndpoints(IEndpointRouteBuilder app)
    {
        app.MapGet($"{BaseRoute}", GetAllStudentsAsync)
            .DefineDefaultResponseCodes()
            .Produces<List<StudentResponseDTO>>();

        app.MapGet($"{BaseRoute}/{{id}}", GetStudentByIdAsync)
            .DefineDefaultResponseCodes()
            .Produces(StatusCodes.Status404NotFound)
            .Produces<StudentResponseDTO>();

        app.MapGet($"{BaseRoute}/StudentWithEnrollments/{{id}}", GetStudentWithEnrollmentsAsync)
            .DefineDefaultResponseCodes()
            .Produces(StatusCodes.Status404NotFound)
            .Produces<StudentWithEnrollmentsDTO>();

        app.MapPut($"{BaseRoute}/{{id}}", UpdateStudentAsync)
            .DefineDefaultResponseCodes()
            .Produces<StudentResponseDTO>()
            .Produces(StatusCodes.Status404NotFound);

        app.MapPost($"{BaseRoute}", CreateStudentAsync)
            .DefineDefaultResponseCodes()
            .Produces<StudentResponseDTO>(StatusCodes.Status201Created);

        app.MapDelete($"{BaseRoute}/{{id}}", DeleteStudentAsync)
            .DefineDefaultResponseCodes()
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound);
    }

    [EndpointSummary("Get all students")]
    [EndpointDescription("Retrieves a list of all students in the system.")]
    private static async Task<IResult> GetAllStudentsAsync(IMediator mediator)
    {
        var students = await mediator.Send(new GetAllStudentsQuery());
        return Results.Ok(students);
    }

    [EndpointSummary("Get student by ID")]
    [EndpointDescription("Retrieves a student by their unique identifier.")]
    private static async Task<IResult> GetStudentByIdAsync(IMediator mediator, string id)
    {
        try
        {
            var student = await mediator.Send(new GetStudentByIdQuery(id));
            return Results.Ok(student);
        }
        catch (Exception)
        {
            return Results.NotFound($"Student with ID {id} not found");
        }
    }

    [EndpointSummary("Get student with enrollments")]
    [EndpointDescription("Retrieves a student along with their enrollment information by student ID.")]
    private static async Task<IResult> GetStudentWithEnrollmentsAsync(IMediator mediator, string id)
    {
        try
        {
            var student = await mediator.Send(new GetStudentWithEnrollmentsQuery(id));
            return Results.Ok(student);
        }
        catch (Exception)
        {
            return Results.NotFound($"Student with ID {id} not found");
        }
    }

    [EndpointSummary("Create a new student")]
    [EndpointDescription("Creates a new student with the provided information.")]
    private static async Task<IResult> CreateStudentAsync(IMediator mediator, StudentRequestDTO studentRequest)
    {
        var student = await mediator.Send(new CreateStudentCommand(studentRequest));
        return Results.Created($"{BaseRoute}/{student.Id}", student);
    }

    [EndpointSummary("Update an existing student")]
    [EndpointDescription("Updates an existing student's information using their ID.")]
    private static async Task<IResult> UpdateStudentAsync(IMediator mediator, string id, StudentRequestDTO studentRequest)
    {
        var student = await mediator.Send(new UpdateStudentCommand(id, studentRequest));
        return Results.Ok(student);
    }

    [EndpointSummary("Delete a student")]
    [EndpointDescription("Deletes an existing student by their ID.")]
    private static async Task<IResult> DeleteStudentAsync(IMediator mediator, string id)
    {
        await mediator.Send(new DeleteStudentCommand(id));
        return Results.NoContent();
    }
}
