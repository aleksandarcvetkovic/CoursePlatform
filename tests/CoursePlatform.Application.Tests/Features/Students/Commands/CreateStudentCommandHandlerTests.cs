using CoursePlatform.Application.DTOs;
using CoursePlatform.Application.Features.Students.Commands.CreateStudent;
using CoursePlatform.Application.Services;
using CoursePlatform.Domain.Entities;
using CoursePlatform.Domain.Repositories;
using FluentAssertions;
using Moq;
using Xunit;

namespace CoursePlatform.Application.Tests.Features.Students.Commands;

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
        
        // Setup validation service to return success by default
        _validationServiceMock
            .Setup(x => x.ValidateStudentAsync(It.IsAny<StudentRequestDTO>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(StudentValidationResult.Success());
            
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

    [Fact]
    public async Task Handle_WithValidStudent_ShouldCreateStudentSuccessfully()
    {
        // Arrange
        var studentRequest = GetValidStudentRequest();
        var command = new CreateStudentCommand(studentRequest);
        var cancellationToken = GetCancellationToken();

        _studentRepositoryMock
            .Setup(x => x.AddAsync(It.IsAny<Student>(), cancellationToken))
            .Returns(Task.FromResult(It.IsAny<Student>()));

        _unitOfWorkMock
            .Setup(x => x.SaveChangesAsync(cancellationToken))
            .Returns(Task.FromResult(1));

        // Act
        var result = await _handler.Handle(command, cancellationToken);

        // Assert
        result.Should().NotBeNull();
        result.Name.Should().Be("John Doe");
        result.Email.Should().Be("john.doe@example.com");
    }

    [Fact]
    public async Task Handle_WithValidStudent_ShouldCallAddAsyncOnRepository()
    {
        // Arrange
        var studentRequest = GetValidStudentRequest();
        var command = new CreateStudentCommand(studentRequest);
        var cancellationToken = GetCancellationToken();

        // Act
        await _handler.Handle(command, cancellationToken);

        // Assert
        _studentRepositoryMock.Verify(
            x => x.AddAsync(It.Is<Student>(s => s.Name == "John Doe" && s.Email == "john.doe@example.com"), cancellationToken),
            Times.Once);
    }

    [Fact]
    public async Task Handle_WithValidStudent_ShouldCallSaveChangesAsync()
    {
        // Arrange
        var studentRequest = GetValidStudentRequest();
        var command = new CreateStudentCommand(studentRequest);
        var cancellationToken = GetCancellationToken();

        // Act
        await _handler.Handle(command, cancellationToken);

        // Assert
        _unitOfWorkMock.Verify(x => x.SaveChangesAsync(cancellationToken), Times.Once);
    }

    [Fact]
    public async Task Handle_WithInvalidStudent_ShouldThrowValidationException()
    {
        // Arrange
        var studentRequest = GetValidStudentRequest();
        var command = new CreateStudentCommand(studentRequest);
        var cancellationToken = GetCancellationToken();

        _validationServiceMock
            .Setup(x => x.ValidateStudentAsync(It.IsAny<StudentRequestDTO>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(StudentValidationResult.Failure("Invalid student data", new List<string> { "Email already exists" }));

        // Act & Assert
        var exception = await Assert.ThrowsAsync<FluentValidation.ValidationException>(
            () => _handler.Handle(command, cancellationToken));

        exception.Errors.Should().NotBeEmpty();
        exception.Errors.First().ErrorMessage.Should().Be("Email already exists");
    }
}
