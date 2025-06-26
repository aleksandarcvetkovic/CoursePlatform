using CoursePlatform.Application.Features.Courses.Queries;
using CoursePlatform.Application.DTOs;
using CoursePlatform.Application.Common.Exceptions;
using CoursePlatform.Domain.Repositories;
using CoursePlatform.Domain.Entities;
using Moq;
using Xunit;
using FluentAssertions;

namespace CoursePlatform.Application.Tests.Features.Courses.Queries;

public class GetCourseByIdQueryHandlerTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<ICourseRepository> _courseRepositoryMock;
    private readonly GetCourseByIdQueryHandler _handler;

    public GetCourseByIdQueryHandlerTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _courseRepositoryMock = new Mock<ICourseRepository>();
        _unitOfWorkMock.Setup(x => x.Courses).Returns(_courseRepositoryMock.Object);
        _handler = new GetCourseByIdQueryHandler(_unitOfWorkMock.Object);
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
            InstructorId = "instructor-1"
        };
        
        _courseRepositoryMock.Setup(x => x.GetByIdAsync(courseId, It.IsAny<CancellationToken>()))
                            .ReturnsAsync(course);

        var query = new GetCourseByIdQuery(courseId);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
    }

    [Fact]
    public async Task Handle_ValidCourseId_ShouldReturnCorrectId()
    {
        // Arrange
        var courseId = "123";
        var course = new Course 
        { 
            Id = courseId, 
            Title = "Test Course",
            Description = "Test Description",
            InstructorId = "instructor-1"
        };
        
        _courseRepositoryMock.Setup(x => x.GetByIdAsync(courseId, It.IsAny<CancellationToken>()))
                            .ReturnsAsync(course);

        var query = new GetCourseByIdQuery(courseId);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result?.Id.Should().Be(courseId);
    }

    [Fact]
    public async Task Handle_ValidCourseId_ShouldReturnCorrectTitle()
    {
        // Arrange
        var courseId = "123";
        var course = new Course 
        { 
            Id = courseId, 
            Title = "Test Course",
            Description = "Test Description",
            InstructorId = "instructor-1"
        };
        
        _courseRepositoryMock.Setup(x => x.GetByIdAsync(courseId, It.IsAny<CancellationToken>()))
                            .ReturnsAsync(course);

        var query = new GetCourseByIdQuery(courseId);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result?.Title.Should().Be("Test Course");
    }

    [Fact]
    public async Task Handle_ValidCourseId_ShouldReturnCorrectDescription()
    {
        // Arrange
        var courseId = "123";
        var course = new Course 
        { 
            Id = courseId, 
            Title = "Test Course",
            Description = "Test Description",
            InstructorId = "instructor-1"
        };
        
        _courseRepositoryMock.Setup(x => x.GetByIdAsync(courseId, It.IsAny<CancellationToken>()))
                            .ReturnsAsync(course);

        var query = new GetCourseByIdQuery(courseId);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result?.Description.Should().Be("Test Description");
    }

    [Fact]
    public async Task Handle_ValidCourseId_ShouldReturnCorrectInstructorId()
    {
        // Arrange
        var courseId = "123";
        var course = new Course 
        { 
            Id = courseId, 
            Title = "Test Course",
            Description = "Test Description",
            InstructorId = "instructor-1"
        };
        
        _courseRepositoryMock.Setup(x => x.GetByIdAsync(courseId, It.IsAny<CancellationToken>()))
                            .ReturnsAsync(course);

        var query = new GetCourseByIdQuery(courseId);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result?.InstructorId.Should().Be("instructor-1");
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
            InstructorId = "instructor-1"
        };
        
        _courseRepositoryMock.Setup(x => x.GetByIdAsync(courseId, It.IsAny<CancellationToken>()))
                            .ReturnsAsync(course);

        var query = new GetCourseByIdQuery(courseId);

        // Act
        await _handler.Handle(query, CancellationToken.None);

        // Assert
        _courseRepositoryMock.Verify(x => x.GetByIdAsync(courseId, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_InvalidCourseId_ShouldThrowNotFoundException()
    {
        // Arrange
        var courseId = "invalid-id";
        
        _courseRepositoryMock.Setup(x => x.GetByIdAsync(courseId, It.IsAny<CancellationToken>()))
                            .ReturnsAsync((Course?)null);

        var query = new GetCourseByIdQuery(courseId);

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
        
        _courseRepositoryMock.Setup(x => x.GetByIdAsync(courseId, It.IsAny<CancellationToken>()))
                            .ReturnsAsync((Course?)null);

        var query = new GetCourseByIdQuery(courseId);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(
            () => _handler.Handle(query, CancellationToken.None));
        
        _courseRepositoryMock.Verify(x => x.GetByIdAsync(courseId, It.IsAny<CancellationToken>()), Times.Once);
    }
}