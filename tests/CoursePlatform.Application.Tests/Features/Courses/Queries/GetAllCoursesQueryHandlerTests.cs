using CoursePlatform.Application.Features.Courses.Queries;
using CoursePlatform.Domain.Entities;
using CoursePlatform.Domain.Repositories;
using FluentAssertions;
using Moq;

namespace CoursePlatform.Application.Tests.Features.Courses.Queries;

public class GetAllCoursesQueryHandlerTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<ICourseRepository> _courseRepositoryMock;
    private readonly GetAllCoursesQueryHandler _handler;

    public GetAllCoursesQueryHandlerTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _courseRepositoryMock = new Mock<ICourseRepository>();
        _unitOfWorkMock.Setup(x => x.Courses).Returns(_courseRepositoryMock.Object);
        _handler = new GetAllCoursesQueryHandler(_unitOfWorkMock.Object);
    }

    [Fact]
    public async Task Handle_WithExistingCourses_ShouldReturnAllCourses()
    {
        // Arrange
        var course1 = Course.Create("Math 101", "Basic Mathematics", "instructor-1");
        course1.Id = "course-1";
        var course2 = Course.Create("Science 101", "Basic Science", "instructor-2");
        course2.Id = "course-2";
        var courses = new List<Course> { course1, course2 };

        var query = new GetAllCoursesQuery();
        var cancellationToken = CancellationToken.None;

        _courseRepositoryMock
            .Setup(x => x.GetAllAsync(cancellationToken))
            .ReturnsAsync(courses);

        // Act
        var result = await _handler.Handle(query, cancellationToken);

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(2);
    }

    [Fact]
    public async Task Handle_WithExistingCourses_ShouldReturnCorrectCourseData()
    {
        // Arrange
        var course1 = Course.Create("Math 101", "Basic Mathematics", "instructor-1");
        course1.Id = "course-1";
        var courses = new List<Course> { course1 };

        var query = new GetAllCoursesQuery();
        var cancellationToken = CancellationToken.None;

        _courseRepositoryMock
            .Setup(x => x.GetAllAsync(cancellationToken))
            .ReturnsAsync(courses);

        // Act
        var result = await _handler.Handle(query, cancellationToken);

        // Assert
        var courseDto = result.First();
        courseDto.Id.Should().Be("course-1");
        courseDto.Title.Should().Be("Math 101");
        courseDto.Description.Should().Be("Basic Mathematics");
        courseDto.InstructorId.Should().Be("instructor-1");
    }

    [Fact]
    public async Task Handle_WithNoCourses_ShouldReturnEmptyCollection()
    {
        // Arrange
        var courses = new List<Course>();
        var query = new GetAllCoursesQuery();
        var cancellationToken = CancellationToken.None;

        _courseRepositoryMock
            .Setup(x => x.GetAllAsync(cancellationToken))
            .ReturnsAsync(courses);

        // Act
        var result = await _handler.Handle(query, cancellationToken);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEmpty();
    }

    [Fact]
    public async Task Handle_ShouldCallGetAllAsyncOnRepository()
    {
        // Arrange
        var courses = new List<Course>();
        var query = new GetAllCoursesQuery();
        var cancellationToken = CancellationToken.None;

        _courseRepositoryMock
            .Setup(x => x.GetAllAsync(cancellationToken))
            .ReturnsAsync(courses);

        // Act
        await _handler.Handle(query, cancellationToken);

        // Assert
        _courseRepositoryMock.Verify(x => x.GetAllAsync(cancellationToken), Times.Once);
    }

    [Fact]
    public async Task Handle_WithMultipleCourses_ShouldPreserveOrder()
    {
        // Arrange
        var course1 = Course.Create("Alpha Course", "First course", "instructor-1");
        course1.Id = "course-1";
        var course2 = Course.Create("Beta Course", "Second course", "instructor-2");
        course2.Id = "course-2";
        var course3 = Course.Create("Gamma Course", "Third course", "instructor-3");
        course3.Id = "course-3";
        var courses = new List<Course> { course1, course2, course3 };

        var query = new GetAllCoursesQuery();
        var cancellationToken = CancellationToken.None;

        _courseRepositoryMock
            .Setup(x => x.GetAllAsync(cancellationToken))
            .ReturnsAsync(courses);

        // Act
        var result = await _handler.Handle(query, cancellationToken);

        // Assert
        var resultList = result.ToList();
        resultList[0].Title.Should().Be("Alpha Course");
        resultList[1].Title.Should().Be("Beta Course");
        resultList[2].Title.Should().Be("Gamma Course");
    }
}
