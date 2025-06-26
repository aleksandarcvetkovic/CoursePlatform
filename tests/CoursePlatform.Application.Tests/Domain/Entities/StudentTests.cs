using CoursePlatform.Domain.Entities;
using FluentAssertions;

namespace CoursePlatform.Application.Tests.Domain.Entities;

public class StudentTests
{
    [Fact]
    public void Create_WithValidData_ShouldCreateStudent()
    {
        // Arrange
        var name = "John Doe";
        var email = "john.doe@example.com";

        // Act
        var student = Student.Create(name, email);

        // Assert
        student.Should().NotBeNull();
        student.Name.Should().Be(name);
        student.Email.Should().Be("john.doe@example.com");
        student.Enrollments.Should().BeEmpty();
        // Note: Id is not auto-generated in the current implementation
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public void Create_WithInvalidName_ShouldThrowArgumentException(string? name)
    {
        // Arrange
        var email = "john.doe@example.com";

        // Act & Assert
        FluentActions.Invoking(() => Student.Create(name!, email))
            .Should().Throw<ArgumentException>()
            .WithParameterName(nameof(name))
            .WithMessage("*Name cannot be empty*");
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public void Create_WithInvalidEmail_ShouldThrowArgumentException(string? email)
    {
        // Arrange
        var name = "John Doe";

        // Act & Assert
        FluentActions.Invoking(() => Student.Create(name, email!))
            .Should().Throw<ArgumentException>()
            .WithParameterName(nameof(email))
            .WithMessage("*Email cannot be empty*");
    }

    [Fact]
    public void Create_WithEmailContainingUppercase_ShouldNormalizeToLowercase()
    {
        // Arrange
        var name = "John Doe";
        var email = "JOHN.DOE@EXAMPLE.COM";

        // Act
        var student = Student.Create(name, email);

        // Assert
        student.Email.Should().Be("john.doe@example.com");
    }

    [Fact]
    public void Create_WithNameAndEmailContainingWhitespace_ShouldTrimWhitespace()
    {
        // Arrange
        var name = "  John Doe  ";
        var email = "  john.doe@example.com  ";

        // Act
        var student = Student.Create(name, email);

        // Assert
        student.Name.Should().Be("John Doe");
        student.Email.Should().Be("john.doe@example.com");
    }

    [Fact]
    public void UpdateInfo_WithValidData_ShouldUpdateStudentInfo()
    {
        // Arrange
        var student = Student.Create("Original Name", "original@example.com");
        var newName = "Updated Name";
        var newEmail = "updated@example.com";

        // Act
        student.UpdateInfo(newName, newEmail);

        // Assert
        student.Name.Should().Be(newName);
        student.Email.Should().Be(newEmail);
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public void UpdateInfo_WithInvalidName_ShouldThrowArgumentException(string? name)
    {
        // Arrange
        var student = Student.Create("Original Name", "original@example.com");
        var email = "updated@example.com";

        // Act & Assert
        FluentActions.Invoking(() => student.UpdateInfo(name!, email))
            .Should().Throw<ArgumentException>()
            .WithParameterName(nameof(name))
            .WithMessage("*Name cannot be empty*");
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public void UpdateInfo_WithInvalidEmail_ShouldThrowArgumentException(string? email)
    {
        // Arrange
        var student = Student.Create("Original Name", "original@example.com");
        var name = "Updated Name";

        // Act & Assert
        FluentActions.Invoking(() => student.UpdateInfo(name, email!))
            .Should().Throw<ArgumentException>()
            .WithParameterName(nameof(email))
            .WithMessage("*Email cannot be empty*");
    }

    [Fact]
    public void UpdateInfo_WithEmailContainingUppercase_ShouldNormalizeToLowercase()
    {
        // Arrange
        var student = Student.Create("Original Name", "original@example.com");
        var name = "Updated Name";
        var email = "UPDATED@EXAMPLE.COM";

        // Act
        student.UpdateInfo(name, email);

        // Assert
        student.Email.Should().Be("updated@example.com");
    }

    [Fact]
    public void UpdateInfo_WithNameAndEmailContainingWhitespace_ShouldTrimWhitespace()
    {
        // Arrange
        var student = Student.Create("Original Name", "original@example.com");
        var name = "  Updated Name  ";
        var email = "  updated@example.com  ";

        // Act
        student.UpdateInfo(name, email);

        // Assert
        student.Name.Should().Be("Updated Name");
        student.Email.Should().Be("updated@example.com");
    }
}
