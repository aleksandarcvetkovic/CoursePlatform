using CoursePlatform.Application.DTOs;
using CoursePlatform.Application.Features.Courses.Commands;
using CoursePlatform.Domain.Entities;
using CoursePlatform.Domain.Repositories;
using FluentAssertions;
using Moq;

namespace CoursePlatform.Application.Tests.Features.Courses.Commands;

public class CreateCourseCommandHandlerTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<ICourseRepository> _courseRepositoryMock;
    private readonly CreateCourseCommandHandler _handler;

    public CreateCourseCommandHandlerTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _courseRepositoryMock = new Mock<ICourseRepository>();
        _unitOfWorkMock.Setup(x => x.Courses).Returns(_courseRepositoryMock.Object);
        _handler = new CreateCourseCommandHandler(_unitOfWorkMock.Object);
    }

    private static CancellationToken GetCancellationToken() => CancellationToken.None;

    [Fact]
    public async Task Handle_WithValidCourse_ShouldCreateCourseSuccessfully()
    {
        // Arrange
        var courseRequest = new CourseRequestDTO
        {
            Title = "Mathematics 101",
            Description = "Introduction to Mathematics",
            InstructorId = "instructor-123"
        };
        var command = new CreateCourseCommand(courseRequest);
        var cancellationToken = GetCancellationToken();

        _courseRepositoryMock
            .Setup(x => x.AddAsync(It.IsAny<Course>(), cancellationToken))
            .Returns(Task.FromResult(It.IsAny<Course>()));

        _unitOfWorkMock
            .Setup(x => x.SaveChangesAsync(cancellationToken))
            .Returns(Task.FromResult(1));

        // Act
        var result = await _handler.Handle(command, cancellationToken);

        // Assert
        result.Should().NotBeNull();
        result.Title.Should().Be("Mathematics 101");
        result.Description.Should().Be("Introduction to Mathematics");
        result.InstructorId.Should().Be("instructor-123");
    }

    [Fact]
    public async Task Handle_WithValidCourse_ShouldCallAddAsyncOnRepository()
    {
        // Arrange
        var courseRequest = new CourseRequestDTO
        {
            Title = "Mathematics 101",
            Description = "Introduction to Mathematics",
            InstructorId = "instructor-123"
        };
        var command = new CreateCourseCommand(courseRequest);
        var cancellationToken = GetCancellationToken();

        // Act
        await _handler.Handle(command, cancellationToken);

        // Assert
        _courseRepositoryMock.Verify(
            x => x.AddAsync(It.Is<Course>(c => 
                c.Title == "Mathematics 101" && 
                c.Description == "Introduction to Mathematics" && 
                c.InstructorId == "instructor-123"), cancellationToken),
            Times.Once);
    }

    [Fact]
    public async Task Handle_WithValidCourse_ShouldCallSaveChangesAsync()
    {
        // Arrange
        var courseRequest = new CourseRequestDTO
        {
            Title = "Mathematics 101",
            Description = "Introduction to Mathematics",
            InstructorId = "instructor-123"
        };
        var command = new CreateCourseCommand(courseRequest);
        var cancellationToken = GetCancellationToken();

        // Act
        await _handler.Handle(command, cancellationToken);

        // Assert
        _unitOfWorkMock.Verify(x => x.SaveChangesAsync(cancellationToken), Times.Once);
    }

    [Fact]
    public async Task Handle_WithEmptyTitle_ShouldThrowArgumentException()
    {
        // Arrange
        var courseRequest = new CourseRequestDTO
        {
            Title = "",
            Description = "Introduction to Mathematics",
            InstructorId = "instructor-123"
        };
        var command = new CreateCourseCommand(courseRequest);
        var cancellationToken = CancellationToken.None;

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() => _handler.Handle(command, cancellationToken));
    }

    [Fact]
    public async Task Handle_WithEmptyDescription_ShouldThrowArgumentException()
    {
        // Arrange
        var courseRequest = new CourseRequestDTO
        {
            Title = "Mathematics 101",
            Description = "",
            InstructorId = "instructor-123"
        };
        var command = new CreateCourseCommand(courseRequest);
        var cancellationToken = CancellationToken.None;

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() => _handler.Handle(command, cancellationToken));
    }

    [Fact]
    public async Task Handle_WithEmptyInstructorId_ShouldThrowArgumentException()
    {
        // Arrange
        var courseRequest = new CourseRequestDTO
        {
            Title = "Mathematics 101",
            Description = "Introduction to Mathematics",
            InstructorId = ""
        };
        var command = new CreateCourseCommand(courseRequest);
        var cancellationToken = CancellationToken.None;

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() => _handler.Handle(command, cancellationToken));
    }

    [Fact]
    public async Task Handle_WithTitleContainingSpaces_ShouldTrimSpaces()
    {
        // Arrange
        var courseRequest = new CourseRequestDTO
        {
            Title = "  Mathematics 101  ",
            Description = "Introduction to Mathematics",
            InstructorId = "instructor-123"
        };
        var command = new CreateCourseCommand(courseRequest);
        var cancellationToken = CancellationToken.None;

        // Act
        var result = await _handler.Handle(command, cancellationToken);

        // Assert
        result.Title.Should().Be("Mathematics 101");
    }

    [Fact]
    public async Task Handle_WithDescriptionContainingSpaces_ShouldTrimSpaces()
    {
        // Arrange
        var courseRequest = new CourseRequestDTO
        {
            Title = "Mathematics 101",
            Description = "  Introduction to Mathematics  ",
            InstructorId = "instructor-123"
        };
        var command = new CreateCourseCommand(courseRequest);
        var cancellationToken = CancellationToken.None;

        // Act
        var result = await _handler.Handle(command, cancellationToken);

        // Assert
        result.Description.Should().Be("Introduction to Mathematics");
    }

    [Fact]
    public async Task Handle_WithWhitespaceOnlyTitle_ShouldThrowArgumentException()
    {
        // Arrange
        var courseRequest = new CourseRequestDTO
        {
            Title = "   ",
            Description = "Introduction to Mathematics",
            InstructorId = "instructor-123"
        };
        var command = new CreateCourseCommand(courseRequest);
        var cancellationToken = CancellationToken.None;

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() => _handler.Handle(command, cancellationToken));
    }

    [Fact]
    public async Task Handle_WithNullTitle_ShouldThrowArgumentException()
    {
        // Arrange
        var courseRequest = new CourseRequestDTO
        {
            Title = null!,
            Description = "Introduction to Mathematics",
            InstructorId = "instructor-123"
        };
        var command = new CreateCourseCommand(courseRequest);
        var cancellationToken = CancellationToken.None;

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() => _handler.Handle(command, cancellationToken));
    }
}
