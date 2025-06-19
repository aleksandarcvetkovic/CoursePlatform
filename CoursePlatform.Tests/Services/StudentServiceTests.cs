using Xunit;
using Microsoft.EntityFrameworkCore;
using CoursePlatform.Models;
using CoursePlatform.Repositories;
using CoursePlatform.Services;
using System.Threading.Tasks;

namespace CoursePlatform.Tests.Services;

public class StudentServiceTests
{
    private CoursePlatformDbContext GetDbContext(string dbName)
    {
        var options = new DbContextOptionsBuilder<CoursePlatformDbContext>()
            .UseInMemoryDatabase(databaseName: dbName)
            .Options;
        return new CoursePlatformDbContext(options);
    }

    [Fact]
    public async Task CreateStudentAsync_CreatesStudent()
    {
        var db = GetDbContext("CreateStudent");
        var repo = new StudentRepository(db);
        var service = new StudentService(repo);

        var studentDto = new StudentRequestDTO { FirstName = "Nikola", LastName = "Jovanović", Email = "nikola.jovanovic@example.com" };
        var created = await service.CreateStudentAsync(studentDto);

        Assert.NotNull(created.Id);
        Assert.Equal("Nikola", created.FirstName);
    }

    [Fact]
    public async Task GetStudentByIdAsync_ReturnsStudent()
    {
        var db = GetDbContext("GetStudent");
        var repo = new StudentRepository(db);
        var service = new StudentService(repo);

        var studentDto = new StudentRequestDTO { FirstName = "Nikola", LastName = "Jovanović", Email = "nikola.jovanovic@example.com" };
        var created = await service.CreateStudentAsync(studentDto);

        var fetched = await service.GetStudentByIdAsync(created.Id);
        Assert.Equal("Nikola", fetched.FirstName);
    }

    [Fact]
    public async Task UpdateStudentAsync_UpdatesStudent()
    {
        var db = GetDbContext("UpdateStudent");
        var repo = new StudentRepository(db);
        var service = new StudentService(repo);

        var studentDto = new StudentRequestDTO { FirstName = "Nikola", LastName = "Jovanović", Email = "nikola.jovanovic@example.com" };
        var created = await service.CreateStudentAsync(studentDto);

        var updateDto = new StudentRequestDTO { FirstName = "Ana", LastName = "Jovanović", Email = "ana.jovanovic@example.com" };
        await service.UpdateStudentAsync(created.Id, updateDto);

        var updated = await service.GetStudentByIdAsync(created.Id);
        Assert.Equal("Ana", updated.FirstName);
        Assert.Equal("ana.jovanovic@example.com", updated.Email);
    }

    [Fact]
    public async Task DeleteStudentAsync_DeletesStudent()
    {
        var db = GetDbContext("DeleteStudent");
        var repo = new StudentRepository(db);
        var service = new StudentService(repo);

        var studentDto = new StudentRequestDTO { FirstName = "Nikola", LastName = "Jovanović", Email = "nikola.jovanovic@example.com" };
        var created = await service.CreateStudentAsync(studentDto);

        await service.DeleteStudentAsync(created.Id);

        await Assert.ThrowsAsync<NotFoundException>(() => service.GetStudentByIdAsync(created.Id));
    }
}
