using CoursePlatform.Application.Features.Instructors.Queries.GetAllInstructors;
using CoursePlatform.Domain.Entities;
using CoursePlatform.Domain.Repositories;
using FluentAssertions;
using Moq;

namespace CoursePlatform.Application.Tests.Features.Instructors.Queries;

public class GetAllInstructorsQueryHandlerTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IInstructorRepository> _instructorRepositoryMock;
    private readonly GetAllInstructorsQueryHandler _handler;

    public GetAllInstructorsQueryHandlerTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _instructorRepositoryMock = new Mock<IInstructorRepository>();
        _unitOfWorkMock.Setup(x => x.Instructors).Returns(_instructorRepositoryMock.Object);
        _handler = new GetAllInstructorsQueryHandler(_unitOfWorkMock.Object);
    }

    [Fact]
    public async Task Handle_WithExistingInstructors_ShouldReturnAllInstructors()
    {
        // Arrange
        var instructor1 = Instructor.Create("Dr. Smith", "dr.smith@example.com");
        instructor1.Id = "instructor-1";
        var instructor2 = Instructor.Create("Prof. Johnson", "prof.johnson@example.com");
        instructor2.Id = "instructor-2";
        var instructors = new List<Instructor> { instructor1, instructor2 };

        var query = new GetAllInstructorsQuery();
        var cancellationToken = CancellationToken.None;

        _instructorRepositoryMock
            .Setup(x => x.GetAllAsync(cancellationToken))
            .ReturnsAsync(instructors);

        // Act
        var result = await _handler.Handle(query, cancellationToken);

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(2);
    }

    [Fact]
    public async Task Handle_WithExistingInstructors_ShouldReturnCorrectInstructorData()
    {
        // Arrange
        var instructor1 = Instructor.Create("Dr. Smith", "dr.smith@example.com");
        instructor1.Id = "instructor-1";
        var instructors = new List<Instructor> { instructor1 };

        var query = new GetAllInstructorsQuery();
        var cancellationToken = CancellationToken.None;

        _instructorRepositoryMock
            .Setup(x => x.GetAllAsync(cancellationToken))
            .ReturnsAsync(instructors);

        // Act
        var result = await _handler.Handle(query, cancellationToken);

        // Assert
        var instructorDto = result.First();
        instructorDto.Id.Should().Be("instructor-1");
        instructorDto.Name.Should().Be("Dr. Smith");
        instructorDto.Email.Should().Be("dr.smith@example.com");
    }

    [Fact]
    public async Task Handle_WithNoInstructors_ShouldReturnEmptyCollection()
    {
        // Arrange
        var instructors = new List<Instructor>();
        var query = new GetAllInstructorsQuery();
        var cancellationToken = CancellationToken.None;

        _instructorRepositoryMock
            .Setup(x => x.GetAllAsync(cancellationToken))
            .ReturnsAsync(instructors);

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
        var instructors = new List<Instructor>();
        var query = new GetAllInstructorsQuery();
        var cancellationToken = CancellationToken.None;

        _instructorRepositoryMock
            .Setup(x => x.GetAllAsync(cancellationToken))
            .ReturnsAsync(instructors);

        // Act
        await _handler.Handle(query, cancellationToken);

        // Assert
        _instructorRepositoryMock.Verify(x => x.GetAllAsync(cancellationToken), Times.Once);
    }

    [Fact]
    public async Task Handle_WithMultipleInstructors_ShouldPreserveOrder()
    {
        // Arrange
        var instructor1 = Instructor.Create("Alpha Instructor", "alpha@example.com");
        instructor1.Id = "instructor-1";
        var instructor2 = Instructor.Create("Beta Instructor", "beta@example.com");
        instructor2.Id = "instructor-2";
        var instructor3 = Instructor.Create("Gamma Instructor", "gamma@example.com");
        instructor3.Id = "instructor-3";
        var instructors = new List<Instructor> { instructor1, instructor2, instructor3 };

        var query = new GetAllInstructorsQuery();
        var cancellationToken = CancellationToken.None;

        _instructorRepositoryMock
            .Setup(x => x.GetAllAsync(cancellationToken))
            .ReturnsAsync(instructors);

        // Act
        var result = await _handler.Handle(query, cancellationToken);

        // Assert
        var resultList = result.ToList();
        resultList[0].Name.Should().Be("Alpha Instructor");
        resultList[1].Name.Should().Be("Beta Instructor");
        resultList[2].Name.Should().Be("Gamma Instructor");
    }

    [Fact]
    public async Task Handle_WithValidInstructorEmailFormats_ShouldReturnCorrectEmails()
    {
        // Arrange
        var instructor1 = Instructor.Create("Dr. Smith", "DR.SMITH@EXAMPLE.COM");
        instructor1.Id = "instructor-1";
        var instructors = new List<Instructor> { instructor1 };

        var query = new GetAllInstructorsQuery();
        var cancellationToken = CancellationToken.None;

        _instructorRepositoryMock
            .Setup(x => x.GetAllAsync(cancellationToken))
            .ReturnsAsync(instructors);

        // Act
        var result = await _handler.Handle(query, cancellationToken);

        // Assert
        var instructorDto = result.First();
        instructorDto.Email.Should().Be("dr.smith@example.com");
    }
}
