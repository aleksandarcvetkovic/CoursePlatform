using Xunit;
using Microsoft.EntityFrameworkCore;
using CoursePlatform.Models;
using CoursePlatform.Repositories;
using CoursePlatform.Services;
using System.Threading.Tasks;

namespace CoursePlatform.Tests.Services;

public class InstructorServiceTests
{
    private CoursePlatformDbContext GetDbContext(string dbName)
    {
        var options = new DbContextOptionsBuilder<CoursePlatformDbContext>()
            .UseInMemoryDatabase(databaseName: dbName)
            .Options;
        return new CoursePlatformDbContext(options);
    }

    [Fact]
    public async Task CreateInstructorAsync_CreatesInstructor()
    {
        var db = GetDbContext("CreateInstructorAsync_CreatesInstructor");
        var repo = new InstructorRepository(db);
        var service = new InstructorService(repo);

        var instructorDto = new InstructorRequestDTO { FirstName = "Miloš", LastName = "Petrović", Email = "milos.petrovic@example.com" };
        var created = await service.CreateInstructorAsync(instructorDto);

        Assert.NotNull(created.Id);
        Assert.Equal("Miloš", created.FirstName);
        Assert.Equal("Petrović", created.LastName);
        Assert.Equal("milos.petrovic@example.com", created.Email);
    }

    [Fact]
    public async Task GetInstructorByIdAsync_ReturnsInstructor()
    {
        var db = GetDbContext("GetInstructorByIdAsync_ReturnsInstructor");
        var repo = new InstructorRepository(db);
        var service = new InstructorService(repo);

        var instructorDto = new InstructorRequestDTO { FirstName = "Miloš", LastName = "Petrović", Email = "milos.petrovic@example.com" };
        var created = await service.CreateInstructorAsync(instructorDto);

        var fetched = await service.GetInstructorByIdAsync(created.Id);

        Assert.Equal("Miloš", fetched.FirstName);
        Assert.Equal("Petrović", fetched.LastName);
        Assert.Equal("milos.petrovic@example.com", fetched.Email);
    }

    [Fact]
    public async Task UpdateInstructorAsync_UpdatesInstructor()
    {
        var db = GetDbContext("UpdateInstructorAsync_UpdatesInstructor");
        var repo = new InstructorRepository(db);
        var service = new InstructorService(repo);

        var instructorDto = new InstructorRequestDTO { FirstName = "Miloš", LastName = "Petrović", Email = "milos.petrovic@example.com" };
        var created = await service.CreateInstructorAsync(instructorDto);

        var updateDto = new InstructorRequestDTO { FirstName = "Jovan", LastName = "Petrović", Email = "jovan.petrovic@example.com" };
        await service.UpdateInstructorAsync(created.Id, updateDto);

        var updated = await service.GetInstructorByIdAsync(created.Id);
        Assert.Equal("Jovan", updated.FirstName);
        Assert.Equal("Petrović", updated.LastName);
        Assert.Equal("jovan.petrovic@example.com", updated.Email);
    }

    [Fact]
    public async Task DeleteInstructorAsync_DeletesInstructor()
    {
        var db = GetDbContext("DeleteInstructorAsync_DeletesInstructor");
        var repo = new InstructorRepository(db);
        var service = new InstructorService(repo);

        var instructorDto = new InstructorRequestDTO { FirstName = "Miloš", LastName = "Petrović", Email = "milos.petrovic@example.com" };
        var created = await service.CreateInstructorAsync(instructorDto);

        await service.DeleteInstructorAsync(created.Id);

        await Assert.ThrowsAsync<NotFoundException>(() => service.GetInstructorByIdAsync(created.Id));
    }
}
