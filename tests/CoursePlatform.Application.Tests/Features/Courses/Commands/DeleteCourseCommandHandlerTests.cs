using CoursePlatform.Application.Features.Courses.Commands;
using CoursePlatform.Application.Common.Exceptions;
using CoursePlatform.Domain.Repositories;
using CoursePlatform.Domain.Entities;
using Moq;
using Xunit;
using FluentAssertions;

namespace CoursePlatform.Application.Tests.Features.Courses.Commands.DeleteCourse;

public class DeleteCourseCommandHandlerTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<ICourseRepository> _courseRepositoryMock;
    private readonly DeleteCourseCommandHandler _handler;

    public DeleteCourseCommandHandlerTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _courseRepositoryMock = new Mock<ICourseRepository>();
        _unitOfWorkMock.Setup(x => x.Courses).Returns(_courseRepositoryMock.Object);
        _handler = new DeleteCourseCommandHandler(_unitOfWorkMock.Object);
    }

    [Fact]
    public async Task Handle_WithValidCourseId_ShouldCallGetByIdAsync()
    {
        // Arrange
        var courseId = "123";
        var course = new Course { Id = courseId, Title = "Test Course" };
        var command = new DeleteCourseCommand(courseId);
        
        _courseRepositoryMock.Setup(x => x.GetByIdAsync(courseId, It.IsAny<CancellationToken>()))
                            .ReturnsAsync(course);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        _courseRepositoryMock.Verify(x => x.GetByIdAsync(courseId, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_WithValidCourseId_ShouldCallDeleteAsync()
    {
        // Arrange
        var courseId = "123";
        var course = new Course { Id = courseId, Title = "Test Course" };
        var command = new DeleteCourseCommand(courseId);
        
        _courseRepositoryMock.Setup(x => x.GetByIdAsync(courseId, It.IsAny<CancellationToken>()))
                            .ReturnsAsync(course);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        _courseRepositoryMock.Verify(x => x.DeleteAsync(course, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_WithValidCourseId_ShouldCallSaveChangesAsync()
    {
        // Arrange
        var courseId = "123";
        var course = new Course { Id = courseId, Title = "Test Course" };
        var command = new DeleteCourseCommand(courseId);
        
        _courseRepositoryMock.Setup(x => x.GetByIdAsync(courseId, It.IsAny<CancellationToken>()))
                            .ReturnsAsync(course);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        _unitOfWorkMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_WithInvalidCourseId_ShouldThrowNotFoundException()
    {
        // Arrange
        var courseId = "invalid-id";
        var command = new DeleteCourseCommand(courseId);
        
        _courseRepositoryMock.Setup(x => x.GetByIdAsync(courseId, It.IsAny<CancellationToken>()))
                            .ReturnsAsync((Course?)null);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(
            () => _handler.Handle(command, CancellationToken.None));
    }

    [Fact]
    public async Task Handle_WithInvalidCourseId_ShouldNotCallDeleteAsync()
    {
        // Arrange
        var courseId = "invalid-id";
        var command = new DeleteCourseCommand(courseId);
        
        _courseRepositoryMock.Setup(x => x.GetByIdAsync(courseId, It.IsAny<CancellationToken>()))
                            .ReturnsAsync((Course?)null);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(
            () => _handler.Handle(command, CancellationToken.None));
        
        _courseRepositoryMock.Verify(x => x.DeleteAsync(It.IsAny<Course>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Handle_WithInvalidCourseId_ShouldNotCallSaveChangesAsync()
    {
        // Arrange
        var courseId = "invalid-id";
        var command = new DeleteCourseCommand(courseId);
        
        _courseRepositoryMock.Setup(x => x.GetByIdAsync(courseId, It.IsAny<CancellationToken>()))
                            .ReturnsAsync((Course?)null);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(
            () => _handler.Handle(command, CancellationToken.None));
        
        _unitOfWorkMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }
}
