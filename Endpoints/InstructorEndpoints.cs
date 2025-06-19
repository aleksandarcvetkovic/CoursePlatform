using CoursePlatform.Services;
using CoursePlatform.Models;

namespace CoursePlatform.Endpoints
{
    public static class InstructorEndpoints
    {
        public static void MapInstructorEndpoints(this IEndpointRouteBuilder app, string routePrefix)
        {
            app.MapGet($"{routePrefix}", async (IInstructorService service) =>
            {
                var instructorsDTOs = await service.GetAllInstructorsAsync();
                return Results.Ok(instructorsDTOs);
            });

            app.MapGet($"{routePrefix}/{{id}}", async (IInstructorService service, string id) =>
            {
                var instructor = await service.GetInstructorByIdAsync(id);
                return instructor is not null ? Results.Ok(instructor) : Results.NotFound();
            });

            app.MapGet($"{routePrefix}/withCourses/{{id}}", async (IInstructorService service, string id) =>
            {
                var instructor = await service.GetInstructorWithCoursesAsync(id);
                return instructor is not null ? Results.Ok(instructor) : Results.NotFound();
            });

            app.MapPut($"{routePrefix}/{{id}}", async (IInstructorService service, string id, InstructorRequestDTO dto) =>
            {
                await service.UpdateInstructorAsync(id, dto);
                return Results.NoContent();
            });

            app.MapPost($"{routePrefix}", async (IInstructorService service, InstructorRequestDTO dto) =>
            {
                var created = await service.CreateInstructorAsync(dto);
                return Results.Created($"{routePrefix}/{created.Id}", created);
            });

            app.MapDelete($"{routePrefix}/{{id}}", async (IInstructorService service, string id) =>
            {
                await service.DeleteInstructorAsync(id);
                return Results.NoContent();
            });
        }
    }
}
