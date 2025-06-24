using CoursePlatform.Domain.Common;
using CoursePlatform.Domain.Events.Students;

namespace CoursePlatform.Domain.Entities;

public class Student : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;

    public ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();

    public static Student Create(string name, string email)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Name cannot be empty", nameof(name));
        
        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("Email cannot be empty", nameof(email));

        var student = new Student
        {
            Name = name.Trim(),
            Email = email.Trim().ToLowerInvariant()
        };

        student.AddDomainEvent(new StudentCreatedEvent(student.Id, student.Name, student.Email));
        return student;
    }

    public void UpdateInfo(string name, string email)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Name cannot be empty", nameof(name));
        
        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("Email cannot be empty", nameof(email));

        Name = name.Trim();
        Email = email.Trim().ToLowerInvariant();
    }
}
