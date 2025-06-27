using CoursePlatform.Application.Features.Students.Commands.DeleteStudent;
using CoursePlatform.Application.Common.Exceptions;
using CoursePlatform.Domain.Repositories;
using CoursePlatform.Domain.Entities;
using Moq;
using Xunit;
using FluentAssertions;

namespace CoursePlatform.Application.Tests.Features.Students.Commands.DeleteStudent;

public class DeleteStudentCommandHandlerTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IStudentRepository> _studentRepositoryMock;
    private readonly DeleteStudentCommandHandler _handler;

    public DeleteStudentCommandHandlerTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _studentRepositoryMock = new Mock<IStudentRepository>();
        _unitOfWorkMock.Setup(x => x.Students).Returns(_studentRepositoryMock.Object);
        _handler = new DeleteStudentCommandHandler(_unitOfWorkMock.Object);
    }

    [Fact]
    public async Task Handle_WithValidStudentId_ShouldCallGetByIdAsync()
    {
        // Arrange
        var studentId = "123";
        var student = new Student { Id = studentId, Name = "John Doe" };
        var command = new DeleteStudentCommand(studentId);
        
        _studentRepositoryMock.Setup(x => x.GetByIdAsync(studentId, It.IsAny<CancellationToken>()))
                             .ReturnsAsync(student);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        _studentRepositoryMock.Verify(x => x.GetByIdAsync(studentId, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_WithValidStudentId_ShouldCallDeleteAsync()
    {
        // Arrange
        var studentId = "123";
        var student = new Student { Id = studentId, Name = "John Doe" };
        var command = new DeleteStudentCommand(studentId);
        
        _studentRepositoryMock.Setup(x => x.GetByIdAsync(studentId, It.IsAny<CancellationToken>()))
                             .ReturnsAsync(student);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        _studentRepositoryMock.Verify(x => x.DeleteAsync(student, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_WithValidStudentId_ShouldCallSaveChangesAsync()
    {
        // Arrange
        var studentId = "123";
        var student = new Student { Id = studentId, Name = "John Doe" };
        var command = new DeleteStudentCommand(studentId);
        
        _studentRepositoryMock.Setup(x => x.GetByIdAsync(studentId, It.IsAny<CancellationToken>()))
                             .ReturnsAsync(student);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        _unitOfWorkMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_WithInvalidStudentId_ShouldThrowNotFoundException()
    {
        // Arrange
        var studentId = "invalid-id";
        var command = new DeleteStudentCommand(studentId);
        
        _studentRepositoryMock.Setup(x => x.GetByIdAsync(studentId, It.IsAny<CancellationToken>()))
                             .ReturnsAsync((Student?)null);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(
            () => _handler.Handle(command, CancellationToken.None));
    }

    [Fact]
    public async Task Handle_WithInvalidStudentId_ShouldNotCallDeleteAsync()
    {
        // Arrange
        var studentId = "invalid-id";
        var command = new DeleteStudentCommand(studentId);
        
        _studentRepositoryMock.Setup(x => x.GetByIdAsync(studentId, It.IsAny<CancellationToken>()))
                             .ReturnsAsync((Student?)null);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(
            () => _handler.Handle(command, CancellationToken.None));
        
        _studentRepositoryMock.Verify(x => x.DeleteAsync(It.IsAny<Student>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Handle_WithInvalidStudentId_ShouldNotCallSaveChangesAsync()
    {
        // Arrange
        var studentId = "invalid-id";
        var command = new DeleteStudentCommand(studentId);
        
        _studentRepositoryMock.Setup(x => x.GetByIdAsync(studentId, It.IsAny<CancellationToken>()))
                             .ReturnsAsync((Student?)null);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(
            () => _handler.Handle(command, CancellationToken.None));
        
        _unitOfWorkMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }
}
