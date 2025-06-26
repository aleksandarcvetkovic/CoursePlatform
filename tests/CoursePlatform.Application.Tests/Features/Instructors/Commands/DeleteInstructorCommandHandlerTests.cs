using CoursePlatform.Application.Features.Instructors.Commands;
using CoursePlatform.Application.Common.Exceptions;
using CoursePlatform.Domain.Repositories;
using CoursePlatform.Domain.Entities;
using Moq;
using Xunit;
using FluentAssertions;

namespace CoursePlatform.Application.Tests.Features.Instructors.Commands.DeleteInstructor;

public class DeleteInstructorCommandHandlerTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IInstructorRepository> _instructorRepositoryMock;
    private readonly DeleteInstructorCommandHandler _handler;

    public DeleteInstructorCommandHandlerTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _instructorRepositoryMock = new Mock<IInstructorRepository>();
        _unitOfWorkMock.Setup(x => x.Instructors).Returns(_instructorRepositoryMock.Object);
        _handler = new DeleteInstructorCommandHandler(_unitOfWorkMock.Object);
    }

    [Fact]
    public async Task Handle_WithValidInstructorId_ShouldCallGetByIdAsync()
    {
        // Arrange
        var instructorId = "123";
        var instructor = new Instructor { Id = instructorId, Name = "Dr. Smith" };
        var command = new DeleteInstructorCommand(instructorId);
        
        _instructorRepositoryMock.Setup(x => x.GetByIdAsync(instructorId, It.IsAny<CancellationToken>()))
                                .ReturnsAsync(instructor);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        _instructorRepositoryMock.Verify(x => x.GetByIdAsync(instructorId, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_WithValidInstructorId_ShouldCallDeleteAsync()
    {
        // Arrange
        var instructorId = "123";
        var instructor = new Instructor { Id = instructorId, Name = "Dr. Smith" };
        var command = new DeleteInstructorCommand(instructorId);
        
        _instructorRepositoryMock.Setup(x => x.GetByIdAsync(instructorId, It.IsAny<CancellationToken>()))
                                .ReturnsAsync(instructor);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        _instructorRepositoryMock.Verify(x => x.DeleteAsync(instructor, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_WithValidInstructorId_ShouldCallSaveChangesAsync()
    {
        // Arrange
        var instructorId = "123";
        var instructor = new Instructor { Id = instructorId, Name = "Dr. Smith" };
        var command = new DeleteInstructorCommand(instructorId);
        
        _instructorRepositoryMock.Setup(x => x.GetByIdAsync(instructorId, It.IsAny<CancellationToken>()))
                                .ReturnsAsync(instructor);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        _unitOfWorkMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_WithInvalidInstructorId_ShouldThrowNotFoundException()
    {
        // Arrange
        var instructorId = "invalid-id";
        var command = new DeleteInstructorCommand(instructorId);
        
        _instructorRepositoryMock.Setup(x => x.GetByIdAsync(instructorId, It.IsAny<CancellationToken>()))
                                .ReturnsAsync((Instructor?)null);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<NotFoundException>(
            () => _handler.Handle(command, CancellationToken.None));
        
        exception.Message.Should().Contain($"Instructor with ID {instructorId} not found.");
    }

    [Fact]
    public async Task Handle_WithInvalidInstructorId_ShouldNotCallDeleteAsync()
    {
        // Arrange
        var instructorId = "invalid-id";
        var command = new DeleteInstructorCommand(instructorId);
        
        _instructorRepositoryMock.Setup(x => x.GetByIdAsync(instructorId, It.IsAny<CancellationToken>()))
                                .ReturnsAsync((Instructor?)null);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(
            () => _handler.Handle(command, CancellationToken.None));
        
        _instructorRepositoryMock.Verify(x => x.DeleteAsync(It.IsAny<Instructor>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Handle_WithInvalidInstructorId_ShouldNotCallSaveChangesAsync()
    {
        // Arrange
        var instructorId = "invalid-id";
        var command = new DeleteInstructorCommand(instructorId);
        
        _instructorRepositoryMock.Setup(x => x.GetByIdAsync(instructorId, It.IsAny<CancellationToken>()))
                                .ReturnsAsync((Instructor?)null);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(
            () => _handler.Handle(command, CancellationToken.None));
        
        _unitOfWorkMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }
}
