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

    private InstructorService GetService(string dbName)
    {
        var db = GetDbContext(dbName);
        var repo = new InstructorRepository(db);
        var service = new InstructorService(repo);
        return service;
    }

    private async Task<InstructorResponseDTO> CreateInstructorAsync(InstructorService service, string name = "Miloš Petrović", string email = "milos.petrovic@example.com")
    {
        var instructorDto = new InstructorRequestDTO { Name = name, Email = email };
        return await service.CreateInstructorAsync(instructorDto);
    }

    [Fact]
    public async Task CreateInstructorAsync_CreatesInstructor()
    {
        // Arrange
        var service = GetService("CreateInstructorAsync_CreatesInstructor");

        // Act
        var created = await CreateInstructorAsync(service);

        // Assert
        Assert.NotNull(created.Id);
        Assert.Equal("Miloš Petrović", created.Name);
        Assert.Equal("milos.petrovic@example.com", created.Email);
    }

    [Fact]
    public async Task GetInstructorByIdAsync_ReturnsInstructor()
    {
        // Arrange
        var service = GetService("GetInstructorByIdAsync_ReturnsInstructor");
        var created = await CreateInstructorAsync(service);

        // Act
        var fetched = await service.GetInstructorByIdAsync(created.Id);

        // Assert
        Assert.Equal("Miloš Petrović", fetched.Name);
        Assert.Equal("milos.petrovic@example.com", fetched.Email);
    }


    [Fact]
    public async Task UpdateInstructorAsync_UpdatesInstructor()
    {
        // Arrange
        var service = GetService("UpdateInstructorAsync_UpdatesInstructor");
        var created = await CreateInstructorAsync(service);
        var updateDto = new InstructorRequestDTO { Name = "Jovan Petrović", Email = "jovan.petrovic@example.com" };

        // Act
        await service.UpdateInstructorAsync(created.Id, updateDto);
        var updated = await service.GetInstructorByIdAsync(created.Id);

        // Assert
        Assert.Equal("Jovan Petrović", updated.Name);
        Assert.Equal("jovan.petrovic@example.com", updated.Email);
    }


    [Fact]
    public async Task DeleteInstructorAsync_DeletesInstructor()
    {
        // Arrange
        var service = GetService("DeleteInstructorAsync_DeletesInstructor");
        var created = await CreateInstructorAsync(service);

        // Act
        await service.DeleteInstructorAsync(created.Id);

        // Assert
        await Assert.ThrowsAsync<NotFoundException>(() => service.GetInstructorByIdAsync(created.Id));
    }
}
    

