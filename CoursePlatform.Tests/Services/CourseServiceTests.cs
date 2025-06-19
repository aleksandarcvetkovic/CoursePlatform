using Xunit;
using Microsoft.EntityFrameworkCore;
using CoursePlatform.Models;
using CoursePlatform.Repositories;
using CoursePlatform.Services;
using System.Threading.Tasks;

namespace CoursePlatform.Tests.Services;

public class CourseServiceTests
{
    private CoursePlatformDbContext GetDbContext(string dbName)
    {
        var options = new DbContextOptionsBuilder<CoursePlatformDbContext>()
            .UseInMemoryDatabase(databaseName: dbName)
            .Options;
        return new CoursePlatformDbContext(options);
    }

    [Fact]
    public async Task CourseService_CRUD_Works()
    {
        var db = GetDbContext("CourseService_CRUD");
        var instructorRepo = new InstructorRepository(db);
        var courseRepo = new CourseRepository(db);
        var instructorService = new InstructorService(instructorRepo);
        var courseService = new CourseService(courseRepo, instructorRepo);

        // Create instructor first
        var instructorDto = new InstructorRequestDTO { FirstName = "Vesna", LastName = "Stojanović", Email = "vesna.stojanovic@example.com" };
        var instructor = await instructorService.CreateInstructorAsync(instructorDto);

        // Create course
        var courseDto = new CourseRequestDTO { Name = "Matematika", Description = "Algebra", InstructorId = instructor.Id };
        var created = await courseService.CreateCourseAsync(courseDto);
        Assert.NotNull(created.Id);

        // Read
        var fetched = await courseService.GetCourseAsync(created.Id);
        Assert.Equal("Matematika", fetched.Name);

        // Update
        var updateDto = new CourseRequestDTO { Name = "Fizika", Description = "Mehanika", InstructorId = instructor.Id };
        await courseService.UpdateCourseAsync(created.Id, updateDto);
        var updated = await courseService.GetCourseAsync(created.Id);
        Assert.Equal("Fizika", updated.Name);

        // Delete
        await courseService.DeleteCourseAsync(created.Id);
        await Assert.ThrowsAsync<NotFoundException>(() => courseService.GetCourseAsync(created.Id));
    }
}
