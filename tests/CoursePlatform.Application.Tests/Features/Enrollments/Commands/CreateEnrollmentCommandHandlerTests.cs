using CoursePlatform.Application.DTOs;
using CoursePlatform.Application.Features.Enrollments.Commands.CreateEnrollment;
using CoursePlatform.Domain.Entities;
using CoursePlatform.Domain.Repositories;
using FluentAssertions;
using Moq;

namespace CoursePlatform.Application.Tests.Features.Enrollments.Commands;

public class CreateEnrollmentCommandHandlerTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IEnrollmentRepository> _enrollmentRepositoryMock;
    private readonly CreateEnrollmentCommandHandler _handler;

    public CreateEnrollmentCommandHandlerTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _enrollmentRepositoryMock = new Mock<IEnrollmentRepository>();
        _unitOfWorkMock.Setup(x => x.Enrollments).Returns(_enrollmentRepositoryMock.Object);
        _handler = new CreateEnrollmentCommandHandler(_unitOfWorkMock.Object);
    }

    [Fact]
    public async Task Handle_WithValidEnrollment_ShouldCreateEnrollmentSuccessfully()
    {
        // Arrange
        var enrollmentRequest = new EnrollmentRequestDTO
        {
            StudentId = "student-123",
            CourseId = "course-456"
        };
        var command = new CreateEnrollmentCommand(enrollmentRequest);
        var cancellationToken = CancellationToken.None;

        _enrollmentRepositoryMock
            .Setup(x => x.AddAsync(It.IsAny<Enrollment>(), cancellationToken))
            .Returns(Task.FromResult(It.IsAny<Enrollment>()));

        _unitOfWorkMock
            .Setup(x => x.SaveChangesAsync(cancellationToken))
            .Returns(Task.FromResult(1));

        // Act
        var result = await _handler.Handle(command, cancellationToken);

        // Assert
        result.Should().NotBeNull();
        result.StudentId.Should().Be("student-123");
        result.CourseId.Should().Be("course-456");
        result.EnrolledOn.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));
        result.Grade.Should().BeNull();
    }

    [Fact]
    public async Task Handle_WithValidEnrollment_ShouldCallAddAsyncOnRepository()
    {
        // Arrange
        var enrollmentRequest = new EnrollmentRequestDTO
        {
            StudentId = "student-123",
            CourseId = "course-456"
        };
        var command = new CreateEnrollmentCommand(enrollmentRequest);
        var cancellationToken = CancellationToken.None;

        // Act
        await _handler.Handle(command, cancellationToken);

        // Assert
        _enrollmentRepositoryMock.Verify(
            x => x.AddAsync(It.Is<Enrollment>(e => 
                e.StudentId == "student-123" && 
                e.CourseId == "course-456"), cancellationToken),
            Times.Once);
    }

    [Fact]
    public async Task Handle_WithValidEnrollment_ShouldCallSaveChangesAsync()
    {
        // Arrange
        var enrollmentRequest = new EnrollmentRequestDTO
        {
            StudentId = "student-123",
            CourseId = "course-456"
        };
        var command = new CreateEnrollmentCommand(enrollmentRequest);
        var cancellationToken = CancellationToken.None;

        // Act
        await _handler.Handle(command, cancellationToken);

        // Assert
        _unitOfWorkMock.Verify(x => x.SaveChangesAsync(cancellationToken), Times.Once);
    }

    [Fact]
    public async Task Handle_WithValidEnrollment_ShouldSetEnrolledOnToCurrentTime()
    {
        // Arrange
        var enrollmentRequest = new EnrollmentRequestDTO
        {
            StudentId = "student-123",
            CourseId = "course-456"
        };
        var command = new CreateEnrollmentCommand(enrollmentRequest);
        var cancellationToken = CancellationToken.None;
        var timeBeforeCall = DateTime.UtcNow;

        // Act
        var result = await _handler.Handle(command, cancellationToken);

        // Assert
        var timeAfterCall = DateTime.UtcNow;
        result.EnrolledOn.Should().BeOnOrAfter(timeBeforeCall);
        result.EnrolledOn.Should().BeOnOrBefore(timeAfterCall);
    }

    [Fact]
    public async Task Handle_WithNewEnrollment_ShouldHaveNullGrade()
    {
        // Arrange
        var enrollmentRequest = new EnrollmentRequestDTO
        {
            StudentId = "student-123",
            CourseId = "course-456"
        };
        var command = new CreateEnrollmentCommand(enrollmentRequest);
        var cancellationToken = CancellationToken.None;

        // Act
        var result = await _handler.Handle(command, cancellationToken);

        // Assert
        result.Grade.Should().BeNull();
    }

    [Fact]
    public async Task Handle_WithValidIds_ShouldPreserveStudentAndCourseIds()
    {
        // Arrange
        var enrollmentRequest = new EnrollmentRequestDTO
        {
            StudentId = "student-999",
            CourseId = "course-888"
        };
        var command = new CreateEnrollmentCommand(enrollmentRequest);
        var cancellationToken = CancellationToken.None;

        // Act
        var result = await _handler.Handle(command, cancellationToken);

        // Assert
        result.StudentId.Should().Be("student-999");
        result.CourseId.Should().Be("course-888");
    }

    [Fact]
    public async Task Handle_WithDifferentStudentsAndCourses_ShouldCreateSeparateEnrollments()
    {
        // Arrange
        var enrollmentRequest1 = new EnrollmentRequestDTO
        {
            StudentId = "student-1",
            CourseId = "course-1"
        };
        var enrollmentRequest2 = new EnrollmentRequestDTO
        {
            StudentId = "student-2",
            CourseId = "course-2"
        };
        var command1 = new CreateEnrollmentCommand(enrollmentRequest1);
        var command2 = new CreateEnrollmentCommand(enrollmentRequest2);
        var cancellationToken = CancellationToken.None;

        // Act
        var result1 = await _handler.Handle(command1, cancellationToken);
        var result2 = await _handler.Handle(command2, cancellationToken);

        // Assert
        result1.StudentId.Should().Be("student-1");
        result1.CourseId.Should().Be("course-1");
        result2.StudentId.Should().Be("student-2");
        result2.CourseId.Should().Be("course-2");
    }
}
