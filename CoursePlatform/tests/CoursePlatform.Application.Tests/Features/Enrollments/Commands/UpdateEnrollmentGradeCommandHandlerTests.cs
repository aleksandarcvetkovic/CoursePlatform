using CoursePlatform.Application.Common.Exceptions;
using CoursePlatform.Application.DTOs;
using CoursePlatform.Application.Features.Enrollments.Commands.UpdateEnrollmentGrade;
using CoursePlatform.Domain.Entities;
using CoursePlatform.Domain.Repositories;
using FluentAssertions;
using Moq;

namespace CoursePlatform.Application.Tests.Features.Enrollments.Commands;

public class UpdateEnrollmentGradeCommandHandlerTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IEnrollmentRepository> _enrollmentRepositoryMock;
    private readonly UpdateEnrollmentGradeCommandHandler _handler;

    public UpdateEnrollmentGradeCommandHandlerTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _enrollmentRepositoryMock = new Mock<IEnrollmentRepository>();
        _unitOfWorkMock.Setup(x => x.Enrollments).Returns(_enrollmentRepositoryMock.Object);
        _handler = new UpdateEnrollmentGradeCommandHandler(_unitOfWorkMock.Object);
    }

    [Fact]
    public async Task Handle_WithValidEnrollmentAndGrade_ShouldReturnNotNull()
    {
        // Arrange
        var enrollmentId = "enrollment-123";
        var existingEnrollment = Enrollment.Create("student-1", "course-1");
        existingEnrollment.Id = enrollmentId;

        var gradeRequest = new EnrollmentGradeRequestDTO
        {
            Grade = 85.5
        };
        var command = new UpdateEnrollmentGradeCommand(enrollmentId, gradeRequest);
        var cancellationToken = CancellationToken.None;

        _enrollmentRepositoryMock
            .Setup(x => x.GetByIdAsync(enrollmentId, cancellationToken))
            .ReturnsAsync(existingEnrollment);

        // Act
        var result = await _handler.Handle(command, cancellationToken);

        // Assert
        result.Should().NotBeNull();
    }

    [Fact]
    public async Task Handle_WithValidEnrollmentAndGrade_ShouldReturnCorrectGrade()
    {
        // Arrange
        var enrollmentId = "enrollment-123";
        var existingEnrollment = Enrollment.Create("student-1", "course-1");
        existingEnrollment.Id = enrollmentId;

        var gradeRequest = new EnrollmentGradeRequestDTO
        {
            Grade = 85.5
        };
        var command = new UpdateEnrollmentGradeCommand(enrollmentId, gradeRequest);
        var cancellationToken = CancellationToken.None;

        _enrollmentRepositoryMock
            .Setup(x => x.GetByIdAsync(enrollmentId, cancellationToken))
            .ReturnsAsync(existingEnrollment);

        // Act
        var result = await _handler.Handle(command, cancellationToken);

        // Assert
        result.Grade.Should().Be(85.5);
    }

    [Fact]
    public async Task Handle_WithValidEnrollmentAndGrade_ShouldReturnCorrectId()
    {
        // Arrange
        var enrollmentId = "enrollment-123";
        var existingEnrollment = Enrollment.Create("student-1", "course-1");
        existingEnrollment.Id = enrollmentId;

        var gradeRequest = new EnrollmentGradeRequestDTO
        {
            Grade = 85.5
        };
        var command = new UpdateEnrollmentGradeCommand(enrollmentId, gradeRequest);
        var cancellationToken = CancellationToken.None;

        _enrollmentRepositoryMock
            .Setup(x => x.GetByIdAsync(enrollmentId, cancellationToken))
            .ReturnsAsync(existingEnrollment);

        // Act
        var result = await _handler.Handle(command, cancellationToken);

        // Assert
        result.Id.Should().Be(enrollmentId);
    }

    [Fact]
    public async Task Handle_WithValidEnrollmentAndGrade_ShouldReturnCorrectStudentId()
    {
        // Arrange
        var enrollmentId = "enrollment-123";
        var existingEnrollment = Enrollment.Create("student-1", "course-1");
        existingEnrollment.Id = enrollmentId;

        var gradeRequest = new EnrollmentGradeRequestDTO
        {
            Grade = 85.5
        };
        var command = new UpdateEnrollmentGradeCommand(enrollmentId, gradeRequest);
        var cancellationToken = CancellationToken.None;

        _enrollmentRepositoryMock
            .Setup(x => x.GetByIdAsync(enrollmentId, cancellationToken))
            .ReturnsAsync(existingEnrollment);

        // Act
        var result = await _handler.Handle(command, cancellationToken);

        // Assert
        result.StudentId.Should().Be("student-1");
    }

    [Fact]
    public async Task Handle_WithValidEnrollmentAndGrade_ShouldReturnCorrectCourseId()
    {
        // Arrange
        var enrollmentId = "enrollment-123";
        var existingEnrollment = Enrollment.Create("student-1", "course-1");
        existingEnrollment.Id = enrollmentId;

        var gradeRequest = new EnrollmentGradeRequestDTO
        {
            Grade = 85.5
        };
        var command = new UpdateEnrollmentGradeCommand(enrollmentId, gradeRequest);
        var cancellationToken = CancellationToken.None;

        _enrollmentRepositoryMock
            .Setup(x => x.GetByIdAsync(enrollmentId, cancellationToken))
            .ReturnsAsync(existingEnrollment);

        // Act
        var result = await _handler.Handle(command, cancellationToken);

        // Assert
        result.CourseId.Should().Be("course-1");
    }

    [Fact]
    public async Task Handle_WithValidEnrollmentAndGrade_ShouldCallUpdateAsyncOnRepository()
    {
        // Arrange
        var enrollmentId = "enrollment-123";
        var existingEnrollment = Enrollment.Create("student-1", "course-1");
        existingEnrollment.Id = enrollmentId;

        var gradeRequest = new EnrollmentGradeRequestDTO
        {
            Grade = 85.5
        };
        var command = new UpdateEnrollmentGradeCommand(enrollmentId, gradeRequest);
        var cancellationToken = CancellationToken.None;

        _enrollmentRepositoryMock
            .Setup(x => x.GetByIdAsync(enrollmentId, cancellationToken))
            .ReturnsAsync(existingEnrollment);

        // Act
        await _handler.Handle(command, cancellationToken);

        // Assert
        _enrollmentRepositoryMock.Verify(
            x => x.UpdateAsync(It.Is<Enrollment>(e => e.Grade == 85.5), cancellationToken),
            Times.Once);
    }

    [Fact]
    public async Task Handle_WithValidEnrollmentAndGrade_ShouldCallSaveChangesAsync()
    {
        // Arrange
        var enrollmentId = "enrollment-123";
        var existingEnrollment = Enrollment.Create("student-1", "course-1");
        existingEnrollment.Id = enrollmentId;

        var gradeRequest = new EnrollmentGradeRequestDTO
        {
            Grade = 85.5
        };
        var command = new UpdateEnrollmentGradeCommand(enrollmentId, gradeRequest);
        var cancellationToken = CancellationToken.None;

        _enrollmentRepositoryMock
            .Setup(x => x.GetByIdAsync(enrollmentId, cancellationToken))
            .ReturnsAsync(existingEnrollment);

        // Act
        await _handler.Handle(command, cancellationToken);

        // Assert
        _unitOfWorkMock.Verify(x => x.SaveChangesAsync(cancellationToken), Times.Once);
    }

    [Fact]
    public async Task Handle_WithNonExistentEnrollmentId_ShouldThrowNotFoundException()
    {
        // Arrange
        var enrollmentId = "non-existent-id";
        var gradeRequest = new EnrollmentGradeRequestDTO
        {
            Grade = 85.5
        };
        var command = new UpdateEnrollmentGradeCommand(enrollmentId, gradeRequest);
        var cancellationToken = CancellationToken.None;

        _enrollmentRepositoryMock
            .Setup(x => x.GetByIdAsync(enrollmentId, cancellationToken))
            .ReturnsAsync((Enrollment?)null);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<NotFoundException>(() => _handler.Handle(command, cancellationToken));
        exception.Message.Should().Contain($"Enrollment with ID '{enrollmentId}' was not found.");
    }

    [Fact]
    public async Task Handle_WithGradeAbove100_ShouldThrowArgumentException()
    {
        // Arrange
        var enrollmentId = "enrollment-123";
        var existingEnrollment = Enrollment.Create("student-1", "course-1");
        existingEnrollment.Id = enrollmentId;

        var gradeRequest = new EnrollmentGradeRequestDTO
        {
            Grade = 110.0
        };
        var command = new UpdateEnrollmentGradeCommand(enrollmentId, gradeRequest);
        var cancellationToken = CancellationToken.None;

        _enrollmentRepositoryMock
            .Setup(x => x.GetByIdAsync(enrollmentId, cancellationToken))
            .ReturnsAsync(existingEnrollment);

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() => _handler.Handle(command, cancellationToken));
    }

    [Fact]
    public async Task Handle_WithNegativeGrade_ShouldThrowArgumentException()
    {
        // Arrange
        var enrollmentId = "enrollment-123";
        var existingEnrollment = Enrollment.Create("student-1", "course-1");
        existingEnrollment.Id = enrollmentId;

        var gradeRequest = new EnrollmentGradeRequestDTO
        {
            Grade = -5.0
        };
        var command = new UpdateEnrollmentGradeCommand(enrollmentId, gradeRequest);
        var cancellationToken = CancellationToken.None;

        _enrollmentRepositoryMock
            .Setup(x => x.GetByIdAsync(enrollmentId, cancellationToken))
            .ReturnsAsync(existingEnrollment);

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() => _handler.Handle(command, cancellationToken));
    }

    [Fact]
    public async Task Handle_WithBoundaryGrade100_ShouldUpdateSuccessfully()
    {
        // Arrange
        var enrollmentId = "enrollment-123";
        var existingEnrollment = Enrollment.Create("student-1", "course-1");
        existingEnrollment.Id = enrollmentId;

        var gradeRequest = new EnrollmentGradeRequestDTO
        {
            Grade = 100.0
        };
        var command = new UpdateEnrollmentGradeCommand(enrollmentId, gradeRequest);
        var cancellationToken = CancellationToken.None;

        _enrollmentRepositoryMock
            .Setup(x => x.GetByIdAsync(enrollmentId, cancellationToken))
            .ReturnsAsync(existingEnrollment);

        // Act
        var result = await _handler.Handle(command, cancellationToken);

        // Assert
        result.Grade.Should().Be(100.0);
    }

    [Fact]
    public async Task Handle_WithBoundaryGrade0_ShouldUpdateSuccessfully()
    {
        // Arrange
        var enrollmentId = "enrollment-123";
        var existingEnrollment = Enrollment.Create("student-1", "course-1");
        existingEnrollment.Id = enrollmentId;

        var gradeRequest = new EnrollmentGradeRequestDTO
        {
            Grade = 0.0
        };
        var command = new UpdateEnrollmentGradeCommand(enrollmentId, gradeRequest);
        var cancellationToken = CancellationToken.None;

        _enrollmentRepositoryMock
            .Setup(x => x.GetByIdAsync(enrollmentId, cancellationToken))
            .ReturnsAsync(existingEnrollment);

        // Act
        var result = await _handler.Handle(command, cancellationToken);

        // Assert
        result.Grade.Should().Be(0.0);
    }
}
