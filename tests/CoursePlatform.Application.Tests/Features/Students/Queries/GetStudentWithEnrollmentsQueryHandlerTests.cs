using CoursePlatform.Application.Common.Exceptions;
using CoursePlatform.Application.Features.Students.Queries.GetStudentWithEnrollments;
using CoursePlatform.Domain.Entities;
using CoursePlatform.Domain.Repositories;
using FluentAssertions;
using Moq;

namespace CoursePlatform.Application.Tests.Features.Students.Queries;

public class GetStudentWithEnrollmentsQueryHandlerTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IStudentRepository> _studentRepositoryMock;
    private readonly GetStudentWithEnrollmentsQueryHandler _handler;

    public GetStudentWithEnrollmentsQueryHandlerTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _studentRepositoryMock = new Mock<IStudentRepository>();
        _unitOfWorkMock.Setup(x => x.Students).Returns(_studentRepositoryMock.Object);
        _handler = new GetStudentWithEnrollmentsQueryHandler(_unitOfWorkMock.Object);
    }

    [Fact]
    public async Task Handle_WithExistingStudentId_ShouldReturnStudentWithEnrollments()
    {
        // Arrange
        var studentId = "student-123";
        var student = Student.Create("John Doe", "john.doe@example.com");
        student.Id = studentId;
        student.Enrollments = new List<Enrollment>();

        var query = new GetStudentWithEnrollmentsQuery(studentId);
        var cancellationToken = CancellationToken.None;

        _studentRepositoryMock
            .Setup(x => x.GetByIdWithEnrollmentsAsync(studentId, cancellationToken))
            .ReturnsAsync(student);

        // Act
        var result = await _handler.Handle(query, cancellationToken);

        // Assert
        result.Should().NotBeNull();
        result!.Id.Should().Be(studentId);
        result.Name.Should().Be("John Doe");
        result.Email.Should().Be("john.doe@example.com");
        result.Enrollments.Should().NotBeNull();
    }

    [Fact]
    public async Task Handle_WithExistingStudentIdAndEnrollments_ShouldReturnEnrollments()
    {
        // Arrange
        var studentId = "student-123";
        var student = Student.Create("John Doe", "john.doe@example.com");
        student.Id = studentId;
        
        var course = Course.Create("Math 101", "Basic Mathematics", "instructor-1");
        course.Id = "course-1";
        
        var enrollment = Enrollment.Create(studentId, course.Id);
        enrollment.Id = "enrollment-1";
        student.Enrollments = new List<Enrollment> { enrollment };

        var query = new GetStudentWithEnrollmentsQuery(studentId);
        var cancellationToken = CancellationToken.None;

        _studentRepositoryMock
            .Setup(x => x.GetByIdWithEnrollmentsAsync(studentId, cancellationToken))
            .ReturnsAsync(student);

        // Act
        var result = await _handler.Handle(query, cancellationToken);

        // Assert
        result!.Enrollments.Should().HaveCount(1);
        result.Enrollments.First().Id.Should().Be("enrollment-1");
    }

    [Fact]
    public async Task Handle_WithExistingStudentId_ShouldCallGetByIdWithEnrollmentsAsync()
    {
        // Arrange
        var studentId = "student-123";
        var student = Student.Create("John Doe", "john.doe@example.com");
        student.Id = studentId;
        student.Enrollments = new List<Enrollment>();

        var query = new GetStudentWithEnrollmentsQuery(studentId);
        var cancellationToken = CancellationToken.None;

        _studentRepositoryMock
            .Setup(x => x.GetByIdWithEnrollmentsAsync(studentId, cancellationToken))
            .ReturnsAsync(student);

        // Act
        await _handler.Handle(query, cancellationToken);

        // Assert
        _studentRepositoryMock.Verify(x => x.GetByIdWithEnrollmentsAsync(studentId, cancellationToken), Times.Once);
    }

    [Fact]
    public async Task Handle_WithNonExistentStudentId_ShouldThrowNotFoundException()
    {
        // Arrange
        var studentId = "non-existent-id";
        var query = new GetStudentWithEnrollmentsQuery(studentId);
        var cancellationToken = CancellationToken.None;

        _studentRepositoryMock
            .Setup(x => x.GetByIdWithEnrollmentsAsync(studentId, cancellationToken))
            .ReturnsAsync((Student?)null);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<NotFoundException>(() => _handler.Handle(query, cancellationToken));
        exception.Message.Should().Contain($"Student with ID '{studentId}' was not found.");
    }

    [Fact]
    public async Task Handle_WithStudentWithoutEnrollments_ShouldReturnEmptyEnrollmentsList()
    {
        // Arrange
        var studentId = "student-123";
        var student = Student.Create("John Doe", "john.doe@example.com");
        student.Id = studentId;
        student.Enrollments = new List<Enrollment>();

        var query = new GetStudentWithEnrollmentsQuery(studentId);
        var cancellationToken = CancellationToken.None;

        _studentRepositoryMock
            .Setup(x => x.GetByIdWithEnrollmentsAsync(studentId, cancellationToken))
            .ReturnsAsync(student);

        // Act
        var result = await _handler.Handle(query, cancellationToken);

        // Assert
        result!.Enrollments.Should().BeEmpty();
    }

    [Fact]
    public async Task Handle_WithMultipleEnrollments_ShouldReturnAllEnrollments()
    {
        // Arrange
        var studentId = "student-123";
        var student = Student.Create("John Doe", "john.doe@example.com");
        student.Id = studentId;
        
        var course1 = Course.Create("Math 101", "Basic Mathematics", "instructor-1");
        course1.Id = "course-1";
        var course2 = Course.Create("Science 101", "Basic Science", "instructor-2");
        course2.Id = "course-2";
        
        var enrollment1 = Enrollment.Create(studentId, course1.Id);
        enrollment1.Id = "enrollment-1";
        var enrollment2 = Enrollment.Create(studentId, course2.Id);
        enrollment2.Id = "enrollment-2";
        
        student.Enrollments = new List<Enrollment> { enrollment1, enrollment2 };

        var query = new GetStudentWithEnrollmentsQuery(studentId);
        var cancellationToken = CancellationToken.None;

        _studentRepositoryMock
            .Setup(x => x.GetByIdWithEnrollmentsAsync(studentId, cancellationToken))
            .ReturnsAsync(student);

        // Act
        var result = await _handler.Handle(query, cancellationToken);

        // Assert
        result!.Enrollments.Should().HaveCount(2);
    }
}
