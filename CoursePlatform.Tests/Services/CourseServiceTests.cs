using Xunit;
using Moq;
using CoursePlatform.Models;
using CoursePlatform.Repositories;
using CoursePlatform.Services;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace CoursePlatform.Tests.Services;

public class CourseServiceTests
{
    private readonly Mock<IInstructorRepository> _instructorRepoMock;
    private readonly Mock<ICourseRepository> _courseRepoMock;
    private readonly CourseService _courseService;

    public CourseServiceTests()
    {
        _instructorRepoMock = new Mock<IInstructorRepository>();
        _courseRepoMock = new Mock<ICourseRepository>();
        _courseService = new CourseService(_courseRepoMock.Object, _instructorRepoMock.Object);
    }

    private CourseRequestDTO GetTestCourseDto(string instructorId) =>
        new CourseRequestDTO { Title = "Matematika", Description = "Algebra", InstructorId = instructorId };

    [Fact]
    public async Task CreateCourseAsync_CreatesCourse()
    {
        // Arrange
        var instructor = new Instructor { Id = "1", Name = "Vesna Stojanović", Email = "vesna.stojanovic@example.com" };
        var course = new Course { Id = "10", Title = "Matematika", Description = "Algebra", InstructorId = "1" };

        _instructorRepoMock.Reset();
        _courseRepoMock.Reset();

        _instructorRepoMock.Setup(r => r.GetByIdAsync("1")).ReturnsAsync(instructor);
        _courseRepoMock.Setup(r => r.AddAsync(It.IsAny<Course>())).Returns(Task.CompletedTask)
            .Callback<Course>(c => c.Id = "10"); // Simulate DB-generated Id

        var courseDto = GetTestCourseDto("1");

        // Act
        var created = await _courseService.CreateCourseAsync(courseDto);

        // Assert
        Assert.Equal("10", created.Id);
        _instructorRepoMock.Verify(r => r.GetByIdAsync("1"), Times.Once);
        _courseRepoMock.Verify(r => r.AddAsync(It.IsAny<Course>()), Times.Once);
    }

    [Fact]
    public async Task GetCourseAsync_ReturnsCourse()
    {
        // Arrange
        var course = new Course { Id = "10", Title = "Matematika", Description = "Algebra", InstructorId = "1" };

        _courseRepoMock.Reset();
        _courseRepoMock.Setup(r => r.GetByIdAsync("10")).ReturnsAsync(course);

        // Act
        var fetched = await _courseService.GetCourseAsync("10");

        // Assert
        Assert.Equal("Matematika", fetched.Title);
        Assert.Equal("Algebra", fetched.Description);
        _courseRepoMock.Verify(r => r.GetByIdAsync("10"), Times.Once);
    }

    [Fact]
    public async Task UpdateCourseAsync_UpdatesCourse()
    {
        // Arrange
        var instructor = new Instructor { Id = "1", Name = "Vesna Stojanović", Email = "vesna.stojanovic@example.com" };
        var course = new Course { Id = "10", Title = "Matematika", Description = "Algebra", InstructorId = "1" };
        var updatedCourse = new Course { Id = "10", Title = "Fizika", Description = "Mehanika", InstructorId = "1" };

        _instructorRepoMock.Reset();
        _courseRepoMock.Reset();

        _instructorRepoMock.Setup(r => r.GetByIdAsync("1")).ReturnsAsync(instructor);
        _courseRepoMock.Setup(r => r.GetByIdAsync("10")).ReturnsAsync(course);
        _courseRepoMock.Setup(r => r.UpdateAsync(It.IsAny<Course>())).Returns(Task.CompletedTask)
            .Callback<Course>(c => {
                course.Title = c.Title;
                course.Description = c.Description;
                course.InstructorId = c.InstructorId;
            });

        var updateDto = new CourseRequestDTO { Title = "Fizika", Description = "Mehanika", InstructorId = "1" };

        // Act
        await _courseService.UpdateCourseAsync("10", updateDto);
        _courseRepoMock.Setup(r => r.GetByIdAsync("10")).ReturnsAsync(course);
        var updated = await _courseService.GetCourseAsync("10");

        // Assert
        Assert.Equal("Fizika", updated.Title);
        Assert.Equal("Mehanika", updated.Description);
        _courseRepoMock.Verify(r => r.UpdateAsync(It.IsAny<Course>()), Times.Once);
    }

    [Fact]
    public async Task DeleteCourseAsync_DeletesCourse()
    {
        // Arrange
        var course = new Course { Id = "10", Title = "Matematika", Description = "Algebra", InstructorId = "1" };

        _courseRepoMock.Reset();
        _courseRepoMock.Setup(r => r.GetByIdAsync("10")).ReturnsAsync(course);
        _courseRepoMock.Setup(r => r.DeleteAsync(It.IsAny<Course>())).Returns(Task.CompletedTask);

        // Act
        await _courseService.DeleteCourseAsync("10");
        _courseRepoMock.Setup(r => r.GetByIdAsync("10")).ReturnsAsync((Course)null);

        // Assert
        await Assert.ThrowsAsync<NotFoundException>(() => _courseService.GetCourseAsync("10"));
        _courseRepoMock.Verify(r => r.DeleteAsync(It.IsAny<Course>()), Times.Once);
    }
}
