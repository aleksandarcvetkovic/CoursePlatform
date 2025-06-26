using CoursePlatform.Application.DTOs;
using CoursePlatform.Application.Features.Instructors.Commands.CreateInstructor;
using CoursePlatform.Domain.Entities;
using CoursePlatform.Domain.Repositories;
using FluentAssertions;
using Moq;

namespace CoursePlatform.Application.Tests.Features.Instructors.Commands;

public class CreateInstructorCommandHandlerTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IInstructorRepository> _instructorRepositoryMock;
    private readonly CreateInstructorCommandHandler _handler;

    public CreateInstructorCommandHandlerTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _instructorRepositoryMock = new Mock<IInstructorRepository>();
        _unitOfWorkMock.Setup(x => x.Instructors).Returns(_instructorRepositoryMock.Object);
        _handler = new CreateInstructorCommandHandler(_unitOfWorkMock.Object);
    }

    private static CancellationToken GetCancellationToken() => CancellationToken.None;

    [Fact]
    public async Task Handle_WithValidInstructor_ShouldCreateInstructorSuccessfully()
    {
        // Arrange
        var instructorRequest = GetValidInstructorRequest();
        var command = new CreateInstructorCommand(instructorRequest);
        var cancellationToken = GetCancellationToken();

        _instructorRepositoryMock
            .Setup(x => x.AddAsync(It.IsAny<Instructor>(), cancellationToken))
            .Returns(Task.FromResult(It.IsAny<Instructor>()));

        _unitOfWorkMock
            .Setup(x => x.SaveChangesAsync(cancellationToken))
            .Returns(Task.FromResult(1));

        // Act
        var result = await _handler.Handle(command, cancellationToken);

        // Assert
        result.Should().NotBeNull();
        result.Name.Should().Be("Dr. Smith");
        result.Email.Should().Be("dr.smith@example.com");
    }

    [Fact]
    public async Task Handle_WithValidInstructor_ShouldCallAddAsyncOnRepository()
    {
        // Arrange
        var instructorRequest = GetValidInstructorRequest();
        var command = new CreateInstructorCommand(instructorRequest);
        var cancellationToken = GetCancellationToken();

        // Act
        await _handler.Handle(command, cancellationToken);

        // Assert
        _instructorRepositoryMock.Verify(
            x => x.AddAsync(It.Is<Instructor>(i => i.Name == "Dr. Smith" && i.Email == "dr.smith@example.com"), cancellationToken),
            Times.Once);
    }

    [Fact]
    public async Task Handle_WithValidInstructor_ShouldCallSaveChangesAsync()
    {
        // Arrange
        var instructorRequest = GetValidInstructorRequest();
        var command = new CreateInstructorCommand(instructorRequest);
        var cancellationToken = GetCancellationToken();

        // Act
        await _handler.Handle(command, cancellationToken);

        // Assert
        _unitOfWorkMock.Verify(x => x.SaveChangesAsync(cancellationToken), Times.Once);
    }

    [Fact]
    public async Task Handle_WithEmptyName_ShouldThrowArgumentException()
    {
        // Arrange
        var instructorRequest = GetValidInstructorRequest() with { Name = "" };
        var command = new CreateInstructorCommand(instructorRequest);
        var cancellationToken = GetCancellationToken();

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() => _handler.Handle(command, cancellationToken));
    }

    [Fact]
    public async Task Handle_WithEmptyEmail_ShouldThrowArgumentException()
    {
        // Arrange
        var instructorRequest = GetValidInstructorRequest() with { Email = "" };
        var command = new CreateInstructorCommand(instructorRequest);
        var cancellationToken = GetCancellationToken();

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() => _handler.Handle(command, cancellationToken));
    }

    [Fact]
    public async Task Handle_WithNameContainingSpaces_ShouldTrimSpaces()
    {
        // Arrange
        var instructorRequest = GetValidInstructorRequest() with { Name = "  Dr. Smith  " };
        var command = new CreateInstructorCommand(instructorRequest);
        var cancellationToken = GetCancellationToken();

        // Act
        var result = await _handler.Handle(command, cancellationToken);

        // Assert
        result.Name.Should().Be("Dr. Smith");
    }

    [Fact]
    public async Task Handle_WithEmailContainingUppercase_ShouldConvertToLowercase()
    {
        // Arrange
        var instructorRequest = GetValidInstructorRequest() with { Email = "DR.SMITH@EXAMPLE.COM" };
        var command = new CreateInstructorCommand(instructorRequest);
        var cancellationToken = GetCancellationToken();

        // Act
        var result = await _handler.Handle(command, cancellationToken);

        // Assert
        result.Email.Should().Be("dr.smith@example.com");
    }

    [Fact]
    public async Task Handle_WithWhitespaceOnlyName_ShouldThrowArgumentException()
    {
        // Arrange
        var instructorRequest = GetValidInstructorRequest() with { Name = "   " };
        var command = new CreateInstructorCommand(instructorRequest);
        var cancellationToken = GetCancellationToken();

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() => _handler.Handle(command, cancellationToken));
    }

    [Fact]
    public async Task Handle_WithNullName_ShouldThrowArgumentException()
    {
        // Arrange
        var instructorRequest = GetValidInstructorRequest() with { Name = null! };
        var command = new CreateInstructorCommand(instructorRequest);
        var cancellationToken = GetCancellationToken();

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() => _handler.Handle(command, cancellationToken));
    }

    private static InstructorRequestDTO GetValidInstructorRequest()
    {
        return new InstructorRequestDTO
        {
            Name = "Dr. Smith",
            Email = "dr.smith@example.com"
        };
    }
}
