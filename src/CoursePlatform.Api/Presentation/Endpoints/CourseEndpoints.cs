using CoursePlatform.Application.Features.Courses.Commands;
using CoursePlatform.Application.Features.Courses.Queries;
using CoursePlatform.Application.DTOs;
using MediatR;
using CoursePlatform.Api.Presentation.Endpoints.Internal;

namespace CoursePlatform.Api.Presentation.Endpoints;

public class CourseEndpoints : IEndpoint
{
    
    public static string BaseRoute => "/api/course";


    public static void DefineEndpoints(IEndpointRouteBuilder app)
    {
        app.MapGet($"{BaseRoute}", GetAllCoursesAsync)
            .DefineDefaultResponseCodes()
            .Produces<List<CourseResponseDTO>>();


        app.MapGet($"{BaseRoute}/{{id}}", GetCourseByIdAsync)
            .DefineDefaultResponseCodes()
            .Produces(StatusCodes.Status404NotFound)
            .Produces<CourseResponseDTO>();

        app.MapGet($"{BaseRoute}/CourseWithInstructor/{{id}}", GetCourseWithInstructorAsync)
            .DefineDefaultResponseCodes()
            .Produces(StatusCodes.Status404NotFound)
            .Produces<CourseWithInstructorDTO>();

        app.MapPut($"{BaseRoute}/{{id}}", UpdateCourseAsync)
            .DefineDefaultResponseCodes()
            .Produces<CourseResponseDTO>()
            .Produces(StatusCodes.Status404NotFound);


        app.MapPost($"{BaseRoute}", CreateCourseAsync)
            .DefineDefaultResponseCodes()
            .Produces<CourseResponseDTO>(StatusCodes.Status201Created);

        app.MapDelete($"{BaseRoute}/{{id}}", DeleteCourseAsync)
            .DefineDefaultResponseCodes()
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound);
    }


    [EndpointDescription("Retrieves a list of all courses.")]
    [EndpointSummary("Get all courses")]
    private static async Task<IResult> GetAllCoursesAsync(IMediator mediator)
    {
        var courses = await mediator.Send(new GetAllCoursesQuery());
        return Results.Ok(courses);
    }

    [EndpointSummary("Get course by ID")]
    [EndpointDescription("Retrieves a course by its ID.")]
    private static async Task<IResult> GetCourseByIdAsync(IMediator mediator, string id)
    {
        var course = await mediator.Send(new GetCourseByIdQuery(id));
        return course is not null ? Results.Ok(course) : Results.NotFound();
    }

    [EndpointSummary("Get course with instructor by ID")]
    [EndpointDescription("Retrieves a course with its instructor details by its ID.")]
    private static async Task<IResult> GetCourseWithInstructorAsync(IMediator mediator, string id)
    {
        var course = await mediator.Send(new GetCourseWithInstructorQuery(id));
        return course is not null ? Results.Ok(course) : Results.NotFound();
    }

    [EndpointSummary("Create a new course")]
    [EndpointDescription("Creates a new course with the provided details.")]
    private static async Task<IResult> CreateCourseAsync(IMediator mediator, CourseRequestDTO courseRequest)
    {
        var course = await mediator.Send(new CreateCourseCommand(courseRequest));
        return Results.Created($"{BaseRoute}/{course.Id}", course);
    }

    [EndpointSummary("Update an existing course")]
    [EndpointDescription("Updates an existing course with the provided details using its ID.")]
    private static async Task<IResult> UpdateCourseAsync(IMediator mediator, string id, CourseRequestDTO courseRequest)
    {
        var course = await mediator.Send(new UpdateCourseCommand(id, courseRequest));
        return Results.Ok(course);
    }

    [EndpointSummary("Delete a course")]
    [EndpointDescription("Deletes an existing course by its ID.")]
    private static async Task<IResult> DeleteCourseAsync(IMediator mediator, string id)
    {
        await mediator.Send(new DeleteCourseCommand(id));
        return Results.NoContent();
    }
}
