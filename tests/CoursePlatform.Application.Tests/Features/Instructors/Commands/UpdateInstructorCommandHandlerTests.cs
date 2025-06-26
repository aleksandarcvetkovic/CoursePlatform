using CoursePlatform.Application.Features.Instructors.Commands;
using CoursePlatform.Application.DTOs;
using CoursePlatform.Application.Common.Exceptions;
using CoursePlatform.Domain.Repositories;
using CoursePlatform.Domain.Entities;
using Moq;
using Xunit;
using FluentAssertions;

namespace CoursePlatform.Application.Tests.Features.Instructors.Commands.UpdateInstructor;

public class UpdateInstructorCommandHandlerTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IInstructorRepository> _instructorRepositoryMock;
    private readonly UpdateInstructorCommandHandler _handler;

    public UpdateInstructorCommandHandlerTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _instructorRepositoryMock = new Mock<IInstructorRepository>();
        _unitOfWorkMock.Setup(x => x.Instructors).Returns(_instructorRepositoryMock.Object);
        _handler = new UpdateInstructorCommandHandler(_unitOfWorkMock.Object);
    }

    [Fact]
    public async Task Handle_WithValidInstructor_ShouldReturnNotNull()
    {
        // Arrange
        var instructorId = "123";
        var instructorRequest = new InstructorRequestDTO
        {
            Name = "Dr. John Smith",
            Email = "john.smith@example.com"
        };
        var command = new UpdateInstructorCommand(instructorId, instructorRequest);
        var existingInstructor = new Instructor { Id = instructorId, Name = "Old Name", Email = "old@email.com" };
        
        _instructorRepositoryMock.Setup(x => x.GetByIdAsync(instructorId, It.IsAny<CancellationToken>()))
                                .ReturnsAsync(existingInstructor);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
    }

    [Fact]
    public async Task Handle_WithValidInstructor_ShouldReturnCorrectId()
    {
        // Arrange
        var instructorId = "123";
        var instructorRequest = new InstructorRequestDTO
        {
            Name = "Dr. John Smith",
            Email = "john.smith@example.com"
        };
        var command = new UpdateInstructorCommand(instructorId, instructorRequest);
        var existingInstructor = new Instructor { Id = instructorId, Name = "Old Name", Email = "old@email.com" };
        
        _instructorRepositoryMock.Setup(x => x.GetByIdAsync(instructorId, It.IsAny<CancellationToken>()))
                                .ReturnsAsync(existingInstructor);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Id.Should().Be(instructorId);
    }

    [Fact]
    public async Task Handle_WithValidInstructor_ShouldReturnUpdatedName()
    {
        // Arrange
        var instructorId = "123";
        var instructorRequest = new InstructorRequestDTO
        {
            Name = "Dr. John Smith",
            Email = "john.smith@example.com"
        };
        var command = new UpdateInstructorCommand(instructorId, instructorRequest);
        var existingInstructor = new Instructor { Id = instructorId, Name = "Old Name", Email = "old@email.com" };
        
        _instructorRepositoryMock.Setup(x => x.GetByIdAsync(instructorId, It.IsAny<CancellationToken>()))
                                .ReturnsAsync(existingInstructor);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Name.Should().Be("Dr. John Smith");
    }

    [Fact]
    public async Task Handle_WithValidInstructor_ShouldReturnUpdatedEmail()
    {
        // Arrange
        var instructorId = "123";
        var instructorRequest = new InstructorRequestDTO
        {
            Name = "Dr. John Smith",
            Email = "john.smith@example.com"
        };
        var command = new UpdateInstructorCommand(instructorId, instructorRequest);
        var existingInstructor = new Instructor { Id = instructorId, Name = "Old Name", Email = "old@email.com" };
        
        _instructorRepositoryMock.Setup(x => x.GetByIdAsync(instructorId, It.IsAny<CancellationToken>()))
                                .ReturnsAsync(existingInstructor);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Email.Should().Be("john.smith@example.com");
    }

    [Fact]
    public async Task Handle_WithValidInstructor_ShouldCallGetByIdAsync()
    {
        // Arrange
        var instructorId = "123";
        var instructorRequest = new InstructorRequestDTO
        {
            Name = "Dr. John Smith",
            Email = "john.smith@example.com"
        };
        var command = new UpdateInstructorCommand(instructorId, instructorRequest);
        var existingInstructor = new Instructor { Id = instructorId, Name = "Old Name", Email = "old@email.com" };
        
        _instructorRepositoryMock.Setup(x => x.GetByIdAsync(instructorId, It.IsAny<CancellationToken>()))
                                .ReturnsAsync(existingInstructor);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        _instructorRepositoryMock.Verify(x => x.GetByIdAsync(instructorId, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_WithValidInstructor_ShouldCallUpdateAsync()
    {
        // Arrange
        var instructorId = "123";
        var instructorRequest = new InstructorRequestDTO
        {
            Name = "Dr. John Smith",
            Email = "john.smith@example.com"
        };
        var command = new UpdateInstructorCommand(instructorId, instructorRequest);
        var existingInstructor = new Instructor { Id = instructorId, Name = "Old Name", Email = "old@email.com" };
        
        _instructorRepositoryMock.Setup(x => x.GetByIdAsync(instructorId, It.IsAny<CancellationToken>()))
                                .ReturnsAsync(existingInstructor);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        _instructorRepositoryMock.Verify(x => x.UpdateAsync(existingInstructor, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_WithValidInstructor_ShouldCallSaveChangesAsync()
    {
        // Arrange
        var instructorId = "123";
        var instructorRequest = new InstructorRequestDTO
        {
            Name = "Dr. John Smith",
            Email = "john.smith@example.com"
        };
        var command = new UpdateInstructorCommand(instructorId, instructorRequest);
        var existingInstructor = new Instructor { Id = instructorId, Name = "Old Name", Email = "old@email.com" };
        
        _instructorRepositoryMock.Setup(x => x.GetByIdAsync(instructorId, It.IsAny<CancellationToken>()))
                                .ReturnsAsync(existingInstructor);

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
        var instructorRequest = new InstructorRequestDTO
        {
            Name = "Dr. John Smith",
            Email = "john.smith@example.com"
        };
        var command = new UpdateInstructorCommand(instructorId, instructorRequest);
        
        _instructorRepositoryMock.Setup(x => x.GetByIdAsync(instructorId, It.IsAny<CancellationToken>()))
                                .ReturnsAsync((Instructor?)null);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<NotFoundException>(
            () => _handler.Handle(command, CancellationToken.None));
        
        exception.Message.Should().Contain($"Instructor with ID {instructorId} not found.");
    }

    [Fact]
    public async Task Handle_WithInvalidInstructorId_ShouldNotCallUpdateAsync()
    {
        // Arrange
        var instructorId = "invalid-id";
        var instructorRequest = new InstructorRequestDTO
        {
            Name = "Dr. John Smith",
            Email = "john.smith@example.com"
        };
        var command = new UpdateInstructorCommand(instructorId, instructorRequest);
        
        _instructorRepositoryMock.Setup(x => x.GetByIdAsync(instructorId, It.IsAny<CancellationToken>()))
                                .ReturnsAsync((Instructor?)null);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(
            () => _handler.Handle(command, CancellationToken.None));
        
        _instructorRepositoryMock.Verify(x => x.UpdateAsync(It.IsAny<Instructor>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Handle_WithInvalidInstructorId_ShouldNotCallSaveChangesAsync()
    {
        // Arrange
        var instructorId = "invalid-id";
        var instructorRequest = new InstructorRequestDTO
        {
            Name = "Dr. John Smith",
            Email = "john.smith@example.com"
        };
        var command = new UpdateInstructorCommand(instructorId, instructorRequest);
        
        _instructorRepositoryMock.Setup(x => x.GetByIdAsync(instructorId, It.IsAny<CancellationToken>()))
                                .ReturnsAsync((Instructor?)null);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(
            () => _handler.Handle(command, CancellationToken.None));
        
        _unitOfWorkMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }
}
