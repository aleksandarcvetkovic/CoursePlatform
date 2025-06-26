using CoursePlatform.Application.DTOs;
using CoursePlatform.Application.Features.Students.Commands.CreateStudent;
using CoursePlatform.Domain.Entities;
using CoursePlatform.Domain.Repositories;
using FluentAssertions;
using Moq;

namespace CoursePlatform.Application.Tests.Features.Students.Commands;

public class CreateStudentCommandHandlerTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IStudentRepository> _studentRepositoryMock;
    private readonly CreateStudentCommandHandler _handler;

    public CreateStudentCommandHandlerTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _studentRepositoryMock = new Mock<IStudentRepository>();
        _unitOfWorkMock.Setup(x => x.Students).Returns(_studentRepositoryMock.Object);
        _handler = new CreateStudentCommandHandler(_unitOfWorkMock.Object);
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
}
