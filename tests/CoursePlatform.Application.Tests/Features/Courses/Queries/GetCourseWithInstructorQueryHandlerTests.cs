using CoursePlatform.Application.Features.Courses.Queries;
using CoursePlatform.Application.DTOs;
using CoursePlatform.Application.Common.Exceptions;
using CoursePlatform.Domain.Repositories;
using CoursePlatform.Domain.Entities;
using Moq;
using Xunit;
using FluentAssertions;

namespace CoursePlatform.Application.Tests.Features.Courses.Queries.GetCourseWithInstructor;

public class GetCourseWithInstructorQueryHandlerTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<ICourseRepository> _courseRepositoryMock;
    private readonly GetCourseWithInstructorQueryHandler _handler;

    public GetCourseWithInstructorQueryHandlerTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _courseRepositoryMock = new Mock<ICourseRepository>();
        _unitOfWorkMock.Setup(x => x.Courses).Returns(_courseRepositoryMock.Object);
        _handler = new GetCourseWithInstructorQueryHandler(_unitOfWorkMock.Object);
    }

    [Fact]
    public async Task Handle_ValidCourseId_ShouldReturnNotNull()
    {
        // Arrange
        var courseId = "123";
        var course = new Course 
        { 
            Id = courseId, 
            Title = "Test Course",
            Description = "Test Description",
            InstructorId = "instructor-1",
            Instructor = new Instructor 
            { 
                Id = "instructor-1", 
                Name = "Dr. Smith", 
                Email = "dr.smith@example.com" 
            }
        };
        
        _courseRepositoryMock.Setup(x => x.GetByIdWithInstructorAsync(courseId, It.IsAny<CancellationToken>()))
                            .ReturnsAsync(course);

        var query = new GetCourseWithInstructorQuery(courseId);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
    }

    [Fact]
    public async Task Handle_ValidCourseId_ShouldReturnCorrectCourseId()
    {
        // Arrange
        var courseId = "123";
        var course = new Course 
        { 
            Id = courseId, 
            Title = "Test Course",
            Description = "Test Description",
            InstructorId = "instructor-1",
            Instructor = new Instructor 
            { 
                Id = "instructor-1", 
                Name = "Dr. Smith", 
                Email = "dr.smith@example.com" 
            }
        };
        
        _courseRepositoryMock.Setup(x => x.GetByIdWithInstructorAsync(courseId, It.IsAny<CancellationToken>()))
                            .ReturnsAsync(course);

        var query = new GetCourseWithInstructorQuery(courseId);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result?.Id.Should().Be(courseId);
    }

    [Fact]
    public async Task Handle_ValidCourseId_ShouldReturnCorrectCourseTitle()
    {
        // Arrange
        var courseId = "123";
        var course = new Course 
        { 
            Id = courseId, 
            Title = "Test Course",
            Description = "Test Description",
            InstructorId = "instructor-1",
            Instructor = new Instructor 
            { 
                Id = "instructor-1", 
                Name = "Dr. Smith", 
                Email = "dr.smith@example.com" 
            }
        };
        
        _courseRepositoryMock.Setup(x => x.GetByIdWithInstructorAsync(courseId, It.IsAny<CancellationToken>()))
                            .ReturnsAsync(course);

        var query = new GetCourseWithInstructorQuery(courseId);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result?.Title.Should().Be("Test Course");
    }

    [Fact]
    public async Task Handle_ValidCourseId_ShouldReturnCorrectInstructorName()
    {
        // Arrange
        var courseId = "123";
        var course = new Course 
        { 
            Id = courseId, 
            Title = "Test Course",
            Description = "Test Description",
            InstructorId = "instructor-1",
            Instructor = new Instructor 
            { 
                Id = "instructor-1", 
                Name = "Dr. Smith", 
                Email = "dr.smith@example.com" 
            }
        };
        
        _courseRepositoryMock.Setup(x => x.GetByIdWithInstructorAsync(courseId, It.IsAny<CancellationToken>()))
                            .ReturnsAsync(course);

        var query = new GetCourseWithInstructorQuery(courseId);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result?.InstructorName.Should().Be("Dr. Smith");
    }

    [Fact]
    public async Task Handle_ValidCourseId_ShouldReturnCorrectInstructorEmail()
    {
        // Arrange
        var courseId = "123";
        var course = new Course 
        { 
            Id = courseId, 
            Title = "Test Course",
            Description = "Test Description",
            InstructorId = "instructor-1",
            Instructor = new Instructor 
            { 
                Id = "instructor-1", 
                Name = "Dr. Smith", 
                Email = "dr.smith@example.com" 
            }
        };
        
        _courseRepositoryMock.Setup(x => x.GetByIdWithInstructorAsync(courseId, It.IsAny<CancellationToken>()))
                            .ReturnsAsync(course);

        var query = new GetCourseWithInstructorQuery(courseId);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result?.InstructorEmail.Should().Be("dr.smith@example.com");
    }

    [Fact]
    public async Task Handle_ValidCourseId_ShouldCallRepositoryOnce()
    {
        // Arrange
        var courseId = "123";
        var course = new Course 
        { 
            Id = courseId, 
            Title = "Test Course",
            Description = "Test Description",
            InstructorId = "instructor-1",
            Instructor = new Instructor 
            { 
                Id = "instructor-1", 
                Name = "Dr. Smith", 
                Email = "dr.smith@example.com" 
            }
        };
        
        _courseRepositoryMock.Setup(x => x.GetByIdWithInstructorAsync(courseId, It.IsAny<CancellationToken>()))
                            .ReturnsAsync(course);

        var query = new GetCourseWithInstructorQuery(courseId);

        // Act
        await _handler.Handle(query, CancellationToken.None);

        // Assert
        _courseRepositoryMock.Verify(x => x.GetByIdWithInstructorAsync(courseId, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_InvalidCourseId_ShouldThrowNotFoundException()
    {
        // Arrange
        var courseId = "invalid-id";
        
        _courseRepositoryMock.Setup(x => x.GetByIdWithInstructorAsync(courseId, It.IsAny<CancellationToken>()))
                            .ReturnsAsync((Course?)null);

        var query = new GetCourseWithInstructorQuery(courseId);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<NotFoundException>(
            () => _handler.Handle(query, CancellationToken.None));
        
        exception.Message.Should().Contain($"Course with ID '{courseId}' was not found.");
    }

    [Fact]
    public async Task Handle_InvalidCourseId_ShouldCallRepositoryOnce()
    {
        // Arrange
        var courseId = "invalid-id";
        
        _courseRepositoryMock.Setup(x => x.GetByIdWithInstructorAsync(courseId, It.IsAny<CancellationToken>()))
                            .ReturnsAsync((Course?)null);

        var query = new GetCourseWithInstructorQuery(courseId);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(
            () => _handler.Handle(query, CancellationToken.None));
        
        _courseRepositoryMock.Verify(x => x.GetByIdWithInstructorAsync(courseId, It.IsAny<CancellationToken>()), Times.Once);
    }
}
