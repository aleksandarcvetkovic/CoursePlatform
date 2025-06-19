using CoursePlatform.Services;
using CoursePlatform.Models;

namespace CoursePlatform.Endpoints
{
    public static class CourseEndpoints
    {
        public static void MapCourseEndpoints(this IEndpointRouteBuilder app, string routePrefix)
        {
            app.MapGet($"{routePrefix}", async (ICourseService service) =>
            {
                var coursesDTOs = await service.GetAllCoursesAsync();
                return Results.Ok(coursesDTOs);
            });

            app.MapGet($"{routePrefix}/{{id}}", async (ICourseService service, string id) =>
            {
                var courseDTO = await service.GetCourseAsync(id);
                return courseDTO is not null ? Results.Ok(courseDTO) : Results.NotFound();
            });

            app.MapGet($"{routePrefix}/withInstructor/{{id}}", async (ICourseService service, string id) =>
            {
                var course = await service.GetCourseWithInstructorAsync(id);
                return course is not null ? Results.Ok(course) : Results.NotFound();
            });

            app.MapPost($"{routePrefix}", async (ICourseService service, CourseRequestDTO dto) =>
            {
                var createdCourse = await service.CreateCourseAsync(dto);
                return Results.Created($"{routePrefix}/{createdCourse.Id}", createdCourse);
            });

            app.MapPut($"{routePrefix}/{{id}}", async (ICourseService service, string id, CourseRequestDTO dto) =>
            {
                await service.UpdateCourseAsync(id, dto);
                return Results.NoContent();
            });

            app.MapDelete($"{routePrefix}/{{id}}", async (ICourseService service, string id) =>
            {
                await service.DeleteCourseAsync(id);
                return Results.NoContent();
            });
        }
    }
}
