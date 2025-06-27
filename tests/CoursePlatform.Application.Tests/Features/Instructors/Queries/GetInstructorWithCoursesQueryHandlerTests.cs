using CoursePlatform.Application.Features.Instructors.Queries;
using CoursePlatform.Application.DTOs;
using CoursePlatform.Application.Common.Exceptions;
using CoursePlatform.Domain.Repositories;
using CoursePlatform.Domain.Entities;
using Moq;
using Xunit;
using FluentAssertions;

namespace CoursePlatform.Application.Tests.Features.Instructors.Queries.GetInstructorWithCourses;

public class GetInstructorWithCoursesQueryHandlerTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IInstructorRepository> _instructorRepositoryMock;
    private readonly GetInstructorWithCoursesQueryHandler _handler;

    public GetInstructorWithCoursesQueryHandlerTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _instructorRepositoryMock = new Mock<IInstructorRepository>();
        _unitOfWorkMock.Setup(x => x.Instructors).Returns(_instructorRepositoryMock.Object);
        _handler = new GetInstructorWithCoursesQueryHandler(_unitOfWorkMock.Object);
    }

    [Fact]
    public async Task Handle_ValidInstructorId_ShouldReturnNotNull()
    {
        // Arrange
        var instructorId = "123";
        var instructor = new Instructor 
        { 
            Id = instructorId, 
            Name = "Dr. Smith",
            Email = "dr.smith@example.com",
            Courses = new List<Course>
            {
                new Course { Id = "course1", Title = "Mathematics", Description = "Math course", InstructorId = instructorId }
            }
        };
        
        _instructorRepositoryMock.Setup(x => x.GetByIdWithCoursesAsync(instructorId, It.IsAny<CancellationToken>()))
                                .ReturnsAsync(instructor);

        var query = new GetInstructorWithCoursesQuery(instructorId);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
    }

    [Fact]
    public async Task Handle_ValidInstructorId_ShouldReturnCorrectId()
    {
        // Arrange
        var instructorId = "123";
        var instructor = new Instructor 
        { 
            Id = instructorId, 
            Name = "Dr. Smith",
            Email = "dr.smith@example.com",
            Courses = new List<Course>
            {
                new Course { Id = "course1", Title = "Mathematics", Description = "Math course", InstructorId = instructorId }
            }
        };
        
        _instructorRepositoryMock.Setup(x => x.GetByIdWithCoursesAsync(instructorId, It.IsAny<CancellationToken>()))
                                .ReturnsAsync(instructor);

        var query = new GetInstructorWithCoursesQuery(instructorId);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result?.Id.Should().Be(instructorId);
    }

    [Fact]
    public async Task Handle_ValidInstructorId_ShouldReturnCorrectName()
    {
        // Arrange
        var instructorId = "123";
        var instructor = new Instructor 
        { 
            Id = instructorId, 
            Name = "Dr. Smith",
            Email = "dr.smith@example.com",
            Courses = new List<Course>
            {
                new Course { Id = "course1", Title = "Mathematics", Description = "Math course", InstructorId = instructorId }
            }
        };
        
        _instructorRepositoryMock.Setup(x => x.GetByIdWithCoursesAsync(instructorId, It.IsAny<CancellationToken>()))
                                .ReturnsAsync(instructor);

        var query = new GetInstructorWithCoursesQuery(instructorId);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result?.Name.Should().Be("Dr. Smith");
    }

    [Fact]
    public async Task Handle_ValidInstructorId_ShouldReturnCorrectEmail()
    {
        // Arrange
        var instructorId = "123";
        var instructor = new Instructor 
        { 
            Id = instructorId, 
            Name = "Dr. Smith",
            Email = "dr.smith@example.com",
            Courses = new List<Course>
            {
                new Course { Id = "course1", Title = "Mathematics", Description = "Math course", InstructorId = instructorId }
            }
        };
        
        _instructorRepositoryMock.Setup(x => x.GetByIdWithCoursesAsync(instructorId, It.IsAny<CancellationToken>()))
                                .ReturnsAsync(instructor);

        var query = new GetInstructorWithCoursesQuery(instructorId);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result?.Email.Should().Be("dr.smith@example.com");
    }

    [Fact]
    public async Task Handle_ValidInstructorId_ShouldReturnCoursesNotNull()
    {
        // Arrange
        var instructorId = "123";
        var instructor = new Instructor 
        { 
            Id = instructorId, 
            Name = "Dr. Smith",
            Email = "dr.smith@example.com",
            Courses = new List<Course>
            {
                new Course { Id = "course1", Title = "Mathematics", Description = "Math course", InstructorId = instructorId }
            }
        };
        
        _instructorRepositoryMock.Setup(x => x.GetByIdWithCoursesAsync(instructorId, It.IsAny<CancellationToken>()))
                                .ReturnsAsync(instructor);

        var query = new GetInstructorWithCoursesQuery(instructorId);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result?.Courses.Should().NotBeNull();
    }

    [Fact]
    public async Task Handle_ValidInstructorId_ShouldReturnCorrectCourseCount()
    {
        // Arrange
        var instructorId = "123";
        var instructor = new Instructor 
        { 
            Id = instructorId, 
            Name = "Dr. Smith",
            Email = "dr.smith@example.com",
            Courses = new List<Course>
            {
                new Course { Id = "course1", Title = "Mathematics", Description = "Math course", InstructorId = instructorId },
                new Course { Id = "course2", Title = "Physics", Description = "Physics course", InstructorId = instructorId }
            }
        };
        
        _instructorRepositoryMock.Setup(x => x.GetByIdWithCoursesAsync(instructorId, It.IsAny<CancellationToken>()))
                                .ReturnsAsync(instructor);

        var query = new GetInstructorWithCoursesQuery(instructorId);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result?.Courses.Should().HaveCount(2);
    }

    [Fact]
    public async Task Handle_ValidInstructorId_ShouldCallRepositoryOnce()
    {
        // Arrange
        var instructorId = "123";
        var instructor = new Instructor 
        { 
            Id = instructorId, 
            Name = "Dr. Smith",
            Email = "dr.smith@example.com",
            Courses = new List<Course>()
        };
        
        _instructorRepositoryMock.Setup(x => x.GetByIdWithCoursesAsync(instructorId, It.IsAny<CancellationToken>()))
                                .ReturnsAsync(instructor);

        var query = new GetInstructorWithCoursesQuery(instructorId);

        // Act
        await _handler.Handle(query, CancellationToken.None);

        // Assert
        _instructorRepositoryMock.Verify(x => x.GetByIdWithCoursesAsync(instructorId, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_InvalidInstructorId_ShouldThrowNotFoundException()
    {
        // Arrange
        var instructorId = "invalid-id";
        
        _instructorRepositoryMock.Setup(x => x.GetByIdWithCoursesAsync(instructorId, It.IsAny<CancellationToken>()))
                                .ReturnsAsync((Instructor?)null);

        var query = new GetInstructorWithCoursesQuery(instructorId);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(
            () => _handler.Handle(query, CancellationToken.None));
    }

    [Fact]
    public async Task Handle_InvalidInstructorId_ShouldCallRepositoryOnce()
    {
        // Arrange
        var instructorId = "invalid-id";
        
        _instructorRepositoryMock.Setup(x => x.GetByIdWithCoursesAsync(instructorId, It.IsAny<CancellationToken>()))
                                .ReturnsAsync((Instructor?)null);

        var query = new GetInstructorWithCoursesQuery(instructorId);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(
            () => _handler.Handle(query, CancellationToken.None));
        
        _instructorRepositoryMock.Verify(x => x.GetByIdWithCoursesAsync(instructorId, It.IsAny<CancellationToken>()), Times.Once);
    }
}
