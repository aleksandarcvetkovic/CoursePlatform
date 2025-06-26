using CoursePlatform.Application.Features.Enrollments.Queries.GetAllEnrollments;
using CoursePlatform.Domain.Entities;
using CoursePlatform.Domain.Repositories;
using FluentAssertions;
using Moq;

namespace CoursePlatform.Application.Tests.Features.Enrollments.Queries;

public class GetAllEnrollmentsQueryHandlerTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IEnrollmentRepository> _enrollmentRepositoryMock;
    private readonly GetAllEnrollmentsQueryHandler _handler;

    public GetAllEnrollmentsQueryHandlerTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _enrollmentRepositoryMock = new Mock<IEnrollmentRepository>();
        _unitOfWorkMock.Setup(x => x.Enrollments).Returns(_enrollmentRepositoryMock.Object);
        _handler = new GetAllEnrollmentsQueryHandler(_unitOfWorkMock.Object);
    }

    [Fact]
    public async Task Handle_WithExistingEnrollments_ShouldReturnAllEnrollments()
    {
        // Arrange
        var enrollment1 = Enrollment.Create("student-1", "course-1");
        enrollment1.Id = "enrollment-1";
        var enrollment2 = Enrollment.Create("student-2", "course-2");
        enrollment2.Id = "enrollment-2";
        var enrollments = new List<Enrollment> { enrollment1, enrollment2 };

        var query = new GetAllEnrollmentsQuery();
        var cancellationToken = CancellationToken.None;

        _enrollmentRepositoryMock
            .Setup(x => x.GetAllAsync(cancellationToken))
            .ReturnsAsync(enrollments);

        // Act
        var result = await _handler.Handle(query, cancellationToken);

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(2);
    }

    [Fact]
    public async Task Handle_WithExistingEnrollments_ShouldReturnCorrectEnrollmentData()
    {
        // Arrange
        var enrollment1 = Enrollment.Create("student-1", "course-1");
        enrollment1.Id = "enrollment-1";
        enrollment1.UpdateGrade(85.0);
        var enrollments = new List<Enrollment> { enrollment1 };

        var query = new GetAllEnrollmentsQuery();
        var cancellationToken = CancellationToken.None;

        _enrollmentRepositoryMock
            .Setup(x => x.GetAllAsync(cancellationToken))
            .ReturnsAsync(enrollments);

        // Act
        var result = await _handler.Handle(query, cancellationToken);

        // Assert
        var enrollmentDto = result.First();
        enrollmentDto.Id.Should().Be("enrollment-1");
        enrollmentDto.StudentId.Should().Be("student-1");
        enrollmentDto.CourseId.Should().Be("course-1");
        enrollmentDto.Grade.Should().Be(85.0);
        enrollmentDto.EnrolledOn.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));
    }

    [Fact]
    public async Task Handle_WithNoEnrollments_ShouldReturnEmptyCollection()
    {
        // Arrange
        var enrollments = new List<Enrollment>();
        var query = new GetAllEnrollmentsQuery();
        var cancellationToken = CancellationToken.None;

        _enrollmentRepositoryMock
            .Setup(x => x.GetAllAsync(cancellationToken))
            .ReturnsAsync(enrollments);

        // Act
        var result = await _handler.Handle(query, cancellationToken);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEmpty();
    }

    [Fact]
    public async Task Handle_ShouldCallGetAllAsyncOnRepository()
    {
        // Arrange
        var enrollments = new List<Enrollment>();
        var query = new GetAllEnrollmentsQuery();
        var cancellationToken = CancellationToken.None;

        _enrollmentRepositoryMock
            .Setup(x => x.GetAllAsync(cancellationToken))
            .ReturnsAsync(enrollments);

        // Act
        await _handler.Handle(query, cancellationToken);

        // Assert
        _enrollmentRepositoryMock.Verify(x => x.GetAllAsync(cancellationToken), Times.Once);
    }

    [Fact]
    public async Task Handle_WithEnrollmentsWithoutGrades_ShouldReturnNullGrades()
    {
        // Arrange
        var enrollment1 = Enrollment.Create("student-1", "course-1");
        enrollment1.Id = "enrollment-1";
        var enrollments = new List<Enrollment> { enrollment1 };

        var query = new GetAllEnrollmentsQuery();
        var cancellationToken = CancellationToken.None;

        _enrollmentRepositoryMock
            .Setup(x => x.GetAllAsync(cancellationToken))
            .ReturnsAsync(enrollments);

        // Act
        var result = await _handler.Handle(query, cancellationToken);

        // Assert
        var enrollmentDto = result.First();
        enrollmentDto.Grade.Should().BeNull();
    }

    [Fact]
    public async Task Handle_WithMixedGradedAndUngradedEnrollments_ShouldReturnCorrectly()
    {
        // Arrange
        var enrollment1 = Enrollment.Create("student-1", "course-1");
        enrollment1.Id = "enrollment-1";
        enrollment1.UpdateGrade(90.0);
        
        var enrollment2 = Enrollment.Create("student-2", "course-2");
        enrollment2.Id = "enrollment-2";
        // No grade set
        
        var enrollments = new List<Enrollment> { enrollment1, enrollment2 };

        var query = new GetAllEnrollmentsQuery();
        var cancellationToken = CancellationToken.None;

        _enrollmentRepositoryMock
            .Setup(x => x.GetAllAsync(cancellationToken))
            .ReturnsAsync(enrollments);

        // Act
        var result = await _handler.Handle(query, cancellationToken);

        // Assert
        var resultList = result.ToList();
        resultList[0].Grade.Should().Be(90.0);
        resultList[1].Grade.Should().BeNull();
    }

    [Fact]
    public async Task Handle_WithMultipleEnrollments_ShouldPreserveOrder()
    {
        // Arrange
        var enrollment1 = Enrollment.Create("student-alpha", "course-alpha");
        enrollment1.Id = "enrollment-1";
        var enrollment2 = Enrollment.Create("student-beta", "course-beta");
        enrollment2.Id = "enrollment-2";
        var enrollment3 = Enrollment.Create("student-gamma", "course-gamma");
        enrollment3.Id = "enrollment-3";
        var enrollments = new List<Enrollment> { enrollment1, enrollment2, enrollment3 };

        var query = new GetAllEnrollmentsQuery();
        var cancellationToken = CancellationToken.None;

        _enrollmentRepositoryMock
            .Setup(x => x.GetAllAsync(cancellationToken))
            .ReturnsAsync(enrollments);

        // Act
        var result = await _handler.Handle(query, cancellationToken);

        // Assert
        var resultList = result.ToList();
        resultList[0].StudentId.Should().Be("student-alpha");
        resultList[1].StudentId.Should().Be("student-beta");
        resultList[2].StudentId.Should().Be("student-gamma");
    }
}
