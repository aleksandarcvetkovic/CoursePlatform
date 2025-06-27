using CoursePlatform.Application.Features.Students.Commands.CreateStudent;
using CoursePlatform.Application.DTOs;
using CoursePlatform.Application.Common.Exceptions;
using CoursePlatform.Application.Services;
using CoursePlatform.Domain.Repositories;
using CoursePlatform.Domain.Entities;
using Moq;
using Xunit;
using FluentAssertions;

namespace CoursePlatform.Application.Tests.Features.Students.Commands.CreateStudent;

public class CreateStudentCommandHandlerTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IStudentRepository> _studentRepositoryMock;
    private readonly Mock<IStudentValidationService> _validationServiceMock;
    private readonly CreateStudentCommandHandler _handler;

    public CreateStudentCommandHandlerTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _studentRepositoryMock = new Mock<IStudentRepository>();
        _validationServiceMock = new Mock<IStudentValidationService>();
        _unitOfWorkMock.Setup(x => x.Students).Returns(_studentRepositoryMock.Object);
        _handler = new CreateStudentCommandHandler(_unitOfWorkMock.Object, _validationServiceMock.Object);
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

    private static CreateStudentCommand GetValidCreateStudentCommand()
    {
        return new CreateStudentCommand(GetValidStudentRequest());
    }

    [Fact]
    public async Task Handle_WithValidStudent_ShouldCallValidateStudentAsync()
    {
        // Arrange
        var command = GetValidCreateStudentCommand();
        var validationResult = StudentValidationResult.Success();
        _validationServiceMock.Setup(x => x.ValidateStudentAsync(command.Student, It.IsAny<CancellationToken>()))
                             .ReturnsAsync(validationResult);

        // Act
        await _handler.Handle(command, GetCancellationToken());

        // Assert
        _validationServiceMock.Verify(x => x.ValidateStudentAsync(command.Student, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_WithValidStudent_ShouldCallAddAsync()
    {
        // Arrange
        var command = GetValidCreateStudentCommand();
        var validationResult = StudentValidationResult.Success();
        _validationServiceMock.Setup(x => x.ValidateStudentAsync(command.Student, It.IsAny<CancellationToken>()))
                             .ReturnsAsync(validationResult);

        // Act
        await _handler.Handle(command, GetCancellationToken());

        // Assert
        _studentRepositoryMock.Verify(x => x.AddAsync(It.IsAny<Student>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_WithValidStudent_ShouldCallSaveChangesAsync()
    {
        // Arrange
        var command = GetValidCreateStudentCommand();
        var validationResult = StudentValidationResult.Success();
        _validationServiceMock.Setup(x => x.ValidateStudentAsync(command.Student, It.IsAny<CancellationToken>()))
                             .ReturnsAsync(validationResult);

        // Act
        await _handler.Handle(command, GetCancellationToken());

        // Assert
        _unitOfWorkMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_WithValidStudent_ShouldReturnNotNull()
    {
        // Arrange
        var command = GetValidCreateStudentCommand();
        var validationResult = StudentValidationResult.Success();
        _validationServiceMock.Setup(x => x.ValidateStudentAsync(command.Student, It.IsAny<CancellationToken>()))
                             .ReturnsAsync(validationResult);

        // Act
        var result = await _handler.Handle(command, GetCancellationToken());

        // Assert
        result.Should().NotBeNull();
    }

    [Fact]
    public async Task Handle_WithValidStudent_ShouldReturnCorrectName()
    {
        // Arrange
        var command = GetValidCreateStudentCommand();
        var validationResult = StudentValidationResult.Success();
        _validationServiceMock.Setup(x => x.ValidateStudentAsync(command.Student, It.IsAny<CancellationToken>()))
                             .ReturnsAsync(validationResult);

        // Act
        var result = await _handler.Handle(command, GetCancellationToken());

        // Assert
        result.Name.Should().Be("John Doe");
    }

    [Fact]
    public async Task Handle_WithValidStudent_ShouldReturnCorrectEmail()
    {
        // Arrange
        var command = GetValidCreateStudentCommand();
        var validationResult = StudentValidationResult.Success();
        _validationServiceMock.Setup(x => x.ValidateStudentAsync(command.Student, It.IsAny<CancellationToken>()))
                             .ReturnsAsync(validationResult);

        // Act
        var result = await _handler.Handle(command, GetCancellationToken());

        // Assert
        result.Email.Should().Be("john.doe@example.com");
    }

    [Fact]
    public async Task Handle_WithValidStudent_ShouldReturnNonEmptyId()
    {
        // Arrange
        var command = GetValidCreateStudentCommand();
        var validationResult = StudentValidationResult.Success();
        _validationServiceMock.Setup(x => x.ValidateStudentAsync(command.Student, It.IsAny<CancellationToken>()))
                             .ReturnsAsync(validationResult);

        // Act
        var result = await _handler.Handle(command, GetCancellationToken());

        // Assert
        result.Id.Should().NotBeEmpty();
    }

    [Fact]
    public async Task Handle_WithInvalidValidationResult_ShouldThrowBadRequestException()
    {
        // Arrange
        var command = GetValidCreateStudentCommand();
        var validationResult = StudentValidationResult.Failure("Validation failed");
        _validationServiceMock.Setup(x => x.ValidateStudentAsync(command.Student, It.IsAny<CancellationToken>()))
                             .ReturnsAsync(validationResult);

        // Act & Assert
        await Assert.ThrowsAsync<BadRequestException>(
            () => _handler.Handle(command, GetCancellationToken()));
    }

    [Fact]
    public async Task Handle_WithInvalidValidationResult_ShouldNotCallAddAsync()
    {
        // Arrange
        var command = GetValidCreateStudentCommand();
        var validationResult = StudentValidationResult.Failure("Validation failed");
        _validationServiceMock.Setup(x => x.ValidateStudentAsync(command.Student, It.IsAny<CancellationToken>()))
                             .ReturnsAsync(validationResult);

        // Act & Assert
        await Assert.ThrowsAsync<BadRequestException>(
            () => _handler.Handle(command, GetCancellationToken()));
        
        _studentRepositoryMock.Verify(x => x.AddAsync(It.IsAny<Student>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Handle_WithInvalidValidationResult_ShouldNotCallSaveChangesAsync()
    {
        // Arrange
        var command = GetValidCreateStudentCommand();
        var validationResult = StudentValidationResult.Failure("Validation failed");
        _validationServiceMock.Setup(x => x.ValidateStudentAsync(command.Student, It.IsAny<CancellationToken>()))
                             .ReturnsAsync(validationResult);

        // Act & Assert
        await Assert.ThrowsAsync<BadRequestException>(
            () => _handler.Handle(command, GetCancellationToken()));
        
        _unitOfWorkMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Handle_WithValidationServiceException_ShouldThrowBadRequestException()
    {
        // Arrange
        var command = GetValidCreateStudentCommand();
        _validationServiceMock.Setup(x => x.ValidateStudentAsync(command.Student, It.IsAny<CancellationToken>()))
                             .ThrowsAsync(new Exception("Validation service error"));

        // Act & Assert
        await Assert.ThrowsAsync<Exception>(
            () => _handler.Handle(command, GetCancellationToken()));
    }

    [Fact]
    public async Task Handle_WithValidationServiceException_ShouldNotCallAddAsync()
    {
        // Arrange
        var command = GetValidCreateStudentCommand();
        _validationServiceMock.Setup(x => x.ValidateStudentAsync(command.Student, It.IsAny<CancellationToken>()))
                             .ThrowsAsync(new Exception("Validation service error"));

        // Act & Assert
        await Assert.ThrowsAsync<Exception>(
            () => _handler.Handle(command, GetCancellationToken()));
        
        _studentRepositoryMock.Verify(x => x.AddAsync(It.IsAny<Student>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Handle_WithValidationServiceException_ShouldNotCallSaveChangesAsync()
    {
        // Arrange
        var command = GetValidCreateStudentCommand();
        _validationServiceMock.Setup(x => x.ValidateStudentAsync(command.Student, It.IsAny<CancellationToken>()))
                             .ThrowsAsync(new Exception("Validation service error"));

        // Act & Assert
        await Assert.ThrowsAsync<Exception>(
            () => _handler.Handle(command, GetCancellationToken()));
        
        _unitOfWorkMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Handle_WithRepositoryException_ShouldThrowException()
    {
        // Arrange
        var command = GetValidCreateStudentCommand();
        var validationResult = StudentValidationResult.Success();
        _validationServiceMock.Setup(x => x.ValidateStudentAsync(command.Student, It.IsAny<CancellationToken>()))
                             .ReturnsAsync(validationResult);
        _studentRepositoryMock.Setup(x => x.AddAsync(It.IsAny<Student>(), It.IsAny<CancellationToken>()))
                             .ThrowsAsync(new Exception("Repository error"));

        // Act & Assert
        await Assert.ThrowsAsync<Exception>(
            () => _handler.Handle(command, GetCancellationToken()));
    }

    [Fact]
    public async Task Handle_WithSaveChangesException_ShouldThrowException()
    {
        // Arrange
        var command = GetValidCreateStudentCommand();
        var validationResult = StudentValidationResult.Success();
        _validationServiceMock.Setup(x => x.ValidateStudentAsync(command.Student, It.IsAny<CancellationToken>()))
                             .ReturnsAsync(validationResult);
        _unitOfWorkMock.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
                      .ThrowsAsync(new Exception("Save changes error"));

        // Act & Assert
        await Assert.ThrowsAsync<Exception>(
            () => _handler.Handle(command, GetCancellationToken()));
    }

    [Fact]
    public async Task Handle_WithNullErrorMessage_ShouldThrowBadRequestExceptionWithDefaultMessage()
    {
        // Arrange
        var command = GetValidCreateStudentCommand();
        var validationResult = StudentValidationResult.Failure(null);
        _validationServiceMock.Setup(x => x.ValidateStudentAsync(command.Student, It.IsAny<CancellationToken>()))
                             .ReturnsAsync(validationResult);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<BadRequestException>(
            () => _handler.Handle(command, GetCancellationToken()));
        
        exception.Message.Should().NotBeNull();
    }

    [Fact]
    public async Task Handle_WithSpecificErrorMessage_ShouldThrowBadRequestExceptionWithSpecificMessage()
    {
        // Arrange
        var command = GetValidCreateStudentCommand();
        var errorMessage = "Email domain is not allowed";
        var validationResult = StudentValidationResult.Failure(errorMessage);
        _validationServiceMock.Setup(x => x.ValidateStudentAsync(command.Student, It.IsAny<CancellationToken>()))
                             .ReturnsAsync(validationResult);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<BadRequestException>(
            () => _handler.Handle(command, GetCancellationToken()));
        
        exception.Message.Should().Be(errorMessage);
    }

    [Fact]
    public async Task Handle_WithValidStudent_ShouldCreateStudentWithCorrectName()
    {
        // Arrange
        var command = GetValidCreateStudentCommand();
        var validationResult = StudentValidationResult.Success();
        _validationServiceMock.Setup(x => x.ValidateStudentAsync(command.Student, It.IsAny<CancellationToken>()))
                             .ReturnsAsync(validationResult);
        Student capturedStudent = null;
        _studentRepositoryMock.Setup(x => x.AddAsync(It.IsAny<Student>(), It.IsAny<CancellationToken>()))
                             .Callback<Student, CancellationToken>((student, ct) => capturedStudent = student);

        // Act
        await _handler.Handle(command, GetCancellationToken());

        // Assert
        capturedStudent.Name.Should().Be("John Doe");
    }

    [Fact]
    public async Task Handle_WithValidStudent_ShouldCreateStudentWithCorrectEmail()
    {
        // Arrange
        var command = GetValidCreateStudentCommand();
        var validationResult = StudentValidationResult.Success();
        _validationServiceMock.Setup(x => x.ValidateStudentAsync(command.Student, It.IsAny<CancellationToken>()))
                             .ReturnsAsync(validationResult);
        Student capturedStudent = null;
        _studentRepositoryMock.Setup(x => x.AddAsync(It.IsAny<Student>(), It.IsAny<CancellationToken>()))
                             .Callback<Student, CancellationToken>((student, ct) => capturedStudent = student);

        // Act
        await _handler.Handle(command, GetCancellationToken());

        // Assert
        capturedStudent.Email.Should().Be("john.doe@example.com");
    }

    [Fact]
    public async Task Handle_WithValidStudent_ShouldCreateStudentWithNonEmptyId()
    {
        // Arrange
        var command = GetValidCreateStudentCommand();
        var validationResult = StudentValidationResult.Success();
        _validationServiceMock.Setup(x => x.ValidateStudentAsync(command.Student, It.IsAny<CancellationToken>()))
                             .ReturnsAsync(validationResult);
        Student capturedStudent = null;
        _studentRepositoryMock.Setup(x => x.AddAsync(It.IsAny<Student>(), It.IsAny<CancellationToken>()))
                             .Callback<Student, CancellationToken>((student, ct) => capturedStudent = student);

        // Act
        await _handler.Handle(command, GetCancellationToken());

        // Assert
        capturedStudent.Id.Should().NotBeEmpty();
    }

    [Fact]
    public async Task Handle_WithEmptyStudentName_ShouldStillCallValidationService()
    {
        // Arrange
        var studentRequest = new StudentRequestDTO
        {
            Name = "",
            Email = "john.doe@example.com"
        };
        var command = new CreateStudentCommand(studentRequest);
        var validationResult = StudentValidationResult.Failure("Name is required");
        _validationServiceMock.Setup(x => x.ValidateStudentAsync(studentRequest, It.IsAny<CancellationToken>()))
                             .ReturnsAsync(validationResult);

        // Act & Assert
        await Assert.ThrowsAsync<BadRequestException>(
            () => _handler.Handle(command, GetCancellationToken()));
        
        _validationServiceMock.Verify(x => x.ValidateStudentAsync(studentRequest, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_WithEmptyStudentEmail_ShouldStillCallValidationService()
    {
        // Arrange
        var studentRequest = new StudentRequestDTO
        {
            Name = "John Doe",
            Email = ""
        };
        var command = new CreateStudentCommand(studentRequest);
        var validationResult = StudentValidationResult.Failure("Email is required");
        _validationServiceMock.Setup(x => x.ValidateStudentAsync(studentRequest, It.IsAny<CancellationToken>()))
                             .ReturnsAsync(validationResult);

        // Act & Assert
        await Assert.ThrowsAsync<BadRequestException>(
            () => _handler.Handle(command, GetCancellationToken()));
        
        _validationServiceMock.Verify(x => x.ValidateStudentAsync(studentRequest, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_WithDifferentStudentData_ShouldReturnCorrectData()
    {
        // Arrange
        var studentRequest = new StudentRequestDTO
        {
            Name = "Jane Smith",
            Email = "jane.smith@university.edu"
        };
        var command = new CreateStudentCommand(studentRequest);
        var validationResult = StudentValidationResult.Success();
        _validationServiceMock.Setup(x => x.ValidateStudentAsync(studentRequest, It.IsAny<CancellationToken>()))
                             .ReturnsAsync(validationResult);

        // Act
        var result = await _handler.Handle(command, GetCancellationToken());

        // Assert
        result.Name.Should().Be("Jane Smith");
    }

    [Fact]
    public async Task Handle_WithDifferentStudentEmail_ShouldReturnCorrectEmail()
    {
        // Arrange
        var studentRequest = new StudentRequestDTO
        {
            Name = "Jane Smith",
            Email = "jane.smith@university.edu"
        };
        var command = new CreateStudentCommand(studentRequest);
        var validationResult = StudentValidationResult.Success();
        _validationServiceMock.Setup(x => x.ValidateStudentAsync(studentRequest, It.IsAny<CancellationToken>()))
                             .ReturnsAsync(validationResult);

        // Act
        var result = await _handler.Handle(command, GetCancellationToken());

        // Assert
        result.Email.Should().Be("jane.smith@university.edu");
    }
}
