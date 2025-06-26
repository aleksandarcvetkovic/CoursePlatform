using CoursePlatform.Application.Features.Instructors.Queries;
using CoursePlatform.Application.DTOs;
using CoursePlatform.Application.Common.Exceptions;
using CoursePlatform.Domain.Repositories;
using CoursePlatform.Domain.Entities;
using Moq;
using Xunit;
using FluentAssertions;
using CoursePlatform.Application.Features.Instructors.Queries.GetInstructorById;

namespace CoursePlatform.Application.Tests.Features.Instructors.Queries.GetInstructorById;

public class GetInstructorByIdQueryHandlerTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IInstructorRepository> _instructorRepositoryMock;
    private readonly GetInstructorByIdQueryHandler _handler;

    public GetInstructorByIdQueryHandlerTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _instructorRepositoryMock = new Mock<IInstructorRepository>();
        _unitOfWorkMock.Setup(x => x.Instructors).Returns(_instructorRepositoryMock.Object);
        _handler = new GetInstructorByIdQueryHandler(_unitOfWorkMock.Object);
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
            Email = "dr.smith@example.com"
        };
        
        _instructorRepositoryMock.Setup(x => x.GetByIdAsync(instructorId, It.IsAny<CancellationToken>()))
                                .ReturnsAsync(instructor);

        var query = new GetInstructorByIdQuery(instructorId);

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
            Email = "dr.smith@example.com"
        };
        
        _instructorRepositoryMock.Setup(x => x.GetByIdAsync(instructorId, It.IsAny<CancellationToken>()))
                                .ReturnsAsync(instructor);

        var query = new GetInstructorByIdQuery(instructorId);

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
            Email = "dr.smith@example.com"
        };
        
        _instructorRepositoryMock.Setup(x => x.GetByIdAsync(instructorId, It.IsAny<CancellationToken>()))
                                .ReturnsAsync(instructor);

        var query = new GetInstructorByIdQuery(instructorId);

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
            Email = "dr.smith@example.com"
        };
        
        _instructorRepositoryMock.Setup(x => x.GetByIdAsync(instructorId, It.IsAny<CancellationToken>()))
                                .ReturnsAsync(instructor);

        var query = new GetInstructorByIdQuery(instructorId);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result?.Email.Should().Be("dr.smith@example.com");
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
            Email = "dr.smith@example.com"
        };
        
        _instructorRepositoryMock.Setup(x => x.GetByIdAsync(instructorId, It.IsAny<CancellationToken>()))
                                .ReturnsAsync(instructor);

        var query = new GetInstructorByIdQuery(instructorId);

        // Act
        await _handler.Handle(query, CancellationToken.None);

        // Assert
        _instructorRepositoryMock.Verify(x => x.GetByIdAsync(instructorId, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_InvalidInstructorId_ShouldThrowNotFoundException()
    {
        // Arrange
        var instructorId = "invalid-id";
        
        _instructorRepositoryMock.Setup(x => x.GetByIdAsync(instructorId, It.IsAny<CancellationToken>()))
                                .ReturnsAsync((Instructor?)null);

        var query = new GetInstructorByIdQuery(instructorId);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<NotFoundException>(
            () => _handler.Handle(query, CancellationToken.None));
        
        exception.Message.Should().Contain($"Instructor with ID '{instructorId}' was not found.");
    }

    [Fact]
    public async Task Handle_InvalidInstructorId_ShouldCallRepositoryOnce()
    {
        // Arrange
        var instructorId = "invalid-id";
        
        _instructorRepositoryMock.Setup(x => x.GetByIdAsync(instructorId, It.IsAny<CancellationToken>()))
                                .ReturnsAsync((Instructor?)null);

        var query = new GetInstructorByIdQuery(instructorId);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(
            () => _handler.Handle(query, CancellationToken.None));
        
        _instructorRepositoryMock.Verify(x => x.GetByIdAsync(instructorId, It.IsAny<CancellationToken>()), Times.Once);
    }
}
