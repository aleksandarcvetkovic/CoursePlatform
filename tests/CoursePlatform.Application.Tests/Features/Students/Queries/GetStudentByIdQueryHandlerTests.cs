using CoursePlatform.Application.Common.Exceptions;
using CoursePlatform.Application.Features.Students.Queries.GetStudentById;
using CoursePlatform.Domain.Entities;
using CoursePlatform.Domain.Repositories;
using FluentAssertions;
using Moq;

namespace CoursePlatform.Application.Tests.Features.Students.Queries;

public class GetStudentByIdQueryHandlerTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IStudentRepository> _studentRepositoryMock;
    private readonly GetStudentByIdQueryHandler _handler;

    public GetStudentByIdQueryHandlerTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _studentRepositoryMock = new Mock<IStudentRepository>();
        _unitOfWorkMock.Setup(x => x.Students).Returns(_studentRepositoryMock.Object);
        _handler = new GetStudentByIdQueryHandler(_unitOfWorkMock.Object);
    }

    [Fact]
    public async Task Handle_WithExistingStudentId_ShouldReturnStudentDto()
    {
        // Arrange
        var studentId = "student-123";
        var student = Student.Create("John Doe", "john.doe@example.com");
        student.Id = studentId;

        var query = new GetStudentByIdQuery(studentId);
        var cancellationToken = CancellationToken.None;

        _studentRepositoryMock
            .Setup(x => x.GetByIdAsync(studentId, cancellationToken))
            .ReturnsAsync(student);

        // Act
        var result = await _handler.Handle(query, cancellationToken);

        // Assert
        result.Should().NotBeNull();
        result!.Id.Should().Be(studentId);
        result.Name.Should().Be("John Doe");
        result.Email.Should().Be("john.doe@example.com");
    }

    [Fact]
    public async Task Handle_WithExistingStudentId_ShouldCallGetByIdAsyncOnRepository()
    {
        // Arrange
        var studentId = "student-123";
        var student = Student.Create("John Doe", "john.doe@example.com");
        student.Id = studentId;

        var query = new GetStudentByIdQuery(studentId);
        var cancellationToken = CancellationToken.None;

        _studentRepositoryMock
            .Setup(x => x.GetByIdAsync(studentId, cancellationToken))
            .ReturnsAsync(student);

        // Act
        await _handler.Handle(query, cancellationToken);

        // Assert
        _studentRepositoryMock.Verify(x => x.GetByIdAsync(studentId, cancellationToken), Times.Once);
    }

    [Fact]
    public async Task Handle_WithNonExistentStudentId_ShouldThrowNotFoundException()
    {
        // Arrange
        var studentId = "non-existent-id";
        var query = new GetStudentByIdQuery(studentId);
        var cancellationToken = CancellationToken.None;

        _studentRepositoryMock
            .Setup(x => x.GetByIdAsync(studentId, cancellationToken))
            .ReturnsAsync((Student?)null);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<NotFoundException>(() => _handler.Handle(query, cancellationToken));
        exception.Message.Should().Contain($"Student with ID '{studentId}' was not found.");
    }

    [Fact]
    public async Task Handle_WithValidStudentData_ShouldReturnCorrectEmailFormat()
    {
        // Arrange
        var studentId = "student-123";
        var student = Student.Create("John Doe", "JOHN.DOE@EXAMPLE.COM");
        student.Id = studentId;

        var query = new GetStudentByIdQuery(studentId);
        var cancellationToken = CancellationToken.None;

        _studentRepositoryMock
            .Setup(x => x.GetByIdAsync(studentId, cancellationToken))
            .ReturnsAsync(student);

        // Act
        var result = await _handler.Handle(query, cancellationToken);

        // Assert
        result!.Email.Should().Be("john.doe@example.com");
    }

    [Fact]
    public async Task Handle_WithValidStudentData_ShouldReturnTrimmedName()
    {
        // Arrange
        var studentId = "student-123";
        var student = Student.Create("  John Doe  ", "john.doe@example.com");
        student.Id = studentId;

        var query = new GetStudentByIdQuery(studentId);
        var cancellationToken = CancellationToken.None;

        _studentRepositoryMock
            .Setup(x => x.GetByIdAsync(studentId, cancellationToken))
            .ReturnsAsync(student);

        // Act
        var result = await _handler.Handle(query, cancellationToken);

        // Assert
        result!.Name.Should().Be("John Doe");
    }

    [Fact]
    public async Task Handle_WithEmptyStudentId_ShouldStillCallRepository()
    {
        // Arrange
        var studentId = "";
        var query = new GetStudentByIdQuery(studentId);
        var cancellationToken = CancellationToken.None;

        _studentRepositoryMock
            .Setup(x => x.GetByIdAsync(studentId, cancellationToken))
            .ReturnsAsync((Student?)null);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() => _handler.Handle(query, cancellationToken));
        _studentRepositoryMock.Verify(x => x.GetByIdAsync(studentId, cancellationToken), Times.Once);
    }
}
