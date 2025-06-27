using CoursePlatform.Application.Features.Students.Commands.UpdateStudent;
using CoursePlatform.Application.DTOs;
using CoursePlatform.Application.Common.Exceptions;
using CoursePlatform.Domain.Repositories;
using CoursePlatform.Domain.Entities;
using Moq;
using Xunit;
using FluentAssertions;

namespace CoursePlatform.Application.Tests.Features.Students.Commands.UpdateStudent;

public class UpdateStudentCommandHandlerTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IStudentRepository> _studentRepositoryMock;
    private readonly UpdateStudentCommandHandler _handler;

    public UpdateStudentCommandHandlerTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _studentRepositoryMock = new Mock<IStudentRepository>();
        _unitOfWorkMock.Setup(x => x.Students).Returns(_studentRepositoryMock.Object);
        _handler = new UpdateStudentCommandHandler(_unitOfWorkMock.Object);
    }

    private static CancellationToken GetCancellationToken() => CancellationToken.None;

    private static StudentRequestDTO GetValidStudentRequest()
    {
        return new StudentRequestDTO
        {
            Name = "John Doe",
            Email = "john.doe@example.com"
        };
    }

    [Fact]
    public async Task Handle_WithValidStudent_ShouldReturnNotNull()
    {
        // Arrange
        var studentId = "123";
        var studentRequest = new StudentRequestDTO
        {
            Name = "John Doe",
            Email = "john.doe@example.com"
        };
        var command = new UpdateStudentCommand(studentId, studentRequest);
        var existingStudent = new Student { Id = studentId, Name = "Old Name", Email = "old@email.com" };
        
        _studentRepositoryMock.Setup(x => x.GetByIdAsync(studentId, It.IsAny<CancellationToken>()))
                             .ReturnsAsync(existingStudent);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
    }

    [Fact]
    public async Task Handle_WithValidStudent_ShouldReturnCorrectId()
    {
        // Arrange
        var studentId = "123";
        var studentRequest = new StudentRequestDTO
        {
            Name = "John Doe",
            Email = "john.doe@example.com"
        };
        var command = new UpdateStudentCommand(studentId, studentRequest);
        var existingStudent = new Student { Id = studentId, Name = "Old Name", Email = "old@email.com" };
        
        _studentRepositoryMock.Setup(x => x.GetByIdAsync(studentId, It.IsAny<CancellationToken>()))
                             .ReturnsAsync(existingStudent);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Id.Should().Be(studentId);
    }

    [Fact]
    public async Task Handle_WithValidStudent_ShouldReturnUpdatedName()
    {
        // Arrange
        var studentId = "123";
        var studentRequest = new StudentRequestDTO
        {
            Name = "John Doe",
            Email = "john.doe@example.com"
        };
        var command = new UpdateStudentCommand(studentId, studentRequest);
        var existingStudent = new Student { Id = studentId, Name = "Old Name", Email = "old@email.com" };
        
        _studentRepositoryMock.Setup(x => x.GetByIdAsync(studentId, It.IsAny<CancellationToken>()))
                             .ReturnsAsync(existingStudent);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Name.Should().Be("John Doe");
    }

    [Fact]
    public async Task Handle_WithValidStudent_ShouldReturnUpdatedEmail()
    {
        // Arrange
        var studentId = "123";
        var studentRequest = new StudentRequestDTO
        {
            Name = "John Doe",
            Email = "john.doe@example.com"
        };
        var command = new UpdateStudentCommand(studentId, studentRequest);
        var existingStudent = new Student { Id = studentId, Name = "Old Name", Email = "old@email.com" };
        
        _studentRepositoryMock.Setup(x => x.GetByIdAsync(studentId, It.IsAny<CancellationToken>()))
                             .ReturnsAsync(existingStudent);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Email.Should().Be("john.doe@example.com");
    }

    [Fact]
    public async Task Handle_WithValidStudent_ShouldCallGetByIdAsync()
    {
        // Arrange
        var studentId = "123";
        var studentRequest = new StudentRequestDTO
        {
            Name = "John Doe",
            Email = "john.doe@example.com"
        };
        var command = new UpdateStudentCommand(studentId, studentRequest);
        var existingStudent = new Student { Id = studentId, Name = "Old Name", Email = "old@email.com" };
        
        _studentRepositoryMock.Setup(x => x.GetByIdAsync(studentId, It.IsAny<CancellationToken>()))
                             .ReturnsAsync(existingStudent);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        _studentRepositoryMock.Verify(x => x.GetByIdAsync(studentId, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_WithValidStudent_ShouldCallUpdateAsync()
    {
        // Arrange
        var studentId = "123";
        var studentRequest = new StudentRequestDTO
        {
            Name = "John Doe",
            Email = "john.doe@example.com"
        };
        var command = new UpdateStudentCommand(studentId, studentRequest);
        var existingStudent = new Student { Id = studentId, Name = "Old Name", Email = "old@email.com" };
        
        _studentRepositoryMock.Setup(x => x.GetByIdAsync(studentId, It.IsAny<CancellationToken>()))
                             .ReturnsAsync(existingStudent);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        _studentRepositoryMock.Verify(x => x.UpdateAsync(existingStudent, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_WithValidStudent_ShouldCallSaveChangesAsync()
    {
        // Arrange
        var studentId = "123";
        var studentRequest = new StudentRequestDTO
        {
            Name = "John Doe",
            Email = "john.doe@example.com"
        };
        var command = new UpdateStudentCommand(studentId, studentRequest);
        var existingStudent = new Student { Id = studentId, Name = "Old Name", Email = "old@email.com" };
        
        _studentRepositoryMock.Setup(x => x.GetByIdAsync(studentId, It.IsAny<CancellationToken>()))
                             .ReturnsAsync(existingStudent);

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
        var studentRequest = new StudentRequestDTO
        {
            Name = "John Doe",
            Email = "john.doe@example.com"
        };
        var command = new UpdateStudentCommand(studentId, studentRequest);
        
        _studentRepositoryMock.Setup(x => x.GetByIdAsync(studentId, It.IsAny<CancellationToken>()))
                             .ReturnsAsync((Student?)null);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(
            () => _handler.Handle(command, CancellationToken.None));
    }

    [Fact]
    public async Task Handle_WithInvalidStudentId_ShouldNotCallUpdateAsync()
    {
        // Arrange
        var studentId = "invalid-id";
        var studentRequest = new StudentRequestDTO
        {
            Name = "John Doe",
            Email = "john.doe@example.com"
        };
        var command = new UpdateStudentCommand(studentId, studentRequest);
        
        _studentRepositoryMock.Setup(x => x.GetByIdAsync(studentId, It.IsAny<CancellationToken>()))
                             .ReturnsAsync((Student?)null);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(
            () => _handler.Handle(command, CancellationToken.None));
        
        _studentRepositoryMock.Verify(x => x.UpdateAsync(It.IsAny<Student>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Handle_WithInvalidStudentId_ShouldNotCallSaveChangesAsync()
    {
        // Arrange
        var studentId = "invalid-id";
        var studentRequest = new StudentRequestDTO
        {
            Name = "John Doe",
            Email = "john.doe@example.com"
        };
        var command = new UpdateStudentCommand(studentId, studentRequest);
        
        _studentRepositoryMock.Setup(x => x.GetByIdAsync(studentId, It.IsAny<CancellationToken>()))
                             .ReturnsAsync((Student?)null);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(
            () => _handler.Handle(command, CancellationToken.None));
        
        _unitOfWorkMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }
}
