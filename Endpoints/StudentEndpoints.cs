using CoursePlatform.Services;
using CoursePlatform.Models;

namespace CoursePlatform.Endpoints
{
    public static class StudentEndpoints
    {
        public static void MapStudentEndpoints(this IEndpointRouteBuilder app, string routePrefix)
        {
            app.MapGet($"{routePrefix}", async (IStudentService service) =>
            {
                var studentsDTO = await service.GetAllStudentsAsync();
                return Results.Ok(studentsDTO);
            });

            app.MapGet($"{routePrefix}/{{id}}", async (IStudentService service, string id) =>
            {
                var student = await service.GetStudentByIdAsync(id);
                return student is not null ? Results.Ok(student) : Results.NotFound();
            });

            app.MapGet($"{routePrefix}/StudentWithEnrollments/{{id}}", async (IStudentService service, string id) =>
            {
                var student = await service.GetStudentWithEnrollmentsAsync(id);
                return student is not null ? Results.Ok(student) : Results.NotFound();
            });

            app.MapPut($"{routePrefix}/{{id}}", async (IStudentService service, string id, StudentRequestDTO dto) =>
            {
                await service.UpdateStudentAsync(id, dto);
                return Results.NoContent();
            });

            app.MapPost($"{routePrefix}", async (IStudentService service, StudentRequestDTO dto) =>
            {
                var created = await service.CreateStudentAsync(dto);
                return Results.Created($"{routePrefix}/{created.Id}", created);
            });

            app.MapDelete($"{routePrefix}/{{id}}", async (IStudentService service, string id) =>
            {
                await service.DeleteStudentAsync(id);
                return Results.NoContent();
            });
        }
    }
}
