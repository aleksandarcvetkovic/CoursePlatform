using CoursePlatform.Application.Common.Exceptions;
using CoursePlatform.Application.DTOs;
using CoursePlatform.Application.Features.Courses.Commands;
using CoursePlatform.Domain.Entities;
using CoursePlatform.Domain.Repositories;
using FluentAssertions;
using Moq;

namespace CoursePlatform.Application.Tests.Features.Courses.Commands;

public class UpdateCourseCommandHandlerTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<ICourseRepository> _courseRepositoryMock;
    private readonly UpdateCourseCommandHandler _handler;

    public UpdateCourseCommandHandlerTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _courseRepositoryMock = new Mock<ICourseRepository>();
        _unitOfWorkMock.Setup(x => x.Courses).Returns(_courseRepositoryMock.Object);
        _handler = new UpdateCourseCommandHandler(_unitOfWorkMock.Object);
    }

    private static CancellationToken GetCancellationToken() => CancellationToken.None;

    private static CourseRequestDTO GetValidCourseRequest()
    {
        return new CourseRequestDTO
        {
            Title = "New Title",
            Description = "New Description",
            InstructorId = "instructor-2"
        };
    }

    [Fact]
    public async Task Handle_WithValidCourseAndExistingId_ShouldUpdateCourseSuccessfully()
    {
        // Arrange
        var courseId = "course-123";
        var existingCourse = Course.Create("Old Title", "Old Description", "instructor-1");
        existingCourse.Id = courseId;

        var courseRequest = GetValidCourseRequest();
        var command = new UpdateCourseCommand(courseId, courseRequest);
        var cancellationToken = GetCancellationToken();

        _courseRepositoryMock
            .Setup(x => x.GetByIdAsync(courseId, cancellationToken))
            .ReturnsAsync(existingCourse);

        _courseRepositoryMock
            .Setup(x => x.UpdateAsync(It.IsAny<Course>(), cancellationToken))
            .Returns(Task.CompletedTask);

        _unitOfWorkMock
            .Setup(x => x.SaveChangesAsync(cancellationToken))
            .Returns(Task.FromResult(1));

        // Act
        var result = await _handler.Handle(command, cancellationToken);

        // Assert
        result.Should().NotBeNull();
        result.Title.Should().Be("New Title");
        result.Description.Should().Be("New Description");
        result.InstructorId.Should().Be("instructor-2");
        result.Id.Should().Be(courseId);
    }

    [Fact]
    public async Task Handle_WithValidCourseAndExistingId_ShouldCallUpdateAsyncOnRepository()
    {
        // Arrange
        var courseId = "course-123";
        var existingCourse = Course.Create("Old Title", "Old Description", "instructor-1");
        existingCourse.Id = courseId;

        var courseRequest = new CourseRequestDTO
        {
            Title = "New Title",
            Description = "New Description",
            InstructorId = "instructor-2"
        };
        var command = new UpdateCourseCommand(courseId, courseRequest);
        var cancellationToken = GetCancellationToken();

        _courseRepositoryMock
            .Setup(x => x.GetByIdAsync(courseId, cancellationToken))
            .ReturnsAsync(existingCourse);

        // Act
        await _handler.Handle(command, cancellationToken);

        // Assert
        _courseRepositoryMock.Verify(
            x => x.UpdateAsync(It.Is<Course>(c => 
                c.Title == "New Title" && 
                c.Description == "New Description" && 
                c.InstructorId == "instructor-2"), cancellationToken),
            Times.Once);
    }

    [Fact]
    public async Task Handle_WithValidCourseAndExistingId_ShouldCallSaveChangesAsync()
    {
        // Arrange
        var courseId = "course-123";
        var existingCourse = Course.Create("Old Title", "Old Description", "instructor-1");
        existingCourse.Id = courseId;

        var courseRequest = new CourseRequestDTO
        {
            Title = "New Title",
            Description = "New Description",
            InstructorId = "instructor-2"
        };
        var command = new UpdateCourseCommand(courseId, courseRequest);
        var cancellationToken = GetCancellationToken();

        _courseRepositoryMock
            .Setup(x => x.GetByIdAsync(courseId, cancellationToken))
            .ReturnsAsync(existingCourse);

        // Act
        await _handler.Handle(command, cancellationToken);

        // Assert
        _unitOfWorkMock.Verify(x => x.SaveChangesAsync(cancellationToken), Times.Once);
    }

    [Fact]
    public async Task Handle_WithNonExistentCourseId_ShouldThrowNotFoundException()
    {
        // Arrange
        var courseId = "non-existent-id";
        var courseRequest = new CourseRequestDTO
        {
            Title = "New Title",
            Description = "New Description",
            InstructorId = "instructor-2"
        };
        var command = new UpdateCourseCommand(courseId, courseRequest);
        var cancellationToken = GetCancellationToken();

        _courseRepositoryMock
            .Setup(x => x.GetByIdAsync(courseId, cancellationToken))
            .ReturnsAsync((Course?)null);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<NotFoundException>(() => _handler.Handle(command, cancellationToken));
        exception.Message.Should().Contain($"Course with ID '{courseId}' was not found.");
    }

    [Fact]
    public async Task Handle_WithEmptyTitle_ShouldThrowArgumentException()
    {
        // Arrange
        var courseId = "course-123";
        var existingCourse = Course.Create("Old Title", "Old Description", "instructor-1");
        existingCourse.Id = courseId;

        var courseRequest = new CourseRequestDTO
        {
            Title = "",
            Description = "New Description",
            InstructorId = "instructor-2"
        };
        var command = new UpdateCourseCommand(courseId, courseRequest);
        var cancellationToken = GetCancellationToken();

        _courseRepositoryMock
            .Setup(x => x.GetByIdAsync(courseId, cancellationToken))
            .ReturnsAsync(existingCourse);

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() => _handler.Handle(command, cancellationToken));
    }

    [Fact]
    public async Task Handle_WithEmptyDescription_ShouldThrowArgumentException()
    {
        // Arrange
        var courseId = "course-123";
        var existingCourse = Course.Create("Old Title", "Old Description", "instructor-1");
        existingCourse.Id = courseId;

        var courseRequest = new CourseRequestDTO
        {
            Title = "New Title",
            Description = "",
            InstructorId = "instructor-2"
        };
        var command = new UpdateCourseCommand(courseId, courseRequest);
        var cancellationToken = GetCancellationToken();

        _courseRepositoryMock
            .Setup(x => x.GetByIdAsync(courseId, cancellationToken))
            .ReturnsAsync(existingCourse);

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() => _handler.Handle(command, cancellationToken));
    }

    [Fact]
    public async Task Handle_WithTitleContainingSpaces_ShouldTrimSpaces()
    {
        // Arrange
        var courseId = "course-123";
        var existingCourse = Course.Create("Old Title", "Old Description", "instructor-1");
        existingCourse.Id = courseId;

        var courseRequest = new CourseRequestDTO
        {
            Title = "  New Title  ",
            Description = "New Description",
            InstructorId = "instructor-2"
        };
        var command = new UpdateCourseCommand(courseId, courseRequest);
        var cancellationToken = GetCancellationToken();

        _courseRepositoryMock
            .Setup(x => x.GetByIdAsync(courseId, cancellationToken))
            .ReturnsAsync(existingCourse);

        // Act
        var result = await _handler.Handle(command, cancellationToken);

        // Assert
        result.Title.Should().Be("New Title");
    }

    [Fact]
    public async Task Handle_WithDescriptionContainingSpaces_ShouldTrimSpaces()
    {
        // Arrange
        var courseId = "course-123";
        var existingCourse = Course.Create("Old Title", "Old Description", "instructor-1");
        existingCourse.Id = courseId;

        var courseRequest = new CourseRequestDTO
        {
            Title = "New Title",
            Description = "  New Description  ",
            InstructorId = "instructor-2"
        };
        var command = new UpdateCourseCommand(courseId, courseRequest);
        var cancellationToken = GetCancellationToken();

        _courseRepositoryMock
            .Setup(x => x.GetByIdAsync(courseId, cancellationToken))
            .ReturnsAsync(existingCourse);

        // Act
        var result = await _handler.Handle(command, cancellationToken);

        // Assert
        result.Description.Should().Be("New Description");
    }
}
