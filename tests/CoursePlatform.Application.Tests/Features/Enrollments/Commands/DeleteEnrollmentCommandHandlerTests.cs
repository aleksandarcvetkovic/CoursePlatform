using CoursePlatform.Application.Features.Enrollments.Commands;
using CoursePlatform.Application.Common.Exceptions;
using CoursePlatform.Domain.Repositories;
using CoursePlatform.Domain.Entities;
using Moq;
using Xunit;
using FluentAssertions;

namespace CoursePlatform.Application.Tests.Features.Enrollments.Commands.DeleteEnrollment;

public class DeleteEnrollmentCommandHandlerTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IEnrollmentRepository> _enrollmentRepositoryMock;
    private readonly DeleteEnrollmentCommandHandler _handler;

    public DeleteEnrollmentCommandHandlerTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _enrollmentRepositoryMock = new Mock<IEnrollmentRepository>();
        _unitOfWorkMock.Setup(x => x.Enrollments).Returns(_enrollmentRepositoryMock.Object);
        _handler = new DeleteEnrollmentCommandHandler(_unitOfWorkMock.Object);
    }

    [Fact]
    public async Task Handle_WithValidEnrollmentId_ShouldCallGetByIdAsync()
    {
        // Arrange
        var enrollmentId = "123";
        var enrollment = new Enrollment { Id = enrollmentId, StudentId = "student1", CourseId = "course1" };
        var command = new DeleteEnrollmentCommand(enrollmentId);
        
        _enrollmentRepositoryMock.Setup(x => x.GetByIdAsync(enrollmentId, It.IsAny<CancellationToken>()))
                                .ReturnsAsync(enrollment);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        _enrollmentRepositoryMock.Verify(x => x.GetByIdAsync(enrollmentId, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_WithValidEnrollmentId_ShouldCallDeleteAsync()
    {
        // Arrange
        var enrollmentId = "123";
        var enrollment = new Enrollment { Id = enrollmentId, StudentId = "student1", CourseId = "course1" };
        var command = new DeleteEnrollmentCommand(enrollmentId);
        
        _enrollmentRepositoryMock.Setup(x => x.GetByIdAsync(enrollmentId, It.IsAny<CancellationToken>()))
                                .ReturnsAsync(enrollment);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        _enrollmentRepositoryMock.Verify(x => x.DeleteAsync(enrollment, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_WithValidEnrollmentId_ShouldCallSaveChangesAsync()
    {
        // Arrange
        var enrollmentId = "123";
        var enrollment = new Enrollment { Id = enrollmentId, StudentId = "student1", CourseId = "course1" };
        var command = new DeleteEnrollmentCommand(enrollmentId);
        
        _enrollmentRepositoryMock.Setup(x => x.GetByIdAsync(enrollmentId, It.IsAny<CancellationToken>()))
                                .ReturnsAsync(enrollment);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        _unitOfWorkMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_WithInvalidEnrollmentId_ShouldThrowNotFoundException()
    {
        // Arrange
        var enrollmentId = "invalid-id";
        var command = new DeleteEnrollmentCommand(enrollmentId);
        
        _enrollmentRepositoryMock.Setup(x => x.GetByIdAsync(enrollmentId, It.IsAny<CancellationToken>()))
                                .ReturnsAsync((Enrollment?)null);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<NotFoundException>(
            () => _handler.Handle(command, CancellationToken.None));
        
        exception.Message.Should().Contain($"Enrollment with ID {enrollmentId} not found.");
    }

    [Fact]
    public async Task Handle_WithInvalidEnrollmentId_ShouldNotCallDeleteAsync()
    {
        // Arrange
        var enrollmentId = "invalid-id";
        var command = new DeleteEnrollmentCommand(enrollmentId);
        
        _enrollmentRepositoryMock.Setup(x => x.GetByIdAsync(enrollmentId, It.IsAny<CancellationToken>()))
                                .ReturnsAsync((Enrollment?)null);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(
            () => _handler.Handle(command, CancellationToken.None));
        
        _enrollmentRepositoryMock.Verify(x => x.DeleteAsync(It.IsAny<Enrollment>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Handle_WithInvalidEnrollmentId_ShouldNotCallSaveChangesAsync()
    {
        // Arrange
        var enrollmentId = "invalid-id";
        var command = new DeleteEnrollmentCommand(enrollmentId);
        
        _enrollmentRepositoryMock.Setup(x => x.GetByIdAsync(enrollmentId, It.IsAny<CancellationToken>()))
                                .ReturnsAsync((Enrollment?)null);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(
            () => _handler.Handle(command, CancellationToken.None));
        
        _unitOfWorkMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }
}
