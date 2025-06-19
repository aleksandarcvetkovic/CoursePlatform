using CoursePlatform.Services;
using CoursePlatform.Models;

namespace CoursePlatform.Endpoints
{
    public static class EnrollmentEndpoints
    {
        public static void MapEnrollmentEndpoints(this IEndpointRouteBuilder app, string routePrefix)
        {
            app.MapGet($"{routePrefix}", async (IEnrollmentService service) =>
            {
                var enrollments = await service.GetAllAsync();
                return Results.Ok(enrollments);
            });

            app.MapGet($"{routePrefix}/{{id}}", async (IEnrollmentService service, string id) =>
            {
                var enrollment = await service.GetByIdAsync(id);
                return enrollment is not null ? Results.Ok(enrollment) : Results.NotFound();
            });

            app.MapGet($"{routePrefix}/EnrollmentStudentCourse/{{id}}", async (IEnrollmentService service, string id) =>
            {
                var enrollment = await service.GetWithStudentCourseAsync(id);
                return enrollment is not null ? Results.Ok(enrollment) : Results.NotFound();
            });

            app.MapPut($"{routePrefix}/{{id}}/{{grade}}", async (IEnrollmentService service, string id, int grade) =>
            {
                await service.UpdateGradeAsync(id, grade);
                return Results.NoContent();
            });

            app.MapPost($"{routePrefix}", async (IEnrollmentService service, EnrollmentRequestDTO dto) =>
            {
                var created = await service.CreateAsync(dto);
                return Results.Created($"{routePrefix}/{created.Id}", created);
            });

            app.MapDelete($"{routePrefix}/{{id}}", async (IEnrollmentService service, string id) =>
            {
                await service.DeleteAsync(id);
                return Results.NoContent();
            });
        }
    }
}
