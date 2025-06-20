using Xunit;
using Microsoft.EntityFrameworkCore;
using CoursePlatform.Models;
using CoursePlatform.Repositories;
using CoursePlatform.Services;
using System.Threading.Tasks;

namespace CoursePlatform.Tests.Services;

public class InstructorServiceTests
{
    private CoursePlatformContext GetDbContext(string dbName)
    {
        var options = new DbContextOptionsBuilder<CoursePlatformContext>()
            .UseInMemoryDatabase(databaseName: dbName)
            .Options;
        return new CoursePlatformContext(options);
    }

    [Fact]
    public async Task CreateInstructorAsync_CreatesInstructor()
    {
        var db = GetDbContext("CreateInstructorAsync_CreatesInstructor");
        var repo = new InstructorRepository(db);
        var service = new InstructorService(repo);

        var instructorDto = new InstructorRequestDTO { Name = "Miloš Petrović", Email = "milos.petrovic@example.com" };
        var created = await service.CreateInstructorAsync(instructorDto);

        Assert.NotNull(created.Id);
        Assert.Equal("Miloš Petrović", created.Name);
        Assert.Equal("milos.petrovic@example.com", created.Email);
    }

    [Fact]
    public async Task GetInstructorByIdAsync_ReturnsInstructor()
    {
        var db = GetDbContext("GetInstructorByIdAsync_ReturnsInstructor");
        var repo = new InstructorRepository(db);
        var service = new InstructorService(repo);

        var instructorDto = new InstructorRequestDTO { Name = "Miloš Petrović", Email = "milos.petrovic@example.com" };
        var created = await service.CreateInstructorAsync(instructorDto);

        var fetched = await service.GetInstructorByIdAsync(created.Id);

        Assert.Equal("Miloš Petrović", fetched.Name);
        Assert.Equal("milos.petrovic@example.com", fetched.Email);
    }

    [Fact]
    public async Task UpdateInstructorAsync_UpdatesInstructor()
    {
        var db = GetDbContext("UpdateInstructorAsync_UpdatesInstructor");
        var repo = new InstructorRepository(db);
        var service = new InstructorService(repo);

        var instructorDto = new InstructorRequestDTO { Name = "Miloš Petrović", Email = "milos.petrovic@example.com" };
        var created = await service.CreateInstructorAsync(instructorDto);

        var updateDto = new InstructorRequestDTO { Name = "Jovan Petrović", Email = "jovan.petrovic@example.com" };
        await service.UpdateInstructorAsync(created.Id, updateDto);

        var updated = await service.GetInstructorByIdAsync(created.Id);
        Assert.Equal("Jovan Petrović", updated.Name);
        Assert.Equal("jovan.petrovic@example.com", updated.Email);
    }

    [Fact]
    public async Task DeleteInstructorAsync_DeletesInstructor()
    {
        var db = GetDbContext("DeleteInstructorAsync_DeletesInstructor");
        var repo = new InstructorRepository(db);
        var service = new InstructorService(repo);

        var instructorDto = new InstructorRequestDTO { Name = "Miloš Petrović", Email = "milos.petrovic@example.com" };
        var created = await service.CreateInstructorAsync(instructorDto);

        await service.DeleteInstructorAsync(created.Id);

        await Assert.ThrowsAsync<NotFoundException>(() => service.GetInstructorByIdAsync(created.Id));
    }
}
    