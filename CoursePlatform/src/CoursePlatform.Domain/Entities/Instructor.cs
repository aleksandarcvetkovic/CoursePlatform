using CoursePlatform.Domain.Common;

namespace CoursePlatform.Domain.Entities;

public class Instructor : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;

    public ICollection<Course> Courses { get; set; } = new List<Course>();

    public static Instructor Create(string name, string email)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Name cannot be empty", nameof(name));
        
        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("Email cannot be empty", nameof(email));

        return new Instructor
        {
            Name = name.Trim(),
            Email = email.Trim().ToLowerInvariant()
        };
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
