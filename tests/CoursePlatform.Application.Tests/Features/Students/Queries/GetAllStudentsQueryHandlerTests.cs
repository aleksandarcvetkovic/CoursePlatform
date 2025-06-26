using CoursePlatform.Application.Features.Students.Queries.GetAllStudents;
using CoursePlatform.Domain.Entities;
using CoursePlatform.Domain.Repositories;
using FluentAssertions;
using Moq;

namespace CoursePlatform.Application.Tests.Features.Students.Queries;

public class GetAllStudentsQueryHandlerTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IStudentRepository> _studentRepositoryMock;
    private readonly GetAllStudentsQueryHandler _handler;

    public GetAllStudentsQueryHandlerTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _studentRepositoryMock = new Mock<IStudentRepository>();
        _unitOfWorkMock.Setup(x => x.Students).Returns(_studentRepositoryMock.Object);
        _handler = new GetAllStudentsQueryHandler(_unitOfWorkMock.Object);
    }

    private static CancellationToken GetCancellationToken() => CancellationToken.None;

    [Fact]
    public async Task Handle_WithExistingStudents_ShouldReturnAllStudents()
    {
        // Arrange
        var student1 = Student.Create("John Doe", "john.doe@example.com");
        student1.Id = "student-1";
        var student2 = Student.Create("Jane Smith", "jane.smith@example.com");
        student2.Id = "student-2";
        var students = new List<Student> { student1, student2 };
        var query = new GetAllStudentsQuery();
        var cancellationToken = GetCancellationToken();

        _studentRepositoryMock
            .Setup(x => x.GetAllAsync(cancellationToken))
            .ReturnsAsync(students);

        // Act
        var result = await _handler.Handle(query, cancellationToken);

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(2);
    }

    [Fact]
    public async Task Handle_WithExistingStudents_ShouldReturnCorrectStudentData()
    {
        // Arrange
        var student = Student.Create("John Doe", "john.doe@example.com");
        student.Id = "student-1";
        var students = new List<Student> { student };

        var query = new GetAllStudentsQuery();
        var cancellationToken = GetCancellationToken();

        _studentRepositoryMock
            .Setup(x => x.GetAllAsync(cancellationToken))
            .ReturnsAsync(students);

        // Act
        var result = await _handler.Handle(query, cancellationToken);

        // Assert
        var studentDto = result.First();
        studentDto.Id.Should().Be("student-1");
        studentDto.Name.Should().Be("John Doe");
        studentDto.Email.Should().Be("john.doe@example.com");
    }

    [Fact]
    public async Task Handle_WithNoStudents_ShouldReturnEmptyCollection()
    {
        // Arrange
        var students = new List<Student>();
        var query = new GetAllStudentsQuery();
        var cancellationToken = GetCancellationToken();

        _studentRepositoryMock
            .Setup(x => x.GetAllAsync(cancellationToken))
            .ReturnsAsync(students);

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
        var students = new List<Student>();
        var query = new GetAllStudentsQuery();
        var cancellationToken = GetCancellationToken();

        _studentRepositoryMock
            .Setup(x => x.GetAllAsync(cancellationToken))
            .ReturnsAsync(students);

        // Act
        await _handler.Handle(query, cancellationToken);

        // Assert
        _studentRepositoryMock.Verify(x => x.GetAllAsync(cancellationToken), Times.Once);
    }

    [Fact]
    public async Task Handle_WithMultipleStudents_ShouldPreserveOrder()
    {
        // Arrange
        var student1 = Student.Create("Alpha Student", "alpha@example.com");
        student1.Id = "student-1";
        var student2 = Student.Create("Beta Student", "beta@example.com");
        student2.Id = "student-2";
        var student3 = Student.Create("Gamma Student", "gamma@example.com");
        student3.Id = "student-3";
        var students = new List<Student> { student1, student2, student3 };

        var query = new GetAllStudentsQuery();
        var cancellationToken = GetCancellationToken();

        _studentRepositoryMock
            .Setup(x => x.GetAllAsync(cancellationToken))
            .ReturnsAsync(students);

        // Act
        var result = await _handler.Handle(query, cancellationToken);

        // Assert
        var resultList = result.ToList();
        resultList[0].Name.Should().Be("Alpha Student");
        resultList[1].Name.Should().Be("Beta Student");
        resultList[2].Name.Should().Be("Gamma Student");
    }
}
