using CoursePlatform.Domain.Common;

namespace CoursePlatform.Domain.Entities;

public class Course : BaseEntity
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string InstructorId { get; set; } = string.Empty;
    
    public Instructor Instructor { get; set; } = null!;
    public ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();

    public static Course Create(string title, string description, string instructorId)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new ArgumentException("Title cannot be empty", nameof(title));
        
        if (string.IsNullOrWhiteSpace(description))
            throw new ArgumentException("Description cannot be empty", nameof(description));
        
        if (string.IsNullOrWhiteSpace(instructorId))
            throw new ArgumentException("InstructorId cannot be empty", nameof(instructorId));

        return new Course
        {
            Title = title.Trim(),
            Description = description.Trim(),
            InstructorId = instructorId
        };
    }

    public void Update(string title, string description, string instructorId)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new ArgumentException("Title cannot be empty", nameof(title));
        
        if (string.IsNullOrWhiteSpace(description))
            throw new ArgumentException("Description cannot be empty", nameof(description));
        
        if (string.IsNullOrWhiteSpace(instructorId))
            throw new ArgumentException("InstructorId cannot be empty", nameof(instructorId));

        Title = title.Trim();
        Description = description.Trim();
        InstructorId = instructorId;
    }
}
