using Xunit;
using Microsoft.EntityFrameworkCore;
using CoursePlatform.Models;
using CoursePlatform.Repositories;
using CoursePlatform.Services;
using System.Threading.Tasks;

namespace CoursePlatform.Tests.Services;

public class StudentServiceTests
{

    
    private CoursePlatformContext GetDbContext(string dbName)
    {
        var options = new DbContextOptionsBuilder<CoursePlatformContext>()
            .UseInMemoryDatabase(databaseName: dbName)
            .Options;
        return new CoursePlatformContext(options);
    }

    [Fact]
    public async Task CreateStudentAsync_CreatesStudent()
    {
        // Arrange
        var db = GetDbContext("CreateStudent");
        var repo = new StudentRepository(db);
        var service = new StudentService(repo);
        var studentDto = new StudentRequestDTO { Name = "Nikola Jovanović", Email = "nikola.jovanovic@example.com" };

        // Act
        var created = await service.CreateStudentAsync(studentDto);

        // Assert
        Assert.NotNull(created.Id);
        Assert.Equal("Nikola Jovanović", created.Name);
    }

    [Fact]
    public async Task GetStudentByIdAsync_ReturnsStudent()
    {
        // Arrange
        var db = GetDbContext("GetStudent");
        var repo = new StudentRepository(db);
        var service = new StudentService(repo);
        var studentDto = new StudentRequestDTO { Name = "Nikola Jovanović", Email = "nikola.jovanovic@example.com" };
        var created = await service.CreateStudentAsync(studentDto);

        // Act
        var fetched = await service.GetStudentByIdAsync(created.Id);

        // Assert
        Assert.Equal("Nikola Jovanović", fetched.Name);
    }

    [Fact]
    public async Task UpdateStudentAsync_UpdatesStudent()
    {
        // Arrange
        var db = GetDbContext("UpdateStudent");
        var repo = new StudentRepository(db);
        var service = new StudentService(repo);
        var studentDto = new StudentRequestDTO { Name = "Nikola Jovanović", Email = "nikola.jovanovic@example.com" };
        var created = await service.CreateStudentAsync(studentDto);
        var updateDto = new StudentRequestDTO { Name = "Ana Jovanović", Email = "ana.jovanovic@example.com" };

        // Act
        await service.UpdateStudentAsync(created.Id, updateDto);
        var updated = await service.GetStudentByIdAsync(created.Id);

        // Assert
        Assert.Equal("Ana Jovanović", updated.Name);
        Assert.Equal("ana.jovanovic@example.com", updated.Email);
    }

    [Fact]
    public async Task DeleteStudentAsync_DeletesStudent()
    {
        // Arrange
        var db = GetDbContext("DeleteStudent");
        var repo = new StudentRepository(db);
        var service = new StudentService(repo);
        var studentDto = new StudentRequestDTO { Name = "Nikola Jovanović", Email = "nikola.jovanovic@example.com" };
        var created = await service.CreateStudentAsync(studentDto);

        // Act
        await service.DeleteStudentAsync(created.Id);

        // Assert
        await Assert.ThrowsAsync<NotFoundException>(() => service.GetStudentByIdAsync(created.Id));
    }
}
